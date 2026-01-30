// (c) Copyright Jacob Johnston.
// This source is subject to Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

//	Revisions by Daniel Patterson 2021.
//	DEP20210624.1207 - Various bracketing.
//	DEP20210624.1601 - Maximize volume display on WaveformData.
//	DEP20210625.0651 - Change waveformCompressedPointCount to publicly
//	accessible dynamic value associated with new public property
//	ChannelDataPointCount.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using NAudio.Wave;
using WPFSoundVisualizationLib;

namespace Sample_NAudio
{
	public class NAudioEngine : INotifyPropertyChanged, ISpectrumPlayer, IWaveformPlayer, IDisposable
	{
		// region Fields
		private static NAudioEngine instance;
		private readonly DispatcherTimer positionTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
		private readonly BackgroundWorker waveformGenerateWorker = new BackgroundWorker();
		private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
		private bool disposed;
		private bool canPlay;
		private bool canPause;
		private bool canStop;
		private bool isPlaying;
		private bool inChannelTimerUpdate;
		private bool inError = false;
		private double channelLength;
		private double channelPosition;
		private bool inChannelSet;
		private WaveOut waveOutDevice;
		private WaveStream activeStream;
		private WaveChannel32 inputStream;
		private SampleAggregator sampleAggregator;
		private SampleAggregator waveformAggregator;
		private string pendingWaveformPath;
		private float[] fullLevelData;
		private float[] waveformData;
		private TagLib.File fileTag;
		private TimeSpan repeatStart;
		private TimeSpan repeatStop;
		private bool inRepeatSet;
		// endregion

		#region Constants
		/// <summary>
		/// DEP20210625.0655 - Now variable by host system.
		/// </summary>
		private int waveformCompressedPointCount = 2000;
		private const int repeatThreshold = 200;
		#endregion

		#region Singleton Pattern
		public static NAudioEngine Instance
		{
			get
			{
				if(instance == null)
					instance = new NAudioEngine();
				return instance;
			}
		}
		#endregion

