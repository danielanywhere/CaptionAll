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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptionAll
{
	public class DropFormShadow
	{
		//private bool _isAeroEnabled = false;
		//private bool _isDraggingEnabled = false;
		private const int WM_NCHITTEST = 0x84;
		private const int WS_MINIMIZEBOX = 0x20000;
		private const int HTCLIENT = 0x1;
		private const int HTCAPTION = 0x2;
		private const int CS_DBLCLKS = 0x8;
		private const int CS_DROPSHADOW = 0x00020000;
		private const int WM_NCPAINT = 0x0085;
		private const int WM_ACTIVATEAPP = 0x001C;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public struct MARGINS
		{
			public int leftWidth;
			public int rightWidth;
			public int topHeight;
			public int bottomHeight;
		}

		[DllImport("dwmapi.dll")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

		[DllImport("dwmapi.dll")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

		[DllImport("dwmapi.dll")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsCompositionEnabled()
		{
			if(Environment.OSVersion.Version.Major < 6) return false;

			bool enabled;
			DwmIsCompositionEnabled(out enabled);

			return enabled;
		}

		[DllImport("dwmapi.dll")]
		private static extern int DwmIsCompositionEnabled(out bool enabled);

		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		private static extern IntPtr CreateRoundRectRgn
		(
				int nLeftRect,
				int nTopRect,
				int nRightRect,
				int nBottomRect,
				int nWidthEllipse,
				int nHeightEllipse
		 );

		private bool CheckIfAeroIsEnabled()
		{
			if(Environment.OSVersion.Version.Major >= 6)
			{
				int enabled = 0;
				DwmIsCompositionEnabled(ref enabled);

				return (enabled == 1) ? true : false;
			}
			return false;
		}

		public void ApplyShadows(Form form)
		{
			var v = 2;

			DwmSetWindowAttribute(form.Handle, 2, ref v, 4);

			MARGINS margins = new MARGINS()
			{
				bottomHeight = 1,
				leftWidth = 0,
				rightWidth = 0,
				topHeight = 0
			};

			DwmExtendFrameIntoClientArea(form.Handle, ref margins);
		}

	}
}