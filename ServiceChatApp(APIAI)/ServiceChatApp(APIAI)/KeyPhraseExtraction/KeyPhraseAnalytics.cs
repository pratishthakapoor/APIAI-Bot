using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceChatApp_APIAI_
{
    public class KeyPhraseAnalytics
    {
        private static string TEXT_ANALYTICS_KEY = Constants.TEXT_ANALYTICS_ID;
        private static KeyPhraseClient _client = new KeyPhraseClient(TEXT_ANALYTICS_KEY);

        public static async Task<List<string>> ExtractPhraseAsync(string sentence)
        {
            var request = new KeyPhrasesRequest();
            request.Documents.Add(new KeyPhraseDocument()
            {
                Id = Guid.NewGuid().ToString(),
                Text = sentence,
                Language = "en"
            });

            var result = await _client.GetKeyPhrasesAsync(request);

            return result.Documents.First().KeyPhrases;
        }
    }
}