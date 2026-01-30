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
	//*	DualIndexCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of DualIndexItem Items.
	/// </summary>
	public class DualIndexCollection : List<DualIndexItem>
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
		//* GetMaxIndex1LessThan																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a reference to the maximum dual index item where index 1 is less
		/// than the specified limit.
		/// </summary>
		/// <param name="limit">
		/// The limit under which index 1 must fall.
		/// </param>
		/// <returns>
		/// Reference to the dual index item where index 1 is less than the
		/// specified limit, if found. Otherwise, null.
		/// </returns>
		public DualIndexItem GetMaxIndex1LessThan(int limit)
		{
			int high = int.MinValue;
			List<DualIndexItem> items = this.FindAll(x => x.Index1 < limit);
			DualIndexItem result = null;

			foreach(DualIndexItem indexItem in items)
			{
				if(indexItem.Index1 > high)
				{
					result = indexItem;
					high = indexItem.Index1;
				}
			}
			return result;
		}

		public static DualIndexItem GetMaxIndex1LessThan(int limit,
			params List<DualIndexItem>[] lists)
		{
			int high = int.MinValue;
			List<DualIndexItem> items = null;
			DualIndexItem result = null;

			foreach(List<DualIndexItem> listList in lists)
			{
				items = listList.FindAll(x => x.Index1 < limit);
				foreach(DualIndexItem indexItem in items)
				{
					if(indexItem.Index1 > high)
					{
						result = indexItem;
						high = indexItem.Index1;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	DualIndexItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual dual index entry.
	/// </summary>
	public class DualIndexItem
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
		//*	Index1																																*
		//*-----------------------------------------------------------------------*
		private int mIndex1 = 0;
		/// <summary>
		/// Get/Set the value of index 1.
		/// </summary>
		public int Index1
		{
			get { return mIndex1; }
			set { mIndex1 = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Index2																																*
		//*-----------------------------------------------------------------------*
		private int mIndex2 = 0;
		/// <summary>
		/// Get/Set the value of index 2.
		/// </summary>
		public int Index2
		{
			get { return mIndex2; }
			set { mIndex2 = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
