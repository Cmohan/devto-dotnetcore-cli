using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace devto_dotnetcore_cli
{
    class CommandFunctions {

        public static void NewAPIKey(string apiKey)
        {
            try
            {
                var key = new DevtoAPIKey(apiKey);
                string jsonstring= JsonSerializer.Serialize(key);

                File.WriteAllText($"{Globals.CONTENT_PATH}\\devto-api-key.json", jsonstring);

                Console.WriteLine("\nNew API Key Saved\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nAPI Key not saved, Please try again\nError: {e.Message}");
            }
        }

        public static void ListAllArticles()
        {
            try
            {
                string result = ListAllArticlesRequest().GetAwaiter().GetResult();
                if (result.StartsWith("{\"error\""))
                {
                    Error error = JsonSerializer.Deserialize<Error>(result);
                    Console.WriteLine(error);
                }
                else
                {
                    result = result.Replace("tag_list", "tags");
                    List<Article> articles = JsonSerializer.Deserialize<List<Article>>(result);
                    Console.WriteLine(articles);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(new Error(e.Message));
            }
        }

        public static async Task<string> ListAllArticlesRequest()
        {
            string apiKey = Program.DevtoAuth();

            if (apiKey != null)
            {
                string url = "https://dev.to/api/articles/me/all";
                var client = new HttpClient();
                string responseJSON = "";
                var response = new HttpResponseMessage();

                try
                {
                    var request = new HttpRequestMessage() {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Get,
                    };
                    request.Headers.Add("api_key", apiKey);
                    response = await client.SendAsync(request);
                }
                catch (Exception e)
                {
                    return $"{{\"error\": \"{e.Message}\",\"status\": 0}}";
                }
                
                if (response.IsSuccessStatusCode)
                {
                    responseJSON = await response.Content.ReadAsStringAsync();
                }
                else {
                    if (response.Content != null)
                    {
                        responseJSON = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return $"{{\"error\": \"Unknown request error. Please try again.\",\"status\": 400}}";
                    }
                }

                return responseJSON;
            }
            else
            {
                return $"{{\"error\": \"Could not get API Key\",\"status\": 0}}";
            }
        }

        public static void PrepArticle(string articlePath, string githubUser, string githubRepo, string imagesPath)
        {
            try
            {
                string imageUrl = $"https://raw.githubusercontent.com/{githubUser}/{githubRepo}/master/.{imagesPath}";
                var articleContent = File.ReadAllText(articlePath);
                articleContent = articleContent.Replace(imagesPath, imageUrl);
                File.WriteAllText(".\\dev-to-post.md", articleContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(new Error(e.Message));
            }
        }

        public static void GetArticleByUrl(string url)
        {
            if (url.StartsWith("https://dev.to/"))
            {
                try
                {
                    url = url.Substring(14);
                }
                catch (Exception e)
                {
                    Console.WriteLine(new Error($"Couldn't parse Url\n{e.Message}"));
                }
            }
            else
            {
                Console.WriteLine(new Error("Invalid Url\nUrl must start with \"https://dev.to/\""));
            }
            
            string result = GetArticleByUrlRequest(url).GetAwaiter().GetResult();
            
            if (result.StartsWith("{\"error\""))
            {
                Error error = JsonSerializer.Deserialize<Error>(result);
                Console.WriteLine(error);
            }
            else
            {
                Article article = JsonSerializer.Deserialize<Article>(result);
                Console.WriteLine(article);
            }
        }

        public static async Task<string> GetArticleByUrlRequest(string url)
        {
            string apiKey = Program.DevtoAuth();

            if (apiKey != null)
            {
                url = "https://dev.to/api/articles" + url;
                var client = new HttpClient();
                string responseJSON = "";
                var response = new HttpResponseMessage();

                try
                {
                    var request = new HttpRequestMessage() {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Get,
                    };
                    request.Headers.Add("api_key", apiKey);
                    response = await client.SendAsync(request);
                }
                catch (Exception e)
                {
                    return $"{{\"error\": \"{e.Message}\",\"status\": 0}}";
                }
                
                if (response.IsSuccessStatusCode)
                {
                    responseJSON = await response.Content.ReadAsStringAsync();
                }
                else {
                    if (response.Content != null)
                    {
                        responseJSON = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        return $"{{\"error\": \"Unknown request error. Please try again.\",\"status\": 400}}";
                    }
                }

                return responseJSON;
            }
            else
            {
                return $"{{\"error\": \"Could not get API Key\",\"status\": 0}}";
            }
        }

        public static void PostArticle(string title, string article_path, string main_image, string series, bool published, string[] tags)
        {
            string body_markdown = File.ReadAllText(article_path);

            string body = $"{{\"title\": \"{title},\"published\": \"{published}\",\"main_image\": \"{main_image}\",\"series\": \"{series}\",\"body_markdonw\": \"{body_markdown}\",\"tags\":[";

            foreach (var tag in tags)
            {
                body += $"\"{tag}\",";
            }
            
            body = body.Substring(0, body.Length-2) + "]}";

            string result = PostArticleRequest(body).GetAwaiter().GetResult();
             Article article = JsonSerializer.Deserialize<Article>(result);
            Console.WriteLine(article);
        }

        public static async Task<string> PostArticleRequest(string body)
        {
            string apiKey = Program.DevtoAuth();
            var client = new HttpClient();
            string url = "https://dev.to/api/articles";
            var request = new HttpRequestMessage() {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };
            request.Headers.Add("api_key", apiKey);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            string articleJSON = "";

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                articleJSON = await response.Content.ReadAsStringAsync();
            }
            else {
                if (response.Content != null)
                {
                    articleJSON = await response.Content.ReadAsStringAsync();
                }
            }

            return articleJSON;
        }

        public static void UpdateArticle(string url, string title, string article_path, string main_image, string series, bool published, string[] tags)
        {
            url = url.Substring(14);

            string getArticle = GetArticleByUrlRequest(url).GetAwaiter().GetResult();
            Article originalArticle = JsonSerializer.Deserialize<Article>(getArticle);


            string body_markdown = File.ReadAllText(article_path);

            string body = $"{{\"title\": \"{title},\"published\": \"{published}\",\"main_image\": \"{main_image}\",\"series\": \"{series}\",\"body_markdonw\": \"{body_markdown}\",\"tags\":[";

            foreach (var tag in tags)
            {
                body += $"\"{tag}\",";
            }
            
            body = body.Substring(0, body.Length-2) + "]}";

            string result = UpdateArticleRequest(originalArticle.id, body).GetAwaiter().GetResult();
            Article article = JsonSerializer.Deserialize<Article>(result);
            Console.WriteLine(article);
        }

        public static async Task<string> UpdateArticleRequest(int id,string body)
        {
            string apiKey = Program.DevtoAuth();
            var client = new HttpClient();
            string url = $"https://dev.to/api/articles/{id}";
            var request = new HttpRequestMessage() {
                RequestUri = new Uri(url),
                Method = HttpMethod.Put,
            };
            request.Headers.Add("api_key", apiKey);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            string articleJSON = "";

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                articleJSON = await response.Content.ReadAsStringAsync();
            }
            else {
                if (response.Content != null)
                {
                    articleJSON = await response.Content.ReadAsStringAsync();
                }
            }

            return articleJSON;
        }
    }
}