using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Nito.AsyncEx;
using System.Linq;

namespace WebsitesPing
{
    class Program
    {
        private const int AmountOfWebsites = 4;

        static void Main(string[] args)
        {
            AsyncContext.Run(() => MainAsync(args));
        }

        static async Task MainAsync(string[] args)
        {
            string[] urls = { "https://www.bing.com/", "https://www.google.com/", "https://www.yahoo.com/",
                "https://www.ask.com/", "https://www.stackoverflow.com/", "https://www.facebook.com/",
                "https://www.twitter.com/", "https://www.youtube.com/" };

            try
            {
                await PingWebsites(urls, AmountOfWebsites);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task PingWebsites(IEnumerable<string> urls, int amountOfWebsites)
        {
            var amountOfAwaited = 0;

            if (amountOfWebsites > urls.Count())
            {
                amountOfWebsites = urls.Count();
            }

            using (var httpClient = new HttpClient())
            {
                var tasks = new List<Task<HttpResponseMessage>>();

                foreach (var url in urls)
                {
                    tasks.Add(httpClient.SendAsync(new HttpRequestMessage
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Head
                    }));
                }

                foreach (var task in tasks.OrderByCompletion())
                {
                    HttpResponseMessage response = await task;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Host: {response.RequestMessage.RequestUri}");

                        if (++amountOfAwaited >= amountOfWebsites)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
