namespace MyApplication;

partial class MainForm
{
	private System.ComponentModel.IContainer components = null;

	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
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
		downloadButton = new Button();
		targetPathTextBox = new TextBox();
		youTubeVideoIdTextBox = new TextBox();
		targetPathLabel = new Label();
		youTubeVideoIdLabel = new Label();
		detectButton = new Button();
		ffmpegPathNameLabel = new Label();
		ffmpegPathNameTextBox = new TextBox();
		headerPanel = new Panel();
		selectFFMpegPathNameButton = new Button();
		selectTargetPathButton = new Button();
		fixAndCheckButton = new Button();
		logsListBox = new ListBox();
		myMenuStrip = new MenuStrip();
		detailsPanel = new Panel();
		footerPanel = new Panel();
		downloadingStatusTextBox = new TextBox();
		downloadingProgressBar = new ProgressBar();
		downloadCaptionCheckBox = new CheckBox();
		downloadVideoCheckBox = new CheckBox();
		videoTitleTextBox = new TextBox();
		gridViewPanel = new Panel();
		myDataGridView = new DataGridView();
		downloadingTimer = new System.Windows.Forms.Timer(components);
		headerPanel.SuspendLayout();
		footerPanel.SuspendLayout();
		gridViewPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)myDataGridView).BeginInit();
		SuspendLayout();
		// 
		// downloadButton
		// 
		downloadButton.Location = new Point(177, 6);
		downloadButton.Name = "downloadButton";
		downloadButton.Size = new Size(151, 29);
		downloadButton.TabIndex = 2;
		downloadButton.Text = "&3 - Download";
		downloadButton.UseVisualStyleBackColor = true;
		downloadButton.Click += DownloadButton_Click;
		// 
		// targetPathTextBox
		// 
		targetPathTextBox.Location = new Point(147, 3);
		targetPathTextBox.Name = "targetPathTextBox";
		targetPathTextBox.Size = new Size(431, 27);
		targetPathTextBox.TabIndex = 1;
		targetPathTextBox.Text = "D:\\YouTubeDownloads";
		// 
		// youTubeVideoIdTextBox
		// 
		youTubeVideoIdTextBox.Location = new Point(147, 69);
		youTubeVideoIdTextBox.Name = "youTubeVideoIdTextBox";
		youTubeVideoIdTextBox.Size = new Size(467, 27);
		youTubeVideoIdTextBox.TabIndex = 7;
		youTubeVideoIdTextBox.TextChanged += YouTubeVideoIdTextBox_TextChanged;
		youTubeVideoIdTextBox.DoubleClick += YouTubeVideoIdTextBox_DoubleClick;
		youTubeVideoIdTextBox.Enter += YouTubeVideoIdTextBox_Enter;
		// 
		// targetPathLabel
		// 
		targetPathLabel.AutoSize = true;
		targetPathLabel.Location = new Point(3, 6);
		targetPathLabel.Name = "targetPathLabel";
		targetPathLabel.Size = new Size(82, 20);
		targetPathLabel.TabIndex = 0;
		targetPathLabel.Text = "&Target Path";
		// 
		// youTubeVideoIdLabel
		// 
		youTubeVideoIdLabel.AutoSize = true;
		youTubeVideoIdLabel.Location = new Point(3, 72);
		youTubeVideoIdLabel.Name = "youTubeVideoIdLabel";
		youTubeVideoIdLabel.Size = new Size(128, 20);
		youTubeVideoIdLabel.TabIndex = 6;
		youTubeVideoIdLabel.Text = "&YouTube Video ID";
		// 
		// detectButton
		// 
		detectButton.Location = new Point(304, 102);
		detectButton.Name = "detectButton";
		detectButton.Size = new Size(151, 29);
		detectButton.TabIndex = 9;
		detectButton.Text = "&2 - Detect";
		detectButton.UseVisualStyleBackColor = true;
		detectButton.Click += DetectButton_Click;
		// 
		// ffmpegPathNameLabel
		// 
		ffmpegPathNameLabel.AutoSize = true;
		ffmpegPathNameLabel.Location = new Point(3, 39);
		ffmpegPathNameLabel.Name = "ffmpegPathNameLabel";
		ffmpegPathNameLabel.Size = new Size(138, 20);
		ffmpegPathNameLabel.TabIndex = 3;
		ffmpegPathNameLabel.Text = "&FFMpeg Path Name";
		// 
		// ffmpegPathNameTextBox
		// 
		ffmpegPathNameTextBox.Location = new Point(147, 36);
		ffmpegPathNameTextBox.Name = "ffmpegPathNameTextBox";
		ffmpegPathNameTextBox.Size = new Size(431, 27);
		ffmpegPathNameTextBox.TabIndex = 4;
		ffmpegPathNameTextBox.Text = "D:\\Download\\FFMpeg\\ffmpeg-windows-x64\\ffmpeg.exe";
		// 
		// headerPanel
		// 
		headerPanel.Controls.Add(selectFFMpegPathNameButton);
		headerPanel.Controls.Add(selectTargetPathButton);
		headerPanel.Controls.Add(fixAndCheckButton);
		headerPanel.Controls.Add(logsListBox);
		headerPanel.Controls.Add(targetPathTextBox);
		headerPanel.Controls.Add(ffmpegPathNameTextBox);
		headerPanel.Controls.Add(ffmpegPathNameLabel);
		headerPanel.Controls.Add(youTubeVideoIdTextBox);
		headerPanel.Controls.Add(targetPathLabel);
		headerPanel.Controls.Add(detectButton);
		headerPanel.Controls.Add(youTubeVideoIdLabel);
		headerPanel.Dock = DockStyle.Top;
		headerPanel.Location = new Point(0, 24);
		headerPanel.Name = "headerPanel";
		headerPanel.Size = new Size(1432, 138);
		headerPanel.TabIndex = 1;
		// 
		// selectFFMpegPathNameButton
		// 
		selectFFMpegPathNameButton.Location = new Point(584, 39);
		selectFFMpegPathNameButton.Name = "selectFFMpegPathNameButton";
		selectFFMpegPathNameButton.Size = new Size(30, 24);
		selectFFMpegPathNameButton.TabIndex = 5;
		selectFFMpegPathNameButton.Text = "...";
		selectFFMpegPathNameButton.UseVisualStyleBackColor = true;
		selectFFMpegPathNameButton.Click += SelectFFMpegPathNameButton_Click;
		// 
		// selectTargetPathButton
		// 
		selectTargetPathButton.Location = new Point(584, 6);
		selectTargetPathButton.Name = "selectTargetPathButton";
		selectTargetPathButton.Size = new Size(30, 24);
		selectTargetPathButton.TabIndex = 2;
		selectTargetPathButton.Text = "...";
		selectTargetPathButton.UseVisualStyleBackColor = true;
		selectTargetPathButton.Click += SelectTargetPathButton_Click;
		// 
		// fixAndCheckButton
		// 
		fixAndCheckButton.Location = new Point(147, 102);
		fixAndCheckButton.Name = "fixAndCheckButton";
		fixAndCheckButton.Size = new Size(151, 29);
		fixAndCheckButton.TabIndex = 8;
		fixAndCheckButton.Text = "&1 - Fix and Check";
		fixAndCheckButton.UseVisualStyleBackColor = true;
		fixAndCheckButton.Click += FixAndCheckButton_Click;
		// 
		// logsListBox
		// 
		logsListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		logsListBox.BorderStyle = BorderStyle.None;
		logsListBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
		logsListBox.FormattingEnabled = true;
		logsListBox.Location = new Point(620, 0);
		logsListBox.Name = "logsListBox";
		logsListBox.Size = new Size(809, 140);
		logsListBox.TabIndex = 10;
		// 
		// myMenuStrip
		// 
		myMenuStrip.ImageScalingSize = new Size(20, 20);
		myMenuStrip.Location = new Point(0, 0);
		myMenuStrip.Name = "myMenuStrip";
		myMenuStrip.Size = new Size(1432, 24);
		myMenuStrip.TabIndex = 0;
		myMenuStrip.Text = "menuStrip1";
		// 
		// detailsPanel
		// 
		detailsPanel.Dock = DockStyle.Top;
		detailsPanel.Location = new Point(0, 162);
		detailsPanel.Name = "detailsPanel";
		detailsPanel.Size = new Size(1432, 13);
		detailsPanel.TabIndex = 2;
		// 
		// footerPanel
		// 
		footerPanel.Controls.Add(downloadingStatusTextBox);
		footerPanel.Controls.Add(downloadingProgressBar);
		footerPanel.Controls.Add(downloadCaptionCheckBox);
		footerPanel.Controls.Add(downloadVideoCheckBox);
		footerPanel.Controls.Add(videoTitleTextBox);
		footerPanel.Controls.Add(downloadButton);
		footerPanel.Dock = DockStyle.Bottom;
		footerPanel.Location = new Point(0, 707);
		footerPanel.Name = "footerPanel";
		footerPanel.Size = new Size(1432, 46);
		footerPanel.TabIndex = 3;
		// 
		// downloadingStatusTextBox
		// 
		downloadingStatusTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		downloadingStatusTextBox.Location = new Point(913, 6);
		downloadingStatusTextBox.Name = "downloadingStatusTextBox";
		downloadingStatusTextBox.ReadOnly = true;
		downloadingStatusTextBox.Size = new Size(147, 27);
		downloadingStatusTextBox.TabIndex = 4;
		downloadingStatusTextBox.Visible = false;
		// 
		// downloadingProgressBar
		// 
		downloadingProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		downloadingProgressBar.Location = new Point(1066, 6);
		downloadingProgressBar.Name = "downloadingProgressBar";
		downloadingProgressBar.Size = new Size(354, 29);
		downloadingProgressBar.Step = 1;
		downloadingProgressBar.TabIndex = 5;
		// 
		// downloadCaptionCheckBox
		// 
		downloadCaptionCheckBox.AutoSize = true;
		downloadCaptionCheckBox.Checked = true;
		downloadCaptionCheckBox.CheckState = CheckState.Checked;
		downloadCaptionCheckBox.Location = new Point(88, 9);
		downloadCaptionCheckBox.Name = "downloadCaptionCheckBox";
		downloadCaptionCheckBox.Size = new Size(83, 24);
		downloadCaptionCheckBox.TabIndex = 1;
		downloadCaptionCheckBox.Text = "Caption";
		downloadCaptionCheckBox.UseVisualStyleBackColor = true;
		// 
		// downloadVideoCheckBox
		// 
		downloadVideoCheckBox.AutoSize = true;
		downloadVideoCheckBox.Checked = true;
		downloadVideoCheckBox.CheckState = CheckState.Checked;
		downloadVideoCheckBox.Location = new Point(12, 9);
		downloadVideoCheckBox.Name = "downloadVideoCheckBox";
		downloadVideoCheckBox.Size = new Size(70, 24);
		downloadVideoCheckBox.TabIndex = 0;
		downloadVideoCheckBox.Text = "Video";
		downloadVideoCheckBox.UseVisualStyleBackColor = true;
		// 
		// videoTitleTextBox
		// 
		videoTitleTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		videoTitleTextBox.Location = new Point(334, 6);
		videoTitleTextBox.Name = "videoTitleTextBox";
		videoTitleTextBox.ReadOnly = true;
		videoTitleTextBox.Size = new Size(573, 27);
		videoTitleTextBox.TabIndex = 3;
		videoTitleTextBox.Visible = false;
		// 
		// gridViewPanel
		// 
		gridViewPanel.Controls.Add(myDataGridView);
		gridViewPanel.Dock = DockStyle.Fill;
		gridViewPanel.Location = new Point(0, 175);
		gridViewPanel.Name = "gridViewPanel";
		gridViewPanel.Size = new Size(1432, 532);
		gridViewPanel.TabIndex = 13;
		// 
		// myDataGridView
		// 
		myDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		myDataGridView.Dock = DockStyle.Fill;
		myDataGridView.Location = new Point(0, 0);
		myDataGridView.Name = "myDataGridView";
		myDataGridView.RowHeadersWidth = 51;
		myDataGridView.Size = new Size(1432, 532);
		myDataGridView.TabIndex = 0;
		// 
		// downloadingTimer
		// 
		downloadingTimer.Interval = 1000;
		// 
		// MainForm
		// 
		AutoScaleDimensions = new SizeF(8F, 20F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1432, 753);
		Controls.Add(gridViewPanel);
		Controls.Add(footerPanel);
		Controls.Add(detailsPanel);
		Controls.Add(headerPanel);
		Controls.Add(myMenuStrip);
		MainMenuStrip = myMenuStrip;
		MinimumSize = new Size(1450, 800);
		Name = "MainForm";
		StartPosition = FormStartPosition.CenterScreen;
		Load += Form_Load;
		headerPanel.ResumeLayout(false);
		headerPanel.PerformLayout();
		footerPanel.ResumeLayout(false);
		footerPanel.PerformLayout();
		gridViewPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)myDataGridView).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Button downloadButton;
	private TextBox targetPathTextBox;
	private TextBox youTubeVideoIdTextBox;
	private Label targetPathLabel;
	private Label youTubeVideoIdLabel;
	private Button detectButton;
	private Label ffmpegPathNameLabel;
	private TextBox ffmpegPathNameTextBox;
	private Panel headerPanel;
	private MenuStrip myMenuStrip;
	private Panel detailsPanel;
	private Panel footerPanel;
	private Panel gridViewPanel;
	private DataGridView myDataGridView;
	private TextBox videoTitleTextBox;
	private CheckBox downloadCaptionCheckBox;
	private CheckBox downloadVideoCheckBox;
	private ListBox logsListBox;
	private Button fixAndCheckButton;
	private Button selectTargetPathButton;
	private Button selectFFMpegPathNameButton;
	private ProgressBar downloadingProgressBar;
	private System.Windows.Forms.Timer downloadingTimer;
	private TextBox downloadingStatusTextBox;
}
