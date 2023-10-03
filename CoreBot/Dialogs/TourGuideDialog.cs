using CoreBot.CognitiveModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class TourGuideDialog : CancelAndHelpDialog
    {
        private readonly ILogger _logger;
        private List<string> MENU_OPTIONS;

        public TourGuideDialog(ILogger<TourGuideDialog> logger) : base(nameof(TourGuideDialog))
        {
            _logger = logger;

            AddDialog(new WaterfallDialog(nameof(BotContextCategoryAsync), new WaterfallStep[]
            {
                BotContextCategoryAsync
            }));

            AddDialog(new WaterfallDialog(nameof(SearchUserRequest), new WaterfallStep[]
            {
                SearchUserRequest,
                DisplayUserResult
            }));


            InitialDialogId = nameof(BotContextCategoryAsync);
        }

        private async Task<DialogTurnResult> BotContextCategoryAsync(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            _logger.LogDebug("CategoryMappingAsync", context);

            string menuItemText = context.Context?.Activity?.Text;

            return await context.BeginDialogAsync(nameof(WaterfallDialog));
        }

        private async Task<DialogTurnResult> SearchUserRequest(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            _logger.LogDebug("SearchUserRequest", context);

            MENU_OPTIONS = new List<string>();
            MENU_OPTIONS.Add("1");
            MENU_OPTIONS.Add("2");

            return await context.NextAsync(JsonConvert.SerializeObject(new { user = "value param" }));
        }

        private async Task<DialogTurnResult> DisplayUserResult(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            _logger.LogDebug("DisplayUserResult", context);

            var param = context.Result.ToString();


            FoundChoice ch = (FoundChoice)context.Result;
            if (ch.Index == -1)
            {
                //context.Context.Activity.Code = "FlowCancel";
                //context.Context.Activity.Type = ActivityTypes.Message;
                //context.Context.Activity.Text = context.Context.Activity.Text;
                //return await context.ReplaceDialogAsync(nameof(NewDialog));
            }

            FoundChoice f = (FoundChoice)context.Result;

            int itemFound = Array.IndexOf(MENU_OPTIONS.ToArray(), f.Value);


            return await context.EndDialogAsync();
            //return await context.ReplaceDialogAsync(param);
        }
    }
}
