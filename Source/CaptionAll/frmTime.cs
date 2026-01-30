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
	//*	frmCurrentTime																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Edit the current time.
	/// </summary>
	public partial class frmTime : CenteredDialog
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
		/// Create a new instance of the frmCurrentTime Item.
		/// </summary>
		public frmTime()
		{
			InitializeComponent();
			btnCancel.Click += btnCancel_Click;
			btnOK.Click += btnOK_Click;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Prompt																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the user prompt text.
		/// </summary>
		public string Prompt
		{
			get { return lblTime.Text; }
			set { lblTime.Text = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TimeText																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the current time text.
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
