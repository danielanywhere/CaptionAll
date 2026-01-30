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
	//*	frmSpeedDial																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Speed-dial form for commonly-used words.
	/// </summary>
	public partial class frmSpeedDial : CenteredDialog
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private List<TextBox> mTextboxes = new List<TextBox>();

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
			int index = 0;

			CommonlyUsedWords.Clear();

			for(index = 0; index < mTextboxes.Count; index ++)
			{
				CommonlyUsedWords.Add(mTextboxes[index].Text);
			}

			base.btnOK_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLoad																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Load event when the form has been loaded and is ready to
		/// display for the first time.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLoad(EventArgs e)
		{
			int count = Math.Min(CommonlyUsedWords.Count, mTextboxes.Count);
			int index = 0;

			for(index = 0; index < count; index ++)
			{
				mTextboxes[index].Text = CommonlyUsedWords[index];
			}
			base.OnLoad(e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmSpeedDial Item.
		/// </summary>
		public frmSpeedDial()
		{
			InitializeComponent();

			mTextboxes.Add(txt1);
			mTextboxes.Add(txt2);
			mTextboxes.Add(txt3);
			mTextboxes.Add(txt4);
			mTextboxes.Add(txt5);
			mTextboxes.Add(txt6);
			mTextboxes.Add(txt7);
			mTextboxes.Add(txt8);
			mTextboxes.Add(txt9);
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
