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

using Newtonsoft.Json;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	CaptionProjectCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of CaptionProjectItem Items.
	/// </summary>
	public class CaptionProjectCollection : List<CaptionProjectItem>
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
	//*	CaptionProjectItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual caption project.
	/// </summary>
	public class CaptionProjectItem
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
		//*	CaptionFilename																												*
		//*-----------------------------------------------------------------------*
		private string mCaptionFilename = "";
		/// <summary>
		/// Get/Set the filename of the caption.
		/// </summary>
		[JsonProperty(Order = 3)]		
		public string CaptionFilename
		{
			get { return mCaptionFilename; }
			set { mCaptionFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonlyUsedWords																											*
		//*-----------------------------------------------------------------------*
		private List<string> mCommonlyUsedWords = new List<string>();
		/// <summary>
		/// Get a reference to the list of commonly-used words in this project.
		/// </summary>
		[JsonProperty(Order = 4)]
		public List<string> CommonlyUsedWords
		{
			get { return mCommonlyUsedWords; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FolderPath																														*
		//*-----------------------------------------------------------------------*
		private string mFolderPath = "";
		/// <summary>
		/// Get/Set the folder path of the project.
		/// </summary>
		[JsonProperty(Order = 1)]
		public string FolderPath
		{
			get { return mFolderPath; }
			set { mFolderPath = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MediaFilename																													*
		//*-----------------------------------------------------------------------*
		private string mMediaFilename = "";
		/// <summary>
		/// Get/Set the relative filename of the media for this project.
		/// </summary>
		[JsonProperty(Order = 2)]
		public string MediaFilename
		{
			get { return mMediaFilename; }
			set { mMediaFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Version																																*
		//*-----------------------------------------------------------------------*
		private string mVersion = "1.0";
		/// <summary>
		/// Get/Set the schema version of the loaded file.
		/// </summary>
		[JsonProperty(Order = 0)]
		public string Version
		{
			get { return mVersion; }
			set { mVersion = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
