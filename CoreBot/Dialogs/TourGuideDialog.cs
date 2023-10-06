using AdaptiveCards.Templating;
using AdaptiveCards;
using CoreBot.CognitiveModels;
using CoreBot.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using CoreBot.Extensions;
using CoreBot.Model;

namespace CoreBot.Dialogs
{
    public class TourGuideDialog : CancelAndHelpDialog
    {
        private readonly IConfiguration _config;
        private const string JSON_CARD_TOUR_LIST = @"Cards\tourList.json";
        private const string JSON_CARD_TOUR_DETAIL = @"Cards\tourDetail.json";

        public TourGuideDialog(IConfiguration configuration) : base(nameof(TourGuideDialog))
        {
            _config = configuration;

            AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt)));
            AddDialog(new WaterfallDialog(nameof(BotContextCategoryAsync), new WaterfallStep[]
            {
                BotContextCategoryAsync
            }));

            AddDialog(new WaterfallDialog(nameof(QueryTour), new WaterfallStep[]
            {
                QueryTour,
                GetUserRequest,
                DisplayUserResult,
                FinalStep
            }));

            AddDialog(new WaterfallDialog(nameof(ListTours), new WaterfallStep[]
            {
                ListTours,
                ListTourDetail,
                FinalStep
            }));

            InitialDialogId = nameof(BotContextCategoryAsync);
        }

        private async Task<DialogTurnResult> BotContextCategoryAsync(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            await context.DebugAsync($"Text {context?.Context?.Activity?.Text}", cancellationToken);

            if (JsonUtil.IsValidJson(context.Options?.ToString()))
            {
                var jobj = JObject.Parse(context.Options?.ToString());
                var categoryStr = JsonUtil.GetJsonValueByKey(jobj, "tourCategory");
                var category = JsonConvert.DeserializeObject<TourCategory>(categoryStr);

                if (category == TourCategory.List)
                {
                    return await context.BeginDialogAsync(nameof(ListTours), null, cancellationToken);
                }
                else if (category == TourCategory.Query)
                {
                    return await context.BeginDialogAsync(nameof(QueryTour), null, cancellationToken);
                }
            }

            return await context.BeginDialogAsync(nameof(QueryTour));
        }

        private async Task<DialogTurnResult> QueryTour(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            await context.DebugAsync($"Text {context?.Context?.Activity?.Text}", cancellationToken);

            string msg = "Please enter your question";
            var promptMessage = MessageFactory.Text(msg, msg, InputHints.ExpectingInput);
            return await context.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> GetUserRequest(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(context.Result.ToString()))
            {
                return await context.ReplaceDialogAsync(nameof(QueryTour));
            }

            return await context.NextAsync(JsonConvert.SerializeObject(new { user = context.Result.ToString() }), cancellationToken);
        }

        private async Task<DialogTurnResult> DisplayUserResult(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            await context.DebugAsync($"Text {context?.Context?.Activity?.Text}", cancellationToken);

            if (context.Result != null)
            {
                //if (context.Result is FoundChoice)
                //{
                //    FoundChoice ch = (FoundChoice)context.Result;
                //    if (ch.Index == -1)
                //    {
                //        //context.Context.Activity.Code = "FlowCancel";
                //        //context.Context.Activity.Type = ActivityTypes.Message;
                //        //context.Context.Activity.Text = context.Context.Activity.Text;
                //        //return await context.ReplaceDialogAsync(nameof(NewDialog));
                //    }

                //    FoundChoice f = (FoundChoice)context.Result;

                //    int itemFound = Array.IndexOf(MENU_OPTIONS.ToArray(), f.Value);

                //}
                //else
                {
                    JObject obj = JObject.Parse(context.Result.ToString());
                    var user = JsonUtil.GetJsonValueByKey(obj, "user");

                    var attachment = await ShowTourDetailCard();
                    return await context.PromptAsync(nameof(AttachmentPrompt), new PromptOptions()
                    {
                        Prompt = (Activity)MessageFactory.Attachment(attachment)
                    }, cancellationToken);
                }
            }

            return await context.NextAsync(null, cancellationToken);
            //return await context.ReplaceDialogAsync(param);
        }

        private async Task<DialogTurnResult> ListTours(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            await context.DebugAsync($"Text {context?.Context?.Activity?.Text}", cancellationToken);

            var tourAvailable = context.Options as Tours;

            List<Attachment> tourList = new List<Attachment>();
            foreach (var item in tourAvailable.tour)
            {
                tourList.Add(await ShowTourDetailCard());
            }


            return await context.PromptAsync(nameof(AttachmentPrompt), new PromptOptions
            {
                Prompt = (Activity)MessageFactory.Carousel(tourList)
            }, cancellationToken);

        }

        private async Task<DialogTurnResult> ListTourDetail(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            await context.DebugAsync($"Text {context?.Context?.Activity?.Text}", cancellationToken);

            return await context.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStep(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            //return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);


            return await context.EndDialogAsync(null, cancellationToken);
        }

        private async Task<string> ConnectAPI()
        {
            return "";
        }

        private async Task<Attachment> ShowTourDetailCard()
        {
            AdaptiveCard acard = await CardUtil.GetAdaptiveCardFromPath(Assembly.GetExecutingAssembly(), JSON_CARD_TOUR_DETAIL);
            var template = new AdaptiveCardTemplate(acard);

            dynamic jobj = JObject.Parse("{}");

            jobj = JObject.Parse(@"{ name: 'TESTING MESSAGE' }");

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = JsonConvert.DeserializeObject(template.Expand(jobj))
            };
        }
    }
}
