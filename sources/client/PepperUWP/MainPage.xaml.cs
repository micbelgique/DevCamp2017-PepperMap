using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            ListView.ScrollIntoView(ListView.Items.Last());
            await MediaElement.PlayStreamAsync(obj.Stream);

        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.Load();
        }
    }
}
