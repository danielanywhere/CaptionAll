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
using System.Threading.Tasks;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	FindReplaceEventArgs																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Find and replace event arguments.
	/// </summary>
	public class FindReplaceEventArgs : EventArgs
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
		//*	FindText																															*
		//*-----------------------------------------------------------------------*
		private string mFindText = "";
		/// <summary>
		/// Get/Set the text to find.
		/// </summary>
		public string FindText
		{
			get { return mFindText; }
			set { mFindText = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Found																																	*
		//*-----------------------------------------------------------------------*
		private bool mFound = false;
		/// <summary>
		/// Get/Set a value indicating whether the match was found.
		/// </summary>
		public bool Found
		{
			get { return mFound; }
			set { mFound = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ReplacementText																												*
		//*-----------------------------------------------------------------------*
		private string mReplacementText = "";
		/// <summary>
		/// Get/Set the replacement text.
		/// </summary>
		public string ReplacementText
		{
			get { return mReplacementText; }
			set { mReplacementText = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RequestType																														*
		//*-----------------------------------------------------------------------*
		private FindReplaceRequestTypeEnum mRequestType =
			FindReplaceRequestTypeEnum.None;
		/// <summary>
		/// Get/Set the find and replace request type.
		/// </summary>
		public FindReplaceRequestTypeEnum RequestType
		{
			get { return mRequestType; }
			set { mRequestType = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//* FindReplaceEventHandler																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Event handler for find and replace activities.
	/// </summary>
	/// <param name="sender">
	/// The object raising this event.
	/// </param>
	/// <param name="e">
	/// Find and replace event arguments.
	/// </param>
	public delegate void FindReplaceEventHandler(object sender,
		FindReplaceEventArgs e);
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	FindReplaceRequestTypeEnum																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of available find and replace request types.
	/// </summary>
	public enum FindReplaceRequestTypeEnum
	{
		/// <summary>
		/// No request type defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Find the first item in the file.
		/// </summary>
		FindFirst,
		/// <summary>
		/// Find the next item in the file.
		/// </summary>
		FindNext,
		/// <summary>
		/// Replace the next available item.
		/// </summary>
		ReplaceCurrent,
		/// <summary>
		/// Replace all matches.
		/// </summary>
		ReplaceAll
	}
	//*-------------------------------------------------------------------------*


}
