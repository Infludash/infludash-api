using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace infludash_api.Helpers
{
	/*
	 * General helper functions
	 */
    public class Helper
    {
		public static string HttpGetRequest(string url, List<(string, string)> headers=null)
		{
			string res = string.Empty;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AutomaticDecompression = DecompressionMethods.GZip;
			request.Method = "GET";

			if (headers != null)
            {
                foreach (var header in headers)
                {
					request.Headers.Set(header.Item1, header.Item2);
                }
            }

            try
            {
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					res = reader.ReadToEnd();
				}

				return res;
			}
            catch (Exception ex)
            {
				return ex.Message;
                throw;
            }
		}
		public static string HttpPostRequest(string url, dynamic data, List<(string, string)> headers=null)
		{
			string res = string.Empty;

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
			byte[] bytes;
            if (data is String)
            {
				// body is string
				bytes = System.Text.Encoding.ASCII.GetBytes(data);
            }
            else
            {
				// body is file
				bytes = data;
            }
			request.ContentLength = bytes.Length;
			System.IO.Stream os = request.GetRequestStream();
			os.Write(bytes, 0, bytes.Length); // Push it out there
			os.Close();

            try
            {
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					res = reader.ReadToEnd();
				}

				return res;
			}
            catch (Exception ex)
            {
				return ex.Message;
                throw;
            }
		}
	}
}
