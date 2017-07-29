using System.Collections.Generic;
using Windows.Storage.Streams;
using PepperUWP.ViewModels;

namespace PepperUWP.Messages
{
    public class StreamToPlayMessage
    {
        public IRandomAccessStream Stream { get; set; }
    }
}
