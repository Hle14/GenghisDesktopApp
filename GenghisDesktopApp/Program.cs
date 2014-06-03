using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespaces I declared
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

//Oauth
using OAuth;

namespace GenghisDesktopApp
{
    class Program
    {
        //the full api address
        static string _address = "https://api.twitter.com/1.0/statuses/user_timeline.json?user_id=hansle518&screen_name=hansle518";

        static void Main(string[] args)
        {
            //RunAsync().Wait();

            HttpClient client = new HttpClient(new OAuthMessageHandler(new HttpClientHandler()));

            //send asynchronous request to twitter
            client.GetAsync(_address).ContinueWith(
                (requestTask) =>
                {
                    //Get HTTP response from completed task
                    HttpResponseMessage response = requestTask.Result;

                    //Check that response was successful or throw exception
                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Uh OH!!{0}",e);
                    }

                    //Read response aynschronously as JsonValue and write out tweet texts
                    response.Content.ReadAsAsync<JArray>().ContinueWith(
                        (readTask) =>
                        {
                            JArray statuses = readTask.Result;
                            Console.WriteLine("\nLast 5 statuses from hansle518's twitter");
                            foreach(var status in statuses)
                            {
                                Console.WriteLine(status["text"] + "\n");
                            }
                        });
                });

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        //static async Task RunAsync()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //    }
        //}
    }
}
