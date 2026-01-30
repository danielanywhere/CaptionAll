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
	//*	ScrollBarVControl																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Vertical scroll bar control.
	/// </summary>
	public partial class ScrollBarVControlVirt : UserControl
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
			float barHeight = 0;
			float netHeight = this.Height - (float)(mButtonHeight * 2);

			barHeight =
				Math.Max(netHeight - (mCanvasHeight - mViewPortHeight),
				mButtonHeight);
			mBarHeight = (int)barHeight;
			mMaximumValue = Math.Max(netHeight - barHeight, 0);
			if(barHeight > mButtonHeight)
			{
				mStepSize = 1f;
			}
			else
			{
				mStepSize =
					(mCanvasHeight - mViewPortHeight) /
					(netHeight - barHeight);
			}
			this.Visible = mCanDisplay && (barHeight < netHeight);
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
			mBarTop = (int)(mButtonHeight + mValue);
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
		//* OnCanvasHeightChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the CanvasHeightChanged event when the canvas height has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnCanvasHeightChanged(EventArgs e)
		{
			UpdateBarSize();
			CanvasHeightChanged?.Invoke(this, e);
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
				if(e.Y >= mBarTop && e.Y <= mBarTop + mBarHeight &&
					mBarHeight > 0)
				{
					mMouseOnBar = true;
				}
				else if(e.Y >= 0 && e.Y < mButtonHeight)
				{
					//	Left arrow button.
					ScrollDecrement();
				}
				else if(e.Y >= this.Height - mButtonHeight)
				{
					//	Right arrow button.
					ScrollIncrement();
				}
				else if(e.Y < mBarTop)
				{
					//	Click to the top of the bar. Page up.
					ScrollPagePrevious();
				}
				else
				{
					//	Click to the bottom of the bar. Page down.
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
			int dY = 0;
			float newValue = 0;

			base.OnMouseMove(e);
			if(mMouseIsDown)
			{
				dY = e.Y - mMouseLocationLast.Y;
				Debug.WriteLine($"Mouse Move " +
					$" Last: {mMouseLocationLast.Y} " +
					$" This: {e.Y} " +
					$" Difference: {dY}");
				newValue = mValue + dY;
				if(mMouseOnBar && newValue >= 0 && newValue <= mMaximumValue)
				{
					mValue += dY;
					mBarTop += dY;
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
			int iconWidth = 0;
			GraphicsPath path = null;
			Pen penHighlight = null;
			Pen penOutline = null;
			Rectangle rect = Rectangle.Empty;

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
				rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
				g.DrawRectangle(penHighlight, rect);
			}
			//	Top control button.
			rect = new Rectangle(0, 0, this.Width - 1, mButtonHeight);
			g.FillRectangle(brushControl, rect);
			g.DrawRectangle(penOutline, rect);
			iconWidth = Math.Min(rect.Width - 4, (mButtonHeight - 4) * 2);
			iconCenterX = rect.X + (rect.Width / 2);
			iconCenterY = rect.Y + (rect.Height / 2);
			path = new GraphicsPath(FillMode.Alternate);
			path.AddLines(
				new Point[]
				{
					new Point(iconCenterX, iconCenterY - (iconWidth / 3)),
					new Point(
						iconCenterX - (iconWidth / 2),
						iconCenterY + (iconWidth / 3)),
					new Point(
						iconCenterX + (iconWidth / 2),
						iconCenterY + (iconWidth / 3))
				});
			g.FillPath(brushArrow, path);
			//	Bar.
			rect = new Rectangle(0, mBarTop, this.Width - 1, mBarHeight);
			g.FillRectangle(brushBar, rect);
			g.DrawRectangle(penOutline, rect);
			//	Bottom control button.
			rect = new Rectangle(
				0, this.Height - mButtonHeight, this.Width - 1, mButtonHeight - 1);
			iconCenterY = rect.Y + (rect.Height / 2);
			g.FillRectangle(brushControl, rect);
			g.DrawRectangle(penOutline, rect);
			path = new GraphicsPath(FillMode.Alternate);
			path.AddLines(
				new Point[]
				{
					new Point(iconCenterX, iconCenterY + (iconWidth / 3)),
					new Point(
						iconCenterX + (iconWidth / 2),
						iconCenterY - (iconWidth / 3)),
					new Point(
						iconCenterX - (iconWidth / 2),
						iconCenterY - (iconWidth / 3))
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
		//* OnViewPortHeightChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the ViewPortHeightChanged event when the width of the viewport
		/// has changed.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected virtual void OnViewPortHeightChanged(EventArgs e)
		{
			UpdateBarSize();
			ViewPortHeightChanged?.Invoke(this, e);
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
		public ScrollBarVControlVirt()
		{
			this.DoubleBuffered = true;
			SetStyle(ControlStyles.Selectable, true);
			InitializeComponent();
			OnResize(new EventArgs());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BarHeight																															*
		//*-----------------------------------------------------------------------*
		private int mBarHeight = 24;
		/// <summary>
		/// Get the height of the sliding bar.
		/// </summary>
		public int BarHeight
		{
			get { return mBarHeight; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BarTop																																*
		//*-----------------------------------------------------------------------*
		private int mBarTop = 21;
		/// <summary>
		/// Get the current top position of the scrolling button bar.
		/// </summary>
		public int BarTop
		{
			get { return mBarTop; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ButtonHeight																													*
		//*-----------------------------------------------------------------------*
		private int mButtonHeight = 21;
		/// <summary>
		/// Get/Set the height of the control buttons.
		/// </summary>
		public int ButtonHeight
		{
			get { return mButtonHeight; }
			set { mButtonHeight = value; }
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
			set { mCanDisplay = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanvasHeight																													*
		//*-----------------------------------------------------------------------*
		private float mCanvasHeight = 10;
		/// <summary>
		/// Get/Set the height of the canvas being handled.
		/// </summary>
		public float CanvasHeight
		{
			get { return mCanvasHeight; }
			set
			{
				float original = mCanvasHeight;
				mCanvasHeight = value;
				if(original != value)
				{
					UpdateBarSize();
					OnCanvasHeightChanged(new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CanvasHeightChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the width of the canvas has changed.
		/// </summary>
		public event EventHandler CanvasHeightChanged;
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
				mValue + (mViewPortHeight / mStepSize) <= mMaximumValue)
			{
				mValue += mViewPortHeight / mStepSize;
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
				mValue - (mViewPortHeight / mStepSize) >= 0)
			{
				mValue -= mViewPortHeight / mStepSize;
				bChanged = true;
			}
			else if(mValue != 0)
			{
				mValue = 0f;
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
				float original = mValue;

				if(value >= 0f && value <= mMaximumValue)
				{
					mValue = value;
					mBarTop = (int)mValue + mButtonHeight;
					mViewPosition = mValue * mStepSize;
					if(original != value)
					{
						OnValueChanged(new EventArgs());
					}
					this.Invalidate();
				}
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
		//*	ViewPortHeight																												*
		//*-----------------------------------------------------------------------*
		private float mViewPortHeight = 10;
		/// <summary>
		/// Get/Set the width of the active view port.
		/// </summary>
		public float ViewPortHeight
		{
			get { return mViewPortHeight; }
			set
			{
				float original = mViewPortHeight;

				mViewPortHeight = value;
				if(original != value)
				{
					UpdateBarSize();
					OnViewPortHeightChanged(new EventArgs());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ViewPortHeightChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the view port width has changed.
		/// </summary>
		public event EventHandler ViewPortHeightChanged;
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
			get { return WindowMin + mViewPortHeight - 1f; }
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
