using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using com.valgut.libs.bots.Wit;
using Microsoft.Bot.Connector;
using Entity = com.valgut.libs.bots.Wit.Models.Entity;

namespace LmrBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        protected static readonly lmr Db = new lmr();
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    WitClient wit = new WitClient("57O7M43QDRWRDPFTDU7ABTOYYCECYDTO");
                    var msg = wit.Converse(activity.From.Id, activity.Text);

                    var intent = string.Empty;
                    double conf = 0;
                    try
                    {
                        if (msg.entities["intent"] != null)
                        {
                            foreach (var z in msg.entities["intent"])
                            {
                                if (z.confidence > conf)
                                {
                                    conf = z.confidence;
                                    intent = z.value.ToString();
                                }
                            }
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                    Activity reply = activity.CreateReply();
                    switch (intent)
                    {
                        case "cat":
                            reply.Text = "Ловіть котика :)";
                            reply.Attachments = new List<Attachment>();  //****** INIT
                            var ts = DateTime.Now;
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = $"http://thecatapi.com/api/images/get?format=src&type=png&timestamp=" + $"{ts}",
                                ContentType = "image/png"
                            });
                            break;
                        // TODO


                        default:
                            reply.Text = GetReplyFromDb(intent);
                            break;
                    }
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    break;

            }
            // return our reply to the user
            
            HandleSystemMessage(activity);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
        private static string GetReplyFromDb(string intent)
        {
            var arrToRandomFrom = Db.Responses.Where(x => x.Intent.content == intent).ToArray();
            if (arrToRandomFrom.Length > 0)
            {
                return arrToRandomFrom[new Random().Next(arrToRandomFrom.Length)].content;
            }

            return Db.Responses.Where(x => x.Intent.content == "noreply").ToArray()[new Random().Next(Db.Responses.Where(x => x.Intent.content == "noreply").ToArray().Length)].content;
        }
    }
}