﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;

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
                OnResultGenerated(args.Result.Text);
            }
        }

        private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
        {
            Start();
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
            SpeechRecognizer.ContinuousRecognitionSession.AutoStopSilenceTimeout = new TimeSpan(0, 0, 3);
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

        public async Task<IRandomAccessStream> SynthesizeTextToSpeechAsync(string text)
        {
            // Windows.Storage.Streams.IRandomAccessStream
            IRandomAccessStream stream = null;

            // Windows.Media.SpeechSynthesis.SpeechSynthesizer
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                // Windows.Media.SpeechSynthesis.SpeechSynthesisStream
                stream = await synthesizer.SynthesizeTextToStreamAsync(text);
            }

            return (stream);
        }
    }
}
