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
        private Order orderform = new Order();
        private string _ordertype { get; set; }
        private string _regionoflviv { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Привіт я бот який допоможе вирішити вашу проблему завповніть форму.");
            context.Wait(RegionRecivedAsync);
        }

        private async Task MessegeRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            List<CardAction> cardButtons;

            var activity = await result as Activity;

            _regionoflviv = activity.Text;

            var reply = activity.CreateReply();
            reply.Attachments = new List<Attachment>();

            //when you write "show cards..." this function work ...
            //if (activity.Text.StartsWith("show") || activity.Text.Contains("перезаповнити"))
            //{
            cardButtons = new List<CardAction>();
            //string[] enstr = Enum.GetNames(typeof(TypeOrder));
            string[] ukrenstr = { "Побутові Проблеми", "ДТП", "Затори", "Інше" };

            //Creation of acction also its buttons)))
            CardAction cd;

            for (int i = 0; i < ukrenstr.Length; i++)
            {
                cd = new CardAction
                {
                    Title = ukrenstr[i], //enstr[i],
                    Value = ukrenstr[i]
                };

                //Get Data from value property)))
                //testgetvalue = (string)cd.Value;

                cardButtons.Add(cd);
            }

            HeroCard hc = new HeroCard()
            {
                Buttons = cardButtons,
                Title = "Виберіть тип вашої проблеми." //I mean you know whay this property do
            };

            reply.Attachments.Add(hc.ToAttachment());
            //}

            await context.PostAsync(reply);
            context.Wait(TestRecivedAsync);
        }

        private async Task RegionRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            string[] regionstr = { "Шевченківський", "Личаківський", "Сихівський", "Франківський", "Залізничний", "Галицький" };

            var activity = await result as Activity;
            var reply = activity.CreateReply();
            reply.Attachments = new List<Attachment>();

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction cd;

            for (int i = 0; i < regionstr.Length; i++)
            {
                cd = new CardAction
                {
                    Title = regionstr[i], 
                    Value = regionstr[i]
                };

                cardButtons.Add(cd);
            }

            HeroCard hc = new HeroCard()
            {
                Buttons = cardButtons,
                Title = "Виберіть район." //I mean you know whay this property do
            };

            reply.Attachments.Add(hc.ToAttachment());

            await context.PostAsync(reply);
            context.Wait(MessegeRecivedAsync);
        }

        private async Task TestRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            _ordertype = activity.Text;

            if (activity.Text.Contains("ДТП"))
            {
                await context.PostAsync($"Опишіть що трапилось.");
                context.Wait(FormRecivedAsync);
            }
            else if (activity.Text.Contains("Побутові Проблеми"))
            {
                await context.PostAsync($"Опишіть що трапилось.");
                context.Wait(FormRecivedAsync);
            }
            else if (activity.Text.Contains("Затори"))
            {
                await context.PostAsync("Опишіть що трапилось.");
                context.Wait(FormRecivedAsync);
            }
            else if (activity.Text.Contains("Інше"))
            {
                await context.PostAsync("Опишіть що трапилось.");
                context.Wait(FormRecivedAsync);
            }
            else
            {
                await context.PostAsync("Вибачте спробуйте ще раз.");
                context.Wait(TestRecivedAsync);
            }
        }

        private async Task FormRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            orderform.DescriptionOfOrder = activity.Text;

            await context.PostAsync($"У вас: { orderform.DescriptionOfOrder}.Тепер напишіть свою електрону пошту.");
            context.Wait(FormMeilRecivedAsync);
        }

        private async Task FormMeilRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            orderform.Email = activity.Text;

            await context.PostAsync($"Ваша пошта: { orderform.Email}.Тепер напишіть свій мобільний номер.");
            context.Wait(FormPhoneRecivedAsync);
        }

        private async Task FormPhoneRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            orderform.PohneNumber = activity.Text;

            await context.PostAsync($"Ваша мобільний номер: { orderform.PohneNumber}.Виберіть пункт вибрати автоматичну локацію чи вказати адресу.Якщо ви згідні то напишіть так.");
            context.Wait(FormPlaceRecivedAsync);
        }

        private async Task FormPlaceRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            List<CardAction> cardButtons;

            var activity = await result as Activity;
            var reply = activity.CreateReply();
            reply.Attachments = new List<Attachment>();

            string[] locattionstring = { "автоматично визначити", "вкзати адрес" };

            //if (activity.Text.StartsWith("так"))
            //{
                cardButtons = new List<CardAction>();

                CardAction cd;

                for (int i = 0; i < locattionstring.Length; i++)
                {
                    cd = new CardAction
                    {
                        Title = locattionstring[i],
                        Value = locattionstring[i]
                    };
                    
                    cardButtons.Add(cd);
                }
                HeroCard hc = new HeroCard()
                {
                    Buttons = cardButtons,
                    Title = "Виберіть" //I mean you know whay this property do
                };

                reply.Attachments.Add(hc.ToAttachment());
            //}
            await context.PostAsync(reply);
            context.Wait(FormPlaceSecondPartRecivedAsync);
        }

        private async Task FormPlaceSecondPartRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text.Contains("автоматично визначити"))
            {
                await context.PostAsync("Ваша локаці: ");
                context.Wait(FormPlaceSecondPartRecivedAsync);
            }
            else if(activity.Text.Contains("вкзати адрес"))
            {
                await context.PostAsync("Напишіть будь-ласка адресу.");
                context.Wait(FormPlaceDressPartRecivedAsync);
            }
            else
            {
                await context.PostAsync("Некоректно вибрані дані.Спробуйте ще раз");
                context.Wait(FormPlaceRecivedAsync);
            }

        }

        private async Task FormPlaceDressPartRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            orderform.Adress = activity.Text;

            var reply = activity.CreateReply();
            reply.Attachments = new List<Attachment>();

            string[] yesorno = { "відправити", "перезаповнити" };

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction cd;

            for (int i = 0; i < yesorno.Length; i++)
            {
                cd = new CardAction
                {
                    Title = yesorno[i],
                    Value = yesorno[i]
                };

                cardButtons.Add(cd);
            }
            HeroCard hc = new HeroCard()
            {
                Subtitle = $"Тип: {_ordertype.ToString()}. \nАдреса: {orderform.Adress}. \nВаш номер: {orderform.PohneNumber}. \nВаш опис: {orderform.DescriptionOfOrder}. \nВаш район: {_regionoflviv.ToString()}",
                Buttons = cardButtons,
                Title = "Виберіть" //I mean you know whay this property do
            };

            reply.Attachments.Add(hc.ToAttachment());

            await context.PostAsync(reply);
            context.Wait(FormEndRecivedAsync);
        }

        private async Task FormEndRecivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.Contains("відправити"))
            {
                await context.PostAsync("Дякую)");
                context.Wait(FormEndRecivedAsync);
            }
            else if (activity.Text.Contains("перезаповнити"))
            {
                await context.PostAsync("щоб продовжити роботу напишіть перезаповнити!");

                context.Wait(MessegeRecivedAsync);
            }
            else
            {
                await context.PostAsync("Незрозумів ввиберіть будь-ласка ще раз");
                context.Wait(FormPlaceDressPartRecivedAsync);
            }
        }

    }
}