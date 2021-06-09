using infludash_api.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace infludash_api.Services
{
    public class YoutubeService
    {
        private IConfiguration Configuration { get; }
        private string apiKey { get; }
        public YoutubeService()
        {
            Configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();
            this.apiKey = Configuration["Socials:youtubeDataAPIKey"];
        }

        public string MyChannel(string accessToken)
        {  
            string url = $"https://youtube.googleapis.com/youtube/v3/channels?part=snippet%2CcontentDetails%2CbrandingSettings&mine=true&key={this.apiKey}";
            List<(string, string)> headers = new List<(string, string)>();
            headers.Add(("Authorization", $"Bearer {accessToken}"));
            headers.Add(("Accept", "application/json"));
            return Helper.HttpGetRequest(url, headers);
        }

        public string UploadVideo(string accessToken, dynamic videoFile, string locationUrl)
        {
            List<(string, string)> headers2 = new List<(string, string)>();
            headers2.Add(("Authorization", $"Bearer {accessToken}"));
            headers2.Add(("Content-Type", "video/*"));

            try
            {
                return Helper.HttpPostRequest(locationUrl, videoFile, headers2);
            }
            catch (Exception)
            {
                return "Something went wrong, please try again";
            }
            
        }

        public string PrepareUploadVideo(string accessToken, string videoData)
        {
            Debug.WriteLine($"{accessToken}, {videoData}");
            string url = $"https://www.googleapis.com/upload/youtube/v3/videos?uploadType=resumable&part=snippet,status,contentDetails";
            List<(string, string)> headers = new List<(string, string)>();
            headers.Add(("Authorization", $"Bearer {accessToken}"));
            headers.Add(("Accept", "application/json"));
            headers.Add(("Content-Type", "application/json"));
            Debug.WriteLine(headers);
            return this.GetLocationHeaderResponsePostRequest(url, videoData, headers);
        }

        public string GetVideoCategories(string regionCode)
        {
            string url = $"https://youtube.googleapis.com/youtube/v3/videoCategories?regionCode={regionCode}&key={this.apiKey}";
            List<(string, string)> headers = new List<(string, string)>();
            headers.Add(("Accept", "application/json"));
            return Helper.HttpGetRequest(url, headers);
        }

        private string GetLocationHeaderResponsePostRequest(string url, string jsonPayload, List<(string, string)> headers)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Method = "POST";

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Set(header.Item1, header.Item2);
                }
            }
            /*
			 Piece of code based on: https://stackoverflow.com/questions/4088625/net-simplest-way-to-send-post-with-data-and-read-response
			 */
            // We need to count how many bytes we're sending. 
            // Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(jsonPayload);
            request.ContentLength = bytes.Length;
            System.IO.Stream os = request.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); // Push it out there
            os.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string locationUrl = response.Headers["Location"];
                        Debug.WriteLine(locationUrl);
                        return locationUrl;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
            }
            catch (Exception)
            {
                return String.Empty;
                throw;
            }
            
        }
    }
}
