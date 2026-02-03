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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xml.Linq;

using CaptionBubbleEditorWF;
using DocumentFormat.OpenXml.Bibliography;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Intent;
using Newtonsoft.Json;

using static CaptionAll.CaptionAllUtil;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	frmMain																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Main form form the application.
	/// </summary>
	public partial class frmMain : Form
	{
		[DllImport("user32.dll")]
		static extern bool GetCursorPos(ref Point lpPoint);

		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private const int cGrip = 16;
		private bool mActivated = false;
		//private int mCaptionAnchorBottom = 0;
		private bool mCaptionBusy = false;
		private int mFindIndex = -1;
		private string mFindReplace = "";
		private string mFindText = "";
		private bool mGripBottom = false;
		private Point mGripLocation = Point.Empty;
		private bool mGripRight = false;
		private Size mGripSize = Size.Empty;
		private ElementHost mHost = null;
		private int mLastCaptionMouseCount = 0;
		private int mMaxLineLength = 42;
		private bool mMediaBusy = false;
		private MouseButtons mMouseButtons = MouseButtons.None;
		private MouseButtons mMouseButtonsLast = MouseButtons.None;
		private int mMouseCount = 0;
		private bool mMouseDown = false;
		private Point mMouseLocation = Point.Empty;
		private Point mMouseLocationLast = Point.Empty;
		private Point mMouseUniversal = Point.Empty;
		private Point mMouseUniversalLast = Point.Empty;
		private MediaPlayerWPFUC.MediaPlayerWPF mMediaPlayer = null;
		//private bool mPauseOnPlayheadChange = false;
		/// <summary>
		/// Value indicating whether the playhead is being altered locally.
		/// </summary>
		private bool mPlayheadBusy = false;
		private double mPlayheadLast = 0d;
		//private bool mResizeMode = false;
		private bool mRippleMode = false;
		private bool mSelectionBusy = false;
		private bool mStatusBusy = false;
		private bool mStripMouseDown = false;
		private Point mStripLocation = Point.Empty;
		private int mTickCount = 0;
		/// <summary>
		/// The direct poll timer.
		/// </summary>
		private System.Windows.Forms.Timer mTimer =
			new System.Windows.Forms.Timer();

		//*-----------------------------------------------------------------------*
		//* AdjustCaptionEnd																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Adjust the end of the last caption, if it is blank. Otherwise, add
		/// a blank caption and adjust that one.
		/// </summary>
		private void AdjustCaptionEnd()
		{
			CaptionItem captionLast = null;
			CaptionItem captionNew = null;
			//double endX = 0d;

			if(captionEditor.Captions.Count > 0)
			{
				captionLast = captionEditor.Captions.Last();
				if(captionLast.Text.Length == 0 &&
					captionLast.EntryType != CaptionEntryTypeEnum.Space)
				{
					captionLast.EntryType = CaptionEntryTypeEnum.Space;
				}
				if(captionLast.EntryType != CaptionEntryTypeEnum.Space)
				{
					//	A space item is needed at the end.
					captionNew = new CaptionItem();
					captionNew.X = captionLast.X + captionLast.Width;
					captionNew.Width = 10d;
					captionNew.EntryType = CaptionEntryTypeEnum.Space;
					captionEditor.Captions.Add(captionNew);
					captionLast = captionNew;
				}
				//	In this version, we won't limit the loaded width of the last
				//	caption.
				//if(captionLast.EntryType == CaptionEntryTypeEnum.Space)
				//{
				//	//	The last item is a space.
				//	endX = Math.Max(
				//		captionLast.X + captionLast.Width, mDuration);
				//	if(endX > captionLast.X + 0.1d)
				//	{
				//		//	Set the end of the blank.
				//		captionLast.Width = endX - captionLast.X;
				//	}
				//	else
				//	{
				//		//	Reduce the end of the blank to the minimum allowable value.
				//		captionLast.Width = 0.25d;
				//	}
				//}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captionEditor_CaptionDoubleClick																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A caption was double-clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Caption click event arguments.
		/// </param>
		private void captionEditor_CaptionDoubleClick(object sender,
			CaptionClickEventArgs e)
		{
			CaptionItem captionNext = null;
			CaptionItem captionPrev = null;
			frmCaptionProperties dialog = new frmCaptionProperties();
			int index = captionEditor.Captions.IndexOf(e.Caption);
			double maximumX = captionEditor.Duration;
			double minimumX = 0d;
			UndoItem undo = new UndoItem()
			{
				Action = ActionTypeEnum.EditCaptionProperties
			};

			if(e.Caption != null)
			{
				captionPrev = captionEditor.Captions.GetPrevious(e.Caption);
				if(captionPrev != null)
				{
					minimumX = Math.Max(minimumX, captionPrev.X + captionPrev.Width);
				}
				captionNext = captionEditor.Captions.GetNext(e.Caption);
				if(captionNext != null)
				{
					maximumX = Math.Min(maximumX, captionNext.X);
				}
			}
			dialog.Owner = this;
			dialog.MinimumX = minimumX;
			dialog.MaximumX = maximumX;
			dialog.Caption = e.Caption;
			undo.Supports.Add(new UndoSupportItem()
			{
				Action = ActionTypeEnum.EditCaptionProperties,
				Caption = e.Caption,
				Index = index
			});
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				Debug.Write("AfterDoubleClick: ");
				Debug.Write($"Playhead:{captionEditor.Playhead} vs. ");
				Debug.WriteLine(
					$"Position: {mMediaPlayer.Player.Position.TotalSeconds}");
				MergeSpace(undo);
				mUndoStack.Add(undo);
				captionEditor.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captionEditor_PlayheadChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The playhead value has changed on the caption editor.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event argument.
		/// </param>
		private void captionEditor_PlayheadChanged(object sender, EventArgs e)
		{
			CaptionItem caption =
				captionEditor.Captions.GetItemAtX(captionEditor.Playhead);

			if(caption?.EntryType == CaptionEntryTypeEnum.Normal)
			{
				mMediaPlayer.Caption = caption.Text;
			}
			else
			{
				mMediaPlayer.Caption = "";
			}

			if(!mPlayheadBusy)
			{
				//	Local playhead is accepting feedback.
				Playhead = captionEditor.Playhead;
				RefreshPlayer();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captionEditor_SelectionEndChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The SelectionEnd property of the caption editor has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void captionEditor_SelectionEndChanged(object sender, EventArgs e)
		{
			TimeSpan timeSpan = TimeSpan.MinValue;

			if(!mSelectionBusy)
			{
				mSelectionBusy = true;
				timeSpan = ToTimeSpan(ToTicks(captionEditor.SelectionEnd));
				mMediaPlayer.SelectionEndText = FormatTimeSpan(timeSpan);
				mSelectionBusy = false;
				//Debug.WriteLine($"Caption editor: Selection end changed - {captionEditor.SelectionEnd}, {timeSpan}...");
			}
			mnuEditRemoveSpace.Enabled = mnuEditMergeCaptions.Enabled =
				captionEditor.SelectionEnd - captionEditor.SelectionStart > 0.25d;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captionEditor_SelectionStartChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The SelectionStart property of the caption editor has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void captionEditor_SelectionStartChanged(object sender,
			EventArgs e)
		{
			TimeSpan timeSpan = TimeSpan.MinValue;

			if(!mSelectionBusy)
			{
				mSelectionBusy = true;
				timeSpan = ToTimeSpan(ToTicks(captionEditor.SelectionStart));
				mMediaPlayer.SelectionStartText = FormatTimeSpan(timeSpan);
				this.Invalidate();
				mSelectionBusy = false;
			}
			mnuEditRemoveSpace.Enabled = mnuEditMergeCaptions.Enabled =
				captionEditor.SelectionEnd - captionEditor.SelectionStart > 0.25d;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captionEditor_WaveformReady																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The caption editor waveform is ready.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event argument.
		/// </param>
		private void captionEditor_WaveformReady(object sender, EventArgs e)
		{
			//Stop();
			Status("Waveform ready...");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CaptionFromMouse																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the current caption hovered by the mouse.
		/// </summary>
		/// <returns>
		/// Caption hovered by the mouse, if found. Otherwise, null.
		/// </returns>
		private CaptionItem CaptionFromMouse()
		{
			CaptionItem result = null;
			double seconds = 0d;

			seconds =
				(double)(mMouseLocation.X + scrollTimeline.ScrollPositionH) /
				captionEditor.PixelsPerSecond;
			result = captionEditor.Captions.GetItemAtX(seconds);
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captions_CollectionChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The contents of the collection have changed in the caption editor
		/// captions collection.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Notify collection changed event arguments.
		/// </param>
		private void captions_CollectionChanged(object sender,
			System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if(captionEditor.Captions.Changed)
			{
				mnuFileSaveCaptions.Enabled = true;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captions_ItemPropertyChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// An item property within the captions collection of the caption editor
		/// has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property changed event arguments.
		/// </param>
		private void captions_ItemPropertyChanged(object sender,
			System.ComponentModel.PropertyChangedEventArgs e)
		{
			if(captionEditor.Captions.Changed)
			{
				mnuFileSaveCaptions.Enabled = true;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* captions_ItemPropertyChanging																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The item property on a caption will be changing.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property changing event arguments.
		/// </param>
		private void captions_ItemPropertyChanging(object sender,
			System.ComponentModel.PropertyChangingEventArgs e)
		{
			CaptionItem caption = null;
			int index = 0;
			UndoSupportItem support = null;
			UndoItem undo = null;

			if(!mCaptionBusy && mMouseDown)
			{
				//	Some external force is acting upon the caption.
				caption = (CaptionItem)sender;
				index = captionEditor.Captions.IndexOf(caption);
				if(mUndoStack.Count > 0 && mMouseCount == mLastCaptionMouseCount)
				{
					undo = mUndoStack.Last();
					if(undo.Action == ActionTypeEnum.EditCaptionUI)
					{
						//	Last action was UI.
						switch(e.PropertyName)
						{
							case "X":
								support = undo.Supports.FirstOrDefault(x =>
									x.Index == index &&
									x.Action == ActionTypeEnum.EditCaptionX);
								if(support == null)
								{
									undo.Supports.Add(new UndoSupportItem()
									{
										Action = ActionTypeEnum.EditCaptionX,
										Caption = caption,
										Index = index
									});
								}
								break;
							case "Width":
								support = undo.Supports.FirstOrDefault(x =>
									x.Index == index &&
									x.Action == ActionTypeEnum.EditCaptionWidth);
								if(support == null)
								{
									undo.Supports.Add(new UndoSupportItem()
									{
										Action = ActionTypeEnum.EditCaptionWidth,
										Caption = caption,
										Index = index
									});
								}
								break;
						}
					}
					else
					{
						undo = null;
					}
				}
				if(undo == null)
				{
					undo = new UndoItem()
					{
						Action = ActionTypeEnum.EditCaptionUI
					};
					switch(e.PropertyName)
					{
						case "X":
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionX,
								Caption = caption,
								Index = index
							});
							break;
						case "Width":
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionWidth,
								Caption = caption,
								Index = index
							});
							break;
					}
					mUndoStack.Add(undo);
				}
			}
			mLastCaptionMouseCount = mMouseCount;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CenterPlayhead																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Center the playhead in the current view.
		/// </summary>
		private void CenterPlayhead()
		{
			int pixels = (int)(mPlayhead * captionEditor.PixelsPerSecond);
			int position = (scrollTimeline.CanvasWidth / 2) - pixels;

			if(position > 0)
			{
				position = 0;
			}
			if(scrollTimeline.ControlWidth + position < scrollTimeline.CanvasWidth)
			{
				position = scrollTimeline.CanvasWidth - scrollTimeline.ControlWidth;
			}
			scrollTimeline.ScrollPositionH = 0 - position;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxCaptionProperties_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Caption Properties context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxCaptionProperties_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditCaptionProperties_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxCaptionText_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Caption Text context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxCaptionText_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditCaptionText_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxCaptionWidth_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Caption Width context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxCaptionWidth_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditCaptionWidth_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxDeleteCaption_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Delete Caption context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxDeleteCaption_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditDeleteCaption_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxInsertCaption_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Insert Caption context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxInsertCaption_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditInsertCaption_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxInsertSpace_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Insert Space context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxInsertSpace_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditInsertSpace_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxMergeCaptions_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Merge Captions context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxMergeCaptions_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditMergeCaptions_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxRemoveSpace_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Remove Space context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxRemoveSpace_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditRemoveSpace_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxSelectNone_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Select None context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxSelectNone_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditSelectNone_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxSelectTimeFromCaption_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Select Time From Caption context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxSelectTimeFromCaption_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditSelectTimeFromCaption_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxSnapCaptionToSelection_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Snap Caption To Selection context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxSnapCaptionToSelection_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditSnapCaptionToSelection_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ctxToggleCaption_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Toggle Caption context menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void ctxToggleCaption_Click(object sender, EventArgs e)
		{
			//	Context menu follows.
			mnuEditToggleCaptionSpace_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dialog_FindReplaceRequest																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Find and Replace dialog has issued a request for handling.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Find and replace event arguments.
		/// </param>
		private void dialog_FindReplaceRequest(object sender,
			FindReplaceEventArgs e)
		{
			switch(e.RequestType)
			{
				case FindReplaceRequestTypeEnum.FindFirst:
					mFindText = e.FindText;
					FindFirst();
					break;
				case FindReplaceRequestTypeEnum.FindNext:
					mFindText = e.FindText;
					FindAgain();
					break;
				case FindReplaceRequestTypeEnum.ReplaceAll:
					mFindText = e.FindText;
					mFindReplace = e.ReplacementText;
					ReplaceAll();
					break;
				case FindReplaceRequestTypeEnum.ReplaceCurrent:
					mFindText = e.FindText;
					mFindReplace = e.ReplacementText;
					ReplaceCurrent();
					break;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FindFirst																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Find the first instance of the specified search term.
		/// </summary>
		private void FindFirst()
		{
			bool bFound = false;
			CaptionCollection captions = captionEditor.Captions;
			string findLower = mFindText.ToLower();
			int index = 0;

			mFindIndex = -1;
			captions.SelectedItems.Clear();
			foreach(CaptionItem captionItem in captions)
			{
				if(captionItem.EntryType != CaptionEntryTypeEnum.Space &&
					captionItem.Text.ToLower().IndexOf(findLower) > -1)
				{
					bFound = true;
					mFindIndex = index;
					Status($"[{mFindText}] found at " +
						$"{captions.GetEffectiveIndex(index)}...");
					captions.SelectedItems.Add(captionItem);
					Playhead = captionItem.X + (captionItem.Width / 2d);
					CenterPlayhead();
					captionEditor.Invalidate();
					break;
				}
				index++;
			}
			if(!bFound)
			{
				Status($"[{mFindText}] not found...");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FindAgain																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Find the next instance of the same text.
		/// </summary>
		private void FindAgain()
		{
			bool bFound = false;
			CaptionItem caption = null;
			CaptionCollection captions = captionEditor.Captions;
			int count = captions.Count;
			string findLower = mFindText.ToLower();
			int index = 0;

			if(mFindText.Length > 0)
			{
				captions.SelectedItems.Clear();
				for(index = mFindIndex + 1; index < count; index++)
				{
					caption = captions[index];
					if(caption.EntryType != CaptionEntryTypeEnum.Space &&
						caption.Text.ToLower().IndexOf(findLower) > -1)
					{
						bFound = true;
						mFindIndex = index;
						Status($"[{mFindText}] found at " +
							$"{captions.GetEffectiveIndex(index)}...");
						captions.SelectedItems.Add(caption);
						Playhead = caption.X + (caption.Width / 2d);
						CenterPlayhead();
						captionEditor.Invalidate();
						break;
					}
				}
				if(!bFound)
				{
					//	The item was not found.
					if(mFindIndex > -1)
					{
						mFindIndex = -1;
						if(MessageBox.Show(this,
							"End of file reached. " +
							"Do you want to search from the beginning?", "Find Again",
							MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							FindAgain();
						}
					}
					else
					{
						MessageBox.Show(this,
							$"{mFindText} was not found...", "Find Again",
							MessageBoxButtons.OK);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCaptionAtLocation																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the caption at the specified location.
		/// </summary>
		/// <param name="x">
		/// Location to test.
		/// </param>
		/// <returns>
		/// Reference to the first caption found at the specified location, if
		/// found. Otherwise, null.
		/// </returns>
		private CaptionItem GetCaptionAtLocation(double x)
		{
			CaptionItem caption = null;

			caption = captionEditor.Captions.FirstOrDefault(item =>
				x >= item.X && x <= item.X + item.Width);
			return caption;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCaptionInRange																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first caption is the specified range.
		/// </summary>
		/// <param name="start">
		/// The starting time to compare.
		/// </param>
		/// <param name="end">
		/// The ending time to compare.
		/// </param>
		/// <returns>
		/// The first caption in the specified range.
		/// </returns>
		private CaptionItem GetCaptionInRange(double start, double end)
		{
			CaptionItem caption = null;

			foreach(CaptionItem captionItem in captionEditor.Captions)
			{
				if(RangeOverlap(captionItem.X, captionItem.X + captionItem.Width,
					start, end))
				{
					caption = captionItem;
					break;
				}
			}
			return caption;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCurrentMediaTime																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get current interpolated media play time.
		/// <see cref="https://github.com/caprica/vlcj/issues/74"/>
		/// </summary>
		/// <returns>
		/// The current interpolated play time, in milliseconds.
		/// </returns>
		private long GetCurrentMediaTime()
		{
			long currentTime = ToMilliseconds(mMediaPlayer.Player.Position);

			//if(mLastPlayTime == currentTime && mLastPlayTime != 0)
			//{
			//	currentTime += CurrentTimeMillis() - mLastPlayTimeGlobal;
			//}
			//else
			//{
			//	mLastPlayTime = currentTime;
			//	mLastPlayTimeGlobal = CurrentTimeMillis();
			//}

			return currentTime;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetNewCaption																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Using the time/text dialog, create and return a new caption.
		/// </summary>
		/// <param name="width">
		/// Default width of the caption.
		/// </param>
		/// <param name="text">
		/// Default text of the caption.
		/// </param>
		/// <returns>
		/// Newly created caption, if successful. Otherwise, null.
		/// </returns>
		private CaptionItem GetNewCaption(double width, string text)
		{
			CaptionItem caption = null;
			frmTimeText dialog = new frmTimeText();

			dialog.Owner = this;
			dialog.Text = "New Caption";
			dialog.TimePrompt = "Caption width:";
			dialog.TimeText = FormatTimeSpan(ToTimeSpan(width));
			dialog.UserText = text;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				caption = new CaptionItem()
				{
					EntryType = CaptionEntryTypeEnum.Normal,
					Text = text,
					Width = width,
					X = 0d
				};
			}
			return caption;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetNewSpace																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Using the time dialog, create and return a new space object.
		/// </summary>
		/// <param name="width">
		/// Default width of the caption.
		/// </param>
		/// <returns>
		/// Newly created space, if successful. Otherwise, null.
		/// </returns>
		private CaptionItem GetNewSpace(double width)
		{
			CaptionItem caption = null;
			frmTime dialog = new frmTime();

			dialog.Owner = this;
			dialog.Text = "New Space";
			dialog.TimeText = FormatTimeSpan(ToTimeSpan(width));
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				caption = new CaptionItem()
				{
					EntryType = CaptionEntryTypeEnum.Space,
					Width = width,
					X = 0d
				};
			}
			return caption;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSelectedCaption																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first selected caption.
		/// </summary>
		/// <param name="allowMouse">
		/// Value indicating whether to consider the current mouse location.
		/// </param>
		/// <param name="allowPlayhead">
		/// Value indicating whether to consider the current playhead.
		/// </param>
		/// <returns>
		/// Reference to the first selected caption, by explicit level. First
		/// choice is a visible caption that has been selected in the selection
		/// list. The second choice is a caption selected by a selection area
		/// wider than 0.25 seconds. The third choice is one intersecting with
		/// the mouse X coordinate.
		/// </returns>
		private CaptionItem GetSelectedCaption(bool allowMouse = true,
			bool allowPlayhead = true)
		{
			CaptionItem caption = null;

			if(captionEditor.Captions.SelectedItems.Count > 0)
			{
				caption = captionEditor.Captions.SelectedItems[0];
			}
			else if(captionEditor.SelectionEnd - captionEditor.SelectionStart >
				0.25d)
			{
				caption = GetCaptionInRange(captionEditor.SelectionStart,
					captionEditor.SelectionEnd);
			}

			if(caption == null && allowPlayhead &&
				captionEditor.SelectionStart > 0.1d)
			{
				//	Get the caption under the current playhead.
				caption = captionEditor.Captions.GetItemAtX(
					captionEditor.SelectionStart);
			}

			if(caption == null && allowMouse)
			{
				//	Edit the caption nearest the mouse.
				caption = CaptionFromMouse();
			}
			return caption;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GoBack																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Go back 5 seconds.
		/// </summary>
		private void GoBack()
		{
			Playhead -= 5f;
			CenterPlayhead();
			RefreshPlayer();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GoForward																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Go forward 5 seconds.
		/// </summary>
		private void GoForward()
		{
			Playhead = Math.Min(Playhead + 5f, Duration);
			CenterPlayhead();
			RefreshPlayer();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GoToEnd																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Go to the end of the media.
		/// </summary>
		private void GoToEnd()
		{
			Playhead = Duration;
			CenterPlayhead();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GoToStart																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Go to the start of the media.
		/// </summary>
		private void GoToStart()
		{
			Playhead = 0f;
			CenterPlayhead();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InsertCaptionAtSplit																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a new caption by splitting an existing one at the specified
		/// coordinate.
		/// </summary>
		/// <param name="entryType">
		/// Entry type of the new caption.
		/// </param>
		/// <param name="caption">
		/// Reference to the caption to be split.
		/// </param>
		/// <param name="x">
		/// X coordinate at which to split the caption and create a new one.
		/// </param>
		/// <param name="undo">
		/// Reference to an undo item to which individual steps will be added.
		/// </param>
		/// <returns>
		/// Newly created caption, inserted to the right side of the original,
		/// if inserted. Otherwise, null.
		/// </returns>
		private CaptionItem InsertCaptionAtSplit(CaptionEntryTypeEnum entryType,
			CaptionItem caption, double x, UndoItem undo)
		{
			CaptionItem captionNew = null;
			CaptionItem captionNext = null;
			CaptionItem captionPrev = null;
			CaptionCollection captions = captionEditor.Captions;
			int index = 0;
			double r = 0d;
			double w = 0d;

			if(caption == null)
			{
				//	Insert a new caption at the specified location.
				captionNext = captions.FirstOrDefault(item => item.X >= x);
				captionPrev = captions.LastOrDefault(item => item.X + item.Width <= x);

				if(captionPrev != null && captionNext != null)
				{
					//	Insert the new item into an existing space.
					index = captions.IndexOf(captionNext);
					captionNew = new CaptionItem()
					{
						EntryType = entryType,
						Text =
							(entryType != CaptionEntryTypeEnum.Space ? "New caption" : ""),
						Width = captionNext.X - (captionPrev.X + captionPrev.Width),
						X = captionPrev.X + captionPrev.Width
					};
					captions.Insert(index, captionNew);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionNew,
						Index = index
					});
				}
				else if(captionPrev != null)
				{
					//	The item is being inserted to the right of the last item.
					if(entryType == CaptionEntryTypeEnum.Normal)
					{
						index = captions.IndexOf(captionPrev);
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Space,
							Width = x - (captionPrev.X + captionPrev.Width),
							X = (captionPrev.X + captionPrev.Width)
						};
						index++;
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
						index++;
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Width = 2d,
							X = x
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
					}
				}
				else if(captionNext != null)
				{
					//	The item is being inserted to the left of the first item.
					if(entryType == CaptionEntryTypeEnum.Normal)
					{
						index = captions.IndexOf(captionNext);
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Text = "New caption",
							Width = 2d,
							X = x
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
					}
				}
			}
			else if(x >= caption.X && x <= caption.X + caption.Width)
			{
				//	Caption was provided and split location is in range.
				index = captions.IndexOf(caption);
				r = caption.X + caption.Width;
				w = r - x;
				undo.Supports.Add(new UndoSupportItem()
				{
					Action = ActionTypeEnum.EditCaptionProperties,
					Caption = caption,
					Index = index
				});
				caption.Width -= w;
				index++;
				if(entryType == CaptionEntryTypeEnum.Space)
				{
					captionNew = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Space,
						Width = w,
						X = x
					};
				}
				else
				{
					captionNew = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Normal,
						Text = "New caption",
						Width = w,
						X = x
					};
				}
				captions.Insert(index, captionNew);
				undo.Supports.Add(new UndoSupportItem()
				{
					Action = ActionTypeEnum.InsertCaption,
					Caption = captionNew,
					Index = index
				});
			}
			else
			{
				Status("Playhead out of range. Could not insert new item...");
			}
			return captionNew;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InsertCaptionInSelectionArea																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a new caption into a selection area.
		/// </summary>
		/// <param name="entryType">
		/// Entry type of the new caption.
		/// </param>
		/// <param name="caption">
		/// Reference to the first caption already in the target area.
		/// </param>
		/// <param name="undo">
		/// Reference to an undo item to which individual steps will be added.
		/// </param>
		/// <returns>
		/// Newly created caption, inserted at the expected location within the
		/// timeline, if appropriate. Otherwise, null.
		/// </returns>
		/// <remarks>
		/// Following are outcomes from various selection patterns.
		/// <list type="bullet">
		/// <item>If the selection crosses one boundary, the area to the left of
		/// boundary will be used as the target of the new item.</item>
		/// <item>If the selection occupies the center area of the caption, the
		/// original caption is split in two and the new caption is inserted
		/// between them.</item>
		/// <item>If the selection crosses both boundaries of the reference
		/// caption, an out of range status message is sent and no action
		/// is taken.</item>
		/// </list>
		/// The following additional considerations are made.
		/// <list type="bullet">
		/// <item> When a caption is created from the current selection,
		/// the starting point of the new caption is placed at the selection
		/// start and the caption can occupy up to the end of the selection,
		/// shifting the right item to the right or shrinking it, according
		/// to the current ripple mode.</item>
		/// </list>
		/// </remarks>
		private CaptionItem InsertCaptionInSelectionArea(
			CaptionEntryTypeEnum entryType, CaptionItem caption, UndoItem undo)
		{
			bool bCaptionFollows = false;
			CaptionItem captionDupe = null;
			CaptionItem captionNew = null;
			CaptionItem captionNext = null;
			CaptionItem captionPrev = null;
			CaptionCollection captions = captionEditor.Captions;
			int index = 0;
			double maximumX = 0d;
			double minimumX = 0d;
			double right = 0d;
			double selectionEnd = 0d;
			double selectionStart = 0d;
			CaptionSelectionAreaEnum selectionType = CaptionSelectionAreaEnum.None;
			double width = 0d;

			selectionEnd = captionEditor.SelectionEnd;
			selectionStart = captionEditor.SelectionStart;
			if(caption != null)
			{
				captionPrev = captions.GetPrevious(caption);
				captionNext = captions.GetNext(caption);
				minimumX = caption.X;
				if(captionNext != null)
				{
					maximumX = captionNext.X + captionNext.Width;
				}
				else
				{
					maximumX = captionEditor.Duration;
				}
			}
			else
			{
				index = 0;
				captionPrev = captions.GetItemBeforeX(selectionStart);
				captionNext = captions.GetItemAfterX(selectionEnd);
				if(captionPrev != null)
				{
					minimumX = captionPrev.X + captionPrev.Width;
				}
				else
				{
					minimumX = 0d;
				}
				if(captionNext != null)
				{
					maximumX = captionNext.X;
					index = captions.IndexOf(captionNext);
				}
				else
				{
					maximumX = captionEditor.Duration;
				}

				if(captionPrev == null)
				{
					//	No previous item exists.
					captionPrev = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Space,
						Width = selectionStart,
						X = 0d
					};
					captions.Insert(0, captionPrev);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionPrev,
						Index = 0
					});
				}

				if(captionPrev != null)
				{
					//	A previous entry exists.
					index = captions.IndexOf(captionPrev) + 1;
					if(captionPrev.EntryType == CaptionEntryTypeEnum.Normal)
					{
						//	The previous item is a caption.
						//	New working caption is a space to the next caption.
						right = captionPrev.X + captionPrev.Width;
						if(captionNext != null)
						{
							if(captionNext.EntryType != CaptionEntryTypeEnum.Space)
							{
								//	Next item is a caption.
								caption = new CaptionItem()
								{
									EntryType = CaptionEntryTypeEnum.Space,
									Width = captionNext.X - right,
									X = right
								};
								captions.Insert(index, caption);
								undo.Supports.Add(new UndoSupportItem()
								{
									Action = ActionTypeEnum.InsertCaption,
									Caption = caption,
									Index = index
								});
							}
							else
							{
								//	Next item is a space and the current caption is
								//	a space. Reposition the next item x to the
								//	right side of the previous item.
								index = captions.IndexOf(captionNext);
								undo.Supports.Add(new UndoSupportItem()
								{
									Action = ActionTypeEnum.EditCaptionProperties,
									Caption = captionNext,
									Index = index
								});
								captionNext.Width += captionNext.X - right;
								captionNext.X = right;
								caption = captionNext;
							}
						}
						else
						{
							//	The previous caption was the last one on the list.
							//	Set the current caption in a space that extends from
							//	the last item to the right side of the selection.
							index = captions.Count;
							caption = new CaptionItem()
							{
								EntryType = CaptionEntryTypeEnum.Space,
								Width = selectionStart - right,
								X = right
							};
							captions.Add(caption);
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.InsertCaption,
								Caption = caption,
								Index = index
							});
						}
					}
					else
					{
						//	The previous item is a space.
						//	New working caption is an extension of that caption.
						if(captionNext != null)
						{
							//	Another caption exists to the right.
							//	This caption will be a space from the right side of the
							//	previous caption to the left side of the next.
							caption = new CaptionItem()
							{
								EntryType = CaptionEntryTypeEnum.Space,
								Width = captionNext.X - right,
								X = right
							};
							captions.Insert(index, caption);
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.InsertCaption,
								Caption = caption,
								Index = index
							});
						}
						else
						{
							//	No other caption to the right. Current right is selection.
							caption = new CaptionItem()
							{
								EntryType = CaptionEntryTypeEnum.Space,
								Width = selectionEnd - right,
								X = right
							};
							captions.Add(caption);
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.InsertCaption,
								Caption = caption,
								Index = index
							});
						}
					}
				}
				else if(captionNext != null)
				{
					//	There is no previous entry.
					index = captions.IndexOf(captionNext);
					caption = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Space,
						Width = captionNext.X,
						X = 0d
					};
					captions.Insert(index, caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = caption,
						Index = index
					});
				}
				else
				{
					//	The list is empty.
				}
			}

			if(caption == null)
			{
				throw new Exception(
					"InsertCaptionInSelectionArea: Caption should not be null.");
			}
			else if(caption.X > selectionStart)
			{
				throw new Exception("InsertCaptionInSelectionArea: " +
					"Selection should start to the left of caption.");
			}

			right = caption.X + caption.Width;
			if(caption.EntryType == CaptionEntryTypeEnum.Space)
			{
				//	Left item is a space.
				if(entryType == CaptionEntryTypeEnum.Normal)
				{
					//	Inserting caption in space.
					if(right > selectionEnd)
					{
						//	Selection is fully within the space.
						selectionType = CaptionSelectionAreaEnum.SpaceSpaceCaption;
					}
					else
					{
						if(captionNext?.EntryType == CaptionEntryTypeEnum.Normal)
						{
							selectionType = CaptionSelectionAreaEnum.SpaceCaptionCaption;
						}
						else
						{
							selectionType = CaptionSelectionAreaEnum.SpaceSpaceCaption;
						}
					}
				}
				else if(selectionEnd > caption.X + caption.Width &&
					captionNext?.EntryType == CaptionEntryTypeEnum.Normal)
				{
					//	Inserting space in space to the left of a caption.
					selectionType = CaptionSelectionAreaEnum.SpaceCaptionSpace;
				}
				else
				{
					//	Inserting a space in space.
					selectionType = CaptionSelectionAreaEnum.SpaceOnly;
				}
			}
			else if(selectionStart >= caption.X &&
				selectionEnd <= caption.X + caption.Width)
			{
				//	Left item is a caption and the selection is completely within it.
				selectionType = CaptionSelectionAreaEnum.CaptionOnly;
			}
			else if(entryType == CaptionEntryTypeEnum.Normal)
			{
				//	Left item is a caption and a caption will be inserted.
				if(captionNext?.EntryType == CaptionEntryTypeEnum.Normal)
				{
					//	Caption will be inserted between two other captions.
					selectionType = CaptionSelectionAreaEnum.CaptionCaptionCaption;
				}
				else
				{
					//	Space will be inserted between two captions.
					selectionType = CaptionSelectionAreaEnum.CaptionSpaceCaption;
				}
			}
			else
			{
				//	Left item is a caption and a space will be inserted.
				if(captionNext?.EntryType == CaptionEntryTypeEnum.Normal)
				{
					//	Space will be inserted between two captions.
					selectionType = CaptionSelectionAreaEnum.CaptionCaptionSpace;
				}
				else
				{
					//	Space will be inserted after a caption.
					selectionType = CaptionSelectionAreaEnum.CaptionSpaceSpace;
				}
			}
			//	Selection type has been configured.
			switch(selectionType)
			{
				case CaptionSelectionAreaEnum.CaptionCaptionCaption:
					//	Inserting a caption between two other captions.
					//	In this case:
					//	- caption is the left item.
					//	- captionNext is the right item.
					//	Adjust left caption.
					index = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth,
						Caption = caption,
						Index = index
					});
					caption.Width = selectionStart - caption.X;
					//	Adjust right caption.
					index = captions.IndexOf(captionNext);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionProperties,
						Caption = captionNext,
						Index = index
					});
					width = captionNext.Width;
					right = captionNext.X + width;
					captionNext.X = selectionEnd;
					if(!mRippleMode)
					{
						//	Only adjust next caption width when not in ripple mode.
						captionNext.Width = right - captionNext.X;
					}
					//	Insert center caption.
					captionNew = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Normal,
						Text = "New caption",
						Width = selectionEnd - selectionStart,
						X = selectionStart
					};
					captions.Insert(index, captionNew);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionNew,
						Index = index
					});
					if(mRippleMode)
					{
						MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
					}
					break;
				case CaptionSelectionAreaEnum.CaptionCaptionSpace:
					//	Insert a space between two captions.
					//	Adjust left caption.
					index = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth,
						Caption = caption,
						Index = index
					});
					caption.Width = selectionStart - caption.X;
					//	Adjust right caption.
					index = captions.IndexOf(captionNext);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionProperties,
						Caption = captionNext,
						Index = index
					});
					width = captionNext.Width;
					right = captionNext.X + width;
					captionNext.X = selectionEnd;
					if(!mRippleMode)
					{
						//	Only adjust next caption width when not in ripple mode.
						captionNext.Width = right - captionNext.X;
					}
					//	Insert center caption.
					captionNew = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Space,
						Width = selectionEnd - selectionStart,
						X = selectionStart
					};
					captions.Insert(index, captionNew);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionNew,
						Index = index
					});
					if(mRippleMode)
					{
						MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
					}
					break;
				case CaptionSelectionAreaEnum.CaptionOnly:
					//	Selection is fully inside of the target caption.
					//	Cut original caption into two pieces and insert new item
					//	between them.
					//	If ripple mode, move right item to the right.
					//	Original caption.
					index = captions.IndexOf(caption);
					right = caption.X + caption.Width;
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth,
						Caption = caption,
						Index = index
					});
					caption.Width = selectionStart - caption.X;
					//	New caption.
					index++;
					captionNew = new CaptionItem()
					{
						EntryType = entryType,
						Text = caption.Text,
						Width = selectionEnd - selectionStart,
						X = caption.X + caption.Width
					};
					bCaptionFollows = (captions.FirstOrDefault(x =>
						x.X >= captionNew.X + captionNew.Width) != null);
					if(captions.Count == 0 && captionNew.X > 0d)
					{
						captionPrev = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Space,
							Width = captionNew.X,
							X = 0d
						};
						captions.Insert(0, captionPrev);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionPrev,
							Index = 0
						});
						index++;
					}
					captions.Insert(index, captionNew);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionNew,
						Index = index
					});
					if(bCaptionFollows)
					{
						//	Duplicate caption.
						index++;
						captionDupe = new CaptionItem()
						{
							EntryType = caption.EntryType,
							Text = caption.Text,
							Width = right - selectionEnd,
							X = selectionEnd
						};
						captions.Insert(index, captionDupe);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionDupe,
							Index = index
						});
						if(mRippleMode)
						{
							//	Move items right in ripple mode.
							captionDupe.Width = right - selectionStart;
							MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
						}
					}
					break;
				case CaptionSelectionAreaEnum.CaptionSpaceCaption:
					//	Insert a caption between a caption and a space.
					//	Adjust left caption.
					index = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth,
						Caption = caption,
						Index = index
					});
					caption.Width = selectionStart - caption.X;
					if(captionNext != null)
					{
						//	Adjust right caption.
						index = captions.IndexOf(captionNext);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = captionNext,
							Index = index
						});
						width = captionNext.Width;
						right = captionNext.X + width;
						captionNext.X = selectionEnd;
						if(!mRippleMode)
						{
							//	Only adjust next caption width when not in ripple mode.
							captionNext.Width = right - captionNext.X;
						}
					}
					else
					{
						index++;
					}
					//	Insert center caption.
					captionNew = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Normal,
						Text = "New caption",
						Width = selectionEnd - selectionStart,
						X = selectionStart
					};
					captions.Insert(index, captionNew);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionNew,
						Index = index
					});
					if(mRippleMode)
					{
						MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
					}
					break;
				case CaptionSelectionAreaEnum.CaptionSpaceSpace:
					//	Insert a space after a caption.
					//	Adjust left caption.
					index = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth,
						Caption = caption,
						Index = index
					});
					caption.Width = selectionStart - caption.X;
					if(captionNext != null)
					{
						//	Adjust right caption.
						index = captions.IndexOf(captionNext);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = captionNext,
							Index = index
						});
						width = captionNext.Width;
						right = captionNext.X + width;
						captionNext.X = selectionEnd;
						if(!mRippleMode)
						{
							//	Only adjust next caption width when not in ripple mode.
							captionNext.Width = right - captionNext.X;
						}
						captionNew = captionNext;
					}
					else
					{
						//	No captions to the right of the current one.
						index++;
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Space,
							Width = selectionEnd - selectionStart,
							X = selectionStart
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
					}
					if(mRippleMode)
					{
						MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
					}
					break;
				case CaptionSelectionAreaEnum.SpaceCaptionCaption:
					//	Insert a caption between a space and a caption.
					//	Adjust left caption.
					index = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth,
						Caption = caption,
						Index = index
					});
					caption.Width = selectionStart - caption.X;
					//	Adjust right caption.
					index = captions.IndexOf(captionNext);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionProperties,
						Caption = captionNext,
						Index = index
					});
					width = captionNext.Width;
					right = captionNext.X + width;
					captionNext.X = selectionEnd;
					if(!mRippleMode)
					{
						//	Only adjust next caption width when not in ripple mode.
						captionNext.Width = right - captionNext.X;
					}
					//	Insert center caption.
					captionNew = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Normal,
						Text = "New caption",
						Width = selectionEnd - selectionStart,
						X = selectionStart
					};
					captions.Insert(index, captionNew);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionNew,
						Index = index
					});
					if(mRippleMode)
					{
						MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
					}
					break;
				case CaptionSelectionAreaEnum.SpaceCaptionSpace:
					//	Extend a space into the next caption.
					//	Adjust left caption.
					index = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth,
						Caption = caption,
						Index = index
					});
					caption.Width = selectionEnd - caption.X;
					//	Adjust right caption.
					index = captions.IndexOf(captionNext);
					if(mRippleMode)
					{
						MoveCaptions(index, selectionEnd - selectionStart, undo);
					}
					else
					{
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = captionNext,
							Index = index
						});
						width = captionNext.Width;
						right = captionNext.X + width;
						captionNext.X = selectionEnd;
						captionNext.Width = right - captionNext.X;
					}
					break;
				case CaptionSelectionAreaEnum.SpaceOnly:
					if(mRippleMode)
					{
						//	The user is inserting more space on a space at the current
						//	location.
						index = captions.IndexOf(caption);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionWidth,
							Caption = caption,
							Index = index
						});
						caption.Width += selectionEnd - selectionStart;
						MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
					}
					break;
				case CaptionSelectionAreaEnum.SpaceSpaceCaption:
					//	Insert a caption between two spaces.
					right = caption.X + caption.Width;
					if(right > selectionEnd)
					{
						//	Entire selection is within the space.
						//	Break the space into two separate spaces and insert the
						//	new caption between them.
						//	Adjust left item.
						index = captions.IndexOf(caption);
						right = caption.X + caption.Width;
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionWidth,
							Caption = caption,
							Index = index
						});
						caption.Width = selectionStart - caption.X;
						//	New caption.
						index++;
						captionNew = new CaptionItem()
						{
							EntryType = entryType,
							Text = "New caption",
							Width = selectionEnd - selectionStart,
							X = caption.X + caption.Width
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
						//	Duplicate caption.
						index++;
						captionDupe = new CaptionItem()
						{
							EntryType = caption.EntryType,
							Text = caption.Text,
							Width = right - selectionEnd,
							X = selectionEnd
						};
						captions.Insert(index, captionDupe);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionDupe,
							Index = index
						});
						if(mRippleMode)
						{
							//	Move items right in ripple mode.
							captionDupe.Width = right - selectionStart;
						}
					}
					else
					{
						//	There are one or two spaces to adjust.
						//	Adjust left caption.
						index = captions.IndexOf(caption);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionWidth,
							Caption = caption,
							Index = index
						});
						caption.Width = selectionStart - caption.X;
						if(captionNext != null)
						{
							//	Adjust right caption.
							index = captions.IndexOf(captionNext);
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionProperties,
								Caption = captionNext,
								Index = index
							});
							width = captionNext.Width;
							right = captionNext.X + width;
							captionNext.X = selectionEnd;
							if(!mRippleMode)
							{
								//	Only adjust next caption width when not in ripple mode.
								captionNext.Width = right - captionNext.X;
							}
						}
						else
						{
							//	There is no next item.
							index++;
						}
						//	Insert center caption.
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Text = "New caption",
							Width = selectionEnd - selectionStart,
							X = selectionStart
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
					}
					if(mRippleMode)
					{
						MoveCaptions(index + 1, selectionEnd - selectionStart, undo);
					}
					break;
				default:
					Status("Can't insert here. Selection is out of range...");
					break;
			}
			return captionNew;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InsertCaptionOnSelectedItem																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a new caption item to the left of the specified selected
		/// caption.
		/// </summary>
		/// <param name="entryType">
		/// Entry type of the new caption.
		/// </param>
		/// <param name="caption">
		/// Reference to the selected caption to work with.
		/// </param>
		/// <param name="undo">
		/// Reference to an undo item to which individual steps will be added.
		/// </param>
		/// <returns>
		/// Reference to the newly created caption, if successful. Otherwise, null.
		/// </returns>
		/// <remarks>
		/// In this version, a space is inserted to the right of the selected item,
		/// using some of the width of the existing item as the new area.
		/// Other insertions occur to the left of the item.
		/// </remarks>
		private CaptionItem InsertCaptionOnSelectedItem(
			CaptionEntryTypeEnum entryType, CaptionItem caption, UndoItem undo)
		{
			double adjustment = 0.5d;
			CaptionItem captionNew = null;
			CaptionItem captionNext = null;
			CaptionItem captionPrev = null;
			CaptionCollection captions = captionEditor.Captions;
			int index = 0;
			double right = 0d;
			CaptionSelectionAreaEnum selectionStyle = CaptionSelectionAreaEnum.None;
			double w = 0d;

			if(caption != null)
			{
				if(caption.EntryType == CaptionEntryTypeEnum.Normal &&
					entryType == CaptionEntryTypeEnum.Space)
				{
					//	Insert space behind normal item.
					index = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionProperties,
						Caption = caption,
						Index = index
					});
					if(caption.Width < 0.75d)
					{
						//	Reduce the adjustment if not enough room available.
						adjustment = caption.Width / 2d;
					}
					captionNew = new CaptionItem()
					{
						EntryType = CaptionEntryTypeEnum.Space,
						Width = adjustment,
						X = caption.X + caption.Width - adjustment
					};
					caption.Width -= adjustment;
					captions.Insert(index + 1, captionNew);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.InsertCaption,
						Caption = captionNew,
						Index = index + 1
					});
				}
				else
				{
					//	Normal insert.
					captionPrev = captions.GetPrevious(caption);
					if(captionPrev == null ||
						captionPrev.EntryType == CaptionEntryTypeEnum.Space)
					{
						//	Space-style previous caption.
						if(caption.EntryType == CaptionEntryTypeEnum.Space)
						{
							//	Space-style current caption.
							if(entryType == CaptionEntryTypeEnum.Space)
							{
								//	Space-style new caption.
								selectionStyle = CaptionSelectionAreaEnum.SpaceOnly;
							}
							else
							{
								//	Normal-style new caption.
								selectionStyle = CaptionSelectionAreaEnum.SpaceSpaceCaption;
							}
						}
						else
						{
							//	Normal-style current caption.
							if(entryType == CaptionEntryTypeEnum.Space)
							{
								//	Space-style new caption.
								selectionStyle = CaptionSelectionAreaEnum.SpaceCaptionSpace;
							}
							else
							{
								//	Normal-style new caption.
								selectionStyle = CaptionSelectionAreaEnum.SpaceCaptionCaption;
							}
						}
					}
					else
					{
						//	Normal previous caption.
						if(caption.EntryType == CaptionEntryTypeEnum.Space)
						{
							//	Space-style current caption.
							if(entryType == CaptionEntryTypeEnum.Space)
							{
								//	Space-style new caption.
								selectionStyle = CaptionSelectionAreaEnum.CaptionSpaceSpace;
							}
							else
							{
								//	Normal-style new caption.
								selectionStyle = CaptionSelectionAreaEnum.CaptionSpaceCaption;
							}
						}
						else
						{
							//	Normal-style current caption.
							if(entryType == CaptionEntryTypeEnum.Space)
							{
								//	Space-style new caption.
								selectionStyle = CaptionSelectionAreaEnum.CaptionCaptionSpace;
							}
							else
							{
								//	Normal-style new caption.
								selectionStyle =
									CaptionSelectionAreaEnum.CaptionCaptionCaption;
							}
						}
					}

					w = (mRippleMode ?
						caption.Width : Math.Max(caption.Width / 2d, 0.25d));
					if(entryType == CaptionEntryTypeEnum.Normal)
					{
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Text = caption.Text,
							Width = w,
							X = caption.X
						};
					}
					else
					{
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Space,
							Width = w,
							X = caption.X
						};
					}

					if(captionNew != null)
					{
						right = caption.X + caption.Width;
						switch(selectionStyle)
						{
							case CaptionSelectionAreaEnum.CaptionCaptionCaption:
							case CaptionSelectionAreaEnum.CaptionCaptionSpace:
							case CaptionSelectionAreaEnum.CaptionOnly:
							case CaptionSelectionAreaEnum.SpaceCaptionCaption:
								//	Break the existing item in half. Left side becomes new
								//	caption or space.
								index = captions.IndexOf(caption);
								if(index == -1)
								{
									//	Caption was a placeholder only.
									captionPrev = captions.LastOrDefault(x =>
										x.X < captionNew.X);
									if(captionPrev != null)
									{
										if(captionPrev.EntryType == CaptionEntryTypeEnum.Space)
										{
											//	Extend the existing space since the last item.
											captionPrev.Width = captionNew.X - captionPrev.X;
											undo.Supports.Add(new UndoSupportItem()
											{
												Action = ActionTypeEnum.EditCaptionWidth,
												Caption = captionPrev,
												Index = captions.IndexOf(captionPrev)
											});
										}
										else
										{
											//	Insert a space prior to this item.
											index = captions.IndexOf(captionPrev) + 1;
											captionPrev = new CaptionItem()
											{
												EntryType = CaptionEntryTypeEnum.Space,
												Width =
													captionNew.X - (captionPrev.X + captionPrev.Width),
												X = captionPrev.X + captionPrev.Width
											};
											captions.Insert(index, captionPrev);
											undo.Supports.Add(new UndoSupportItem()
											{
												Action = ActionTypeEnum.InsertCaption,
												Caption = captionPrev,
												Index = index
											});
											index++;
										}
									}
									captionNext = captions.FirstOrDefault(x =>
										x.X > captionNew.X);
									if(captionNext != null)
									{
										//	A next item exists.
										index = captions.IndexOf(captionNext);
										if(captionNew.X + captionNew.Width > captionNext.X)
										{
											captionNew.Width =
												Math.Max(0.25d, (Math.Min(caption.Width,
													captionNext.X - captionNew.X)));
										}
									}
									else
									{
										index = captions.Count;
									}
								}
								if(index > -1)
								{
									captions.Insert(index, captionNew);
									undo.Supports.Add(new UndoSupportItem()
									{
										Action = ActionTypeEnum.InsertCaption,
										Caption = captionNew,
										Index = index
									});
								}
								if(mRippleMode)
								{
									MoveCaptions(index + 1, captionNew.Width, undo);
								}
								else
								{
									//	Keep the right-side captions in place when ripple mode is
									//	off.
									undo.Supports.Add(new UndoSupportItem()
									{
										Action = ActionTypeEnum.EditCaptionProperties,
										Caption = caption,
										Index = index + 1
									});

									if(index == -1)
									{
										caption.X = captionNew.X;
									}
									else
									{
										caption.X = captionNew.X + captionNew.Width;
									}
									caption.Width = right - caption.X;
								}
								break;
							case CaptionSelectionAreaEnum.CaptionSpaceCaption:
							case CaptionSelectionAreaEnum.CaptionSpaceSpace:
							case CaptionSelectionAreaEnum.SpaceOnly:
							case CaptionSelectionAreaEnum.SpaceSpaceCaption:
								//	No action.
								break;
							case CaptionSelectionAreaEnum.SpaceCaptionSpace:
								//	Space in previous is extended into half of caption.
								//	New caption is set to previous item.
								index = captions.IndexOf(captionPrev);
								undo.Supports.Add(new UndoSupportItem()
								{
									Action = ActionTypeEnum.EditCaptionWidth,
									Caption = captionPrev,
									Index = index
								});
								captionPrev.Width += w;
								captionNew = captionPrev;
								index = captions.IndexOf(caption);
								if(mRippleMode)
								{
									MoveCaptions(index, captionNew.Width, undo);
								}
								else
								{
									//	Keep the right-side captions in place when ripple mode is
									//	off.
									undo.Supports.Add(new UndoSupportItem()
									{
										Action = ActionTypeEnum.EditCaptionProperties,
										Caption = caption,
										Index = index
									});

									caption.X += w;
									caption.Width = right - caption.X;
								}
								break;
						}
					}
				}
			}
			return captionNew;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InsertItem																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a caption item at the selected location.
		/// </summary>
		/// <param name="entryType">
		/// Type of caption to create.
		/// </param>
		/// <returns>
		/// Reference to the newly created caption.
		/// </returns>
		/// <remarks>
		/// The following chain of preferences is followed.
		/// <list type="bullet">
		/// <item>Selection area.</item>
		/// <item>Selected item.</item>
		/// <item>Playhead position.</item>
		/// </list>
		/// </remarks>
		private CaptionItem InsertItem(CaptionEntryTypeEnum entryType)
		{
			CaptionItem captionNew = null;
			CaptionItem captionPrev = null;
			CaptionItem captionRef = null;
			CaptionCollection captions = captionEditor.Captions;
			int index = 0;
			double maximumX = 0d;
			double selectionEnd = captionEditor.SelectionEnd;
			double selectionStart = captionEditor.SelectionStart;
			UndoItem undo = null;

			//	If a new caption is added beyond the end of the last caption
			//	on the track, a space item is added to the track and the new
			//	caption is returned.
			mCaptionBusy = true;
			undo = new UndoItem()
			{
				Action = ActionTypeEnum.InsertCaption
			};
			if(selectionEnd >= selectionStart + 0.25d)
			{
				//	Selection area.
				captionRef = GetCaptionInRange(selectionStart, selectionEnd);
				//if(captionRef != null)
				//{
				////	A new caption can be created in the current selection.
				captionNew = InsertCaptionInSelectionArea(entryType, captionRef, undo);
				//}
				captionRef = captionNew;
			}
			if(captionRef == null)
			{
				//	Get the selected item.
				//	When a caption is created from a selected item, half of that
				//	selected object's width is used as the width of the new caption,
				//	the new caption is inserted to the left of the original, and
				//	the width of the original item is divided by two.
				captionRef = GetSelectedCaption(
					allowMouse: false, allowPlayhead: false);
				if(captionRef != null)
				{
					captionNew = InsertCaptionOnSelectedItem(entryType, captionRef, undo);
				}
			}
			if(captionRef == null)
			{
				//	Get the selection start.
				//	When a new caption is inserted at the playhead location,
				//	the new caption width is initialized to the right portion of the
				//	caption under the playhead, and no choice of width is given.
				//	This has the effect of splitting the current caption and inserting
				//	the new object to the right of the original.
				captionRef = GetCaptionAtLocation(selectionStart);
				if(captionRef != null)
				{
					captionNew =
						InsertCaptionAtSplit(entryType, captionRef,
						selectionStart, undo);
				}
				else
				{
					//	There is no caption at this location.
					//	Insert a space to this location and a new caption at the
					//	lower of 2 seconds or to the end of the duration.
					maximumX = Math.Min(selectionStart + 2d, captionEditor.Duration);
					captionPrev = captions.GetItemBeforeX(selectionStart);
					if(captionPrev == null)
					{
						//	No captions exist in this list.
						captionPrev = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Space,
							Width = selectionStart,
							X = 0d
						};
						captions.Insert(0, captionPrev);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionPrev,
							Index = 0
						});
					}
					if(captionPrev.EntryType == CaptionEntryTypeEnum.Space &&
						entryType == CaptionEntryTypeEnum.Space)
					{
						//	Extend the previous space over the current area.
						index = captions.IndexOf(captionPrev);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionWidth,
							Caption = captionPrev,
							Index = index
						});
						captionPrev.Width = maximumX - captionPrev.X;
						captionNew = captionPrev;
					}
					else if(captionPrev.EntryType != CaptionEntryTypeEnum.Space &&
						entryType == CaptionEntryTypeEnum.Space)
					{
						//	The new item will be a space appended to the previous item
						//	and the lower of 2 seconds past the selection start or the
						//	end the duration.
						index = captions.Count;
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Space,
							Width = maximumX - (captionPrev.X + captionPrev.Width),
							X = captionPrev.X + captionPrev.Width
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
					}
					else if(captionPrev.EntryType == CaptionEntryTypeEnum.Space &&
						entryType != CaptionEntryTypeEnum.Space)
					{
						//	Extend the previous item to the playhead and insert the
						//	caption.
						index = captions.IndexOf(captionPrev);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionWidth,
							Caption = captionPrev,
							Index = index
						});
						captionPrev.Width = selectionStart - captionPrev.X;
						index = captions.Count;
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Text = "New caption",
							Width = maximumX - selectionStart,
							X = selectionStart
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
					}
					else if(captionPrev.EntryType != CaptionEntryTypeEnum.Space &&
						entryType != CaptionEntryTypeEnum.Space)
					{
						//	Add a space to the end of the previous item and insert
						//	the caption.
						index = captions.Count;
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Space,
							Width = selectionStart - (captionPrev.X + captionPrev.Width),
							X = captionPrev.X + captionPrev.Width
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
						index++;
						captionNew = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Text = "New caption",
							Width = maximumX - selectionStart,
							X = selectionStart
						};
						captions.Insert(index, captionNew);
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.InsertCaption,
							Caption = captionNew,
							Index = index
						});
					}
				}
			}
			MergeSpace(undo);
			if(undo.Supports.Count > 0)
			{
				mUndoStack.Add(undo);
			}
			captionEditor.Invalidate();
			mCaptionBusy = false;
			return captionNew;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsOverForm																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified location is within the
		/// screen boundaries of the form.
		/// </summary>
		/// <param name="point">
		/// Location to test.
		/// </param>
		/// <returns>
		/// Value indicating whether the specified point is located within the
		/// screen boundaries of the form.
		/// </returns>
		private bool IsOverForm(Point point)
		{
			bool result = false;

			if(point.X >= this.Left && point.X <= this.Left + this.Width &&
				point.Y >= this.Top && point.Y <= this.Top + this.Height)
			{
				result = true;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* LoadCaptions																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Load the captions file.
		/// </summary>
		private void LoadCaptions()
		{
			bool bActive = false;
			CaptionItem caption = null;
			string entry = "";
			FileInfo file = null;
			string line = "";
			Match match = null;
			StreamReader reader = null;
			TimeSpan timeStart = TimeSpan.MinValue;
			TimeSpan timeStop = TimeSpan.MinValue;
			TimeSpan timeStopPrev = TimeSpan.FromSeconds(0d);

			if(mCaptionFilename?.Length > 0)
			{
				mUndoStack.Clear();
				captionEditor.Captions.Initializing = true;
				captionEditor.Captions.Clear();
				try
				{
					reader = File.OpenText(mCaptionFilename);
					while(!reader.EndOfStream)
					{
						line = reader.ReadLine().Trim();
						if(line.Length > 0)
						{
							match = Regex.Match(line, ResourceMain.rxVTTTimeStamp);
							if(match.Success)
							{
								//	Timestamp line.
								entry = GetValue(match, "timeBegin");
								_ = TimeSpan.TryParse(entry, out timeStart);
								entry = GetValue(match, "timeEnd");
								_ = TimeSpan.TryParse(entry, out timeStop);
								if(!reader.EndOfStream)
								{
									line = reader.ReadLine();
								}
								if(timeStart > timeStopPrev + TimeSpan.FromMilliseconds(250))
								{
									//	The current item starts after a delay.
									caption = new CaptionItem()
									{
										EntryType = CaptionEntryTypeEnum.Space,
										Width = (timeStart - timeStopPrev).TotalSeconds,
										X = timeStopPrev.TotalSeconds
									};
									captionEditor.Captions.Add(caption);
								}
								//	Add the current caption.
								if(timeStop > timeStart + TimeSpan.FromMilliseconds(250))
								{
									caption = new CaptionItem()
									{
										Text = line,
										Width = (timeStop - timeStart).TotalSeconds,
										X = timeStart.TotalSeconds
									};
									// , X = timeStart.TotalSeconds
									captionEditor.Captions.Add(caption);
								}
								timeStopPrev = timeStop;
								bActive = true;
							}
							else if(bActive && caption != null)
							{
								//	Text is following the timestamp or other text.
								caption.Text += $"\r\n{line}";
							}
						}
						else
						{
							bActive = false;
						}
					}
					reader.Close();
					reader.Dispose();

					//	Add a blank item to the end and adjust to loaded media.
					AdjustCaptionEnd();

					////	TEST.
					//foreach(CaptionItem captionItem in captionBubbleControl.Captions)
					//{
					//	content.AppendLine($"X:{captionItem.X:0.000}; Width:{captionItem.Width}");
					//}
					//File.WriteAllText("C:\\Temp\\Captions-Load.txt", content.ToString());

					file = new FileInfo(mCaptionFilename);
					Status($"Captions loaded: {file.Name}...");
				}
				catch { }
				captionEditor.Captions.Changed = false;
				captionEditor.Captions.Initializing = false;
				captionEditor.Captions.OverrideCaptionWidth();
				captionEditor.UpdateWidth();
				mMediaPlayer.Caption = "";
				mnuFileSaveCaptions.Enabled = false;
				mnuFileSaveCaptionsAs.Enabled = true;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* LoadMedia																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Load the media file.
		/// </summary>
		private void LoadMedia()
		{
			if(mMediaFilename?.Length > 0)
			{
				mMediaPlayer.Caption = "";
				Status($"Rendering timeline...");
				mStatusBusy = true;

				mMediaPlayer.Load(mMediaFilename);
				mMediaPlayer.Play();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* menuStrip_MouseDown																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse has been depressed on the menu strip.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void menuStrip_MouseDown(object sender, MouseEventArgs e)
		{
			mStripMouseDown = true;
			mStripLocation = e.Location;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* menuStrip_MouseMove																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse has been moved over the menu strip.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void menuStrip_MouseMove(object sender, MouseEventArgs e)
		{
			Point delta = Point.Empty;

			if(e.Button != MouseButtons.Left)
			{
				mStripMouseDown = false;
			}
			if(mStripMouseDown)
			{
				delta = new Point(e.Location.X - mStripLocation.X,
					e.Location.Y - mStripLocation.Y);
				this.Location = new Point(
					this.Location.X + delta.X,
					this.Location.Y + delta.Y);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* menuStrip_MouseUp																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse has been released over the menu strip.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void menuStrip_MouseUp(object sender, MouseEventArgs e)
		{
			mStripMouseDown = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MergeSpace																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Merge consecutive spaces in the collection.
		/// </summary>
		/// <param name="undo">
		/// Reference to an undo object where the actions will be recorded.
		/// </param>
		private void MergeSpace(UndoItem undo = null)
		{
			CaptionItem caption = null;
			CaptionItem captionLast = null;
			CaptionCollection captions = captionEditor.Captions;
			int count = captions.Count;
			int index = 0;
			double right = 0d;

			if(count > 0)
			{
				captionLast = captions[0];
				for(index = 1; index < count; index++)
				{
					caption = captions[index];
					right = caption.X + caption.Width;
					if(caption.EntryType == CaptionEntryTypeEnum.Space &&
						captionLast.EntryType == CaptionEntryTypeEnum.Space)
					{
						//	The current item can be merged into the previous.
						if(undo != null)
						{
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionWidth,
								Caption = captionLast,
								Index = index - 1
							});
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.DeleteCaption,
								Caption = caption,
								Index = index
							});
						}
						captionLast.Width += caption.Width;
						captions.RemoveAt(index);
						captionLast = caption;
						count--;    //	Decount.
						index--;    //	Deindex.
					}
					captionLast = caption;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mMediaPlayer_SelectionEndTextChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selection end text has changed on the media player.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mMediaPlayer_SelectionEndTextChanged(object sender,
			EventArgs e)
		{
			TimeSpan timeSpan = TimeSpan.MinValue;

			if(!mSelectionBusy)
			{
				mSelectionBusy = true;
				timeSpan = ToTimeSpan(mMediaPlayer.SelectionEndText);
				captionEditor.SelectionEnd = timeSpan.TotalSeconds;
				mSelectionBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mMediaPlayer_SelectionStartTextChanged																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selection start text has changed on the media player.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mMediaPlayer_SelectionStartTextChanged(object sender,
			EventArgs e)
		{
			TimeSpan timeSpan = TimeSpan.MinValue;

			if(!mSelectionBusy)
			{
				mSelectionBusy = true;
				timeSpan = ToTimeSpan(mMediaPlayer.SelectionStartText);
				captionEditor.SelectionStart = timeSpan.TotalSeconds;
				mSelectionBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditCaptionProperties_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Caption Properties menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditCaptionProperties_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			frmCaptionProperties dialog = new frmCaptionProperties();
			double minimumX = 0d;
			double maximumX = 0d;
			CaptionItem partnerNext = null;
			CaptionItem partnerPrev = null;
			double right = 0d;
			UndoItem undo = new UndoItem();
			double w = 0d;
			double x = 0d;

			mCaptionBusy = true;
			dialog.Owner = this;
			caption = GetSelectedCaption();
			if(caption != null)
			{
				x = caption.X;
				w = caption.Width;
				partnerPrev = captionEditor.Captions.GetPrevious(caption);
				if(partnerPrev != null)
				{
					minimumX = partnerPrev.X + 0.25d;
				}
				else
				{
					minimumX = 0d;
				}
				partnerNext = captionEditor.Captions.GetNext(caption);
				if(partnerNext != null)
				{
					right = partnerNext.X + partnerNext.Width;
					maximumX = right - 0.25d;
				}
				else
				{
					maximumX = captionEditor.Duration;
				}
				dialog.MinimumX = minimumX;
				dialog.MaximumX = maximumX;
				dialog.Caption = caption;
				undo.Action = ActionTypeEnum.EditCaptionProperties;
				undo.Supports.Add(new UndoSupportItem()
				{
					Action = ActionTypeEnum.EditCaptionProperties,
					Caption = caption,
					Index = captionEditor.Captions.IndexOf(caption)
				});
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					if(partnerPrev != null)
					{
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = partnerPrev,
							Index = captionEditor.Captions.IndexOf(partnerPrev)
						});
						partnerPrev.Width = caption.X - partnerPrev.X;
					}
					if(partnerNext != null)
					{
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = partnerNext,
							Index = captionEditor.Captions.IndexOf(partnerNext)
						});
						partnerNext.X = caption.X + caption.Width;
						partnerNext.Width = right - partnerNext.X;
					}
					MergeSpace(undo);
					mUndoStack.Add(undo);
					captionEditor.Invalidate();
				}
			}
			mCaptionBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditCaptionProperties_EnabledChanged																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Caption Properties menu
		/// option has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditCaptionProperties_EnabledChanged(object sender,
			EventArgs e)
		{
			//	Caption menu follows.
			ctxCaptionProperties.Enabled = mnuEditCaptionProperties.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditCaptionText_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Caption Text menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditCaptionText_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			frmText dialog = new frmText();
			UndoItem undo = null;

			mCaptionBusy = true;
			dialog.Owner = this;
			if(captionEditor.Captions.SelectedItems.Count > 0)
			{
				undo = new UndoItem();
				undo.Action = ActionTypeEnum.EditCaptionText;
				foreach(CaptionItem captionItem in captionEditor.Captions.SelectedItems)
				{
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionText,
						Caption = captionItem,
						Index = captionEditor.Captions.IndexOf(captionItem)
					});
				}
				dialog.CaptionText = string.Join("\r\n",
					captionEditor.Captions.SelectedItems.Select(x => x.Text).ToArray());
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					foreach(CaptionItem captionItem in
						captionEditor.Captions.SelectedItems)
					{
						captionItem.Text = dialog.CaptionText;
					}
					mUndoStack.Add(undo);
					captionEditor.Invalidate();
				}
			}
			else
			{
				//	Edit the caption nearest the mouse.
				caption = GetSelectedCaption();
				if(caption != null && caption.EntryType == CaptionEntryTypeEnum.Normal)
				{
					undo = new UndoItem()
					{
						Action = ActionTypeEnum.EditCaptionText
					};
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionText,
						Caption = caption,
						Index = captionEditor.Captions.IndexOf(caption)
					});
					dialog.CaptionText = caption.Text;
					if(dialog.ShowDialog() == DialogResult.OK)
					{
						caption.Text = dialog.CaptionText;
						mUndoStack.Add(undo);
						captionEditor.Invalidate();
					}
				}
				else
				{
					//	There is no caption under the mouse or within the selection.
					undo = new UndoItem()
					{
						Action = ActionTypeEnum.InsertCaption
					};
					if(captionEditor.SelectionEnd - captionEditor.SelectionStart > 0.25d)
					{
						caption = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Text = "New caption",
							Width =
								captionEditor.SelectionEnd - captionEditor.SelectionStart,
							X = captionEditor.SelectionStart
						};
						InsertCaptionInSelectionArea(CaptionEntryTypeEnum.Normal,
							caption, undo);
					}
					else
					{
						caption = new CaptionItem()
						{
							EntryType = CaptionEntryTypeEnum.Normal,
							Text = "New caption",
							Width = 1d,
							X = captionEditor.SelectionStart
						};
						InsertCaptionOnSelectedItem(CaptionEntryTypeEnum.Normal,
							caption, undo);
					}
					dialog.CaptionText = caption.Text;
					if(dialog.ShowDialog() == DialogResult.OK)
					{
						caption.Text = dialog.CaptionText;
						mUndoStack.Add(undo);
						captionEditor.Invalidate();
					}
				}
			}
			mCaptionBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditCaptionText_EnabledChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Caption Text menu option has
		/// changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditCaptionText_EnabledChanged(object sender, EventArgs e)
		{
			//	Context menu follows.
			ctxCaptionText.Enabled = mnuEditCaptionText.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditCaptionWidth_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Caption Width menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditCaptionWidth_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			int count = 0;
			frmTime dialog = new frmTime();
			int index = 0;
			CaptionItem partner = null;
			double right = 0d;
			UndoItem undo = null;
			double x = 0d;

			mCaptionBusy = true;
			dialog.Owner = this;
			caption = GetSelectedCaption();
			if(caption != null)
			{
				undo = new UndoItem()
				{
					Action = ActionTypeEnum.EditCaptionWidth
				};
				dialog.Text = "Width";
				dialog.Prompt = "Caption width:";
				dialog.TimeText = FormatTimeSpan(ToTimeSpan(caption.Width));
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					if(mRippleMode)
					{
						//	Reposition all captions to the right in ripple mode.
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = caption,
							Index = captionEditor.Captions.IndexOf(caption)
						});
						caption.Width =
							Math.Max(ToTimeSpan(dialog.TimeText).TotalSeconds, 0.1f);
						count = captionEditor.Captions.Count;
						index = captionEditor.Captions.IndexOf(caption);
						x = caption.X + caption.Width;
						if(index > -1 && index + 1 < count)
						{
							for(index++; index < count; index++)
							{
								caption = captionEditor.Captions[index];
								undo.Supports.Add(new UndoSupportItem()
								{
									Action = ActionTypeEnum.EditCaptionProperties,
									Caption = caption,
									Index = index
								});
								caption.X = x;
								x += caption.Width;
							}
						}
					}
					else
					{
						//	Only expand the width of the current caption in non-ripple
						//	mode.
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = caption,
							Index = captionEditor.Captions.IndexOf(caption)
						});
						partner = captionEditor.Captions.GetNext(caption);
						if(partner != null)
						{
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionProperties,
								Caption = partner,
								Index = captionEditor.Captions.IndexOf(partner)
							});
							right = partner.X + partner.Width;
							caption.Width = Math.Min(
								Math.Max(ToTimeSpan(dialog.TimeText).TotalSeconds, 0.1f),
								right - 0.25d);
							partner.X = caption.X + caption.Width;
							partner.Width = right - partner.X;
						}
						else
						{
							caption.Width =
								Math.Max(ToTimeSpan(dialog.TimeText).TotalSeconds, 0.1f);
						}
					}
					mUndoStack.Add(undo);
					captionEditor.Invalidate();
				}
			}
			mCaptionBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditCaptionWidth_EnabledChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Caption Width menu option has
		/// changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditCaptionWidth_EnabledChanged(object sender, EventArgs e)
		{
			//	Context menu follows.
			ctxCaptionWidth.Enabled = mnuEditCaptionWidth.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditConvertMultiSingle_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Convert Multi-Line to Single Line menu option has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditConvertMultiSingle_Click(object sender, EventArgs e)
		{
			bool bChange = false;
			CaptionItem captionItem = null;
			int captionLength = 0;
			CaptionItem captionNew = null;
			int count = 0;
			int index = 0;
			string line1 = "";
			string line2 = "";
			Match match = null;
			UndoItem undo = null;
			double width1 = 0d;
			double width2 = 0d;
			double widthTotal = 0d;
			double x1 = 0d;
			double x2 = 0d;

			if(captionEditor.Captions.Count > 0)
			{
				undo = new UndoItem();
				undo.Action = ActionTypeEnum.InsertCaption;
				count = captionEditor.Captions.Count;
				for(index = 0; index < count; index++)
				{
					captionItem = captionEditor.Captions[index];
					match = Regex.Match(captionItem.Text,
						"(?<line1>[^\r\n]*)[\r\n]+(?<line2>[^\r\n]*)");
					if(match.Success)
					{
						//	A multi-line value is present.
						//	TODO: Handle more than 2 lines per caption.
						captionLength = captionItem.Text.Length;
						widthTotal = captionItem.Width;
						line1 = GetValue(match, "line1");
						line2 = GetValue(match, "line2");
						if(captionLength > mMaxLineLength &&
							line1.Length > 0 && line2.Length > 0)
						{
							//	There are two lines. This caption needs to be split.
							width1 = ((double)line1.Length /
								(double)captionItem.Text.Length) * widthTotal;
							width2 = ((double)line2.Length /
								(double)captionItem.Text.Length) * widthTotal;
							if(width1 < 0.5d || width2 < 0.5d)
							{
								//	If either new caption are less than a half-second,
								//	use equal widths.
								width1 = width2 = widthTotal / 2d;
							}
							x1 = captionItem.X;
							x2 = x1 + width1;
							//	Start with setting the second line (edit).
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionProperties,
								Caption = captionItem,
								Index = index
							});
							captionItem.Text = line2;
							captionItem.Width = width2;
							captionItem.X = x2;
							//	Now, insert the new caption before it.
							captionNew = new CaptionItem();
							captionNew.EntryType = CaptionEntryTypeEnum.Normal;
							captionNew.Text = line1;
							captionNew.Width = width1;
							captionNew.X = x1;
							captionEditor.Captions.Insert(index, captionNew);
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.InsertCaption,
								Caption = captionNew,
								Index = index
							});
							bChange = true;
							index++;
							count++;
						}
						else
						{
							//	Remove the line feed.
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionText,
								Caption = captionItem,
								Index = index
							});
							captionItem.Text =
								Regex.Replace(captionItem.Text, @"[\r\n]+", " ").Trim();
							bChange = true;
						}
					}
				}
				if(bChange)
				{
					mUndoStack.Add(undo);
					captionEditor.Invalidate();
					Status("Multi-line captions have been converted to single line...");
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditCurrentTime_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Current Time menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditCurrentTime_Click(object sender, EventArgs e)
		{
			frmTime dialog = new frmTime();

			dialog.Owner = this;
			dialog.Text = "Current Time";
			dialog.TimeText = FormatTimeSpan(ToTimeSpan(ToTicks(Playhead)));
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				Playhead = ToTimeSpan(dialog.TimeText).TotalSeconds;
				CenterPlayhead();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditDeleteCaption_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Delete Caption menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditDeleteCaption_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			int count = 0;
			int index = 0;
			UndoItem undo = null;
			double x = 0d;

			mCaptionBusy = true;
			if(captionEditor.Captions.SelectedItems.Count > 0)
			{
				undo = new UndoItem()
				{
					Action = ActionTypeEnum.DeleteCaption
				};
				foreach(CaptionItem captionItem in
					captionEditor.Captions.SelectedItems)
				{
					if(captionEditor.Captions.Contains(captionItem))
					{
						if(mRippleMode)
						{
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.DeleteCaption,
								Caption = captionItem,
								Index = captionEditor.Captions.IndexOf(captionItem)
							});
							captionEditor.Captions.Remove(captionItem);
						}
						else
						{
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionType,
								Caption = captionItem,
								Index = captionEditor.Captions.IndexOf(captionItem)
							});
							captionItem.EntryType = CaptionEntryTypeEnum.Space;
						}
					}
				}
				captionEditor.Captions.SelectedItems.Clear();
				if(mRippleMode)
				{
					captionEditor.Captions.RecalculateChain();
				}
				mUndoStack.Add(undo);
				captionEditor.Invalidate();
			}
			else
			{
				//	Edit the caption nearest the mouse.
				caption = GetSelectedCaption();
				if(caption != null)
				{
					undo = new UndoItem()
					{
						Action = ActionTypeEnum.DeleteCaption
					};
					x = caption.X;
					index = captionEditor.Captions.IndexOf(caption);
					if(mRippleMode)
					{
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.DeleteCaption,
							Caption = caption,
							Index = captionEditor.Captions.IndexOf(caption)
						});
						captionEditor.Captions.Remove(caption);
						count = captionEditor.Captions.Count;
						for(; index < count; index++)
						{
							caption = captionEditor.Captions[index];
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionProperties,
								Caption = caption,
								Index = index
							});
							caption.X = x;
							x += caption.Width;
						}
					}
					else
					{
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionType,
							Caption = caption,
							Index = captionEditor.Captions.IndexOf(caption)
						});
						caption.EntryType = CaptionEntryTypeEnum.Space;
					}
					mUndoStack.Add(undo);
					captionEditor.Invalidate();
				}
			}
			mCaptionBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditDeleteCaption_EnabledChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Delete Caption menu option
		/// has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditDeleteCaption_EnabledChanged(object sender,
			EventArgs e)
		{
			//	Context menu follows.
			ctxDeleteCaption.Enabled = mnuEditDeleteCaption.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditExtendCaptionTails_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Extend Caption Tails menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditExtendCaptionTails_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			CaptionItem captionPrev = null;
			CaptionCollection captions = null;
			int count = 0;
			frmInput dialog = new frmInput();
			double extension = 0.5d;
			int index = 0;
			double minExt = 0.25d;
			UndoItem undo = null;

			dialog.Owner = this;
			dialog.Text = "Caption Tail Length";
			dialog.Prompt = "Enter extension, in sec.:";
			dialog.UserText = "0.5";
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				extension = ToDouble(dialog.UserText);
				if(extension > 0f)
				{
					undo = new UndoItem()
					{
						Action = ActionTypeEnum.EditCaptionWidth
					};
					captions = captionEditor.Captions;
					count = captions.Count;
					for(index = 0; index < count; index ++)
					{
						caption = captions[index];
						if(captionPrev != null)
						{
							if(captionPrev.EntryType == CaptionEntryTypeEnum.Normal &&
								caption.EntryType == CaptionEntryTypeEnum.Space)
							{
								//	The current item is a space and the last was a normal
								//	item.
								if(caption.Width > extension)
								{
									//	We have room to expand.
									undo.Supports.Add(new UndoSupportItem()
									{
										Action = ActionTypeEnum.EditCaptionWidth,
										Caption = caption,
										Index = index
									});
									captionPrev.Width += extension;
									caption.Width -= extension;
								}
								else if(extension > minExt && caption.Width > minExt)
								{
									//	Expand to the maximum available value.
									undo.Supports.Add(new UndoSupportItem()
									{
										Action = ActionTypeEnum.EditCaptionWidth,
										Caption = caption,
										Index = index
									});
									captionPrev.Width += caption.Width - minExt;
									caption.Width = minExt;
								}
							}
						}
						captionPrev = caption;
					}
					if(undo.Supports.Count > 0)
					{
						mUndoStack.Add(undo);
						captionEditor.Captions.RecalculateChain();
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditFind_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Find menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditFind_Click(object sender, EventArgs e)
		{
			frmInput dialog = new frmInput();

			dialog.Owner = this;
			dialog.Text = "Find Text";
			dialog.Prompt = "Enter text to find:";
			dialog.UserText = mFindText;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				mFindText = dialog.UserText;
				FindFirst();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditFindAgain_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Find Again menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditFindAgain_Click(object sender, EventArgs e)
		{
			FindAgain();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditFindAndReplace_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Find And Replace menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditFindAndReplace_Click(object sender, EventArgs e)
		{
			frmFindReplace dialog = new frmFindReplace();

			dialog.FindReplaceRequest += dialog_FindReplaceRequest;
			dialog.Owner = this;
			dialog.FindText = mFindText;
			dialog.ReplacementText = mFindReplace;
			dialog.Show();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditInsertCaption_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Insert Caption menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditInsertCaption_Click(object sender, EventArgs e)
		{
			InsertItem(CaptionEntryTypeEnum.Normal);

			//caption.EntryType = CaptionEntryTypeEnum.Normal;
			//caption.Text = "New caption";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditInsertCaption_EnabledChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Insert Caption menu option
		/// has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditInsertCaption_EnabledChanged(object sender,
			EventArgs e)
		{
			//	Context menu follows.
			ctxInsertCaption.Enabled = mnuEditInsertCaption.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditInsertSpace_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Insert Space menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditInsertSpace_Click(object sender, EventArgs e)
		{
			InsertItem(CaptionEntryTypeEnum.Space);

			//caption.EntryType = CaptionEntryTypeEnum.Space;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditInsertSpace_EnabledChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Insert Space menu option has
		/// changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditInsertSpace_EnabledChanged(object sender, EventArgs e)
		{
			//	Context menu follows.
			ctxInsertSpace.Enabled = mnuEditInsertSpace.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditMergeCaptions_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Merge Captions menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditMergeCaptions_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			int captionIndex = 0;
			CaptionCollection captions = captionEditor.Captions;
			int count = 0;
			int index = 0;
			CaptionItem item = null;
			double right = 0d;
			double selectionEnd = captionEditor.SelectionEnd;
			double selectionStart = captionEditor.SelectionStart;
			List<CaptionItem> sources = new List<CaptionItem>();
			List<string> texts = new List<string>();
			UndoItem undo = new UndoItem()
			{
				Action = ActionTypeEnum.MergeCaptions
			};

			foreach(CaptionItem captionItem in captions)
			{
				if(captionItem.X + captionItem.Width >= selectionStart &&
					captionItem.X < selectionEnd)
				{
					sources.Add(captionItem);
				}
			}
			count = sources.Count;
			if(count > 1)
			{
				item = sources[^1];
				right = item.X + item.Width;
				item = sources[0];
				if(item.Text.Length > 0)
				{
					texts.Add(item.Text);
				}
				for(index = 1; index < count; index++)
				{
					caption = sources[index];
					if(caption.Text.Length > 0 && !texts.Contains(caption.Text))
					{
						texts.Add(caption.Text);
					}
					captionIndex = captions.IndexOf(caption);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.DeleteCaption,
						Caption = caption,
						Index = captionIndex
					});
					captions.Remove(caption);
				}
				captionIndex = captions.IndexOf(item);
				undo.Supports.Add(new UndoSupportItem()
				{
					Action = ActionTypeEnum.EditCaptionProperties,
					Caption = item,
					Index = captionIndex
				});
				item.Width = right - item.X;
				item.Text = string.Join("\r\n", texts);
				MergeSpace(undo);
				mUndoStack.Add(undo);
				captionEditor.Invalidate();
			}

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditMergeCaptions_EnabledChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Merge Captions menu option
		/// has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditMergeCaptions_EnabledChanged(object sender,
			EventArgs e)
		{
			//	Context menu follows.
			ctxMergeCaptions.Enabled = mnuEditMergeCaptions.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditMoveContentToTime_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Move Content To Current Time menu option has
		/// been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditMoveContentToTime_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			int count = 0;
			int index = 0;
			int startIndex = 0;
			double timeDiff = 0d;
			double timeStart = 0d;

			if(captionEditor.Captions.Count > 0)
			{
				caption = captionEditor.Captions.FirstOrDefault(x =>
					x.EntryType == CaptionEntryTypeEnum.Normal);
				if(caption != null)
				{
					captionEditor.SuspendLayout();
					startIndex = captionEditor.Captions.IndexOf(caption);
					timeStart = ToSeconds(GetCurrentMediaTime());
					timeDiff = timeStart - caption.X;
					count = captionEditor.Captions.Count;
					for(index = 0; index < count; index++)
					{
						caption = captionEditor.Captions[index];
						if(index >= startIndex)
						{
							//	This is part of the content being moved.
							caption.X += timeDiff;
							if(index == 0)
							{
								//	If this is the first time, place a blank before it.
								caption = new CaptionItem()
								{
									EntryType = CaptionEntryTypeEnum.Space,
									X = 0,
									Width = timeStart
								};
								captionEditor.Captions.Insert(0, caption);
								count++;
								index++;
							}
						}
						else if(index == 0)
						{
							//	This item is being resized if pre-content.
							//	Typically, there is one blank item at the beginning.
							//	This item will have a width equal to the starting location
							//	of the first item.
							caption.X = 0d;
							caption.Width = timeStart;
						}
						else
						{
							//	Shorten the number of blanks at the beginning to one.
							captionEditor.Captions.RemoveAt(index);
							count--;
							index--;
						}
					}
					captionEditor.ResumeLayout();
					captionEditor.Invalidate();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditMoveLeftToTime_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Move Next Content Left to Time menu option has
		/// been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditMoveLeftToTime_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			List<CaptionItem> captions = null;
			int count = 0;
			int index = 0;
			int sourceIndex = 0;
			double timeDiff = 0d;
			double timeStart = 0d;
			UndoItem undo = null;

			if(captionEditor.Captions.Count > 0)
			{
				timeStart = ToSeconds(GetCurrentMediaTime());
				captions = captionEditor.Captions.FindAll(x => x.X >= timeStart);
				count = captions.Count;
				if(count > 0)
				{
					captionEditor.SuspendLayout();
					undo = new UndoItem();
					timeDiff = captions[0].X - timeStart;
					if(timeDiff > 0d)
					{
						//	The captions will be shifted left.
						sourceIndex = captionEditor.Captions.IndexOf(captions[0]);
						if(sourceIndex > 0)
						{
							//	Shrink the width of the previous item to the start time.
							caption = captionEditor.Captions[sourceIndex - 1];
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionWidth,
								Caption = caption,
								Index = sourceIndex - 1
							});
							caption.Width = timeStart - caption.X;
						}
						for(index = 0; index < count; index++, sourceIndex++)
						{
							//	Each item to be moved.
							caption = captions[index];
							undo.Supports.Add(new UndoSupportItem()
							{
								Action = ActionTypeEnum.EditCaptionX,
								Caption = caption,
								Index = sourceIndex
							});
							caption.X -= timeDiff;
						}
					}
					captionEditor.ResumeLayout();
					if(undo.Supports.Count > 0)
					{
						mUndoStack.Add(undo);
						captionEditor.Invalidate();
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditRemoveSpace_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Remove Space menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditRemoveSpace_Click(object sender, EventArgs e)
		{
			CaptionCollection captions = captionEditor.Captions;
			int count = 0;
			int indexLeft = 0;
			int indexRight = 0;
			CaptionItem itemLeft = null;
			CaptionItem itemRight = null;
			double selectionEnd = captionEditor.SelectionEnd;
			double selectionStart = captionEditor.SelectionStart;
			List<CaptionItem> sources = new List<CaptionItem>();
			List<string> texts = new List<string>();
			UndoItem undo = new UndoItem()
			{
				Action = ActionTypeEnum.RemoveSpace
			};
			double width = 0d;

			//	Get the selected captions.
			foreach(CaptionItem captionItem in captions)
			{
				if(captionItem.X + captionItem.Width >= selectionStart &&
					captionItem.X < selectionEnd)
				{
					sources.Add(captionItem);
				}
			}
			count = sources.Count;
			if(count > 1)
			{
				//	Two or more captions selected.
				for(indexLeft = 0; indexLeft < count - 1; indexLeft++)
				{
					itemLeft = sources[indexLeft];
					indexRight = indexLeft + 1;
					itemRight = sources[indexRight];
					while(indexRight < count &&
						itemRight.EntryType == CaptionEntryTypeEnum.Space)
					{
						//	Capture all sequential spaces for deletion.
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.DeleteCaption,
							Caption = itemRight,
							Index = indexRight
						});
						captions.Remove(itemRight);
						sources.RemoveAt(indexRight);
						count--;
						if(indexRight < count)
						{
							itemRight = sources[indexRight];
						}
					}
					if(indexRight < count &&
						itemRight.EntryType == CaptionEntryTypeEnum.Normal)
					{
						//	The item to the right is a normal caption.
						//	Butt this one to the left item.
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = itemLeft,
							Index = indexLeft
						});
						width = itemRight.X - itemLeft.X;
						itemLeft.Width = width;
					}
				}
				mUndoStack.Add(undo);
				captionEditor.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditRemoveSpace_EnabledChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Remove Space menu option has
		/// changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditRemoveSpace_EnabledChanged(object sender, EventArgs e)
		{
			//	Context menu follows.
			ctxRemoveSpace.Enabled = mnuEditRemoveSpace.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditRippleOnOff_CheckedChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of the Edit / Ripple On | Off menu option has
		/// changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditRippleOnOff_CheckedChanged(object sender, EventArgs e)
		{
			tbtnEditRippleOnOff.Checked = mnuEditRippleOnOff.Checked;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditRippleOnOff_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Ripple On | Off menu option has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditRippleOnOff_Click(object sender, EventArgs e)
		{
			RippleOnOff();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSelectionEndTime_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Selection End Time menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSelectionEndTime_Click(object sender, EventArgs e)
		{
			mMediaPlayer.SetFocusSelectionEnd();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSelectionStartTime_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Selection Start Time menu option has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSelectionStartTime_Click(object sender, EventArgs e)
		{
			mMediaPlayer.SetFocusSelectionStart();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSelectNone_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Select None menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSelectNone_Click(object sender, EventArgs e)
		{
			captionEditor.SelectionEnd = captionEditor.SelectionStart;
			captionEditor.Captions.SelectedItems.Clear();
			captionEditor.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSelectNone_EnabledChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Selection / Select None
		/// menu option has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSelectNone_EnabledChanged(object sender, EventArgs e)
		{
			//	Context menu follows.
			ctxSelectNone.Enabled = mnuEditSelectNone.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSelectTimeFromCaption_Click																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Select Time From Caption menu option has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSelectTimeFromCaption_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			double maximumX = 0d;
			double minimumX = 0d;

			if(captionEditor.Captions.SelectedItems.Count > 0)
			{
				minimumX = captionEditor.Captions.SelectedItems.Min(x => x.X);
				maximumX =
					captionEditor.Captions.SelectedItems.Max(x => x.X + x.Width);
				captionEditor.SelectionStart = minimumX;
				captionEditor.SelectionEnd = maximumX;
			}
			else
			{
				//	Edit the caption nearest the mouse.
				caption = GetSelectedCaption();
				if(caption != null && caption.EntryType == CaptionEntryTypeEnum.Normal)
				{
					captionEditor.SelectionStart = caption.X;
					captionEditor.SelectionEnd = caption.X + caption.Width;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSelectTimeFromCaption_EnabledChanged														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Selection / Selection Time From Caption
		/// menu option has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSelectTimeFromCaption_EnabledChanged(object sender,
			EventArgs e)
		{
			//	Context menu follows.
			ctxSelectTimeFromCaption.Enabled = mnuEditSelectTimeFromCaption.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSetTrackDuration_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Set Track Duration menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSetTrackDuration_Click(object sender, EventArgs e)
		{
			frmTime dialog = new frmTime();

			dialog.Owner = this;
			dialog.Text = "Track Duration";
			dialog.Prompt = "Set track duration:";
			dialog.TimeText = FormatTimeSpan(ToTimeSpan(captionEditor.Duration));
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				captionEditor.Duration =
					Math.Max(1d, ToTimeSpan(dialog.TimeText).TotalSeconds);
				captionEditor.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSnapCaptionToSelection_Click																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Selection / Snap Caption To Selection menu option has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSnapCaptionToSelection_Click(object sender,
			EventArgs e)
		{
			CaptionItem caption = null;
			int index = 0;
			double maximumX = 0d;
			double minimumX = 0d;
			CaptionItem partnerNext = null;
			CaptionItem partnerPrev = null;
			double right = 0d;
			double selectionEnd = 0d;
			double selectionStart = 0d;
			UndoItem undo = null;
			double x = 0d;

			mCaptionBusy = true;
			caption = GetSelectedCaption(allowMouse: false, allowPlayhead: false);
			if(caption != null)
			{
				selectionStart = captionEditor.SelectionStart;
				selectionEnd = captionEditor.SelectionEnd;
				partnerPrev = captionEditor.Captions.GetPrevious(caption);
				minimumX = (partnerPrev == null ? 0d : partnerPrev.X + 0.25d);
				partnerNext = captionEditor.Captions.GetNext(caption);
				maximumX = (partnerNext == null ?
					captionEditor.Duration :
					partnerNext.X + partnerNext.Width - 0.25d);
			}
			if(caption != null &&
				captionEditor.SelectionEnd > captionEditor.SelectionStart + 0.25d &&
				RangeOverlap(caption.X, caption.X + caption.Width,
					captionEditor.SelectionStart, captionEditor.SelectionEnd,
					minimumX, maximumX))
			{
				undo = new UndoItem()
				{
					Action = ActionTypeEnum.EditCaptionProperties
				};
				undo.Supports.Add(new UndoSupportItem()
				{
					Action = ActionTypeEnum.EditCaptionProperties,
					Caption = caption,
					Index = captionEditor.Captions.IndexOf(caption)
				});
				if(partnerPrev != null)
				{
					index = captionEditor.Captions.IndexOf(partnerPrev);
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionProperties,
						Caption = partnerPrev,
						Index = index
					});
					x = caption.X + caption.Width;
					caption.X = Math.Max(minimumX, captionEditor.SelectionStart);
					//caption.Width = x - caption.X;
					partnerPrev.Width = caption.X - partnerPrev.X;
				}
				if(partnerNext != null)
				{
					index = captionEditor.Captions.IndexOf(partnerNext);
					caption.Width = selectionEnd - selectionStart;
					if(mRippleMode)
					{
						MoveCaptions(index, caption.Width, undo);
					}
					else
					{
						right = partnerNext.X + partnerNext.Width;
						undo.Supports.Add(new UndoSupportItem()
						{
							Action = ActionTypeEnum.EditCaptionProperties,
							Caption = partnerNext,
							Index = index
						});
						partnerNext.X = caption.X + caption.Width;
						partnerNext.Width = right - partnerNext.X;
					}
				}
				else
				{
					maximumX = captionEditor.Duration;
					caption.Width = Math.Min(selectionEnd - caption.X,
						maximumX - caption.X);
				}
				mUndoStack.Add(undo);
				captionEditor.Invalidate();
				Status("Caption updated...");
			}
			else
			{
				Status("Selection was not in caption's range...");
			}
			mCaptionBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSnapCaptionToSelection_EnabledChanged													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Selection / Snap Caption To Selection
		/// menu option has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSnapCaptionToSelection_EnabledChanged(object sender,
			EventArgs e)
		{
			//	Context menu follows.
			ctxSnapCaptionToSelection.Enabled =
				mnuEditSnapCaptionToSelection.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSpeedDialList_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Speed Dial List menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSpeedDialList_Click(object sender, EventArgs e)
		{
			frmSpeedDial dialog = new frmSpeedDial();

			dialog.ShowDialog();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditToggleCaptionSpace_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Caption / Toggle Caption | Space menu option has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditToggleCaptionSpace_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			UndoItem undo = null;

			mCaptionBusy = true;
			caption = GetSelectedCaption();
			if(caption != null)
			{
				undo = new UndoItem()
				{
					Action = ActionTypeEnum.EditCaptionType
				};
				undo.Supports.Add(new UndoSupportItem()
				{
					Action = ActionTypeEnum.EditCaptionType,
					Caption = caption,
					Index = captionEditor.Captions.IndexOf(caption)
				});
				caption.EntryType =
					(caption.EntryType == CaptionEntryTypeEnum.Normal ?
					CaptionEntryTypeEnum.Space : CaptionEntryTypeEnum.Normal);
				MergeSpace(undo);
				mUndoStack.Add(undo);
				captionEditor.Invalidate();
			}
			mCaptionBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditToggleCaptionSpace_EnabledChanged															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the Edit / Caption / Toggle Caption | Space menu
		/// option has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditToggleCaptionSpace_EnabledChanged(object sender,
			EventArgs e)
		{
			//	Context menu follows.
			ctxToggleCaption.Enabled = mnuEditToggleCaptionSpace.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditUndo_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Undo menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditUndo_Click(object sender, EventArgs e)
		{
			CaptionItem caption = null;
			int count = 0;
			int index = 0;
			UndoSupportItem support = null;
			CaptionItem supportCaption = null;
			UndoItem undo = null;

			mCaptionBusy = true;
			if(mUndoStack.Count > 0)
			{
				undo = mUndoStack.Last();
				count = undo.Supports.Count;
				for(index = count - 1; index > -1; index--)
				{
					support = undo.Supports[index];
					supportCaption = support.Caption;
					if(support.Index > -1 &&
						support.Index < captionEditor.Captions.Count)
					{
						caption = captionEditor.Captions[support.Index];
					}
					else
					{
						caption = null;
					}
					switch(support.Action)
					{
						case ActionTypeEnum.DeleteCaption:
							captionEditor.Captions.Insert(support.Index, new CaptionItem()
							{
								EntryType = support.Caption.EntryType,
								Text = support.Caption.Text,
								Width = support.Caption.Width,
								X = support.Caption.X
							});
							break;
						case ActionTypeEnum.EditCaptionProperties:
						case ActionTypeEnum.EditCaptionUI:
							if(caption != null)
							{
								caption.EntryType = supportCaption.EntryType;
								caption.Text = supportCaption.Text;
								caption.Width = supportCaption.Width;
								caption.X = supportCaption.X;
							}
							break;
						case ActionTypeEnum.EditCaptionText:
							if(caption != null)
							{
								caption.Text = supportCaption.Text;
							}
							break;
						case ActionTypeEnum.EditCaptionType:
							if(caption != null)
							{
								caption.EntryType = supportCaption.EntryType;
							}
							break;
						case ActionTypeEnum.EditCaptionWidth:
							if(caption != null)
							{
								caption.Width = supportCaption.Width;
							}
							break;
						case ActionTypeEnum.EditCaptionX:
							if(caption != null)
							{
								caption.X = supportCaption.X;
							}
							break;
						case ActionTypeEnum.InsertCaption:
							if(caption != null)
							{
								captionEditor.Captions.Remove(caption);
							}
							break;
					}
				}
				mUndoStack.Remove(undo);
				captionEditor.Invalidate();
			}
			mCaptionBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuExportCaptionsSRT_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Export / Captions / As SRT menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuExportCaptionsSRT_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			FileInfo file = null;
			string newFilename = "";


			if(mProjectFilename.Length > 0)
			{
				newFilename = LeftOf(Path.GetFileName(mProjectFilename), ".");
				newFilename += "-Captions.srt";
			}

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = false;
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.DefaultExt = ".srt";
			dialog.DereferenceLinks = true;
			if(mProjectFilename.Length > 0)
			{
				file = new FileInfo(mProjectFilename);
				dialog.InitialDirectory = file.Directory.FullName;
				dialog.FileName = newFilename;
			}
			dialog.Filter =
				"SRT Caption Files " +
				"(*.srt)|" +
				"*.srt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.OverwritePrompt = true;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Export SRT Caption File As";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				SaveCaptionsSRT(dialog.FileName);
				file = new FileInfo(dialog.FileName);
				Status($"Caption file exported: {file.Name}...");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileExit_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Exit menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileExportTranscriptText_Click																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Export / Transcript Text menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileExportTranscriptText_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			FileInfo file = null;
			string newFilename = "";

			if(mProjectFilename.Length > 0)
			{
				newFilename = LeftOf(Path.GetFileName(mProjectFilename), ".");
				newFilename += "-Transcript.txt";
			}

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = false;
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.DefaultExt = ".txt";
			dialog.DereferenceLinks = true;
			if(mProjectFilename?.Length > 0)
			{
				file = new FileInfo(mProjectFilename);
				dialog.InitialDirectory = file.Directory.FullName;
				dialog.FileName = newFilename;
			}
			dialog.Filter =
				"TXT Transcript Files " +
				"(*.txt)|" +
				"*.txt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.OverwritePrompt = true;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Export Transscript File As";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				SaveTranscript(dialog.FileName);
				file = new FileInfo(dialog.FileName);
				Status($"Transcript file exported: {file.Name}...");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileLoadProject_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Load Project menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		/// <remarks>
		/// In versions 23.1228 and later, the path of the project file is used
		/// as the relative base source for other components so the project can
		/// be used collaboratively.
		/// </remarks>
		private void mnuFileLoadProject_Click(object sender, EventArgs e)
		{
			string content = "";
			OpenFileDialog dialog = new OpenFileDialog();
			FileInfo file = null;
			string path = "";

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.CheckPathExists = true;
			dialog.DefaultExt = ".captionproject.json";
			dialog.DereferenceLinks = true;
			dialog.Filter = "Project Files " +
				"(*.captionproject.json)|*.captionproject.json|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Open Project File";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				path = Path.GetDirectoryName(dialog.FileName);
				content = File.ReadAllText(dialog.FileName);
				mProjectInfo =
					JsonConvert.DeserializeObject<CaptionProjectItem>(content);
				mProjectFilename = dialog.FileName;
				if(mProjectInfo.CaptionFilename?.Length > 0)
				{
					mCaptionFilename = Path.Combine(path,
						mProjectInfo.CaptionFilename);
					LoadCaptions();
				}
				if(mProjectInfo.MediaFilename?.Length > 0)
				{
					mMediaFilename = Path.Combine(path,
						mProjectInfo.MediaFilename);
					LoadMedia();
				}
				CommonlyUsedWords.Clear();
				CommonlyUsedWords.AddRange(mProjectInfo.CommonlyUsedWords);


				file = new FileInfo(dialog.FileName);
				Status($"Project opened: {file.Name}...");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileOpenCaptions_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Open Captions menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileOpenCaptions_Click(object sender, EventArgs e)
		{
			string filename = OpenVTTDialog();

			if(filename.Length > 0)
			{
				mCaptionFilename = filename;
				LoadCaptions();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileOpenMedia_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Open Media menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileOpenMedia_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			//FileInfo file = null;
			//TimeSpan timeSpan = TimeSpan.MinValue;

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".mp4";
			dialog.DereferenceLinks = true;
			dialog.Filter = "All media files " +
				"(*.mp3;*.mp4;*.m4a;*.wav)|" +
				"*.mp3;*.mp4;*.m4a;*.wav|" +
				"Audio files (*.mp3;*.m4a)|*.mp3;*.m4a|" +
				"Video files (*.mp4)|*.mp4|" +
				"All files (*.*)|*.*";
			dialog.FilterIndex = 1;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Open a media file";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				mMediaFilename = dialog.FileName;
				LoadMedia();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileSaveCaptions_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Save Captions menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileSaveCaptions_Click(object sender, EventArgs e)
		{
			SaveCaptions();
			mUndoStack.Clear();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileSaveCaptions_EnabledChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The enabled state of the File / Save Captions menu option has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileSaveCaptions_EnabledChanged(object sender,
			EventArgs e)
		{
			//mnuFileSaveCaptionsAs.Enabled = mnuFileSaveCaptions.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileSaveCaptionsAs_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Save Captions As ... menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileSaveCaptionsAs_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			FileInfo file = null;

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = false;
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.DefaultExt = ".vtt";
			dialog.DereferenceLinks = true;
			if(mCaptionFilename?.Length > 0)
			{
				file = new FileInfo(mCaptionFilename);
				dialog.InitialDirectory = file.Directory.FullName;
				dialog.FileName = file.Name;
			}
			dialog.Filter =
				"WebVTT Caption Files " +
				"(*.vtt)|" +
				"*.vtt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.OverwritePrompt = true;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Save Captions File As";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				mCaptionFilename = dialog.FileName;
				SaveCaptions();
				mUndoStack.Clear();
				file = new FileInfo(mCaptionFilename);
				Status($"Caption file: {file.Name}...");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileSaveProject_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Save Project menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileSaveProject_Click(object sender, EventArgs e)
		{
			if(mProjectFilename.Length > 0)
			{
				SaveProject();
			}
			else
			{
				mnuFileSaveProjectAs_Click(sender, e);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileSaveProjectAs_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Save Project As menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileSaveProjectAs_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			FileInfo file = null;

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = false;
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.DefaultExt = ".captionproject.json";
			dialog.DereferenceLinks = true;
			dialog.Filter = "Project Files " +
				"(*.captionproject.json)|*.captionproject.json|All Files(*.*)|*.*";
			dialog.FilterIndex = 0;
			if(mProjectFilename?.Length > 0)
			{
				file = new FileInfo(mProjectFilename);
				dialog.InitialDirectory = file.Directory.FullName;
				dialog.FileName = file.Name;
			}
			dialog.OverwritePrompt = true;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Save Project As";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				mProjectFilename = dialog.FileName;
				SaveProject();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuHelpAbout_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Help / About menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuHelpAbout_Click(object sender, EventArgs e)
		{
			frmAbout dialog = new frmAbout();

			dialog.ShowDialog();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuToolsAlignScriptToCaptions_Click																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tools / Align Script To Captions menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuToolsAlignScriptToCaptions_Click(object sender,
			EventArgs e)
		{
			frmScriptToCaption dialog = new frmScriptToCaption();

			dialog.StartPosition = FormStartPosition.CenterParent;
			dialog.ShowDialog(this);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuToolsDisplayVTTInfo_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tools / Display VTT Information menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuToolsDisplayVTTInfo_Click(object sender, EventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			string content = "";
			string filename = OpenVTTDialog();
			VttInfoItem vttInfo = null;

			if(filename.Length > 0 && File.Exists(filename))
			{
				content = File.ReadAllText(filename);
				vttInfo = VttInfoItem.Parse(content);

				if(vttInfo != null)
				{
					builder.AppendLine($"Filename: {Path.GetFileName(filename)}");
					builder.AppendLine($"Captions: {vttInfo.Entries.Count}");
					builder.AppendLine(
						$"Total file time: {ToText(vttInfo.GetTotalFileTime())}");
					builder.AppendLine(
						$"Alpha characters: {vttInfo.GetAlphaCharacterCount()}");
					builder.AppendLine(
						$"Long-sound characters: {vttInfo.GetLongSoundCount()}");
					builder.AppendLine(
						$"Short-sound characters: {vttInfo.GetShortSoundCount()}");
					builder.AppendLine(
						"Total captioned time: " +
						$"{ToText(vttInfo.GetTotalCaptionedTime())}");

					MessageBox.Show(builder.ToString(), "VTT Information");
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuToolsExportMSBurstsExcel_Click																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tools / Export MS-Stream Audio Bursts To Excel menu option has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuToolsExportMSBurstsExcel_Click(object sender, EventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			string content = "";
			DataSet data = null;
			string excelFilename = "";
			TimeSpan timeBegin = TimeSpan.Zero;
			TimeSpan timeEndLast = TimeSpan.Zero;
			string filename = OpenVTTDialog();
			string groupNameLast = "";
			DataRow row = null;
			DataTable table = null;
			VttInfoItem vttInfo = null;

			if(filename.Length > 0)
			{
				//	A file was selected.
				content = File.ReadAllText(filename);
				vttInfo = VttInfoItem.Parse(content);

				if(vttInfo != null)
				{
					data = new DataSet("AudioBursts");
					table = new DataTable("AudioEntries");
					table.Columns.Add("Slide", typeof(string));
					table.Columns.Add("FreezeEnds", typeof(string));
					table.Columns.Add("TimeBegin", typeof(string));
					table.Columns.Add("TimeEnd", typeof(string));
					table.Columns.Add("Text", typeof(string));
					data.Tables.Add(table);

					foreach(VttEntryItem entryItem in vttInfo.Entries)
					{
						if(entryItem.GroupName != groupNameLast &&
							entryItem.TimeBegin != timeEndLast)
						{
							//	If the group name has changed and the time is not continued
							//	from the previous item, then start a new line.
							if(builder.Length > 0)
							{
								//	Store the previous row.
								row = table.NewRow();
								row.SetField<string>("TimeBegin", ToText(timeBegin));
								row.SetField<string>("TimeEnd", ToText(timeEndLast));
								row.SetField<string>("Text", builder.ToString());
								table.Rows.Add(row);
							}
							Clear(builder);
							timeBegin = entryItem.TimeBegin;
						}
						if(builder.Length > 0)
						{
							builder.Append(' ');
						}
						builder.Append(
							Regex.Replace(entryItem.Text, @"[\r\n]+", " ").Trim());
						timeEndLast = entryItem.TimeEnd;
						groupNameLast = entryItem.GroupName;
					}
					if(builder.Length > 0)
					{
						//	Store last row.
						row = table.NewRow();
						row.SetField<string>("TimeBegin", ToText(timeBegin));
						row.SetField<string>("TimeEnd", ToText(timeEndLast));
						row.SetField<string>("Text", builder.ToString());
						table.Rows.Add(row);
					}
					if(table.Rows.Count > 0)
					{
						excelFilename = SaveXLSXDialog();
						if(excelFilename.Length > 0)
						{
							dotExcel.ExcelFile.WriteWorkbook(excelFilename, data);
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuToolsLongSoundCount_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tools / Display Long-Sound Count menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuToolsLongSoundCount_Click(object sender, EventArgs e)
		{
			int count = 0;
			frmInput dialog = new frmInput();
			string text = "";

			dialog.Text = "Long-Sound Count";
			dialog.Prompt = "Enter text to inspect:";
			dialog.UserText = "";
			if(dialog.ShowDialog(this) == DialogResult.OK)
			{
				text = Regex.Replace(dialog.UserText, "[^a-zA-Z]+", "").ToLower();
				count = Regex.Replace(text, "[aeiouy]", "").Length;
				MessageBox.Show($"There are {count} long-sound letters.",
					"Long=Sound Count");
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* mnuToolsMoveContent_Click																							*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// The Tools / Move Content To Cursor menu option has been clicked.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Standard event arguments.
		///// </param>
		//private void mnuToolsMoveContent_Click(object sender, EventArgs e)
		//{

		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuToolsReWrapStreamVTT_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tools / Re-Wrap Microsoft Stream VTT menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private async void mnuToolsReWrapStreamVTT_Click(object sender, EventArgs e)
		{
			List<string> audioFilenames = new List<string>();
			bool bRetry = false;
			bool bTerminated = false;
			StringBuilder builder = new StringBuilder();
			List<char> capBuilder = new List<char>();
			int capIndex = 0;
			int capCount = 0;
			double capDuration = 0d;
			List<string> capList = new List<string>();
			double capPerSecond = 0d;
			string capText = "";
			TimeSpan capTimeBegin = TimeSpan.Zero;
			TimeSpan capTimeEnd = TimeSpan.Zero;
			char charCurrent = '\0';
			double charPerSecond = 0d;
			int charPosition = 0;
			char[] chars = null;
			char[] charSpaces = new char[] { ' ', '\t', '\r', '\n' };
			string content = "";
			int count = 0;
			int countCharPerCap = 0;
			int countSpoken = 0;
			int countSoundLong = 0;
			int countSoundShort = 0;
			int countText = 0;
			double duration = 0d;
			double durationLong = 0d;
			double durationShort = 0d;
			string filename = OpenVTTDialog();
			string folderPath = "";
			int groupCount = 0;
			int groupIndex = 0;
			List<string> groups = new List<string>();
			int index = 0;
			List<VttEntryItem> items = null;
			string mediaFilename = OpenMediaDialog();
			string outputFilename = "";
			double secPerCharacter = 0d;
			double soundLongShortRatio = 1.0;
			TimeSpan span = TimeSpan.Zero;
			//	TODO: Dummy subscription included. Switch the following value with your own API key for now.
			//	TODO: Place subscription code in config file or environment string.
			SpeechConfig speechConfig =
				SpeechConfig.FromSubscription(
					"238083385e8f482c8049f729e81f235c", "centralus");
			DetailedSpeechRecognitionResult speechDetailed = null;
			List<DetailedSpeechRecognitionResult> speechDetaileds = null;
			SpeechRecognitionResult speechResult = null;
			List<WordLevelTimingResult> speechWords = null;
			string text = "";
			TimeSpan timeBegin = TimeSpan.Zero;
			TimeSpan timeEnd = TimeSpan.Zero;
			VttEntryItem vttEntry = null;
			VttEntryItem vttEntryLast = null;
			VttInfoItem vttTarget = new VttInfoItem();
			VttInfoItem vttInfo = null;
			WordTrackerItem word = null;
			int wordOffset = 0;
			WordTrackerCollection wordsSpeech = new WordTrackerCollection();
			WordTrackerCollection wordsText = null;

			speechConfig.RequestWordLevelTimestamps();

			if(filename.Length > 0 && File.Exists(filename))
			{
				folderPath = Path.GetDirectoryName(filename);
				content = File.ReadAllText(filename);
				vttInfo = VttInfoItem.Parse(content);
				//	Get all starting items.
				items = vttInfo.Entries.FindAll(x => x.RelativeIndex == 0);
				foreach(VttEntryItem entryItem in items)
				{
					groups.Add(entryItem.GroupName);
				}
				groupCount = groups.Count;
				groupIndex = 1;
				foreach(string groupName in groups)
				{
					//	Process each audio burst.
					outputFilename = "";
					capList.Clear();
					items = vttInfo.Entries.FindAll(x =>
						x.GroupName == groupName).
						OrderBy(y => y.RelativeIndex).ToList();
					if(items.Count > 0)
					{
						//	All of the items in this set are members of the same burst.
						text = GetText(items).Trim();

						timeBegin = items[0].TimeBegin;
						timeEnd = items[^1].TimeEnd;

						duration = (timeEnd - timeBegin).TotalSeconds;
						countText = GetTextCharacterCount(items);
						countSpoken = GetSpokenCharacterCount(items);
						capCount = (int)Math.Ceiling(
							(double)countText / (double)mMaxLineLength);
						charPerSecond = (double)countText / duration;
						capPerSecond = (double)capCount / duration;
						countCharPerCap = (int)Math.Ceiling(charPerSecond / capPerSecond);
						secPerCharacter = duration / (double)countSpoken;
						countSoundLong = GetLongSoundCount(items);
						countSoundShort = GetShortSoundCount(items);
						soundLongShortRatio =
							((double)countSoundLong / (double)countSoundShort) * 0.8d;
						durationLong = secPerCharacter / soundLongShortRatio;
						durationShort = secPerCharacter * soundLongShortRatio;

						if(mediaFilename.Length > 0)
						{
							//	If the media file was selected, then extract a section.
							//	TODO: Delete all created .WAV files upon completion of task.
							Status($"Extracting audio as {groupName}.wav...");
							outputFilename = Path.Combine(folderPath, $"{groupName}.wav");
							ExtractAudioSection(mediaFilename, timeBegin, timeEnd,
								outputFilename);
							if(!File.Exists(outputFilename))
							{
								outputFilename = "";
							}
							if(outputFilename.Length > 0)
							{
								audioFilenames.Add(outputFilename);
							}
						}


						//	Place characters in appropriate containers.
						Clear(builder);
						chars = text.ToCharArray();
						count = chars.Length;
						index = 0;

						bRetry = true;
						while(bRetry)
						{
							bRetry = false;
							for(capIndex = 0; capIndex < capCount; capIndex++)
							{
								//	Each caption.
								capBuilder.Clear();
								charPosition = 0;
								bTerminated = false;
								//	Skip current spaces at the start of the caption.
								while(index < count &&
									charSpaces.Contains(chars[index]))
								{
									index++;
								}
								//	Build the current caption.
								for(;
									index < count && charPosition < mMaxLineLength;
									index++, charPosition++)
								{
									//	Each character.
									charCurrent = chars[index];
									if(charPosition >= countCharPerCap &&
										charSpaces.Contains(charCurrent))
									{
										//	Leave the caption without adding the space.
										bTerminated = true;
										break;
									}
									else
									{
										//	When the position is less than characters per cap,
										//	add unconditionally.
										//	Otherwise, when the position is less than max and
										//	not a space, add anyway.
										capBuilder.Add(charCurrent);
									}
								}
								if(index >= count)
								{
									bTerminated = true;
								}
								//	After processing the current caption, rewind to the
								//	previous space if the last item was not terminated.
								if(!bTerminated)
								{
									while(capBuilder.Count > 0)
									{
										if(charSpaces.Contains(capBuilder[^1]))
										{
											//	The current item is a space.
											bTerminated = true;
										}
										else if(bTerminated)
										{
											//	The current last item is not a space,
											//	and the termination space has been found.
											//	Don't consume this item.
											break;
										}
										capBuilder.RemoveAt(capBuilder.Count - 1);
										index--;
									}
								}
								//	Place the adjusted caption into the collection.
								capText = string.Join("", capBuilder);
								capList.Add(capText);
							}
							if(index < count)
							{
								//	If too few containers, then add another one.
								//	More characters are available. Create another caption from
								//	the remaining items.
								Clear(builder);
								//	Skip current spaces at the start of the caption.
								while(index < count &&
									charSpaces.Contains(chars[index]))
								{
									index++;
								}
								for(; index < count; index++)
								{
									builder.Append(chars[index]);
								}
								if(builder.Length > 0)
								{
									capList.Add(builder.ToString());
									capCount++;
									capPerSecond = (double)capCount / duration;
									countCharPerCap =
										(int)Math.Ceiling(charPerSecond / capPerSecond);
									capList.Clear();
									index = 0;
									bRetry = true;
								}
							}
						}

						//	Recognize the entire burst as a single unit.
						speechWords = null;
						if(outputFilename.Length > 0)
						{
							//	The WAV file was created in an earlier step.
							Status(
								$"{groupIndex} of {groupCount}: Recognizing {groupName}...");
							//	Recognize the relative positions of each of the words
							//	in this burst.
							using(AudioConfig audioInput =
								AudioConfig.FromWavFileInput(outputFilename))
							{
								//	This can be intent or speech recognizer.
								//	However, it doesn't appear that the intent recognizer
								//	provides access to single-word placement.
								using(SpeechRecognizer recognizer =
									new SpeechRecognizer(speechConfig, audioInput))
								{
									//recognizer.AddIntent(text.ToLower());
									//	TODO: If speechResult Reason is Cancelled, retry.
									//	Use a list to retry all of the items that were
									//	cancelled in this pass.
									//	Try to get back to the original context.
									speechResult =
										await recognizer.RecognizeOnceAsync().
											ConfigureAwait(true);
									//speechResult =
									//	await recognizer.RecognizeOnceAsync().
									//		ConfigureAwait(false);
									if(speechResult != null)
									{
										try
										{
											speechDetaileds = speechResult.Best().ToList();
											if(speechDetaileds.Count > 0)
											{
												speechDetailed = speechDetaileds[0];
												speechWords = speechDetailed.Words.ToList();
											}
										}
										catch(Exception ex)
										{
											statMessage.Text = $"Error: {ex.Message}";
											Debug.WriteLine(
												$"Error on '{text}'. " +
												$"Status: {speechResult.Reason}. {ex.Message}");
											speechResult = null;
											speechWords = null;
										}
									}
								}
							}
						}
						if(speechWords != null)
						{
							wordsText = new WordTrackerCollection(text.ToLower());
							wordsSpeech.Clear();
							foreach(WordLevelTimingResult speechWordItem in speechWords)
							{
								wordsSpeech.Add(new WordTrackerItem()
								{
									Begin =
										TimeSpan.FromSeconds(
											(double)speechWordItem.Offset / 10000000d),
									End =
										TimeSpan.FromSeconds(
											((double)speechWordItem.Offset +
											(double)speechWordItem.Duration) / 10000000d),
									Text = WordOnly(speechWordItem.Word.ToLower())
								});
							}
							if(WordTrackerCollection.
								CompareConsistency(wordsText, wordsSpeech) >= 0.8d)
							{
								//	If there is reasonable consistency in the word
								//	group, find the offsets for each of the captions.
								capTimeBegin = timeBegin;
								count = capList.Count;
								wordOffset = 0;
								for(index = 0; index < count; index++)
								{
									//	Position each caption.
									text = capList[index];
									if(index + 1 == count)
									{
										//	If this is the last item, then use the ending time as
										//	the caption end time.
										capDuration =
											timeEnd.TotalSeconds - capTimeBegin.TotalSeconds;
										capTimeEnd = timeEnd;
									}
									else
									{
										//	Advance the word list early to create end point.
										wordOffset += WordsToList(text).Count;
										word = wordsText.GetNearestMatchingItem(wordOffset);
										if(word != null)
										{
											//	A close-by matching value was found.
											capTimeEnd = (word.Begin + timeBegin);
											capDuration =
												capTimeEnd.TotalSeconds - capTimeBegin.TotalSeconds;
										}
									}
									//	Create the VTT entry.
									vttEntry = new VttEntryItem()
									{
										GroupName = groupName,
										RelativeIndex = index,
										Id = $"{groupName}-{index}",
										Text = capList[index],
										TimeBegin = capTimeBegin,
										TimeEnd = capTimeEnd
									};
									vttTarget.Entries.Add(vttEntry);
									//	Advance the start time to the next caption.
									capTimeBegin = capTimeEnd;
								}
							}
							else
							{
								speechWords = null;
							}
						}

						if(speechWords == null)
						{
							//	Create each of the entries by calculation only.
							capTimeBegin = timeBegin;
							count = capList.Count;
							for(index = 0; index < count; index++)
							{
								text = capList[index];
								if(index + 1 == count)
								{
									//	If this is the last item, then use the ending time as the
									//	caption end time.
									capTimeEnd = timeEnd;
								}
								else
								{
									capDuration =
										CalcTextDuration(text, durationShort, durationLong);
									capTimeEnd = ToTimeSpan(
										ToText(capTimeBegin + TimeSpan.FromSeconds(capDuration)));
								}
								vttEntry = new VttEntryItem()
								{
									GroupName = groupName,
									RelativeIndex = index,
									Id = $"{groupName}-{index}",
									Text = capList[index],
									TimeBegin = capTimeBegin,
									TimeEnd = capTimeEnd
								};
								vttTarget.Entries.Add(vttEntry);
								//	Advance the start time to the next caption.
								capTimeBegin = capTimeEnd;
								vttEntryLast = vttEntry;
							}
						}
					}
					groupIndex++;
				}
				foreach(string audioFilenameItem in audioFilenames)
				{
					try
					{
						File.Delete(audioFilenameItem);
					}
					catch { }
				}
				filename = SaveVTTDialog("Save Processed VTT As",
					$"{GetFilenameNoExtension(filename)}-Processed" +
					GetExtension(filename));
				if(filename?.Length > 0)
				{
					content = vttTarget.ToString();
					File.WriteAllText(filename, content);
					Status("Stream VTT file has been re-wrapped");
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuToolsShortSoundCount_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tools / Display Short-Sound Count menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuToolsShortSoundCount_Click(object sender, EventArgs e)
		{
			int count = 0;
			frmInput dialog = new frmInput();
			string text = "";

			dialog.Text = "Short-Sound Count";
			dialog.Prompt = "Enter text to inspect:";
			dialog.UserText = "";
			if(dialog.ShowDialog(this) == DialogResult.OK)
			{
				text = Regex.Replace(dialog.UserText, "[^a-zA-Z]+", "").ToLower();
				count = Regex.Replace(text, "[^aeiouy]+", "").Length;
				MessageBox.Show($"There are {count} short-sound letters.",
					"Short=Sound Count");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuToolsUpdateTargetFromSource_Click																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tools / Update Target From Source VTT menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuToolsUpdateTargetFromSource_Click(object sender,
			EventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			string content = "";
			int count = 0;
			VttEntryItem entrySource = null;
			VttEntryItem entryTarget = null;
			string filenameSource = OpenVTTDialog("Source VTT File");
			string filenameTarget = "";
			int index = 0;
			VttInfoItem vttInfoSource = null;
			VttInfoItem vttInfoTarget = null;

			if(filenameSource.Length > 0 && File.Exists(filenameSource))
			{
				filenameTarget = OpenVTTDialog("Target VTT File");
				if(filenameTarget.Length > 0 && File.Exists(filenameTarget))
				{
					content = File.ReadAllText(filenameSource);
					vttInfoSource = VttInfoItem.Parse(content);
					content = File.ReadAllText(filenameTarget);
					vttInfoTarget = VttInfoItem.Parse(content);

					if(vttInfoSource != null && vttInfoTarget != null)
					{
						//	Both files have been opened.
						if(vttInfoSource.Entries.Count > 0 &&
							vttInfoSource.Entries.Count ==
							vttInfoTarget.Entries.Count)
						{
							//	Both sets of entries line up.
							count = vttInfoSource.Entries.Count;
							for(index = 0; index < count; index++)
							{
								entrySource = vttInfoSource.Entries[index];
								entryTarget = vttInfoTarget.Entries[index];
								entryTarget.Text = entrySource.Text;
							}
							filenameTarget =
								SaveVTTDialog("Save Target VTT File As", filenameTarget);
							if(filenameTarget.Length > 0)
							{
								Status("VTT source text has been copied to target...");
								File.WriteAllText(filenameTarget, vttInfoTarget.ToString());
							}
						}
						else
						{
							MessageBox.Show("Can not synchronize source and target.\r\n" +
								"Entry counts are different.\r\n" +
								$"Source: {vttInfoSource.Entries.Count}, " +
								$"Target: {vttInfoTarget.Entries.Count}",
								"Update Target from Source VTT");
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoBack_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go Back 5 Seconds menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoBack_Click(object sender, EventArgs e)
		{
			GoBack();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoBack_EnabledChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go Back 5 Seconds Enabled status has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoBack_EnabledChanged(object sender, EventArgs e)
		{
			tbtnTransportGoBack.Enabled = mnuTransportGoBack.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoForward_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go Forward 5 Seconds menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoForward_Click(object sender, EventArgs e)
		{
			GoForward();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoForward_EnabledChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go Forward 5 Seconds Enabled state has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoForward_EnabledChanged(object sender,
			EventArgs e)
		{
			tbtnTransportGoForward.Enabled = mnuTransportGoForward.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoToEnd_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go To End menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoToEnd_Click(object sender, EventArgs e)
		{
			GoToEnd();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoToEnd_EnabledChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go To End Enabled state has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoToEnd_EnabledChanged(object sender, EventArgs e)
		{
			tbtnTransportGoToEnd.Enabled = mnuTransportGoToEnd.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoToStart_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go To Start menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoToStart_Click(object sender, EventArgs e)
		{
			GoToStart();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportGoToStart_EnabledChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go To Start Enabled state has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportGoToStart_EnabledChanged(object sender,
			EventArgs e)
		{
			tbtnTransportGoToStart.Enabled = mnuTransportGoToStart.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportPlayPause_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Play menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportPlayPause_Click(object sender, EventArgs e)
		{
			PlayPause();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportPlayPause_EnabledChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Play Enabled state has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportPlayPause_EnabledChanged(object sender,
			EventArgs e)
		{
			tbtnTransportPlayPause.Enabled = mnuTransportPlayPause.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportStop_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Stop menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportStop_Click(object sender, EventArgs e)
		{
			Stop();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuTransportStop_EnabledChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Stop Enabled state has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuTransportStop_EnabledChanged(object sender, EventArgs e)
		{
			tbtnTransportStop.Enabled = mnuTransportStop.Enabled;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuViewCenterCursor_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The View / Center Cursor On Screen menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuViewCenterCursor_Click(object sender, EventArgs e)
		{
			CenterPlayhead();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MoveCaptions																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move captions by the specified amount, starting at a given index.
		/// </summary>
		/// <param name="startIndex">
		/// The index at which to start moving all remaining captions.
		/// </param>
		/// <param name="adjustment">
		/// The horizontal adjustment to make.
		/// </param>
		/// <param name="undo">
		/// Reference to an active undo action to which the movements will be
		/// added.
		/// </param>
		private void MoveCaptions(int startIndex, double adjustment,
			UndoItem undo = null)
		{
			CaptionItem caption = null;
			CaptionCollection captions = captionEditor.Captions;
			int count = captions.Count;
			int index = 0;

			for(index = startIndex; index < count; index++)
			{
				caption = captions[index];
				if(undo != null)
				{
					undo.Supports.Add(new UndoSupportItem()
					{
						Action = ActionTypeEnum.EditCaptionX,
						Caption = caption,
						Index = index
					});
				}
				caption.X += adjustment;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mTimer_Tick																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The playhead timer has elapsed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mTimer_Tick(object sender, EventArgs e)
		{
			double selectionWidth = 0d;

			mTickCount++;

			UpdateMouse();
			//	Transport update.
			if(TransportState == TransportStateEnum.Play)
			{
				//	On play, don't update the media channel unless too far off.
				selectionWidth =
					captionEditor.SelectionEnd - captionEditor.SelectionStart;
				if(selectionWidth >= 0.25 &&
					mPlayhead >= captionEditor.SelectionEnd)
				{
					//	Playhead has crossed the selection line.
					Playhead = captionEditor.SelectionStart;
				}
				else
				{
					mMediaBusy = true;
					Playhead = ToSeconds(GetCurrentMediaTime());
					mMediaBusy = false;
				}
			}
			//else if(TransportState == TransportStateEnum.Pause ||
			//	TransportState == TransportStateEnum.Stop)
			//{
			//	//	When the transport is in Pause, the player might be running
			//	//	until an update is reached.
			//	if(mPauseOnPlayheadChange && mPlayhead != mPlayheadLast &&
			//		mMediaPlayer.IsPlaying)
			//	{
			//		mMediaPlayer.Pause();
			//		mPauseOnPlayheadChange = false;
			//	}
			//}
			mPlayheadLast = mPlayhead;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mUndoStack_CollectionChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Contents of the undo stack have changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Notify collection changed event arguments.
		/// </param>
		private void mUndoStack_CollectionChanged(object sender,
			System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if(mUndoStack.Count > 0)
			{
				mnuEditUndo.Text = $"&Undo ({UndoTitle()})";
				mnuEditUndo.Enabled = true;
			}
			else
			{
				mnuEditUndo.Text = "&Undo";
				mnuEditUndo.Enabled = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlayPause																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Play or pause, according to the current transport state.
		/// </summary>
		private void PlayPause()
		{
			Debug.Write($"Before PlayPause during {TransportState}:");
			Debug.Write($"Playhead: {captionEditor.Playhead} vs. ");
			Debug.WriteLine($"Media: {mMediaPlayer.Player.Position.TotalSeconds}");
			if(TransportState == TransportStateEnum.Play)
			{
				mMediaPlayer.Pause();
				//mTimer.Stop();
				TransportState = TransportStateEnum.Pause;
			}
			else if(TransportState == TransportStateEnum.Pause)
			{
				mMediaPlayer.Play();
				mMediaPlayer.Position = ToTimeSpan(captionEditor.Playhead);
				//mTimer.Start();
				TransportState = TransportStateEnum.Play;
			}
			else
			{
				mMediaPlayer.Play();
				mMediaPlayer.Position = ToTimeSpan(captionEditor.Playhead);
				//mTimer.Start();
				TransportState = TransportStateEnum.Play;
			}
			Debug.Write("After PlayPause: ");
			Debug.Write($"Playhead: {captionEditor.Playhead} vs. ");
			Debug.WriteLine($"Media: {mMediaPlayer.Player.Position.TotalSeconds}");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* player_MediaOpened																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The media has opened on the media player.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Routed event arguments.
		/// </param>
		private void player_MediaOpened(object sender,
			System.Windows.RoutedEventArgs e)
		{
			Duration = mMediaPlayer.Duration;
			captionEditor.LoadWaveform(mMediaFilename, Duration);
			mMediaPlayer.Stop();
			TransportState = TransportStateEnum.Stop;
			mStatusBusy = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RefreshPlayer																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Refresh the player view.
		/// </summary>
		private void RefreshPlayer()
		{
			//if(TransportState == TransportStateEnum.Pause ||
			//	TransportState == TransportStateEnum.Stop)
			//{
			//	mMediaPlayer.Play();
			//	mPauseOnPlayheadChange = true;
			//}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ReplaceAll																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Replace all instances of the find text with the replacement text.
		/// </summary>
		private void ReplaceAll()
		{
			CaptionCollection captions = captionEditor.Captions;
			string findLower = mFindText.ToLower();
			int replacementCaptionCount = 0;
			int replacementCount = 0;

			captions.SelectedItems.Clear();
			mFindIndex = -1;
			foreach(CaptionItem captionItem in captions)
			{
				if(captionItem.EntryType != CaptionEntryTypeEnum.Space &&
					captionItem.Text.ToLower().IndexOf(findLower) > -1)
				{
					replacementCount += CountPattern(captionItem.Text, findLower);
					replacementCaptionCount++;
					captionItem.Text = captionItem.Text.Replace(mFindText, mFindReplace,
						StringComparison.CurrentCultureIgnoreCase);
				}
			}
			Status($"[{mFindText}] -> [{mFindReplace}]: " +
				$"{replacementCount} replacements made in " +
				$"{replacementCaptionCount} captions...");
			if(replacementCount > 0)
			{
				captionEditor.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ReplaceCurrent																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Replace the currently available item, starting at the current caption,
		/// if applicable.
		/// </summary>
		private void ReplaceCurrent()
		{
			bool bFound = false;
			CaptionItem caption = null;
			CaptionCollection captions = captionEditor.Captions;
			int count = captions.Count;
			string findLower = mFindText.ToLower();
			int index = 0;
			int replacementCount = 0;

			if(mFindIndex > -1 && mFindIndex < captions.Count)
			{
				index = mFindIndex;
			}
			else
			{
				index = 0;
			}
			for(; index < count; index++)
			{
				caption = captions[index];
				if(caption.EntryType != CaptionEntryTypeEnum.Space &&
					caption.Text.ToLower().IndexOf(findLower) > -1)
				{
					replacementCount += CountPattern(caption.Text, findLower);
					caption.Text = caption.Text.Replace(mFindText, mFindReplace,
						StringComparison.CurrentCultureIgnoreCase);
					bFound = true;
					Status($"[{mFindText}] -> [{mFindReplace}]: " +
						$"{replacementCount} replacements made in " +
						$"1 caption...");
					Playhead = caption.X + (caption.Width / 2d);
					CenterPlayhead();
					captionEditor.Invalidate();
					break;
				}
			}
			if(!bFound)
			{
				if(mFindIndex > -1)
				{
					mFindIndex = -1;
					if(MessageBox.Show(this,
						"End of file reached. " +
						"Do you want to search from the beginning?", "Replace Next",
						MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						ReplaceCurrent();
					}
				}
				else
				{
					MessageBox.Show(this,
						$"{mFindText} was not found...", "Replace Next");
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RippleOnOff																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Toggle the ripple edit mode on and off.
		/// </summary>
		private void RippleOnOff()
		{
			mRippleMode = !mRippleMode;
			mnuEditRippleOnOff.Checked = mRippleMode;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveCaptions																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Save captions data to the appointed file.
		/// </summary>
		private void SaveCaptions()
		{
			StringBuilder builder = null;
			//int lineIndex = 1;
			MatchCollection matches = null;
			TimeSpan timeStart = TimeSpan.MinValue;
			TimeSpan timeStop = TimeSpan.MinValue;
			TimeSpan timeStopPrev = TimeSpan.FromSeconds(0d);

			if(mCaptionFilename?.Length > 0)
			{
				builder = new StringBuilder();
				builder.AppendLine("WEBVTT");
				builder.AppendLine("Kind: captions");
				builder.AppendLine("Source: CaptionAll Closed Caption Editor");
				//	TODO: Post current application version in file data.
				builder.Append("Source Version: ");
				builder.AppendLine($"{typeof(frmMain).Assembly.GetName().Version}");
				builder.AppendLine("");
				foreach(CaptionItem captionItem in captionEditor.Captions)
				{
					timeStart = ToTimeSpan(captionItem.X);
					if(timeStart == timeStopPrev)
					{
						timeStart = timeStart.Add(TimeSpan.FromMilliseconds(1d));
					}
					timeStop = TimeSpan.FromSeconds(
						(captionItem.X + captionItem.Width));
					if(captionItem.EntryType != CaptionEntryTypeEnum.Space)
					{
						//builder.AppendLine($"{lineIndex++}");
						builder.Append(timeStart.ToString(@"hh\:mm\:ss\.fff"));
						builder.Append(" --> ");
						builder.AppendLine(timeStop.ToString(@"hh\:mm\:ss\.fff"));
						//builder.AppendLine(
						//	$"{timeStart:hh:mm:ss.fff} --> {timeStop:hh:mm:ss.fff}");
						if(captionItem.Text?.Length > 0)
						{
							matches =
								Regex.Matches(captionItem.Text, ResourceMain.rxSingleLine);
							foreach(Match matchItem in matches)
							{
								builder.AppendLine(GetValue(matchItem, "line"));
							}
						}
						builder.AppendLine("");
					}
					timeStopPrev = timeStop;
				}
				try
				{
					File.WriteAllText(mCaptionFilename, builder.ToString());
				}
				catch { }
			}
			captionEditor.Captions.Changed = false;
			mnuFileSaveCaptions.Enabled = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveCaptionsSRT																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Save captions data, in SRT format, to the appointed file.
		/// </summary>
		/// <param name="filename">
		/// Fully qualified path and filename of the file to write.
		/// </param>
		private void SaveCaptionsSRT(string filename)
		{
			StringBuilder builder = null;
			int lineIndex = 1;
			MatchCollection matches = null;
			TimeSpan timeStart = TimeSpan.MinValue;
			TimeSpan timeStop = TimeSpan.MinValue;
			TimeSpan timeStopPrev = TimeSpan.FromSeconds(0d);

			if(filename?.Length > 0)
			{
				builder = new StringBuilder();
				builder.AppendLine("");
				foreach(CaptionItem captionItem in captionEditor.Captions)
				{
					timeStart = ToTimeSpan(captionItem.X);
					if(timeStart == timeStopPrev)
					{
						timeStart = timeStart.Add(TimeSpan.FromMilliseconds(1d));
					}
					timeStop = TimeSpan.FromSeconds(
						(captionItem.X + captionItem.Width));
					if(captionItem.EntryType != CaptionEntryTypeEnum.Space)
					{
						builder.AppendLine($"{lineIndex++}");
						builder.Append(timeStart.ToString(@"hh\:mm\:ss\,fff"));
						builder.Append(" --> ");
						builder.AppendLine(timeStop.ToString(@"hh\:mm\:ss\,fff"));
						if(captionItem.Text?.Length > 0)
						{
							matches =
								Regex.Matches(captionItem.Text, ResourceMain.rxSingleLine);
							foreach(Match matchItem in matches)
							{
								builder.AppendLine(GetValue(matchItem, "line"));
							}
						}
						builder.AppendLine("");
					}
					timeStopPrev = timeStop;
				}
				try
				{
					File.WriteAllText(filename, builder.ToString());
				}
				catch { }
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveProject																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Save the current project.
		/// </summary>
		private void SaveProject()
		{
			string content = "";
			FileInfo file = null;
			string folderPath = "";

			if(mProjectFilename?.Length > 0)
			{
				{
					file = new FileInfo(mProjectFilename);
					folderPath = file.Directory.FullName;
					mProjectInfo = new CaptionProjectItem()
					{
						FolderPath = folderPath
					};
				}
				if(mCaptionFilename == "")
				{
					mCaptionFilename =
						Path.Combine(folderPath, LeftOf(file.Name, ".") + ".vtt");
				}
				if(captionEditor.Captions.Count > 0)
				{
					SaveCaptions();
				}
				mProjectInfo.CaptionFilename =
					GetRelativeFilename(folderPath, mCaptionFilename);
				mProjectInfo.MediaFilename =
					GetRelativeFilename(folderPath, mMediaFilename);
				mProjectInfo.CommonlyUsedWords.Clear();
				mProjectInfo.CommonlyUsedWords.AddRange(
					CaptionAllUtil.CommonlyUsedWords);
				content = JsonConvert.SerializeObject(mProjectInfo);
				File.WriteAllText(mProjectFilename, content);
				Status("Project saved...");
			}
			else
			{
				Status("Could not save project. No filename specified...");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveTranscript																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Save transcript text, in TXT format, to the appointed file.
		/// </summary>
		/// <param name="filename">
		/// Fully qualified path and filename of the file to write.
		/// </param>
		private void SaveTranscript(string filename)
		{
			StringBuilder builder = new StringBuilder();
			StringBuilder content = new StringBuilder();
			CaptionEntryTypeEnum lastEntryType = CaptionEntryTypeEnum.Space;
			string line = "";
			string lastText = "";

			if(filename?.Length > 0)
			{
				foreach(CaptionItem captionItem in captionEditor.Captions)
				{
					switch(captionItem.EntryType)
					{
						case CaptionEntryTypeEnum.Normal:
							if(lastEntryType == CaptionEntryTypeEnum.Normal &&
								!lastText.EndsWith(" "))
							{
								if(Regex.IsMatch(captionItem.Text, @"(?<find>^\(\w+\))"))
								{
									//	If this line starts with a named character, break with
									//	new line.
									builder.AppendLine("");
								}
								else
								{
									//	Otherwise, connect the new text to the previous with
									//	a space join.
									builder.Append(" ");
								}
							}
							builder.Append(captionItem.Text);
							break;
						case CaptionEntryTypeEnum.Space:
						default:
							if(builder.Length > 0 &&
								lastEntryType == CaptionEntryTypeEnum.Normal &&
								lastText.Length > 0)
							{
								line = builder.ToString();
								//	Replace multi-line quote breaks.
								line = line.Replace("\" \"", " ");
								content.AppendLine($"{line}\r\n");
								//builder.AppendLine("\r\n");
								Clear(builder);
							}
							break;
					}
					lastText = captionItem.Text;
					lastEntryType = captionItem.EntryType;
				}
				//	Paste any remaining content.
				if(builder.Length > 0)
				{
					line = builder.ToString();
					//	Replace multi-line quote breaks.
					line = line.Replace("\" \"", " ");
					content.AppendLine($"{line}\r\n");
				}
				//builder.AppendLine("");
				try
				{
					File.WriteAllText(filename, content.ToString());
				}
				catch { }
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveVTTDialog																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Display a Save VTT file dialog and return the name of the selected
		/// file.
		/// </summary>
		/// <param name="dialogTitle">
		/// Optional title to display on the save file dialog.
		/// </param>
		/// <param name="defaultFilename">
		/// Optional default filename to select.
		/// </param>
		/// <returns>
		/// The full path and filename to save, if selected. Otherwise, and empty
		/// string.
		/// </returns>
		private string SaveVTTDialog(string dialogTitle = "",
			string defaultFilename = "")
		{
			SaveFileDialog dialog = new SaveFileDialog();
			FileInfo file = null;
			string result = "";

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = false;
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.DefaultExt = ".vtt";
			dialog.DereferenceLinks = true;
			if(defaultFilename?.Length > 0)
			{
				file = new FileInfo(defaultFilename);
				dialog.InitialDirectory = file.Directory.FullName;
				dialog.FileName = file.Name;
			}
			dialog.Filter =
				"WebVTT Caption Files " +
				"(*.vtt)|" +
				"*.vtt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.OverwritePrompt = true;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = (dialogTitle?.Length > 0 ?
				dialogTitle : "Save Captions File As");
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.FileName;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveXLSXDialog																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Display a Save XLSX file dialog and return the name of the selected
		/// file.
		/// </summary>
		/// <param name="dialogTitle">
		/// Optional title to display on the save file dialog.
		/// </param>
		/// <param name="defaultFilename">
		/// Optional default filename to select.
		/// </param>
		/// <returns>
		/// The full path and filename to save, if selected. Otherwise, and empty
		/// string.
		/// </returns>
		private string SaveXLSXDialog(string dialogTitle = "",
			string defaultFilename = "")
		{
			SaveFileDialog dialog = new SaveFileDialog();
			FileInfo file = null;
			string result = "";

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = false;
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.DefaultExt = ".xlsx";
			dialog.DereferenceLinks = true;
			if(defaultFilename?.Length > 0)
			{
				file = new FileInfo(defaultFilename);
				dialog.InitialDirectory = file.Directory.FullName;
				dialog.FileName = file.Name;
			}
			dialog.Filter =
				"Excel Files " +
				"(*.xlsx)|" +
				"*.xlsx;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.OverwritePrompt = true;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = (dialogTitle?.Length > 0 ?
				dialogTitle : "Save Excel File As");
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.FileName;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* scrollTimeline_ValueChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The scrolling timeline window position has updated.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void scrollTimeline_ValueChanged(object sender, EventArgs e)
		{
			//Status($"Scroll offset: {scrollTimeline.ScrollPositionH}");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Status																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the status bar message.
		/// </summary>
		public async void Status(string message)
		{
			if(message != null && !mStatusBusy)
			{
				await Task.Run(() =>
				{
					statusStrip.Invoke((MethodInvoker)delegate
					{
						statMessage.Text = message;
						statMessage.Invalidate();
						statusStrip.Refresh();
					});
				});
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* statusStrip_MouseLeave																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse has left the status strip.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void statusStrip_MouseLeave(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Stop																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The stop button has been clicked.
		/// </summary>
		private void Stop()
		{
			mMediaPlayer.Stop();
			//mTimer.Stop();
			TransportState = TransportStateEnum.Stop;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* tbtnEditRippleOnOff_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Ripple On | Off toolbar button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void tbtnEditRippleOnOff_Click(object sender, EventArgs e)
		{
			RippleOnOff();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* tbtnTransportGoBack_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go Back 5 Seconds toolbar button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void tbtnTransportGoBack_Click(object sender, EventArgs e)
		{
			GoBack();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* tbtnTransportGoForward_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go Forward 5 Seconds toolbar button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void tbtnTransportGoForward_Click(object sender, EventArgs e)
		{
			GoForward();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* tbtnTransportGoToEnd_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go To End toolbar button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void tbtnTransportGoToEnd_Click(object sender, EventArgs e)
		{
			GoToEnd();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* tbtnTransportGoToStart_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Go To Start toolbar button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void tbtnTransportGoToStart_Click(object sender, EventArgs e)
		{
			GoToStart();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* tbtnTransportPlayPause_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Play and Pause multiuse toolbar button has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void tbtnTransportPlayPause_Click(object sender, EventArgs e)
		{
			PlayPause();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* tbtnTransportStop_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Transport / Stop toolbar button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void tbtnTransportStop_Click(object sender, EventArgs e)
		{
			Stop();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Time																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Display the current time on the time status control.
		/// </summary>
		private async void Time(double seconds)
		{
			double ticks = 0d;
			TimeSpan time = TimeSpan.MinValue;

			ticks = ToMilliseconds(seconds) * 10000d;
			time = new TimeSpan((long)ticks);
			await Task.Run(() =>
			{
				statusStrip.Invoke((MethodInvoker)delegate
				{
					mMediaPlayer.SetClock(time);
					statTime.Text =
						$"{time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}." +
						$"{time.Milliseconds:000}";
					statTime.Invalidate();
					statusStrip.Refresh();
				});
			});
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UndoTitle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the title related to the current undo item in the stack.
		/// </summary>
		/// <returns>
		/// Title of the current undo item in the stack, if present. Otherwise,
		/// an empty string.
		/// </returns>
		private string UndoTitle()
		{
			string result = "";

			if(mUndoStack.Count > 0)
			{
				switch(mUndoStack.Last().Action)
				{
					case ActionTypeEnum.DeleteCaption:
						result = "Delete caption";
						break;
					case ActionTypeEnum.EditCaptionProperties:
						result = "Caption properties";
						break;
					case ActionTypeEnum.EditCaptionText:
						result = "Caption text";
						break;
					case ActionTypeEnum.EditCaptionType:
						result = "Change caption type";
						break;
					case ActionTypeEnum.EditCaptionUI:
						result = "Adjust caption";
						break;
					case ActionTypeEnum.EditCaptionWidth:
						result = "Caption width";
						break;
					case ActionTypeEnum.EditCaptionX:
						result = "Caption position";
						break;
					case ActionTypeEnum.InsertCaption:
						result = "Insert caption";
						break;
					case ActionTypeEnum.MergeCaptions:
						result = "Merge captions";
						break;
					case ActionTypeEnum.None:
						result = "Unknown action";
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateMouse																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the mouse statistics from universal mouse information.
		/// </summary>
		private void UpdateMouse()
		{
			MouseButtons buttons = System.Windows.Forms.Control.MouseButtons;
			Point point = new Point();

			//	Mouse update.
			GetCursorPos(ref point);
			mMouseUniversalLast = mMouseUniversal;
			mMouseUniversal = point;
			mMouseLocationLast = mMouseLocation;
			mMouseLocation = new Point(point.X - this.Left, point.Y - this.Top);
			mMouseButtonsLast = mMouseButtons;
			mMouseButtons = buttons;
			if(mMouseButtons != mMouseButtonsLast)
			{
				//	Mouse button changed.
				mMouseDown = (mMouseButtons == MouseButtons.Left);
				if(mMouseDown)
				{
					mMouseCount++;
				}
				switch(mMouseButtons)
				{
					case MouseButtons.Left:
					case MouseButtons.Middle:
					case MouseButtons.Right:
						OnMouseDown(new MouseEventArgs(mMouseButtons, 0,
							mMouseLocation.X, mMouseLocation.Y, 0));
						break;
					case MouseButtons.None:
						switch(mMouseButtonsLast)
						{
							case MouseButtons.Left:
							case MouseButtons.Middle:
							case MouseButtons.Right:
								OnMouseUp(new MouseEventArgs(mMouseButtons, 1,
									mMouseLocation.X, mMouseLocation.Y, 0));
								break;
						}
						if(mMouseButtonsLast == MouseButtons.Left)
						{
							OnMouseClick(new MouseEventArgs(mMouseButtons, 1,
								mMouseLocation.X, mMouseLocation.Y, 0));
						}
						break;
				}
			}
			if(!mMouseLocation.Equals(mMouseLocationLast))
			{
				//	Mouse moved.
				OnMouseMove(new MouseEventArgs(mMouseButtons, 0,
					mMouseLocation.X, mMouseLocation.Y, 0));
				if(!IsOverForm(mMouseUniversalLast) &&
					IsOverForm(mMouseUniversal))
				{
					//	Entered form.
					OnMouseEnter(new EventArgs());
				}
				else if(IsOverForm(mMouseUniversalLast) &&
					!IsOverForm(mMouseUniversal))
				{
					//	Left form.
					OnMouseLeave(new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//* OnActivated																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Activated event when the form has been activated.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if(!mActivated)
			{
				mActivated = true;
				TransportState = TransportStateEnum.None;
			}
			captionEditor.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnKeyDown																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the KeyDown event when a keyboard key has been depressed.
		/// </summary>
		/// <param name="e">
		/// Key event arguments.
		/// </param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if(!e.Handled)
			{
				switch(e.KeyCode)
				{
					case Keys.End:
						//	End.
						e.Handled = true;
						mnuTransportGoToEnd_Click(this, new EventArgs());
						mnuViewCenterCursor_Click(this, new EventArgs());
						break;
					case Keys.F2:
						//	Edit
						e.Handled = true;
						break;
					case Keys.Home:
						//	Home.
						e.Handled = true;
						mnuTransportGoToStart_Click(this, new EventArgs());
						break;
					case Keys.Left:
						Playhead -= 1f;
						e.Handled = true;
						break;
					case Keys.Right:
						Playhead -= 1f;
						e.Handled = true;
						break;
					case Keys.F11:
						if(this.WindowState != FormWindowState.Maximized)
						{
							this.WindowState = FormWindowState.Maximized;
							e.Handled = true;
						}
						break;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnKeyPress																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the KeyPress event.
		/// </summary>
		/// <param name="e">
		/// KeyPress event arguments.
		/// </param>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if(!e.Handled)
			{
				switch(e.KeyChar)
				{
					case '\x1B':
						if(this.WindowState == FormWindowState.Maximized)
						{
							this.WindowState = FormWindowState.Normal;
							e.Handled = true;
						}
						break;
					case ' ':
						mnuTransportPlayPause_Click(this, e);
						e.Handled = true;
						break;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLoad																																*
		//*-----------------------------------------------------------------------*
		//	Sample file: C:\Users\Daniel\OneDrive - Ascendant\Documents\Videos\
		//	Blender\BlenRig5\Tutorial1\BlenRig5T01-01.mp4
		/// <summary>
		/// Raises the Load event when the form has been loaded and is ready to
		/// display for the first time.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			mHost = new ElementHost();
			mHost.Dock = DockStyle.Fill;

			mMediaPlayer = new MediaPlayerWPFUC.MediaPlayerWPF();
			mMediaPlayer.Player.MediaOpened += player_MediaOpened;
			mMediaPlayer.SelectionEndTextChanged +=
				mMediaPlayer_SelectionEndTextChanged;
			mMediaPlayer.SelectionStartTextChanged +=
				mMediaPlayer_SelectionStartTextChanged;
			mHost.Child = mMediaPlayer;

			pnlMedia.Controls.Add(mHost);

			captionEditor.UpdateWidth();

			mMediaPlayer.Caption = "No file open...";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseDown																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseDown event when a mouse button has been depressed.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(Cursor == Cursors.SizeNS)
			{
				mGripBottom = true;
				mGripRight = false;
				mGripLocation = new Point(mMouseUniversal.X, mMouseUniversal.Y);
				mGripSize = new Size(this.Width, this.Height);
			}
			else if(Cursor == Cursors.SizeNWSE)
			{
				mGripBottom = true;
				mGripRight = true;
				mGripLocation = new Point(mMouseUniversal.X, mMouseUniversal.Y);
				mGripSize = new Size(this.Width, this.Height);
			}
			else if(Cursor == Cursors.SizeWE)
			{
				mGripBottom = false;
				mGripRight = true;
				mGripLocation = new Point(mMouseUniversal.X, mMouseUniversal.Y);
				mGripSize = new Size(this.Width, this.Height);
			}
			base.OnMouseDown(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseMove																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseMove event when the mouse has moved over the form.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			int height = 0;
			int width = 0;

			if(IsOverForm(mMouseUniversal) && mMouseLocation.Y > titleBar.Height)
			{
				//	The mouse is below the title bar on the main form.
				{
					if(mMouseLocation.X >= this.Width - cGrip &&
						mMouseLocation.Y >= this.Height - cGrip)
					{
						this.Cursor = Cursors.SizeNWSE;
					}
					else if(mMouseLocation.X >= this.Width - cGrip)
					{
						this.Cursor = Cursors.SizeWE;
					}
					else if(mMouseLocation.Y >= this.Height - cGrip)
					{
						this.Cursor = Cursors.SizeNS;
					}
					else if(this.Cursor != Cursors.Default)
					{
						this.Cursor = Cursors.Default;
					}
				}
			}
			if(mMouseDown && (mGripRight || mGripBottom))
			{
				if(mGripRight)
				{
					width = mGripSize.Width + (mMouseUniversal.X - mGripLocation.X);
					if(width < 150)
					{
						width = 150;
					}
				}
				else
				{
					width = mGripSize.Width;
				}
				if(mGripBottom)
				{
					height = mGripSize.Height + (mMouseUniversal.Y - mGripLocation.Y);
					if(height < 150)
					{
						height = 150;
					}
				}
				else
				{
					height = mGripSize.Height;
				}
				this.Size = new Size(width, height);
			}
			base.OnMouseMove(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseUp																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseUp event when a mouse button has been released.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if(mGripBottom || mGripRight)
			{
				mGripBottom = false;
				mGripRight = false;
				this.Cursor = Cursors.Default;
			}
			base.OnMouseDown(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnPaint																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Paint event when the form is being painted.
		/// </summary>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnPlayheadChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the PlayheadChanged event when the value of Playhead has
		/// changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnPlayheadChanged(EventArgs e)
		{
			PlayheadChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnTransportStateChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the TransportStateChanged event when the value of the
		/// TransportState property has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnTransportStateChanged(EventArgs e)
		{
			TransportStateChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmMain Item.
		/// </summary>
		public frmMain()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			//this.SetStyle(ControlStyles.ResizeRedraw, true);

			//	This technique re-establishes the form shadow on borderless form.
			(new DropFormShadow()).ApplyShadows(this);

			titleBar.IconImage = ResourceMain.CaptionAllIcon32;
			//menuStrip.Cursor = Cursors.SizeAll;
			menuStrip.MouseDown += menuStrip_MouseDown;
			menuStrip.MouseMove += menuStrip_MouseMove;
			menuStrip.MouseUp += menuStrip_MouseUp;

			mUndoStack.CollectionChanged += mUndoStack_CollectionChanged;

			//captionEditor.Captions.Add(new CaptionItem()
			//{
			//	EntryType = CaptionEntryTypeEnum.Space,
			//	Width = 1d,
			//	X = 0d
			//});
			//captionEditor.Captions.Add(new CaptionItem()
			//{
			//	EntryType = CaptionEntryTypeEnum.Normal,
			//	Text = "New caption",
			//	Width = 2d,
			//	X = 1d
			//});

			//	Configure the context menu to follow its driver items.
			ctxCaptionProperties.Click += ctxCaptionProperties_Click;
			ctxCaptionText.Click += ctxCaptionText_Click;
			ctxCaptionWidth.Click += ctxCaptionWidth_Click;
			ctxDeleteCaption.Click += ctxDeleteCaption_Click;
			ctxInsertCaption.Click += ctxInsertCaption_Click;
			ctxInsertSpace.Click += ctxInsertSpace_Click;
			ctxMergeCaptions.Click += ctxMergeCaptions_Click;
			ctxRemoveSpace.Click += ctxRemoveSpace_Click;
			ctxSelectNone.Click += ctxSelectNone_Click;
			ctxSelectTimeFromCaption.Click += ctxSelectTimeFromCaption_Click;
			ctxSnapCaptionToSelection.Click += ctxSnapCaptionToSelection_Click;
			ctxToggleCaption.Click += ctxToggleCaption_Click;

			//captionEditor.ContextMenuStrip = ctxMenuCaption;
			this.ContextMenuStrip = ctxMenuCaption;
			captionEditor.Captions.CollectionChanged +=
				captions_CollectionChanged;
			captionEditor.Captions.ItemPropertyChanged +=
				captions_ItemPropertyChanged;
			captionEditor.Captions.ItemPropertyChanging +=
				captions_ItemPropertyChanging;

			captionEditor.CaptionDoubleClick += captionEditor_CaptionDoubleClick;
			captionEditor.PlayheadChanged += captionEditor_PlayheadChanged;
			captionEditor.SelectionEndChanged += captionEditor_SelectionEndChanged;
			captionEditor.SelectionStartChanged +=
				captionEditor_SelectionStartChanged;
			captionEditor.WaveformReady += captionEditor_WaveformReady;

			mnuEditCaptionProperties.Click += mnuEditCaptionProperties_Click;
			mnuEditCaptionProperties.EnabledChanged +=
				mnuEditCaptionProperties_EnabledChanged;
			mnuEditCaptionText.Click += mnuEditCaptionText_Click;
			mnuEditCaptionText.EnabledChanged += mnuEditCaptionText_EnabledChanged;
			mnuEditCaptionWidth.Click += mnuEditCaptionWidth_Click;
			mnuEditCaptionWidth.EnabledChanged += mnuEditCaptionWidth_EnabledChanged;
			mnuEditCurrentTime.Click += mnuEditCurrentTime_Click;
			mnuEditDeleteCaption.Click += mnuEditDeleteCaption_Click;
			mnuEditDeleteCaption.EnabledChanged += mnuEditDeleteCaption_EnabledChanged;
			mnuEditExtendCaptionTails.Click += mnuEditExtendCaptionTails_Click;
			mnuEditFind.Click += mnuEditFind_Click;
			mnuEditFindAgain.Click += mnuEditFindAgain_Click;
			mnuEditFindAndReplace.Click += mnuEditFindAndReplace_Click;
			mnuEditInsertCaption.Click += mnuEditInsertCaption_Click;
			mnuEditInsertCaption.EnabledChanged += mnuEditInsertCaption_EnabledChanged;
			mnuEditInsertSpace.Click += mnuEditInsertSpace_Click;
			mnuEditInsertSpace.EnabledChanged += mnuEditInsertSpace_EnabledChanged;
			mnuEditMergeCaptions.Click += mnuEditMergeCaptions_Click;
			mnuEditMergeCaptions.EnabledChanged += mnuEditMergeCaptions_EnabledChanged;
			mnuEditMergeCaptions.Enabled = false;
			mnuEditMoveContentToTime.Click += mnuEditMoveContentToTime_Click;
			mnuEditMoveLeftToTime.Click += mnuEditMoveLeftToTime_Click;
			mnuEditConvertMultiSingle.Click += mnuEditConvertMultiSingle_Click;
			mnuEditRemoveSpace.Click += mnuEditRemoveSpace_Click;
			mnuEditRemoveSpace.EnabledChanged += mnuEditRemoveSpace_EnabledChanged;
			mnuEditRemoveSpace.Enabled = false;
			mnuEditRippleOnOff.Click += mnuEditRippleOnOff_Click;
			mnuEditRippleOnOff.CheckedChanged += mnuEditRippleOnOff_CheckedChanged;
			mnuEditSelectionEndTime.Click += mnuEditSelectionEndTime_Click;
			mnuEditSelectionStartTime.Click += mnuEditSelectionStartTime_Click;
			mnuEditSelectNone.Click += mnuEditSelectNone_Click;
			mnuEditSelectNone.EnabledChanged += mnuEditSelectNone_EnabledChanged;
			mnuEditSelectTimeFromCaption.Click += mnuEditSelectTimeFromCaption_Click;
			mnuEditSelectTimeFromCaption.EnabledChanged +=
				mnuEditSelectTimeFromCaption_EnabledChanged;
			mnuEditSetTrackDuration.Click += mnuEditSetTrackDuration_Click;
			mnuEditSnapCaptionToSelection.Click +=
				mnuEditSnapCaptionToSelection_Click;
			mnuEditSnapCaptionToSelection.EnabledChanged +=
				mnuEditSnapCaptionToSelection_EnabledChanged;
			mnuEditSpeedDialList.Click += mnuEditSpeedDialList_Click;
			mnuEditToggleCaptionSpace.Click += mnuEditToggleCaptionSpace_Click;
			mnuEditToggleCaptionSpace.EnabledChanged +=
				mnuEditToggleCaptionSpace_EnabledChanged;
			mnuEditUndo.Click += mnuEditUndo_Click;

			mnuExportCaptionsSRT.Click += mnuExportCaptionsSRT_Click;

			mnuFileExit.Click += mnuFileExit_Click;
			mnuFileExportTranscriptText.Click += mnuFileExportTranscriptText_Click;
			mnuFileLoadProject.Click += mnuFileLoadProject_Click;
			mnuFileOpenCaptions.Click += mnuFileOpenCaptions_Click;
			mnuFileOpenMedia.Click += mnuFileOpenMedia_Click;
			mnuFileSaveCaptions.Click += mnuFileSaveCaptions_Click;
			mnuFileSaveCaptions.EnabledChanged += mnuFileSaveCaptions_EnabledChanged;
			mnuFileSaveCaptionsAs.Click += mnuFileSaveCaptionsAs_Click;
			mnuFileSaveProject.Click += mnuFileSaveProject_Click;
			mnuFileSaveProjectAs.Click += mnuFileSaveProjectAs_Click;

			mnuHelpAbout.Click += mnuHelpAbout_Click;

			mnuToolsAlignScriptToCaptions.Click += mnuToolsAlignScriptToCaptions_Click;
			mnuToolsDisplayVTTInfo.Click += mnuToolsDisplayVTTInfo_Click;
			mnuToolsExportMSBurstsExcel.Click += mnuToolsExportMSBurstsExcel_Click;
			mnuToolsLongSoundCount.Click += mnuToolsLongSoundCount_Click;
			//mnuToolsMoveContent.Click += mnuToolsMoveContent_Click;
			mnuToolsReWrapStreamVTT.Click += mnuToolsReWrapStreamVTT_Click;
			mnuToolsShortSoundCount.Click += mnuToolsShortSoundCount_Click;
			mnuToolsUpdateTargetFromSource.Click += mnuToolsUpdateTargetFromSource_Click;

			mnuTransportGoBack.Click += mnuTransportGoBack_Click;
			mnuTransportGoBack.EnabledChanged += mnuTransportGoBack_EnabledChanged;
			mnuTransportGoForward.Click += mnuTransportGoForward_Click;
			mnuTransportGoForward.EnabledChanged +=
				mnuTransportGoForward_EnabledChanged;
			mnuTransportGoToEnd.Click += mnuTransportGoToEnd_Click;
			mnuTransportGoToEnd.EnabledChanged += mnuTransportGoToEnd_EnabledChanged;
			mnuTransportGoToStart.Click += mnuTransportGoToStart_Click;
			mnuTransportGoToStart.EnabledChanged +=
				mnuTransportGoToStart_EnabledChanged;
			mnuTransportPlayPause.Click += mnuTransportPlayPause_Click;
			mnuTransportPlayPause.EnabledChanged +=
				mnuTransportPlayPause_EnabledChanged;
			mnuTransportStop.Click += mnuTransportStop_Click;
			mnuTransportStop.EnabledChanged += mnuTransportStop_EnabledChanged;

			mnuViewCenterCursor.Click += mnuViewCenterCursor_Click;

			mTimer.Interval = 100;
			mTimer.Tick += mTimer_Tick;
			mTimer.Start();

			tbtnEditRippleOnOff.Click += tbtnEditRippleOnOff_Click;
			tbtnTransportGoBack.Click += tbtnTransportGoBack_Click;
			tbtnTransportGoForward.Click += tbtnTransportGoForward_Click;
			tbtnTransportGoToEnd.Click += tbtnTransportGoToEnd_Click;
			tbtnTransportGoToStart.Click += tbtnTransportGoToStart_Click;
			tbtnTransportPlayPause.Click += tbtnTransportPlayPause_Click;
			tbtnTransportStop.Click += tbtnTransportStop_Click;

			scrollTimeline.AttachCanvasControl(captionEditor);
			scrollTimeline.ScrollWheelAssignment =
				ScrollPanelVirtual.ScrollWheelAssignmentEnum.NPanSScrollCZoom;
			scrollTimeline.SetScrollSpeed(10f);
			scrollTimeline.ValueChanged += scrollTimeline_ValueChanged;
			scrollTimeline.VerticalScrollEnabled = false;

			statusStrip.SizingGrip = false;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CaptionFilename																												*
		//*-----------------------------------------------------------------------*
		private string mCaptionFilename = "";
		/// <summary>
		/// Get/Set the filename of the loaded caption file.
		/// </summary>
		public string CaptionFilename
		{
			get { return mCaptionFilename; }
			set { mCaptionFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Duration																															*
		//*-----------------------------------------------------------------------*
		private double mDuration = 0f;
		/// <summary>
		/// Get/Set the duration of the media, in decimal seconds.
		/// </summary>
		public double Duration
		{
			get { return mDuration; }
			set { mDuration = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MediaFilename																													*
		//*-----------------------------------------------------------------------*
		private string mMediaFilename = "";
		/// <summary>
		/// Get/Set the filename of the loaded media file.
		/// </summary>
		public string MediaFilename
		{
			get { return mMediaFilename; }
			set { mMediaFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Playhead																															*
		//*-----------------------------------------------------------------------*
		private double mPlayhead = 0f;
		/// <summary>
		/// Get/Set the current playhead, in decimal seconds.
		/// </summary>
		public double Playhead
		{
			get { return mPlayhead; }
			set
			{
				bool changed = (mPlayhead != value);
				int pixels = 0;

				mPlayhead = Math.Max(value, 0d);

				if(!mMediaBusy)
				{
					mMediaPlayer.Position = ToTimeSpan(mPlayhead);
				}
				if(changed)
				{
					if(TransportState == TransportStateEnum.Play)
					{
						pixels = (int)(mPlayhead * captionEditor.PixelsPerSecond);
						scrollTimeline.Invoke((MethodInvoker)delegate
						{
							if(pixels > scrollTimeline.ScrollPositionH +
								scrollTimeline.CanvasWidth)
							{
								scrollTimeline.ScrollPositionH = pixels - 32;
							}
						});
					}
					mPlayheadBusy = true;
					captionEditor.Playhead = mPlayhead;
					Time(value);
					OnPlayheadChanged(new EventArgs());
					mPlayheadBusy = false;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlayheadChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the value of the playhead changes.
		/// </summary>
		public event EventHandler PlayheadChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ProjectFilename																												*
		//*-----------------------------------------------------------------------*
		private string mProjectFilename = "";
		/// <summary>
		/// Get/Set the filename of the open project.
		/// </summary>
		public string ProjectFilename
		{
			get { return mProjectFilename; }
			set { mProjectFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ProjectInfo																														*
		//*-----------------------------------------------------------------------*
		private CaptionProjectItem mProjectInfo = new CaptionProjectItem();
		/// <summary>
		/// Get/Set a reference to the loaded project information.
		/// </summary>
		public CaptionProjectItem ProjectInfo
		{
			get { return mProjectInfo; }
			set { mProjectInfo = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TransportState																												*
		//*-----------------------------------------------------------------------*
		private TransportStateEnum mTransportState = TransportStateEnum.Stop;
		/// <summary>
		/// Get/Set the current transport state.
		/// </summary>
		public TransportStateEnum TransportState
		{
			get { return mTransportState; }
			set
			{
				mTransportState = value;
				this.Invoke((MethodInvoker)delegate
				{
					//	Run in UI thread every time.
					//	Event is unconditionally raised on Set.
					mnuTransportGoToStart.Enabled = true;
					mnuTransportGoToEnd.Enabled = true;

					OnTransportStateChanged(new EventArgs());
					switch(mTransportState)
					{
						case TransportStateEnum.Pause:
							mnuTransportGoBack.Enabled = true;
							mnuTransportGoForward.Enabled = true;
							//mnuTransportGoToEnd.Enabled = true;
							//mnuTransportGoToStart.Enabled = true;
							mnuTransportPlayPause.Enabled = true;
							mnuTransportStop.Enabled = true;
							mnuTransportPlayPause.Image = ResourceMain.Play32;
							tbtnTransportPlayPause.Image = ResourceMain.Play32;
							Status("Paused...");
							break;
						case TransportStateEnum.Play:
							mnuTransportGoBack.Enabled = true;
							mnuTransportGoForward.Enabled = true;
							//mnuTransportGoToEnd.Enabled = true;
							//mnuTransportGoToStart.Enabled = true;
							mnuTransportPlayPause.Enabled = true;
							mnuTransportStop.Enabled = true;
							mnuTransportPlayPause.Image = ResourceMain.Pause32;
							tbtnTransportPlayPause.Image = ResourceMain.Pause32;
							Status("Playing...");
							break;
						case TransportStateEnum.Stop:
							mnuTransportGoBack.Enabled = false;
							mnuTransportGoForward.Enabled = (Duration > 0f);
							//mnuTransportGoToEnd.Enabled = (Duration > 0f);
							//mnuTransportGoToStart.Enabled = false;
							mnuTransportPlayPause.Enabled = (Duration > 0f);
							mnuTransportStop.Enabled = false;
							mnuTransportPlayPause.Image = ResourceMain.Play32;
							tbtnTransportPlayPause.Image = ResourceMain.Play32;
							Playhead = 0f;
							Status("Stopped...");
							break;
						case TransportStateEnum.None:
						default:
							mnuTransportGoBack.Enabled = false;
							mnuTransportGoForward.Enabled = false;
							//mnuTransportGoToEnd.Enabled = false;
							//mnuTransportGoToStart.Enabled = false;
							mnuTransportPlayPause.Enabled = false;
							mnuTransportStop.Enabled = false;
							mnuTransportPlayPause.Image = ResourceMain.Play32;
							tbtnTransportPlayPause.Image = ResourceMain.Play32;
							break;
					}
				});
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransportStateChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the TransportState property has changed.
		/// </summary>
		public event EventHandler TransportStateChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UndoStack																															*
		//*-----------------------------------------------------------------------*
		private UndoCollection mUndoStack = new UndoCollection();
		/// <summary>
		/// Get a reference to the collection of undo items.
		/// </summary>
		public UndoCollection UndoStack
		{
			get { return mUndoStack; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
