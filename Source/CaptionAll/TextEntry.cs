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

using TagLib.IFD.Tags;

using static CaptionAll.CaptionAllUtil;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	TextEntryCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of TextEntryItem Items.
	/// </summary>
	public class TextEntryCollection : List<TextEntryItem>
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
		//* ParseWords																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the individual words and non-space characters found in the
		/// caller's string and return the collection of those words.
		/// </summary>
		/// <param name="value">
		/// Original string to parse.
		/// </param>
		/// <returns>
		/// Reference to a text entry collection containing the words and non-space
		/// characters found in the source value.
		/// </returns>
		public static TextEntryCollection ParseWords(string value)
		{
			MatchCollection matches = null;
			TextEntryCollection result = new TextEntryCollection();
			string text = "";

			if(value?.Length > 0)
			{
				matches = Regex.Matches(value,
					"(?<word>[a-zA-Z0-9]+)|(?<char>[^a-zA-Z0-9\\s]+)|(?<space>\\s+)");
				foreach(Match matchItem in matches)
				{
					text = GetValue(matchItem, "word");
					if(text.Length > 0)
					{
						result.Add(new TextEntryItem()
						{
							Text = text,
							ItemType = TextEntryItemType.Word,
							Position = matchItem.Index
						});
					}
					else
					{
						text = GetValue(matchItem, "char");
						if(text.Length > 0)
						{
							result.Add(new TextEntryItem()
							{
								Text = text,
								ItemType = TextEntryItemType.Character,
								Position = matchItem.Index
							});
						}
						else
						{
							text = GetValue(matchItem, "space");
							if(text.Length > 0)
							{
								result.Add(new TextEntryItem()
								{
									Text = text,
									ItemType = TextEntryItemType.Space,
									Position = matchItem.Index
								});
							}
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RenderContent																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the portion of the source string corresponding with the set of
		/// text comparisons.
		/// </summary>
		/// <param name="source">
		/// Source string from which the text will be rendered.
		/// </param>
		/// <param name="comparisons">
		/// Reference to a list of text comparisons.
		/// </param>
		/// <returns>
		/// String content from the original source corresponding to the supplied
		/// text entries, if found. Otherwise, an empty string.
		/// </returns>
		public static string RenderContent(string source,
			List<TextEntryItem> textEntries)
		{
			StringBuilder result = new StringBuilder();

			if(source?.Length > 0 && textEntries?.Count > 0)
			{
				foreach(TextEntryItem entryItem in textEntries)
				{
					if(entryItem.Text?.Length > 0 && entryItem.Position > -1 &&
						entryItem.Position + entryItem.Text.Length - 1 < source.Length)
					{
						//	Text template can be cut from source.
						//	Paste original text in the shape of the entry.
						result.Append(
							source.Substring(entryItem.Position, entryItem.Text.Length));
					}
				}
			}
			return result.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//* TextEntryComparisonCollection																						*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of TextEntryComparisonItem Items.
	/// </summary>
	public class TextEntryComparisonCollection : List<TextEntryComparisonItem>
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
		//* PreviousTo																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a list of all items in the list previous to the first existing
		/// item in the provided list.
		/// </summary>
		/// <param name="items">
		/// Reference to the list of items to be inspected.
		/// </param>
		/// <returns>
		/// Reference to a list containing all of the items that occur in the list
		/// previous to any of the items in the caller's list, if found. Otherwise,
		/// an empty list.
		/// </returns>
		public List<TextEntryComparisonItem> PreviousTo(
			List<TextEntryComparisonItem> items)
		{
			List<TextEntryComparisonItem> result =
				new List<TextEntryComparisonItem>();

			if(items?.Count > 0)
			{
				foreach(TextEntryComparisonItem item in this)
				{
					if(!items.Exists(x => x == item))
					{
						result.Add(item);
					}
					else
					{
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
	//* TextEntryComparisonItem																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual result of a comparison of text entry items.
	/// </summary>
	public class TextEntryComparisonItem
	{
		//*-----------------------------------------------------------------------*
		//*	LeftItem																															*
		//*-----------------------------------------------------------------------*
		private TextEntryItem mLeftItem = null;
		/// <summary>
		/// Get/Set a reference to the item on the left.
		/// </summary>
		public TextEntryItem LeftItem
		{
			get { return mLeftItem; }
			set { mLeftItem = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RightItem																															*
		//*-----------------------------------------------------------------------*
		private TextEntryItem mRightItem = null;
		/// <summary>
		/// Get/Set a reference to the item on the right.
		/// </summary>
		public TextEntryItem RightItem
		{
			get { return mRightItem; }
			set { mRightItem = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


	//*-------------------------------------------------------------------------*
	//*	TextEntryItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual text entry.
	/// </summary>
	public class TextEntryItem
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
		//*	ItemType																															*
		//*-----------------------------------------------------------------------*
		private TextEntryItemType mItemType = TextEntryItemType.None;
		/// <summary>
		/// Get/Set the recognized item type for this item.
		/// </summary>
		public TextEntryItemType ItemType
		{
			get { return mItemType; }
			set { mItemType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Position																															*
		//*-----------------------------------------------------------------------*
		private int mPosition = 0;
		/// <summary>
		/// Get/Set the position at which this text is found.
		/// </summary>
		public int Position
		{
			get { return mPosition; }
			set { mPosition = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Text																																	*
		//*-----------------------------------------------------------------------*
		private string mText = "";
		/// <summary>
		/// Get/Set the text associated with this entry.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set { mText = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	VttEntry																															*
		//*-----------------------------------------------------------------------*
		private VttEntryItem mVttEntry = null;
		/// <summary>
		/// Get/Set a reference to the VTT entry to which this entry is associated.
		/// </summary>
		public VttEntryItem VttEntry
		{
			get { return mVttEntry; }
			set { mVttEntry = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
