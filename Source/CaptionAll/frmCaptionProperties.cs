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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CaptionBubbleEditorWF;

using static CaptionAll.CaptionAllUtil;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	frmCaptionProperties																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Caption properties dialog.
	/// </summary>
	public partial class frmCaptionProperties : CenteredDialog
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private bool mActivated = false;

		//*-----------------------------------------------------------------------*
		//* btnSpeedDial_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Speed Dial button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnSpeedDial_Click(object sender, EventArgs e)
		{
			frmSpeedDial dialog = new frmSpeedDial();

			if(dialog.ShowDialog() == DialogResult.OK)
			{
				lblSpeedDialList.Text = GetSpeedDialText();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CalcEnd																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Calculate the end time of the caption, based upon the start time and
		/// duration.
		/// </summary>
		private void CalcEnd()
		{
			txtEnd.Text = FormatTimeSpan(
				ToTimeSpan(txtTime.Text).Add(ToTimeSpan(txtDuration.Text)));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* chkBlankSpace_CheckedChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The state of the blank space checkbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void chkBlankSpace_CheckedChanged(object sender, EventArgs e)
		{
			txtText.Enabled = !chkBlankSpace.Checked;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* LimitX																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Limit the time and duration amounts to the minimum and maximum limits,
		/// respectively.
		/// </summary>
		private void LimitX()
		{
			double w = 0d;
			double x = 0d;

			x = ToTimeSpan(txtTime.Text).TotalSeconds;
			w = ToTimeSpan(txtDuration.Text).TotalSeconds;
			if(x < mMinimumX || x + w > mMaximumX)
			{
				x = mMinimumX;
				if(x + w > mMaximumX)
				{
					w = mMaximumX - x;
				}
				txtTime.Text = FormatTimeSpan(ToTimeSpan(x));
				txtDuration.Text = FormatTimeSpan(ToTimeSpan(w));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtDuration_GotFocus																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The duration textbox has received focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtDuration_GotFocus(object sender, EventArgs e)
		{
			txtDuration.SelectAll();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtDuration_LostFocus																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The duration textbox has lost focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtDuration_LostFocus(object sender, EventArgs e)
		{
			txtDuration.Text = FormatTimeSpan(txtDuration.Text);
			LimitX();
			CalcEnd();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtText_GotFocus																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The caption textbox has received the focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtText_GotFocus(object sender, EventArgs e)
		{
			txtText.SelectAll();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtText_TextChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the textbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtText_TextChanged(object sender, EventArgs e)
		{
			txtCharacterCount.Text = txtText.Text.Length.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtTime_GotFocus																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The time textbox has received the focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtTime_GotFocus(object sender, EventArgs e)
		{
			txtTime.SelectAll();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtTime_LostFocus																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The time textbox has lost the focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtTime_LostFocus(object sender, EventArgs e)
		{
			txtTime.Text = FormatTimeSpan(txtTime.Text);
			LimitX();
			CalcEnd();
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* btnOK_Click																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The OK button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void btnOK_Click(object sender, EventArgs e)
		{
			if(mCaption != null)
			{
				mCaption.EntryType = (chkBlankSpace.Checked == true ?
					CaptionEntryTypeEnum.Space : CaptionEntryTypeEnum.Normal);
				mCaption.Text = txtText.Text;
				mCaption.Width = ToTimeSpan(txtDuration.Text).TotalSeconds;
				mCaption.X = ToTimeSpan(txtTime.Text).TotalSeconds;
			}
			base.btnOK_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnActivated																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Activated event when the form has been activated.
		/// </summary>
		/// <param name="e">
		/// Event arguments.
		/// </param>
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if(!mActivated)
			{
				mActivated = true;
				lblSpeedDialList.Text = GetSpeedDialText();
				txtText.Focus();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnKeyDown																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the KeyDown event when a key has been depressed.
		/// </summary>
		/// <param name="e">
		/// Key event arguments.
		/// </param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			InsertSpeedDialValue(txtText, e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmCaptionProperties Item.
		/// </summary>
		public frmCaptionProperties()
		{
			InitializeComponent();
			this.KeyPreview = true;

			btnSpeedDial.Click += btnSpeedDial_Click;
			chkBlankSpace.CheckedChanged += chkBlankSpace_CheckedChanged;
			txtDuration.GotFocus += txtDuration_GotFocus;
			txtDuration.LostFocus += txtDuration_LostFocus;
			txtText.TextChanged += txtText_TextChanged;
			txtText.GotFocus += txtText_GotFocus;
			txtTime.GotFocus += txtTime_GotFocus;
			txtTime.LostFocus += txtTime_LostFocus;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BlankSpaceCheckBoxEnabled																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="BlankSpaceCheckBoxEnabled">BlankSpaceCheckBoxEnabled</see>.
		/// </summary>
		private bool mBlankSpaceCheckBoxEnabled = true;
		/// <summary>
		/// Get/Set a value indicating whether the blank space checkbox is enabled.
		/// </summary>
		public bool BlankSpaceCheckBoxEnabled
		{
			get { return mBlankSpaceCheckBoxEnabled; }
			set
			{
				mBlankSpaceCheckBoxEnabled = value;
				chkBlankSpace.Enabled = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Caption																																*
		//*-----------------------------------------------------------------------*
		private CaptionItem mCaption = null;
		/// <summary>
		/// Get/Set a reference to the caption for which the properties are being
		/// edited.
		/// </summary>
		public CaptionItem Caption
		{
			get { return mCaption; }
			set
			{
				mCaption = value;
				if(mCaption != null)
				{
					chkBlankSpace.Checked =
						(mCaption.EntryType == CaptionEntryTypeEnum.Space);
					txtTime.Text = FormatTimeSpan(ToTimeSpan(mCaption.X));
					txtDuration.Text = FormatTimeSpan(ToTimeSpan(mCaption.Width));
					txtText.Text = mCaption.Text;
					LimitX();
					CalcEnd();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DurationEnabled																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="DurationEnabled">DurationEnabled</see>.
		/// </summary>
		private bool mDurationEnabled = true;
		/// <summary>
		/// Get/Set a value indicating whether the duration textbox is enabled.
		/// </summary>
		public bool DurationEnabled
		{
			get { return mDurationEnabled; }
			set
			{
				mDurationEnabled = value;
				txtDuration.ReadOnly = (!value);
				txtDuration.TabStop = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MaximumX																															*
		//*-----------------------------------------------------------------------*
		private double mMaximumX = 0d;
		/// <summary>
		/// Get/Set the maximum X value of the caption.
		/// </summary>
		public double MaximumX
		{
			get { return mMaximumX; }
			set
			{
				mMaximumX = value;
				LimitX();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MinimumX																															*
		//*-----------------------------------------------------------------------*
		private double mMinimumX = 0d;
		/// <summary>
		/// Get/Set the minimum X value of the caption.
		/// </summary>
		public double MinimumX
		{
			get { return mMinimumX; }
			set
			{
				mMinimumX = value;
				LimitX();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartTimeEnabled																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="StartTimeEnabled">StartTimeEnabled</see>.
		/// </summary>
		private bool mStartTimeEnabled = true;
		/// <summary>
		/// Get/Set a value indicating whether the start time textbox is enabled.
		/// </summary>
		public bool StartTimeEnabled
		{
			get { return mStartTimeEnabled; }
			set
			{
				mStartTimeEnabled = value;
				txtTime.ReadOnly = (!value);
				txtTime.TabStop = value;
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
