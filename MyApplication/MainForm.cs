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

	private void Form_Load(object sender, EventArgs e)
	{
		downloadButton.Enabled = false;
	}

	private async void DetectButton_Click(object sender, EventArgs e)
	{
		detectButton.Enabled = false;
		downloadButton.Enabled = false;

		resolutionComboBox.Enabled = false;

		targetPathTextBox.Enabled = false;
		youTubeVideoIdTextBox.Enabled = false;
		ffmpegPathNameTextBox.Enabled = false;

		try
		{
			resolutionComboBox.Items.Clear();
			resolutionComboBox.Items.Add(item: string.Empty);

			var youtube =
				new YoutubeClient();

			var video =
				await
				youtube.Videos.GetAsync
				(videoId: youTubeVideoIdTextBox.Text);

			var videoTitle = video.Title;
			var channelId = video.Author.ChannelId;
			var channelTitle = video.Author.ChannelTitle;

			var streamManifest =
				await
				youtube.Videos.Streams
				.GetManifestAsync(videoId: youTubeVideoIdTextBox.Text);

			var videoStreams =
				streamManifest.GetVideoStreams();

			foreach (var stream in videoStreams)
			{
				var videoCodec = stream.VideoCodec;
				var bitrate = stream.Bitrate.BitsPerSecond;
				var size = $"{Math.Round(stream.Size.MegaBytes, 2)} MB";

				var containerName = stream.Container.Name;
				var videoQualityLabel = stream.VideoQuality.Label;
				var videoQualityFramerate = stream.VideoQuality.Framerate;

				var item =
					$"{containerName} | {videoQualityLabel} | {videoQualityFramerate} | {size}";

				if (containerName.ToLower() != "webm")
				{
					resolutionComboBox.Items.Add(item: item);
				}
			}

			downloadButton.Enabled = true;

			MessageBox.Show(text: "Finished.");
		}
		catch (Exception ex)
		{
			var errorMessage =
				$"Error! - {ex.Message}";

			MessageBox.Show(text: errorMessage);
		}

		detectButton.Enabled = true;
		resolutionComboBox.Enabled = true;

		targetPathTextBox.Enabled = true;
		youTubeVideoIdTextBox.Enabled = true;
		ffmpegPathNameTextBox.Enabled = true;
	}

	private async void DownloadButton_Click(object sender, EventArgs e)
	{
		detectButton.Enabled = false;
		downloadButton.Enabled = false;
		targetPathTextBox.Enabled = false;
		resolutionComboBox.Enabled = false;
		ffmpegPathNameTextBox.Enabled = false;
		youTubeVideoIdTextBox.Enabled = false;

		try
		{
			if (Directory.Exists(path: targetPathTextBox.Text) == false)
			{
				Directory.CreateDirectory(path: targetPathTextBox.Text);
			}

			// **************************************************
			var selectedItem =
				resolutionComboBox.SelectedItem;

			if (selectedItem is null)
			{
				return;
			}

			var selectedItemString = selectedItem.ToString();

			if (string.IsNullOrWhiteSpace(value: selectedItemString))
			{
				return;
			}

			selectedItemString =
				selectedItemString.Replace(" ", string.Empty);

			var selectedItems =
				selectedItemString.Split('|');

			var containerName = selectedItems[0];
			var videoQualityLabel = selectedItems[1];
			var videoQualityFramerate = Convert.ToInt32(selectedItems[2]);
			// **************************************************

			var youtube =
				new YoutubeClient();

			var video =
				await
				youtube.Videos.GetAsync
				(videoId: youTubeVideoIdTextBox.Text);

			var channelId = video.Author.ChannelId;
			var videoTitle = FixText(value: video.Title);
			var channelTitle = FixText(value: video.Author.ChannelTitle);

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
				.Where(current => current.VideoQuality.Label == videoQualityLabel)
				.Where(current => current.VideoQuality.Framerate == videoQualityFramerate)
				.First();

			var streamInfos =
				new IStreamInfo[] { audioStreamInfo, videoStreamInfo };

			var fileExtension =
				videoStreamInfo.Container.Name.ToLower();

			var fileName =
				$"{channelId}_{channelTitle}_{videoTitle}_{videoQualityLabel}_{videoQualityFramerate}";

			var videoFilePathName =
				$"{targetPathTextBox.Text}\\{fileName}.{fileExtension}";

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
					$"{targetPathTextBox.Text}\\{fileName}.srt";

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
		targetPathTextBox.Enabled = true;
		resolutionComboBox.Enabled = true;
		ffmpegPathNameTextBox.Enabled = true;
		youTubeVideoIdTextBox.Enabled = true;
	}

	public static string FixText(string value)
	{
		value =
			value
			.Replace(oldValue: ":", newValue: " ")
			.Replace(oldValue: "/", newValue: " ")
			.Replace(oldValue: "\\", newValue: " ")
			;

		value = value.Trim();

		while(value.Contains("  "))
		{
			value =
				value
				.Replace(oldValue: "  ", newValue: " ");
		}

		return value;
	}
}
