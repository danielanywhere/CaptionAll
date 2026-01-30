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
	partial class frmScriptToCaption
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
			menuScriptToCaption = new System.Windows.Forms.MenuStrip();
			mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileOpenCaption = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileOpenScript = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
			mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileSep2 = new System.Windows.Forms.ToolStripSeparator();
			mnuFileClose = new System.Windows.Forms.ToolStripMenuItem();
			mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
			mnuEditAssign = new System.Windows.Forms.ToolStripMenuItem();
			mnuEditOverlayCaptions = new System.Windows.Forms.ToolStripMenuItem();
			statusScriptToCaption = new System.Windows.Forms.StatusStrip();
			statMessage = new System.Windows.Forms.ToolStripStatusLabel();
			pnlCaption = new System.Windows.Forms.Panel();
			dgCaption = new System.Windows.Forms.DataGridView();
			spltCaption = new System.Windows.Forms.Splitter();
			pnlScript = new System.Windows.Forms.Panel();
			rtScript = new System.Windows.Forms.RichTextBox();
			menuScriptToCaption.SuspendLayout();
			statusScriptToCaption.SuspendLayout();
			pnlCaption.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgCaption).BeginInit();
			pnlScript.SuspendLayout();
			SuspendLayout();
			// 
			// menuScriptToCaption
			// 
			menuScriptToCaption.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuScriptToCaption.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFile, mnuEdit });
			menuScriptToCaption.Location = new System.Drawing.Point(0, 0);
			menuScriptToCaption.Name = "menuScriptToCaption";
			menuScriptToCaption.Size = new System.Drawing.Size(800, 28);
			menuScriptToCaption.TabIndex = 0;
			menuScriptToCaption.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFileOpenCaption, mnuFileOpenScript, mnuFileSep1, mnuFileSave, mnuFileSep2, mnuFileClose });
			mnuFile.Name = "mnuFile";
			mnuFile.Size = new System.Drawing.Size(46, 24);
			mnuFile.Text = "&File";
			// 
			// mnuFileOpenCaption
			// 
			mnuFileOpenCaption.Name = "mnuFileOpenCaption";
			mnuFileOpenCaption.Size = new System.Drawing.Size(240, 26);
			mnuFileOpenCaption.Text = "Open VTT &Caption File";
			// 
			// mnuFileOpenScript
			// 
			mnuFileOpenScript.Name = "mnuFileOpenScript";
			mnuFileOpenScript.Size = new System.Drawing.Size(240, 26);
			mnuFileOpenScript.Text = "Open TXT &Script File";
			// 
			// mnuFileSep1
			// 
			mnuFileSep1.Name = "mnuFileSep1";
			mnuFileSep1.Size = new System.Drawing.Size(237, 6);
			// 
			// mnuFileSave
			// 
			mnuFileSave.Name = "mnuFileSave";
			mnuFileSave.Size = new System.Drawing.Size(240, 26);
			mnuFileSave.Text = "&Save Caption File";
			// 
			// mnuFileSep2
			// 
			mnuFileSep2.Name = "mnuFileSep2";
			mnuFileSep2.Size = new System.Drawing.Size(237, 6);
			// 
			// mnuFileClose
			// 
			mnuFileClose.Name = "mnuFileClose";
			mnuFileClose.Size = new System.Drawing.Size(240, 26);
			mnuFileClose.Text = "&Close";
			// 
			// mnuEdit
			// 
			mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuEditAssign, mnuEditOverlayCaptions });
			mnuEdit.Name = "mnuEdit";
			mnuEdit.Size = new System.Drawing.Size(49, 24);
			mnuEdit.Text = "&Edit";
			// 
			// mnuEditAssign
			// 
			mnuEditAssign.Name = "mnuEditAssign";
			mnuEditAssign.Size = new System.Drawing.Size(312, 26);
			mnuEditAssign.Text = "&Assign Selected Script to Caption";
			mnuEditAssign.Visible = false;
			// 
			// mnuEditOverlayCaptions
			// 
			mnuEditOverlayCaptions.Name = "mnuEditOverlayCaptions";
			mnuEditOverlayCaptions.Size = new System.Drawing.Size(312, 26);
			mnuEditOverlayCaptions.Text = "&Overlay Captions";
			// 
			// statusScriptToCaption
			// 
			statusScriptToCaption.ImageScalingSize = new System.Drawing.Size(20, 20);
			statusScriptToCaption.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statMessage });
			statusScriptToCaption.Location = new System.Drawing.Point(0, 424);
			statusScriptToCaption.Name = "statusScriptToCaption";
			statusScriptToCaption.Size = new System.Drawing.Size(800, 26);
			statusScriptToCaption.TabIndex = 1;
			statusScriptToCaption.Text = "statusStrip1";
			// 
			// statMessage
			// 
			statMessage.Name = "statMessage";
			statMessage.Size = new System.Drawing.Size(59, 20);
			statMessage.Text = "Ready...";
			// 
			// pnlCaption
			// 
			pnlCaption.Controls.Add(dgCaption);
			pnlCaption.Dock = System.Windows.Forms.DockStyle.Left;
			pnlCaption.Location = new System.Drawing.Point(0, 28);
			pnlCaption.Name = "pnlCaption";
			pnlCaption.Size = new System.Drawing.Size(432, 396);
			pnlCaption.TabIndex = 2;
			// 
			// dgCaption
			// 
			dgCaption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			dgCaption.Location = new System.Drawing.Point(0, 0);
			dgCaption.Name = "dgCaption";
			dgCaption.RowHeadersWidth = 51;
			dgCaption.RowTemplate.Height = 29;
			dgCaption.Size = new System.Drawing.Size(432, 396);
			dgCaption.TabIndex = 0;
			// 
			// spltCaption
			// 
			spltCaption.Location = new System.Drawing.Point(432, 28);
			spltCaption.Name = "spltCaption";
			spltCaption.Size = new System.Drawing.Size(6, 396);
			spltCaption.TabIndex = 3;
			spltCaption.TabStop = false;
			// 
			// pnlScript
			// 
			pnlScript.Controls.Add(rtScript);
			pnlScript.Dock = System.Windows.Forms.DockStyle.Fill;
			pnlScript.Location = new System.Drawing.Point(438, 28);
			pnlScript.Name = "pnlScript";
			pnlScript.Size = new System.Drawing.Size(362, 396);
			pnlScript.TabIndex = 4;
			// 
			// rtScript
			// 
			rtScript.BorderStyle = System.Windows.Forms.BorderStyle.None;
			rtScript.Dock = System.Windows.Forms.DockStyle.Fill;
			rtScript.Location = new System.Drawing.Point(0, 0);
			rtScript.Name = "rtScript";
			rtScript.Size = new System.Drawing.Size(362, 396);
			rtScript.TabIndex = 0;
			rtScript.Text = "";
			// 
			// frmScriptToCaption
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(800, 450);
			Controls.Add(pnlScript);
			Controls.Add(spltCaption);
			Controls.Add(pnlCaption);
			Controls.Add(statusScriptToCaption);
			Controls.Add(menuScriptToCaption);
			MainMenuStrip = menuScriptToCaption;
			Name = "frmScriptToCaption";
			Text = "Script to Caption Converter";
			menuScriptToCaption.ResumeLayout(false);
			menuScriptToCaption.PerformLayout();
			statusScriptToCaption.ResumeLayout(false);
			statusScriptToCaption.PerformLayout();
			pnlCaption.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgCaption).EndInit();
			pnlScript.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.MenuStrip menuScriptToCaption;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuFileOpenScript;
		private System.Windows.Forms.ToolStripMenuItem mnuFileOpenCaption;
		private System.Windows.Forms.ToolStripSeparator mnuFileSep1;
		private System.Windows.Forms.ToolStripMenuItem mnuFileSave;
		private System.Windows.Forms.ToolStripSeparator mnuFileSep2;
		private System.Windows.Forms.ToolStripMenuItem mnuFileClose;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
		private System.Windows.Forms.ToolStripMenuItem mnuEditAssign;
		private System.Windows.Forms.StatusStrip statusScriptToCaption;
		private System.Windows.Forms.ToolStripStatusLabel statMessage;
		private System.Windows.Forms.Panel pnlCaption;
		private System.Windows.Forms.Splitter spltCaption;
		private System.Windows.Forms.Panel pnlScript;
		private System.Windows.Forms.DataGridView dgCaption;
		private System.Windows.Forms.RichTextBox rtScript;
		private System.Windows.Forms.ToolStripMenuItem mnuEditOverlayCaptions;
	}
}