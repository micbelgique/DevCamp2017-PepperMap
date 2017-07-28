using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using PepperUWP.Messages;

namespace PepperUWP.Services
{
    public class SpeechService
    {
        public SpeechRecognizer SpeechRecognizer { get; set; }

        private StringBuilder _dictatedTextBuilder;

        public event EventHandler<string> ResultGenerated;

        public SpeechService()
        {
        }

        private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {

            if (args.Result.Confidence == SpeechRecognitionConfidence.Medium ||
                args.Result.Confidence == SpeechRecognitionConfidence.High)
            {
                Debug.Write($"Listen : {args.Result.Text}");
                _dictatedTextBuilder.Append(args.Result.Text + " ");
                OnResultGenerated(_dictatedTextBuilder.ToString());
            }
        }

        private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            if (args.Status != SpeechRecognitionResultStatus.Success)
            {
                if (args.Status == SpeechRecognitionResultStatus.TimeoutExceeded)
                {
                    var sentence = _dictatedTextBuilder.ToString();

                    if (!string.IsNullOrWhiteSpace(sentence))
                    {
                        OnResultGenerated(sentence);
                    }
                }
            }
            else
            {
                Start();
            }
        }

        public async Task Init()
        {
            if (SpeechRecognizer != null)
            {
                await SpeechRecognizer.ContinuousRecognitionSession.CancelAsync();
            }

            var language = new Language("fr-FR");
            SpeechRecognizer = new SpeechRecognizer(language);

            SpeechRecognizer.StateChanged += SpeechRecognizer_StateChanged;
            SpeechRecognizer.ContinuousRecognitionSession.AutoStopSilenceTimeout = new TimeSpan(0, 0, 5);
            SpeechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
            SpeechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;

            SpeechRecognitionCompilationResult result =
                await SpeechRecognizer.CompileConstraintsAsync();

            if (SpeechRecognizer.State == SpeechRecognizerState.Idle)
            {
                await Start();
            }
        }

        private async void SpeechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            Debug.WriteLine($"State : {args.State}");
            if (args.State == SpeechRecognizerState.Idle)
            {
                await Start();
            }
        }

        public async Task Start()
        {
            _dictatedTextBuilder = new StringBuilder();
            await SpeechRecognizer.ContinuousRecognitionSession.StartAsync(SpeechContinuousRecognitionMode.Default);

            Debug.WriteLine($"Call : Start()");
        }

        protected virtual void OnResultGenerated(string e)
        {
            ResultGenerated?.Invoke(this, e);
        }
    }
}
