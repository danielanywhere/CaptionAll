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
	//*	PointEv																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Point with events.
	/// </summary>
	public class PointEv
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		protected virtual void OnLeftChanged(object sender, EventArgs e)
		{
			if(LeftChanged != null)
			{
				LeftChanged.Invoke(sender, e);
			}
			OnLocationChanged(sender, e);
		}
		protected virtual void OnLocationChanged(object sender, EventArgs e)
		{
			if(LocationChanged != null)
			{
				LocationChanged.Invoke(sender, e);
			}
		}
		protected virtual void OnTopChanged(object sender, EventArgs e)
		{
			if(TopChanged != null)
			{
				TopChanged.Invoke(sender, e);
			}
			OnLocationChanged(sender, e);
		}

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	Left																																	*
		//*-----------------------------------------------------------------------*
		private int mLeft = 0;
		/// <summary>
		/// Get/Set the left coordinate.
		/// </summary>
		public int Left
		{
			get { return mLeft; }
			set
			{
				bool bChanged = (mLeft != value);

				mLeft = value;
				if(bChanged)
				{
					OnLeftChanged(this, new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		public event EventHandler LeftChanged;

		public event EventHandler LocationChanged;

		//*-----------------------------------------------------------------------*
		//*	Top																																		*
		//*-----------------------------------------------------------------------*
		private int mTop = 0;
		/// <summary>
		/// Get/Set the top coordinate.
		/// </summary>
		public int Top
		{
			get { return mTop; }
			set
			{
				bool bChanged = (mTop != value);

				mTop = value;
				if(bChanged)
				{
					OnTopChanged(this, new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		public event EventHandler TopChanged;

		public Point ToPoint()
		{
			return new Point(mLeft, mTop);
		}

	}
	//*-------------------------------------------------------------------------*

}
