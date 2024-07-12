using System.ComponentModel;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace MyApplication;

internal class YouTubeVideoItem : object
{
	public YouTubeVideoItem(Video video) : base()
	{
		Id = video.Id;
		Url = video.Url;
		Title = video.Title;
		Duration = video.Duration;
		UploadDate = video.UploadDate;
		Description = video.Description;

		// keywords = video.Keywords; // TODO
		// thumbnails = video.Thumbnails; // TODO

		// authorTitle = video.Author.Title; // Deprecated

		AuthorChannelId = video.Author.ChannelId;
		AuthorChannelUrl = video.Author.ChannelUrl;
		AuthorChannelTitle = video.Author.ChannelTitle;

		EngagementLikeCount = video.Engagement.LikeCount;
		EngagementViewCount = video.Engagement.ViewCount;
		EngagementDislikeCount = video.Engagement.DislikeCount;
		EngagementAverageRating = video.Engagement.AverageRating;
	}

	[DisplayName(displayName: "Channel Title")]
	public string? AuthorChannelTitle { get; set; }

	public string? Title { get; set; }

	public TimeSpan? Duration { get; set; }

	[Browsable(browsable: false)]
	public DateTimeOffset UploadDate { get; set; }

	[DisplayName(displayName: "Uploaded")]
	public string UploadDateString
	{
		get
		{
			var result =
				UploadDate.ToString(format: "yyyy/MM/dd");

			return result;
		}
	}

	[Browsable(browsable: false)]
	public double StreamSizeMegaBytes { get; set; }

	[DisplayName(displayName: "Size")]
	public string StreamSizeMegaBytesString
	{
		get
		{
			var result = $"{StreamSizeMegaBytes} MB";

			return result;
		}
	}

	[DisplayName(displayName: "Format")]
	public string? StreamContainerName { get; set; }

	[DisplayName(displayName: "Quality")]
	public int StreamVideoQualityMaxHeight { get; set; }

	[DisplayName(displayName: "Framerate")]
	public int StreamVideoQualityFramerate { get; set; }

	[DisplayName(displayName: "Width")]
	public int StreamVideoResolutionWidth { get; set; }

	[DisplayName(displayName: "Height")]
	public int StreamVideoResolutionHeight { get; set; }

	[DisplayName(displayName: "High Definition")]
	public bool StreamVideoQualityIsHighDefinition { get; set; }

	[DisplayName(displayName: "Views")]
	public long EngagementViewCount { get; set; }

	[DisplayName(displayName: "Likes")]
	public long EngagementLikeCount { get; set; }

	[DisplayName(displayName: "Dislikes")]
	public long EngagementDislikeCount { get; set; }

	[DisplayName(displayName: "Rating")]
	public double EngagementAverageRating { get; set; }



	[Browsable(browsable: false)]
	public string? Id { get; set; }

	[Browsable(browsable: false)]
	public string? Url { get; set; }

	[Browsable(browsable: false)]
	public string? Description { get; set; }

	[Browsable(browsable: false)]
	public string? AuthorChannelId { get; set; }

	[Browsable(browsable: false)]
	public string? AuthorChannelUrl { get; set; }

	[Browsable(browsable: false)]
	public string? StreamUrl { get; set; }

	[Browsable(browsable: false)]
	public string? StreamVideoCodec { get; set; }

	[Browsable(browsable: false)]
	public long StreamBitrateBitsPerSecond { get; set; }

	[Browsable(browsable: false)]
	public int StreamVideoResolutionArea { get; set; }

	[Browsable(browsable: false)]
	public string? StreamVideoQualityLabel { get; set; }

	public void Update(IVideoStreamInfo videoStreamInfo)
	{
		StreamUrl = videoStreamInfo.Url;
		StreamVideoCodec = videoStreamInfo.VideoCodec;

		StreamContainerName = videoStreamInfo.Container.Name.ToLower();

		StreamBitrateBitsPerSecond = videoStreamInfo.Bitrate.BitsPerSecond;

		StreamVideoResolutionArea = videoStreamInfo.VideoResolution.Area;
		StreamVideoResolutionWidth = videoStreamInfo.VideoResolution.Width;
		StreamVideoResolutionHeight = videoStreamInfo.VideoResolution.Height;

		StreamSizeMegaBytes = Math.Round(videoStreamInfo.Size.MegaBytes, 2);

		StreamVideoQualityLabel = videoStreamInfo.VideoQuality.Label;
		StreamVideoQualityFramerate = videoStreamInfo.VideoQuality.Framerate;
		StreamVideoQualityMaxHeight = videoStreamInfo.VideoQuality.MaxHeight;
		StreamVideoQualityIsHighDefinition = videoStreamInfo.VideoQuality.IsHighDefinition;
	}

	public string GetFileName()
	{
		var title = FixText(value: Title);
		var authorChannelTitle = FixText(value: AuthorChannelTitle);

		var uploadDate = UploadDate.ToString("yyyy_MM_dd");

		var result =
			$"{AuthorChannelId}_{uploadDate}_{authorChannelTitle}_{title}_{StreamVideoQualityMaxHeight}_{StreamVideoQualityFramerate}";

		return result;
	}

	public static string? FixText(string? value)
	{
		if (string.IsNullOrWhiteSpace(value: value))
		{
			return null;
		}

		value =
			value
			.Replace(oldValue: ":", newValue: " ")
			.Replace(oldValue: "/", newValue: " ")
			.Replace(oldValue: "\\", newValue: " ")
			;

		value = value.Trim();

		while (value.Contains("  "))
		{
			value =
				value
				.Replace(oldValue: "  ", newValue: " ");
		}

		return value;
	}
}
