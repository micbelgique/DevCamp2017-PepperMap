using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;

namespace PepperUWP.Services
{
    public interface IDirectLineService
    {
        void Connect(string connectionSecret);
        Task StartConversation();
        Task<Conversation> RenewConversation();
        Task<ActivitySet> LoadMessages(string watermark = null);
        Task<bool> SendMessage(Activity message);
    }
}