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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Speech.Recognition;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace CaptionAll
{
	//*-------------------------------------------------------------------------*
	//*	CaptionAllUtil																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Common functionality for the CaptionAll application.
	/// </summary>
	public class CaptionAllUtil
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private static readonly DateTime Jan1st1970 = new DateTime
			(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//* AnchorControlBottom																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the bottom-anchored location of the control, given the current
		/// location and the last known reference.
		/// </summary>
		/// <param name="control">
		/// Reference to the control whose new location will be updated.
		/// </param>
		/// <param name="referenceLocation">
		/// The last known location of the control.
		/// </param>
		/// <returns>
		/// Newly calculated vertical location for the control.
		/// </returns>
		public static Point AnchorControlBottom(Control control,
			Point referenceLocation)
		{
			Point delta = Point.Empty;
			Rectangle cr = Rectangle.Empty;
			Rectangle pr = Rectangle.Empty;
			Point result = Point.Empty;

			if(control != null && control.Parent != null)
			{
				pr = new Rectangle(control.Parent.Location, control.Parent.Size);
				cr = new Rectangle(control.Location, control.Size);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CalcTextDuration																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Calculate the duration of the sounds in the specified text, given the
		/// average durations for each type of sound.
		/// </summary>
		/// <param name="text">
		/// Text to calculate.
		/// </param>
		/// <param name="durationShort">
		/// Average duration of short sounds.
		/// </param>
		/// <param name="durationLong">
		/// Average duration of long sounds.
		/// </param>
		/// <returns>
		/// The total duration of the specified text, in seconds, if eligible.
		/// Otherwise, 0.
		/// </returns>
		public static double CalcTextDuration(string text, double durationShort,
			double durationLong)
		{
			double result = 0d;

			if(text?.Length > 0 && (durationShort > 0d || durationLong > 0d))
			{
				//	Legal information provided.
				result = (GetShortSoundCount(text) * durationShort) +
					(GetLongSoundCount(text) * durationLong);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//	NOTE: The Microsoft SpeechRecognitionEngine is not reliable enough
		//	for general use.

		////*-----------------------------------------------------------------------*
		////* CalcWordPlacement																											*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Using the source text known to be found in the audio burst, find the
		///// locations of each of the specified words within that audio.
		///// </summary>
		///// <param name="sourceText">
		///// The source text known to occcupy the specified audio burst.
		///// </param>
		///// <param name="wavFilename">
		///// Fully qualified filename of the WAV file to inspect.
		///// </param>
		///// <returns>
		///// Collection of found words and their relative positions within the
		///// audio burst.
		///// </returns>
		//public static WordRecognizedCollection CalcWordPlacement(
		//	string sourceText, string wavFilename)
		//{
		//	//Choices choices = null;
		//	Grammar grammar = null;
		//	GrammarBuilder grammarBuilder = null;
		//	//List<string> grammarList = new List<string>();
		//	//MatchCollection matches = null;
		//	//string text = "";
		//	WordRecognizedCollection words = new WordRecognizedCollection();

		//	if(sourceText?.Length > 0 &&
		//		wavFilename?.Length > 0 && File.Exists(wavFilename))
		//	{
		//		//	Legal parameters presented.
		//		////	Get the individual words.
		//		//matches = Regex.Matches(sourceText, "(?i:(?<word>[a-z']+))");
		//		//foreach(Match matchItem in matches)
		//		//{
		//		//	text = GetValue(matchItem, "word").ToLower();
		//		//	if(text.Length > 0 && !grammarList.Contains(text))
		//		//	{
		//		//		//	Add the current word to the grammar list if it is unique.
		//		//		grammarList.Add(text);
		//		//	}
		//		//}
		//		//grammarList.Sort();
		//		//grammarList = grammarList.OrderByDescending(x => x.Length).ToList();
		//		//grammarList.RemoveAll(x => x.Length < 6);
		//		//if(grammarList.Count > 0)
		//		//{
		//			using(SpeechRecognitionEngine recognizer =
		//				new SpeechRecognitionEngine(
		//					new System.Globalization.CultureInfo("en-US")))
		//			{
		//				//choices = new Choices(grammarList.ToArray());
		//				grammarBuilder = new GrammarBuilder(sourceText.ToLower().Replace("'", "").Replace(",", ""));
		//				//grammarBuilder.Append(choices);
		//				grammar = new Grammar(grammarBuilder);
		//				recognizer.LoadGrammar(grammar);
		//				recognizer.SpeechRecognized += words.SpeechRecognized;
		//				recognizer.SetInputToWaveFile(wavFilename);
		//				//recognizer.Recognize(new TimeSpan(0, 0, 10));
		//				recognizer.Recognize();
		//				//words.LastRecognition = DateTime.Now;
		//				//while(true)
		//				//{
		//				//	words.TimeSinceLastRecognition =
		//				//		DateTime.Now - words.LastRecognition;
		//				//	if(words.TimeSinceLastRecognition.TotalSeconds > 2d)
		//				//	{
		//				//		//	Time-out.
		//				//		recognizer.RecognizeAsyncCancel();
		//				//		break;
		//				//	}
		//				//}
		//			}
		//		//}
		//	}
		//	return words;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CenterControl																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Center the specified control within its parent.
		/// </summary>
		/// <param name="control">
		/// Reference to the control to be centered.
		/// </param>
		public static void CenterControl(Control control)
		{
			Rectangle cr = Rectangle.Empty;
			Rectangle pr = Rectangle.Empty;

			if(control?.Parent != null)
			{
				pr = new Rectangle(control.Parent.Location, control.Parent.Size);
				cr = new Rectangle(control.Location, control.Size);
				control.Location = new Point(
					(pr.Width / 2) - (cr.Width / 2),
					(pr.Height / 2) - (cr.Height / 2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CenterControlHorizontal																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Center the specified control horizontally within its parent.
		/// </summary>
		/// <param name="control">
		/// Reference to the control to be centered.
		/// </param>
		public static void CenterControlHorizontal(Control control)
		{
			Rectangle cr = Rectangle.Empty;
			Rectangle pr = Rectangle.Empty;

			if(control?.Parent != null)
			{
				pr = new Rectangle(control.Parent.Location, control.Parent.Size);
				cr = new Rectangle(control.Location, control.Size);
				control.Location = new Point(
					(pr.Width / 2) - (cr.Width / 2),
					control.Location.Y);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the contents of the specified string builder.
		/// </summary>
		/// <param name="builder">
		/// Reference to the string builder to clear.
		/// </param>
		public static void Clear(StringBuilder builder)
		{
			if(builder?.Length > 0)
			{
				builder.Remove(0, builder.Length);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonlyUsedWords																											*
		//*-----------------------------------------------------------------------*
		private static List<string> mCommonlyUsedWords = new List<string>();
		/// <summary>
		/// Get a reference to the list of commonly-used words in this session.
		/// </summary>
		public static List<string> CommonlyUsedWords
		{
			get { return mCommonlyUsedWords; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CompareText																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Using a Longest Common Subsequence (LCS) strategy, compare two text
		/// bodies and return a diff-style comparison collection that can be used
		/// to align the elements of the entire content.
		/// </summary>
		/// <param name="leftText">
		/// Reference to the series of tokens representing the left text.
		/// </param>
		/// <param name="rightText">
		/// Reference to the series of tokens representing the right text.
		/// </param>
		/// <returns>
		/// Reference to a text comparison collection where all of the tokens in
		/// both files have been accounted for after performing LCS matching.
		/// </returns>
		/// <remarks>
		/// This operation is case-sensitive. If you want to perform
		/// case-insensitive matching, please convert all text to the same case
		/// before submitting it to this process.
		/// </remarks>
		public static TextEntryComparisonCollection CompareText(
			TextEntryCollection leftText,
			TextEntryCollection rightText,
			MessageCallback messageHandler = null)
		{
			bool bSequence = false;
			DualIndexCollection dualIndex = new DualIndexCollection();
			List<DualIndexItem> dualIndexSorted = null;
			List<TextEntryItem> entriesL = new List<TextEntryItem>();
			List<TextEntryItem> entriesR = new List<TextEntryItem>();
			DualIndexCollection found = new DualIndexCollection();
			int i = 0;
			DualIndexItem indexItem = null;
			TextEntryItem itemL = null;
			TextEntryItem itemR = null;
			int j = 0;
			int jLow = 0;
			int minMatchCount = 10;
			DualIndexItem prevMatchR = null;
			TextEntryComparisonCollection result =
				new TextEntryComparisonCollection();

			if(leftText != null && rightText != null)
			{
				entriesL = leftText.FindAll(x =>
					x.ItemType == TextEntryItemType.Word);
				entriesR = rightText.FindAll(x =>
					x.ItemType == TextEntryItemType.Word);

				while(minMatchCount > 0)
				{
					if(messageHandler != null)
					{
						messageHandler($"Patterns of minimum width: {minMatchCount}");
					}
					//	Start from the highest expectation and work downward.
					bSequence = false;
					found.Clear();
					for(i = 0; i < entriesL.Count; i ++)
					{
						//	Review the entire left series.
						if(messageHandler != null && i > 0 && i % 100 == 0)
						{
							messageHandler(
								$"Patterns of minimum width: {minMatchCount}. " +
								$"{i} of {entriesL.Count}");
						}
						if(!dualIndex.Exists(x => x.Index1 == i))
						{
							//	This line has not already been found.
							jLow = 0;
							indexItem =
								DualIndexCollection.GetMaxIndex1LessThan(i, dualIndex, found);
							if(indexItem != null)
							{
								jLow = indexItem.Index2 + 1;
							}
							itemL = entriesL[i];
							for(j = jLow; j < entriesR.Count; j++)
							{
								//	Review the entire right series.
								prevMatchR = dualIndex.FirstOrDefault(x => x.Index2 == j);
								if(prevMatchR == null)
								{
									//	This line has not already been matched.
									itemR = entriesR[j];
									if(itemL.Text == itemR.Text)
									{
										//	Items are equal.
										if(!bSequence)
										{
											//	A sequence is running. Increment the success.
											bSequence = true;
										}
										found.Add(new DualIndexItem()
										{
											Index1 = i,
											Index2 = j
										});
										break;
									}
									else
									{
										//	Items don't match.
										if(bSequence)
										{
											//	A sequence was running.
											if(found.Count < minMatchCount)
											{
												found.Clear();
											}
											if(found.Count > 0)
											{
												dualIndex.AddRange(found);
												found.Clear();
											}
											bSequence = false;
										}
									}
								}
								else
								{
									//	This item has already been matched.
									if(prevMatchR.Index1 > i)
									{
										//	If the match occurred further down on the left side,
										//	then continue from there.
										//i = prevMatchR.Index1;
										break;
									}
								}
							}
						}
						else
						{
							//	Left line has already been found.
							if(bSequence)
							{
								//	A sequence was running.
								if(found.Count < minMatchCount)
								{
									found.Clear();
								}
								if(found.Count > 0)
								{
									dualIndex.AddRange(found);
									found.Clear();
								}
								bSequence = false;
							}
						}
					}
					if(bSequence)
					{
						//	A sequence was running.
						if(found.Count < minMatchCount)
						{
							found.Clear();
						}
						if(found.Count > 0)
						{
							dualIndex.AddRange(found);
							found.Clear();
						}
						bSequence = false;
					}
					minMatchCount--;
				}

				//	The current file set is showing up with a 95.5% to 97.7% match
				//	on a file of 5000 words.
				//	This procedure might be working! :-)
				dualIndexSorted = dualIndex.OrderBy(x => x.Index1).ToList();
				foreach(DualIndexItem dualIndexItem in dualIndexSorted)
				{
					result.Add(new TextEntryComparisonItem()
					{
						LeftItem = entriesL[dualIndexItem.Index1],
						RightItem = entriesR[dualIndexItem.Index2]
					});
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CountPattern																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the number of times the specified pattern occurs within the
		/// source.
		/// </summary>
		/// <param name="source">
		/// Source string to inspect.
		/// </param>
		/// <param name="pattern">
		/// Pattern to search for.
		/// </param>
		/// <returns>
		/// Count of times the specified pattern occurs within the caller's
		/// source.
		/// </returns>
		public static int CountPattern(string source, string pattern)
		{
			int result = 0;
			string text = "";

			if(source?.Length > 0 && pattern?.Length > 0)
			{
				text = source.Replace(pattern, "");
				result = (source.Length - text.Length) / pattern.Length;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CurrentTimeMillis																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the current UNIX time, in milliseconds.
		/// </summary>
		/// <returns>
		/// The current UNIX time, in milliseconds.
		/// </returns>
		public static long CurrentTimeMillis()
		{
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ExtractAudioSection																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Using FFMPEG, extract a section of audio and create a WAV file.
		/// </summary>
		/// <param name="mediaFilename">
		/// Fully qualified path and filename to the source media file.
		/// </param>
		/// <param name="timeBegin">
		/// The time at which to begin extracting.
		/// </param>
		/// <param name="timeEnd">
		/// The time at which to end the extraction.
		/// </param>
		/// <param name="outputFilename">
		/// Fully qualified path and filename of the WAV file to generate.
		/// </param>
		/// <param name="ffmpegPath">
		/// Path to the folder where the FFMPEG tools are stored on the local
		/// PC.
		/// </param>
		public static void ExtractAudioSection(string mediaFilename,
			TimeSpan timeBegin, TimeSpan timeEnd,
			string outputFilename,
			string ffmpegPath = "")
		{
			StringBuilder builder = new StringBuilder();
			List<string> consoles = null;

			if(mediaFilename?.Length > 0 && File.Exists(mediaFilename) &&
				timeBegin >= TimeSpan.Zero && timeEnd > TimeSpan.Zero &&
				outputFilename?.Length > 0)
			{
				if(!(ffmpegPath?.Length > 0))
				{
					//	Use the default FFMPEG path if not supplied.
					ffmpegPath = ResourceMain.ffmpegPath;
				}
				//	Legal values have been presented.
				builder.Append("-y");
				builder.Append($" -ss {ToText(timeBegin)}");
				builder.Append($" -t {ToText(timeEnd - timeBegin)}");
				builder.Append($" -i \"{mediaFilename}\"");
				builder.Append($" \"{outputFilename}\"");
				consoles = RunExe(ffmpegPath, "FFMPEG.EXE", builder.ToString());
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FitMaximumSize																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the maximum size the control can fit within the specified
		/// canvas.
		/// </summary>
		/// <param name="control">
		/// Reference to the control to measure.
		/// </param>
		/// <param name="canvasSize">
		/// Total allowable canvas size.
		/// </param>
		/// <returns>
		/// Maximum size allowable for the control, in the original W:H ratio.
		/// </returns>
		public static Size FitMaximumSize(Control control, Size canvasSize)
		{
			Size minimum = new Size(64, 64);
			Size original = new Size(1280, 720);
			float percent = 0f;
			Size result = Size.Empty;

			if(control != null)
			{
				//	Control has been provided.
				//original = control.Size;
				percent = (new List<float>
				{
					(float)minimum.Width / (float)original.Width,
					(float)minimum.Height / (float)original.Height
				}).Max();
				minimum = new Size(
					(int)Math.Floor(original.Width * percent),
					(int)Math.Floor(original.Height * percent));
				percent = (new List<float>
				{
					(float)canvasSize.Width / (float)original.Width,
					(float)canvasSize.Height / (float)original.Height
				}).Min();
				result = new Size(
					(int)Math.Floor(original.Width * percent),
					(int)Math.Floor(original.Height * percent));
				if(result.Width < minimum.Width || result.Height < minimum.Height)
				{
					percent = (new List<float>
					{
						(float)minimum.Width / (float)original.Width,
						(float)minimum.Height / (float)original.Height
					}).Min();
					result = new Size(
						(int)Math.Floor(original.Width * percent),
						(int)Math.Floor(original.Height * percent));
				}
			}
			Debug.WriteLine($"Original: {original} Result: {result}");
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FormatTimeSpan																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Format a time span with a general time syntax.
		/// </summary>
		/// <param name="rawTime">
		/// A raw time expression from freehand.
		/// </param>
		/// <returns>
		/// A consistently formatted time pattern.
		/// </returns>
		public static string FormatTimeSpan(string rawTime)
		{
			return FormatTimeSpan(ToTimeSpan(rawTime));
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Format a time span with a general time syntax.
		/// </summary>
		/// <param name="timeSpan">
		/// Reference to the time span to be formatted.
		/// </param>
		/// <returns>
		/// A consistently formatted time pattern.
		/// </returns>
		public static string FormatTimeSpan(TimeSpan timeSpan)
		{
			return timeSpan.ToString(@"hh\:mm\:ss\.fff");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetExtension																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the extension portion of the filename.
		/// </summary>
		/// <param name="filename">
		/// Filename to inspect.
		/// </param>
		/// <returns>
		/// The extension portion of the filename.
		/// </returns>
		public static string GetExtension(string filename)
		{
			int index = 0;
			string result = "";

			if(filename?.Length > 0)
			{
				index = filename.IndexOf('.');
				if(index > -1)
				{
					result = filename.Substring(index);
				}
				else
				{
					//	No extension.
					result = filename;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetFilenameNoExtension																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the specified filename with no extension.
		/// </summary>
		/// <param name="filename">
		/// The filename to process.
		/// </param>
		/// <returns>
		/// The caller's filename without the extension.
		/// </returns>
		public static string GetFilenameNoExtension(string filename)
		{
			return LeftOf(filename, ".");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetLongSoundCount																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the count of long-sound characters.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of entries to review.
		/// </param>
		/// <returns>
		/// Count of long-sound characters found in the text.
		/// </returns>
		public static int GetLongSoundCount(List<VttEntryItem> items)
		{
			int result = 0;

			if(items?.Count > 0)
			{
				foreach(VttEntryItem entryItem in items)
				{
					result += GetLongSoundCount(entryItem.Text);
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the count of long-sound characters.
		/// </summary>
		/// <param name="text">
		/// The text to inspect.
		/// </param>
		/// <returns>
		/// Count of long-sound characters found in the text.
		/// </returns>
		public static int GetLongSoundCount(string text)
		{
			int result = 0;

			if(text?.Length > 0)
			{
				result = Regex.Replace(text, "[^aefhilmnorsuwy,]+", "").Length;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetRelativeDirectory																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the relative portion of the directory name between the new and
		/// base names.
		/// </summary>
		/// <param name="baseName">
		/// Base directory name.
		/// </param>
		/// <param name="newName">
		/// Full name of the sub-directory.
		/// </param>
		/// <returns>
		/// Relative offset name of the two directories.
		/// </returns>
		public static string GetRelativeDirectory(string baseName, string newName)
		{
			int index = 0;
			string result = newName;

			if(baseName?.Length > 0 && newName?.Length > 0 &&
				newName.ToLower().StartsWith(baseName.ToLower()))
			{
				//	The new directory is an extension of the base.
				index = baseName.Length;
				result = newName.Substring(index, newName.Length - index);
				if(result.StartsWith(@"\") || result.StartsWith("/"))
				{
					result = result.Substring(1, result.Length - 1);
				}
				if(result.EndsWith(@"\") || result.EndsWith("/"))
				{
					result = result.Substring(0, result.Length - 1);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetRelativeFilename																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the relative form of the filename, given a base directory and
		/// a filename to prepare.
		/// </summary>
		/// <param name="baseName">
		/// Base directory name.
		/// </param>
		/// <param name="filename">
		/// Full or partial name of the file to prepare.
		/// </param>
		/// <returns>
		/// Relative filename.
		/// </returns>
		public static string GetRelativeFilename(string baseName, string filename)
		{
			List<string> baseElements = new List<string>();
			MatchCollection baseMatches = null;
			StringBuilder builder = new StringBuilder();
			int count = 0;
			List<string> filenameElements = new List<string>();
			MatchCollection filenameMatches = null;
			int index = 0;
			Match match = null;
			int matchIndex = 0;
			StringBuilder prefix = new StringBuilder();
			string result = (filename?.Length > 0 ? filename : "");

			//	Some portion of the base and file names was shared, allowing for
			//	a relative name.
			baseMatches = Regex.Matches(baseName.ToLower(),
				ResourceMain.rxFolderElement);
			foreach(Match matchItem in baseMatches)
			{
				baseElements.Add(GetValue(matchItem, "folder"));
			}
			filenameMatches = Regex.Matches(filename.ToLower(),
				ResourceMain.rxFolderElement);
			foreach(Match matchItem in filenameMatches)
			{
				filenameElements.Add(GetValue(matchItem, "folder"));
			}
			count = Math.Min(baseElements.Count, filenameElements.Count);
			for(index = 0; index < count; index ++)
			{
				if(baseElements[index] == filenameElements[index])
				{
					matchIndex = index;
				}
				else
				{
					if(matchIndex > 1)
					{
						//	Some portion of the filename is in common.
						if(matchIndex + 1 < count)
						{
							//	Additional levels exist past the current matching level.
							prefix.Append(
								Repeat("..\\", baseMatches.Count - matchIndex - 1));
						}
					}
					break;
				}
			}
			if(matchIndex > 1)
			{
				//	Complete the filename from the base.
				for(matchIndex++; matchIndex < filenameElements.Count; matchIndex++)
				{
					if(builder.Length > 0)
					{
						builder.Append('\\');
					}
					match = filenameMatches[matchIndex];
					builder.Append(filename.Substring(match.Index, match.Length));
				}
				if(prefix.Length > 0)
				{
					builder.Insert(0, prefix);
				}
				result = builder.ToString();
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
		/// <param name="items">
		/// Reference to the collection of items to count.
		/// </param>
		/// <returns>
		/// Count of short-sound characters found in the text.
		/// </returns>
		public static int GetShortSoundCount(List<VttEntryItem> items)
		{
			int result = 0;

			if(items?.Count > 0)
			{
				foreach(VttEntryItem entryItem in items)
				{
					result += GetShortSoundCount(entryItem.Text);
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the count of short-sound characters.
		/// </summary>
		/// <param name="text">
		/// Text to inspect.
		/// </param>
		/// <returns>
		/// Count of short-sound characters found in the text.
		/// </returns>
		public static int GetShortSoundCount(string text)
		{
			int result = 0;

			if(text?.Length > 0)
			{
				result = Regex.Replace(text, "[^bcdgjkpqtvxz]+", "").Length;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSpeedDialText																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the full reference list of speed-dial text.
		/// </summary>
		/// <returns>
		/// Full list of speed-dial settings.
		/// </returns>
		public static string GetSpeedDialText()
		{
			StringBuilder builder = new StringBuilder();
			int count = mCommonlyUsedWords.Count;
			int index = 0;

			for(index = 0; index < 9; index ++)
			{
				builder.Append($"Ctrl+{index + 1}: ");
				if(index < count)
				{
					if(index < 8)
					{
						builder.AppendLine(mCommonlyUsedWords[index]);
					}
					else
					{
						builder.Append(mCommonlyUsedWords[index]);
					}
				}
				else
				{
					builder.AppendLine("");
				}
			}

			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSpeedDialValue																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value associated with the currently pressed speed-dial keys.
		/// </summary>
		/// <param name="e">
		/// Key event arguments.
		/// </param>
		/// <returns>
		/// The text associated with the active speed-dial.
		/// </returns>
		public static string GetSpeedDialValue(KeyEventArgs e)
		{
			string text = "";

			if(!e.Handled && e.Modifiers == Keys.Control)
			{
				switch(e.KeyCode)
				{
					case Keys.D1:
						if(CommonlyUsedWords.Count > 0)
						{
							text = CommonlyUsedWords[0];
						}
						break;
					case Keys.D2:
						if(CommonlyUsedWords.Count > 1)
						{
							text = CommonlyUsedWords[1];
						}
						break;
					case Keys.D3:
						if(CommonlyUsedWords.Count > 2)
						{
							text = CommonlyUsedWords[2];
						}
						break;
					case Keys.D4:
						if(CommonlyUsedWords.Count > 3)
						{
							text = CommonlyUsedWords[3];
						}
						break;
					case Keys.D5:
						if(CommonlyUsedWords.Count > 4)
						{
							text = CommonlyUsedWords[4];
						}
						break;
					case Keys.D6:
						if(CommonlyUsedWords.Count > 5)
						{
							text = CommonlyUsedWords[5];
						}
						break;
					case Keys.D7:
						if(CommonlyUsedWords.Count > 6)
						{
							text = CommonlyUsedWords[6];
						}
						break;
					case Keys.D8:
						if(CommonlyUsedWords.Count > 7)
						{
							text = CommonlyUsedWords[7];
						}
						break;
					case Keys.D9:
						if(CommonlyUsedWords.Count > 8)
						{
							text = CommonlyUsedWords[8];
						}
						break;
				}
			}
			return text;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSpokenCharacterCount																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the total count of spoken characters in the caller's collection.
		/// That is, the total count of alpha characters with no punctuation and
		/// no spacing.
		/// </summary>
		/// <param name="items">
		/// Reference to a collection of items to inspect.
		/// </param>
		/// <returns>
		/// Count of all apha-only characters in the collection.
		/// </returns>
		public static int GetSpokenCharacterCount(List<VttEntryItem> items)
		{
			int result = 0;
			string text = "";

			if(items?.Count > 0)
			{
				foreach(VttEntryItem entryItem in items)
				{
					text = Regex.Replace(entryItem.Text, "[^a-zA-Z]+", "");
					result += text.Length;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetText																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the combined effective text content of the provided entries.
		/// </summary>
		/// <param name="items">
		/// Reference to a collection of items from which to gather text.
		/// </param>
		/// <returns>
		/// The effective text content of the specified collection of caption
		/// entries.
		/// </returns>
		public static string GetText(List<VttEntryItem> items)
		{
			StringBuilder builder = new StringBuilder();
			string text = "";

			if(items?.Count > 0)
			{
				foreach(VttEntryItem entryItem in items)
				{
					text = Regex.Replace(entryItem.Text, @"[\r\n]+", " ").Trim();
					if(builder.Length > 0)
					{
						builder.Append(' ');
					}
					builder.Append(text);
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetTextCharacterCount																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the count of text-oriented characters in the set of provided
		/// items.
		/// </summary>
		/// <param name="items">
		/// Reference to a collection of items to inspect.
		/// </param>
		/// <returns>
		/// Count of all text-oriented characters in the collection, including
		/// spaces, punctuation, etc.
		/// </returns>
		public static int GetTextCharacterCount(List<VttEntryItem> items)
		{
			int result = 0;
			if(items?.Count > 0)
			{
				foreach(VttEntryItem entryItem in items)
				{
					result += Regex.Replace(entryItem.Text, @"[\r\n]+", " ").Length;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified group member in the provided match.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be inspected.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(Match match, string groupName)
		{
			string result = "";

			if(match != null && match.Groups[groupName] != null &&
				match.Groups[groupName].Value != null)
			{
				result = match.Groups[groupName].Value;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the value found for the specified regular expression in the
		/// source string.
		/// </summary>
		/// <param name="source">
		/// Source string to search.
		/// </param>
		/// <param name="regex">
		/// Regular expression pattern to apply.
		/// </param>
		/// <param name="groupName">
		/// Name of the group containing the match to return.
		/// </param>
		/// <returns>
		/// Value, if found. Otherwise, an empty string.
		/// </returns>
		public static string GetValue(string source, string regex,
			string groupName)
		{
			Match match = null;
			string result = "";

			if(source?.Length > 0 && regex?.Length > 0 && groupName?.Length > 0)
			{
				match = Regex.Match(source, regex);
				result = GetValue(match, groupName);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InsertSpeedDialValue																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a speed-dial value in the provided textbox, where appropriate.
		/// </summary>
		/// <param name="textbox">
		/// Reference to the textbox within which the speed-dial value will be
		/// inserted.
		/// </param>
		/// <param name="e">
		/// Key event arguments.
		/// </param>
		public static void InsertSpeedDialValue(TextBox textbox, KeyEventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			int selectedIndex = 0;
			int selectedLength = 0;
			string text = "";

			if(textbox.Focused)
			{
				text = GetSpeedDialValue(e);
				if(text.Length > 0)
				{
					selectedIndex = textbox.SelectionStart;
					selectedLength = textbox.SelectionLength;
					if(selectedIndex > 0)
					{
						builder.Append(textbox.Text.Substring(0, selectedIndex));
					}
					builder.Append(text);
					if(textbox.TextLength >= selectedIndex + selectedLength)
					{
						builder.Append(
							textbox.Text.Substring(selectedIndex + selectedLength));
					}
					textbox.Text = builder.ToString();
					textbox.SelectionLength = 0;
					textbox.SelectionStart = selectedIndex + text.Length;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* LeftOf																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the portion of the source string to the left of the pattern.
		/// </summary>
		/// <param name="source">
		/// Source string to inspect.
		/// </param>
		/// <param name="pattern">
		/// Pattern to find within the source.
		/// </param>
		/// <returns>
		/// Portion of the source string to the left of the pattern, if found.
		/// Otherwise, the source string.
		/// </returns>
		public static string LeftOf(string source, string pattern)
		{
			int index = 0;
			string result = "";

			if(source?.Length > 0)
			{
				index = source.IndexOf(pattern);
				if(pattern?.Length > 0 && index > -1)
				{
					result = source.Substring(0, index);
				}
				else
				{
					result = source;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		public delegate void MessageCallback(string message);

		//*-----------------------------------------------------------------------*
		//* OpenMediaDialog																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Present the file open dialog for media files and return the result to
		/// the caller.
		/// </summary>
		/// <param name="dialogTitle">
		/// The title to display on the open file dialog.
		/// </param>
		/// <returns>
		/// Full path and filename of the media file to open, if a file was
		/// selected. Otherwise, an empty string.
		/// </returns>
		public static string OpenMediaDialog(string dialogTitle = "")
		{
			OpenFileDialog dialog = new OpenFileDialog();
			string result = "";

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".mp4";
			dialog.DereferenceLinks = true;
			dialog.Filter =
				"MP3 Files " +
				"(*.mp3)|" +
				"*.mp3;|" +
				"MP4 Files " +
				"(*.mp4)|" +
				"*.mp4;|" +
				"WAV Files " +
				"(*.wav)|" +
				"*.wav;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 1;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = (dialogTitle?.Length > 0 ?
				dialogTitle : "Open Media File (Optional)");
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.FileName;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OpenTextDialog																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Present the file open dialog for text files and return the result to
		/// the caller.
		/// </summary>
		/// <param name="dialogTitle">
		/// The title to display on the open file dialog.
		/// </param>
		/// <returns>
		/// Full path and filename of the text file to open, if a file was
		/// selected. Otherwise, an empty string.
		/// </returns>
		public static string OpenTextDialog(string dialogTitle = "")
		{
			OpenFileDialog dialog = new OpenFileDialog();
			string result = "";

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".txt";
			dialog.DereferenceLinks = true;
			dialog.Filter =
				"Text Files " +
				"(*.txt)|" +
				"*.txt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = (dialogTitle?.Length > 0 ?
				dialogTitle : "Open Text File");
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.FileName;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OpenVTTDialog																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Present the file open dialog for VTT files and return the result to
		/// the caller.
		/// </summary>
		/// <param name="dialogTitle">
		/// The title to display on the open file dialog.
		/// </param>
		/// <returns>
		/// Full path and filename of the VTT file to open, if a file was selected.
		/// Otherwise, an empty string.
		/// </returns>
		public static string OpenVTTDialog(string dialogTitle = "")
		{
			OpenFileDialog dialog = new OpenFileDialog();
			string result = "";

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".vtt";
			dialog.DereferenceLinks = true;
			dialog.Filter =
				"WebVTT Caption Files " +
				"(*.vtt)|" +
				"*.vtt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = (dialogTitle?.Length > 0 ?
				dialogTitle : "Open Caption File");
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.FileName;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OutputInfo																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Output a message to the console and to the provided string builder.
		/// </summary>
		/// <param name="message">
		/// The message to output.
		/// </param>
		/// <param name="builder">
		/// Reference to the builder to receive the message.
		/// </param>
		public static void OutputInfo(string message, StringBuilder builder)
		{
			Console.WriteLine(message);
			builder.Append(message);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RangeOverlap																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the two ranges overlap.
		/// </summary>
		/// <param name="start1">
		/// Starting coordinate of the first range.
		/// </param>
		/// <param name="end1">
		/// Ending coordinate of the first range.
		/// </param>
		/// <param name="start2">
		/// Starting coordinate of the second range.
		/// </param>
		/// <param name="end2">
		/// Ending coordinate of the second range.
		/// </param>
		/// <param name="limitLow">
		/// Lower limit.
		/// </param>
		/// <param name="limitHigh">
		/// High limit.
		/// </param>
		/// <returns>
		/// True if the ranges overlap. Otherwise, false.
		/// </returns>
		public static bool RangeOverlap(double start1, double end1,
			double start2, double end2,
			double limitLow = double.MinValue,
			double limitHigh = double.MaxValue)
		{
			bool result = false;

			if(start1 < end2 && end1 > start2 &&
				start1 >= limitLow && start2 >= limitLow &&
				end1 <= limitHigh && end2 <= limitHigh)
			{
				result = true;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Repeat																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Repeat the specified pattern.
		/// </summary>
		/// <param name="pattern">
		/// Pattern to repeat.
		/// </param>
		/// <param name="count">
		/// Count of times to repeat the pattern.
		/// </param>
		/// <returns>
		/// Resulting combination of repeated strings.
		/// </returns>
		public static string Repeat(string pattern, int count)
		{
			StringBuilder builder = new StringBuilder();
			int index = 0;

			for(index = 0; index < count; index ++)
			{
				builder.Append(pattern);
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RunExe																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Run a generic command and return the resulting strings.
		/// </summary>
		/// <param name="exePath">
		/// The path of the executable utility to run.
		/// </param>
		/// <param name="exeName">
		/// The name of the executable file to run.
		/// </param>
		/// <param name="arguments">
		/// Arguments to place on the command line.
		/// </param>
		/// <returns>
		/// Reference to the list of outputs generated during the operation.
		/// [0] - Standard.
		/// [1] - Error.
		/// </returns>
		public static List<string> RunExe(string exePath, string exeName,
			string arguments)
		{
			List<string> consoles = new List<string>();
			StringBuilder errorBuilder = new StringBuilder();
			Process process = null;
			StringBuilder standardBuilder = new StringBuilder();

			if(exePath?.Length > 0 && exeName?.Length > 0 &&
				File.Exists(Path.Combine(exePath, exeName)))
			{
				process = new Process();
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.FileName = Path.Combine(exePath, exeName);
				process.StartInfo.Arguments = arguments;

				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;

				process.OutputDataReceived += (sender, args) =>
					OutputInfo($"  {args.Data}", standardBuilder);
				process.ErrorDataReceived += (sender, args) =>
					OutputInfo($"  {args.Data}", errorBuilder);

				//process.OutputDataReceived +=
				//	(sender, args) => Console.WriteLine("  {0}", args.Data);
				//process.ErrorDataReceived +=
				//	(sender, args) => Console.WriteLine("  {0}", args.Data);
				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();

				consoles.Add(standardBuilder.ToString().Trim());
				consoles.Add(errorBuilder.ToString().Trim());
			}
			return consoles;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SharedPathLength																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the length of the base path shared by the new path.
		/// </summary>
		/// <param name="basePath">
		/// The base path to test.
		/// </param>
		/// <param name="newPath">
		/// The new path to compare.
		/// </param>
		/// <returns>
		/// Count of characters shared between the base and new paths.
		/// </returns>
		public static int SharedPathLength(string basePath, string newPath)
		{
			List<string> baseElements = null;
			int count = 0;
			int index = 0;
			Match match = null;
			int matchCount = 0;
			MatchCollection matches = null;
			List<string> newElements = null;
			int result = 0;

			if(basePath?.Length > 0 && newPath?.Length > 0)
			{
				baseElements = new List<string>();
				newElements = new List<string>();
				matches = Regex.Matches(basePath.ToLower(),
					ResourceMain.rxFolderElement);
				foreach(Match matchItem in matches)
				{
					baseElements.Add(GetValue(matchItem, "folder"));
				}
				matches = Regex.Matches(newPath.ToLower(),
					ResourceMain.rxFolderElement);
				foreach(Match matchItem in matches)
				{
					newElements.Add(GetValue(matchItem, "folder"));
				}
				count = Math.Min(baseElements.Count, newElements.Count);
				for(index = 0; index < count; index ++)
				{
					if(baseElements[index] == newElements[index])
					{
						matchCount++;
					}
					else
					{
						if(matchCount > 1 && matches.Count >= matchCount)
						{
							match = matches[matchCount - 1];
							result = match.Index + match.Length;
						}
						break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToDouble																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Double-precision floating point value. 0 if not convertible.
		/// </returns>
		public static double ToDouble(object value)
		{
			double result = 0d;
			if(value != null)
			{
				result = ToDouble(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Double-precision floating point value. 0 if not convertible.
		/// </returns>
		public static double ToDouble(string value)
		{
			double result = 0d;
			try
			{
				result = double.Parse(value);
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToFloat																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Single-precision floating point value. 0 if not convertible.
		/// </returns>
		public static float ToFloat(object value)
		{
			float result = 0f;
			if(value != null)
			{
				result = ToFloat(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Single-precision floating point value. 0 if not convertible.
		/// </returns>
		public static float ToFloat(string value)
		{
			float result = 0f;
			try
			{
				result = float.Parse(value);
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToMilliseconds																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert the caller's number of seconds to milliseconds.
		/// </summary>
		/// <param name="seconds">
		/// Number of seconds to translate.
		/// </param>
		/// <returns>
		/// Number of milliseconds represented by the caller's value.
		/// </returns>
		public static long ToMilliseconds(double seconds)
		{
			return (long)(seconds * 1000d);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Convert the caller's timespan to milliseconds.
		/// </summary>
		/// <param name="timeSpan">
		/// The timespan to inspect.
		/// </param>
		/// <returns>
		/// Total number of milliseconds in the caller's timespan.
		/// </returns>
		/// <remarks>
		/// In modern .NET versions, the result of this call is the same as
		/// (long)timespan.TotalMilliseconds.
		/// </remarks>
		public static long ToMilliseconds(TimeSpan timeSpan)
		{
			return (long)(timeSpan.Hours * 60 * 60 * 1000) +
				(long)(timeSpan.Minutes * 60 * 1000) +
				(long)(timeSpan.Seconds * 1000) +
				(long)(timeSpan.Milliseconds);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*-----------------------------------------------------------------------*
		public static double ToSeconds(long milliseconds)
		{
			return (double)(milliseconds / 1000d);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToText																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the general text representation of the provided time span.
		/// </summary>
		/// <param name="time">
		/// The timespan to format.
		/// </param>
		/// <returns>
		/// General SMPTE text representation of the specified time span.
		/// </returns>
		public static string ToText(TimeSpan time)
		{
			return time.ToString(@"hh\:mm\:ss\.fff");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*-----------------------------------------------------------------------*
		public static long ToTicks(double seconds)
		{
			return (long)(seconds * 1000d) * 10000;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToTimeSpan																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a time span corresponding to the caller's seconds.
		/// </summary>
		/// <param name="seconds">
		/// Decimal seconds.
		/// </param>
		/// <returns>
		/// Reference to a time span object containing the referenced time.
		/// </returns>
		public static TimeSpan ToTimeSpan(double seconds)
		{
			return new TimeSpan(ToTicks(seconds));
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpan(long ticks)
		{
			return new TimeSpan(ticks);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a time span corresponding to a text-based time expression.
		/// </summary>
		/// <param name="time">
		/// Time-formatted text.
		/// </param>
		/// <returns>
		/// Reference to a time span object containing the specified time.
		/// </returns>
		public static TimeSpan ToTimeSpan(string time)
		{
			float seconds = 0;
			TimeSpan timeSpan = new TimeSpan(0, 0, 0);

			if(time?.Length > 0)
			{
				if(time.IndexOfAny(new char[] { ':' }) == -1)
				{
					if(float.TryParse(time, out seconds))
					{
						timeSpan = new TimeSpan((long)(seconds * 1000 * 10000));
					}
					else
					{
						timeSpan = new TimeSpan(0, 0, 0);
					}
				}
				else if(time.Count(x => x == ':') == 1)
				{
					//	User is indicating minutes and seconds.
					time = $"00:{time}";
					if(!TimeSpan.TryParse(time, out timeSpan))
					{
						timeSpan = new TimeSpan(0, 0, 0);
					}
				}
				else
				{
					if(!TimeSpan.TryParse(time, out timeSpan))
					{
						//	A valid parseable timespan was not supplied.
						timeSpan = new TimeSpan(0, 0, 0);
					}
				}
			}
			return timeSpan;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WordOnly																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return only the non-punctuated word content of the caller's value.
		/// </summary>
		/// <param name="text">
		/// The text to convert.
		/// </param>
		/// <returns>
		/// The caller's word with all punctuation removed.
		/// </returns>
		public static string WordOnly(string text)
		{
			string result = "";

			if(text?.Length > 0)
			{
				result = text.Replace("\"", "").Replace("'", "").
					Replace("-", "").Replace("_", "").Replace(".", "");
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WordsToList																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a list to the caller containing only the sterilized,
		/// non-punctuated words in the provided text.
		/// </summary>
		/// <param name="text">
		/// The text inspect.
		/// </param>
		/// <returns>
		/// List of words found in the provided text, if found. Otherwise, an
		/// empty collection.
		/// </returns>
		public static List<string> WordsToList(string text)
		{
			MatchCollection matches = null;
			List<string> result = new List<string>();

			if(text?.Length > 0)
			{
				text = WordOnly(text);
				matches = Regex.Matches(text, "(?<word>[a-zA-Z]+)");
				foreach(Match matchItem in matches)
				{
					result.Add(GetValue(matchItem, "word"));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
