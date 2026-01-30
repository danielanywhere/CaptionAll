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

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	frmFindReplace																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Find and replace form.
	/// </summary>
	public partial class frmFindReplace : CenteredDialog
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* btnFind_Click																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Find button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnFind_Click(object sender, EventArgs e)
		{
			OnFindReplaceRequest(new FindReplaceEventArgs()
			{
				FindText = txtFind.Text,
				ReplacementText = txtReplaceWith.Text,
				RequestType = FindReplaceRequestTypeEnum.FindFirst
			});
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnFindNext_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Find Next button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnFindNext_Click(object sender, EventArgs e)
		{
			OnFindReplaceRequest(new FindReplaceEventArgs()
			{
				FindText = txtFind.Text,
				ReplacementText = txtReplaceWith.Text,
				RequestType = FindReplaceRequestTypeEnum.FindNext
			});
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnReplace_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Replace button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnReplace_Click(object sender, EventArgs e)
		{
			OnFindReplaceRequest(new FindReplaceEventArgs()
			{
				FindText = txtFind.Text,
				ReplacementText = txtReplaceWith.Text,
				RequestType = FindReplaceRequestTypeEnum.ReplaceCurrent
			});
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnReplaceAll_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Replace All button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnReplaceAll_Click(object sender, EventArgs e)
		{
			OnFindReplaceRequest(new FindReplaceEventArgs()
			{
				FindText = txtFind.Text,
				ReplacementText = txtReplaceWith.Text,
				RequestType = FindReplaceRequestTypeEnum.ReplaceAll
			});
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnFindReplaceRequest																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the FindReplaceRequest event when a request is being made to
		/// handle a find and replace activity.
		/// </summary>
		/// <param name="e">
		/// Find and replace event arguments.
		/// </param>
		protected virtual void OnFindReplaceRequest(FindReplaceEventArgs e)
		{
			FindReplaceRequest?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmFindReplace Item.
		/// </summary>
		public frmFindReplace()
		{
			InitializeComponent();

			btnFind.Click += btnFind_Click;
			btnFindNext.Click += btnFindNext_Click;
			btnReplace.Click += btnReplace_Click;
			btnReplaceAll.Click += btnReplaceAll_Click;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FindReplaceRequest																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A request is being made to handle a find and replace action.
		/// </summary>
		public event FindReplaceEventHandler FindReplaceRequest;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FindText																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the text to find.
		/// </summary>
		public string FindText
		{
			get { return txtFind.Text; }
			set { txtFind.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ReplacementText																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the replacement text.
		/// </summary>
		public string ReplacementText
		{
			get { return txtReplaceWith.Text; }
			set { txtReplaceWith.Text = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
