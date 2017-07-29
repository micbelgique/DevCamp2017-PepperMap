﻿using System;
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
using PepperMap.Infrastructure.Models;

namespace PepperMapBot.Dialogs
{
    [LuisModel("38ff8c12-4a7d-4a54-a630-608175bb7957", "31118333649a4e85b5b72e6c13b8a99b")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        public IRouteService Routes { get; private set; }

        public string[] PreSelectedRoutes { get; set; }

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
                if (routes.Count() > 1)
                {
                    await context.PostAsync("Merci de préciser votre destination parmi : ");
                    foreach (Route r in routes)
                    {
                        message += r.DestinationName;
                    }

                    context.Wait(MultipleDestinationsFound);
                }
                else
                {
                    message = $"Pour vous rendre en '{routes.ToArray()[0].DestinationName}', suivez la route '{routes.ToArray()[0].RouteIndication}' - '{routes.ToArray()[0].RouteNumber}";
                }
            }

            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        private async Task MultipleDestinationsFound(IDialogContext context, IAwaitable<IMessageActivity> item)
        {

        }

        [LuisIntent("Hello")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Bonjour. Je m'appelle Hypolite, à votre service ! Avez-vous un rendez-vous ? Cherchez vous un de nos services ou un patient ?");
            context.Wait(this.MessageReceived);
        }

       [LuisIntent("Meeting")]
        public async Task Meeting(IDialogContext context, LuisResult result)
        {
            if(result.Entities.Count > 0)
            {
                // Service defined in intent, don't ask user
                foreach (var entity in result.Entities)
                {
                    string message = string.Empty;
                    var routes = await this.Routes.GetPublicRoutesAsync(entity.Entity);
                    foreach (Route r in routes)
                    {
                        message += $"Pour vous rendre en '{r.DestinationName}', suivez la route '{r.RouteIndication}' - '{r.RouteNumber}";
                    }
                    await context.PostAsync(message);
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

        /// <summary>
        /// Special service that don't require meeting
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private bool IsSpecialService(string serviceName)
        {
            if (serviceName.ToUpper().Contains("NUCL") || serviceName.ToUpper().Contains("PHYSIQUE") || serviceName.ToUpper().Contains("IMAGERIE"))
                return true;
            else
                return false;
        }

        #endregion
        private async Task MeetingDetectedAskForService(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            string text = message.Text;
            if(IsSpecialService(text))
            {
                await context.PostAsync($"servicespécial");
            }
            await MessageReceived(context, item);
        }
    }
}