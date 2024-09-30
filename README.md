# DT YouTube Downloader

DT YouTube Downloader is a YouTube downloader application implemented in C# using Windows Forms. It allows you to download YouTube videos and captions using the `YoutubeExplode` and `YoutubeExplode.Converter` packages.

## Setup

To set up the project, follow these steps:

1. Clone the repository:
   ```sh
   git clone https://github.com/Dariush-Tasdighi/DT-YouTube-Downloader.git
   ```

2. Open the solution in Visual Studio:
   ```sh
   cd DT-YouTube-Downloader
   start MyApplication.sln
   ```

3. Restore NuGet packages:
   ```sh
   dotnet restore
   ```

## Prerequisites

Before running the project, ensure you have the following software installed:

- .NET 8.0 SDK
- FFMpeg

## Usage

To use the application, follow these steps:

1. Specify the target path where the downloaded videos and captions will be saved.
2. Enter the YouTube video ID or URL in the provided text box.
3. Specify the path to the FFMpeg executable.
4. Click the "Fix and Check" button to validate the input and check if the video or caption has already been downloaded.
5. Click the "Detect" button to detect available video streams for the specified YouTube video.
6. Select the desired video stream from the list.
7. Click the "Download" button to start downloading the selected video and/or caption.

## Main Features

- Download YouTube videos in various formats and resolutions.
- Download captions for YouTube videos.
- Log information and errors during the download process.
