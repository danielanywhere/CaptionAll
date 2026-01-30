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
using System.Windows.Forms;

namespace CaptionAll
{
	partial class frmMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			titleBar = new TitleBarWF.TitleBarWF();
			menuStrip = new MenuStrip();
			mnuFile = new ToolStripMenuItem();
			mnuFileLoadProject = new ToolStripMenuItem();
			mnuFileSaveProject = new ToolStripMenuItem();
			mnuFileSaveProjectAs = new ToolStripMenuItem();
			mnuFileSep1 = new ToolStripSeparator();
			mnuFileOpenCaptions = new ToolStripMenuItem();
			mnuFileSaveCaptions = new ToolStripMenuItem();
			mnuFileSaveCaptionsAs = new ToolStripMenuItem();
			mnuFileOpenMedia = new ToolStripMenuItem();
			mnuFileSep2 = new ToolStripSeparator();
			mnuExport = new ToolStripMenuItem();
			mnuExportCaptions = new ToolStripMenuItem();
			mnuExportCaptionsSRT = new ToolStripMenuItem();
			mnuFileExportTranscriptText = new ToolStripMenuItem();
			mnuFileSep3 = new ToolStripSeparator();
			mnuFileExit = new ToolStripMenuItem();
			mnuEdit = new ToolStripMenuItem();
			mnuEditUndo = new ToolStripMenuItem();
			mnuEditSep1 = new ToolStripSeparator();
			mnuEditFind = new ToolStripMenuItem();
			mnuEditFindAgain = new ToolStripMenuItem();
			mnuEditFindAndReplace = new ToolStripMenuItem();
			mnuEditSep2 = new ToolStripSeparator();
			mnuEditRippleOnOff = new ToolStripMenuItem();
			mnuEditSpeedDialList = new ToolStripMenuItem();
			mnuEditSep3 = new ToolStripSeparator();
			mnuEditCaption = new ToolStripMenuItem();
			mnuEditCaptionText = new ToolStripMenuItem();
			mnuEditCaptionWidth = new ToolStripMenuItem();
			mnuEditInsertSpace = new ToolStripMenuItem();
			mnuEditInsertCaption = new ToolStripMenuItem();
			mnuEditToggleCaptionSpace = new ToolStripMenuItem();
			mnuEditRemoveSpace = new ToolStripMenuItem();
			mnuEditMergeCaptions = new ToolStripMenuItem();
			mnuEditDeleteCaption = new ToolStripMenuItem();
			mnuEditCaptionProperties = new ToolStripMenuItem();
			mnuEditExtendCaptionTails = new ToolStripMenuItem();
			mnuEditSelection = new ToolStripMenuItem();
			mnuEditCurrentTime = new ToolStripMenuItem();
			mnuEditSelectionStartTime = new ToolStripMenuItem();
			mnuEditSelectionEndTime = new ToolStripMenuItem();
			mnuEditSelectNone = new ToolStripMenuItem();
			mnuEditSelectTimeFromCaption = new ToolStripMenuItem();
			mnuEditSnapCaptionToSelection = new ToolStripMenuItem();
			mnuEditMoveContentToTime = new ToolStripMenuItem();
			mnuEditMoveLeftToTime = new ToolStripMenuItem();
			mnuEditSetTrackDuration = new ToolStripMenuItem();
			mnuEditConvertMultiSingle = new ToolStripMenuItem();
			mnuView = new ToolStripMenuItem();
			mnuViewCenterCursor = new ToolStripMenuItem();
			mnuTransport = new ToolStripMenuItem();
			mnuTransportStop = new ToolStripMenuItem();
			mnuTransportSep1 = new ToolStripSeparator();
			mnuTransportGoToStart = new ToolStripMenuItem();
			mnuTransportGoBack = new ToolStripMenuItem();
			mnuTransportSep2 = new ToolStripSeparator();
			mnuTransportPlayPause = new ToolStripMenuItem();
			mnuTransportSep3 = new ToolStripSeparator();
			mnuTransportGoForward = new ToolStripMenuItem();
			mnuTransportGoToEnd = new ToolStripMenuItem();
			mnuTools = new ToolStripMenuItem();
			mnuToolsDisplayVTTInfo = new ToolStripMenuItem();
			mnuToolsLongSoundCount = new ToolStripMenuItem();
			mnuToolsShortSoundCount = new ToolStripMenuItem();
			mnuToolsUpdateTargetFromSource = new ToolStripMenuItem();
			mnuToolsReWrapStreamVTT = new ToolStripMenuItem();
			mnuToolsExportMSBurstsExcel = new ToolStripMenuItem();
			mnuToolsAlignScriptToCaptions = new ToolStripMenuItem();
			mnuHelp = new ToolStripMenuItem();
			mnuHelpAbout = new ToolStripMenuItem();
			statusStrip = new StatusStrip();
			statMessage = new ToolStripStatusLabel();
			statTime = new ToolStripStatusLabel();
			pnlBorder = new Panel();
			pnlBorderInner = new Panel();
			pnlMedia = new Panel();
			split = new Splitter();
			pnlCaption = new Panel();
			captionEditor = new CaptionBubbleEditorWF.CaptionBubbleControl();
			scrollTimeline = new ScrollPanelVirtual.ScrollPanelControlVirt();
			tbtnTransportStop = new ToolStripButton();
			tsepStop = new ToolStripSeparator();
			tbtnTransportGoToStart = new ToolStripButton();
			tbtnTransportGoBack = new ToolStripButton();
			tbtnTransportPlayPause = new ToolStripButton();
			tbtnTransportGoForward = new ToolStripButton();
			tbtnTransportGoToEnd = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			tbtnEditRippleOnOff = new ToolStripButton();
			toolStrip = new ToolStrip();
			ctxMenuCaption = new ContextMenuStrip(components);
			ctxCaptionText = new ToolStripMenuItem();
			ctxCaptionWidth = new ToolStripMenuItem();
			ctxInsertSpace = new ToolStripMenuItem();
			ctxInsertCaption = new ToolStripMenuItem();
			ctxToggleCaption = new ToolStripMenuItem();
			ctxRemoveSpace = new ToolStripMenuItem();
			ctxMergeCaptions = new ToolStripMenuItem();
			ctxDeleteCaption = new ToolStripMenuItem();
			ctxCaptionProperties = new ToolStripMenuItem();
			ctxSep1 = new ToolStripSeparator();
			ctxSelectNone = new ToolStripMenuItem();
			ctxSelectTimeFromCaption = new ToolStripMenuItem();
			ctxSnapCaptionToSelection = new ToolStripMenuItem();
			titleBar.MenuArea.SuspendLayout();
			menuStrip.SuspendLayout();
			statusStrip.SuspendLayout();
			pnlBorder.SuspendLayout();
			pnlBorderInner.SuspendLayout();
			pnlCaption.SuspendLayout();
			toolStrip.SuspendLayout();
			ctxMenuCaption.SuspendLayout();
			SuspendLayout();
			// 
			// titleBar
			// 
			titleBar.BorderStyle = BorderStyle.FixedSingle;
			titleBar.Dock = DockStyle.Top;
			titleBar.IconImage = (System.Drawing.Image)resources.GetObject("titleBar.IconImage");
			titleBar.Location = new System.Drawing.Point(0, 0);
			// 
			// 
			// 
			titleBar.MenuArea.BackColor = System.Drawing.Color.White;
			titleBar.MenuArea.Controls.Add(menuStrip);
			titleBar.MenuArea.Dock = DockStyle.Fill;
			titleBar.MenuArea.Location = new System.Drawing.Point(44, 0);
			titleBar.MenuArea.Name = "pnlMenu";
			titleBar.MenuArea.Size = new System.Drawing.Size(616, 38);
			titleBar.MenuArea.TabIndex = 2;
			titleBar.Name = "titleBar";
			titleBar.Size = new System.Drawing.Size(918, 40);
			titleBar.TabIndex = 0;
			titleBar.WindowControlWidth = 256;
			// 
			// menuStrip
			// 
			menuStrip.BackColor = System.Drawing.Color.White;
			menuStrip.Dock = DockStyle.Fill;
			menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, mnuEdit, mnuView, mnuTransport, mnuTools, mnuHelp });
			menuStrip.Location = new System.Drawing.Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.Size = new System.Drawing.Size(616, 38);
			menuStrip.TabIndex = 3;
			menuStrip.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuFileLoadProject, mnuFileSaveProject, mnuFileSaveProjectAs, mnuFileSep1, mnuFileOpenCaptions, mnuFileSaveCaptions, mnuFileSaveCaptionsAs, mnuFileOpenMedia, mnuFileSep2, mnuExport, mnuFileSep3, mnuFileExit });
			mnuFile.Name = "mnuFile";
			mnuFile.Size = new System.Drawing.Size(46, 34);
			mnuFile.Text = "&File";
			// 
			// mnuFileLoadProject
			// 
			mnuFileLoadProject.Name = "mnuFileLoadProject";
			mnuFileLoadProject.ShortcutKeys = Keys.Control | Keys.O;
			mnuFileLoadProject.Size = new System.Drawing.Size(305, 26);
			mnuFileLoadProject.Text = "Load &Project";
			// 
			// mnuFileSaveProject
			// 
			mnuFileSaveProject.Name = "mnuFileSaveProject";
			mnuFileSaveProject.ShortcutKeys = Keys.Control | Keys.S;
			mnuFileSaveProject.Size = new System.Drawing.Size(305, 26);
			mnuFileSaveProject.Text = "Sa&ve Project";
			// 
			// mnuFileSaveProjectAs
			// 
			mnuFileSaveProjectAs.Name = "mnuFileSaveProjectAs";
			mnuFileSaveProjectAs.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
			mnuFileSaveProjectAs.Size = new System.Drawing.Size(305, 26);
			mnuFileSaveProjectAs.Text = "Save P&roject As ...";
			// 
			// mnuFileSep1
			// 
			mnuFileSep1.Name = "mnuFileSep1";
			mnuFileSep1.Size = new System.Drawing.Size(302, 6);
			// 
			// mnuFileOpenCaptions
			// 
			mnuFileOpenCaptions.Name = "mnuFileOpenCaptions";
			mnuFileOpenCaptions.ShortcutKeys = Keys.Control | Keys.L;
			mnuFileOpenCaptions.Size = new System.Drawing.Size(305, 26);
			mnuFileOpenCaptions.Text = "Open &Captions";
			// 
			// mnuFileSaveCaptions
			// 
			mnuFileSaveCaptions.Enabled = false;
			mnuFileSaveCaptions.Name = "mnuFileSaveCaptions";
			mnuFileSaveCaptions.ShortcutKeys = Keys.Control | Keys.J;
			mnuFileSaveCaptions.Size = new System.Drawing.Size(305, 26);
			mnuFileSaveCaptions.Text = "&Save Captions";
			// 
			// mnuFileSaveCaptionsAs
			// 
			mnuFileSaveCaptionsAs.Enabled = false;
			mnuFileSaveCaptionsAs.Name = "mnuFileSaveCaptionsAs";
			mnuFileSaveCaptionsAs.ShortcutKeys = Keys.Control | Keys.Shift | Keys.J;
			mnuFileSaveCaptionsAs.Size = new System.Drawing.Size(305, 26);
			mnuFileSaveCaptionsAs.Text = "Save Captions &As ...";
			// 
			// mnuFileOpenMedia
			// 
			mnuFileOpenMedia.Name = "mnuFileOpenMedia";
			mnuFileOpenMedia.ShortcutKeys = Keys.Control | Keys.Shift | Keys.L;
			mnuFileOpenMedia.Size = new System.Drawing.Size(305, 26);
			mnuFileOpenMedia.Text = "Open &Media";
			// 
			// mnuFileSep2
			// 
			mnuFileSep2.Name = "mnuFileSep2";
			mnuFileSep2.Size = new System.Drawing.Size(302, 6);
			// 
			// mnuExport
			// 
			mnuExport.DropDownItems.AddRange(new ToolStripItem[] { mnuExportCaptions, mnuFileExportTranscriptText });
			mnuExport.Name = "mnuExport";
			mnuExport.Size = new System.Drawing.Size(305, 26);
			mnuExport.Text = "&Export";
			// 
			// mnuExportCaptions
			// 
			mnuExportCaptions.DropDownItems.AddRange(new ToolStripItem[] { mnuExportCaptionsSRT });
			mnuExportCaptions.Name = "mnuExportCaptions";
			mnuExportCaptions.Size = new System.Drawing.Size(187, 26);
			mnuExportCaptions.Text = "&Captions";
			// 
			// mnuExportCaptionsSRT
			// 
			mnuExportCaptionsSRT.Name = "mnuExportCaptionsSRT";
			mnuExportCaptionsSRT.Size = new System.Drawing.Size(136, 26);
			mnuExportCaptionsSRT.Text = "As &SRT";
			// 
			// mnuFileExportTranscriptText
			// 
			mnuFileExportTranscriptText.Name = "mnuFileExportTranscriptText";
			mnuFileExportTranscriptText.Size = new System.Drawing.Size(187, 26);
			mnuFileExportTranscriptText.Text = "&Transcript Text";
			// 
			// mnuFileSep3
			// 
			mnuFileSep3.Name = "mnuFileSep3";
			mnuFileSep3.Size = new System.Drawing.Size(302, 6);
			// 
			// mnuFileExit
			// 
			mnuFileExit.Name = "mnuFileExit";
			mnuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
			mnuFileExit.Size = new System.Drawing.Size(305, 26);
			mnuFileExit.Text = "E&xit";
			// 
			// mnuEdit
			// 
			mnuEdit.DropDownItems.AddRange(new ToolStripItem[] { mnuEditUndo, mnuEditSep1, mnuEditFind, mnuEditFindAgain, mnuEditFindAndReplace, mnuEditSep2, mnuEditRippleOnOff, mnuEditSpeedDialList, mnuEditSep3, mnuEditCaption, mnuEditSelection, mnuEditSetTrackDuration, mnuEditConvertMultiSingle });
			mnuEdit.Name = "mnuEdit";
			mnuEdit.Size = new System.Drawing.Size(49, 34);
			mnuEdit.Text = "&Edit";
			// 
			// mnuEditUndo
			// 
			mnuEditUndo.Enabled = false;
			mnuEditUndo.Name = "mnuEditUndo";
			mnuEditUndo.ShortcutKeys = Keys.Control | Keys.Z;
			mnuEditUndo.Size = new System.Drawing.Size(308, 26);
			mnuEditUndo.Text = "&Undo";
			// 
			// mnuEditSep1
			// 
			mnuEditSep1.Name = "mnuEditSep1";
			mnuEditSep1.Size = new System.Drawing.Size(305, 6);
			// 
			// mnuEditFind
			// 
			mnuEditFind.Name = "mnuEditFind";
			mnuEditFind.ShortcutKeys = Keys.Control | Keys.F;
			mnuEditFind.Size = new System.Drawing.Size(308, 26);
			mnuEditFind.Text = "&Find";
			// 
			// mnuEditFindAgain
			// 
			mnuEditFindAgain.Name = "mnuEditFindAgain";
			mnuEditFindAgain.ShortcutKeys = Keys.F3;
			mnuEditFindAgain.Size = new System.Drawing.Size(308, 26);
			mnuEditFindAgain.Text = "Find &Again";
			// 
			// mnuEditFindAndReplace
			// 
			mnuEditFindAndReplace.Name = "mnuEditFindAndReplace";
			mnuEditFindAndReplace.ShortcutKeys = Keys.Control | Keys.H;
			mnuEditFindAndReplace.Size = new System.Drawing.Size(308, 26);
			mnuEditFindAndReplace.Text = "Find And &Replace";
			// 
			// mnuEditSep2
			// 
			mnuEditSep2.Name = "mnuEditSep2";
			mnuEditSep2.Size = new System.Drawing.Size(305, 6);
			// 
			// mnuEditRippleOnOff
			// 
			mnuEditRippleOnOff.Image = ResourceMain.Ripple32;
			mnuEditRippleOnOff.Name = "mnuEditRippleOnOff";
			mnuEditRippleOnOff.ShortcutKeys = Keys.Control | Keys.Alt | Keys.R;
			mnuEditRippleOnOff.Size = new System.Drawing.Size(308, 26);
			mnuEditRippleOnOff.Text = "R&ipple On | Off";
			// 
			// mnuEditSpeedDialList
			// 
			mnuEditSpeedDialList.Name = "mnuEditSpeedDialList";
			mnuEditSpeedDialList.ShortcutKeys = Keys.Control | Keys.Alt | Keys.W;
			mnuEditSpeedDialList.Size = new System.Drawing.Size(308, 26);
			mnuEditSpeedDialList.Text = "Speed &Dial List";
			// 
			// mnuEditSep3
			// 
			mnuEditSep3.Name = "mnuEditSep3";
			mnuEditSep3.Size = new System.Drawing.Size(305, 6);
			// 
			// mnuEditCaption
			// 
			mnuEditCaption.DropDownItems.AddRange(new ToolStripItem[] { mnuEditCaptionText, mnuEditCaptionWidth, mnuEditInsertSpace, mnuEditInsertCaption, mnuEditToggleCaptionSpace, mnuEditRemoveSpace, mnuEditMergeCaptions, mnuEditDeleteCaption, mnuEditCaptionProperties, mnuEditExtendCaptionTails });
			mnuEditCaption.Name = "mnuEditCaption";
			mnuEditCaption.Size = new System.Drawing.Size(308, 26);
			mnuEditCaption.Text = "&Caption";
			// 
			// mnuEditCaptionText
			// 
			mnuEditCaptionText.Name = "mnuEditCaptionText";
			mnuEditCaptionText.ShortcutKeys = Keys.F2;
			mnuEditCaptionText.Size = new System.Drawing.Size(298, 26);
			mnuEditCaptionText.Text = "Caption &Text";
			// 
			// mnuEditCaptionWidth
			// 
			mnuEditCaptionWidth.Name = "mnuEditCaptionWidth";
			mnuEditCaptionWidth.ShortcutKeys = Keys.Control | Keys.W;
			mnuEditCaptionWidth.Size = new System.Drawing.Size(298, 26);
			mnuEditCaptionWidth.Text = "Caption &Width";
			// 
			// mnuEditInsertSpace
			// 
			mnuEditInsertSpace.Name = "mnuEditInsertSpace";
			mnuEditInsertSpace.ShortcutKeys = Keys.Control | Keys.Shift | Keys.I;
			mnuEditInsertSpace.Size = new System.Drawing.Size(298, 26);
			mnuEditInsertSpace.Text = "Insert &Space";
			// 
			// mnuEditInsertCaption
			// 
			mnuEditInsertCaption.Name = "mnuEditInsertCaption";
			mnuEditInsertCaption.ShortcutKeys = Keys.Control | Keys.I;
			mnuEditInsertCaption.Size = new System.Drawing.Size(298, 26);
			mnuEditInsertCaption.Text = "Split &Caption";
			// 
			// mnuEditToggleCaptionSpace
			// 
			mnuEditToggleCaptionSpace.Name = "mnuEditToggleCaptionSpace";
			mnuEditToggleCaptionSpace.ShortcutKeys = Keys.Control | Keys.G;
			mnuEditToggleCaptionSpace.Size = new System.Drawing.Size(298, 26);
			mnuEditToggleCaptionSpace.Text = "T&oggle Caption | Space";
			// 
			// mnuEditRemoveSpace
			// 
			mnuEditRemoveSpace.Name = "mnuEditRemoveSpace";
			mnuEditRemoveSpace.ShortcutKeys = Keys.Control | Keys.R;
			mnuEditRemoveSpace.Size = new System.Drawing.Size(298, 26);
			mnuEditRemoveSpace.Text = "&Remove Space";
			// 
			// mnuEditMergeCaptions
			// 
			mnuEditMergeCaptions.Name = "mnuEditMergeCaptions";
			mnuEditMergeCaptions.ShortcutKeys = Keys.Control | Keys.M;
			mnuEditMergeCaptions.Size = new System.Drawing.Size(298, 26);
			mnuEditMergeCaptions.Text = "&Merge Captions";
			// 
			// mnuEditDeleteCaption
			// 
			mnuEditDeleteCaption.Name = "mnuEditDeleteCaption";
			mnuEditDeleteCaption.ShortcutKeys = Keys.Control | Keys.D;
			mnuEditDeleteCaption.Size = new System.Drawing.Size(298, 26);
			mnuEditDeleteCaption.Text = "&Delete Caption";
			// 
			// mnuEditCaptionProperties
			// 
			mnuEditCaptionProperties.Name = "mnuEditCaptionProperties";
			mnuEditCaptionProperties.ShortcutKeys = Keys.Control | Keys.Return;
			mnuEditCaptionProperties.Size = new System.Drawing.Size(298, 26);
			mnuEditCaptionProperties.Text = "Caption &Properties";
			// 
			// mnuEditExtendCaptionTails
			// 
			mnuEditExtendCaptionTails.Name = "mnuEditExtendCaptionTails";
			mnuEditExtendCaptionTails.Size = new System.Drawing.Size(298, 26);
			mnuEditExtendCaptionTails.Text = "&Extend Caption Tails";
			// 
			// mnuEditSelection
			// 
			mnuEditSelection.DropDownItems.AddRange(new ToolStripItem[] { mnuEditCurrentTime, mnuEditSelectionStartTime, mnuEditSelectionEndTime, mnuEditSelectNone, mnuEditSelectTimeFromCaption, mnuEditSnapCaptionToSelection, mnuEditMoveContentToTime, mnuEditMoveLeftToTime });
			mnuEditSelection.Name = "mnuEditSelection";
			mnuEditSelection.Size = new System.Drawing.Size(308, 26);
			mnuEditSelection.Text = "&Selection";
			// 
			// mnuEditCurrentTime
			// 
			mnuEditCurrentTime.Name = "mnuEditCurrentTime";
			mnuEditCurrentTime.ShortcutKeys = Keys.Control | Keys.T;
			mnuEditCurrentTime.Size = new System.Drawing.Size(353, 26);
			mnuEditCurrentTime.Text = "Cu&rrent Time";
			// 
			// mnuEditSelectionStartTime
			// 
			mnuEditSelectionStartTime.Name = "mnuEditSelectionStartTime";
			mnuEditSelectionStartTime.ShortcutKeys = Keys.F8;
			mnuEditSelectionStartTime.Size = new System.Drawing.Size(353, 26);
			mnuEditSelectionStartTime.Text = "Selection St&art Time";
			// 
			// mnuEditSelectionEndTime
			// 
			mnuEditSelectionEndTime.Name = "mnuEditSelectionEndTime";
			mnuEditSelectionEndTime.ShortcutKeys = Keys.F9;
			mnuEditSelectionEndTime.Size = new System.Drawing.Size(353, 26);
			mnuEditSelectionEndTime.Text = "Selection &End Time";
			// 
			// mnuEditSelectNone
			// 
			mnuEditSelectNone.Name = "mnuEditSelectNone";
			mnuEditSelectNone.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
			mnuEditSelectNone.Size = new System.Drawing.Size(353, 26);
			mnuEditSelectNone.Text = "Select &None";
			// 
			// mnuEditSelectTimeFromCaption
			// 
			mnuEditSelectTimeFromCaption.Name = "mnuEditSelectTimeFromCaption";
			mnuEditSelectTimeFromCaption.ShortcutKeys = Keys.Control | Keys.Shift | Keys.T;
			mnuEditSelectTimeFromCaption.Size = new System.Drawing.Size(353, 26);
			mnuEditSelectTimeFromCaption.Text = "Se&lect Time From Caption";
			// 
			// mnuEditSnapCaptionToSelection
			// 
			mnuEditSnapCaptionToSelection.Name = "mnuEditSnapCaptionToSelection";
			mnuEditSnapCaptionToSelection.ShortcutKeys = Keys.Control | Keys.E;
			mnuEditSnapCaptionToSelection.Size = new System.Drawing.Size(353, 26);
			mnuEditSnapCaptionToSelection.Text = "Sna&p Caption To Selection";
			// 
			// mnuEditMoveContentToTime
			// 
			mnuEditMoveContentToTime.Name = "mnuEditMoveContentToTime";
			mnuEditMoveContentToTime.Size = new System.Drawing.Size(353, 26);
			mnuEditMoveContentToTime.Text = "&Move Content To Current Time";
			mnuEditMoveContentToTime.ToolTipText = "Move all captions in the file to start at the current time cursor.";
			// 
			// mnuEditMoveLeftToTime
			// 
			mnuEditMoveLeftToTime.Name = "mnuEditMoveLeftToTime";
			mnuEditMoveLeftToTime.Size = new System.Drawing.Size(353, 26);
			mnuEditMoveLeftToTime.Text = "Move Ne&xt Content Left To Time";
			// 
			// mnuEditSetTrackDuration
			// 
			mnuEditSetTrackDuration.Name = "mnuEditSetTrackDuration";
			mnuEditSetTrackDuration.ShortcutKeys = Keys.Control | Keys.K;
			mnuEditSetTrackDuration.Size = new System.Drawing.Size(308, 26);
			mnuEditSetTrackDuration.Text = "Set Trac&k Duration";
			// 
			// mnuEditConvertMultiSingle
			// 
			mnuEditConvertMultiSingle.Name = "mnuEditConvertMultiSingle";
			mnuEditConvertMultiSingle.Size = new System.Drawing.Size(308, 26);
			mnuEditConvertMultiSingle.Text = "Co&nvert Multi-Line to Single Line";
			// 
			// mnuView
			// 
			mnuView.DropDownItems.AddRange(new ToolStripItem[] { mnuViewCenterCursor });
			mnuView.Name = "mnuView";
			mnuView.Size = new System.Drawing.Size(55, 34);
			mnuView.Text = "&View";
			// 
			// mnuViewCenterCursor
			// 
			mnuViewCenterCursor.Name = "mnuViewCenterCursor";
			mnuViewCenterCursor.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
			mnuViewCenterCursor.Size = new System.Drawing.Size(343, 26);
			mnuViewCenterCursor.Text = "&Center Cursor On Screen";
			// 
			// mnuTransport
			// 
			mnuTransport.DropDownItems.AddRange(new ToolStripItem[] { mnuTransportStop, mnuTransportSep1, mnuTransportGoToStart, mnuTransportGoBack, mnuTransportSep2, mnuTransportPlayPause, mnuTransportSep3, mnuTransportGoForward, mnuTransportGoToEnd });
			mnuTransport.Name = "mnuTransport";
			mnuTransport.Size = new System.Drawing.Size(85, 34);
			mnuTransport.Text = "T&ransport";
			// 
			// mnuTransportStop
			// 
			mnuTransportStop.Image = ResourceMain.Stop32;
			mnuTransportStop.Name = "mnuTransportStop";
			mnuTransportStop.ShortcutKeys = Keys.Alt | Keys.F5;
			mnuTransportStop.Size = new System.Drawing.Size(274, 26);
			mnuTransportStop.Text = "&Stop";
			// 
			// mnuTransportSep1
			// 
			mnuTransportSep1.Name = "mnuTransportSep1";
			mnuTransportSep1.Size = new System.Drawing.Size(271, 6);
			// 
			// mnuTransportGoToStart
			// 
			mnuTransportGoToStart.Image = ResourceMain.GoToStart32;
			mnuTransportGoToStart.Name = "mnuTransportGoToStart";
			mnuTransportGoToStart.Size = new System.Drawing.Size(274, 26);
			mnuTransportGoToStart.Text = "Go To S&tart      (Home Key)";
			// 
			// mnuTransportGoBack
			// 
			mnuTransportGoBack.Image = ResourceMain.GoBackward32;
			mnuTransportGoBack.Name = "mnuTransportGoBack";
			mnuTransportGoBack.ShortcutKeys = Keys.F4;
			mnuTransportGoBack.Size = new System.Drawing.Size(274, 26);
			mnuTransportGoBack.Text = "Go &Back 5 Seconds";
			// 
			// mnuTransportSep2
			// 
			mnuTransportSep2.Name = "mnuTransportSep2";
			mnuTransportSep2.Size = new System.Drawing.Size(271, 6);
			// 
			// mnuTransportPlayPause
			// 
			mnuTransportPlayPause.Image = ResourceMain.Play32;
			mnuTransportPlayPause.Name = "mnuTransportPlayPause";
			mnuTransportPlayPause.ShortcutKeys = Keys.F5;
			mnuTransportPlayPause.Size = new System.Drawing.Size(274, 26);
			mnuTransportPlayPause.Text = "&Play | Pause (Space Key)";
			// 
			// mnuTransportSep3
			// 
			mnuTransportSep3.Name = "mnuTransportSep3";
			mnuTransportSep3.Size = new System.Drawing.Size(271, 6);
			// 
			// mnuTransportGoForward
			// 
			mnuTransportGoForward.Image = ResourceMain.GoForward32;
			mnuTransportGoForward.Name = "mnuTransportGoForward";
			mnuTransportGoForward.ShortcutKeys = Keys.F6;
			mnuTransportGoForward.Size = new System.Drawing.Size(274, 26);
			mnuTransportGoForward.Text = "Go &Forward 5 Seconds";
			// 
			// mnuTransportGoToEnd
			// 
			mnuTransportGoToEnd.Image = ResourceMain.GoToEnd32;
			mnuTransportGoToEnd.Name = "mnuTransportGoToEnd";
			mnuTransportGoToEnd.Size = new System.Drawing.Size(274, 26);
			mnuTransportGoToEnd.Text = "Go To &End        (End Key)";
			// 
			// mnuTools
			// 
			mnuTools.DropDownItems.AddRange(new ToolStripItem[] { mnuToolsDisplayVTTInfo, mnuToolsLongSoundCount, mnuToolsShortSoundCount, mnuToolsUpdateTargetFromSource, mnuToolsReWrapStreamVTT, mnuToolsExportMSBurstsExcel, mnuToolsAlignScriptToCaptions });
			mnuTools.Name = "mnuTools";
			mnuTools.Size = new System.Drawing.Size(58, 34);
			mnuTools.Text = "&Tools";
			// 
			// mnuToolsDisplayVTTInfo
			// 
			mnuToolsDisplayVTTInfo.Name = "mnuToolsDisplayVTTInfo";
			mnuToolsDisplayVTTInfo.Size = new System.Drawing.Size(358, 26);
			mnuToolsDisplayVTTInfo.Text = "Display &VTT Information";
			// 
			// mnuToolsLongSoundCount
			// 
			mnuToolsLongSoundCount.Name = "mnuToolsLongSoundCount";
			mnuToolsLongSoundCount.Size = new System.Drawing.Size(358, 26);
			mnuToolsLongSoundCount.Text = "Display &Long-Sound Count";
			mnuToolsLongSoundCount.ToolTipText = "Display the count of long-sound letters in text you supply.";
			// 
			// mnuToolsShortSoundCount
			// 
			mnuToolsShortSoundCount.Name = "mnuToolsShortSoundCount";
			mnuToolsShortSoundCount.Size = new System.Drawing.Size(358, 26);
			mnuToolsShortSoundCount.Text = "Display &Short-Sound Count";
			mnuToolsShortSoundCount.ToolTipText = "Display the count of short-sound letters in text you supply.";
			// 
			// mnuToolsUpdateTargetFromSource
			// 
			mnuToolsUpdateTargetFromSource.Name = "mnuToolsUpdateTargetFromSource";
			mnuToolsUpdateTargetFromSource.Size = new System.Drawing.Size(358, 26);
			mnuToolsUpdateTargetFromSource.Text = "&Update Target Text From Source VTT";
			// 
			// mnuToolsReWrapStreamVTT
			// 
			mnuToolsReWrapStreamVTT.Name = "mnuToolsReWrapStreamVTT";
			mnuToolsReWrapStreamVTT.Size = new System.Drawing.Size(358, 26);
			mnuToolsReWrapStreamVTT.Text = "Re-&Wrap Microsoft Stream VTT";
			mnuToolsReWrapStreamVTT.ToolTipText = "Re-wrap the contents of a VTT file generated by Microsoft Stream for better caption viewing experience.";
			// 
			// mnuToolsExportMSBurstsExcel
			// 
			mnuToolsExportMSBurstsExcel.Name = "mnuToolsExportMSBurstsExcel";
			mnuToolsExportMSBurstsExcel.Size = new System.Drawing.Size(358, 26);
			mnuToolsExportMSBurstsExcel.Text = "Export MS-Stream Audio Bursts To &Excel";
			// 
			// mnuToolsAlignScriptToCaptions
			// 
			mnuToolsAlignScriptToCaptions.Name = "mnuToolsAlignScriptToCaptions";
			mnuToolsAlignScriptToCaptions.Size = new System.Drawing.Size(358, 26);
			mnuToolsAlignScriptToCaptions.Text = "&Align Script To Captions";
			// 
			// mnuHelp
			// 
			mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuHelpAbout });
			mnuHelp.Name = "mnuHelp";
			mnuHelp.Size = new System.Drawing.Size(55, 34);
			mnuHelp.Text = "&Help";
			// 
			// mnuHelpAbout
			// 
			mnuHelpAbout.Name = "mnuHelpAbout";
			mnuHelpAbout.Size = new System.Drawing.Size(133, 26);
			mnuHelpAbout.Text = "&About";
			// 
			// statusStrip
			// 
			statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			statusStrip.Items.AddRange(new ToolStripItem[] { statMessage, statTime });
			statusStrip.Location = new System.Drawing.Point(0, 556);
			statusStrip.Name = "statusStrip";
			statusStrip.Size = new System.Drawing.Size(918, 26);
			statusStrip.TabIndex = 2;
			statusStrip.Text = "statusStrip1";
			// 
			// statMessage
			// 
			statMessage.Name = "statMessage";
			statMessage.Size = new System.Drawing.Size(285, 20);
			statMessage.Text = "No media file open. No caption file open.";
			// 
			// statTime
			// 
			statTime.Name = "statTime";
			statTime.Size = new System.Drawing.Size(618, 20);
			statTime.Spring = true;
			statTime.Text = "00:00:00.000";
			statTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pnlBorder
			// 
			pnlBorder.BackColor = System.Drawing.Color.Black;
			pnlBorder.Controls.Add(pnlBorderInner);
			pnlBorder.Dock = DockStyle.Fill;
			pnlBorder.Location = new System.Drawing.Point(0, 80);
			pnlBorder.Name = "pnlBorder";
			pnlBorder.Size = new System.Drawing.Size(918, 476);
			pnlBorder.TabIndex = 2;
			// 
			// pnlBorderInner
			// 
			pnlBorderInner.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pnlBorderInner.Controls.Add(pnlMedia);
			pnlBorderInner.Controls.Add(split);
			pnlBorderInner.Controls.Add(pnlCaption);
			pnlBorderInner.Location = new System.Drawing.Point(4, 0);
			pnlBorderInner.Name = "pnlBorderInner";
			pnlBorderInner.Size = new System.Drawing.Size(910, 436);
			pnlBorderInner.TabIndex = 0;
			// 
			// pnlMedia
			// 
			pnlMedia.BackColor = System.Drawing.Color.FromArgb(34, 23, 70);
			pnlMedia.Dock = DockStyle.Fill;
			pnlMedia.Location = new System.Drawing.Point(0, 0);
			pnlMedia.Name = "pnlMedia";
			pnlMedia.Size = new System.Drawing.Size(910, 200);
			pnlMedia.TabIndex = 3;
			// 
			// split
			// 
			split.BackColor = System.Drawing.SystemColors.Control;
			split.Dock = DockStyle.Bottom;
			split.Location = new System.Drawing.Point(0, 200);
			split.Name = "split";
			split.Size = new System.Drawing.Size(910, 8);
			split.TabIndex = 4;
			split.TabStop = false;
			// 
			// pnlCaption
			// 
			pnlCaption.BackColor = System.Drawing.Color.FromArgb(34, 23, 70);
			pnlCaption.Controls.Add(captionEditor);
			pnlCaption.Controls.Add(scrollTimeline);
			pnlCaption.Dock = DockStyle.Bottom;
			pnlCaption.Location = new System.Drawing.Point(0, 208);
			pnlCaption.Name = "pnlCaption";
			pnlCaption.Size = new System.Drawing.Size(910, 228);
			pnlCaption.TabIndex = 5;
			// 
			// captionEditor
			// 
			captionEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			captionEditor.BackColor = System.Drawing.Color.White;
			captionEditor.Duration = 0D;
			captionEditor.Location = new System.Drawing.Point(0, 0);
			captionEditor.Name = "captionEditor";
			captionEditor.PixelsPerSecond = 80D;
			captionEditor.Playhead = 0D;
			captionEditor.SelectionEnd = 0D;
			captionEditor.SelectionStart = 0D;
			captionEditor.Size = new System.Drawing.Size(633, 205);
			captionEditor.TabIndex = 0;
			captionEditor.WaveformBitmap = null;
			captionEditor.WaveformHeight = 100;
			captionEditor.WaveformVisible = true;
			// 
			// scrollTimeline
			// 
			scrollTimeline.BackColor = System.Drawing.SystemColors.Control;
			scrollTimeline.CanvasBackColor = System.Drawing.Color.FromArgb(34, 23, 70);
			scrollTimeline.Dock = DockStyle.Fill;
			scrollTimeline.Location = new System.Drawing.Point(0, 0);
			scrollTimeline.Margin = new Padding(3, 4, 3, 4);
			scrollTimeline.Name = "scrollTimeline";
			scrollTimeline.ScrollPositionH = 0;
			scrollTimeline.ScrollPositionV = 0;
			scrollTimeline.ScrollWheelAssignment = ScrollPanelVirtual.ScrollWheelAssignmentEnum.NScrollSPanCZoom;
			scrollTimeline.Size = new System.Drawing.Size(910, 228);
			scrollTimeline.TabIndex = 3;
			// 
			// tbtnTransportStop
			// 
			tbtnTransportStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tbtnTransportStop.Image = ResourceMain.Stop32;
			tbtnTransportStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbtnTransportStop.Name = "tbtnTransportStop";
			tbtnTransportStop.Size = new System.Drawing.Size(36, 37);
			tbtnTransportStop.Text = "Transport / Stop";
			// 
			// tsepStop
			// 
			tsepStop.Name = "tsepStop";
			tsepStop.Size = new System.Drawing.Size(6, 40);
			// 
			// tbtnTransportGoToStart
			// 
			tbtnTransportGoToStart.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tbtnTransportGoToStart.Image = ResourceMain.GoToStart32;
			tbtnTransportGoToStart.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbtnTransportGoToStart.Name = "tbtnTransportGoToStart";
			tbtnTransportGoToStart.Size = new System.Drawing.Size(36, 37);
			tbtnTransportGoToStart.Text = "Transport / Go To Start";
			// 
			// tbtnTransportGoBack
			// 
			tbtnTransportGoBack.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tbtnTransportGoBack.Image = ResourceMain.GoBackward32;
			tbtnTransportGoBack.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbtnTransportGoBack.Name = "tbtnTransportGoBack";
			tbtnTransportGoBack.Size = new System.Drawing.Size(36, 37);
			tbtnTransportGoBack.Text = "Transport / Go Back 5 Seconds";
			// 
			// tbtnTransportPlayPause
			// 
			tbtnTransportPlayPause.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tbtnTransportPlayPause.Image = ResourceMain.Play32;
			tbtnTransportPlayPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbtnTransportPlayPause.Name = "tbtnTransportPlayPause";
			tbtnTransportPlayPause.Size = new System.Drawing.Size(36, 37);
			tbtnTransportPlayPause.Text = "Transport / Play | Pause";
			// 
			// tbtnTransportGoForward
			// 
			tbtnTransportGoForward.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tbtnTransportGoForward.Image = ResourceMain.GoForward32;
			tbtnTransportGoForward.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbtnTransportGoForward.Name = "tbtnTransportGoForward";
			tbtnTransportGoForward.Size = new System.Drawing.Size(36, 37);
			tbtnTransportGoForward.Text = "Transport / Go Forward 5 Seconds";
			// 
			// tbtnTransportGoToEnd
			// 
			tbtnTransportGoToEnd.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tbtnTransportGoToEnd.Image = ResourceMain.GoToEnd32;
			tbtnTransportGoToEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbtnTransportGoToEnd.Name = "tbtnTransportGoToEnd";
			tbtnTransportGoToEnd.Size = new System.Drawing.Size(36, 37);
			tbtnTransportGoToEnd.Text = "Transport / Go To End";
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
			// 
			// tbtnEditRippleOnOff
			// 
			tbtnEditRippleOnOff.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tbtnEditRippleOnOff.Image = ResourceMain.Ripple32;
			tbtnEditRippleOnOff.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbtnEditRippleOnOff.Name = "tbtnEditRippleOnOff";
			tbtnEditRippleOnOff.Size = new System.Drawing.Size(36, 37);
			tbtnEditRippleOnOff.Text = "Edit / Ripple On | Off";
			// 
			// toolStrip
			// 
			toolStrip.AutoSize = false;
			toolStrip.BackColor = System.Drawing.SystemColors.Control;
			toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
			toolStrip.Items.AddRange(new ToolStripItem[] { tbtnTransportStop, tsepStop, tbtnTransportGoToStart, tbtnTransportGoBack, tbtnTransportPlayPause, tbtnTransportGoForward, tbtnTransportGoToEnd, toolStripSeparator1, tbtnEditRippleOnOff });
			toolStrip.Location = new System.Drawing.Point(0, 40);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new System.Drawing.Size(918, 40);
			toolStrip.TabIndex = 1;
			toolStrip.Text = "toolStrip";
			// 
			// ctxMenuCaption
			// 
			ctxMenuCaption.ImageScalingSize = new System.Drawing.Size(20, 20);
			ctxMenuCaption.Items.AddRange(new ToolStripItem[] { ctxCaptionText, ctxCaptionWidth, ctxInsertSpace, ctxInsertCaption, ctxToggleCaption, ctxRemoveSpace, ctxMergeCaptions, ctxDeleteCaption, ctxCaptionProperties, ctxSep1, ctxSelectNone, ctxSelectTimeFromCaption, ctxSnapCaptionToSelection });
			ctxMenuCaption.Name = "ctxMenuCaption";
			ctxMenuCaption.Size = new System.Drawing.Size(340, 298);
			// 
			// ctxCaptionText
			// 
			ctxCaptionText.Name = "ctxCaptionText";
			ctxCaptionText.ShortcutKeys = Keys.F2;
			ctxCaptionText.Size = new System.Drawing.Size(339, 24);
			ctxCaptionText.Text = "Caption &Text";
			// 
			// ctxCaptionWidth
			// 
			ctxCaptionWidth.Name = "ctxCaptionWidth";
			ctxCaptionWidth.ShortcutKeys = Keys.Control | Keys.W;
			ctxCaptionWidth.Size = new System.Drawing.Size(339, 24);
			ctxCaptionWidth.Text = "Caption &Width";
			// 
			// ctxInsertSpace
			// 
			ctxInsertSpace.Name = "ctxInsertSpace";
			ctxInsertSpace.ShortcutKeys = Keys.Control | Keys.Shift | Keys.I;
			ctxInsertSpace.Size = new System.Drawing.Size(339, 24);
			ctxInsertSpace.Text = "Insert &Space";
			// 
			// ctxInsertCaption
			// 
			ctxInsertCaption.Name = "ctxInsertCaption";
			ctxInsertCaption.ShortcutKeys = Keys.Control | Keys.I;
			ctxInsertCaption.Size = new System.Drawing.Size(339, 24);
			ctxInsertCaption.Text = "Split &Caption";
			// 
			// ctxToggleCaption
			// 
			ctxToggleCaption.Name = "ctxToggleCaption";
			ctxToggleCaption.ShortcutKeys = Keys.Control | Keys.G;
			ctxToggleCaption.Size = new System.Drawing.Size(339, 24);
			ctxToggleCaption.Text = "T&oggle Caption | Space";
			// 
			// ctxRemoveSpace
			// 
			ctxRemoveSpace.Name = "ctxRemoveSpace";
			ctxRemoveSpace.ShortcutKeys = Keys.Control | Keys.R;
			ctxRemoveSpace.Size = new System.Drawing.Size(339, 24);
			ctxRemoveSpace.Text = "&Remove Space";
			// 
			// ctxMergeCaptions
			// 
			ctxMergeCaptions.Name = "ctxMergeCaptions";
			ctxMergeCaptions.ShortcutKeys = Keys.Control | Keys.M;
			ctxMergeCaptions.Size = new System.Drawing.Size(339, 24);
			ctxMergeCaptions.Text = "&Merge Captions";
			// 
			// ctxDeleteCaption
			// 
			ctxDeleteCaption.Name = "ctxDeleteCaption";
			ctxDeleteCaption.ShortcutKeys = Keys.Control | Keys.D;
			ctxDeleteCaption.Size = new System.Drawing.Size(339, 24);
			ctxDeleteCaption.Text = "&Delete Caption";
			// 
			// ctxCaptionProperties
			// 
			ctxCaptionProperties.Name = "ctxCaptionProperties";
			ctxCaptionProperties.ShortcutKeys = Keys.Control | Keys.Return;
			ctxCaptionProperties.Size = new System.Drawing.Size(339, 24);
			ctxCaptionProperties.Text = "Caption &Properties";
			// 
			// ctxSep1
			// 
			ctxSep1.Name = "ctxSep1";
			ctxSep1.Size = new System.Drawing.Size(336, 6);
			// 
			// ctxSelectNone
			// 
			ctxSelectNone.Name = "ctxSelectNone";
			ctxSelectNone.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
			ctxSelectNone.Size = new System.Drawing.Size(339, 24);
			ctxSelectNone.Text = "Select &None";
			// 
			// ctxSelectTimeFromCaption
			// 
			ctxSelectTimeFromCaption.Name = "ctxSelectTimeFromCaption";
			ctxSelectTimeFromCaption.ShortcutKeys = Keys.Control | Keys.Shift | Keys.T;
			ctxSelectTimeFromCaption.Size = new System.Drawing.Size(339, 24);
			ctxSelectTimeFromCaption.Text = "Se&lect Time From Caption";
			// 
			// ctxSnapCaptionToSelection
			// 
			ctxSnapCaptionToSelection.Name = "ctxSnapCaptionToSelection";
			ctxSnapCaptionToSelection.ShortcutKeys = Keys.Control | Keys.E;
			ctxSnapCaptionToSelection.Size = new System.Drawing.Size(339, 24);
			ctxSnapCaptionToSelection.Text = "Snap Caption To S&election";
			// 
			// frmMain
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(918, 582);
			Controls.Add(pnlBorder);
			Controls.Add(toolStrip);
			Controls.Add(titleBar);
			Controls.Add(statusStrip);
			FormBorderStyle = FormBorderStyle.None;
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip;
			Name = "frmMain";
			Text = "CaptionAll";
			titleBar.MenuArea.ResumeLayout(false);
			titleBar.MenuArea.PerformLayout();
			menuStrip.ResumeLayout(false);
			menuStrip.PerformLayout();
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			pnlBorder.ResumeLayout(false);
			pnlBorderInner.ResumeLayout(false);
			pnlCaption.ResumeLayout(false);
			toolStrip.ResumeLayout(false);
			toolStrip.PerformLayout();
			ctxMenuCaption.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TitleBarWF.TitleBarWF titleBar;
		private MenuStrip menuStrip;
		private ToolStripMenuItem mnuFile;
		private ToolStripMenuItem mnuFileExit;
		private ToolStripMenuItem mnuFileOpenMedia;
		private ToolStripMenuItem mnuFileOpenCaptions;
		private ToolStripMenuItem mnuFileSaveCaptions;
		private ToolStripSeparator mnuFileSep1;
		private StatusStrip statusStrip;
		private ToolStripStatusLabel statMessage;
		private Panel pnlMedia;
		private Panel pnlBorder;
		private Panel pnlBorderInner;
		private Splitter split;
		private Panel pnlCaption;
		private ToolStripMenuItem mnuTransport;
		private ToolStripMenuItem mnuTransportStop;
		private ToolStripSeparator mnuTransportSep1;
		private ToolStripMenuItem mnuTransportGoToStart;
		private ToolStripMenuItem mnuTransportGoBack;
		private ToolStripSeparator mnuTransportSep2;
		private ToolStripMenuItem mnuTransportPlayPause;
		private ToolStripSeparator mnuTransportSep3;
		private ToolStripMenuItem mnuTransportGoForward;
		private ToolStripMenuItem mnuTransportGoToEnd;
		private ToolStripStatusLabel statTime;
		private CaptionBubbleEditorWF.CaptionBubbleControl captionEditor;
		private ScrollPanelVirtual.ScrollPanelControlVirt scrollTimeline;
		private ToolStripMenuItem mnuEdit;
		private ToolStripMenuItem mnuEditCurrentTime;
		private ToolStripMenuItem mnuEditSelectionStartTime;
		private ToolStripMenuItem mnuEditSelectionEndTime;
		private ToolStripMenuItem mnuEditSelectTimeFromCaption;
		private ToolStripSeparator mnuEditSep1;
		private ToolStripMenuItem mnuEditCaptionText;
		private ToolStripMenuItem mnuEditCaptionWidth;
		private ToolStripMenuItem mnuEditInsertSpace;
		private ToolStripMenuItem mnuEditInsertCaption;
		private ToolStripMenuItem mnuEditDeleteCaption;
		private ToolStripMenuItem mnuEditSnapCaptionToSelection;
		private ToolStripMenuItem mnuEditCaptionProperties;
		private ToolStripMenuItem mnuView;
		private ToolStripMenuItem mnuViewCenterCursor;
		private ToolStripMenuItem mnuEditToggleCaptionSpace;
		private ToolStripMenuItem mnuEditUndo;
		private ToolStripMenuItem mnuFileSaveCaptionsAs;
		private ToolStripMenuItem mnuEditRippleOnOff;
		private ToolStripSeparator mnuEditSep2;
		private ToolStripSeparator mnuEditSep3;
		private ToolStripMenuItem mnuEditSetTrackDuration;
		private ToolStripMenuItem mnuEditSelectNone;
		private ToolStripMenuItem mnuEditMergeCaptions;
		private ToolStripMenuItem mnuFileLoadProject;
		private ToolStripMenuItem mnuFileSaveProject;
		private ToolStripMenuItem mnuFileSaveProjectAs;
		private ToolStripSeparator mnuFileSep2;
		private ToolStripMenuItem mnuEditSpeedDialList;
		private ToolStripMenuItem mnuEditSelection;
		private ToolStripMenuItem mnuEditFind;
		private ToolStripMenuItem mnuEditCaption;
		private ToolStripMenuItem mnuEditFindAgain;
		private ToolStripMenuItem mnuEditFindAndReplace;
		private ToolStripButton tbtnTransportStop;
		private ToolStripSeparator tsepStop;
		private ToolStripButton tbtnTransportGoToStart;
		private ToolStripButton tbtnTransportGoBack;
		private ToolStripButton tbtnTransportPlayPause;
		private ToolStripButton tbtnTransportGoForward;
		private ToolStripButton tbtnTransportGoToEnd;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton tbtnEditRippleOnOff;
		private ToolStrip toolStrip;
		private ToolStripMenuItem mnuEditRemoveSpace;
		private ContextMenuStrip ctxMenuCaption;
		private ToolStripMenuItem ctxCaptionText;
		private ToolStripMenuItem ctxCaptionWidth;
		private ToolStripMenuItem ctxInsertSpace;
		private ToolStripMenuItem ctxInsertCaption;
		private ToolStripMenuItem ctxToggleCaption;
		private ToolStripMenuItem ctxRemoveSpace;
		private ToolStripMenuItem ctxMergeCaptions;
		private ToolStripMenuItem ctxDeleteCaption;
		private ToolStripMenuItem ctxCaptionProperties;
		private ToolStripSeparator ctxSep1;
		private ToolStripMenuItem ctxSelectNone;
		private ToolStripMenuItem ctxSelectTimeFromCaption;
		private ToolStripMenuItem ctxSnapCaptionToSelection;
		private ToolStripMenuItem mnuEditMoveContentToTime;
		private ToolStripMenuItem mnuExport;
		private ToolStripMenuItem mnuExportCaptions;
		private ToolStripMenuItem mnuExportCaptionsSRT;
		private ToolStripSeparator mnuFileSep3;
		private ToolStripMenuItem mnuFileExportTranscriptText;
		private ToolStripMenuItem mnuEditConvertMultiSingle;
		private ToolStripMenuItem mnuEditMoveLeftToTime;
		private ToolStripMenuItem mnuTools;
		private ToolStripMenuItem mnuToolsDisplayVTTInfo;
		private ToolStripMenuItem mnuToolsLongSoundCount;
		private ToolStripMenuItem mnuToolsShortSoundCount;
		private ToolStripMenuItem mnuToolsUpdateTargetFromSource;
		private ToolStripMenuItem mnuToolsReWrapStreamVTT;
		private ToolStripMenuItem mnuToolsExportMSBurstsExcel;
		private ToolStripMenuItem mnuHelp;
		private ToolStripMenuItem mnuHelpAbout;
		private ToolStripMenuItem mnuToolsAlignScriptToCaptions;
		private ToolStripMenuItem mnuEditExtendCaptionTails;
	}
}