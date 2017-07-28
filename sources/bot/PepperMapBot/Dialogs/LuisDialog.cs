using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using PepperMap.Infrastructure.Interfaces;

namespace PepperMapBot.Dialogs
{
    [LuisModel("38ff8c12-4a7d-4a54-a630-608175bb7957", "31118333649a4e85b5b72e6c13b8a99b")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        private readonly IRouteService _routeService;

        public LuisDialog(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Désolé je ne comprends pas '{result.Query}'.";

            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }

        [LuisIntent("GoTo")]
        public async Task Goto(IDialogContext context, LuisResult result)
        {
            var message = string.Empty;
            if (result.Entities == null || result.Entities.Count == 0)
            {
                message = "Je ne connais pas cette destination";
            }
            else
            {
                foreach (var entity in result.Entities)
                {
                    var routeResult = await _routeService.GetRoutesAsync(entity.Entity);
                    var route = routeResult.FirstOrDefault();
                    message = route != null 
                        ? $"Pour vous rendre en '{route.DestinationName}', suivez la route '{route}'" 
                        : "Je ne connais pas cette destination";
                }
            }

            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }


        [LuisIntent("Hello")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            string message = string.Empty;
            if (result.Entities == null || result.Entities.Count == 0)
            {
                message = "Bonjour. Etes-vous êtes un patient ou un visiteur ?";
            }

            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }
    }
}