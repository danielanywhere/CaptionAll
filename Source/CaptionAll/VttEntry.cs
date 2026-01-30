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

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	VttEntryCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of VttEntryItem Items.
	/// </summary>
	public class VttEntryCollection : List<VttEntryItem>
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


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	VttEntryItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual caption entry within a VTT file.
	/// </summary>
	public class VttEntryItem
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
		//*	GroupName																															*
		//*-----------------------------------------------------------------------*
		private string mGroupName = "";
		/// <summary>
		/// Get/Set the name of the group within which this entry is found.
		/// </summary>
		public string GroupName
		{
			get { return mGroupName; }
			set { mGroupName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Id																																		*
		//*-----------------------------------------------------------------------*
		private string mId = "";
		/// <summary>
		/// Get/Set the ID of this clip uniqe to this collection.
		/// </summary>
		public string Id
		{
			get { return mId; }
			set { mId = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RelativeIndex																													*
		//*-----------------------------------------------------------------------*
		private int mRelativeIndex = 0;
		/// <summary>
		/// Get/Set the relative index of this entry within its group.
		/// </summary>
		public int RelativeIndex
		{
			get { return mRelativeIndex; }
			set { mRelativeIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Text																																	*
		//*-----------------------------------------------------------------------*
		private string mText = "";
		/// <summary>
		/// Get/Set the text content of this entry.
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
		/// Get/Set the beginning time of this clip.
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
		/// Get/Set the ending time of this clip.
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
