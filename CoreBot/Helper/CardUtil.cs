using AdaptiveCards;
using AdaptiveCards.Templating;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace CoreBot.Helper
{
    public class CardUtil
    {
        public static async Task<AdaptiveCard> GetAdaptiveCardFromPath(Assembly assembly, string path)
        {
            AdaptiveCard masterCard = null;

            var outPutDirectory = Path.GetDirectoryName(assembly.CodeBase);
            var filePath = Path.Combine(outPutDirectory, path);
            string LocalPath = new Uri(filePath).LocalPath;
            using (StreamReader reader = new StreamReader(LocalPath))
            {
                string adaptiveCard = await reader.ReadToEndAsync();
                masterCard = AdaptiveCard.FromJson(adaptiveCard).Card;
            }

            return masterCard;
        }
    }
}
