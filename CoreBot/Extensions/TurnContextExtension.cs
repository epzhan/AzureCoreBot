using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using NuGet.Common;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Extensions
{
    public static class TurnContextExtension
    {
        public static async Task<ResourceResponse> DebugAsync(this ITurnContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await LogAsync(context, LogLevel.Debug, message, cancellationToken, label, filelabel).ConfigureAwait(false);
        }
        public static async Task<ResourceResponse> InformationAsync(this ITurnContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await LogAsync(context, LogLevel.Information, message, cancellationToken, label, filelabel).ConfigureAwait(false);
        }
        public static async Task<ResourceResponse> WarningAsync(this ITurnContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await LogAsync(context, LogLevel.Warning, message, cancellationToken, label, filelabel).ConfigureAwait(false);
        }
        public static async Task<ResourceResponse> ErrorAsync(this ITurnContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await LogAsync(context, LogLevel.Error, message, cancellationToken, label, filelabel).ConfigureAwait(false);
        }

        private static async Task<ResourceResponse> LogAsync(this ITurnContext context, LogLevel logLevel, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await context.SendActivityAsync(MessageFactory.Text(message), cancellationToken).ConfigureAwait(false);
        }
    }
}
