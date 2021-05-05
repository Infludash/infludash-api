using infludash_api.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace infludash_api.Services
{
    public class YoutubeService
    {
        private IConfiguration Configuration { get; }
        public void MyChannel(string accessToken)
        { 
            string apiKey = Configuration["Socials:youtubeDataAPIKey"];
            string url = $"https://youtube.googleapis.com/youtube/v3/channels?part=snippet%2CcontentDetails%2Cstatistics&mine=true&key={apiKey}";
            List<(string,string)> headers = new List<(string, string)>();
            headers.Add(("Authorization", $"Bearer: {accessToken}"));
            headers.Add(("Accept", "application/json"));
            Helper.HttpGetRequest(url);
        }
    }
}
