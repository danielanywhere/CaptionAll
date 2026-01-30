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
	//*	frmTextEdit																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Basic text editing form.
	/// </summary>
	public partial class frmText : CenteredDialog
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
		//* text_GotFocus																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The textbox received the focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void text_GotFocus(object sender, EventArgs e)
		{
			text.SelectAll();
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
				text.Focus();
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
			InsertSpeedDialValue(text, e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmTextEdit Item.
		/// </summary>
		public frmText()
		{
			InitializeComponent();
			this.KeyPreview = true;

			btnSpeedDial.Click += btnSpeedDial_Click;
			text.GotFocus += text_GotFocus;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CaptionText																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the user's text content.
		/// </summary>
		public string CaptionText
		{
			get { return text.Text; }
			set { text.Text = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
