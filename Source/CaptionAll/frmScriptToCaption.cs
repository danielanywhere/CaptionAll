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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using static CaptionAll.CaptionAllUtil;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	frmScriptToCaption																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Script to Caption assignment form.
	/// </summary>
	public partial class frmScriptToCaption : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private VttInfoItem mCaptionFile = null;
		private TextEntryCollection mTextFile = null;
		private string mTextFileContent = "";

		//*-----------------------------------------------------------------------*
		//* mnuEditAssign_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Assign Selected Script To Caption menu option has been
		/// clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditAssign_Click(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditOverlayCaptions_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Overlay Captions menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditOverlayCaptions_Click(object sender, EventArgs e)
		{
			bool bFound = false;
			TextEntryComparisonItem comparison = null;
			List<TextEntryComparisonItem> comparisons = null;
			DataSet data = null;
			List<int> indices = new List<int>();
			CaptionAllUtil.MessageCallback messageHandler = SetMessage;
			List<TextEntryItem> prevWords = new List<TextEntryItem>();
			List<TextEntryItem> remainingWords = new List<TextEntryItem>();
			DataTable table = null;
			TextEntryComparisonCollection textMatches = null;
			TextEntryItem word = null;
			List<TextEntryItem> words = null;

			dgCaption.Visible = false;
			rtScript.Enabled = false;
			this.Cursor = Cursors.WaitCursor;
			if(mCaptionFile != null && mTextFile != null)
			{
				foreach(TextEntryItem entryItem in mTextFile)
				{
					remainingWords.Add(entryItem);
				}
				textMatches =
					CompareText(mCaptionFile.Words, mTextFile, messageHandler);

				////	This section is for diagnostics only.
				//SetMessage("Updating diagnostic information...");
				//Console.WriteLine("Overlay captions. Matching words:");
				//foreach(TextEntryComparisonItem comparisonItem in textMatches)
				//{
				//	Trace.Write(" Left: ");
				//	Trace.Write(
				//		FormatTimeSpan(comparisonItem.LeftItem.VttEntry.TimeBegin));
				//	Trace.Write("; Right: ");
				//	Trace.Write(
				//		comparisonItem.RightItem.Position.ToString().PadLeft(5, ' '));
				//	Trace.WriteLine($"; Word: {comparisonItem.LeftItem.Text}");
				//}

				foreach(VttEntryItem entryItem in mCaptionFile.Entries)
				{
					//	Get the parsed elements of the original caption file.
					//if(FormatTimeSpan(entryItem.TimeBegin).EndsWith("03:45.948"))
					//{
					//	Trace.WriteLine("Break here. mnuEditOverlay");
					//}
					words = mCaptionFile.Words.FindAll(x => x.VttEntry == entryItem);
					if(words.Count > 0)
					{
						//	Get all of the successful matches for this caption.
						comparisons =
							textMatches.FindAll(x => words.Exists(y => y == x.LeftItem));
						if(comparisons.Count > 0)
						{
							//	Comparisons were found on this caption.
							prevWords.Clear();
							//	Bring in all of the words prior to the first comparison.
							if(comparisons.Count > 0)
							{
								comparison = comparisons[0];
								while(remainingWords.Count > 0 &&
									comparison.RightItem != remainingWords[0])
								{
									//	Word prior to matching caption.
									prevWords.Add(remainingWords[0]);
									remainingWords.RemoveAt(0);
								}
								if(remainingWords.Count > 0 &&
									comparison.RightItem == remainingWords[0])
								{
									//	The first matching word in the caption was found.
									prevWords.Add(remainingWords[0]);
									remainingWords.RemoveAt(0);
									if(comparisons.Count > 1)
									{
										comparison = comparisons[^1];
										while(remainingWords.Count > 0 &&
											comparison.RightItem != remainingWords[0])
										{
											//	Word within the caption area.
											prevWords.Add(remainingWords[0]);
											remainingWords.RemoveAt(0);
										}
										if(remainingWords.Count > 0 &&
											comparison.RightItem == remainingWords[0])
										{
											//	Last matching word in the caption was found.
											prevWords.Add(remainingWords[0]);
											remainingWords.RemoveAt(0);
										}
										while(remainingWords.Count > 0)
										{
											word = remainingWords[0];
											bFound = false;
											switch(word.ItemType)
											{
												case TextEntryItemType.Character:
													switch(word.Text)
													{
														case "[":
														case "{":
														case "(":
															//	We don't consume set openers here.
															break;
														default:
															prevWords.Add(remainingWords[0]);
															remainingWords.RemoveAt(0);
															bFound = true;
															break;
													}
													break;
												case TextEntryItemType.Space:
													prevWords.Add(remainingWords[0]);
													remainingWords.RemoveAt(0);
													bFound = true;
													break;
											}
											if(!bFound)
											{
												break;
											}
										}
									}
									else
									{
										//	Only one word within caption area. However, there may
										//	also be trailing characters.
										while(remainingWords.Count > 0)
										{
											word = remainingWords[0];
											bFound = false;
											switch(word.ItemType)
											{
												case TextEntryItemType.Character:
													switch(word.Text)
													{
														case "[":
														case "{":
														case "(":
															//	We don't consume set openers here.
															break;
														default:
															prevWords.Add(remainingWords[0]);
															remainingWords.RemoveAt(0);
															bFound = true;
															break;
													}
													break;
												case TextEntryItemType.Space:
													prevWords.Add(remainingWords[0]);
													remainingWords.RemoveAt(0);
													bFound = true;
													break;
											}
											if(!bFound)
											{
												break;
											}
										}
									}
								}
							}
						}
						if(prevWords.Count > 0)
						{
							//	Information was found for the caption.
							entryItem.Text =
								TextEntryCollection.
									RenderContent(mTextFileContent, prevWords).Trim();
						}
						else
						{
							entryItem.Text = "";
						}
					}
				}
				this.Cursor = Cursors.Default;
				//	Re-render the captions table so it can be reviewed.
				dgCaption.DataSource = null;
				data = VttInfoItem.ToDataSet(mCaptionFile,
					new string[] { "TimeBegin", "Text" });
				if(data.Tables.Count > 0)
				{
					table = data.Tables[0];
					dgCaption.DataSource = table;
				}
				this.Cursor = Cursors.WaitCursor;
				statMessage.Text = "Caption dataset updated...";
			}
			this.Cursor = Cursors.Default;
			rtScript.Enabled = true;
			dgCaption.Visible = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileClose_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Close menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileOpenCaption_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Open VTT Caption File menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileOpenCaption_Click(object sender, EventArgs e)
		{
			string content = "";
			DataSet data = null;
			string filename = OpenVTTDialog();
			DataTable table = null;

			if(filename.Length > 0 && File.Exists(filename))
			{
				//	A file was selected.
				content = File.ReadAllText(filename);
				mCaptionFile = VttInfoItem.Parse(content);
				foreach(TextEntryItem textEntryItem in mCaptionFile.Words)
				{
					if(textEntryItem.ItemType == TextEntryItemType.Word)
					{
						textEntryItem.Text = textEntryItem.Text.ToLower();
					}
				}
				data = VttInfoItem.ToDataSet(mCaptionFile,
					new string[] { "TimeBegin", "Text" });
				if(data.Tables.Count > 0)
				{
					table = data.Tables[0];
					dgCaption.DataSource = table;
				}
				else
				{
					dgCaption.DataSource = null;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileOpenScript_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Open TXT Script File menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileOpenScript_Click(object sender, EventArgs e)
		{
			string content = "";
			string filename = OpenTextDialog();

			if(filename.Length > 0 && File.Exists(filename))
			{
				//	A file was selected.
				content = File.ReadAllText(filename);
				//	Remove all line-feeds.
				content = Regex.Replace(content, @"[\r\n]+", @" ");
				content = Regex.Replace(content, @"\s{2,}", @" ");
				mTextFileContent = content;
				mTextFile = TextEntryCollection.ParseWords(content);
				foreach(TextEntryItem textEntryItem in mTextFile)
				{
					if(textEntryItem.ItemType == TextEntryItemType.Word)
					{
						textEntryItem.Text = textEntryItem.Text.ToLower();
					}
				}
				rtScript.Text = content;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetMessage																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the message on the status bar.
		/// </summary>
		/// <param name="message">
		/// Message to place.
		/// </param>
		private void SetMessage(string message)
		{
			statMessage.Text = message;
			this.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmScriptToCaption Item.
		/// </summary>
		public frmScriptToCaption()
		{
			InitializeComponent();

			//	Edit.
			mnuEditAssign.Click += mnuEditAssign_Click;
			mnuEditOverlayCaptions.Click += mnuEditOverlayCaptions_Click;

			//	File.
			mnuFileClose.Click += mnuFileClose_Click;
			mnuFileOpenCaption.Click += mnuFileOpenCaption_Click;
			mnuFileOpenScript.Click += mnuFileOpenScript_Click;

		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
