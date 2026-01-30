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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrollPanelVirtual
{
	//*-------------------------------------------------------------------------*
	//*	ScrollBarHControl																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Horizontal scroll bar control.
	/// </summary>
	public partial class ScrollBarHControlVirt : UserControl
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private bool mMouseOnBar = false;

		//*-----------------------------------------------------------------------*
		//* UpdateBarSize																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the size the bar relative to the control, canvas, and view port.
		/// </summary>
		private void UpdateBarSize()
		{
			float barWidth = 0;
			float netWidth = this.Width - (float)(mButtonWidth * 2);

			barWidth =
				Math.Max(netWidth - (mCanvasWidth - mViewPortWidth),
				mButtonWidth);
			mBarWidth = (int)barWidth;
			mMaximumValue = Math.Max(netWidth - barWidth, 0);
			if(barWidth > mButtonWidth)
			{
				mStepSize = 1f;
			}
			else
			{
				mStepSize =
					(mCanvasWidth - mViewPortWidth) /
					(netWidth - barWidth);
			}
			Trace.WriteLine($"ScrollH: UpdateBarSize width: {mBarWidth}");
			//mWindowMin = mStepSize * mValue;
			//mWindowMax = mWindowMin + mViewPortWidth - 1;
			this.Visible = mCanDisplay && (barWidth < netWidth);
			this.Invalidate();
			this.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateValueDependents																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update values that are dependent upon the Value property.
		/// </summary>
		/// <param name="valueEvent">
		/// Value indicating whether to fire the ValueChanged event.
		/// </param>
		/// <param name="redraw">
		/// Value indicating whether to redraw the control.
		/// </param>
		private void UpdateValueDependents(bool valueEvent = true,
			bool redraw = true)
		{
			mBarLeft = (int)(mButtonWidth + mValue);
			mViewPosition = mValue * mStepSize;
			if(valueEvent)
			{
				OnValueChanged(new EventArgs());
			}
			if(redraw)
			{
				this.Invalidate();
				this.Refresh();
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnCanvasWidthChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the CanvasWidthChanged event when the canvas width has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnCanvasWidthChanged(EventArgs e)
		{
			Trace.WriteLine("ScrollH: OnCanvasWidthChanged.");
			UpdateBarSize();
			CanvasWidthChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

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
			this.Invalidate();
			this.Refresh();
			base.OnGotFocus(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLostFocus																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ListFocus event when the control has lost the focus.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLostFocus(EventArgs e)
		{
			this.Invalidate();
			this.Refresh();
			base.OnLostFocus(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseDown																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseDown event when a mouse button has been depressed.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if(e.X >= mBarLeft && e.X <= mBarLeft + mBarWidth &&
					mBarWidth > 0)
				{
					mMouseOnBar = true;
				}
				else if(e.X >= 0 && e.X < mButtonWidth)
				{
					//	Left arrow button.
					ScrollDecrement();
				}
				else if(e.X >= this.Width - mButtonWidth)
				{
					//	Right arrow button.
					ScrollIncrement();
				}
				else if(e.X < mBarLeft)
				{
					//	Click to the left of the bar. Page up.
					ScrollPagePrevious();
				}
				else
				{
					//	Click to the right of the bar. Page down.
					ScrollPageNext();
				}
				mMouseIsDown = true;
			}
			Debug.WriteLine($"Mouse down. On bar: {mMouseOnBar}");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseMove																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseMove event when the mouse is moved over this control.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			int dX = 0;
			float newValue = 0;

			base.OnMouseMove(e);
			if(mMouseIsDown)
			{
				dX = e.X - mMouseLocationLast.X;
				Debug.WriteLine($"Mouse Move " +
					$" Last: {mMouseLocationLast.X} " +
					$" This: {e.X} " +
					$" Difference: {dX}");
				newValue = mValue + dX;
				if(mMouseOnBar && newValue >= 0 && newValue <= mMaximumValue)
				{
					mValue += dX;
					mBarLeft += dX;
					mViewPosition = mValue * mStepSize;
					OnValueChanged(new EventArgs());
					this.Invalidate();
					this.Refresh();
				}
			}
			mMouseLocationLast.X = e.X;
			mMouseLocationLast.Y = e.Y;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseUp																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseUp event when a mouse button has been released.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				mMouseOnBar = false;
				mMouseIsDown = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnMouseWheel																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the MouseWheel event when the mouse wheel has been adjusted.
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if((ModifierKeys & Keys.Control) == Keys.Control)
			{
				if(e.Delta < 0)
				{
					ScrollPageNext();
				}
				else if(e.Delta > 0)
				{
					ScrollPagePrevious();
				}
			}
			else
			{
				if(e.Delta < 0)
				{
					ScrollIncrement();
				}
				else if(e.Delta > 0)
				{
					ScrollDecrement();
				}
			}
			base.OnMouseWheel(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnPaint																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Paint event when the control is being painted.
		/// </summary>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Brush brushArrow = null;
			Brush brushBackground = null;
			Brush brushBar = null;
			Brush brushControl = null;
			Graphics g = e.Graphics;
			int iconCenterX = 0;
			int iconCenterY = 0;
			int iconHeight = 0;
			GraphicsPath path = null;
			Pen penHighlight = null;
			Pen penOutline = null;
			Rectangle rect = Rectangle.Empty;

			Trace.WriteLine("ScrollH.OnPaint");

			if(Enabled)
			{
				brushArrow = new SolidBrush(Color.FromArgb(134, 137, 153));
				brushBackground = new SolidBrush(Color.FromArgb(232, 232, 236));
				brushBar = new SolidBrush(Color.FromArgb(194, 195, 201));
				brushControl = new SolidBrush(Color.FromArgb(232, 232, 236));
				penHighlight = new Pen(Color.FromArgb(8, 174, 232), 1f);
				penOutline = new Pen(Color.Black, 1f);
			}
			else
			{
				brushArrow = new SolidBrush(Color.FromArgb(188, 191, 208));
				brushBackground = new SolidBrush(Color.FromArgb(232, 232, 236));
				brushBar = new SolidBrush(Color.FromArgb(232, 232, 236));
				brushControl = new SolidBrush(Color.FromArgb(232, 232, 236));
				penHighlight = new Pen(Color.FromArgb(8, 174, 232), 1f);
				penOutline = new Pen(Color.FromArgb(114, 117, 133), 1f);
			}

			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.CompositingQuality = CompositingQuality.HighQuality;

			base.OnPaint(e);
			//	Clear background.
			rect = new Rectangle(0, 0, this.Width, this.Height);
			g.FillRectangle(brushBackground, rect);
			//	Outline on selected.
			if(Focused)
			{
				rect = new Rectangle(0, 0, this.Width, this.Height - 1);
				g.DrawRectangle(penHighlight, rect);
			}
			//	Left control button.
			rect = new Rectangle(0, 0, mButtonWidth, this.Height - 1);
			g.FillRectangle(brushControl, rect);
			g.DrawRectangle(penOutline, rect);
			iconHeight = Math.Min(rect.Height - 4, (mButtonWidth - 4) * 2);
			iconCenterX = rect.X + (rect.Width / 2);
			iconCenterY = rect.Height / 2;
			path = new GraphicsPath(FillMode.Alternate);
			path.AddLines(
				new Point[]
				{
					new Point(iconCenterX - (iconHeight / 3), iconCenterY),
					new Point(
						iconCenterX + (iconHeight / 3),
						iconCenterY + (iconHeight / 2)),
					new Point(
						iconCenterX + (iconHeight / 3),
						iconCenterY - (iconHeight / 2))
				});
			g.FillPath(brushArrow, path);
			//	Bar.
			rect = new Rectangle(mBarLeft, 0, mBarWidth, this.Height - 1);
			g.FillRectangle(brushBar, rect);
			g.DrawRectangle(penOutline, rect);
			//	Right control button.
			rect = new Rectangle(
				this.Width - mButtonWidth, 0, mButtonWidth - 1, this.Height - 1);
			iconCenterX = rect.X + (rect.Width / 2);
			g.FillRectangle(brushControl, rect);
			g.DrawRectangle(penOutline, rect);
			path = new GraphicsPath(FillMode.Alternate);
			path.AddLines(
				new Point[]
				{
					new Point(iconCenterX + (iconHeight / 3), iconCenterY),
					new Point(
						iconCenterX - (iconHeight / 3),
						iconCenterY + (iconHeight / 2)),
					new Point(
						iconCenterX - (iconHeight / 3),
						iconCenterY - (iconHeight / 2))
				});
			g.FillPath(brushArrow, path);
		}
		//*-----------------------------------------------------------------------*

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
			Trace.WriteLine($"ScrollH.OnResize: {this.Width}, {this.Height}");
			UpdateBarSize();
			base.OnResize(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnValueChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ValueChanged event when the scroll value has been changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Invalidate();
			this.Refresh();
			ValueChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnViewPortWidthChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ViewPortWidthChanged event when the width of the viewport
		/// has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnViewPortWidthChanged(EventArgs e)
		{
			Trace.WriteLine("ScrollH.OnViewPortWidthChanged");
			UpdateBarSize();
			ViewPortWidthChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnViewPositionChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ViewPositionChanged event when the position of the view port
		/// upon the canvas has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnViewPositionChanged(EventArgs e)
		{
			this.Invalidate();
			this.Refresh();
			ViewPositionChanged?.Invoke(this, e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ProcessCmdKey																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Processes a command key.
		/// </summary>
		/// <param name="msg">
		/// A message, passed by reference, representing the window message to
		/// process.
		/// </param>
		/// <param name="keyData">
		/// One of the Keys values representing the key to process.
		/// </param>
		/// <returns>
		/// Whether the specified key was processed.
		/// </returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = false;

			switch(keyData)
			{
				case Keys.Down:
				case Keys.Right:
					ScrollIncrement();
					result = true;
					break;
				case Keys.Up:
				case Keys.Left:
					ScrollDecrement();
					result = true;
					break;
				case Keys.PageDown:
					ScrollPageNext();
					result = true;
					break;
				case Keys.PageUp:
					ScrollPagePrevious();
					result = true;
					break;
			}
			if(!result)
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the ScrollBarHControl Item.
		/// </summary>
		public ScrollBarHControlVirt()
		{
			this.DoubleBuffered = true;
			SetStyle(ControlStyles.Selectable, true);
			InitializeComponent();
			OnResize(new EventArgs());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BarLeft																																*
		//*-----------------------------------------------------------------------*
		private int mBarLeft = 21;
		/// <summary>
		/// Get the current left position of the scrolling button bar.
		/// </summary>
		public int BarLeft
		{
			get { return mBarLeft; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BarWidth																															*
		//*-----------------------------------------------------------------------*
		private int mBarWidth = 24;
		/// <summary>
		/// Get the width of the sliding bar.
		/// </summary>
		public int BarWidth
		{
			get { return mBarWidth; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ButtonWidth																														*
		//*-----------------------------------------------------------------------*
		private int mButtonWidth = 21;
		/// <summary>
		/// Get/Set the width of the control buttons.
		/// </summary>
		public int ButtonWidth
		{
			get { return mButtonWidth; }
			set { mButtonWidth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanDisplay																														*
		//*-----------------------------------------------------------------------*
		private bool mCanDisplay = true;
		/// <summary>
		/// Get/Set a value indicating whether this control can be displayed.
		/// </summary>
		public bool CanDisplay
		{
			get { return mCanDisplay; }
			set
			{
				Trace.WriteLine($"ScrollH.CanDisplay: {value}");
				mCanDisplay = value;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanvasWidth																														*
		//*-----------------------------------------------------------------------*
		private float mCanvasWidth = 10;
		/// <summary>
		/// Get/Set the width of the canvas being handled.
		/// </summary>
		public float CanvasWidth
		{
			get { return mCanvasWidth; }
			set
			{
				float original = mCanvasWidth;
				mCanvasWidth = value;
				Trace.WriteLine($"ScrollH.CanvasWidth: {value}");
				if(original != value)
				{
					UpdateBarSize();
					OnCanvasWidthChanged(new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CanvasWidthChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the width of the canvas has changed.
		/// </summary>
		public event EventHandler CanvasWidthChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MaximumValue																													*
		//*-----------------------------------------------------------------------*
		private float mMaximumValue = 0;
		/// <summary>
		/// Get the maximum allowable value in the current scope.
		/// </summary>
		public float MaximumValue
		{
			get { return mMaximumValue; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MouseIsDown																														*
		//*-----------------------------------------------------------------------*
		private bool mMouseIsDown = false;
		/// <summary>
		/// Get/Set a value indicating whether the mouse button is currently
		/// depressed on this control.
		/// </summary>
		public bool MouseIsDown
		{
			get { return mMouseIsDown; }
			set { mMouseIsDown = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MouseLocationLast																											*
		//*-----------------------------------------------------------------------*
		private Point mMouseLocationLast = new Point();
		/// <summary>
		/// Get/Set the last-recorded location of the mouse.
		/// </summary>
		public Point MouseLocationLast
		{
			get { return mMouseLocationLast; }
			set { mMouseLocationLast = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScrollDecrement																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Decrement the scroll value.
		/// </summary>
		public void ScrollDecrement()
		{
			bool bChanged = false;

			if(mValue >= mSpeed)
			{
				mValue -= mSpeed;
				bChanged = true;
			}
			else if(mValue > 0)
			{
				mValue = 0;
				bChanged = true;
			}
			if(bChanged)
			{
				UpdateValueDependents();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScrollIncrement																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Increment the scroll value.
		/// </summary>
		public void ScrollIncrement()
		{
			bool bChanged = false;

			if(mValue + mSpeed <= mMaximumValue)
			{
				mValue += mSpeed;
				bChanged = true;
			}
			else if(mValue < mMaximumValue)
			{
				mValue = mMaximumValue;
				bChanged = true;
			}
			if(bChanged)
			{
				UpdateValueDependents();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScrollPageNext																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Issue a next page command on the scroll.
		/// </summary>
		public void ScrollPageNext()
		{
			bool bChanged = false;

			if(mStepSize != 0f &&
				mValue + (mViewPortWidth / mStepSize) <= mMaximumValue)
			{
				mValue += mViewPortWidth / mStepSize;
				bChanged = true;
			}
			else if(mValue != mMaximumValue)
			{
				mValue = mMaximumValue;
				bChanged = true;
			}
			if(bChanged)
			{
				UpdateValueDependents();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ScrollPagePrevious																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Issue a previous page command on the scroll.
		/// </summary>
		public void ScrollPagePrevious()
		{
			bool bChanged = false;

			if(mStepSize != 0f &&
				mValue - (mViewPortWidth / mStepSize) >= 0)
			{
				mValue -= mViewPortWidth / mStepSize;
				bChanged = true;
			}
			else if(mValue != 0)
			{
				mValue = 0;
				bChanged = true;
			}
			if(bChanged)
			{
				UpdateValueDependents();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Speed																																	*
		//*-----------------------------------------------------------------------*
		private float mSpeed = 1.0f;
		/// <summary>
		/// Get/Set the scroll speed for incremental scrolls.
		/// </summary>
		public float Speed
		{
			get { return mSpeed; }
			set { mSpeed = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StepSize																															*
		//*-----------------------------------------------------------------------*
		private float mStepSize = 1f;
		/// <summary>
		/// Get the current view port step size per value on the scroll.
		/// </summary>
		public float StepSize
		{
			get { return mStepSize; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		private float mValue = 0;
		/// <summary>
		/// Get/Set the current scroll value.
		/// </summary>
		public float Value
		{
			get { return mValue; }
			set
			{
				float newValue = value;
				float original = mValue;

				if(newValue < 0f)
				{
					newValue = 0f;
				}
				if(newValue > mMaximumValue)
				{
					newValue = mMaximumValue;
				}
				mValue = newValue;
				mBarLeft = (int)mValue + mButtonWidth;
				mViewPosition = mValue * mStepSize;
				if(original != newValue)
				{
					OnValueChanged(new EventArgs());
				}
				this.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ValueChanged																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the scroll value has changed.
		/// </summary>
		public event EventHandler ValueChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ViewPortWidth																													*
		//*-----------------------------------------------------------------------*
		private float mViewPortWidth = 10;
		/// <summary>
		/// Get/Set the width of the active view port.
		/// </summary>
		public float ViewPortWidth
		{
			get { return mViewPortWidth; }
			set
			{
				float original = mViewPortWidth;

				Trace.WriteLine($"ScrollH.ViewPortWidth: {value}");
				mViewPortWidth = value;
				if(original != value)
				{
					UpdateBarSize();
					OnViewPortWidthChanged(new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ViewPortWidthChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the view port width has changed.
		/// </summary>
		public event EventHandler ViewPortWidthChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ViewPosition																													*
		//*-----------------------------------------------------------------------*
		private float mViewPosition = 0;
		/// <summary>
		/// Get/Set the translated view position of the view port on the canvas.
		/// </summary>
		public float ViewPosition
		{
			get { return mViewPosition; }
			set
			{
				float original = mViewPosition;

				mViewPosition = value;
				if(original != value)
				{
					OnViewPositionChanged(new EventArgs());
					if(mStepSize != 0f)
					{
						Value = (int)(mViewPosition / mStepSize);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ViewPositionChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the position of the view port has changed upon the canvas.
		/// </summary>
		public event EventHandler ViewPositionChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WindowMax																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the maximum relative window value at this setting.
		/// </summary>
		public float WindowMax
		{
			get { return WindowMin + mViewPortWidth - 1f; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WindowMin																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the minimum relative window value at this setting.
		/// </summary>
		public float WindowMin
		{
			get { return mStepSize * mValue; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
