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
using System.Windows.Forms;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	CenteredDialog																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A dialog form that will typically be centered over the parent.
	/// </summary>
	public class CenteredDialog : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* btnCancel_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The cancel button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			if(this.Modal)
			{
				this.Hide();
			}
			else
			{
				this.Close();
			}
		}
		//*-----------------------------------------------------------------------*

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
		protected virtual void btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			if(this.Modal)
			{
				this.Hide();
			}
			else
			{
				this.Close();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Dispose																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">
		/// True if managed resources should be disposed; otherwise, false.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InitializeComponent																										*
		//*-----------------------------------------------------------------------*
		protected virtual void InitializeComponent()
		{

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
			if(!e.Handled)
			{
				switch(e.KeyCode)
				{
					case Keys.Enter:
						if(e.Modifiers == Keys.None)
						{
							//	Raw Enter key.
							btnOK_Click(this, new EventArgs());
						}
						break;
					case Keys.Escape:
						//	Escape key.
						btnCancel_Click(this, new EventArgs());
						break;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLoad																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fires the Load event when the form has loaded and is ready to display
		/// for the first time.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLoad(EventArgs e)
		{
			Control[] controls = null;

			base.OnLoad(e);

			this.StartPosition = FormStartPosition.CenterScreen;

			controls = this.Controls.Find("btnOK", true);
			if(controls?.Length > 0)
			{
				controls[0].Click += btnOK_Click;
			}
			controls = this.Controls.Find("btnCancel", true);
			if(controls?.Length > 0)
			{
				controls[0].Click += btnCancel_Click;
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the CenteredDialog Item.
		/// </summary>
		public CenteredDialog()
		{
			InitializeComponent();
			this.KeyPreview = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Owner																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set a reference to the owner form of this instance.
		/// </summary>
		public new Form Owner
		{
			get { return base.Owner; }
			set
			{
				base.Owner = value;
				if(value != null)
				{
					this.StartPosition = FormStartPosition.CenterParent;
				}
				else
				{
					this.StartPosition = FormStartPosition.CenterScreen;
				}
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
