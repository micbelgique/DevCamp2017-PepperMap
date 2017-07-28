using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using PepperUWP.Services;

namespace PepperUWP.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _textSpeech;

        public string TextSpeech
        {
            get { return _textSpeech; }
            set
            {
                _textSpeech = value;
                RaisePropertyChanged();
            }
        }

        public void Load()
        {
            var speechService = SimpleIoc.Default.GetInstance<SpeechService>();
            speechService.Init();
            speechService.ResultGenerated += SpeechService_ResultGenerated;
        }

        private void SpeechService_ResultGenerated(object sender, string e)
        {
            TextSpeech = e;
        }
    }
}
