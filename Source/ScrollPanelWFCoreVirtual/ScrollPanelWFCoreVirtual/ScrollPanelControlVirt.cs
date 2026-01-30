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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Diagnostics;

namespace ScrollPanelVirtual
{
	//*-------------------------------------------------------------------------*
	//*	ScrollPanelControl																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Self-enclosed scroll panel control with custom scroll bars.
	/// </summary>
	//[Designer(typeof(ScrollPanelControlDesigner))]
	public partial class ScrollPanelControlVirt : UserControl
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private int mCanvasBottom = 0;
		private int mCanvasLeft = 0;
		private int mCanvasRight = 0;
		private int mCanvasTop = 0;
		private bool mLocationBusy = false;
		private List<IPaintableControl> mPaintableControls =
			new List<IPaintableControl>();

		//*-----------------------------------------------------------------------*
		//* control_Invalidated																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The attached control has been invalidated.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Invalidate event arguments.
		/// </param>
		private void control_Invalidated(object sender, InvalidateEventArgs e)
		{
			Trace.WriteLine("SPCV: control_Invalidated");
			pnlMain.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* pnlMain_Control_LocationChanged																				*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// A child control of pnlMain has been moved.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Standard event arguments.
		///// </param>
		//private void pnlMain_Control_LocationChanged(object sender, EventArgs e)
		//{
		//	Control control = null;

		//	if(!mLocationBusy && sender is Control)
		//	{
		//		mLocationBusy = true;
		//		control = (Control)sender;
		//		scrollBarH.ViewPosition = 0 - control.Left;
		//		scrollBarV.ViewPosition = 0 - control.Top;
		//		mLocationBusy = false;
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlMain_Control_Resize																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A child control has been resized.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void pnlMain_Control_Resize(object sender, EventArgs e)
		{
			int bottom = int.MinValue;
			int height = 0;
			int left = int.MaxValue;
			int right = int.MinValue;
			int top = int.MaxValue;
			int width = 0;

			foreach(IPaintableControl control in mPaintableControls)
			{
				top = Math.Min(top, control.DrawingLocation.Top);
				bottom = Math.Max(bottom, control.DrawingLocation.Top +
					control.DrawingSize.Height - 1);
				left = Math.Min(left, control.DrawingLocation.Left);
				right = Math.Max(right, control.DrawingLocation.Left +
					control.DrawingSize.Width - 1);
			}
			Trace.WriteLine("SPCV: pnlMain_Control_Resize " +
				$"top: {top}; bottom: {bottom}; left: {left}; right: {right}");
			if(left > 0)
			{
				left = 0;
			}
			if(top > 0)
			{
				top = 0;
			}
			//	Determine whether the virtual canvas has been moved.
			if(left != mCanvasLeft ||
				top != mCanvasTop ||
				right != mCanvasRight ||
				bottom != mCanvasBottom)
			{
				//	Update the scroll bars based upon the new canvas.
				width = right - left + 1;
				height = bottom - top + 1;
				//	Horizontal.
				scrollBarH.CanvasWidth = width;
				scrollBarH.ViewPortWidth = pnlMain.Width;
				//	Vertical.
				scrollBarV.CanvasHeight = height;
				scrollBarV.ViewPortHeight = pnlMain.Height;
				mCanvasBottom = bottom;
				mCanvasLeft = left;
				mCanvasRight = right;
				mCanvasTop = top;
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* pnlMain_ControlAdded																									*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// A control has been added to the main canvas panel.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Control event arguments.
		///// </param>
		//private void pnlMain_ControlAdded(object sender, ControlEventArgs e)
		//{
		//	e.Control.LocationChanged += pnlMain_Control_Resize;
		//	e.Control.Resize += pnlMain_Control_Resize;
		//	pnlMain_Control_Resize(e.Control, new EventArgs());
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* pnlMain_ControlRemoved																								*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// A control has been removed from the main canvas panel.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Control event arguments.
		///// </param>
		//private void pnlMain_ControlRemoved(object sender, ControlEventArgs e)
		//{
		//	e.Control.LocationChanged -= pnlMain_Control_Resize;
		//	e.Control.Resize -= pnlMain_Control_Resize;
		//	pnlMain_Control_Resize(e.Control, new EventArgs());
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlMain_MouseDoubleClick																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse button has been double-clicked on the main panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		private void pnlMain_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			MouseEventArgs ce = null;
			IPaintableControl control = null;

			base.OnMouseDoubleClick(e);
			if(mPaintableControls.Count > 0)
			{
				control = mPaintableControls[0];
				ce = new MouseEventArgs(e.Button,
					e.Clicks, e.X + (0 - control.DrawingLocation.Left), e.Y, e.Delta);
				control.RaiseMouseDoubleClick(ce);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlMain_MouseDown																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse button has been depressed on the main panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		private void pnlMain_MouseDown(object sender, MouseEventArgs e)
		{
			MouseEventArgs ce = null;
			IPaintableControl control = null;

			base.OnMouseDown(e);
			if(mPaintableControls.Count > 0)
			{
				control = mPaintableControls[0];
				ce = new MouseEventArgs(e.Button,
					e.Clicks, e.X + (0 - control.DrawingLocation.Left), e.Y, e.Delta);
				control.RaiseMouseDown(ce);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlMain_MouseMove																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse has moved over the main panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		private void pnlMain_MouseMove(object sender, MouseEventArgs e)
		{
			MouseEventArgs ce = null;
			IPaintableControl control = null;

			base.OnMouseMove(e);
			if(mPaintableControls.Count > 0)
			{
				control = mPaintableControls[0];
				ce = new MouseEventArgs(e.Button,
					e.Clicks, e.X + (0 - control.DrawingLocation.Left), e.Y, e.Delta);
				control.RaiseMouseMove(ce);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlMain_MouseUp																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse button has been released over the main panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		private void pnlMain_MouseUp(object sender, MouseEventArgs e)
		{
			MouseEventArgs ce = null;
			IPaintableControl control = null;

			base.OnMouseUp(e);
			if(mPaintableControls.Count > 0)
			{
				control = mPaintableControls[0];
				ce = new MouseEventArgs(e.Button,
					e.Clicks, e.X + (0 - control.DrawingLocation.Left), e.Y, e.Delta);
				control.RaiseMouseUp(ce);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlMain_Paint																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The main panel is being painted.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		private void pnlMain_Paint(object sender, PaintEventArgs e)
		{
			Rectangle hostArea = new Rectangle(0, 0, pnlMain.Width, pnlMain.Height);

			e.Graphics.FillRectangle(new SolidBrush(pnlMain.BackColor), hostArea);
			foreach(IPaintableControl controlItem in mPaintableControls)
			{
				Trace.WriteLine($"SPCV: pnlMain_Paint {controlItem.Name}; " +
					$"left: {controlItem.DrawingLocation.Left}; top: {controlItem.DrawingLocation.Top}; " +
					$"width: {hostArea.Width}; height: {hostArea.Height}");
				controlItem.PaintArea(e.Graphics,
					new Rectangle(0 - controlItem.DrawingLocation.Left,
					0 - controlItem.DrawingLocation.Top,
					Math.Min(controlItem.DrawingSize.Width, hostArea.Width),
					Math.Min(controlItem.DrawingSize.Height, hostArea.Height)), hostArea);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RefreshLayout																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Reposition the moveable components after the control has been resized.
		/// </summary>
		private void RefreshLayout()
		{
			Trace.WriteLine("SPCV: RefreshLayout");
			//	Line up the constituent controls.
			pnlMain.Location = new Point(0, 0);
			pnlMain.Size = new Size(
				this.Width - (mVerticalScrollEnabled ? 22 : 0),
				this.Height - (mHorizontalScrollEnabled ? 22 : 0)
				);
			pnlMain.TabIndex = 0;
			scrollBarH.Location = new Point(0, this.Height - 21);
			scrollBarH.Size = new Size(this.Width - 22, 21);
			scrollBarH.TabIndex = 1;
			scrollBarV.Location = new Point(this.Width - 22, 0);
			scrollBarV.Size = new Size(22, this.Height - 22);
			scrollBarV.TabIndex = 2;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* scrollBarH_CanvasWidthChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The width of the canvas has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void scrollBarH_CanvasWidthChanged(object sender, EventArgs e)
		{
			Trace.WriteLine("SPCV: scrollBarH_CanvasWidthChanged");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* scrollBarH_ValueChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The horizontal scroll value has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void scrollBarH_ValueChanged(object sender, EventArgs e)
		{
			IPaintableControl control = null;

			if(!mLocationBusy)
			{
				mLocationBusy = true;
				if(mPaintableControls.Count > 0)
				{
					control = mPaintableControls[0];
					control.DrawingLocation.Left = 0 - (int)scrollBarH.ViewPosition;
					//control.DrawingLocation =
					//	new PointEv()
					//	{
					//		Left = 0 - (int)scrollBarH.ViewPosition,
					//		Top = 0 - (int)scrollBarV.ViewPosition
					//	};
					pnlMain.Invalidate();
				}
				mLocationBusy = false;
			}
			OnValueChanged(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* scrollBarH_ViewPortWidthChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The width of the horizontal view port has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void scrollBarH_ViewPortWidthChanged(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* scrollBarV_CanvasHeightChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The height of the canvas has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void scrollBarV_CanvasHeightChanged(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* scrollBarV_ValueChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The vertical scroll value has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void scrollBarV_ValueChanged(object sender, EventArgs e)
		{
			IPaintableControl control = null;

			if(!mLocationBusy)
			{
				mLocationBusy = true;
				if(mPaintableControls.Count > 0)
				{
					control = mPaintableControls[0];
					control.DrawingLocation.Top = 0 - (int)scrollBarV.ViewPosition;
					//control.DrawingLocation =
					//	new PointEv()
					//	{
					//		Left = 0 - (int)scrollBarH.ViewPosition,
					//		Top = 0 - (int)scrollBarV.ViewPosition
					//	};
					pnlMain.Invalidate();
				}
				mLocationBusy = false;
			}
			OnValueChanged(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* scrollBarV_ViewPortHeightChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The height of the viewport has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void scrollBarV_ViewPortHeightChanged(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnGotFocus																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the GotFocus event when the control has received the focus.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.Invalidate();
			this.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLoad																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Load event when the control has been loaded and is ready
		/// to display for the first time.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			//SetScrollSpeed();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLostFocus																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the LostFocus event when the control has lost focus.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.Invalidate();
			this.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseWheel																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseWheel event when the mouse wheel is receiving activity.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			switch(mScrollWheelAssignment)
			{
				case ScrollWheelAssignmentEnum.NPanSScrollCZoom:
					//	No mod - Pan; Shift - Scroll; Ctrl - Zoom.
					if((ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						//	[Shift][Scroll]: Up / Down.
						if(e.Delta > 0)
						{
							scrollBarV.ScrollDecrement();
						}
						else
						{
							scrollBarV.ScrollIncrement();
						}
					}
					else if((ModifierKeys & Keys.Control) == Keys.Control)
					{
						//	[Ctrl][Scroll]: In / Out.
					}
					else
					{
						//	[Scroll]: Left / Right.
						if(e.Delta > 0)
						{
							scrollBarH.ScrollDecrement();
						}
						else
						{
							scrollBarH.ScrollIncrement();
						}
					}
					break;
				case ScrollWheelAssignmentEnum.NScrollSPanCZoom:
					//	No mod - Scroll; Shift - Pan; Ctrl - Zoom.
					//	Default.
					if((ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						//	[Shift][Scroll]: Left / Right.
						if(e.Delta > 0)
						{
							scrollBarH.ScrollDecrement();
						}
						else
						{
							scrollBarH.ScrollIncrement();
						}
					}
					else if((ModifierKeys & Keys.Control) == Keys.Control)
					{
						//	[Ctrl][Scroll]: In / Out.
					}
					else
					{
						//	[Scroll]: Up / Down.
						if(e.Delta > 0)
						{
							scrollBarV.ScrollDecrement();
						}
						else
						{
							scrollBarV.ScrollIncrement();
						}
					}
					break;
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* OnPaint																																*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Raises the Paint event when the control is being repainted.
		///// </summary>
		///// <param name="e">
		///// Paint event arguments.
		///// </param>
		//protected override void OnPaint(PaintEventArgs e)
		//{
		//	Brush brush = null;
		//	Graphics g = e.Graphics;
		//	Rectangle rect = Rectangle.Empty;

		//	g.CompositingQuality = CompositingQuality.HighQuality;
		//	g.SmoothingMode = SmoothingMode.AntiAlias;
		//	base.OnPaint(e);

		//	if(Focused)
		//	{
		//		brush = new SolidBrush(Color.FromArgb(52, 99, 220));
		//		rect = new Rectangle(this.Width - 22, this.Height - 22, 10, 10);
		//		g.FillEllipse(brush, rect);
		//		brush = new SolidBrush(Color.FromArgb(162, 181, 233));
		//		rect = new Rectangle(this.Width - 21, this.Height - 21, 3, 3);
		//		g.FillEllipse(brush, rect);
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnResize																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Resize event when the control has been resized.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnResize(EventArgs e)
		{
			IPaintableControl control = null;
			PointEv pt = null;

			mLocationBusy = true;
			RefreshLayout();
			scrollBarH.ViewPortWidth = pnlMain.Width;
			scrollBarV.ViewPortHeight = pnlMain.Height;
			if(!scrollBarH.Visible && mPaintableControls.Count > 0)
			{
				control = mPaintableControls[0];
				pt = control.DrawingLocation;

				scrollBarH.Value = 0;
				//pnlMain.Controls[0].Left = 0;
				if(mPaintableControls.Count > 0)
				{
					if(pt != null)
					{
						pt.Left = 0;
					}
				}
			}
			if(!scrollBarV.Visible && mPaintableControls.Count > 0)
			{
				scrollBarV.Value = 0;
				//pnlMain.Controls[0].Top = 0;
				if(mPaintableControls.Count > 0)
				{
					if(pt != null)
					{
						pt.Top = 0;
					}
				}
			}
			//SetScrollSpeed();
			mLocationBusy = false;
			base.OnResize(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnValueChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A scroll value has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* ProcessCmdKey																													*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Processes a command key.
		///// </summary>
		///// <param name="msg">
		///// A message, passed by reference, representing the window message to
		///// process.
		///// </param>
		///// <param name="keyData">
		///// One of the Keys values representing the key to process.
		///// </param>
		///// <returns>
		///// Whether the specified key was processed.
		///// </returns>
		//protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		//{
		//	bool result = false;

		//	switch(keyData)
		//	{
		//		case Keys.Down:
		//			scrollBarV.ScrollIncrement();
		//			result = true;
		//			break;
		//		case Keys.Left:
		//			scrollBarH.ScrollDecrement();
		//			result = true;
		//			break;
		//		case Keys.PageDown:
		//			if((ModifierKeys & Keys.Alt) == Keys.Alt)
		//			{
		//				scrollBarH.ScrollPageNext();
		//			}
		//			else
		//			{
		//				scrollBarV.ScrollPageNext();
		//			}
		//			result = true;
		//			break;
		//		case Keys.PageUp:
		//			if((ModifierKeys & Keys.Alt) == Keys.Alt)
		//			{
		//				scrollBarH.ScrollPagePrevious();
		//			}
		//			else
		//			{
		//				scrollBarV.ScrollPagePrevious();
		//			}
		//			result = true;
		//			break;
		//		case Keys.Right:
		//			scrollBarH.ScrollIncrement();
		//			result = true;
		//			break;
		//		case Keys.Up:
		//			scrollBarV.ScrollDecrement();
		//			result = true;
		//			break;
		//	}
		//	if(!result)
		//	{
		//		result = base.ProcessCmdKey(ref msg, keyData);
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the ScrollPanelControl Item.
		/// </summary>
		public ScrollPanelControlVirt()
		{
			InitializeComponent();

			//Trace.Listeners.Add(
			//	new TextWriterTraceListener(
			//		@"C:\Temp\DebugScrollPanelVirt-New.txt"));
			//Trace.AutoFlush = true;

			typeof(Panel).InvokeMember("DoubleBuffered",
					BindingFlags.SetProperty | BindingFlags.Instance |
					BindingFlags.NonPublic,
					null, pnlMain, new object[] { true });

			this.DoubleBuffered = true;
			//SetStyle(ControlStyles.Selectable, true);

			pnlMain.BackColor = mCanvasBackColor;
			pnlMain.MouseDoubleClick += pnlMain_MouseDoubleClick;
			pnlMain.MouseDown += pnlMain_MouseDown;
			pnlMain.MouseMove += pnlMain_MouseMove;
			pnlMain.MouseUp += pnlMain_MouseUp;
			pnlMain.Paint += pnlMain_Paint;

			RefreshLayout();

			////	Capture the event of any control added or removed from the host area.
			//pnlMain.ControlAdded += pnlMain_ControlAdded;
			//pnlMain.ControlRemoved += pnlMain_ControlRemoved;

			////	Wire any controls added from the designer.
			//foreach(Control control in pnlMain.Controls)
			//{
			//	control.LocationChanged += pnlMain_Control_LocationChanged;
			//	control.Resize += pnlMain_Control_Resize;
			//}

			scrollBarH.CanvasWidthChanged += scrollBarH_CanvasWidthChanged;
			scrollBarH.ValueChanged += scrollBarH_ValueChanged;
			scrollBarH.ViewPortWidthChanged += scrollBarH_ViewPortWidthChanged;
			scrollBarV.CanvasHeightChanged += scrollBarV_CanvasHeightChanged;
			scrollBarV.ValueChanged += scrollBarV_ValueChanged;
			scrollBarV.ViewPortHeightChanged += scrollBarV_ViewPortHeightChanged;

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AttachCanvasControl																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Attach the canvas to be controlled.
		/// </summary>
		/// <param name="control">
		/// Reference to the control to be attached.
		/// </param>
		public void AttachCanvasControl(IPaintableControl control)
		{
			if(control != null)
			{
				if(control.Parent != null)
				{
					control.Parent.Controls.Remove((Control)control);
				}
				control.Parent = this;
				control.Invalidated += control_Invalidated;
				control.LocationChanged += pnlMain_Control_Resize;
				control.Resize += pnlMain_Control_Resize;
				control.DrawingLocation.LocationChanged += pnlMain_Control_Resize;
				control.DrawingSize.SizeChanged += pnlMain_Control_Resize;
				pnlMain_Control_Resize(control, new EventArgs());
				mPaintableControls.Add(control);
			}
		}

		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanvasBackColor																												*
		//*-----------------------------------------------------------------------*
		private Color mCanvasBackColor = Color.FromArgb(34, 68, 102);
		/// <summary>
		/// Get/Set the background color of the empty canvas.
		/// </summary>
		public Color CanvasBackColor
		{
			get { return mCanvasBackColor; }
			set
			{
				mCanvasBackColor = value;
				pnlMain.BackColor = mCanvasBackColor;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanvasHeight																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the height of the canvas.
		/// </summary>
		public int CanvasHeight
		{
			get { return pnlMain.Height; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanvasWidth																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the width of the canvas.
		/// </summary>
		public int CanvasWidth
		{
			get { return pnlMain.Width; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	ControlCanvas																													*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Get a reference to the main control canvas.
		///// </summary>
		//public Panel ControlCanvas
		//{
		//	get { return pnlMain; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ControlWidth																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the width of the client control.
		/// </summary>
		public int ControlWidth
		{
			get
			{
				int result = 0;

				if(mPaintableControls.Count > 0)
				{
					result = mPaintableControls[0].DrawingSize.Width;
				}
				return result;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DetachCanvasControl																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Detach the specified control from the canvas controller.
		/// </summary>
		/// <param name="control">
		/// Reference to the control to be detached.
		/// </param>
		public void DetachCanvasControl(IPaintableControl control)
		{
			if(control != null)
			{
				control.Invalidated -= control_Invalidated;
				control.LocationChanged -= pnlMain_Control_Resize;
				control.Resize -= pnlMain_Control_Resize;
				pnlMain_Control_Resize(control, new EventArgs());
				if(mPaintableControls.Contains(control))
				{
					mPaintableControls.Remove(control);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HorizontalScrollEnabled																								*
		//*-----------------------------------------------------------------------*
		private bool mHorizontalScrollEnabled = true;
		/// <summary>
		/// Get/Set a value indicating whether horizontal scroll is enabled.
		/// </summary>
		[DefaultValue(true)]
		public bool HorizontalScrollEnabled
		{
			get { return mHorizontalScrollEnabled; }
			set
			{
				Trace.WriteLine($"SPCV: HorzScrollEnabled: {value}");
				mHorizontalScrollEnabled = value;
				scrollBarH.CanDisplay = value;
				this.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ScrollWheelAssignment																									*
		//*-----------------------------------------------------------------------*
		private ScrollWheelAssignmentEnum mScrollWheelAssignment =
			ScrollWheelAssignmentEnum.NScrollSPanCZoom;
		/// <summary>
		/// Get/Set the scroll wheel behavior assignment for this instance.
		/// </summary>
		public ScrollWheelAssignmentEnum ScrollWheelAssignment
		{
			get { return mScrollWheelAssignment; }
			set { mScrollWheelAssignment = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetScrollSpeed																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the scroll speed based upon the size of the scroll bar.
		/// </summary>
		/// <param name="multiplier">
		/// The multiplier to use.
		/// </param>
		public void SetScrollSpeed(float multiplier = 1f)
		{
			if(multiplier > 0f)
			{
				//if(scrollBarH.MaximumValue > multiplier)
				//{
				//	scrollBarH.Speed = scrollBarH.MaximumValue / multiplier;
				//}
				//if(scrollBarV.MaximumValue > multiplier)
				//{
				//	scrollBarV.Speed = scrollBarV.MaximumValue / multiplier;
				//}
				scrollBarH.Speed = multiplier;
				scrollBarV.Speed = multiplier;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ScrollPositionH																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the horizontal scroll window position, in pixels.
		/// </summary>
		[DefaultValue(0f)]
		public int ScrollPositionH
		{
			get { return (int)(scrollBarH.Value * scrollBarH.StepSize); }
			set { scrollBarH.Value = (float)(value / scrollBarH.StepSize); }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ScrollPositionV																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the vertical scroll window position, in pixels.
		/// </summary>
		[DefaultValue(0f)]
		public int ScrollPositionV
		{
			get { return (int)(scrollBarV.Value * scrollBarV.StepSize); }
			set { scrollBarV.Value = (float)(value / scrollBarV.StepSize); }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ScrollValueH																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the horizontal scroll value.
		/// </summary>
		[DefaultValue(0f)]
		public float ScrollValueH
		{
			get { return scrollBarH.Value; }
			set { scrollBarH.Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ScrollValueV																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the vertical scroll value.
		/// </summary>
		[DefaultValue(0f)]
		public float ScrollValueV
		{
			get { return scrollBarV.Value; }
			set { scrollBarV.Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ValueChanged																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A scroll value has changed.
		/// </summary>
		public event EventHandler ValueChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	VerticalScrollEnabled																									*
		//*-----------------------------------------------------------------------*
		private bool mVerticalScrollEnabled = true;
		/// <summary>
		/// Get/Set a value indicating whether vertical scroll is enabled.
		/// </summary>
		[DefaultValue(true)]
		public bool VerticalScrollEnabled
		{
			get { return mVerticalScrollEnabled; }
			set
			{
				mVerticalScrollEnabled = value;
				scrollBarV.CanDisplay = value;
				this.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*



	}
	//*-------------------------------------------------------------------------*

	////*-------------------------------------------------------------------------*
	////*	ScrollPanelControlDesigner																							*
	////*-------------------------------------------------------------------------*
	///// <summary>
	///// Custom designer for ScrollPanelControl.
	///// </summary>
	//public class ScrollPanelControlDesigner : ControlDesigner
	//{
	//	//*************************************************************************
	//	//*	Private																																*
	//	//*************************************************************************
	//	//*************************************************************************
	//	//*	Protected																															*
	//	//*************************************************************************
	//	//*************************************************************************
	//	//*	Public																																*
	//	//*************************************************************************
	//	//*-----------------------------------------------------------------------*
	//	//* Initialize																														*
	//	//*-----------------------------------------------------------------------*
	//	/// <summary>
	//	/// Initialize the control designer.
	//	/// </summary>
	//	/// <param name="component">
	//	/// Reference to the component to be initialized.
	//	/// </param>
	//	public override void Initialize(IComponent component)
	//	{
	//		base.Initialize(component);
	//		this.EnableDesignMode(
	//			((ScrollPanelControl)this.Control).ControlCanvas, "ControlCanvas");
	//	}
	//	//*-----------------------------------------------------------------------*
	//}
	////*-------------------------------------------------------------------------*


}
