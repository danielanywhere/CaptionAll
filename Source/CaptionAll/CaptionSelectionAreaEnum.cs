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
	//*	CaptionSelectionAreaEnum																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of selection area types.
	/// </summary>
	public enum CaptionSelectionAreaEnum
	{
		/// <summary>
		/// No caption selection area defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// The selection is completely over a single caption.
		/// </summary>
		CaptionOnly,
		/// <summary>
		/// A caption will be inserted into the area shared by two captions.
		/// </summary>
		CaptionCaptionCaption,
		/// <summary>
		/// A space will be inserted into the area shared by two captions.
		/// </summary>
		CaptionCaptionSpace,
		/// <summary>
		/// A caption will be inserted into the area shared by a caption and a
		/// space.
		/// </summary>
		CaptionSpaceCaption,
		/// <summary>
		/// A space will be inserted into the area shared by a caption and a space.
		/// </summary>
		CaptionSpaceSpace,
		/// <summary>
		/// The selection is completely over a single space or blank.
		/// </summary>
		SpaceOnly,
		/// <summary>
		/// A caption will be inserted into the area shared by a space and a
		/// caption.
		/// </summary>
		SpaceCaptionCaption,
		/// <summary>
		/// A space will be inserted into the area shared by a space and a caption.
		/// </summary>
		SpaceCaptionSpace,
		/// <summary>
		/// A caption will be inserted into the area shared by two spaces.
		/// </summary>
		SpaceSpaceCaption
	}
	//*-------------------------------------------------------------------------*

}
