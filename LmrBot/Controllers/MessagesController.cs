using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using com.valgut.libs.bots.Wit;
using Microsoft.Bot.Connector;
using static Microsoft.Bot.Connector.ActivityTypes;

namespace LmrBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static lmr db = new lmr();
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            switch (activity.Type)
            {

                case Message:
                    WitClient wit = new WitClient("ZHKXGGSDV7VISTIZDWQOPWJ7DZYQ3APD");
                    var msg = wit.Converse(activity.From.Id, activity.Text);

                    var intent = string.Empty;
                    double conf = 0;
                    try
                    {
                        var a = msg.entities["intent"];
                        if (a != null)
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
            if (message.Type == DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == Ping)
            {
            }

            return null;
        }
        private static string GetReplyFromDb(string intent)
        {
            var arrToRandomFrom = db.Responses.Where(x => x.Intent.content == intent).ToArray();
            if (arrToRandomFrom.Length > 0)
                return arrToRandomFrom[new Random().Next(arrToRandomFrom.Length)].content;
            else
            {
                var noreply = db.Responses.Where(x => x.Intent.content == "noreply").ToArray();
                return noreply[new Random().Next(noreply.Length)].content;
            }
        }
    }
}