using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using HelperBotForLvivProblem.Models;
using Microsoft.Bot.Connector;

namespace HelperBotForLvivProblem.Dialogs
{
    [Serializable]
    public class ProblemDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessegeRecivedAsync);
        }

        private async Task MessegeRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {

            string[] str = Enum.GetNames(typeof(TypeOrder));
            List<CardAction> cardButtons;

            var activity = await result as Activity;
            var reply = activity.CreateReply();
            reply.Attachments = new List<Attachment>();

            if (activity.Text.StartsWith("who"))
            {
                cardButtons = new List<CardAction>();
                string[] enstr = Enum.GetNames(typeof(TypeOrder));

                CardAction cd;

                for(int i = 0;i<enstr.Length;i++)
                {
                     cd = new CardAction
                    {
                        Title = enstr[i],
                        Value = enstr[i]
                    };
                    cardButtons.Add(cd);
                }

                HeroCard hc = new HeroCard()
                {
                    Buttons = cardButtons,
                    Title = "Choose ypur problem"
                };

                reply.Attachments.Add(hc.ToAttachment());
            }

            await context.PostAsync(reply);
            context.Wait(MessegeRecivedAsync);
        }
    }
}