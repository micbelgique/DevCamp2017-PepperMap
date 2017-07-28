using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
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
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                TextSpeech = e;
            });
        }
    }
}
