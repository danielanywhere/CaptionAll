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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CaptionBubbleEditorWF
{
	//*-------------------------------------------------------------------------*
	//*	CaptionObservableObject																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Object with observable characteristics.
	/// </summary>
	public class CaptionObservableObject : INotifyPropertyChanged
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	OnPropertyChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the PropertyChanged event when the value of a property has
		/// changed.
		/// </summary>
		/// <param name="name">
		/// Name of the changed property.
		/// </param>
		protected virtual void OnPropertyChanged(
			[CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OnPropertyChanging																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the PropertyChanging event when the value of a property will be
		/// changed.
		/// </summary>
		/// <param name="name">
		/// Name of the changed property.
		/// </param>
		protected virtual void OnPropertyChanging(
			[CallerMemberName] string name = null)
		{
			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//* PropertyChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the value of a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PropertyChanging																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the value of a property will be changed.
		/// </summary>
		public event PropertyChangingEventHandler PropertyChanging;
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
