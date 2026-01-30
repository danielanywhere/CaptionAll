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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptionBubbleEditorWF
{
	//*-------------------------------------------------------------------------*
	//*	CaptionBubbleEditorUtil																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Common functionality for the Caption Bubble Editor control.
	/// </summary>
	public class CaptionBubbleEditorUtil
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
		//* GetRoundedRect																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the rounded rectangle path configured by this shape.
		/// </summary>
		public static GraphicsPath GetRoundedRect(RectangleF area, float radius)
		{
			float diameter = radius * 2f;
			SizeF size = new SizeF(diameter, diameter);
			RectangleF arc = new RectangleF(new PointF(0f, 0f), size);
			GraphicsPath path = new GraphicsPath();

			if(area != RectangleF.Empty && radius > 0f)
			{
				//	Top left corner.
				arc = new RectangleF(area.Location, size);
				path.AddArc(arc, 180f, 90f);

				//	Top right corner.
				arc.X = area.Right - diameter;
				path.AddArc(arc, 270f, 90f);

				// Bottom right corner.
				arc.Y = area.Bottom - diameter;
				path.AddArc(arc, 0f, 90f);

				//	Bottom left corner.
				arc.X = area.Left;
				path.AddArc(arc, 90f, 90f);

				path.CloseFigure();
			}
			else
			{
				path.AddRectangle(area);
			}

			return path;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
