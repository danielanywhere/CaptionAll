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

using static CaptionAll.CaptionAllUtil;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	frmTimeText																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Dialog form for noting both time and text.
	/// </summary>
	public partial class frmTimeText : CenteredDialog
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
		//* txtText_GotFocus																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The user textbox has received focus.
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
		//* txtTime_LostFocus																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The time textbox has lost focus.
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
		/// Create a new instance of the frmTimeText Item.
		/// </summary>
		public frmTimeText()
		{
			InitializeComponent();
			this.KeyPreview = true;

			btnSpeedDial.Click += btnSpeedDial_Click;
			txtText.GotFocus += txtText_GotFocus;
			txtTime.LostFocus += txtTime_LostFocus;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CaptionPrompt																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the prompt for the user text.
		/// </summary>
		public string CaptionPrompt
		{
			get { return lblText.Text; }
			set { lblText.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CaptionText																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the current user text.
		/// </summary>
		public string UserText
		{
			get { return txtText.Text; }
			set { txtText.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TimePrompt																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the prompt for the time.
		/// </summary>
		public string TimePrompt
		{
			get { return lblTime.Text; }
			set { lblTime.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TimeText																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the current time-specific text.
		/// </summary>
		public string TimeText
		{
			get { return txtTime.Text; }
			set { txtTime.Text = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
