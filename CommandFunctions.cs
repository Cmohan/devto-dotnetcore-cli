using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace devto_dotnetcore_cli
{
    class CommandFunctions {

        public static void NewAPIKey(string apiKey)
        {
            var key = new DevtoAPIKey(apiKey);
            string jsonstring= JsonSerializer.Serialize(key);
            File.WriteAllText($"{Globals.CONTENT_PATH}\\devto-api-key.json", jsonstring);

            Console.WriteLine("\nNew API Key Saved\n");
        }

        public static void ListAllArticles()
        {
            string result = getAllArticles().GetAwaiter().GetResult();
             List<Article> articles = JsonSerializer.Deserialize<List<Article>>(result);
            Console.WriteLine(articles[0]);
        }

        public static async Task<string> getAllArticles()
        {
            string apiKey = Program.DevtoAuth();
            string url = "https://dev.to/api/articles/me/all";
            var client = new HttpClient();

            var request = new HttpRequestMessage() {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("api_key", apiKey);
            string articlesJSON = "";
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                articlesJSON = await response.Content.ReadAsStringAsync();
            }
            else {
                if (response.Content != null)
                {
                    articlesJSON = await response.Content.ReadAsStringAsync();
                }
            }

            return articlesJSON;
        }

        public static void PrepArticle(string articlePath, string githubUser, string githubRepo, string imagesPath)
        {
            string imageUrl = $"https://raw.githubusercontent.com/{githubUser}/{githubRepo}/master/.{imagesPath}";

            var articleContent = File.ReadAllText(articlePath);
            articleContent = articleContent.Replace(imagesPath, imageUrl);

            File.WriteAllText(".\\dev-to-post.md", articleContent);
        }
    }
}