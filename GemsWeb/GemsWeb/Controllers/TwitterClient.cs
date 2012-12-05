using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Twitterizer;

namespace GemsWeb.Controllers
{
    public class TwitterClient
    {
        private static OAuthTokens tokens;

        static TwitterClient()
        {
            tokens = new OAuthTokens();
            tokens.ConsumerKey = "TJsNENXQpENdbZfu7REPNQ";
            tokens.ConsumerSecret = "BzsNUP8sFKXOZRBOy1q2s7erBFJRqwQPPGvtRBAl02I";
            tokens.AccessToken = "985961396-EmkCjiJFqQJ8KYv2IUY8lnpA9fLj76F9192t57HO";
            tokens.AccessTokenSecret = "PRMnstfw3DVAXRpWpR4EIzoBeTfqwnTmkXkZnDrgDA";

        }
        public static void SendMessage(string message)
        {
            string post = "";

            if (message.Length >= 140)
                post = message.Substring(0, 140);

            else
                post = message;

            try
            {
                IAsyncResult asyncResult = TwitterStatusAsync.Update(
                   tokens,                     // The OAuth tokens
                   post,    // The text of the tweet
                   null,                       // Optional parameters (none given here)
                   new TimeSpan(0, 10, 0),      // The maximum time to let the process run
                   updateResponse =>           // The callback method
                   {
                       if (updateResponse.Result == RequestResult.Success)
                       {
                           Console.WriteLine("Success!");
                       }
                       else
                       {
                           Console.WriteLine(updateResponse.Result);
                       }
                   });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}