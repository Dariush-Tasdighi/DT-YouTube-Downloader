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
		// **************************************************
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
	}

	private async void DetectButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(youTubeVideoIdTextBox.Text))
		{
			MessageBox.Show
				(text: "You did not specify YouTube video id!");

			return;
		}

		// **************************************************
		detectButton.Enabled = false;
		downloadButton.Enabled = false;

		myDataGridView.Enabled = false;
		myDataGridView.DataSource = null;

		targetPathTextBox.Enabled = false;
		youTubeVideoIdTextBox.Enabled = false;
		ffmpegPathNameTextBox.Enabled = false;

		downloadVideoCheckBox.Enabled = false;
		downloadCaptionCheckBox.Enabled = false;
		// **************************************************

		try
		{
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
					(text: "Something Wrong! Please try again...");

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

			MessageBox.Show(text: "Detection Finished...");

			downloadButton.Enabled = true;

			myDataGridView.Rows[0].Selected = true;
			myDataGridView.Focus();
		}
		catch (Exception ex)
		{
			var errorMessage =
				$"Error! - {ex.Message}";

			if (ex.InnerException is not null)
			{
				errorMessage +=
					$"{Environment.NewLine}{ex.InnerException.Message}";
			}

			MessageBox.Show(text: errorMessage);
		}

		// **************************************************
		detectButton.Enabled = true;
		myDataGridView.Enabled = true;

		targetPathTextBox.Enabled = true;
		youTubeVideoIdTextBox.Enabled = true;
		ffmpegPathNameTextBox.Enabled = true;

		downloadVideoCheckBox.Enabled = true;
		downloadCaptionCheckBox.Enabled = true;
		// **************************************************
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

		// **************************************************
		videoTitleTextBox.Visible = true;
		videoTitleTextBox.Text = selectedItem.Title;

		detectButton.Enabled = false;
		downloadButton.Enabled = false;

		myDataGridView.Enabled = false;

		targetPathTextBox.Enabled = false;
		ffmpegPathNameTextBox.Enabled = false;
		youTubeVideoIdTextBox.Enabled = false;

		downloadVideoCheckBox.Enabled = false;
		downloadCaptionCheckBox.Enabled = false;
		// **************************************************

		try
		{
			if (Directory.Exists(path: targetPathTextBox.Text) == false)
			{
				Directory.CreateDirectory(path: targetPathTextBox.Text);
			}

			var youtube =
				new YoutubeClient();

			if(downloadVideoCheckBox.Checked)
			{
				var videoFilePathName =
					$"{targetPathTextBox.Text}\\{selectedItem.GetFileName()}.{selectedItem.StreamContainerName}";

				if (File.Exists(path: videoFilePathName))
				{
					MessageBox.Show
						(text: "This movie is already exist!");
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
				}
			}

			if (downloadCaptionCheckBox.Checked)
			{
				var captionFilePathName =
					$"{targetPathTextBox.Text}\\{selectedItem.GetFileName()}.srt";

				if (File.Exists(path: captionFilePathName))
				{
					MessageBox.Show
						(text: "This caption is already exist!");
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
				}
			}

			MessageBox.Show(text: "Download Finished...");
		}
		catch (Exception ex)
		{
			var errorMessage =
				$"Error! - {ex.Message}";

			MessageBox.Show(text: errorMessage);
		}

		// **************************************************
		videoTitleTextBox.Visible = false;

		detectButton.Enabled = true;
		downloadButton.Enabled = true;

		myDataGridView.Enabled = true;

		targetPathTextBox.Enabled = true;
		ffmpegPathNameTextBox.Enabled = true;
		youTubeVideoIdTextBox.Enabled = true;

		downloadVideoCheckBox.Enabled = true;
		downloadCaptionCheckBox.Enabled = true;
		// **************************************************
	}
}
