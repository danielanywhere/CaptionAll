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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace dotExcel
{
	//*-------------------------------------------------------------------------*
	//*	ExcelReader																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Stand-alone Excel data reader / writer using OpenXml.
	/// </summary>
	public class ExcelFile
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* CreateCellReference																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create the cell reference for the specified cell.
		/// </summary>
		/// <param name="columnIndex">
		/// 0-based column index.
		/// </param>
		/// <param name="rowIndex">
		/// 0-based row index.
		/// </param>
		/// <returns>
		/// 1-based column index, in the form of A1.
		/// </returns>
		private static string CreateCellReference(int columnIndex, int rowIndex)
		{

			string result = "";
			int value = (columnIndex / 26);

			if(value > 0)
			{
				result = ((char)(value + 65)).ToString();
			}
			value = (columnIndex + 1) % 26;
			result += ((char)(value + 65)).ToString();
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CreateStylesheet																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create the stylesheet object so cells can refer to their own individual
		/// styles.
		/// </summary>
		/// <returns>
		/// Newly created and configured stylesheet.
		/// </returns>
		/// <remarks>This information was found at:
		/// <see cref="https://jason-ge.medium.com/create-excel-using-openxml-in-net-6-3b601ddf48f7"/>
		/// </remarks>
		private static Stylesheet CreateStylesheet()
		{
			Stylesheet stylesheet = new Stylesheet();

			//	#region Number format
			uint DATETIME_FORMAT = 164;
			uint DIGITS4_FORMAT = 165;
			var numberingFormats = new NumberingFormats();
			numberingFormats.Append(new NumberingFormat // Datetime format
			{
				NumberFormatId = UInt32Value.FromUInt32(DATETIME_FORMAT),
				FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss")
			});
			numberingFormats.Append(new NumberingFormat // four digits format
			{
				NumberFormatId = UInt32Value.FromUInt32(DIGITS4_FORMAT),
				FormatCode = StringValue.FromString("0000")
			});
			numberingFormats.Count = UInt32Value.FromUInt32((uint)numberingFormats.ChildElements.Count);
			//	#endregion

			//	#region Fonts
			var fonts = new Fonts();
			fonts.Append(new DocumentFormat.OpenXml.Spreadsheet.Font()  // Font index 0 - default
			{
				FontName = new FontName { Val = StringValue.FromString("Calibri") },
				FontSize = new FontSize { Val = DoubleValue.FromDouble(10) }
			});
			fonts.Append(new DocumentFormat.OpenXml.Spreadsheet.Font()  // Font index 1
			{
				FontName = new FontName { Val = StringValue.FromString("Arial") },
				FontSize = new FontSize { Val = DoubleValue.FromDouble(10) },
				Bold = new Bold()
			});
			fonts.Count = UInt32Value.FromUInt32((uint)fonts.ChildElements.Count);
			//	#endregion

			//	#region Fills
			var fills = new Fills();
			fills.Append(new Fill() // Fill index 0
			{
				PatternFill = new PatternFill { PatternType = PatternValues.None }
			});
			fills.Append(new Fill() // Fill index 1
			{
				PatternFill = new PatternFill { PatternType = PatternValues.Gray125 }
			});
			fills.Append(new Fill() // Fill index 2
			{
				PatternFill = new PatternFill
				{
					PatternType = PatternValues.Solid,
					ForegroundColor = TranslateForeground(System.Drawing.Color.LightBlue),
					BackgroundColor = new BackgroundColor { Rgb = TranslateForeground(System.Drawing.Color.LightBlue).Rgb }
				}
			});
			fills.Append(new Fill() // Fill index 3
			{
				PatternFill = new PatternFill
				{
					PatternType = PatternValues.Solid,
					ForegroundColor = TranslateForeground(System.Drawing.Color.LightSkyBlue),
					BackgroundColor = new BackgroundColor { Rgb = TranslateForeground(System.Drawing.Color.LightBlue).Rgb }
				}
			});
			fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);
			//	#endregion

			//	#region Borders
			var borders = new Borders();
			borders.Append(new Border   // Border index 0: no border
			{
				LeftBorder = new LeftBorder(),
				RightBorder = new RightBorder(),
				TopBorder = new TopBorder(),
				BottomBorder = new BottomBorder(),
				DiagonalBorder = new DiagonalBorder()
			});
			borders.Append(new Border    //Boarder Index 1: All
			{
				LeftBorder = new LeftBorder { Style = BorderStyleValues.Thin },
				RightBorder = new RightBorder { Style = BorderStyleValues.Thin },
				TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
				BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
				DiagonalBorder = new DiagonalBorder()
			});
			borders.Append(new Border   // Boarder Index 2: Top and Bottom
			{
				LeftBorder = new LeftBorder(),
				RightBorder = new RightBorder(),
				TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
				BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
				DiagonalBorder = new DiagonalBorder()
			});
			borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);
			//	#endregion

			//	#region Cell Style Format
			var cellStyleFormats = new CellStyleFormats();
			cellStyleFormats.Append(new CellFormat  // Cell style format index 0: no format
			{
				NumberFormatId = 0,
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0
			});
			cellStyleFormats.Count = UInt32Value.FromUInt32((uint)cellStyleFormats.ChildElements.Count);
			//	#endregion

			//	#region Cell format
			var cellFormats = new CellFormats();
			cellFormats.Append(new CellFormat());    // Cell format index 0
			cellFormats.Append(new CellFormat   // CellFormat index 1
			{
				NumberFormatId = 14,        // 14 = 'mm-dd-yy'. Standard Date format;
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true)
			});
			cellFormats.Append(new CellFormat   // Cell format index 2: Standard Number format with 2 decimal placing
			{
				NumberFormatId = 4,        // 4 = '#,##0.00';
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true)
			});
			cellFormats.Append(new CellFormat   // Cell formt index 3
			{
				NumberFormatId = DATETIME_FORMAT,        // 164 = 'dd/mm/yyyy hh:mm:ss'. Standard Datetime format;
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true)
			});
			cellFormats.Append(new CellFormat   // Cell format index 4
			{
				NumberFormatId = 3, // 3   #,##0
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true)
			});
			cellFormats.Append(new CellFormat    // Cell format index 5
			{
				NumberFormatId = 4, // 4   #,##0.00
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true)
			});
			cellFormats.Append(new CellFormat   // Cell format index 6
			{
				NumberFormatId = 10,    // 10  0.00 %,
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true)
			});
			cellFormats.Append(new CellFormat   // Cell format index 7
			{
				NumberFormatId = DIGITS4_FORMAT,    // Format cellas 4 digits. If less than 4 digits, prepend 0 in front
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true)
			});
			cellFormats.Append(new CellFormat   // Cell format index 8: Cell header
			{
				NumberFormatId = 49,
				FontId = 1,
				FillId = 3,
				BorderId = 2,
				FormatId = 0,
				ApplyNumberFormat = BooleanValue.FromBoolean(true),
				Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center }
			});
			cellFormats.Count = UInt32Value.FromUInt32((uint)cellFormats.ChildElements.Count);
			//	#endregion

			stylesheet.Append(numberingFormats);
			stylesheet.Append(fonts);
			stylesheet.Append(fills);
			stylesheet.Append(borders);
			stylesheet.Append(cellStyleFormats);
			stylesheet.Append(cellFormats);

			#region Cell styles
			var css = new CellStyles();
			css.Append(new CellStyle
			{
				Name = StringValue.FromString("Normal"),
				FormatId = 0,
				BuiltinId = 0
			});
			css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
			stylesheet.Append(css);
			#endregion

			var dfs = new DifferentialFormats { Count = 0 };
			stylesheet.Append(dfs);
			var tss = new TableStyles
			{
				Count = 0,
				DefaultTableStyle = StringValue.FromString("TableStyleMedium9"),
				DefaultPivotStyle = StringValue.FromString("PivotStyleLight16")
			};
			stylesheet.Append(tss);

			return stylesheet;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCellValue																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified cell.
		/// </summary>
		/// <param name="workbook">
		/// Reference to the spreadsheet document to read.
		/// </param>
		/// <param name="cell">
		/// Reference to the cell to be read.
		/// </param>
		/// <returns>
		/// String value of the specified cell.
		/// </returns>
		private static string GetCellValue(SpreadsheetDocument workbook, Cell cell)
		{
			string value = "";

			if(cell != null)
			{
				if(cell.CellValue != null)
				{
					value = cell.CellValue.InnerText;
				}
				if(cell.DataType != null &&
					cell.DataType.Value == CellValues.SharedString)
				{
					//value = workbook.WorkbookPart.SharedStringTablePart.
					//	SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
					value = workbook.WorkbookPart.SharedStringTablePart.
						SharedStringTable.ChildElements[int.Parse(value)].InnerText;
				}
			}
			return value;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TranslateForeground																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Translate to OpenXml foreground color from system drawing color.
		/// </summary>
		/// <param name="fillColor">
		/// System drawing color to translate.
		/// </param>
		/// <returns>
		/// OpenXml.Spreadsheet.ForegroundColor.
		/// </returns>
		/// <remarks>This code was found at:
		/// <see cref="https://jason-ge.medium.com/create-excel-using-openxml-in-net-6-3b601ddf48f7"/>
		/// </remarks>
		private static ForegroundColor TranslateForeground(
			System.Drawing.Color fillColor)
		{
			return new ForegroundColor()
			{
				Rgb = new HexBinaryValue()
				{
					Value =
						System.Drawing.ColorTranslator.ToHtml(
						System.Drawing.Color.FromArgb(
								fillColor.A,
								fillColor.R,
								fillColor.G,
								fillColor.B)).Replace("#", "")
				}
			};
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//*	ReadWorkbook																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the content of the specified Excel file.
		/// </summary>
		/// <param name="filename">
		/// Path and filename of the Excel file to read.
		/// </param>
		/// <param name="dataHasHeaders">
		/// Value indicating whether the first row of the Excel sheet has headers.
		/// </param>
		/// <returns>
		/// Reference to a DataSet where each table represents the contents of
		/// an individual sheet in the specified Excel file, if successful.
		/// Otherwise, an empty DataSet. If an error occurred while attempting to
		/// read the data, the data set's ExtendedProperties collection will
		/// contain an "ErrorMessage" entry describing the error.
		/// </returns>
		public static DataSet ReadWorkbook(string filename, bool dataHasHeaders)
		{
			//int colIndex = 0;
			string colName = "";
			Dictionary<string, string> colNames = new Dictionary<string, string>();
			int counter = 0;
			DataSet data = new DataSet("ExcelData");
			List<string> headers = new List<string>();
			string key = "";
			OpenSettings openSettings = new OpenSettings()
			{
				AutoSave = false
			};
			DataRow row = null;
			IEnumerable<Row> rows = null;
			Sheets sheets = null;
			DataTable table = null;
			Worksheet worksheet = null;

			try
			{
				using(SpreadsheetDocument workbook =
					SpreadsheetDocument.Open(filename, false, openSettings))
				{
					if(workbook.WorkbookPart != null)
					{
						sheets = workbook.WorkbookPart.Workbook.Sheets;
						foreach(Sheet sheetItem in sheets.Cast<Sheet>())
						{
							table = new DataTable($"{sheetItem.Name}");
							data.Tables.Add(table);
							Console.WriteLine($"Table: {sheetItem.Name}");
							worksheet = ((WorksheetPart)workbook.WorkbookPart.
								GetPartById(sheetItem.Id.Value)).Worksheet;
							rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
							counter = 0;
							colNames.Clear();
							foreach(Row rowItem in rows)
							{
								//	Read the first row as header.
								if(counter == 0)
								{
									//colIndex = 1;
									foreach(Cell cellItem in rowItem.Descendants<Cell>())
									{
										key = Regex.Replace(cellItem.CellReference,
											@"(?<f>[a-zA-Z]+)\d+", "${f}");
										colName =
											(dataHasHeaders ?
											GetCellValue(workbook, cellItem) : key);
										colNames.Add(key, colName);
										Console.WriteLine($" Column: {colName}");
										headers.Add(colName);
										table.Columns.Add(colName);
									}
									if(!dataHasHeaders)
									{
										row = table.NewRow();
										//colIndex = 0;
										foreach(Cell cellItem in rowItem.Descendants<Cell>())
										{
											key = Regex.Replace(cellItem.CellReference,
												@"(?<f>[a-zA-Z]+)\d+", "${f}");
											row.SetField<string>(colNames[key],
												GetCellValue(workbook, cellItem));
										}
										table.Rows.Add(row);
									}
								}
								else
								{
									row = table.NewRow();
									//colIndex = 0;
									foreach(Cell cellItem in rowItem.Descendants<Cell>())
									{
										key = Regex.Replace(cellItem.CellReference,
											@"(?<f>[a-zA-Z]+)\d+", "${f}");
										if(!colNames.ContainsKey(key))
										{
											colName = $"Field{key}";
											table.Columns.Add(colName);
											colNames.Add(key, colName);
										}
										row.SetField<string>(colNames[key],
											GetCellValue(workbook, cellItem));
									}
									table.Rows.Add(row);
								}
								counter++;
							}
						}
					}
					else
					{
						data.ExtendedProperties.Add("ErrorMessage",
							$"Error reading Excel file: Workbook data not found.");
					}
				}
			}
			catch(Exception ex)
			{
				data.ExtendedProperties.Add("ErrorMessage",
					$"Error opening Excel file: {ex.Message}");
			}
			return data;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WriteWorkbook																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Write the provided data to the specified Excel file.
		/// </summary>
		/// <param name="filename">
		/// Name of the sheet to update.
		/// </param>
		/// <param name="data">
		/// Dataset to write to the specified file.
		/// </param>
		/// <returns>
		/// Value indicating whether the operation was completed successfully.
		/// </returns>
		/// <remarks>
		/// An existing sheet is overwritten. If the specified sheet didn't
		/// previously exist, it will be created.
		/// </remarks>
		public static bool WriteWorkbook(string filename, DataSet data)
		{
			Cell cell = null;
			List<String> colNames = null;
			int columnIndex = 0;
			Row headerRow = null;
			string relationshipId = "";
			bool result = false;
			int rowIndex = 0;
			Sheet sheet = null;
			SheetData sheetData = null;
			uint sheetId = 0;
			WorksheetPart sheetPart = null;
			Sheets sheets = null;
			WorkbookStylesPart stylesPart = null;
			Row targetRow = null;
			object value = null;
			WorkbookPart workbookPart = null;

			if(filename?.Length > 0 && data?.Tables.Count > 0)
			{
				using(SpreadsheetDocument workbook =
					SpreadsheetDocument.Create(filename,
					DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
				{
					workbookPart = workbook.AddWorkbookPart();
					workbook.WorkbookPart.Workbook = new Workbook();
					workbook.WorkbookPart.Workbook.Sheets = new Sheets();

					stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
					stylesPart.Stylesheet = CreateStylesheet();
					stylesPart.Stylesheet.Save();


					sheetId = 1;

					foreach(DataTable table in data.Tables)
					{
						sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
						sheetData = new SheetData();
						sheetPart.Worksheet = new Worksheet(sheetData);

						sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
						relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

						if(sheets.Elements<Sheet>().Count() > 0)
						{
							sheetId =
								sheets.Elements<Sheet>().
								Select(s => s.SheetId.Value).Max() + 1;
						}

						sheet = new Sheet()
						{
							Id = relationshipId,
							SheetId = sheetId,
							Name = table.TableName
						};
						sheets.Append(sheet);

						//	Header row.
						rowIndex = 0;
						headerRow = new Row();

						columnIndex = 0;
						colNames = new List<string>();
						foreach(DataColumn column in table.Columns)
						{
							colNames.Add(column.ColumnName);

							cell = new Cell();
							cell.DataType = CellValues.String;
							cell.CellReference = CreateCellReference(columnIndex, rowIndex);
							cell.CellValue = new CellValue(column.ColumnName);
							headerRow.AppendChild(cell);
							columnIndex++;
						}

						sheetData.AppendChild(headerRow);
						rowIndex++;

						//	Data rows.
						foreach(DataRow dataRow in table.Rows)
						{
							targetRow = new Row();
							columnIndex = 0;
							foreach(string colName in colNames)
							{
								cell = new Cell();
								cell.CellReference =
									CreateCellReference(columnIndex, rowIndex);

								switch(table.Columns[colName].DataType.Name)
								{
									case "DateTime":
										//	ISO8601 compliant date string is required when specifying the
										//	date data type.
										//	CellValues.Date is not yet fully supported.
										cell.DataType = CellValues.Number;
										cell.CellValue = new CellValue(
											((DateTime)dataRow[colName]).ToOADate().ToString());
										cell.StyleIndex = 3;
										break;
									case "Double":
										cell.DataType = CellValues.Number;
										cell.CellValue = new CellValue(((double)dataRow[colName]).ToString());
										cell.StyleIndex = 0;
										break;
									case "Float":
										cell.DataType = CellValues.Number;
										cell.CellValue = new CellValue(((float)dataRow[colName]).ToString());
										cell.StyleIndex = 0;
										break;
									case "Int32":
										cell.DataType = CellValues.Number;
										cell.CellValue = new CellValue(((int)dataRow[colName]).ToString());
										cell.StyleIndex = 0;
										break;
									case "String":
									default:
										cell.DataType = CellValues.String;
										value = dataRow[colName];
										if(value != null)
										{
											cell.CellValue = new CellValue(value.ToString());
										}
										cell.StyleIndex = 0;
										break;
								}
								targetRow.AppendChild(cell);
								columnIndex++;
							}
							sheetData.AppendChild(targetRow);
							rowIndex++;
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
