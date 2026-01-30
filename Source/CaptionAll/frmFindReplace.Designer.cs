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
	partial class frmFindReplace
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
			this.lblFind = new System.Windows.Forms.Label();
			this.txtFind = new System.Windows.Forms.TextBox();
			this.lblReplaceWith = new System.Windows.Forms.Label();
			this.txtReplaceWith = new System.Windows.Forms.TextBox();
			this.btnFind = new System.Windows.Forms.Button();
			this.btnFindNext = new System.Windows.Forms.Button();
			this.btnReplace = new System.Windows.Forms.Button();
			this.btnReplaceAll = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblFind
			// 
			this.lblFind.AutoSize = true;
			this.lblFind.Location = new System.Drawing.Point(12, 54);
			this.lblFind.Name = "lblFind";
			this.lblFind.Size = new System.Drawing.Size(40, 20);
			this.lblFind.TabIndex = 0;
			this.lblFind.Text = "Fin&d:";
			// 
			// txtFind
			// 
			this.txtFind.Location = new System.Drawing.Point(119, 51);
			this.txtFind.Name = "txtFind";
			this.txtFind.Size = new System.Drawing.Size(333, 27);
			this.txtFind.TabIndex = 1;
			// 
			// lblReplaceWith
			// 
			this.lblReplaceWith.AutoSize = true;
			this.lblReplaceWith.Location = new System.Drawing.Point(12, 90);
			this.lblReplaceWith.Name = "lblReplaceWith";
			this.lblReplaceWith.Size = new System.Drawing.Size(97, 20);
			this.lblReplaceWith.TabIndex = 2;
			this.lblReplaceWith.Text = "Replace &with:";
			// 
			// txtReplaceWith
			// 
			this.txtReplaceWith.Location = new System.Drawing.Point(119, 87);
			this.txtReplaceWith.Name = "txtReplaceWith";
			this.txtReplaceWith.Size = new System.Drawing.Size(333, 27);
			this.txtReplaceWith.TabIndex = 3;
			// 
			// btnFind
			// 
			this.btnFind.Location = new System.Drawing.Point(475, 12);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(94, 30);
			this.btnFind.TabIndex = 4;
			this.btnFind.Text = "&Find";
			this.btnFind.UseVisualStyleBackColor = true;
			// 
			// btnFindNext
			// 
			this.btnFindNext.Location = new System.Drawing.Point(475, 48);
			this.btnFindNext.Name = "btnFindNext";
			this.btnFindNext.Size = new System.Drawing.Size(94, 30);
			this.btnFindNext.TabIndex = 5;
			this.btnFindNext.Text = "Find &Next";
			this.btnFindNext.UseVisualStyleBackColor = true;
			// 
			// btnReplace
			// 
			this.btnReplace.Location = new System.Drawing.Point(475, 84);
			this.btnReplace.Name = "btnReplace";
			this.btnReplace.Size = new System.Drawing.Size(94, 30);
			this.btnReplace.TabIndex = 6;
			this.btnReplace.Text = "&Replace";
			this.btnReplace.UseVisualStyleBackColor = true;
			// 
			// btnReplaceAll
			// 
			this.btnReplaceAll.Location = new System.Drawing.Point(475, 120);
			this.btnReplaceAll.Name = "btnReplaceAll";
			this.btnReplaceAll.Size = new System.Drawing.Size(94, 30);
			this.btnReplaceAll.TabIndex = 7;
			this.btnReplaceAll.Text = "Replace &All";
			this.btnReplaceAll.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(375, 168);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(94, 38);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Visible = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(475, 168);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(94, 38);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "&Close";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// frmFindReplace
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(581, 218);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnReplaceAll);
			this.Controls.Add(this.btnReplace);
			this.Controls.Add(this.btnFindNext);
			this.Controls.Add(this.btnFind);
			this.Controls.Add(this.lblReplaceWith);
			this.Controls.Add(this.txtReplaceWith);
			this.Controls.Add(this.txtFind);
			this.Controls.Add(this.lblFind);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmFindReplace";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Find and Replace";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFind;
		private System.Windows.Forms.TextBox txtFind;
		private System.Windows.Forms.Label lblReplaceWith;
		private System.Windows.Forms.TextBox txtReplaceWith;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.Button btnFindNext;
		private System.Windows.Forms.Button btnReplace;
		private System.Windows.Forms.Button btnReplaceAll;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}