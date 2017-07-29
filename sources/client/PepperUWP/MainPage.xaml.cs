using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using PepperUWP.Messages;
using PepperUWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PepperUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Messenger.Default.Register<StreamToPlayMessage>(this, OnMessageReceived);
        }

        private async void OnMessageReceived(StreamToPlayMessage obj)
        {
            await MediaElement.PlayStreamAsync(obj.Stream);
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.Load();
        }
    }
}
