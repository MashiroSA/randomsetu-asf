using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ArchiSteamFarm;
using Newtonsoft.Json;

namespace ArchiSteamFarm.CustomPlugins.Setu
{
	internal static class SetuAPI
	{
		private const string URL = "https://el-bot-api.vercel.app/api";

		internal static async Task<string?> GetRandomSetuURL(WebBrowser webBrowser)
		{
			if (webBrowser == null)
			{
				throw new ArgumentNullException(nameof(webBrowser));
			}

			const string request = URL + "/setu";

			WebBrowser.ObjectResponse<SetuResponse>? response = await webBrowser.UrlGetToJsonObject<SetuResponse>(request).ConfigureAwait(false);

			if (response == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(response.Content.Link))
			{
				throw new InvalidOperationException(nameof(response.Content.Link));
			}

			return response.Content.Link;
		}

		[SuppressMessage("ReSharper", "ClassCannotBeInstantiated")]
		private sealed class SetuResponse
		{
			[JsonProperty(PropertyName = "pid", Required = Required.Always)]
			internal readonly string Pid = "";
			
			[JsonProperty(PropertyName = "p", Required = Required.Always)]
			internal readonly int P;
			
			[JsonProperty(PropertyName = "uid", Required = Required.Always)]
			internal readonly int Uid;
			
			[JsonProperty(PropertyName = "title", Required = Required.Always)]
			internal readonly string Title = "";
			
			[JsonProperty(PropertyName = "author", Required = Required.Always)]
			internal readonly string Author = "";
			
			[JsonProperty(PropertyName = "url", Required = Required.Always)]
			internal readonly string Link = "";
			
			[JsonProperty(PropertyName = "r18", Required = Required.Always)]
			internal readonly bool R18;
			
			[JsonProperty(PropertyName = "width", Required = Required.Always)]
			internal readonly int Width = 0;
			
			[JsonProperty(PropertyName = "height", Required = Required.Always)]
			internal readonly int Height = 0;
			
			[JsonProperty(PropertyName = "tags", Required = Required.Always)]
			internal readonly string[] Tags = null;

			[JsonConstructor]
			private SetuResponse() { }
		}
	}
}
