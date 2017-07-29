using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Bot.Connector.DirectLine;
using PepperUWP.Services;

namespace PepperUWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private SpeechService _speechService;
        private DirectLineService _directLineService;

        public ObservableCollection<string> Conversations { get; set; }

        public MainViewModel()
        {
            Conversations = new ObservableCollection<string>();
        }

        public async void Load()
        {
            _speechService = SimpleIoc.Default.GetInstance<SpeechService>();
            _speechService.ResultGenerated += SpeechService_ResultGenerated;
            _speechService.Init();

            _directLineService = new DirectLineService();
            // Add a Secrets class with constants (ignored from the git repo)
            _directLineService.Connect(Secrets.BotSecret);
            await _directLineService.StartConversation();
        }

        private async void SpeechService_ResultGenerated(object sender, string sentence)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Conversations.Add(sentence);
                });

            await _directLineService.SendMessage(new Activity()
            {
                Type = ActivityTypes.Message,
                Text = sentence,
                From = new ChannelAccount()
                {
                    Id = "Pepper"
                }
            });
            var activitySet = await _directLineService.LoadMessages();
            var activities = activitySet.Activities;

            for (var i = 0; i < 2; i++)
            {
                Conversations.Add(activities[i].Text);
            }
        }
    }
}
