using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Bot.Connector.DirectLine;
using PepperUWP.Messages;
using PepperUWP.Services;

namespace PepperUWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private SpeechService _speechService;
        private DirectLineService _directLineService;

        public ObservableCollection<Message> Conversations { get; set; }

        public MainViewModel()
        {
            Conversations = new ObservableCollection<Message>();
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

        private void SpeechService_ResultGenerated(object sender, string sentence)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
                {
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
                    var activities = activitySet?.Activities;

                    var messageToSay = new List<Message>();

                    if (activities != null)
                    {
                        foreach (var activity in activities)
                        {

                            var message = new Message()
                            {
                                Text = activity.Text,
                                FromBot = activity.From.Id != "Pepper"
                            };

                            if (message.FromBot)
                            {
                                messageToSay.Add(message);
                            }

                            Conversations.Add(message);
                        }

                        if (messageToSay.Count > 0)
                        {
                            StringBuilder str = new StringBuilder();
                            foreach (var message in messageToSay)
                            {
                                str.AppendLine(message.Text);
                            }

                            var stream = await _speechService.SynthesizeTextToSpeechAsync(str.ToString());


                            MessengerInstance.Send(new StreamToPlayMessage() { Stream = stream });
                        }
                    }
                });
        }
    }

    public class Message
    {
        public string Text { get; set; }

        public bool FromBot { get; set; }
    }
}
