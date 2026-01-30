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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrollPanelVirtual
{
	//*-------------------------------------------------------------------------*
	//*	IPaintableControl																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Object that can be painted during a graphics update.
	/// </summary>
	public interface IPaintableControl
	{
		//int Height { get; set; }

		event InvalidateEventHandler Invalidated;

		PointEv DrawingLocation { get; }

		SizeEv DrawingSize { get; }

		//*-----------------------------------------------------------------------*
		//* LocationChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the location of the control has changed.
		/// </summary>
		event EventHandler LocationChanged;
		//*-----------------------------------------------------------------------*

		//int MasterLeft { get; set; }

		////*-----------------------------------------------------------------------*
		////* MasterLocation																												*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// A master location that supports the full 2G signed int32 space.
		///// </summary>
		//Point MasterLocation { get; set; }
		////*-----------------------------------------------------------------------*

		//int MasterTop { get; set; }

		string Name { get; set; }

		//*-----------------------------------------------------------------------*
		//* PaintArea																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Paint the specified client area onto the provided host area.
		/// </summary>
		/// <param name="graphics">
		/// Active graphics object upon which all painting should be done.
		/// </param>
		/// <param name="clientArea">
		/// The client area to clip.
		/// </param>
		/// <param name="hostArea">
		/// The target area upon which to paint the content.
		/// </param>
		void PaintArea(Graphics graphics, Rectangle clientArea,
			Rectangle hostArea);
		//*-----------------------------------------------------------------------*

		Control Parent { get; set; }

		void RaiseMouseDoubleClick(MouseEventArgs e);

		void RaiseMouseDown(MouseEventArgs e);

		void RaiseMouseMove(MouseEventArgs e);

		void RaiseMouseUp(MouseEventArgs e);

		//*-----------------------------------------------------------------------*
		//* Resize																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the control has been resized.
		/// </summary>
		event EventHandler Resize;
		//*-----------------------------------------------------------------------*

		//int Width { get; set; }

	}
	//*-------------------------------------------------------------------------*

}
