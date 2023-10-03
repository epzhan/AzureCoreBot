using CoreBot.CognitiveModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
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

            return await context.NextAsync(JsonConvert.SerializeObject(new { user = "value param" }));
        }

        private async Task<DialogTurnResult> DisplayUserResult(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            _logger.LogDebug("DisplayUserResult", context);

            var param = context.Result.ToString();


            return await context.EndDialogAsync();
            //return await context.ReplaceDialogAsync(param);
        }
    }
}
