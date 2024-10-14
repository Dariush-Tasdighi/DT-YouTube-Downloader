using System.Media;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace MyApplication;

public partial class MainForm : Form
{
	public MainForm()
	{
		InitializeComponent();
	}

	private IList<YouTubeVideoItem> List { get; set; } = [];

	private void Form_Load(object sender, EventArgs e)
	{
		Text = "DT YouTube Downloader! - Version 3.4 - Always! Persian Gulf";

		downloadingTimer.Tick += DownloadingTimer_Tick;

		LogInformation(message: "Program Started.");

		// **************************************************
		detectButton.Enabled = false;
		downloadButton.Enabled = false;

		downloadVideoCheckBox.Enabled = false;
		downloadCaptionCheckBox.Enabled = false;
		// **************************************************

		// **************************************************
		myDataGridView.MultiSelect = false;

		myDataGridView.SelectionMode =
			DataGridViewSelectionMode.FullRowSelect;

		myDataGridView.EditMode =
			DataGridViewEditMode.EditProgrammatically;
		// **************************************************
	}

	private void YouTubeVideoIdTextBox_Enter(object sender, EventArgs e)
	{
		youTubeVideoIdTextBox.Select
			(start: 0, length: youTubeVideoIdTextBox.Text.Length);
	}

	private void YouTubeVideoIdTextBox_DoubleClick(object sender, EventArgs e)
	{
		youTubeVideoIdTextBox.Text = Clipboard.GetText();

		youTubeVideoIdTextBox.Select
			(start: 0, length: youTubeVideoIdTextBox.Text.Length);
	}

	private void YouTubeVideoIdTextBox_TextChanged(object sender, EventArgs e)
	{
		List.Clear();
		myDataGridView.DataSource = null;

		detectButton.Enabled = false;
		downloadButton.Enabled = false;
	}

	private void FixAndCheckButton_Click(object sender, EventArgs e)
	{
		if (CheckTargetPath() == false)
		{
			return;
		}

		LogInformation(message: $"Fix and check started.");

		FixYouTubeVideoIdTextBox();

		var videoId =
			youTubeVideoIdTextBox.Text
			.ToLower()
			.Replace(oldValue: "https://youtube.com/watch?v=", newValue: string.Empty)
			.Replace(oldValue: "https://www.youtube.com/watch?v=", newValue: string.Empty)
			;

		if (videoId.Length != 11)
		{
			return;
		}

		var directoryInfo =
			new DirectoryInfo(path: targetPathTextBox.Text);

		var files =
			directoryInfo
			.GetFiles(searchPattern: $"*{videoId}*",
			searchOption: SearchOption.AllDirectories)
			;

		detectButton.Enabled = true;

		if (files is not null && files.Length > 0)
		{
			LogInformation(message: $"You have been already downloaded the video / caption of {videoId}!");

			NotifyUserBySound();
		}
		else
		{
			LogInformation(message: $"Fix and check finished.");
		}
	}

	private void FixYouTubeVideoIdTextBox()
	{
		if (string.IsNullOrEmpty(youTubeVideoIdTextBox.Text))
		{
			return;
		}

		youTubeVideoIdTextBox.Text =
			youTubeVideoIdTextBox.Text
			.Trim()
			//.ToLower() // Never! Video Id & Address is case sensitive!
			;

		if (youTubeVideoIdTextBox.Text.Contains(value: "&t="))
		{
			var index =
				youTubeVideoIdTextBox.Text
				.ToLower()
				.IndexOf(value: "&t=");

			if (index != -1)
			{
				youTubeVideoIdTextBox.Text =
					youTubeVideoIdTextBox.Text
					.Substring(startIndex: 0, length: index);
			}
		}

		if (youTubeVideoIdTextBox.Text.Contains(value: "&list="))
		{
			var index =
				youTubeVideoIdTextBox.Text
				.ToLower()
				.IndexOf(value: "&list=");

			if (index != -1)
			{
				youTubeVideoIdTextBox.Text =
					youTubeVideoIdTextBox.Text
					.Substring(startIndex: 0, length: index);
			}
		}
	}

