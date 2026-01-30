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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptionBubbleEditorWF
{
	//*-------------------------------------------------------------------------*
	//*	CaptionCollection																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of CaptionItem Items.
	/// </summary>
	public class CaptionCollection : ObservableCollection<CaptionItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* EnforceMaximumWidth																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Ensure that the total aggregate width of the collection is within the
		/// maximum allowable value.
		/// </summary>
		private void EnforceMaximumWidth()
		{
			//bool bChanged = false;
			//int count = 0;
			//double difference = 0d;
			//int index = 0;
			//CaptionItem item = null;
			//double minWidth = 32d;
			//double width = 0d;

			//if(mMaximumAllowableWidth > 0d && this.Count > 0)
			//{
			//	width = this.Sum(x => x.Width);
			//	if(width > mMaximumAllowableWidth)
			//	{
			//		//	Width needs to be reduced.
			//		difference = width - mMaximumAllowableWidth;
			//		count = this.Count;
			//		index = count - 1;
			//		item = this[index];
			//		if(item.EntryType == CaptionEntryTypeEnum.Space &&
			//			item.Width > 0d)
			//		{
			//			if(item.Width > difference)
			//			{
			//				item.Width -= difference;
			//				width -= difference;
			//				difference = 0d;
			//			}
			//			else
			//			{
			//				difference -= item.Width;
			//				width -= item.Width;
			//				item.Width = 0d;
			//			}
			//			bChanged = true;
			//			index--;
			//		}
			//		while(index > -1 && difference > 0d)
			//		{
			//			item = this[index];
			//			if(item.Width > minWidth)
			//			{
			//				if(item.Width > difference)
			//				{
			//					item.Width -= difference;
			//					width -= difference;
			//					difference = 0d;
			//				}
			//				else
			//				{
			//					difference -= item.Width;
			//					width -= item.Width;
			//					item.Width = 0d;
			//				}
			//				bChanged = true;
			//			}
			//			index--;
			//		}
			//	}
			//}
			//if(bChanged)
			//{
			//	OnAlignmentChanged();
			//}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* item_PropertyChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The value of a property has changed on a member item.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property changed event arguments.
		/// </param>
		private void item_PropertyChanged(object sender,
			PropertyChangedEventArgs e)
		{
			if(!mInitializing)
			{
				OnItemPropertyChanged(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* item_PropertyChanging																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The value of a property will be changed on a member item.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property changing event arguments.
		/// </param>
		private void item_PropertyChanging(object sender,
			PropertyChangingEventArgs e)
		{
			if(!mInitializing)
			{
				OnItemPropertyChanging(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnAlignmentChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the AlignmentChanged event when the alignment of the collection
		/// has changed.
		/// </summary>
		protected virtual void OnAlignmentChanged()
		{
			if(!mInitializing)
			{
				AlignmentChanged?.Invoke(this, new EventArgs());
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnItemPropertyChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ItemPropertyChanged event when the value of an item has
		/// changed in the list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property changed event arguments.
		/// </param>
		protected virtual void OnItemPropertyChanged(object sender,
			PropertyChangedEventArgs e)
		{
			if(!mInitializing)
			{
				mChanged = true;
				ItemPropertyChanged?.Invoke(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnItemPropertyChanging																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ItemPropertyChanging event when the value of an item will be
		/// changed in the list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property changing event arguments.
		/// </param>
		protected virtual void OnItemPropertyChanging(object sender,
			PropertyChangingEventArgs e)
		{
			if(!mInitializing)
			{
				ItemPropertyChanging?.Invoke(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//*	Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an item to the end of the list.
		/// </summary>
		/// <param name="item">
		/// Reference of the item to be added.
		/// </param>
		public new void Add(CaptionItem item)
		{
			if(item != null)
			{
				item.PropertyChanged += item_PropertyChanged;
				item.PropertyChanging += item_PropertyChanging;
				base.Add(item);
				EnforceMaximumWidth();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a range of captions to the collection.
		/// </summary>
		/// <param name="captions">
		/// Reference to an array of captions to add.
		/// </param>
		public void AddRange(CaptionItem[] captions)
		{
			if(captions?.Length > 0)
			{
				foreach(CaptionItem captionItem in captions)
				{
					this.Add(captionItem);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AlignmentChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the caption layout alignment has changed.
		/// </summary>
		public event EventHandler AlignmentChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Changed																																*
		//*-----------------------------------------------------------------------*
		private bool mChanged = false;
		/// <summary>
		/// Get/Set a value indicating whether the contents of this collection have
		/// been changed since the last time it was saved.
		/// </summary>
		public bool Changed
		{
			get { return mChanged; }
			set { mChanged = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Duration																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the total duration of the loaded captions.
		/// </summary>
		public double Duration
		{
			get
			{
				int highIndex = -1;
				double highX = 0d;
				int index = 0;
				CaptionItem item = null;
				double result = 0d;

				index = 0;
				foreach(CaptionItem captionItem in this)
				{
					if(captionItem.X > highX)
					{
						highIndex = index;
						highX = captionItem.X;
					}
					index++;
				}
				if(highIndex > -1)
				{
					item = this[highIndex];
					result = item.X + item.Width;
				}
				return result;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FindAll																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return all items matching the caller's predicate expression.
		/// </summary>
		/// <param name="match">
		/// Reference to an expression to match.
		/// </param>
		/// <returns>
		/// List of all items found matching the provided expression, if found.
		/// Otherwise, an empty list.
		/// </returns>
		public List<CaptionItem> FindAll(Predicate<CaptionItem> match)
		{
			List<CaptionItem> result = new List<CaptionItem>();

			if(match != null)
			{
				foreach(CaptionItem captionItem in this)
				{
					if(match(captionItem))
					{
						result.Add(captionItem);
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetEffectiveIndex																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the effective visible caption index of the absolute caption.
		/// </summary>
		/// <param name="captionIndex">
		/// 0-based ordinal index of the caption within the collection.
		/// </param>
		/// <returns>
		/// 1-based visible caption index.
		/// </returns>
		public int GetEffectiveIndex(int captionIndex)
		{
			int index = 0;
			int result = 0;

			foreach(CaptionItem captionItem in this)
			{
				if(captionItem.EntryType != CaptionEntryTypeEnum.Space)
				{
					result++;
				}
				if(index == captionIndex)
				{
					break;
				}
				index++;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetItemAfterX																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first item past X.
		/// </summary>
		/// <param name="x">
		/// X coordinate to test for.
		/// </param>
		/// <returns>
		/// Reference to the first item following the specified coordinate, if
		/// found. Otherwise, null.
		/// </returns>
		public CaptionItem GetItemAfterX(double x)
		{
			CaptionItem caption = this.FirstOrDefault(item => item.X >= x);

			return caption;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetItemBeforeX																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the last item before x.
		/// </summary>
		/// <param name="x">
		/// X coordinate to test for.
		/// </param>
		/// <returns>
		/// Reference to the last item before the specified coordinate, if found.
		/// Otherwise, null.
		/// </returns>
		public CaptionItem GetItemBeforeX(double x)
		{
			CaptionItem caption = this.LastOrDefault(item =>
				item.X + item.Width <= x);

			return caption;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetNext																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the next caption in the collection.
		/// </summary>
		/// <param name="caption">
		/// Reference to the caption for which the next item will be found.
		/// </param>
		/// <returns>
		/// Reference to the next sequential item in the collection, if found.
		/// Otherwise, null.
		/// </returns>
		public CaptionItem GetNext(CaptionItem caption)
		{
			int index = 0;
			CaptionItem result = null;

			if(caption != null)
			{
				index = this.IndexOf(caption);
				if(index > -1 && index + 1 < this.Count)
				{
					result = this[index + 1];
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetPrevious																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the previous caption in the collection.
		/// </summary>
		/// <param name="caption">
		/// Reference to the caption for which the previous item will be found.
		/// </param>
		/// <returns>
		/// Reference to the previous sequential item in the collection, if found.
		/// Otherwise, null.
		/// </returns>
		public CaptionItem GetPrevious(CaptionItem caption)
		{
			int index = 0;
			CaptionItem result = null;

			if(caption != null)
			{
				index = this.IndexOf(caption);
				if(index > 0)
				{
					result = this[index - 1];
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a list of items in the specified ordinal range within the
		/// collection.
		/// </summary>
		/// <param name="index">
		/// Index of the first item to return.
		/// </param>
		/// <param name="count">
		/// Count of items to return.
		/// </param>
		/// <returns>
		/// Reference to a list of items within the specified range.
		/// </returns>
		/// <remarks>
		/// This variation only allows ranges within the legitimate range of the
		/// collection.
		/// </remarks>
		public List<CaptionItem> GetRange(int index, int count)
		{
			int current = 0;
			int final = 0;
			List<CaptionItem> result = null;

			if(index > -1 && index < this.Count && index + count - 1 < this.Count)
			{
				final = index + count;
				for(current = index; current < final; current++)
				{
					result.Add(this[current]);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetItemAtX																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a reference to the item found at the specified X coordinate.
		/// </summary>
		/// <param name="x">
		/// Specific X coordinate to find.
		/// </param>
		/// <returns>
		/// Reference to the item found at the specified X coordinate, if found.
		/// Otherwise, null.
		/// </returns>
		public CaptionItem GetItemAtX(double x)
		{
			double itemX = 0d;
			CaptionItem result = null;

			foreach(CaptionItem captionItem in this)
			{
				//if(captionItem.EntryType != CaptionEntryTypeEnum.Space)
				//{
				if(x >= itemX && x <= itemX + captionItem.Width)
				{
					result = captionItem;
					break;
				}
				//}
				itemX += captionItem.Width;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetTextAtX																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the text found at the specified X coordinate.
		/// </summary>
		/// <param name="x">
		/// Specific X coordinate to find.
		/// </param>
		/// <returns>
		/// The text found at the specified X coordinate, if found. Otherwise,
		/// and empty string.
		/// </returns>
		public string GetTextAtX(double x)
		{
			double itemX = 0d;
			string result = "";

			foreach(CaptionItem captionItem in this)
			{
				if(captionItem.EntryType != CaptionEntryTypeEnum.Space)
				{
					itemX = captionItem.X;
					if(x >= itemX && x <= itemX + captionItem.Width)
					{
						result = captionItem.Text;
						break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetTotalWidth																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the total width of all items in the collection.
		/// </summary>
		/// <returns>
		/// The sum of width for all items in the collection.
		/// </returns>
		public double GetTotalWidth()
		{
			return this.Sum(x => x.Width);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Initializing																													*
		//*-----------------------------------------------------------------------*
		private bool mInitializing = false;
		/// <summary>
		/// Get/Set a value indicating whether the collection is being initialized.
		/// </summary>
		public bool Initializing
		{
			get { return mInitializing; }
			set { mInitializing = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ItemPropertyChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the property value of an item in the list has changed.
		/// </summary>
		public event PropertyChangedEventHandler ItemPropertyChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ItemPropertyChanging																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the property value of an item in the list will be changed.
		/// </summary>
		public event PropertyChangingEventHandler ItemPropertyChanging;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MaximumAllowableWidth																									*
		//*-----------------------------------------------------------------------*
		private double mMaximumAllowableWidth = 0d;
		/// <summary>
		/// Get/Set the maximum total allowable width for all items in the
		/// collection.
		/// </summary>
		public double MaximumAllowableWidth
		{
			get { return mMaximumAllowableWidth; }
			set
			{
				mMaximumAllowableWidth = value;
				EnforceMaximumWidth();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RecalculateChain																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Recalculate the positions of all captions in the list along a chain.
		/// </summary>
		public void RecalculateChain()
		{
			double x = 0;

			if(this.Count > 0)
			{
				x = this[0].X;
				foreach(CaptionItem captionItem in this)
				{
					captionItem.X = x;
					x += captionItem.Width;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Remove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the specified item from the collection.
		/// </summary>
		/// <param name="item">
		/// Reference of the item to remove.
		/// </param>
		/// <returns>
		/// Value indicating whether the specified item was removed from the
		/// collection.
		/// </returns>
		public new bool Remove(CaptionItem item)
		{
			bool result = false;

			if(item != null)
			{
				item.PropertyChanged -= item_PropertyChanged;
				item.PropertyChanging -= item_PropertyChanging;
				result = base.Remove(item);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveAll																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all items in the collection matching the specified predicate.
		/// </summary>
		/// <param name="match">
		/// Reference to the predicate to match.
		/// </param>
		/// <returns>
		/// Count of matching items removed from the collection.
		/// </returns>
		public int RemoveAll(Predicate<CaptionItem> match)
		{
			List<CaptionItem> items = null;
			int result = 0;

			if(match != null)
			{
				items = FindAll(match);
				foreach(CaptionItem captionItem in items)
				{
					//captionItem.PropertyChanged -= item_PropertyChanged;
					this.Remove(captionItem);
				}
				//result = base.RemoveAll(match);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveAt																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the item at the specified ordinal location in the collection.
		/// </summary>
		/// <param name="index">
		/// Index of the item to remove.
		/// </param>
		public new void RemoveAt(int index)
		{
			CaptionItem item = null;

			if(index > -1 && index < this.Count)
			{
				item = this[index];
				item.PropertyChanged -= item_PropertyChanged;
				item.PropertyChanging -= item_PropertyChanging;
				base.RemoveAt(index);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveRange																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the items within the specified ordinal range in the collection.
		/// </summary>
		/// <param name="index">
		/// Starting index of items to remove.
		/// </param>
		/// <param name="count">
		/// Count of items to remove.
		/// </param>
		/// <remarks>
		/// This variation only allows ranges within the legitimate range of the
		/// collection.
		/// </remarks>
		public void RemoveRange(int index, int count)
		{
			List<CaptionItem> items = null;

			if(index > -1 && index < this.Count && index + count - 1 < this.Count)
			{
				items = this.GetRange(index, count);
				foreach(CaptionItem captionItem in items)
				{
					//captionItem.PropertyChanged -= item_PropertyChanged;
					this.Remove(captionItem);
				}
				//base.RemoveRange(index, count);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Select																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Select the specified caption.
		/// </summary>
		/// <param name="caption">
		/// Caption to select.
		/// </param>
		public void Select(CaptionItem caption)
		{
			mSelectedItems.Clear();
			if(caption != null && caption.EntryType != CaptionEntryTypeEnum.Space &&
				this.Contains(caption))
			{
				mSelectedItems.Add(caption);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelectedItems																													*
		//*-----------------------------------------------------------------------*
		private List<CaptionItem> mSelectedItems = new List<CaptionItem>();
		/// <summary>
		/// Get a reference to the collection of selected items.
		/// </summary>
		public List<CaptionItem> SelectedItems
		{
			get { return mSelectedItems; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetItemX																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the X coordinate of the specified item.
		/// </summary>
		/// <param name="caption">
		/// Reference to the caption to be adjusted.
		/// </param>
		/// <param name="x">
		/// The X coordinate to apply.
		/// </param>
		/// <remarks>
		/// This version only adjusts from the beginning of the previous caption
		/// to the end of the current, minus a small buffer of 16.
		/// </remarks>
		public void SetItemX(CaptionItem caption, double x)
		{
			CaptionItem captionPrev = null;
			int index = 0;
			double delta = 0d;

			if(caption != null && caption.X != x && x > 0d)
			{
				index = this.IndexOf(caption);
				if(index > 0)
				{
					captionPrev = this[index - 1];
				}
				if(captionPrev != null)
				{
					//	Because all aspects of X are implicit of the widths of
					//	the members, X can nonly be adjusted on items that have a
					//	previous item.
					if(x > caption.X)
					{
						//	Reduce the width of the current item while increasing the
						//	width of the previous.
						delta = x - caption.X;
						if(caption.Width - delta <= 16d)
						{
							delta = caption.Width - 16d;
							if(delta < 0d)
							{
								delta = 0d;
							}
						}
						caption.Width -= delta;
						captionPrev.Width += delta;
						caption.X += delta;
					}
					else
					{

						//	Increase the width of the current item while decreasing
						//	the width of the previous.
						delta = caption.X - x;
						if(captionPrev.Width - delta <= 16d)
						{
							delta = captionPrev.Width - 16d;
							if(delta < 0d)
							{
								delta = 0d;
							}
						}
						caption.Width += delta;
						captionPrev.Width -= delta;
						caption.X -= delta;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OverrideCaptionWidth																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the widths on all captions to follow the present locations.
		/// </summary>
		public void OverrideCaptionWidth()
		{
			bool initializing = mInitializing;
			CaptionItem prevCaption = null;

			mInitializing = true;
			foreach(CaptionItem captionItem in this)
			{
				if(prevCaption != null)
				{
					prevCaption.Width = captionItem.X - prevCaption.X;
				}
				prevCaption = captionItem;
			}
			mInitializing = initializing;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	CaptionItem																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual caption instance.
	/// </summary>
	public class CaptionItem : CaptionObservableObject
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
		//* Copy																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a memberwise copy of the provided caption item.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to be copied.
		/// </param>
		/// <returns>
		/// Reference to a newly created item, if the supplied item was legitimate.
		/// Otherwise, null.
		/// </returns>
		public static CaptionItem Copy(CaptionItem item)
		{
			CaptionItem result = null;

			if(item != null)
			{
				result = new CaptionItem()
				{
					mEntryType = item.mEntryType,
					mTag = item.mTag,
					mText = item.mText,
					//mTimeIndex = item.mTimeIndex,
					mWidth = item.mWidth,
					mX = item.mX
				};
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EntryType																															*
		//*-----------------------------------------------------------------------*
		private CaptionEntryTypeEnum mEntryType = CaptionEntryTypeEnum.Normal;
		/// <summary>
		/// Get/Set the current entry type for this caption.
		/// </summary>
		public CaptionEntryTypeEnum EntryType
		{
			get { return mEntryType; }
			set
			{
				bool changing = (mEntryType != value);

				if(changing)
				{
					OnPropertyChanging();
				}
				mEntryType = value;
				if(changing)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Tag																																		*
		//*-----------------------------------------------------------------------*
		private object mTag = null;
		/// <summary>
		/// Get/Set a reference to a tracking reference value.
		/// </summary>
		public object Tag
		{
			get { return mTag; }
			set
			{
				bool changing = (mTag != value);

				if(changing)
				{
					OnPropertyChanging();
				}
				mTag = value;
				if(changing)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Text																																	*
		//*-----------------------------------------------------------------------*
		private string mText = "";
		/// <summary>
		/// Get/Set the text of this item.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set
			{
				bool changing = (mText != value);

				if(changing)
				{
					OnPropertyChanging();
				}
				mText = value;
				if(changing)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	TimeIndex																															*
		////*-----------------------------------------------------------------------*
		//private int mTimeIndex = 0;
		///// <summary>
		///// Get/Set the current time offset of this caption.
		///// </summary>
		//public int TimeIndex
		//{
		//	get { return mTimeIndex; }
		//	set
		//	{
		//		int vOriginal = mTimeIndex;

		//		mTimeIndex = value;
		//		if(mTimeIndex != vOriginal)
		//		{
		//			OnPropertyChanged();
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Width																																	*
		//*-----------------------------------------------------------------------*
		private double mWidth = 0d;
		/// <summary>
		/// Get/Set the width of this item.
		/// </summary>
		public double Width
		{
			get { return mWidth; }
			set
			{
				bool changing = (mWidth != value);

				if(changing)
				{
					OnPropertyChanging();
				}
				mWidth = value;
				if(changing)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	X																																			*
		//*-----------------------------------------------------------------------*
		private double mX = 0d;
		/// <summary>
		/// Get/Set the relative X coordinate of this item from the start of the
		/// control.
		/// </summary>
		public double X
		{
			get { return mX; }
			set
			{
				bool changing = (mX != value);

				if(changing)
				{
					OnPropertyChanging();
				}
				mX = value;
				if(changing)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
