using System.Net;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Rest;

namespace PepperUWP.Services
{
    public class DirectLineService : IDirectLineService
    {
        private HttpOperationResponse<Conversation> _conversation;
        private string _conversationId;
        private string _watermark;

        public IDirectLineClient DirectClient { get; private set; }
        public void Connect(string connectionSecret)
        {
            var client = new DirectLineClient(connectionSecret);
            DirectClient = client;
        }

        public async Task StartConversation()
        {
            _conversation = await DirectClient.Conversations.StartConversationWithHttpMessagesAsync();
            _conversationId = _conversation.Body.ConversationId;
        }

        public async Task<Conversation> RenewConversation()
        {
            var newConversation = await DirectClient.Tokens.RefreshTokenWithHttpMessagesAsync();
            return newConversation?.Body;
        }

        public async Task<bool> SendMessage(Activity message)
        {
            var sendResponse =
                await DirectClient.Conversations.PostActivityWithHttpMessagesAsync(
                    _conversationId, message);
            return sendResponse?.Response?.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<ActivitySet> LoadMessages()
        {
            var sendResponse =
                await DirectClient.Conversations.GetActivitiesWithHttpMessagesAsync(
                    _conversationId, _watermark);
            var activities = sendResponse?.Body;

            _watermark = activities?.Watermark;

            return activities;
        }
    }
}
