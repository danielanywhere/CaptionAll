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
using System.Data;
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
	//*	VttInfoCollection																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of VttInfoItem Items.
	/// </summary>
	public class VttInfoCollection : List<VttInfoItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	VttInfoItem																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a single VTT file.
	/// </summary>
	public class VttInfoItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//*	Entries																																*
		//*-----------------------------------------------------------------------*
		private VttEntryCollection mEntries = new VttEntryCollection();
		/// <summary>
		/// Get a reference to the collection of entries on this VTT file.
		/// </summary>
		public VttEntryCollection Entries
		{
			get { return mEntries; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetAlphaCharacterCount																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the count of alpha characters in the collection.
		/// </summary>
		/// <returns>
		/// Count of alpha characters in the entire Entries collection.
		/// </returns>
		public int GetAlphaCharacterCount()
		{
			int result = 0;

			foreach(VttEntryItem entryItem in mEntries)
			{
				result += Regex.Replace(entryItem.Text, @"\W+", "").Length;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetLongSoundCount																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the count of long-sound characters.
		/// </summary>
		/// <returns>
		/// Count of long-sound characters found in the text.
		/// </returns>
		public int GetLongSoundCount()
		{
			int result = 0;

			foreach(VttEntryItem entryItem in mEntries)
			{
				result += Regex.Replace(entryItem.Text, "[^aeiouy]+", "").Length;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetShortSoundCount																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the count of short-sound characters.
		/// </summary>
		/// <returns>
		/// Count of short-sound characters found in the text.
		/// </returns>
		public int GetShortSoundCount()
		{
			int result = 0;

			foreach(VttEntryItem entryItem in mEntries)
			{
				result += Regex.Replace(entryItem.Text,
					"[^bcdefghjklmnpqrstvwxz]+", "").Length;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetTotalCaptionedTime																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the total amount of captioned time from the loaded entries.
		/// </summary>
		/// <returns>
		/// Total amount of captioned time.
		/// </returns>
		public TimeSpan GetTotalCaptionedTime()
		{
			TimeSpan result = TimeSpan.Zero;

			foreach(VttEntryItem entryItem in mEntries)
			{
				if(entryItem.TimeEnd > entryItem.TimeBegin)
				{
					result += (entryItem.TimeEnd - entryItem.TimeBegin);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetTotalFileTime																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the total time in the file.
		/// </summary>
		/// <returns>
		/// The total time spent in the file.
		/// </returns>
		public TimeSpan GetTotalFileTime()
		{
			TimeSpan result = TimeSpan.Zero;

			if(mEntries.Count > 0)
			{
				result = mEntries[^1].TimeEnd;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Parse																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the VTT file and return the corresponding information.
		/// </summary>
		/// <param name="content">
		/// The VTT content to parse.
		/// </param>
		/// <returns>
		/// Reference to information about the VTT file.
		/// </returns>
		public static VttInfoItem Parse(string content)
		{
			bool bHandled = false;
			VttEntryItem entry = null;
			int index = 0;
			VttInfoItem info = new VttInfoItem();
			string line = "";
			Match match = null;
			StringReader reader = null;
			string text = "";
			TextEntryCollection textEntries = null;
			TimeSpan timeValue = TimeSpan.Zero;

			if(content?.Length > 0)
			{
				reader = new StringReader(content);
				line = reader.ReadLine();
				while(line != null)
				{
					line = line.Trim();
					if(line.Length > 0)
					{
						bHandled = false;
						if(!bHandled)
						{
							//	Identification line.
							match = Regex.Match(line, ResourceMain.rxVTTMSID);
							if(match.Success)
							{
								entry = new VttEntryItem();
								info.mEntries.Add(entry);

								entry.GroupName = GetValue(match, "groupName");
								entry.Id = match.Value;
								if(int.TryParse(GetValue(match, "relIndex"), out index))
								{
									entry.RelativeIndex = index;
								}
								bHandled = true;
							}
						}
						if(!bHandled)
						{
							//	Timestamp line.
							match = Regex.Match(line, ResourceMain.rxVTTTimeStamp);
							if(match.Success)
							{
								if(entry == null)
								{
									//	Create the entry if an identification was not specified.
									entry = new VttEntryItem();
									info.mEntries.Add(entry);
								}

								text = GetValue(match, "timeBegin");
								if(TimeSpan.TryParse(text, out timeValue))
								{
									entry.TimeBegin = timeValue;
								}

								text = GetValue(match, "timeEnd");
								if(TimeSpan.TryParse(text, out timeValue))
								{
									entry.TimeEnd = timeValue;
								}
								bHandled = true;
							}
						}
						if(!bHandled && entry != null)
						{
							//	Text line.
							if(entry.Text.Length > 0)
							{
								entry.Text += "\r\n";
							}
							entry.Text += line;
							bHandled = true;
						}
					}
					else
					{
						//	Make sure the previous entry is not reused.
						entry = null;
					}
					line = reader.ReadLine();
				}
			}
			foreach(VttEntryItem entryItem in info.mEntries)
			{
				//	Parse the words of each caption.
				textEntries = TextEntryCollection.ParseWords(entryItem.Text);
				foreach(TextEntryItem textEntryItem in textEntries)
				{
					//	Add the association to this caption.
					textEntryItem.VttEntry = entryItem;
					info.mWords.Add(textEntryItem);
				}
			}
			foreach(VttEntryItem entryItem in info.mEntries)
			{
				if(!(entryItem.GroupName?.Length > 0))
				{
					//	No group name has been assigned.
					entryItem.GroupName = Guid.NewGuid().ToString("D").ToLower();
				}
			}
			return info;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToDataSet																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert the caller's VTT info object to a dataset.
		/// </summary>
		/// <param name="info">
		/// Reference to the information item to be translated.
		/// </param>
		/// <param name="columns">
		/// Optional columns override. By default, all columns are included. The
		/// choices are as follows.
		/// <list type="bullet">
		/// <item>TimeBegin. The start time.</item>
		/// <item>TimeEnd. The end time.</item>
		/// <item>GroupName. Name of the caption group to which this item
		/// belongs.</item>
		/// <item>RelativeIndex. Index of this clip within the group.</item>
		/// <item>Id. The globally unique identification of this clip.</item>
		/// <item>Text. Entry text.</item>
		/// </list>
		/// </param>
		/// <returns>
		/// Reference to a dataset containing a table named 'CaptionItem' where the
		/// columns of each row contain the properties of each of the caption
		/// enties in the Entries collection of the caller's info item, if found.
		/// Otherwise, an empty dataset named 'CaptionData'.
		/// </returns>
		public static DataSet ToDataSet(VttInfoItem info, string[] columns = null)
		{
			DataSet data = new DataSet("CaptionData");
			DataRow row = null;
			DataTable table = null;
			string[] tableColumns = new string[0];

			if(info != null)
			{
				if(columns == null)
				{
					tableColumns = new string[]
					{
						"TimeBegin", "TimeEnd",
						"GroupName", "RelativeIndex", "Id",
						"Text"
					};
				}
				else
				{
					tableColumns = columns;
				}
				table = data.Tables.Add();
				table.TableName = "CaptionItem";
				foreach(string tableColumnName in tableColumns)
				{
					table.Columns.Add(tableColumnName, typeof(string));
				}
				foreach(VttEntryItem entryItem in info.Entries)
				{
					row = table.NewRow();
					foreach(string tableColumnName in tableColumns)
					{
						switch(tableColumnName)
						{
							case "GroupName":
								row.SetField<string>("GroupName", entryItem.GroupName);
								break;
							case "Id":
								row.SetField<string>("Id", entryItem.Id);
								break;
							case "RelativeIndex":
								row.SetField<string>("RelativeIndex",
									entryItem.RelativeIndex.ToString());
								break;
							case "Text":
								row.SetField<string>("Text", entryItem.Text);
								break;
							case "TimeBegin":
								row.SetField<string>("TimeBegin",
									FormatTimeSpan(entryItem.TimeBegin));
								break;
							case "TimeEnd":
								row.SetField<string>("TimeEnd",
									FormatTimeSpan(entryItem.TimeEnd));
								break;
						}
					}
					table.Rows.Add(row);
				}
				table.AcceptChanges();
			}
			return data;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this file.
		/// </summary>
		/// <returns>
		/// The full VTT string representation of this item.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine("WEBVTT");
			builder.AppendLine("");
			foreach(VttEntryItem entryItem in this.mEntries)
			{
				if(entryItem.Id.Length > 0)
				{
					builder.AppendLine(entryItem.Id);
				}
				builder.Append(ToText(entryItem.TimeBegin));
				builder.Append(" --> ");
				builder.AppendLine(ToText(entryItem.TimeEnd));
				builder.AppendLine(entryItem.Text);
				builder.AppendLine("");
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Words																																	*
		//*-----------------------------------------------------------------------*
		private TextEntryCollection mWords = new TextEntryCollection();
		/// <summary>
		/// Get a reference to the collection of words parsed on all entries of
		/// this container.
		/// </summary>
		public TextEntryCollection Words
		{
			get { return mWords; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
