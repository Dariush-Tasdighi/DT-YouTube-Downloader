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
		downloadButton.Enabled = false;

		myDataGridView.MultiSelect = false;

		myDataGridView.SelectionMode =
			DataGridViewSelectionMode.FullRowSelect;

		myDataGridView.EditMode =
			DataGridViewEditMode.EditProgrammatically;
	}

	private async void DetectButton_Click(object sender, EventArgs e)
	{
		detectButton.Enabled = false;
		downloadButton.Enabled = false;

		myDataGridView.Enabled = false;
		myDataGridView.DataSource = null;

		targetPathTextBox.Enabled = false;
		youTubeVideoIdTextBox.Enabled = false;
		ffmpegPathNameTextBox.Enabled = false;

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

				if (youTubeVideoItem.StreamContainerName != "webm")
				{
					List.Add(item: youTubeVideoItem);
				}
			}

			downloadButton.Enabled = true;

			myDataGridView.DataSource =
				List
				.OrderByDescending(current => current.StreamVideoQualityIsHighDefinition)
				.ThenByDescending(current => current.StreamVideoQualityMaxHeight)
				.ToList()
				;

			for (int columnIndex = 0; columnIndex <= myDataGridView.Columns.Count - 1; columnIndex++)
			{
				switch(columnIndex)
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

			myDataGridView.Rows[0].Selected = true;

			MessageBox.Show(text: "Finished.");
		}
		catch (Exception ex)
		{
			var errorMessage =
				$"Error! - {ex.Message}";

			if(ex.InnerException is not null)
			{
				errorMessage +=
					$"{Environment.NewLine}{ex.InnerException.Message}";
			}

			MessageBox.Show(text: errorMessage);
		}

		detectButton.Enabled = true;
		myDataGridView.Enabled = true;

		targetPathTextBox.Enabled = true;
		youTubeVideoIdTextBox.Enabled = true;
		ffmpegPathNameTextBox.Enabled = true;
	}

	private async void DownloadButton_Click(object sender, EventArgs e)
	{
		// **************************************************
		if (myDataGridView.SelectedRows.Count == 0)
		{
			MessageBox.Show("Please select an item!");
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

		detectButton.Enabled = false;
		downloadButton.Enabled = false;
		myDataGridView.Enabled = false;
		targetPathTextBox.Enabled = false;
		ffmpegPathNameTextBox.Enabled = false;
		youTubeVideoIdTextBox.Enabled = false;

		try
		{
			if (Directory.Exists(path: targetPathTextBox.Text) == false)
			{
				Directory.CreateDirectory(path: targetPathTextBox.Text);
			}

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

			var audioStreamInfo =
				streamManifest
				.GetAudioOnlyStreams()
				.GetWithHighestBitrate()
				;

			var videoStreamInfo = streamManifest
				.GetVideoOnlyStreams()
				.Where(current => current.Container == YoutubeExplode.Videos.Streams.Container.Mp4)
				.Where(current => current.VideoQuality.Label == selectedItem.StreamVideoQualityLabel)
				.Where(current => current.VideoQuality.Framerate == selectedItem.StreamVideoQualityFramerate)
				.First();

			var streamInfos =
				new IStreamInfo[] { audioStreamInfo, videoStreamInfo };

			var videoFilePathName =
				$"{targetPathTextBox.Text}\\{selectedItem.GetFileName()}.{selectedItem.StreamContainerName}";

			var conversionRequestBuilder =
				new ConversionRequestBuilder(outputFilePath: videoFilePathName);

			conversionRequestBuilder.SetFFmpegPath(path: ffmpegPathNameTextBox.Text);

			await youtube.Videos.DownloadAsync
				(streamInfos: streamInfos, conversionRequestBuilder.Build());

			// **************************************************
			// Download Caption
			// **************************************************
			var trackManifest =
				await
				youtube.Videos.ClosedCaptions
				.GetManifestAsync(videoId: youTubeVideoIdTextBox.Text);

			var trackInfo =
				trackManifest.GetByLanguage(language: "en");

			if (trackInfo is not null)
			{
				var captionFilePathName =
					$"{targetPathTextBox.Text}\\{selectedItem.GetFileName()}.srt";

				await youtube.Videos.ClosedCaptions.DownloadAsync
					(trackInfo: trackInfo, filePath: captionFilePathName);
			}
			// **************************************************

			MessageBox.Show(text: "Finished.");
		}
		catch (Exception ex)
		{
			var errorMessage =
				$"Error! - {ex.Message}";

			MessageBox.Show(text: errorMessage);
		}

		detectButton.Enabled = true;
		downloadButton.Enabled = true;
		myDataGridView.Enabled = true;
		targetPathTextBox.Enabled = true;
		ffmpegPathNameTextBox.Enabled = true;
		youTubeVideoIdTextBox.Enabled = true;
	}
}
