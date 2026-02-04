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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaPlayerWPFUC
{
	//*-------------------------------------------------------------------------*
	//*	MediaPlayerWPF																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// WPF Media Player control.
	/// </summary>
	public partial class MediaPlayerWPF : UserControl
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private bool mSelectionEndFocused = false;
		private string mSelectionEndText = "";
		private bool mSelectionStartFocused = false;
		private string mSelectionStartText = "";

		//*-----------------------------------------------------------------------*
		//* FormatSelectionText																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Format a selection textbox with a general time syntax.
		/// </summary>
		/// <param name="textBox">
		/// Reference to the textbox control containing the text to be formatted.
		/// </param>
		private void FormatSelectionText(TextBox textBox)
		{
			float seconds = 0;
			TimeSpan timeSpan = new TimeSpan(0, 0, 0);
			string text = textBox.Text;

			if(text.IndexOfAny(new char[] { ':' }) == -1)
			{
				if(float.TryParse(text, out seconds))
				{
					timeSpan = new TimeSpan((long)(seconds * 1000 * 10000));
				}
				else
				{
					timeSpan = new TimeSpan(0, 0, 0);
				}
			}
			else if(text.Count(x => x == ':') == 1)
			{
				//	User is indicating minutes and seconds.
				text = $"00:{text}";
				if(!TimeSpan.TryParse(text, out timeSpan))
				{
					timeSpan = new TimeSpan(0, 0, 0);
				}
			}
			else
			{
				if(!TimeSpan.TryParse(text, out timeSpan))
				{
					//	A valid parseable timespan was not supplied.
					timeSpan = new TimeSpan(0, 0, 0);
				}
			}
			textBox.Text = timeSpan.ToString(@"hh\:mm\:ss\.fff");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetTimeSpan																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a time span corresponding to a text-based time expression.
		/// </summary>
		/// <param name="time">
		/// Time-formatted text.
		/// </param>
		/// <returns>
		/// Reference to a time span object containing the specified time.
		/// </returns>
		private TimeSpan GetTimeSpan(string time)
		{
			TimeSpan result = TimeSpan.MinValue;

			if(!TimeSpan.TryParse(time, out result))
			{
				//	A valid parseable timespan was not supplied.
				result = new TimeSpan(0, 0, 0);
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a time span corresponding to a text-based time expression in
		/// a textbox.
		/// </summary>
		/// <param name="textBox">
		/// Reference to a textbox containing time-formatted text.
		/// </param>
		/// <returns>
		/// Reference to a time span object containing the specified time.
		/// </returns>
		private TimeSpan GetTimeSpan(TextBox textBox)
		{
			return GetTimeSpan(textBox.Text);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtSelectionEnd_GotFocus																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection End textbox has received focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Rounted event arguments.
		/// </param>
		private void txtSelectionEnd_GotFocus(object sender, RoutedEventArgs e)
		{
			mSelectionEndText = txtSelectionEnd.Text;
			mSelectionEndFocused = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtSelectionEnd_LostFocus																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection End textbox has lost focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Rounted event arguments.
		/// </param>
		private void txtSelectionEnd_LostFocus(object sender, RoutedEventArgs e)
		{
			FormatSelectionText(txtSelectionEnd);
			if(GetTimeSpan(txtSelectionStart) > GetTimeSpan(txtSelectionEnd))
			{
				txtSelectionEnd.Text = txtSelectionStart.Text;
			}
			if(txtSelectionEnd.Text != mSelectionEndText)
			{
				OnSelectionEndTextChanged(new EventArgs());
			}
			mSelectionEndFocused = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtSelectionEnd_TextChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection End text value has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Text changed event arguments.
		/// </param>
		private void txtSelectionEnd_TextChanged(object sender,
			TextChangedEventArgs e)
		{
			if(!mSelectionEndFocused)
			{
				OnSelectionEndTextChanged(new EventArgs());
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtSelectionStart_GotFocus																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection Start textbox has received focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Rounted event arguments.
		/// </param>
		private void txtSelectionStart_GotFocus(object sender, RoutedEventArgs e)
		{
			mSelectionStartText = txtSelectionStart.Text;
			mSelectionStartFocused = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtSelectionStart_LostFocus																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection Start textbox has lost focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Rounted event arguments.
		/// </param>
		private void txtSelectionStart_LostFocus(object sender, RoutedEventArgs e)
		{
			FormatSelectionText(txtSelectionStart);
			if(GetTimeSpan(txtSelectionStart) > GetTimeSpan(txtSelectionEnd))
			{
				txtSelectionEnd.Text = txtSelectionStart.Text;
			}
			if(txtSelectionStart.Text != mSelectionStartText)
			{
				OnSelectionStartTextChanged(new EventArgs());
			}
			mSelectionStartFocused = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtSelectionStart_TextChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection Start text value has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Text changed event arguments.
		/// </param>
		private void txtSelectionStart_TextChanged(object sender,
			TextChangedEventArgs e)
		{
			if(!mSelectionStartFocused)
			{
				OnSelectionStartTextChanged(new EventArgs());
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnSelectionEndTextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectionEndTextChanged(EventArgs e)
		{
			SelectionEndTextChanged?.Invoke(this, new EventArgs());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnSelectionStartTextChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectionStartTextChanged(EventArgs e)
		{
			SelectionStartTextChanged?.Invoke(this, new EventArgs());
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the MediaPlayerWPF Item.
		/// </summary>
		public MediaPlayerWPF()
		{
			InitializeComponent();
			txtSelectionEnd.GotFocus += txtSelectionEnd_GotFocus;
			txtSelectionEnd.LostFocus += txtSelectionEnd_LostFocus;
			txtSelectionEnd.TextChanged += txtSelectionEnd_TextChanged;
			txtSelectionStart.GotFocus += txtSelectionStart_GotFocus;
			txtSelectionStart.LostFocus += txtSelectionStart_LostFocus;
			txtSelectionStart.TextChanged += txtSelectionStart_TextChanged;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Caption																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the visible caption text.
		/// </summary>
		public string Caption
		{
			get { return captionCurrent.MessageText; }
			set { captionCurrent.MessageText = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the media player.
		/// </summary>
		public void Clear()
		{
			mediaPlayer.Stop();
			mediaPlayer.Source = null;
			mIsPlaying = false;
			clockDisplay.Time = new TimeSpan(0);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Duration																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the duration of the loaded media, in seconds.
		/// </summary>
		public double Duration
		{
			get { return mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsPlaying																															*
		//*-----------------------------------------------------------------------*
		private bool mIsPlaying = false;
		/// <summary>
		/// Get a value indicating whether the player is playing.
		/// </summary>
		public bool IsPlaying
		{
			get { return mIsPlaying; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Load																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Load the player from the specified filename.
		/// </summary>
		/// <param name="filename">
		/// Fully qualified path and filename of the media to load.
		/// </param>
		public void Load(string filename)
		{
			mediaPlayer.Source = new Uri(filename);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Play																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Play the currently loaded media.
		/// </summary>
		public void Play()
		{
			mediaPlayer.Play();
			mIsPlaying = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Player																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get a reference to the media player element on this control.
		/// </summary>
		public MediaElement Player
		{
			get { return mediaPlayer; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Pause																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Pause the media player.
		/// </summary>
		public void Pause()
		{
			mediaPlayer.Pause();
			mIsPlaying = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelectionEndText																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the selection end time text.
		/// </summary>
		public string SelectionEndText
		{
			get { return txtSelectionEnd.Text; }
			set { txtSelectionEnd.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SelectionEndTextChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection End text value has changed.
		/// </summary>
		public event EventHandler SelectionEndTextChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelectionStartText																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the selection start time text.
		/// </summary>
		public string SelectionStartText
		{
			get { return txtSelectionStart.Text; }
			set { txtSelectionStart.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SelectionStartTextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Selection Start text value has changed.
		/// </summary>
		public event EventHandler SelectionStartTextChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetClock																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the time displayed on the digital clock.
		/// </summary>
		/// <param name="timeSpan">
		/// The time to display.
		/// </param>
		public void SetClock(TimeSpan timeSpan)
		{
			clockDisplay.Time = timeSpan;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetFocusSelectionEnd																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the current focus to the Selection End textbox.
		/// </summary>
		public void SetFocusSelectionEnd()
		{
			txtSelectionEnd.Focus();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetFocusSelectionStart																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the current focus to the Selection Start textbox.
		/// </summary>
		public void SetFocusSelectionStart()
		{
			txtSelectionStart.Focus();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Stop																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Stop the media player.
		/// </summary>
		public void Stop()
		{
			mediaPlayer.Stop();
			mIsPlaying = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Position																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the current position.
		/// </summary>
		public TimeSpan Position
		{
			get { return mediaPlayer.Position; }
			set { mediaPlayer.Position = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
