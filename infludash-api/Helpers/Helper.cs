using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace infludash_api.Helpers
{
	/*
	 * General helper functions
	 */
    public class Helper
    {
		public static string HttpGetRequest(string url)
		{
			string res = string.Empty;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AutomaticDecompression = DecompressionMethods.GZip;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				res = reader.ReadToEnd();
			}

			return res;
		}

		/* Piece of code based on 
		 * http://findnerd.com/list/view/Converting-a-given-string-to-SHA1-hash-using-c/17922/
		 */
		public static string GetSha1(string value)
		{
			var data = Encoding.ASCII.GetBytes(value);
			var hashData = new SHA1Managed().ComputeHash(data);
			var hash = string.Empty;
			foreach (var b in hashData)
			{
				hash += b.ToString("X2");
			}
			return hash;
		}
	}
}
