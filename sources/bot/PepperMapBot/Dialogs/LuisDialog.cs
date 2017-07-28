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
using PepperMapBot.Services;

namespace PepperMapBot.Dialogs
{
    [LuisModel("38ff8c12-4a7d-4a54-a630-608175bb7957", "31118333649a4e85b5b72e6c13b8a99b")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        public RouteService Routes { get; private set; }

        public LuisDialog(RouteService routesService) : base()
        {
            this.Routes = routesService;
        }

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
            if (result.Entities == null || result.Entities.Count == 0)
            {
                message = "Je ne connais pas cette destination";
            }

            foreach (var entity in result.Entities)
            {
                message = $"Pour vous rendre en '{entity.Entity}', suivez la route '{this.Routes.GetRoutes(entity.Entity)}'";
            }

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }


        [LuisIntent("Hello")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            if (result.Entities != null && result.Entities.Count > 0)
            {
                var entity = result.Entities.FirstOrDefault();

                switch (entity.Entity.ToUpper())
                {
                    case "VISITEUR":
                        await context.PostAsync("Avez-vous un rendez-vous ? ou cherchez vous un de nos services ?");
                        context.Wait(this.MessageReceived);
                        break;
                    case "PATIENT":
                        await context.PostAsync("Est-ce que vous cherchez un de nos services ?");
                        context.Wait(this.MessageReceived);
                        break;
                    default:

                        break;
                }
            }
            else
            {
                string message = "Bonjour. Etes-vous êtes un patient ou un visiteur ?";
                await context.PostAsync(message);
                context.Wait(this.IdentityUserType);
            }
        }

        public async Task IdentityUserType(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            string text = message.Text;
            if(text.ToUpper().Contains("VISITEUR"))
                await context.PostAsync("Avez-vous un rendez-vous ? ou cherchez vous un de nos services ?");

            if (text.ToUpper().Contains("PATIENT"))
                await context.PostAsync("Est-ce que vous cherchez un de nos services ?");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Meeting")]
        public async Task Meeting(IDialogContext context, LuisResult result)
        {
            string message = "#MEETING#";

         
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }


    }
}