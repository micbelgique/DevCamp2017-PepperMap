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
using System.Threading;
using PepperMap.Infrastructure.Interfaces;

namespace PepperMapBot.Dialogs
{
    [LuisModel("38ff8c12-4a7d-4a54-a630-608175bb7957", "31118333649a4e85b5b72e6c13b8a99b")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        public IRouteService Routes { get; private set; }

        public LuisDialog(IRouteService routesService) : base()
        {
            this.Routes = routesService;
        }

        #region MAIN_INTENTS

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Désolé je ne comprends pas '{result.Query}'.");
            await context.PostAsync("Avez-vous un rendez-vous ou est-ce que vous cherchez un service ?");
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
                var routes = await this.Routes.GetPublicRoutesAsync(entity.Entity);
                message = $"Pour vous rendre en '{entity.Entity}', suivez la route '{routes.FirstOrDefault()}'";
            }

            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Hello")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Bonjour. Je m'appelle Hypolite, à votre service ! Avez-vous un rendez-vous ? ou cherchez vous un de nos services ?");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("SpecificDestination")]
        public async Task SpecificDestination(IDialogContext context, LuisResult result)
        {
            if (result.Entities == null || result.Entities.Count == 0)
            {
                await context.PostAsync("Je n'ai pas bien compris, quel service cherchez-vous ?");
                context.Wait(this.MessageReceived);

            }
            foreach (var entity in result.Entities)
            {
                var routes = await this.Routes.GetPublicRoutesAsync(entity.Entity);
                await context.PostAsync($"Pour vous rendre en '{entity.Entity}', suivez la route '{routes.FirstOrDefault()}'");
            }
        }

        [LuisIntent("Meeting")]
        public async Task Meeting(IDialogContext context, LuisResult result)
        {
            if(result.Entities.Count > 0)
            {
                // Service defined in intent, don't ask user
                foreach (var entity in result.Entities)
                {
                    var routes = await this.Routes.GetPublicRoutesAsync(entity.Entity);
                    await context.PostAsync($"Pour vous rendre en '{entity.Entity}', suivez la route '{routes.FirstOrDefault()}'");
                }
                context.Done(new object { });
            }
            else
            {
                // ask for service
                await context.PostAsync("Dans quel service ?");
                context.Wait(this.MeetingDetectedAskForService);
            }
        }

        #endregion
        private async Task MeetingDetectedAskForService(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await MessageReceived(context, item);
        }

        //protected override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        //{
        //    return base.MessageReceived(context, item);
        //}

        //private async Task AfterHelloVisitOrMeeting(IDialogContext context, IAwaitable<IMessageActivity> item)
        //{
        //    var message = await item;
        //    string text = message.Text;
        //    if (text.ToUpper().Contains("RENDEZ") || text.ToUpper().Contains("RDV"))
        //    {
        //        context.Wait(this.MeetingDetectedAskForService);
        //    }
        //    else
        //    {
        //        if (text.ToUpper().Contains("CHERCHE"))
        //        {
        //            await context.PostAsync("Est-ce que vous cherchez un de nos services ?");
        //            context.Wait(this.AfterIdentityUserTypePatientSimpleAnswer);
        //        }
        //    }
        //}

        //public async Task AfterIdentityUserTypePatientSimpleAnswer(IDialogContext context, IAwaitable<IMessageActivity> item)
        //{
        //    var message = await item;
        //    string text = message.Text;
        //    if (text.ToUpper().Contains("OUI"))
        //        await context.PostAsync("Quel service cherchez vous ?");

        //    if (text.ToUpper().Contains("NON"))
        //        await context.PostAsync("Comment puis-je vous aider ?");

        //    context.Done(new object { });
        //}

    }
}