using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace NNMFSearchResultClustering
{
	/// <summary>
	/// Simplifies downloading HTTP resources asynchronously
	/// </summary>
	class AsyncDownload
	{
		public delegate void CallbackDelegate(string xml);

		class ReadContext
		{
			public StringBuilder sb = new StringBuilder();
			public const int BUFFER_SIZE = 1024;
			public byte[] buffer = new byte[BUFFER_SIZE];
			public Stream responseStream = null;
			public CallbackDelegate callback = null;

			public ReadContext(CallbackDelegate cb)
			{
				callback = cb;
			}
		}

		public static void DownloadUrl(string url, CallbackDelegate callback)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.UserAgent = "NNMFSearchResultClustering";

			ReadContext readContext = new ReadContext(callback);
			request.BeginGetResponse(delegate(IAsyncResult ar) {
				HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
				readContext.responseStream = response.GetResponseStream();
				readContext.responseStream.BeginRead(readContext.buffer, 0, ReadContext.BUFFER_SIZE, new AsyncCallback(_ReadCompletedCallback), readContext);
			}, null);
		}

		private static void _ReadCompletedCallback(IAsyncResult ar)
		{
			ReadContext readContext = (ReadContext)ar.AsyncState;
			int readCount = readContext.responseStream.EndRead(ar);
			if(readCount > 0) {
				readContext.sb.Append(Encoding.UTF8.GetString(readContext.buffer, 0, readCount));
				readContext.responseStream.BeginRead(readContext.buffer, 0, ReadContext.BUFFER_SIZE, new AsyncCallback(_ReadCompletedCallback), readContext);
			} else {
				string str = readContext.sb.ToString();
				readContext.callback(str);
			}
		}
	}
}
