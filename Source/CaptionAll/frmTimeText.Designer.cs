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
	partial class frmTimeText
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
			this.txtTime = new System.Windows.Forms.TextBox();
			this.lblTime = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtText = new System.Windows.Forms.TextBox();
			this.lblText = new System.Windows.Forms.Label();
			this.btnSpeedDial = new System.Windows.Forms.Button();
			this.lblSpeedDialList = new System.Windows.Forms.Label();
			this.lblSpeedDialListTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtTime
			// 
			this.txtTime.Location = new System.Drawing.Point(172, 12);
			this.txtTime.Name = "txtTime";
			this.txtTime.Size = new System.Drawing.Size(103, 27);
			this.txtTime.TabIndex = 1;
			this.txtTime.Text = "00:00:00.000";
			// 
			// lblTime
			// 
			this.lblTime.AutoSize = true;
			this.lblTime.Location = new System.Drawing.Point(11, 15);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(155, 20);
			this.lblTime.TabIndex = 0;
			this.lblTime.Text = "Enter the current time:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(637, 277);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(94, 38);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(537, 277);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(94, 38);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// txtText
			// 
			this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtText.Location = new System.Drawing.Point(12, 72);
			this.txtText.Multiline = true;
			this.txtText.Name = "txtText";
			this.txtText.Size = new System.Drawing.Size(435, 199);
			this.txtText.TabIndex = 3;
			// 
			// lblText
			// 
			this.lblText.AutoSize = true;
			this.lblText.Location = new System.Drawing.Point(12, 49);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(154, 20);
			this.lblText.TabIndex = 2;
			this.lblText.Text = "Enter the caption text:";
			// 
			// btnSpeedDial
			// 
			this.btnSpeedDial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSpeedDial.Location = new System.Drawing.Point(12, 277);
			this.btnSpeedDial.Name = "btnSpeedDial";
			this.btnSpeedDial.Size = new System.Drawing.Size(123, 38);
			this.btnSpeedDial.TabIndex = 4;
			this.btnSpeedDial.Text = "S&peed Dial";
			this.btnSpeedDial.UseVisualStyleBackColor = true;
			// 
			// lblSpeedDialList
			// 
			this.lblSpeedDialList.Location = new System.Drawing.Point(475, 72);
			this.lblSpeedDialList.Name = "lblSpeedDialList";
			this.lblSpeedDialList.Size = new System.Drawing.Size(246, 199);
			this.lblSpeedDialList.TabIndex = 17;
			this.lblSpeedDialList.Text = "Ctrl+1:\r\nCtrl+2:\r\nCtrl+3:\r\nCtrl+4:\r\nCtrl+5:\r\nCtrl+6:\r\nCtrl+7:\r\nCtrl+8:\r\nCtrl+9:";
			// 
			// lblSpeedDialListTitle
			// 
			this.lblSpeedDialListTitle.AutoSize = true;
			this.lblSpeedDialListTitle.Location = new System.Drawing.Point(475, 49);
			this.lblSpeedDialListTitle.Name = "lblSpeedDialListTitle";
			this.lblSpeedDialListTitle.Size = new System.Drawing.Size(111, 20);
			this.lblSpeedDialListTitle.TabIndex = 16;
			this.lblSpeedDialListTitle.Text = "Speed Dial List:";
			// 
			// frmTimeText
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 325);
			this.Controls.Add(this.lblSpeedDialList);
			this.Controls.Add(this.lblSpeedDialListTitle);
			this.Controls.Add(this.btnSpeedDial);
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtText);
			this.Controls.Add(this.txtTime);
			this.Controls.Add(this.lblTime);
			this.Name = "frmTimeText";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Time and Text";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtTime;
		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox txtText;
		private System.Windows.Forms.Label lblText;
		private System.Windows.Forms.Button btnSpeedDial;
		private System.Windows.Forms.Label lblSpeedDialList;
		private System.Windows.Forms.Label lblSpeedDialListTitle;
	}
}