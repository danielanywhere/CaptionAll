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
	partial class frmAbout
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
		private void InitializeComponent()
		{
			lblTitle = new System.Windows.Forms.Label();
			lblDescription = new System.Windows.Forms.Label();
			lblVersionLabel = new System.Windows.Forms.Label();
			lblVersionText = new System.Windows.Forms.Label();
			btnClose = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// lblTitle
			// 
			lblTitle.AutoSize = true;
			lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblTitle.Location = new System.Drawing.Point(12, 9);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new System.Drawing.Size(168, 41);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "CaptionAll";
			// 
			// lblDescription
			// 
			lblDescription.AutoSize = true;
			lblDescription.Location = new System.Drawing.Point(12, 61);
			lblDescription.MaximumSize = new System.Drawing.Size(400, 0);
			lblDescription.Name = "lblDescription";
			lblDescription.Size = new System.Drawing.Size(334, 40);
			lblDescription.TabIndex = 1;
			lblDescription.Text = "A Windows desktop application for creating and managing text captions on media files.";
			// 
			// lblVersionLabel
			// 
			lblVersionLabel.AutoSize = true;
			lblVersionLabel.Location = new System.Drawing.Point(62, 140);
			lblVersionLabel.Name = "lblVersionLabel";
			lblVersionLabel.Size = new System.Drawing.Size(60, 20);
			lblVersionLabel.TabIndex = 2;
			lblVersionLabel.Text = "Version:";
			// 
			// lblVersionText
			// 
			lblVersionText.AutoSize = true;
			lblVersionText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblVersionText.Location = new System.Drawing.Point(128, 140);
			lblVersionText.Name = "lblVersionText";
			lblVersionText.Size = new System.Drawing.Size(57, 20);
			lblVersionText.TabIndex = 2;
			lblVersionText.Text = "1.2.3.4";
			// 
			// btnClose
			// 
			btnClose.Location = new System.Drawing.Point(319, 269);
			btnClose.Name = "btnClose";
			btnClose.Size = new System.Drawing.Size(94, 29);
			btnClose.TabIndex = 3;
			btnClose.Text = "&Close";
			btnClose.UseVisualStyleBackColor = true;
			// 
			// frmAbout
			// 
			AcceptButton = btnClose;
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = btnClose;
			ClientSize = new System.Drawing.Size(425, 310);
			Controls.Add(btnClose);
			Controls.Add(lblVersionText);
			Controls.Add(lblVersionLabel);
			Controls.Add(lblDescription);
			Controls.Add(lblTitle);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "frmAbout";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "About CaptionAll...";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblVersionLabel;
		private System.Windows.Forms.Label lblVersionText;
		private System.Windows.Forms.Button btnClose;
	}
}