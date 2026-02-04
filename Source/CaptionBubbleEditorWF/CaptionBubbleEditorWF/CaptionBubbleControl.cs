/*
* Copyright (c). 2020-2026 Daniel Patterson, MCSD (danielanywhere).
* 
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program.  If not, see <https://www.gnu.org/licenses/>.
* 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

using NAudio.Wave;
using NAudio.WaveFormRenderer;
using ScrollPanelVirtual;

using static CaptionBubbleEditorWF.CaptionBubbleEditorUtil;

namespace CaptionBubbleEditorWF
{
	//*-------------------------------------------------------------------------*
	//*	CaptionBubbleControl																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Caption bubble control.
	/// </summary>
	public partial class CaptionBubbleControl : UserControl, IPaintableControl
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private Brush mBackgroundBrush = new SolidBrush(Color.White);
		private Brush mCaptionBrush =
			new SolidBrush(ColorTranslator.FromHtml("#afd5f8"));
		private Brush mCaptionBrushSelected =
			new SolidBrush(ColorTranslator.FromHtml("#ddebf8"));
		private CaptionEditModeEnum mCaptionEditMode = CaptionEditModeEnum.None;
		private Font mCaptionFont = new Font("Verdana", 9f);
		private SolidBrush mCaptionFontBrush =
			new SolidBrush(ColorTranslator.FromHtml("#221746"));
		private double mCaptionLeftX = 0d;
		private float mCaptionMinY = 24f;
		private CaptionItem mCaptionPartner = null;
		private Pen mCaptionPen = new Pen(ColorTranslator.FromHtml("#0d4d78"), 3f);
		private Pen mCaptionPenSelected =
			new Pen(ColorTranslator.FromHtml("#0000ff"), 3f);
		private double mCaptionRightX = 0d;
		private CaptionItem mCaptionSelected = null;
		private Brush mGlowBrush =
			new SolidBrush(ColorTranslator.FromHtml("#d0ff15a8"));
		private Pen mGlowPen = new Pen(ColorTranslator.FromHtml("#d0ff15a8"), 1f);
		private Pen mGlowPenDark =
			new Pen(ColorTranslator.FromHtml("#ffb3006d"), 1f);
		//private Pen mGlowPen1 = new Pen(ColorTranslator.FromHtml("#a0f1cb02"), 1f);
		//private Pen mGlowPen2 = new Pen(ColorTranslator.FromHtml("#c0dc0301"), 3f);
		private Pen mLinePen = new Pen(ColorTranslator.FromHtml("#221746"), 1f);
		private Brush mMeterBackBrush =
			new SolidBrush(ColorTranslator.FromHtml("#f0f0ff"));
		private CaptionAreaEnum mMouseArea = CaptionAreaEnum.None;
		private bool mMouseDown = false;
		private Point mMouseDownLocation = Point.Empty;
		private bool mMouseMoved = false;
		private const int mMouseMoveResolution = 5;
		private WaveFormRenderer mRenderer = null;
		private float mScaleTickMajor = 16f;
		private float mScaleTickMinor = 8f;
		private SolidBrush mSelectionBrush =
			new SolidBrush(ColorTranslator.FromHtml("#300000ff"));
		private Pen mZeroPen = new Pen(ColorTranslator.FromHtml("#ff0000"), 1f);

		//*-----------------------------------------------------------------------*
		//* FinishedRender																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The waveform render has completed.
		/// </summary>
		/// <param name="image">
		/// Reference to the rendered waveform image.
		/// </param>
		private void FinishedRender(Image image)
		{
			mWaveformBitmap = (Bitmap)image;
			UpdateWidth();
			this.Invalidate();
			OnWaveformReady(new EventArgs());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsInRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the left and width values are within
		/// the provided area.
		/// </summary>
		/// <param name="area">
		/// The area to match.
		/// </param>
		/// <param name="left">
		/// The left coordinate to test.
		/// </param>
		/// <param name="width">
		/// The width to test.
		/// </param>
		/// <returns>
		/// True if the left and width values coincide with the supplied area.
		/// </returns>
		private bool IsInRange(Rectangle area, int left, int width)
		{
			bool result = false;

			if(left <= area.Right && left + width >= area.Left)
			{
				result = true;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RenderThread																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Stand-alone rendering thread.
		/// </summary>
		/// <param name="peakProvider">
		/// Peak analysis provider.
		/// </param>
		/// <param name="settings">
		/// Rendering settings.
		/// </param>
		private void RenderThread(IPeakProvider peakProvider,
			WaveFormRendererSettings settings)
		{
			Image image = null;
			try
			{
				using(AudioFileReader waveStream =
					new AudioFileReader(mMediaFilename))
				{
					//	DEP20231117: By setting width here, we get actual duration.
					settings.Width =
						(int)(waveStream.TotalTime.TotalSeconds * mPixelsPerSecond);
					image = mRenderer.Render(waveStream, peakProvider, settings);
				}
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message);
			}
			BeginInvoke((Action)(() => FinishedRender(image)));
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnCaptionDoubleClick																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the CaptionDoubleClick event when a caption has been
		/// double-clicked.
		/// </summary>
		/// <param name="e">
		/// Caption click event arguments.
		/// </param>
		protected virtual void OnCaptionDoubleClick(CaptionClickEventArgs e)
		{
			CaptionDoubleClick?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseDoubleClick																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseDoubleClick event when a double-click has occurred on
		/// the canvas.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			CaptionItem caption = null;
			CaptionClickEventArgs ce = null;
			double seconds = (double)e.X / mPixelsPerSecond;

			base.OnMouseDoubleClick(e);
			if((mWaveformVisible && e.Y > mWaveformHeight) ||
				(!mWaveformVisible && e.Y > mCaptionMinY))
			{
				caption = mCaptions.GetItemAtX(seconds);
				if(caption != null)
				{
					ce = new CaptionClickEventArgs()
					{
						Caption = caption,
						X = seconds
					};
					OnCaptionDoubleClick(ce);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseDown																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse button has been depressed.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			double seconds = (double)e.X / mPixelsPerSecond;

			if(e.Button == MouseButtons.Left)
			{
				mMouseDown = true;
				mMouseDownLocation = new Point(e.X, e.Y);
				mMouseMoved = false;
				base.OnMouseDown(e);
				if((mWaveformVisible && e.Y <= mWaveformHeight) ||
					e.Y <= mCaptionMinY)
				{
					mMouseArea = CaptionAreaEnum.Waveform;
					SelectionEnd = SelectionStart = Playhead = seconds;
				}
				else
				{
					mMouseArea = CaptionAreaEnum.Caption;
					mCaptionSelected =
						mCaptions.GetItemAtX(seconds);
					mCaptions.Select(mCaptionSelected);
					if(mCaptionSelected != null)
					{
						if(seconds > mCaptionSelected.X + (mCaptionSelected.Width / 2d))
						{
							mCaptionEditMode = CaptionEditModeEnum.Width;
							mCaptionLeftX = mCaptionSelected.X + 0.1d;
							mCaptionPartner = mCaptions.GetNext(mCaptionSelected);
							if(mCaptionPartner != null)
							{
								mCaptionRightX =
									mCaptionPartner.X + mCaptionPartner.Width - 0.1d;
							}
							else
							{
								mCaptionRightX = mDuration;
							}
						}
						else
						{
							mCaptionEditMode = CaptionEditModeEnum.Left;
							mCaptionRightX =
								mCaptionSelected.X + mCaptionSelected.Width - 0.1d;
							mCaptionPartner = mCaptions.GetPrevious(mCaptionSelected);
							if(mCaptionPartner != null)
							{
								mCaptionLeftX = mCaptionPartner.X + 0.1d;
							}
							else
							{
								mCaptionLeftX = 0d;
							}
						}
					}
					else
					{
						mCaptionEditMode = CaptionEditModeEnum.None;
					}
					this.Invalidate();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseMove																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse is moving over the control.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			double right = 0d;
			double seconds = (double)e.X / mPixelsPerSecond;

			base.OnMouseMove(e);
			if(mMouseDown && e.Button == MouseButtons.Left)
			{
				Debug.WriteLine($"Mouse move?: isMoving: {mMouseMoved}," +
					$"e.X: {e.X}, e.Y: {e.Y}," +
					$"mdLocationX: {mMouseDownLocation.X}, " +
					$"mdLocationY: {mMouseDownLocation.Y}");
				if(!mMouseMoved &&
					(Math.Abs(e.X - mMouseDownLocation.X) > mMouseMoveResolution) ||
					(Math.Abs(e.Y - mMouseDownLocation.Y) > mMouseMoveResolution))
				{
					mMouseMoved = true;
					if(mMouseArea == CaptionAreaEnum.Waveform)
					{
						SelectionStart = Playhead;
					}
				}
				if(mMouseMoved)
				{
					switch(mMouseArea)
					{
						case CaptionAreaEnum.Caption:
							if(mCaptionSelected != null &&
								seconds >= mCaptionLeftX &&
								seconds <= mCaptionRightX)
							{
								switch(mCaptionEditMode)
								{
									case CaptionEditModeEnum.Left:
										right = mCaptionSelected.X + mCaptionSelected.Width;
										mCaptionSelected.X = seconds;
										mCaptionSelected.Width = right - mCaptionSelected.X;
										if(mCaptionPartner != null)
										{
											mCaptionPartner.Width =
												mCaptionSelected.X - mCaptionPartner.X;
										}
										break;
									case CaptionEditModeEnum.Width:
										mCaptionSelected.Width = seconds - mCaptionSelected.X;
										if(mCaptionPartner != null)
										{
											right = mCaptionPartner.X + mCaptionPartner.Width;
											mCaptionPartner.X = seconds;
											mCaptionPartner.Width = right - mCaptionPartner.X;
										}
										break;
								}
								this.Invalidate();
							}
							break;
						case CaptionAreaEnum.Waveform:
							SelectionEnd = (double)e.X / mPixelsPerSecond;
							break;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseUp																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse button has been released over the control.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			mMouseDown = false;
			mMouseArea = CaptionAreaEnum.None;
			base.OnMouseUp(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnPaint																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Paint event when the control should be painted.
		/// </summary>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			//PaintArea(e.Graphics, e.ClipRectangle,
			//	new Rectangle(this.Location, this.Size));
			PaintArea(e.Graphics, e.ClipRectangle,
				new Rectangle(mDrawingLocation.ToPoint(), mDrawingSize.ToSize()));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnPlayheadChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the PlayheadChanged event when the value of the Playhead
		/// property has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnPlayheadChanged(EventArgs e)
		{
			PlayheadChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnResize																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Resize event when the control has been resized.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnSelectionEndChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the SelectionEndChanged event when the value of the SelectionEnd
		/// property has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnSelectionEndChanged(EventArgs e)
		{
			SelectionEndChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnSelectionStartChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the SelectionStartChanged event when the value of the
		/// SelectionStart property has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnSelectionStartChanged(EventArgs e)
		{
			SelectionStartChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnWaveformReady																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the WaveformReady event when the waveform bitmap has been
		/// rendered.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnWaveformReady(EventArgs e)
		{
			WaveformReady?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ProcessCmdKey																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Process arrow keys for this control.
		/// </summary>
		/// <param name="msg">
		/// The command message being passed.
		/// </param>
		/// <param name="keyData">
		/// Key data to analyze.
		/// </param>
		/// <returns>
		/// Value indicating whether the key was handled.
		/// </returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = base.ProcessCmdKey(ref msg, keyData);

			if(!result)
			{
				switch(keyData)
				{
					case Keys.Left:
						if(Playhead >= 1d)
						{
							Playhead -= 1d;
							result = true;
						}
						break;
					case Keys.Right:
						if(Playhead <= Duration - 1d)
						{
							Playhead += 1d;
							result = true;
						}
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the UserControl Item.
		/// </summary>
		public CaptionBubbleControl()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			mRenderer = new WaveFormRenderer();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CaptionDoubleClick																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A caption has been double-clicked.
		/// </summary>
		public event CaptionClickEventHandler CaptionDoubleClick;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Captions																															*
		//*-----------------------------------------------------------------------*
		private CaptionCollection mCaptions = new CaptionCollection();
		/// <summary>
		/// Get a reference to the collection of captions loaded.
		/// </summary>
		public CaptionCollection Captions
		{
			get { return mCaptions; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the caption bubble control's media display.
		/// </summary>
		public void ClearMedia()
		{
			mMediaFilename = "";
			mWaveformBitmap = null;
			mRenderer = new WaveFormRenderer();
			this.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DrawingLocation																												*
		//*-----------------------------------------------------------------------*
		private PointEv mDrawingLocation = new PointEv();
		/// <summary>
		/// Get a reference to the drawing location coordinate for this paintable
		/// object.
		/// </summary>
		public PointEv DrawingLocation
		{
			get { return mDrawingLocation; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	DrawingLocation																												*
		////*-----------------------------------------------------------------------*
		//private PointEv mDrawingLocation = new PointEv();
		///// <summary>
		///// Get/Set the virtual drawing location of this control.
		///// </summary>
		//public PointEv DrawingLocation
		//{
		//	get
		//	{
		//		PointEv result = new PointEv()
		//		{
		//			Left = mDrawingLocation.Left,
		//			Top = mDrawingLocation.Top
		//		};

		//		if(result.Left == 0)
		//		{
		//			result.Left = this.Left;
		//		}
		//		if(result.Top == 0)
		//		{
		//			result.Top = this.Top;
		//		}
		//		result.LocationChanged += DrawingLocation_LocationChanged;
		//		return result;
		//	}
		//	set { mDrawingLocation = value; }
		//}
		////*-----------------------------------------------------------------------*

		//private void DrawingLocation_LocationChanged(object sender, EventArgs e)
		//{
		//	if(sender != null && sender is PointEv point &&
		//		point != mDrawingLocation)
		//	{
		//		//	This item is being updated by a delegate.
		//		mDrawingLocation.Left = point.Left;
		//		mDrawingLocation.Top = point.Top;
		//	}
		//}

		//*-----------------------------------------------------------------------*
		//*	DrawingSize																														*
		//*-----------------------------------------------------------------------*
		private SizeEv mDrawingSize = new SizeEv();
		/// <summary>
		/// Get a reference to the paintable size dimensions for this object.
		/// </summary>
		public SizeEv DrawingSize
		{
			get { return mDrawingSize; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	DrawingSize																														*
		////*-----------------------------------------------------------------------*
		//private SizeEv mDrawingSize = new SizeEv();
		///// <summary>
		///// Get/Set the drawing size of this component.
		///// </summary>
		//public SizeEv DrawingSize
		//{
		//	get
		//	{
		//		SizeEv result = new SizeEv()
		//		{
		//			Height = mDrawingSize.Height,
		//			Width = mDrawingSize.Width
		//		};

		//		//	If part of the drawing size hasn't been initialized, then use
		//		//	values from the control.
		//		if(result.Width == 0)
		//		{
		//			result.Width = this.Width;
		//		}
		//		if(result.Height == 0)
		//		{
		//			result.Height = this.Height;
		//		}
		//		result.SizeChanged += DrawingSize_SizeChanged;
		//		return result;
		//	}
		//	set { mDrawingSize = value; }
		//}
		////*-----------------------------------------------------------------------*

		//private void DrawingSize_SizeChanged(object sender, EventArgs e)
		//{
		//	if(sender != null && sender is SizeEv size &&
		//		size != mDrawingSize)
		//	{
		//		//	This value is being updated by a delegate.
		//		mDrawingSize.Height = size.Height;
		//		mDrawingSize.Width = size.Width;
		//	}
		//}

		//*-----------------------------------------------------------------------*
		//*	Duration																															*
		//*-----------------------------------------------------------------------*
		private double mDuration = 0d;
		/// <summary>
		/// Get the duration of the media, in seconds.
		/// </summary>
		public double Duration
		{
			get { return mDuration; }
			set
			{
				CaptionItem captionLast = (mCaptions.Count > 0 ? mCaptions[^1] : null);
				CaptionItem captionNew = null;
				double duration = value;
				double minDuration =
					(captionLast != null ? captionLast.X + 0.25d : 0.25d);
				double right = 0d;

				duration = Math.Max(duration, minDuration);
				if(captionLast != null)
				{
					//	Caption was present.
					right = captionLast.X + captionLast.Width;
					if(right > duration)
					{
						//	Last caption will be shrunken.
						captionLast.Width = duration - captionLast.X;
					}
					else if(right < duration)
					{
						//	Last caption will be extended if it is a space or appended
						//	to with a space if it is text.
						if(captionLast.EntryType == CaptionEntryTypeEnum.Normal)
						{
							//	Text.
							captionNew = new CaptionItem()
							{
								EntryType = CaptionEntryTypeEnum.Space,
								Width = duration - right,
								X = right
							};
							mCaptions.Add(captionNew);
						}
						else
						{
							//	Space.
							captionLast.Width = duration - captionLast.X;
						}
					}
				}
				else
				{
					//	Add a default space.
					mCaptions.Add(new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Space,
						Width = duration,
						X = 0d
					});
				}
				mDuration = duration;
				//this.Width = (int)(mDuration * mPixelsPerSecond);
				this.mDrawingSize.Width = (int)(mDuration * mPixelsPerSecond);
				this.OnResize(new EventArgs());
			}
		}
		//*-----------------------------------------------------------------------*

		public new int Height
		{
			get { return base.Height; }
			set
			{
				mDrawingSize.Height = value;
				base.Height = value;
			}
		}

		public new int Left
		{
			get { return base.Left; }
			set
			{
				mDrawingLocation.Left = value;
				base.Left = value;
			}
		}

		//*-----------------------------------------------------------------------*
		//*	LoadWaveform																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Load and render the waveform.
		/// </summary>
		/// <param name="filename">
		/// Fully qualified path and filename of the media file to load.
		/// </param>
		public void LoadWaveform(string filename, double duration)
		{
			//Bitmap bitmap = null;
			int height = (mWaveformHeight > 2 ? mWaveformHeight : 32);
			IPeakProvider peakProvider = new MaxPeakProvider();
			WaveFormRendererSettings settings = null;

			if(filename?.Length > 0 && duration > 0)
			{
				mMediaFilename = filename;
				//	DEP20231117: +1 added to control media duration.
				Duration = Math.Max(duration + 1, mCaptions.Duration);
				//this.Width = (int)(mDuration * mPixelsPerSecond);
				//bitmap = new Bitmap(this.Width, height);
				settings = new StandardWaveFormRendererSettings()
				{
					BackgroundColor = Color.Transparent,
					//BackgroundImage = bitmap,
					DecibelScale = false,
					Name = "Standard",
					TopPeakPen = new Pen(ColorTranslator.FromHtml("#003660"), 1f),
					BottomPeakPen = new Pen(ColorTranslator.FromHtml("#350f70"), 1f),
					BottomSpacerPen = new Pen(ColorTranslator.FromHtml("#221746"), 1f),
					TopHeight = (height / 2),
					BottomHeight = (height / 2),
					//	DEP20231117: Width is now resolved by reader in RenderThread.
					//Width = (int)(duration * mPixelsPerSecond)
				};
				Task.Factory.StartNew(() =>
					RenderThread(peakProvider, settings));

			}
			mPlayhead = 0d;
		}
		//*-----------------------------------------------------------------------*

		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				mDrawingLocation.Left = value.X;
				mDrawingLocation.Top = value.Y;
				base.Location = value;
			}
		}

		////*-----------------------------------------------------------------------*
		////*	MasterLeft																														*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Get/Set the master left coordinate of this control.
		///// </summary>
		//public int MasterLeft
		//{
		//	get { return mMasterLocation.X; }
		//	set
		//	{
		//		bool changed = (mMasterLocation.X != value);
		//		mMasterLocation.X = value;
		//		if(changed)
		//		{
		//			OnLocationChanged(new EventArgs());
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		// //*-----------------------------------------------------------------------*
		// //*	MasterLocation																												*
		// //*-----------------------------------------------------------------------*
		// private Point mMasterLocation = Point.Empty;
		// /// <summary>
		// /// Get/Set the master location of this control.
		// /// </summary>
		// public Point MasterLocation
		// {
		// 	get { return mMasterLocation; }
		// 	set
		// 	{
		// 		bool changed = !object.Equals(mMasterLocation, value);

		// 		mMasterLocation = value;
		// 		if(changed)
		// 		{
		// 			OnLocationChanged(new EventArgs());
		// 		}
		// 	}
		// }
		// //*-----------------------------------------------------------------------*

		// //*-----------------------------------------------------------------------*
		// //*	MasterTop																															*
		// //*-----------------------------------------------------------------------*
		// /// <summary>
		// /// Get/Set the master top coordinate of this control.
		// /// </summary>
		// public int MasterTop
		// {
		// 	get { return mMasterLocation.Y; }
		// 	set
		// 	{
		// 		bool changed = (mMasterLocation.Y != value);
		// 		mMasterLocation.Y = value;
		// 		if(changed)
		// 		{
		// 			OnLocationChanged(new EventArgs());
		// 		}
		// 	}
		// }
		// //*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MediaFilename																													*
		//*-----------------------------------------------------------------------*
		private string mMediaFilename = "";
		/// <summary>
		/// Get the filename of the currently assigned media file.
		/// </summary>
		public string MediaFilename
		{
			get { return mMediaFilename; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PaintArea																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Paint a portion of this control's client area onto the provided host
		/// area.
		/// </summary>
		/// <param name="graphics">
		/// Reference to the active graphics object.
		/// </param>
		/// <param name="clientArea">
		/// The client area to draw.
		/// </param>
		/// <param name="hostArea">
		/// The target host area.
		/// </param>
		public void PaintArea(Graphics graphics,
			Rectangle clientArea, Rectangle hostArea)
		{
			Rectangle area = hostArea;
			RectangleF captionArea = RectangleF.Empty;
			List<CaptionItem> captions = null;
			float dx = 0f;
			Graphics g = graphics;
			float height = 0f;
			int index = 0;
			Rectangle meterArea = Rectangle.Empty;
			GraphicsPath path = null;
			float px = 0f;
			float width = 0f;
			float x = 0f;
			float y = 0f;

			g.CompositingMode =
				System.Drawing.Drawing2D.CompositingMode.SourceOver;
			g.CompositingQuality =
				System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
			g.SmoothingMode =
				System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.InterpolationMode =
				System.Drawing.Drawing2D.InterpolationMode.Bilinear;

			area = new Rectangle(hostArea.X, hostArea.Y,
				Math.Min(hostArea.Width, clientArea.Width),
				Math.Min(hostArea.Height, clientArea.Height));

			//	Clear the background.
			g.FillRectangle(mBackgroundBrush, area);

			//	Draw the waveform.
			if(mWaveformVisible && mWaveformHeight > 0)
			{
				meterArea =
					new Rectangle(area.X - 1, 0, area.Width + 2, mWaveformHeight);
				g.FillRectangle(mMeterBackBrush, meterArea);
				if(mWaveformBitmap != null)
				{
					g.DrawImage(mWaveformBitmap,
						hostArea.X, hostArea.Y, clientArea, GraphicsUnit.Pixel);
				}
				y = mWaveformHeight / 2;
				g.DrawLine(mZeroPen, area.X, y, area.X + area.Width, y);
				g.DrawLine(mLinePen,
					area.X, mWaveformHeight, area.X + area.Width, mWaveformHeight);
				y = mWaveformHeight + 2;
			}
			else
			{
				y = mCaptionMinY;
			}

			//	Draw the captions.
			captions = mCaptions.FindAll(c => IsInRange(clientArea,
				(int)(c.X * mPixelsPerSecond), (int)(c.Width * mPixelsPerSecond)));
			foreach(CaptionItem captionItem in captions)
			{
				if(captionItem.EntryType != CaptionEntryTypeEnum.Space)
				{
					captionArea = new RectangleF(
						(float)(captionItem.X * mPixelsPerSecond) - clientArea.X,
						mWaveformHeight + 4f,
						(float)(captionItem.Width * mPixelsPerSecond),
						clientArea.Height - mWaveformHeight - 8f);
					path = GetRoundedRect(captionArea, 8f);
					if(mCaptions.SelectedItems.Contains(captionItem))
					{
						g.FillPath(mCaptionBrushSelected, path);
						g.DrawPath(mCaptionPenSelected, path);
					}
					else
					{
						g.FillPath(mCaptionBrush, path);
						g.DrawPath(mCaptionPen, path);
					}
					captionArea = new RectangleF(
						captionArea.X + 6,
						captionArea.Y + 6,
						captionArea.Width - 12,
						captionArea.Height - 12);
					g.DrawString(captionItem.Text,
						mCaptionFont, mCaptionFontBrush, captionArea);
				}
			}

			//	Draw the scale.
			g.DrawLine(mLinePen, 0, 0, mDrawingSize.Width, 0);
			//g.DrawLine(mLinePen, 0, 0, this.Width, 0);
			dx = (float)(mPixelsPerSecond / 2d);
			//for(px = 0, index = 0; px < this.Width; px += dx, index++)
			for(px = 0, index = 0; px < mDrawingSize.Width; px += dx, index++)
			{
				x = (int)px;
				if(x >= clientArea.X && x <= clientArea.Right)
				{
					if(index % 2 == 0)
					{
						g.DrawLine(mLinePen,
							px - clientArea.X, 0f, px - clientArea.X, mScaleTickMajor);
					}
					else
					{
						g.DrawLine(mLinePen,
							px - clientArea.X, 0f, px - clientArea.X, mScaleTickMinor);
					}
				}
			}

			//	Draw selection.
			if(mSelectionStart > 0 && mSelectionEnd > mSelectionStart)
			{
				x = (int)(mSelectionStart * mPixelsPerSecond) - clientArea.X;
				y = 0;
				width = (float)((mSelectionEnd - mSelectionStart) * mPixelsPerSecond);
				height = clientArea.Height;
				g.FillRectangle(mSelectionBrush, x, y, width, height);
			}

			//	Draw playhead indicator.
			x = (int)(mPlayhead * mPixelsPerSecond) - clientArea.X;
			y = mWaveformHeight - 1;
			path = new GraphicsPath();
			path.AddLines(new PointF[]
			{
				new PointF() { X = x - 8, Y = 0 },
				new PointF() { X = x, Y = 8 },
				new PointF() { X = x + 8, Y = 0 }
			});
			g.FillPath(mGlowBrush, path);
			g.DrawLine(mGlowPen, x, 0, x, Math.Max(y, 32));
			g.DrawLine(mGlowPenDark, x - 1, 0, x - 1, Math.Max(y, 32));
			g.DrawLine(mGlowPenDark, x + 1, 0, x + 1, Math.Max(y, 32));

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PixelsPerSecond																												*
		//*-----------------------------------------------------------------------*
		private double mPixelsPerSecond = 80f;
		/// <summary>
		/// Get/Set the number of pixels to display per second of play time.
		/// </summary>
		public double PixelsPerSecond
		{
			get { return mPixelsPerSecond; }
			set { mPixelsPerSecond = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Playhead																															*
		//*-----------------------------------------------------------------------*
		private double mPlayhead = 0d;
		/// <summary>
		/// Get/Set the current playhead position.
		/// </summary>
		public double Playhead
		{
			get { return mPlayhead; }
			set
			{
				mPlayhead = value;
				this.Invalidate();
				OnPlayheadChanged(new EventArgs());
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlayheadChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the value of the playhead has changed.
		/// </summary>
		public event EventHandler PlayheadChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RaiseMouseDoubleClick																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raise the MouseDoubleClick event.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		public void RaiseMouseDoubleClick(MouseEventArgs e)
		{
			OnMouseDoubleClick(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RaiseMouseDown																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raise the MouseDown event.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		public void RaiseMouseDown(MouseEventArgs e)
		{
			OnMouseDown(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RaiseMouseMove																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raise the MouseMove event.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		public void RaiseMouseMove(MouseEventArgs e)
		{
			OnMouseMove(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RaiseMouseUp																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raise the MouseUp event.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		public void RaiseMouseUp(MouseEventArgs e)
		{
			OnMouseUp(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelectionEnd																													*
		//*-----------------------------------------------------------------------*
		private double mSelectionEnd = 0d;
		/// <summary>
		/// Get/Set the ending selection point, in seconds.
		/// </summary>
		public double SelectionEnd
		{
			get { return mSelectionEnd; }
			set
			{
				bool changed = (mSelectionEnd != value);

				mSelectionEnd = value;
				if(mSelectionEnd < mSelectionStart || mSelectionEnd < 0d)
				{
					mSelectionEnd = Math.Max(mSelectionStart, 0d);
				}
				if(changed)
				{
					OnSelectionEndChanged(new EventArgs());
					this.Invalidate();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SelectionEndChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selection end value has changed.
		/// </summary>
		public event EventHandler SelectionEndChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelectionStart																												*
		//*-----------------------------------------------------------------------*
		private double mSelectionStart = 0d;
		/// <summary>
		/// Get/Set the starting selection point, in seconds.
		/// </summary>
		public double SelectionStart
		{
			get { return mSelectionStart; }
			set
			{
				bool changed = (mSelectionStart != value);

				mSelectionStart = value;
				if(mSelectionEnd < mSelectionStart || mSelectionStart < 0d)
				{
					SelectionEnd = Math.Max(mSelectionStart, 0d);
				}
				if(changed)
				{
					OnSelectionStartChanged(new EventArgs());
					this.Invalidate();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SelectionStartChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selection start value has changed.
		/// </summary>
		public event EventHandler SelectionStartChanged;
		//*-----------------------------------------------------------------------*

		public new System.Drawing.Size Size
		{
			get { return base.Size; }
			set
			{
				mDrawingSize.Width = value.Width;
				mDrawingSize.Height = value.Height;
				base.Size = value;
			}
		}

		public new int Top
		{
			get { return base.Top; }
			set
			{
				mDrawingLocation.Top = value;
				base.Top = value;
			}
		}

		//*-----------------------------------------------------------------------*
		//* UpdateWidth																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the width of this control.
		/// </summary>
		public void UpdateWidth()
		{
			if(mWaveformBitmap != null)
			{
				Duration = Math.Max(
					(double)mWaveformBitmap.Width / mPixelsPerSecond,
					mCaptions.Duration);
			}
			else
			{
				Duration = Math.Max(10d, mCaptions.Duration);
			}
			this.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WaveformBitmap																												*
		//*-----------------------------------------------------------------------*
		private Bitmap mWaveformBitmap = null;
		/// <summary>
		/// Get/Set a reference to the rendered waveform bitmap for this instance.
		/// </summary>
		/// <remarks>
		/// If a media file has not yet been loaded, the bitmap will be null.
		/// </remarks>
		public Bitmap WaveformBitmap
		{
			get { return mWaveformBitmap; }
			set { mWaveformBitmap = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WaveformHeight																												*
		//*-----------------------------------------------------------------------*
		private int mWaveformHeight = 100;
		/// <summary>
		/// Get/Set the height of the waveform display, when active.
		/// </summary>
		public int WaveformHeight
		{
			get { return mWaveformHeight; }
			set { mWaveformHeight = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WaveformReady																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the waveform has been rendered.
		/// </summary>
		public event EventHandler WaveformReady;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WaveformVisible																												*
		//*-----------------------------------------------------------------------*
		private bool mWaveformVisible = true;
		/// <summary>
		/// Get/Set a value indicating whether the waveform is visible.
		/// </summary>
		public bool WaveformVisible
		{
			get { return mWaveformVisible; }
			set { mWaveformVisible = value; }
		}
		//*-----------------------------------------------------------------------*

		public new int Width
		{
			get { return base.Width; }
			set
			{
				mDrawingSize.Width = value;
				base.Width = value;
			}
		}

	}
	//*-------------------------------------------------------------------------*
}
