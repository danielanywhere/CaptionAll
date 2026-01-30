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

namespace ScrollPanelVirtual
{
	partial class ScrollPanelControlVirt
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.scrollBarV = new ScrollPanelVirtual.ScrollBarVControlVirt();
			this.scrollBarH = new ScrollPanelVirtual.ScrollBarHControlVirt();
			this.pnlMain = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// scrollBarV
			// 
			this.scrollBarV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.scrollBarV.ButtonHeight = 21;
			this.scrollBarV.CanvasHeight = 10F;
			this.scrollBarV.Location = new System.Drawing.Point(679, 0);
			this.scrollBarV.Margin = new System.Windows.Forms.Padding(0);
			this.scrollBarV.MouseIsDown = false;
			this.scrollBarV.MouseLocationLast = new System.Drawing.Point(0, 0);
			this.scrollBarV.Name = "scrollBarV";
			this.scrollBarV.Size = new System.Drawing.Size(21, 429);
			this.scrollBarV.TabIndex = 3;
			this.scrollBarV.Value = 0F;
			this.scrollBarV.ViewPortHeight = 10F;
			this.scrollBarV.ViewPosition = 0F;
			this.scrollBarV.Visible = false;
			// 
			// scrollBarH
			// 
			this.scrollBarH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.scrollBarH.ButtonWidth = 21;
			this.scrollBarH.CanvasWidth = 10F;
			this.scrollBarH.Location = new System.Drawing.Point(0, 429);
			this.scrollBarH.Margin = new System.Windows.Forms.Padding(0);
			this.scrollBarH.MouseIsDown = false;
			this.scrollBarH.MouseLocationLast = new System.Drawing.Point(0, 0);
			this.scrollBarH.Name = "scrollBarH";
			this.scrollBarH.Size = new System.Drawing.Size(679, 21);
			this.scrollBarH.TabIndex = 2;
			this.scrollBarH.Value = 0F;
			this.scrollBarH.ViewPortWidth = 10F;
			this.scrollBarH.ViewPosition = 0F;
			this.scrollBarH.Visible = false;
			// 
			// pnlMain
			// 
			this.pnlMain.BackColor = System.Drawing.Color.White;
			this.pnlMain.Location = new System.Drawing.Point(3, 3);
			this.pnlMain.Name = "pnlMain";
			this.pnlMain.Size = new System.Drawing.Size(669, 423);
			this.pnlMain.TabIndex = 1;
			// 
			// ScrollPanelControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.scrollBarV);
			this.Controls.Add(this.scrollBarH);
			this.Controls.Add(this.pnlMain);
			this.Name = "ScrollPanelControl";
			this.Size = new System.Drawing.Size(700, 450);
			this.ResumeLayout(false);

		}

		#endregion

		private ScrollBarVControlVirt scrollBarV;
		private ScrollBarHControlVirt scrollBarH;
		private System.Windows.Forms.Panel pnlMain;
	}
}
