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
using PepperMap.Infrastructure.Models;

namespace PepperMapBot.Dialogs
{
    [LuisModel("38ff8c12-4a7d-4a54-a630-608175bb7957", "31118333649a4e85b5b72e6c13b8a99b")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        public IRouteService Routes { get; private set; }

        public Route[] PreSelectedRoutes { get; set; }

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
            PreSelectedRoutes = new Route[] { };

            if (result.Entities == null || result.Entities.Count == 0)
            {
                message = "Je ne connais pas cette destination";
            }

            foreach (var entity in result.Entities)
            {
                var routes = await this.Routes.GetPublicRoutesAsync(entity.Entity);
                
                if (routes.Count() > 1)
                {
                    PreSelectedRoutes = routes.ToArray();

                    message += "Nous avons trouvé plusieurs destination possible.\n\n";
                    message += "Veuillez préciser votre choix  parmi:\n\n\n\n";

                    foreach (var r in PreSelectedRoutes)
                    {
                        message += r.DestinationName + "\n\n";
                    }
                }
                else
                {
                    var firstRoute = routes.FirstOrDefault();
                    message = $"Pour vous rendre en '{firstRoute.DestinationName}', suivez la route '{firstRoute.RouteIndication}' - '{firstRoute.RouteNumber}";
                }
            }

            await context.PostAsync(message);

            if (PreSelectedRoutes.Length > 0)
                context.Wait(this.MultipleDestinationsFound);
            else
                context.Wait(this.MessageReceived);
        }

        private async Task MultipleDestinationsFound(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            string text = message.Text;

            var route = PreSelectedRoutes.FirstOrDefault(r => r.DestinationName.IsComparableTo(text));
            if (route == null)
                route = (await this.Routes.GetPublicRoutesAsync(text))?.FirstOrDefault();

            if (route != null)
                await context.PostAsync($"Pour vous rendre en '{route.DestinationName}', suivez la route '{route.RouteIndication}' - '{route.RouteNumber}");

            context.Wait(this.MessageReceived);
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
            if (result.Entities.Count > 0)
            {
                // Service defined in intent, don't ask user
                foreach (var entity in result.Entities)
                {
                    string message = string.Empty;
                    var routes = await this.Routes.GetPublicRoutesAsync(entity.Entity);
                    Route r = routes.ToArray()[0];
                    if (IsSpecialService(r.DestinationName))
                    {
                        message += $"Pour vous rendre en '{r.DestinationName}', suivez la route '{r.RouteIndication}' - '{r.RouteNumber}";
                        context.Done(new object { });
                    }
                    else
                    {
                        message = "Est-ce que vous vous êtes inscrits au guichet et payé votre consultation ?";
                        PreSelectedRoutes = null;
                        PreSelectedRoutes = new Route[] { r };
                        await context.PostAsync(message);
                        context.Wait(this.MeetingDetectedAskForSubscription);
                    }
                   
                }
            }
            else
            {
                // ask for service
                await context.PostAsync("Dans quel service ?");
                context.Wait(this.MeetingDetectedAskForService);
            }
        }

        #endregion

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

        private async Task MeetingDetectedAskForSubscription(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            string text = message.Text;
            if(text.Contains("oui"))
            {
                await context.PostAsync($"Pour vous rendre en '{this.PreSelectedRoutes[0].DestinationName}', suivez la route '{this.PreSelectedRoutes[0].RouteIndication}' - '{this.PreSelectedRoutes[0].RouteNumber}");
            }
            else
            {
                await context.PostAsync("Merci de vous diriger vers les guichets 1 à 8 pour payer votre consultation");
                await context.PostAsync("N'hésitez pas à revenir me voir pour trouver votre route");
            }
            context.Done(new object { });
        }


        private async Task MeetingDetectedAskForService(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            //var message = await item;
            //string text = message.Text;
            //if(text.ToUpper().Contains("NUCL") || text.ToUpper().Contains("PHYSIQUE") || text.ToUpper().Contains("IMAGERIE"))
            //{
            //    await context.PostAsync($"Pour vous rendre en '{entity.Entity}', suivez la route '{routes.FirstOrDefault()}'");
            //}
            await MessageReceived(context, item);
        }
    }
}