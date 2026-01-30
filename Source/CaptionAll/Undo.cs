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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CaptionBubbleEditorWF;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	UndoCollection																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of UndoItem Items.
	/// </summary>
	public class UndoCollection : ObservableCollection<UndoItem>
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
	//*	UndoItem																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual undo action.
	/// </summary>
	public class UndoItem
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
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the UndoItem Item.
		/// </summary>
		public UndoItem()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the UndoItem Item.
		/// </summary>
		/// <param name="actionType">
		/// Type of action being recorded.
		/// </param>
		public UndoItem(ActionTypeEnum actionType)
		{
			mAction = actionType;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Action																																*
		//*-----------------------------------------------------------------------*
		private ActionTypeEnum mAction = ActionTypeEnum.None;
		/// <summary>
		/// Get/Set the action on this undo.
		/// </summary>
		public ActionTypeEnum Action
		{
			get { return mAction; }
			set { mAction = value; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Caption																																*
		////*-----------------------------------------------------------------------*
		//private CaptionItem mCaption = new CaptionItem();
		///// <summary>
		///// Get/Set a reference to the caption associated with this action.
		///// </summary>
		//public CaptionItem Caption
		//{
		//	get { return mCaption; }
		//	set
		//	{
		//		if(value != null)
		//		{
		//			mCaption.EntryType = value.EntryType;
		//			mCaption.Text = value.Text;
		//			mCaption.Width = value.Width;
		//			mCaption.X = value.X;
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Index																																	*
		////*-----------------------------------------------------------------------*
		//private int mIndex = -1;
		///// <summary>
		///// Get/Set the index of the affected caption.
		///// </summary>
		//public int Index
		//{
		//	get { return mIndex; }
		//	set { mIndex = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Supports																															*
		//*-----------------------------------------------------------------------*
		private UndoSupportCollection mSupports = new UndoSupportCollection();
		/// <summary>
		/// Get a reference to the collection of undo support actions.
		/// </summary>
		public UndoSupportCollection Supports
		{
			get { return mSupports; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	UndoSupportCollection																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of UndoSupportItem Items.
	/// </summary>
	public class UndoSupportCollection : List<UndoSupportItem>
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
	//*	UndoSupportItem																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual base support style for multiple-level undo operations.
	/// </summary>
	public class UndoSupportItem
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
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the UndoSupportItem Item.
		/// </summary>
		public UndoSupportItem()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the UndoSupportItem Item.
		/// </summary>
		/// <param name="actionType">
		/// Type of action being recorded.
		/// </param>
		/// <param name="caption">
		/// Caption to inspect.
		/// </param>
		/// <param name="index">
		/// Collection index at which the original caption is located.
		/// </param>
		public UndoSupportItem(ActionTypeEnum actionType,
			CaptionItem caption = null,
			int index = -1)
		{
			mAction = actionType;
			if(caption != null)
			{
				mCaption.EntryType = caption.EntryType;
				mCaption.Text = caption.Text;
				mCaption.Width = caption.Width;
				mCaption.X = caption.X;
			}
			mIndex = index;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Action																																*
		//*-----------------------------------------------------------------------*
		private ActionTypeEnum mAction = ActionTypeEnum.None;
		/// <summary>
		/// Get/Set the action on this undo.
		/// </summary>
		public ActionTypeEnum Action
		{
			get { return mAction; }
			set { mAction = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Caption																																*
		//*-----------------------------------------------------------------------*
		private CaptionItem mCaption = new CaptionItem();
		/// <summary>
		/// Get/Set a reference to the caption associated with this action.
		/// </summary>
		public CaptionItem Caption
		{
			get { return mCaption; }
			set
			{
				if(value != null)
				{
					mCaption.EntryType = value.EntryType;
					mCaption.Text = value.Text;
					mCaption.Width = value.Width;
					mCaption.X = value.X;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Index																																	*
		//*-----------------------------------------------------------------------*
		private int mIndex = -1;
		/// <summary>
		/// Get/Set the index of the affected caption.
		/// </summary>
		public int Index
		{
			get { return mIndex; }
			set { mIndex = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}
