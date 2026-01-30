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

namespace ScrollPanelVirtual
{
	//*-------------------------------------------------------------------------*
	//*	SizeEv																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Size with events.
	/// </summary>
	public class SizeEv
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		protected virtual void OnHeightChanged(object sender, EventArgs e)
		{
			if(HeightChanged != null)
			{
				HeightChanged.Invoke(sender, e);
			}
		}
		protected virtual void OnSizeChanged(object sender, EventArgs e)
		{
			if(SizeChanged != null)
			{
				SizeChanged.Invoke(sender, e);
			}
		}
		protected virtual void OnWidthChanged(object sender, EventArgs e)
		{
			if(WidthChanged != null)
			{
				WidthChanged.Invoke(sender, e);
			}
		}

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	Height																																*
		//*-----------------------------------------------------------------------*
		private int mHeight = 0;
		/// <summary>
		/// Get/Set the Height coordinate.
		/// </summary>
		public int Height
		{
			get { return mHeight; }
			set
			{
				bool bChanged = (mHeight != value);

				mHeight = value;
				if(bChanged)
				{
					OnHeightChanged(this, new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		public event EventHandler HeightChanged;

		public event EventHandler SizeChanged;

		public Size ToSize()
		{
			return new Size(mWidth, mHeight);
		}

		//*-----------------------------------------------------------------------*
		//*	Width																																	*
		//*-----------------------------------------------------------------------*
		private int mWidth = 0;
		/// <summary>
		/// Get/Set the Width coordinate.
		/// </summary>
		public int Width
		{
			get { return mWidth; }
			set
			{
				bool bChanged = (mWidth != value);

				mWidth = value;
				if(bChanged)
				{
					OnWidthChanged(this, new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		public event EventHandler WidthChanged;

	}
	//*-------------------------------------------------------------------------*

}
