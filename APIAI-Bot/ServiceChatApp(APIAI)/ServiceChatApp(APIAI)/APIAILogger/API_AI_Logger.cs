using ApiAiSDK;
using System;

namespace ServiceChatApp_APIAI_
{
    internal class API_AI_Logger
    {
        internal static string API_Connection_Action(string text)
        {
            ApiAi api;

            var AIconfiguration = new AIConfiguration(Constants.APIAI_SUBSCRIPTION_ID, SupportedLanguage.English);

            api = new ApiAi(config: AIconfiguration);
            var response = api.TextRequest(text);
            return response.Result.Action.ToString();
        }

        internal static string API_Response(string text)
        {
            ApiAi apiAi;

            var config = new AIConfiguration(Constants.APIAI_SUBSCRIPTION_ID, SupportedLanguage.English);

            apiAi = new ApiAi(config);

            var resposne = apiAi.TextRequest(text);
            return resposne.Result.Fulfillment.Speech;
        }
    }
}