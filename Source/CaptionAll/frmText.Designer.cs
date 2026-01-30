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
	partial class frmText
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
			this.label = new System.Windows.Forms.Label();
			this.text = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSpeedDial = new System.Windows.Forms.Button();
			this.lblSpeedDialList = new System.Windows.Forms.Label();
			this.lblSpeedDialListTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(12, 9);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(200, 20);
			this.label.TabIndex = 0;
			this.label.Text = "Please enter the caption text:";
			// 
			// text
			// 
			this.text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.text.Location = new System.Drawing.Point(12, 32);
			this.text.Multiline = true;
			this.text.Name = "text";
			this.text.Size = new System.Drawing.Size(435, 199);
			this.text.TabIndex = 1;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(536, 237);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(94, 38);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(636, 237);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(94, 38);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSpeedDial
			// 
			this.btnSpeedDial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSpeedDial.Location = new System.Drawing.Point(12, 237);
			this.btnSpeedDial.Name = "btnSpeedDial";
			this.btnSpeedDial.Size = new System.Drawing.Size(123, 38);
			this.btnSpeedDial.TabIndex = 2;
			this.btnSpeedDial.Text = "S&peed Dial";
			this.btnSpeedDial.UseVisualStyleBackColor = true;
			// 
			// lblSpeedDialList
			// 
			this.lblSpeedDialList.Location = new System.Drawing.Point(475, 32);
			this.lblSpeedDialList.Name = "lblSpeedDialList";
			this.lblSpeedDialList.Size = new System.Drawing.Size(246, 199);
			this.lblSpeedDialList.TabIndex = 15;
			this.lblSpeedDialList.Text = "Ctrl+1:\r\nCtrl+2:\r\nCtrl+3:\r\nCtrl+4:\r\nCtrl+5:\r\nCtrl+6:\r\nCtrl+7:\r\nCtrl+8:\r\nCtrl+9:";
			// 
			// lblSpeedDialListTitle
			// 
			this.lblSpeedDialListTitle.AutoSize = true;
			this.lblSpeedDialListTitle.Location = new System.Drawing.Point(475, 9);
			this.lblSpeedDialListTitle.Name = "lblSpeedDialListTitle";
			this.lblSpeedDialListTitle.Size = new System.Drawing.Size(111, 20);
			this.lblSpeedDialListTitle.TabIndex = 14;
			this.lblSpeedDialListTitle.Text = "Speed Dial List:";
			// 
			// frmText
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 288);
			this.Controls.Add(this.lblSpeedDialList);
			this.Controls.Add(this.lblSpeedDialListTitle);
			this.Controls.Add(this.btnSpeedDial);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.text);
			this.Controls.Add(this.label);
			this.Name = "frmText";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Text";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label;
		private System.Windows.Forms.TextBox text;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSpeedDial;
		private System.Windows.Forms.Label lblSpeedDialList;
		private System.Windows.Forms.Label lblSpeedDialListTitle;
	}
}