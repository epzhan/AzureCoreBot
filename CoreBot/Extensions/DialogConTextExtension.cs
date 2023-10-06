using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Extensions
{
    public static class DialogConTextExtension
    {
        public static async Task<ResourceResponse> DebugAsync(this DialogContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await context.Context.DebugAsync(MessageWithHeader(context), cancellationToken, label, filelabel);
        }
        public static async Task<ResourceResponse> InformationAsync(this DialogContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await context.Context.InformationAsync(MessageWithHeader(context), cancellationToken, label, filelabel);
        }
        public static async Task<ResourceResponse> WarningAsync(this DialogContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await context.Context.WarningAsync(MessageWithHeader(context), cancellationToken, label, filelabel);
        }
        public static async Task<ResourceResponse> ErrorAsync(this DialogContext context, string message, CancellationToken cancellationToken = default, [CallerMemberName] string label = null, [CallerFilePath] string filelabel = null)
        {
            return await context.Context.ErrorAsync(MessageWithHeader(context), cancellationToken, label, filelabel);
        }

        private static string MessageWithHeader(DialogContext context)
        {
            var header = $"";
            return header;
        }
    }
}
