using Newtonsoft.Json;

namespace ServiceChatApp_APIAI_
{
    public class KeyPhraseDocument : Document, IDocument
    {
        /// <summary>
        /// Gets or sets the language the text is in
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [JsonProperty("language")]
        public string Language
        {
            get;
            set;
        }
    }
}