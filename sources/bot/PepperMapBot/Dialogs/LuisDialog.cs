using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;


namespace PepperMapBot.Dialogs
{
    [LuisModel("38ff8c12-4a7d-4a54-a630-608175bb7957", "31118333649a4e85b5b72e6c13b8a99b")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Désolé je ne comprends pas '{result.Query}'.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("GoTo")]
        public async Task Goto(IDialogContext context, LuisResult result)
        {
            string message = string.Empty;
            if (result.Entities == null)
            {
                message = "Je ne connais pas cette destination";
            }

            foreach (var entity in result.Entities)
            {
                message = $"Pour vous rendre en '{entity.Entity}', suivez la route 1";
            }

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }
    }
}