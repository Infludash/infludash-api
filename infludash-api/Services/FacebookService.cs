using infludash_api.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Services
{
    public class FacebookService
    {

        public string GetPagesByUserId(string userId, string userAccessToken)
        {
            return Helper.HttpGetRequest($"https://graph.facebook.com/{userId}/accounts?fields=name,access_token&access_token={userAccessToken}");
        }

        public string postToPage(string pageId, string message, string accessToken)
        {
            List<(string, string)> headers = new List<(string, string)>();
            headers.Add(("Content-Type", "application/json"));
            headers.Add(("Accept", "application/json"));
            var fbFeedPost = new
            {
                message = message,
                access_token = accessToken
            };
            return Helper.HttpPostRequest($"https://graph.facebook.com/{pageId}/feed", JObject.FromObject(fbFeedPost).ToString(), headers);
        }
    }
}
