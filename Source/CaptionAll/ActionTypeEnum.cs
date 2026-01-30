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
	//*	ActionTypeEnum																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of known action types.
	/// </summary>
	public enum ActionTypeEnum
	{
		/// <summary>
		/// No action type defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Insert a caption.
		/// </summary>
		InsertCaption,
		/// <summary>
		/// Merge captions.
		/// </summary>
		MergeCaptions,
		/// <summary>
		/// Caption is being edited at the UI level.
		/// </summary>
		EditCaptionUI,
		/// <summary>
		/// Change the caption's starting position.
		/// </summary>
		EditCaptionX,
		/// <summary>
		/// Change the caption's width.
		/// </summary>
		EditCaptionWidth,
		/// <summary>
		/// Change the caption's entry type.
		/// </summary>
		EditCaptionType,
		/// <summary>
		/// Change the caption's text.
		/// </summary>
		EditCaptionText,
		/// <summary>
		/// Change one or more unidentified properties.
		/// </summary>
		EditCaptionProperties,
		/// <summary>
		/// Delete the caption.
		/// </summary>
		DeleteCaption,
		/// <summary>
		/// Remove space between selected captions, either explicit or implicit.
		/// </summary>
		RemoveSpace
	}
	//*-------------------------------------------------------------------------*

}
