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
		downloadButton = new Button();
		targetPathTextBox = new TextBox();
		youTubeVideoIdTextBox = new TextBox();
		targetPathLabel = new Label();
		youTubeVideoIdLabel = new Label();
		detectButton = new Button();
		resolutionComboBox = new ComboBox();
		ffmpegPathNameLabel = new Label();
		ffmpegPathNameTextBox = new TextBox();
		SuspendLayout();
		// 
		// downloadButton
		// 
		downloadButton.Location = new Point(156, 180);
		downloadButton.Name = "downloadButton";
		downloadButton.Size = new Size(151, 29);
		downloadButton.TabIndex = 8;
		downloadButton.Text = "&2 - Download";
		downloadButton.UseVisualStyleBackColor = true;
		downloadButton.Click += DownloadButton_Click;
		// 
		// targetPathTextBox
		// 
		targetPathTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		targetPathTextBox.Location = new Point(156, 12);
		targetPathTextBox.Name = "targetPathTextBox";
		targetPathTextBox.Size = new Size(457, 27);
		targetPathTextBox.TabIndex = 1;
		targetPathTextBox.Text = "D:\\YouTubeDownloads";
		// 
		// youTubeVideoIdTextBox
		// 
		youTubeVideoIdTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		youTubeVideoIdTextBox.Location = new Point(156, 78);
		youTubeVideoIdTextBox.Name = "youTubeVideoIdTextBox";
		youTubeVideoIdTextBox.Size = new Size(457, 27);
		youTubeVideoIdTextBox.TabIndex = 5;
		youTubeVideoIdTextBox.Text = "https://youtube.com/watch?v=u_yIGGhubZs";
		// 
		// targetPathLabel
		// 
		targetPathLabel.AutoSize = true;
		targetPathLabel.Location = new Point(12, 15);
		targetPathLabel.Name = "targetPathLabel";
		targetPathLabel.Size = new Size(82, 20);
		targetPathLabel.TabIndex = 0;
		targetPathLabel.Text = "&Target Path";
		// 
		// youTubeVideoIdLabel
		// 
		youTubeVideoIdLabel.AutoSize = true;
		youTubeVideoIdLabel.Location = new Point(12, 81);
		youTubeVideoIdLabel.Name = "youTubeVideoIdLabel";
		youTubeVideoIdLabel.Size = new Size(128, 20);
		youTubeVideoIdLabel.TabIndex = 4;
		youTubeVideoIdLabel.Text = "&YouTube Video ID";
		// 
		// detectButton
		// 
		detectButton.Location = new Point(156, 111);
		detectButton.Name = "detectButton";
		detectButton.Size = new Size(151, 29);
		detectButton.TabIndex = 6;
		detectButton.Text = "&1 - Detect";
		detectButton.UseVisualStyleBackColor = true;
		detectButton.Click += DetectButton_Click;
		// 
		// resolutionComboBox
		// 
		resolutionComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		resolutionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
		resolutionComboBox.FormattingEnabled = true;
		resolutionComboBox.Location = new Point(156, 146);
		resolutionComboBox.Name = "resolutionComboBox";
		resolutionComboBox.Size = new Size(457, 28);
		resolutionComboBox.Sorted = true;
		resolutionComboBox.TabIndex = 7;
		// 
		// ffmpegPathNameLabel
		// 
		ffmpegPathNameLabel.AutoSize = true;
		ffmpegPathNameLabel.Location = new Point(12, 48);
		ffmpegPathNameLabel.Name = "ffmpegPathNameLabel";
		ffmpegPathNameLabel.Size = new Size(138, 20);
		ffmpegPathNameLabel.TabIndex = 2;
		ffmpegPathNameLabel.Text = "&FFMpeg Path Name";
		// 
		// ffmpegPathNameTextBox
		// 
		ffmpegPathNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		ffmpegPathNameTextBox.Location = new Point(156, 45);
		ffmpegPathNameTextBox.Name = "ffmpegPathNameTextBox";
		ffmpegPathNameTextBox.Size = new Size(457, 27);
		ffmpegPathNameTextBox.TabIndex = 3;
		ffmpegPathNameTextBox.Text = "D:\\Download\\FFMpeg\\ffmpeg-windows-x64\\ffmpeg.exe";
		// 
		// MainForm
		// 
		AutoScaleDimensions = new SizeF(8F, 20F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(625, 217);
		Controls.Add(ffmpegPathNameTextBox);
		Controls.Add(ffmpegPathNameLabel);
		Controls.Add(resolutionComboBox);
		Controls.Add(detectButton);
		Controls.Add(youTubeVideoIdLabel);
		Controls.Add(targetPathLabel);
		Controls.Add(youTubeVideoIdTextBox);
		Controls.Add(targetPathTextBox);
		Controls.Add(downloadButton);
		Name = "MainForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "DT YouTube Downloader! - Version 1.5";
		Load += Form_Load;
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
	private ComboBox resolutionComboBox;
	private Label ffmpegPathNameLabel;
	private TextBox ffmpegPathNameTextBox;
}
