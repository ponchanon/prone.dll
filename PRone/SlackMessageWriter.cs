using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PRone
{
    public class SlackMessageWriter
    {
        private readonly string _url;
        private readonly HttpClient _client;

        public SlackMessageWriter(string token)
        {
            _url = "https://slack.com/api/chat.postMessage";
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task WriteMessage(string channelName,string messageBody)
        {
            var postObject = new { channel = channelName, text = messageBody, as_user = true };
            var json = JsonConvert.SerializeObject(postObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync(_url, content);
        }
    }
}