	private async void DetectButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(youTubeVideoIdTextBox.Text))
		{
			MessageBox.Show
				(text: "You did not specify YouTube video id!");

			youTubeVideoIdTextBox.Focus();

			return;
		}

		FixYouTubeVideoIdTextBox();

		DisableControls();
		myDataGridView.DataSource = null;

		try
		{
			LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Detection started.");

			List.Clear();

			var youtube =
				new YoutubeClient();

			var video =
				await
				youtube.Videos.GetAsync
				(videoId: youTubeVideoIdTextBox.Text);

			var streamManifest =
				await
				youtube.Videos.Streams
				.GetManifestAsync(videoId: youTubeVideoIdTextBox.Text);

			var videoStreams =
				streamManifest.GetVideoStreams();

			foreach (var stream in videoStreams)
			{
				var youTubeVideoItem =
					new YouTubeVideoItem(video: video);

				youTubeVideoItem.Update(stream);

				List.Add(item: youTubeVideoItem);
			}

			if (List.Count == 0)
			{
				MessageBox.Show
					(text: "Something Wrong! Please try again.");

				return;
			}

			myDataGridView.DataSource =
				List
				.OrderBy(current => current.StreamContainerName)
				.ThenByDescending(current => current.StreamVideoQualityIsHighDefinition)
				.ThenByDescending(current => current.StreamVideoQualityMaxHeight)
				.ThenByDescending(current => current.StreamSizeMegaBytes)
				.ToList()
				;

			for (var columnIndex = 0; columnIndex <= myDataGridView.Columns.Count - 1; columnIndex++)
			{
				switch (columnIndex)
				{
					case 1:
					{
						myDataGridView.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
						break;
					}

					default:
					{
						myDataGridView.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
						break;
					}
				}
			}

			myDataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Detection finished.");

			myDataGridView.Rows[0].Selected = true;
			myDataGridView.Focus();
		}
		catch (Exception ex)
		{
			LogException(exception: ex);
		}

		EnableControls();

		NotifyUserBySound();
	}

	private void NotifyUserBySound()
	{
		SystemSounds.Beep.Play();
	}

	private YouTubeVideoItem? SelectedYouTubeVideoItem { get; set; }

	private async void DownloadButton_Click(object sender, EventArgs e)
	{
		// **************************************************
		if (myDataGridView.SelectedRows.Count == 0)
		{
			MessageBox.Show
				(text: "Please select an item!");

			return;
		}

		var selectedItem =
			myDataGridView.Rows[myDataGridView.SelectedRows[0].Index].DataBoundItem
			as YouTubeVideoItem;

		if (selectedItem is null)
		{
			return;
		}

		SelectedYouTubeVideoItem = selectedItem;
		// **************************************************

		DisableControls();

		videoTitleTextBox.Visible = true;
		videoTitleTextBox.Text = selectedItem.Title;

		downloadingTimer.Enabled = true;
		downloadingProgressBar.Value = 0;
		downloadingProgressBar.Visible = true;
		downloadingStatusTextBox.Visible = true;

		try
		{
			if (CheckTargetPath() == false)
			{
				return;
			}

			var youtube =
				new YoutubeClient();

			if (downloadVideoCheckBox.Checked)
			{
				LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Downloading movie started.");

				var pathName = GetPathName();

				var videoPathNameWithExtension =
					$"{pathName}.{selectedItem.StreamContainerName}";

				if (File.Exists(path: videoPathNameWithExtension))
				{
					LogInformation(message: "This movie is already exist!");
				}
				else
				{
					var streamManifest =
						await
						youtube.Videos.Streams
						.GetManifestAsync(videoId: youTubeVideoIdTextBox.Text);

					var audioStreamInfo =
						streamManifest
						.GetAudioOnlyStreams()
						.GetWithHighestBitrate()
						;

					var videoStreamInfo = streamManifest
						.GetVideoOnlyStreams()
						.Where(current => current.Container.Name == selectedItem.StreamContainerName)
						.Where(current => current.VideoQuality.Label == selectedItem.StreamVideoQualityLabel)
						.Where(current => current.VideoQuality.Framerate == selectedItem.StreamVideoQualityFramerate)
						.First();

					var streamInfos =
						new IStreamInfo[] { audioStreamInfo, videoStreamInfo };

					var conversionRequestBuilder =
						new ConversionRequestBuilder(outputFilePath: videoPathNameWithExtension);

					conversionRequestBuilder.SetFFmpegPath(path: ffmpegPathNameTextBox.Text);

					await youtube.Videos.DownloadAsync
						(streamInfos: streamInfos, conversionRequestBuilder.Build());

					LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Downloading movie finished.");
				}
			}

			if (downloadCaptionCheckBox.Checked)
			{
				LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Downloading caption started.");

				var pathName = GetPathName();

				var captionPathNameWithExtension = $"{pathName}.srt";

				if (File.Exists(path: captionPathNameWithExtension))
				{
					LogInformation(message: "This caption is already exist!");
				}
				else
				{
					var trackManifest =
						await
						youtube.Videos.ClosedCaptions
						.GetManifestAsync(videoId: youTubeVideoIdTextBox.Text);

					var trackInfo =
						trackManifest.GetByLanguage(language: "en");

					if (trackInfo is not null)
					{
						await youtube.Videos.ClosedCaptions.DownloadAsync
							(trackInfo: trackInfo, filePath: captionPathNameWithExtension);
					}

					LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Downloading caption finished.");
				}
			}
		}
		catch (Exception ex)
		{
			LogException(exception: ex);
		}

		EnableControls();

		downloadingTimer.Enabled = false;
		videoTitleTextBox.Visible = false;
		downloadingProgressBar.Visible = false;
		downloadingStatusTextBox.Visible = false;

		NotifyUserBySound();
	}

	private string GetPathName()
	{
		if (SelectedYouTubeVideoItem is null)
		{
			LogInformation(message: "Error! - Unexpected Error!");
			return string.Empty;
		}

		var result =
			$"{targetPathTextBox.Text}\\{SelectedYouTubeVideoItem.GetFileName()}";

		return result;
	}

	private void DownloadingTimer_Tick(object? sender, EventArgs e)
	{
		if (SelectedYouTubeVideoItem is null)
		{
			return;
		}

		var pathName = GetPathName();

		var videoPathNameWithExtension =
			$"{pathName}.{SelectedYouTubeVideoItem.StreamContainerName}";

		var stream0PathNameWithExtension =
			$"{pathName}.{SelectedYouTubeVideoItem.StreamContainerName}.stream-0.tmp";

		var stream1PathNameWithExtension =
			$"{pathName}.{SelectedYouTubeVideoItem.StreamContainerName}.stream-1.tmp";

		var totalFileLength =
			SelectedYouTubeVideoItem.StreamSizeMegaBytes +
			SelectedYouTubeVideoItem.StreamSizeMegaBytes / (double)3;

		double percent = 0.0;

		string downloadingStatus;

		if (File.Exists(path: videoPathNameWithExtension))
		{
			downloadingStatus = "Merging";

			var fileInfo =
				new FileInfo(fileName: videoPathNameWithExtension);

			var currentFileLength =
				(double)fileInfo.Length / (double)(1024 * 1024);

			percent =
				((double)100 * currentFileLength) / totalFileLength;
		}
		else
		{
			downloadingStatus = "Downloading";

			double currentFilesLength = 0.0;

			if (File.Exists(path: stream0PathNameWithExtension))
			{
				var fileInfo =
					new FileInfo(fileName: stream0PathNameWithExtension);

				var currentFileLength =
					(double)fileInfo.Length / (double)(1024 * 1024);

				currentFilesLength += currentFileLength;
			}

			if (File.Exists(path: stream1PathNameWithExtension))
			{
				var fileInfo =
					new FileInfo(fileName: stream1PathNameWithExtension);

				var currentFileLength =
					(double)fileInfo.Length / (double)(1024 * 1024);

				currentFilesLength += currentFileLength;
			}

			percent =
				((double)100 * currentFilesLength) / totalFileLength;
		}

		if (percent > 100)
		{
			percent = 100;
		}

		int percentInt =
			Convert.ToInt32(percent);

		downloadingProgressBar.Value = percentInt;
		downloadingStatusTextBox.Text = $"{downloadingStatus}: {percentInt}%";
	}

	private void DisableControls()
	{
		myDataGridView.ReadOnly = true;

		targetPathTextBox.ReadOnly = true;
		youTubeVideoIdTextBox.ReadOnly = true;
		ffmpegPathNameTextBox.ReadOnly = true;

		downloadVideoCheckBox.Enabled = false;
		downloadCaptionCheckBox.Enabled = false;

		detectButton.Enabled = false;
		downloadButton.Enabled = false;
		fixAndCheckButton.Enabled = false;
		selectTargetPathButton.Enabled = false;
		selectFFMpegPathNameButton.Enabled = false;
	}

	private void EnableControls()
	{
		myDataGridView.ReadOnly = false;

		targetPathTextBox.ReadOnly = false;
		youTubeVideoIdTextBox.ReadOnly = false;
		ffmpegPathNameTextBox.ReadOnly = false;

		downloadVideoCheckBox.Enabled = true;
		downloadCaptionCheckBox.Enabled = true;

		detectButton.Enabled = true;
		downloadButton.Enabled = true;
		fixAndCheckButton.Enabled = true;
		selectTargetPathButton.Enabled = true;
		selectFFMpegPathNameButton.Enabled = true;
	}

	private bool CheckTargetPath()
	{
		LogInformation(message: $"Check target path ({targetPathTextBox.Text}) started.");

		bool result;

		try
		{
			if (Directory.Exists(path: targetPathTextBox.Text) == false)
			{
				Directory.CreateDirectory(path: targetPathTextBox.Text);
				LogInformation(message: $"The directory {targetPathTextBox.Text} created.");

			}

			result = true;
		}
		catch (Exception ex)
		{
			LogException(exception: ex);

			result = false;
		}

		LogInformation(message: $"Check target path ({targetPathTextBox.Text}) finished.");

		return result;
	}

	public void LogInformation(string message)
	{
		var item =
			$"{Utility.GetNow()} - {message}";

		logsListBox.Items.Insert(index: 0, item: item);
	}

	public void LogException(Exception exception)
	{
		if (exception.InnerException is not null)
		{
			var innerItem =
				$"\t{exception.InnerException.Message}";

			logsListBox.Items.Insert(index: 0, item: innerItem);
		}

		var item =
			$"{Utility.GetNow()} - Error! - {exception.Message}";

		logsListBox.Items.Insert(index: 0, item: item);
	}

	private FolderBrowserDialog MyFolderBrowserDialog { get; set; } = new();

	private void SelectTargetPathButton_Click(object sender, EventArgs e)
	{
		MyFolderBrowserDialog.ShowNewFolderButton = true;
		MyFolderBrowserDialog.InitialDirectory = targetPathTextBox.Text;

		var result =
			MyFolderBrowserDialog.ShowDialog();

		if (result == DialogResult.OK)
		{
			targetPathTextBox.Text =
				MyFolderBrowserDialog.SelectedPath;
		}
	}

	private OpenFileDialog MyOpenFileDialog { get; set; } = new();

	private void SelectFFMpegPathNameButton_Click(object sender, EventArgs e)
	{
		MyOpenFileDialog.Multiselect = false;
		MyOpenFileDialog.CheckFileExists = true;
		MyOpenFileDialog.CheckPathExists = true;
		MyOpenFileDialog.InitialDirectory = "D:\\";
		MyOpenFileDialog.Filter = "Executable Files|*.exe|All Files|*.*";

		var result =
			MyOpenFileDialog.ShowDialog();

		if (result == DialogResult.OK)
		{
			ffmpegPathNameTextBox.Text = MyOpenFileDialog.FileName;
		}
	}
}
