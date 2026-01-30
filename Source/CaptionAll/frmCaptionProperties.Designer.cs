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

namespace CaptionAll
{
	partial class frmCaptionProperties
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private new void InitializeComponent()
		{
			lblText = new System.Windows.Forms.Label();
			btnCancel = new System.Windows.Forms.Button();
			btnOK = new System.Windows.Forms.Button();
			txtText = new System.Windows.Forms.TextBox();
			txtTime = new System.Windows.Forms.TextBox();
			lblTime = new System.Windows.Forms.Label();
			lblDuration = new System.Windows.Forms.Label();
			txtDuration = new System.Windows.Forms.TextBox();
			lblEnd = new System.Windows.Forms.Label();
			txtEnd = new System.Windows.Forms.TextBox();
			chkBlankSpace = new System.Windows.Forms.CheckBox();
			btnSpeedDial = new System.Windows.Forms.Button();
			lblSpeedDialListTitle = new System.Windows.Forms.Label();
			lblSpeedDialList = new System.Windows.Forms.Label();
			lblCharacterCount = new System.Windows.Forms.Label();
			txtCharacterCount = new System.Windows.Forms.TextBox();
			SuspendLayout();
			// 
			// lblText
			// 
			lblText.AutoSize = true;
			lblText.Location = new System.Drawing.Point(12, 117);
			lblText.Name = "lblText";
			lblText.Size = new System.Drawing.Size(93, 20);
			lblText.TabIndex = 9;
			lblText.Text = "Caption &text:";
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(636, 345);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(94, 38);
			btnCancel.TabIndex = 15;
			btnCancel.Text = "&Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOK.Location = new System.Drawing.Point(536, 345);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(94, 38);
			btnOK.TabIndex = 14;
			btnOK.Text = "&OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// txtText
			// 
			txtText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtText.Location = new System.Drawing.Point(12, 140);
			txtText.Multiline = true;
			txtText.Name = "txtText";
			txtText.Size = new System.Drawing.Size(435, 199);
			txtText.TabIndex = 10;
			// 
			// txtTime
			// 
			txtTime.Location = new System.Drawing.Point(118, 12);
			txtTime.Name = "txtTime";
			txtTime.Size = new System.Drawing.Size(103, 27);
			txtTime.TabIndex = 1;
			txtTime.Text = "00:00:00.000";
			// 
			// lblTime
			// 
			lblTime.AutoSize = true;
			lblTime.Location = new System.Drawing.Point(12, 15);
			lblTime.Name = "lblTime";
			lblTime.Size = new System.Drawing.Size(98, 20);
			lblTime.TabIndex = 0;
			lblTime.Text = "&Starting time:";
			// 
			// lblDuration
			// 
			lblDuration.AutoSize = true;
			lblDuration.Location = new System.Drawing.Point(12, 48);
			lblDuration.Name = "lblDuration";
			lblDuration.Size = new System.Drawing.Size(70, 20);
			lblDuration.TabIndex = 2;
			lblDuration.Text = "&Duration:";
			// 
			// txtDuration
			// 
			txtDuration.Location = new System.Drawing.Point(118, 45);
			txtDuration.Name = "txtDuration";
			txtDuration.Size = new System.Drawing.Size(103, 27);
			txtDuration.TabIndex = 3;
			txtDuration.Text = "00:00:00.000";
			// 
			// lblEnd
			// 
			lblEnd.AutoSize = true;
			lblEnd.Location = new System.Drawing.Point(238, 48);
			lblEnd.Name = "lblEnd";
			lblEnd.Size = new System.Drawing.Size(92, 20);
			lblEnd.TabIndex = 6;
			lblEnd.Text = "Ending time:";
			// 
			// txtEnd
			// 
			txtEnd.Location = new System.Drawing.Point(344, 45);
			txtEnd.Name = "txtEnd";
			txtEnd.ReadOnly = true;
			txtEnd.Size = new System.Drawing.Size(103, 27);
			txtEnd.TabIndex = 7;
			txtEnd.TabStop = false;
			txtEnd.Text = "00:00:00.000";
			// 
			// chkBlankSpace
			// 
			chkBlankSpace.AutoSize = true;
			chkBlankSpace.Location = new System.Drawing.Point(12, 90);
			chkBlankSpace.Name = "chkBlankSpace";
			chkBlankSpace.Size = new System.Drawing.Size(179, 24);
			chkBlankSpace.TabIndex = 8;
			chkBlankSpace.Text = "Caption is &blank space";
			chkBlankSpace.UseVisualStyleBackColor = true;
			// 
			// btnSpeedDial
			// 
			btnSpeedDial.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnSpeedDial.Location = new System.Drawing.Point(12, 345);
			btnSpeedDial.Name = "btnSpeedDial";
			btnSpeedDial.Size = new System.Drawing.Size(123, 38);
			btnSpeedDial.TabIndex = 13;
			btnSpeedDial.Text = "S&peed Dial";
			btnSpeedDial.UseVisualStyleBackColor = true;
			// 
			// lblSpeedDialListTitle
			// 
			lblSpeedDialListTitle.AutoSize = true;
			lblSpeedDialListTitle.Location = new System.Drawing.Point(475, 117);
			lblSpeedDialListTitle.Name = "lblSpeedDialListTitle";
			lblSpeedDialListTitle.Size = new System.Drawing.Size(111, 20);
			lblSpeedDialListTitle.TabIndex = 11;
			lblSpeedDialListTitle.Text = "Speed Dial List:";
			// 
			// lblSpeedDialList
			// 
			lblSpeedDialList.Location = new System.Drawing.Point(475, 140);
			lblSpeedDialList.Name = "lblSpeedDialList";
			lblSpeedDialList.Size = new System.Drawing.Size(246, 199);
			lblSpeedDialList.TabIndex = 12;
			lblSpeedDialList.Text = "Ctrl+1:\r\nCtrl+2:\r\nCtrl+3:\r\nCtrl+4:\r\nCtrl+5:\r\nCtrl+6:\r\nCtrl+7:\r\nCtrl+8:\r\nCtrl+9:";
			// 
			// lblCharacterCount
			// 
			lblCharacterCount.AutoSize = true;
			lblCharacterCount.Location = new System.Drawing.Point(238, 15);
			lblCharacterCount.Name = "lblCharacterCount";
			lblCharacterCount.Size = new System.Drawing.Size(81, 20);
			lblCharacterCount.TabIndex = 4;
			lblCharacterCount.Text = "Characters:";
			// 
			// txtCharacterCount
			// 
			txtCharacterCount.Location = new System.Drawing.Point(344, 12);
			txtCharacterCount.Name = "txtCharacterCount";
			txtCharacterCount.ReadOnly = true;
			txtCharacterCount.Size = new System.Drawing.Size(103, 27);
			txtCharacterCount.TabIndex = 5;
			txtCharacterCount.TabStop = false;
			txtCharacterCount.Text = "0";
			// 
			// frmCaptionProperties
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(742, 396);
			Controls.Add(lblSpeedDialList);
			Controls.Add(lblSpeedDialListTitle);
			Controls.Add(chkBlankSpace);
			Controls.Add(lblText);
			Controls.Add(btnCancel);
			Controls.Add(btnSpeedDial);
			Controls.Add(btnOK);
			Controls.Add(txtText);
			Controls.Add(txtCharacterCount);
			Controls.Add(lblCharacterCount);
			Controls.Add(txtEnd);
			Controls.Add(lblEnd);
			Controls.Add(txtDuration);
			Controls.Add(lblDuration);
			Controls.Add(txtTime);
			Controls.Add(lblTime);
			Name = "frmCaptionProperties";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Caption Properties";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblText;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox txtText;
		private System.Windows.Forms.TextBox txtTime;
		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Label lblDuration;
		private System.Windows.Forms.TextBox txtDuration;
		private System.Windows.Forms.Label lblEnd;
		private System.Windows.Forms.TextBox txtEnd;
		private System.Windows.Forms.CheckBox chkBlankSpace;
		private System.Windows.Forms.Button btnSpeedDial;
		private System.Windows.Forms.Label lblSpeedDialListTitle;
		private System.Windows.Forms.Label lblSpeedDialList;
		private System.Windows.Forms.Label lblCharacterCount;
		private System.Windows.Forms.TextBox txtCharacterCount;
	}
}