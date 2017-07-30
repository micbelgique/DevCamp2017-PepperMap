using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PepperUWP.ViewModels;

namespace PepperUWP.Views
{
    public class ConversationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PepperTemplate { get; set; }
        public DataTemplate HippoTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var message = item as Message;

            if (message != null)
            {
                if (message.FromBot)
                    return HippoTemplate;
                else
                    return PepperTemplate;
            }
            return base.SelectTemplateCore(item);
        }
    }
}
