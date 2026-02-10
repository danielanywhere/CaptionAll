<p>&nbsp;</p>

## Distribution


CaptionAll is available as a **.torrent** file at The Internet Archive, the world's most comprehensive public digital library and most complete archive of web history. It is a non-profit organization that operates under the ambitious mission of providing *Universal Access to All Knowledge*.

According to their About page at <https://archive.org/about/>, their archive consists of the following.

-   1 trillion web pages.
-   49 million books and texts.
-   15 million audio recordings, which include 268,000 live concerts.
-   13 million videos, which include 3 million television news programs.
-   5 million images.
-   1 million software applications.

They also have the distinction of serving as a congressionally designated depository for U.S. Government documents, the public access of which is guaranteed by Title 44 of the United States Code.

<p>&nbsp;</p>

### Working with Torrent Files

Torrent files, their networks, and their associated applications application enable users to download and share files by distributing small pieces across many peers rather than relying on a single server, improving speed and resilience while using a completely decentralized, peer‑to‑peer network. Many Linux files are distributed in this way.

They allow groups and individuals to avoid the suppressive efforts of authoritarian, dictatorial, totalitarian, and draconian organizations and institutions, while remaining free to access and publicly available at all times.

<p>&nbsp;</p>

### Install Transmission

To work with torrent files, you will need to get a torrent file manager if you don't already have one. In this instance, I am going to outline the steps for downloading and installing **Transmission**, a popular and reliable software application for handling .torrent files. This section describes the basic installation and configuration process.

Follow these steps if you need to install and configure Transmission. Otherwise, you may skip to the next section.

-   Navigate to the **Transmission** home page at <https://transmissionbt.com/>.
-   Click the button most closely resembling "**Download v4.1.0 stable**".
-   In the **Windows** group, click **transmission-4.1.0-x64.msi**.
-   In your browser downloads list, click to open and run the file.
-   If you are presented with any kind of warning that the file is an executable (common in Firefox), click **OK**.
-   Follow the instructions on each page of the wizard, clicking **Next** to advance to the next page, and finally clicking **Finish** to perform the installation.
-   Assuming that you left the option checked to create a link on your desktop, minimize your windows, find the '**Transmission Qt Client**' shortcut on your desktop, and double-click to launch the application.
-   In the **Change Session** dialog, select **Start Local Session**.
-   Click **OK**.
-   If you see a **Windows Security Alert** for **Windows Defender Firewall**, do NOT allow access to either **Private** or **Public** networks, unless you have specific crowd-based reasons to do so. After unchecking both options, you will notice that the only enabled buttons are "**X**" and **Cancel**. Click either button to close the dialog.

<p>&nbsp;</p>

### Retrieve and Install CaptionAll

With Transmission installed, follow the steps of one of these two processes to retrieve and install CaptionAll.

<p>&nbsp;</p>

#### Download Within Transmission

You can initiate the torrent file download from within Transmission while it is already running.

-   Select the menu option **File / Open URL**.
-   In the dialog **Open Torrent from URL or Magnet Link**, set the following values.
-   Source:  
    `https://archive.org/download/caption-all-setup/caption-all-setup_archive.torrent`
-   Destination folder: (Your choice. This is set to the Downloads folder by default.)
-   Priority: **Normal**
-   Start when added: **Checked**
-   Move .torrent file to the trash: **Checked**
-   Click **Open**.
-   The package will download to your specified folder. This may take a few moments.
-   Right-click the download progress bar for caption-all-setup, and from the context menu, select **Open Folder**.
-   Double-click to open the **CaptionAll** sub-folder.

Following is the file structure you can expect to see within the CaptionAll folder.

-   **CaptionAll**. Main application content folder.

    -   CaptionAll-GettingStartedGuide.pdf. Tips and tricks for getting started. Note that several other versions of this document are created and maintained automatically in this folder by Internet Archive for accessibility reasons.

    -   LICENSE. AGPL-3.0 License.

    -   Setup

        -   **CaptionAllSetup.exe**. CaptionAll setup application.

    -   Source

        -   CaptionAll-main.zip. CaptionAll source code from the Main branch of the GitHub repository at <https://github.com/danielanywhere/CaptionAll>.

From here, you can setup CaptionAll by opening the setup folder and double-clicking **CaptionAllSetup.exe**. Acknowledge the license and follow the instructions on each page, clicking **Next** each time to advance to the next page. Finally, click **Finish** to perform the installation.

