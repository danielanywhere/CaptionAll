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
using System.Linq;
//using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;


namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	WordRecognizedCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of WordRecognizedItem Items.
	/// </summary>
	public class WordRecognizedCollection : List<WordRecognizedItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//*	LastRecognition																												*
		//*-----------------------------------------------------------------------*
		private DateTime mLastRecognition = DateTime.Now;
		/// <summary>
		/// Get/Set the last time upon which a word was recognized on this instance.
		/// </summary>
		public DateTime LastRecognition
		{
			get { return mLastRecognition; }
			set { mLastRecognition = value; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* SpeechRecognized																											*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// A word has been recognized.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Speech recognized event arguments.
		///// </param>
		//public void SpeechRecognized(object sender,
		//	SpeechRecognizedEventArgs e)
		//{
		//	//Console.Write($" Rec: {e.Result.Text} ");
		//	//Console.Write($"at {e.Result.Audio.AudioPosition.ToString()} ");
		//	//Console.WriteLine($"for {e.Result.Audio.Duration.ToString()}");
		//	this.Add(new WordRecognizedItem()
		//	{
		//		Text = e.Result.Text,
		//		TimeBegin = e.Result.Audio.AudioPosition,
		//		TimeEnd = e.Result.Audio.AudioPosition + e.Result.Audio.Duration
		//	});
		//	mTimeSinceLastRecognition = TimeSpan.Zero;
		//	mLastRecognition = DateTime.Now;
		//	//mRecognitionCount++;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TimeSinceLastRecognition																							*
		//*-----------------------------------------------------------------------*
		private TimeSpan mTimeSinceLastRecognition = TimeSpan.Zero;
		/// <summary>
		/// Get/Set the time since the last recognition was made.
		/// </summary>
		public TimeSpan TimeSinceLastRecognition
		{
			get { return mTimeSinceLastRecognition; }
			set { mTimeSinceLastRecognition = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	WordRecognizedItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual word placement.
	/// </summary>
	public class WordRecognizedItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	Text																																	*
		//*-----------------------------------------------------------------------*
		private string mText = "";
		/// <summary>
		/// Get/Set the text of the word.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set { mText = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TimeBegin																															*
		//*-----------------------------------------------------------------------*
		private TimeSpan mTimeBegin = TimeSpan.Zero;
		/// <summary>
		/// Get/Set the relative time at which the word begins.
		/// </summary>
		public TimeSpan TimeBegin
		{
			get { return mTimeBegin; }
			set { mTimeBegin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TimeEnd																																*
		//*-----------------------------------------------------------------------*
		private TimeSpan mTimeEnd = TimeSpan.Zero;
		/// <summary>
		/// Get/Set the relative time at which the word ends.
		/// </summary>
		public TimeSpan TimeEnd
		{
			get { return mTimeEnd; }
			set { mTimeEnd = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
