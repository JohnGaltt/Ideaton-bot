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
        private string testgetvalue;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessegeRecivedAsync);
        }

        private async Task MessegeRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            List<CardAction> cardButtons;

            var activity = await result as Activity;
            var reply = activity.CreateReply();
            reply.Attachments = new List<Attachment>();

            //when you write "show cards..." this function work ...
            if (activity.Text.StartsWith("show cards"))
            {
                cardButtons = new List<CardAction>();
                string[] enstr = Enum.GetNames(typeof(TypeOrder));

                //Creation of acction also its buttons)))
                CardAction cd;

                for(int i = 0; i < enstr.Length ; i++)
                {
                    cd = new CardAction
                    {
                        Title = enstr[i],
                        Value = enstr[i]
                    };

                    //Get Data from value property)))
                    //testgetvalue = (string)cd.Value;

                    cardButtons.Add(cd);
                }

                HeroCard hc = new HeroCard()
                {
                    Buttons = cardButtons,
                    Title = "Choose ypur problem" //I mean you know whay this property do
                };

                reply.Attachments.Add(hc.ToAttachment());
            }

            await context.PostAsync(reply);
            context.Wait(TestRecivedAsync);
        }

        private async Task TestRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text.Contains("DTP"))
            {
                await context.PostAsync($"ok DTP");
                context.Wait(TestRecivedAsync);
            }
            else if (activity.Text.Contains("HomeTruble"))
            {
                await context.PostAsync($"you choose HomeTruble");
                context.Wait(TestRecivedAsync);
            }
            else if (activity.Text.Contains("Traffic"))
            {
                await context.PostAsync("You choose TRaffic");
                context.Wait(TestRecivedAsync);
            }
            else
            {
                await context.PostAsync("Sorry");
                context.Wait(TestRecivedAsync);
            }
        }
    }
}