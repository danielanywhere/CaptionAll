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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static CaptionAll.CaptionAllUtil;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	WordTrackerCollection																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of WordTrackerItem Items.
	/// </summary>
	public class WordTrackerCollection : List<WordTrackerItem>
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
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the WordTrackerCollection Item.
		/// </summary>
		public WordTrackerCollection()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the WordTrackerCollection Item.
		/// </summary>
		/// <param name="text">
		/// Text with which to initialize the collection.
		/// </param>
		public WordTrackerCollection(string text)
		{
			MatchCollection matches = null;
			List<string> result = new List<string>();

			if(text?.Length > 0)
			{
				text = WordOnly(text);
				matches = Regex.Matches(text, "(?<word>[a-zA-Z]+)");
				foreach(Match matchItem in matches)
				{
					this.Add(new WordTrackerItem()
					{
						Text = GetValue(matchItem, "word")
					});
				}
			}
		}
		//*-----------------------------------------------------------------------*


		//*-----------------------------------------------------------------------*
		//* CompareConsistency																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Compare the consistency of two word lists.
		/// </summary>
		/// <param name="words1">
		/// Reference to the first and master set of words to compare.
		/// </param>
		/// <param name="words2">
		/// Reference to the second set of words to compare.
		/// </param>
		/// <returns>
		/// The number of matches found in word list 1 as a decimal of 1.
		/// </returns>
		public static double CompareConsistency(WordTrackerCollection words1,
			WordTrackerCollection words2)
		{
			double countFound = 0d;
			int countLeft = 0;
			int countRight = 0;
			int index = 0;
			int indexLeft = 0;
			int indexRight = 0;
			WordTrackerItem itemLeft = null;
			WordTrackerItem itemRight = null;
			double result = 0d;
			string wordLeft = "";

			if(words1?.Count > 0 && words2?.Count > 0)
			{
				countLeft = words1.Count;
				countRight = words2.Count;
				for(indexLeft = 0; indexLeft < countLeft; indexLeft ++)
				{
					//	Try to find each word in the left list.
					itemLeft = words1[indexLeft];
					wordLeft = itemLeft.Text;

					for(index = indexRight; index < countRight; index ++)
					{
						itemRight = words2[index];
						if(itemRight.Text == wordLeft)
						{
							itemLeft.MatchingItem = itemRight;
							itemRight.MatchingItem = itemLeft;
							countFound++;
							indexRight++;
							break;
						}
					}
				}
			}
			if(words1?.Count > 0)
			{
				result = countFound / words1.Count;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetNearestMatchingItem																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a reference to the nearest matching item to the item at the
		/// specified ordinal index of this list.
		/// </summary>
		/// <param name="index">
		/// Ordinal index at which to start searching.
		/// </param>
		/// <returns>
		/// Nearest matching word tracker item to the specified index in this
		/// list, if found. Otherwise, null.
		/// </returns>
		public WordTrackerItem GetNearestMatchingItem(int index)
		{
			bool bActivity = false;
			int count = this.Count;
			int radius = 0;
			WordTrackerItem result = null;
			WordTrackerItem word = null;

			if(index > -1 && index < count)
			{
				while(result == null)
				{
					bActivity = false;
					if(index - radius > -1)
					{
						word = this[index - radius];
						result = word.MatchingItem;
						bActivity = true;
					}
					if(result == null && radius > 0 && index + radius < count)
					{
						word = this[index + radius];
						result = word.MatchingItem;
						bActivity = true;
					}
					if(result == null)
					{
						radius++;
					}
					if(!bActivity)
					{
						//	Both directions have gone out of scope.
						break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	WordTrackerItem																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual word tracker.
	/// </summary>
	public class WordTrackerItem
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
		//*	Begin																																	*
		//*-----------------------------------------------------------------------*
		private TimeSpan mBegin = TimeSpan.Zero;
		/// <summary>
		/// Get/Set the time at which the word begins.
		/// </summary>
		public TimeSpan Begin
		{
			get { return mBegin; }
			set { mBegin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	End																																		*
		//*-----------------------------------------------------------------------*
		private TimeSpan mEnd = TimeSpan.Zero;
		/// <summary>
		/// Get/Set the time at which the word ends.
		/// </summary>
		public TimeSpan End
		{
			get { return mEnd; }
			set { mEnd = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MatchingItem																													*
		//*-----------------------------------------------------------------------*
		private WordTrackerItem mMatchingItem = null;
		/// <summary>
		/// Get/Set a reference to a matching item for processing.
		/// </summary>
		public WordTrackerItem MatchingItem
		{
			get { return mMatchingItem; }
			set { mMatchingItem = value; }
		}
		//*-----------------------------------------------------------------------*

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


	}
	//*-------------------------------------------------------------------------*


}
