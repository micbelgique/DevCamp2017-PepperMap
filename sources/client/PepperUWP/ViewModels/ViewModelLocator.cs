using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PepperUWP.Services;

namespace PepperUWP.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<SpeechService>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}
