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

	private IList<YouTubeVideoItem> List = [];

	private void Form_Load(object sender, EventArgs e)
	{
		Text = "DT YouTube Downloader! - Version 2.3 - Always! Persian Gulf";

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
			.GetFiles(searchPattern: $"*{videoId}*")
			;

		if (files is not null && files.Length > 0)
		{
			LogInformation(message: $"You have been already downloaded the video or caption of {videoId}!");

			NotifyUserBySound();
		}

		detectButton.Enabled = true;
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

			for (int columnIndex = 0; columnIndex <= myDataGridView.Columns.Count - 1; columnIndex++)
			{
				switch (columnIndex)
				{
					case 1:
					{
						break;
					}

					default:
					{
						myDataGridView.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
						break;
					}
				}
			}

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
		// **************************************************

		DisableControls();

		videoTitleTextBox.Visible = true;
		videoTitleTextBox.Text = selectedItem.Title;

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

				var videoFilePathName =
					$"{targetPathTextBox.Text}\\{selectedItem.GetFileName()}.{selectedItem.StreamContainerName}";

				if (File.Exists(path: videoFilePathName))
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
						new ConversionRequestBuilder(outputFilePath: videoFilePathName);

					conversionRequestBuilder.SetFFmpegPath(path: ffmpegPathNameTextBox.Text);

					await youtube.Videos.DownloadAsync
						(streamInfos: streamInfos, conversionRequestBuilder.Build());

					LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Downloading movie finished.");
				}
			}

			if (downloadCaptionCheckBox.Checked)
			{
				LogInformation(message: $"{youTubeVideoIdTextBox.Text}: Downloading caption started.");

				var captionFilePathName =
					$"{targetPathTextBox.Text}\\{selectedItem.GetFileName()}.srt";

				if (File.Exists(path: captionFilePathName))
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
							(trackInfo: trackInfo, filePath: captionFilePathName);
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

		videoTitleTextBox.Visible = false;

		NotifyUserBySound();
	}

	private void DisableControls()
	{
		detectButton.Enabled = false;
		downloadButton.Enabled = false;
		fixAndCheckButton.Enabled = false;

		myDataGridView.Enabled = false;

		targetPathTextBox.Enabled = false;
		youTubeVideoIdTextBox.Enabled = false;
		ffmpegPathNameTextBox.Enabled = false;

		downloadVideoCheckBox.Enabled = false;
		downloadCaptionCheckBox.Enabled = false;
	}

	private void EnableControls()
	{
		detectButton.Enabled = true;
		downloadButton.Enabled = true;
		fixAndCheckButton.Enabled = true;

		myDataGridView.Enabled = true;

		targetPathTextBox.Enabled = true;
		youTubeVideoIdTextBox.Enabled = true;
		ffmpegPathNameTextBox.Enabled = true;

		downloadVideoCheckBox.Enabled = true;
		downloadCaptionCheckBox.Enabled = true;
	}

	private bool CheckTargetPath()
	{
		try
		{
			if (Directory.Exists(path: targetPathTextBox.Text) == false)
			{
				Directory.CreateDirectory(path: targetPathTextBox.Text);
				LogInformation(message: $"The directory {targetPathTextBox.Text} created.");

			}

			return true;
		}
		catch (Exception ex)
		{
			LogException(exception: ex);

			return false;
		}
	}

	private static string GetNow()
	{
		var result =
			DateTime.Now.ToString
			(format: "yyyy/mm/dd - HH:mm:ss");

		return result;
	}

	public void LogInformation(string message)
	{
		var item =
			$"{GetNow()} - {message}";

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
			$"{GetNow()} - Error! - {exception.Message}";

		logsListBox.Items.Insert(index: 0, item: item);
	}
}
