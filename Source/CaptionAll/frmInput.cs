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
	//*	frmInput																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// General text input form.
	/// </summary>
	public partial class frmInput : CenteredDialog
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
		/// Create a new instance of the frmInput Item.
		/// </summary>
		public frmInput()
		{
			InitializeComponent();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Prompt																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the prompt text.
		/// </summary>
		public string Prompt
		{
			get { return lblPrompt.Text; }
			set { lblPrompt.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserText																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the user text.
		/// </summary>
		public string UserText
		{
			get { return txtUserText.Text; }
			set { txtUserText.Text = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
