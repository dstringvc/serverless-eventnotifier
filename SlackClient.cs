using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace EventNotifier
{
	class SlackClient
	{
		private readonly Uri _uri;
		private readonly Encoding _encoding = new UTF8Encoding();

		public SlackClient(string urlWithAccessToken)
		{
			_uri = new Uri(urlWithAccessToken);
		}

		public void PostMessage(string text)
		{
			Payload payload = new Payload()
			{
				Text = text
			};

			PostMessage(payload);
		}

		public void PostMessage(string text, string icon_emoji = null)
		{
			Payload payload = new Payload()
			{
				Text = text,
				Icon_Emoji = icon_emoji
			};

			PostMessage(payload);
		}

		public void PostMessage(string text, string channel = null, string username = null, string icon_emoji = null)
		{
			Payload payload = new Payload()
			{
				Channel = channel,
				Username = username,
				Text = text,
				Icon_Emoji = icon_emoji
			};

			PostMessage(payload);
		}

		//Post a message using a Payload object
		public void PostMessage(Payload payload)
		{
			string payloadJson = JsonConvert.SerializeObject(payload);

			using (WebClient client = new WebClient())
			{
				NameValueCollection data = new NameValueCollection();
				data["payload"] = payloadJson;

				var response = client.UploadValues(_uri, "POST", data);

				//The response text is usually "ok"
				string responseText = _encoding.GetString(response);
			}
		}

	}

	//This class serializes into the Json payload required by Slack Incoming WebHooks
	public class Payload
	{
		[JsonProperty("channel")]
		public string Channel { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("icon_emoji")]
		public string Icon_Emoji { get; set; }
	}

}