		#region Constructor
		private NAudioEngine()
		{
			positionTimer.Interval = TimeSpan.FromMilliseconds(50);
			positionTimer.Tick += positionTimer_Tick;

			waveformGenerateWorker.DoWork += waveformGenerateWorker_DoWork;
			waveformGenerateWorker.RunWorkerCompleted += waveformGenerateWorker_RunWorkerCompleted;
			waveformGenerateWorker.WorkerSupportsCancellation = true;
		}
		#endregion

		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!disposed)
			{
				if(disposing)
				{
					StopAndCloseStream();
				}

				disposed = true;
			}
		}
		#endregion

		#region ISpectrumPlayer
		public bool GetFFTData(float[] fftDataBuffer)
		{
			sampleAggregator.GetFFTResults(fftDataBuffer);
			return isPlaying;
		}

		public int GetFFTFrequencyIndex(int frequency)
		{
			double maxFrequency;
			if(ActiveStream != null)
				maxFrequency = ActiveStream.WaveFormat.SampleRate / 2.0d;
			else
				maxFrequency = 22050; // Assume a default 44.1 kHz sample rate.
			return (int)((frequency / maxFrequency) * (fftDataSize / 2));
		}
		#endregion

		#region IWaveformPlayer
		public TimeSpan SelectionBegin
		{
			get { return repeatStart; }
			set
			{
				if(!inRepeatSet)
				{
					inRepeatSet = true;
					TimeSpan oldValue = repeatStart;
					repeatStart = value;
					if(oldValue != repeatStart)
					{
						NotifyPropertyChanged("SelectionBegin");
					}
					inRepeatSet = false;
				}
			}
		}

		public TimeSpan SelectionEnd
		{
			get { return repeatStop; }
			set
			{
				if(!inChannelSet)
				{
					inRepeatSet = true;
					TimeSpan oldValue = repeatStop;
					repeatStop = value;
					if(oldValue != repeatStop)
					{
						NotifyPropertyChanged("SelectionEnd");
					}
					inRepeatSet = false;
				}
			}
		}

		public float[] WaveformData
		{
			get { return waveformData; }
			protected set
			{
				float[] oldValue = waveformData;
				waveformData = value;
				Debug.WriteLine(
					$"NAudioEngine: WaveformData set to {value.Length} samples.");
				if(oldValue != waveformData)
				{
					NotifyPropertyChanged("WaveformData");
				}
			}
		}

		/// <summary>
		/// Get/Set the count of compressed data points available on the loaded
		/// media.
		/// </summary>
		// DEP20210625.0655 - Host system can now set the data point count.
		public int ChannelDataPointCount
		{
			get { return waveformCompressedPointCount; }
			set
			{
				if(value >= 2000)
				{
					waveformCompressedPointCount = value;
				}
			}
		}

		public double ChannelLength
		{
			get { return channelLength; }
			protected set
			{
				double oldValue = channelLength;
				channelLength = value;
				if(oldValue != channelLength)
				{
					NotifyPropertyChanged("ChannelLength");
				}
			}
		}

		public double ChannelPosition
		{
			get { return channelPosition; }
			set
			{
				if(!inChannelSet)
				{
					inChannelSet = true; // Avoid recursion
					double oldValue = channelPosition;
					double position = Math.Max(0, Math.Min(value, ChannelLength));
					if(!inChannelTimerUpdate && ActiveStream != null)
						ActiveStream.Position = (long)((position / ActiveStream.TotalTime.TotalSeconds) * ActiveStream.Length);
					channelPosition = position;
					if(oldValue != channelPosition)
						NotifyPropertyChanged("ChannelPosition");
					inChannelSet = false;
				}
			}
		}
		#endregion

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if(PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion

		#region Waveform Generation
		private class WaveformGenerationParams
		{
			public WaveformGenerationParams(int points, string path)
			{
				Points = points;
				Path = path;
			}

			public int Points { get; protected set; }
			public string Path { get; protected set; }
		}

		private void GenerateWaveformData(string path)
		{
			if(waveformGenerateWorker.IsBusy)
			{
				pendingWaveformPath = path;
				waveformGenerateWorker.CancelAsync();
				return;
			}

			if(!waveformGenerateWorker.IsBusy && waveformCompressedPointCount != 0)
				waveformGenerateWorker.RunWorkerAsync(new WaveformGenerationParams(waveformCompressedPointCount, path));
		}

		private void waveformGenerateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if(e.Cancelled)
			{
				if(!waveformGenerateWorker.IsBusy && waveformCompressedPointCount != 0)
					waveformGenerateWorker.RunWorkerAsync(new WaveformGenerationParams(waveformCompressedPointCount, pendingWaveformPath));
			}
		}

		private void waveformGenerateWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			int count = 0;
			int index = 0;

			WaveformGenerationParams waveformParams =
				e.Argument as WaveformGenerationParams;
			//Mp3FileReader waveformMp3Stream =
			//new Mp3FileReader(waveformParams.Path);
			MediaFoundationReader waveformMp3Stream =
				new MediaFoundationReader(waveformParams.Path);
			WaveChannel32 waveformInputStream =
				new WaveChannel32(waveformMp3Stream);
			waveformInputStream.Sample += waveStream_Sample;

			int frameLength = fftDataSize;
			int frameCount =
				(int)((double)waveformInputStream.Length / (double)frameLength);
			int waveformLength = frameCount * 2;
			byte[] readBuffer = new byte[frameLength];
			waveformAggregator = new SampleAggregator(frameLength);

			float maxLeftPointLevel = float.MinValue;
			float maxRightPointLevel = float.MinValue;
			int currentPointIndex = 0;
			float[] waveformCompressedPoints = new float[waveformParams.Points];
			List<float> waveformData = new List<float>();
			List<int> waveMaxPointIndexes = new List<int>();

			for(int i = 1; i <= waveformParams.Points; i++)
			{
				waveMaxPointIndexes.Add(
					(int)Math.Round(waveformLength *
						((double)i / (double)waveformParams.Points), 0));
			}
			int readCount = 0;
			while(currentPointIndex * 2 < waveformParams.Points)
			{
				waveformInputStream.Read(readBuffer, 0, readBuffer.Length);

				waveformData.Add(waveformAggregator.LeftMaxVolume);
				waveformData.Add(waveformAggregator.RightMaxVolume);

				if(waveformAggregator.LeftMaxVolume > maxLeftPointLevel)
				{
					maxLeftPointLevel = waveformAggregator.LeftMaxVolume;
				}
				if(waveformAggregator.RightMaxVolume > maxRightPointLevel)
				{
					maxRightPointLevel = waveformAggregator.RightMaxVolume;
				}

				if(readCount > waveMaxPointIndexes[currentPointIndex])
				{
					waveformCompressedPoints[(currentPointIndex * 2)] =
						maxLeftPointLevel;
					waveformCompressedPoints[(currentPointIndex * 2) + 1] =
						maxRightPointLevel;
					maxLeftPointLevel = float.MinValue;
					maxRightPointLevel = float.MinValue;
					currentPointIndex++;
				}
				if(readCount % 3000 == 0)
				{
					float[] clonedData = (float[])waveformCompressedPoints.Clone();
					Application.Current.Dispatcher.Invoke(new Action(() =>
					{
						WaveformData = clonedData;
					}));
				}

				if(waveformGenerateWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				readCount++;
			}

			float[] finalClonedData = (float[])waveformCompressedPoints.Clone();
			//	Max out the waveform display to 0dB.
			maxLeftPointLevel = 0f;
			count = finalClonedData.Length;
			for(index = 0; index < count; index ++)
			{
				maxRightPointLevel = finalClonedData[index];
				if(maxRightPointLevel > maxLeftPointLevel)
				{
					maxLeftPointLevel = maxRightPointLevel;
				}
			}
			if(maxLeftPointLevel > 0f)
			{
				maxLeftPointLevel = 1f / maxLeftPointLevel;
				for(index = 0; index < count; index ++)
				{
					finalClonedData[index] *= maxLeftPointLevel;
				}
			}

			//Debug.Write("NAudioEngine: Final Cloned Data: ");
			//Debug.WriteLine(String.Join(", ", finalClonedData));
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				fullLevelData = waveformData.ToArray();
				//Debug.WriteLine($"NAudioEngine: Full Level Data: {String.Join(", ", fullLevelData)}");
				WaveformData = finalClonedData;
			}));
			waveformInputStream.Close();
			waveformInputStream.Dispose();
			waveformInputStream = null;
			waveformMp3Stream.Close();
			waveformMp3Stream.Dispose();
			waveformMp3Stream = null;
		}
		#endregion

		#region Private Utility Methods
		private void StopAndCloseStream()
		{
			if(waveOutDevice != null)
			{
				waveOutDevice.Stop();
			}
			if(activeStream != null)
			{
				inputStream.Close();
				inputStream = null;
				ActiveStream.Close();
				ActiveStream = null;
			}
			if(waveOutDevice != null)
			{
				waveOutDevice.Dispose();
				waveOutDevice = null;
			}
		}
		#endregion

		#region Public Methods
		public void Stop()
		{
			if(waveOutDevice != null)
			{
				waveOutDevice.Stop();
			}
			IsPlaying = false;
			CanStop = false;
			CanPlay = true;
			CanPause = false;
		}

		public void Pause()
		{
			if(IsPlaying && CanPause)
			{
				waveOutDevice.Pause();
				IsPlaying = false;
				CanPlay = true;
				CanPause = false;
			}
		}

		public void Play()
		{
			if(CanPlay)
			{
				waveOutDevice.Play();
				IsPlaying = true;
				CanPause = true;
				CanPlay = false;
				CanStop = true;
			}
		}

		//	DEP20210625.0700 - Added samples per second specifier.
		public void OpenFile(string path, double waveformSamplesPerSecond = 0d)
		{
			Stop();

			if(ActiveStream != null)
			{
				SelectionBegin = TimeSpan.Zero;
				SelectionEnd = TimeSpan.Zero;
				ChannelPosition = 0;
			}

			StopAndCloseStream();

			if(System.IO.File.Exists(path))
			{
				try
				{
					waveOutDevice = new WaveOut()
					{
						DesiredLatency = 100
					};
					//ActiveStream = new Mp3FileReader(path);

					ActiveStream = new MediaFoundationReader(path);

					inputStream = new WaveChannel32(ActiveStream);
					ChannelLength = inputStream.TotalTime.TotalSeconds;
					if(waveformSamplesPerSecond > 0)
					{
						//	Pixels per second * number of seconds * 2 audio tracks.
						waveformCompressedPointCount =
							(int)Math.Ceiling(waveformSamplesPerSecond * channelLength * 2d);
					}


					sampleAggregator = new SampleAggregator(fftDataSize);
					inputStream.Sample += inputStream_Sample;
					waveOutDevice.Init(inputStream);
					FileTag = TagLib.File.Create(path);
					GenerateWaveformData(path);
					CanPlay = true;
				}
				catch(Exception ex)
				{
					Debug.WriteLine($"NAudio.OpenFile.Error: {ex.Message}");
					ActiveStream = null;
					CanPlay = false;
					inError = true;
				}
			}
		}
		#endregion

		#region Public Properties
		public TagLib.File FileTag
		{
			get { return fileTag; }
			set
			{
				TagLib.File oldValue = fileTag;
				fileTag = value;
				if(oldValue != fileTag)
				{
					NotifyPropertyChanged("FileTag");
				}
			}
		}

		public WaveStream ActiveStream
		{
			get { return activeStream; }
			protected set
			{
				WaveStream oldValue = activeStream;
				activeStream = value;
				if(oldValue != activeStream)
				{
					NotifyPropertyChanged("ActiveStream");
				}
			}
		}

		public bool CanPlay
		{
			get { return canPlay; }
			protected set
			{
				bool oldValue = canPlay;
				if(!inError)
				{
					canPlay = value;
				}
				else
				{
					canPlay = false;
				}
				if(oldValue != canPlay)
				{
					NotifyPropertyChanged("CanPlay");
				}
			}
		}

		public bool CanPause
		{
			get { return canPause; }
			protected set
			{
				bool oldValue = canPause;
				canPause = value;
				if(oldValue != canPause)
				{
					NotifyPropertyChanged("CanPause");
				}
			}
		}

		public bool CanStop
		{
			get { return canStop; }
			protected set
			{
				bool oldValue = canStop;
				canStop = value;
				if(oldValue != canStop)
				{
					NotifyPropertyChanged("CanStop");
				}
			}
		}


		public bool IsPlaying
		{
			get { return isPlaying; }
			protected set
			{
				bool oldValue = isPlaying;
				isPlaying = value;
				if(oldValue != isPlaying)
				{
					NotifyPropertyChanged("IsPlaying");
				}
				positionTimer.IsEnabled = value;
			}
		}
		#endregion

		#region Event Handlers
		private void inputStream_Sample(object sender, SampleEventArgs e)
		{
			//	DEP20210927.1151 - Catch error on null.
			sampleAggregator.Add(e.Left, e.Right);
			long repeatStartPosition = 0;
			long repeatStopPosition = 0;

			try
			{
				repeatStartPosition = (long)((SelectionBegin.TotalSeconds / ActiveStream.TotalTime.TotalSeconds) * ActiveStream.Length);
				repeatStopPosition = (long)((SelectionEnd.TotalSeconds / ActiveStream.TotalTime.TotalSeconds) * ActiveStream.Length);
				if(((SelectionEnd - SelectionBegin) >= TimeSpan.FromMilliseconds(repeatThreshold)) && ActiveStream.Position >= repeatStopPosition)
				{
					sampleAggregator.Clear();
					ActiveStream.Position = repeatStartPosition;
				}
			}
			catch { }
		}

		void waveStream_Sample(object sender, SampleEventArgs e)
		{
			waveformAggregator.Add(e.Left, e.Right);
		}

		void positionTimer_Tick(object sender, EventArgs e)
		{
			inChannelTimerUpdate = true;
			ChannelPosition = ((double)ActiveStream.Position / (double)ActiveStream.Length) * ActiveStream.TotalTime.TotalSeconds;
			inChannelTimerUpdate = false;
		}
		#endregion
	}
}
