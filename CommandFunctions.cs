using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace devto_dotnetcore_cli
{
    class CommandFunctions
    {
        private static HttpClient client = new HttpClient();
        public static void NewAPIKey(string apiKey)
        {
            //Get API key from user and save it to a file in the Conent folder
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
            //Gets all articles of the user, published and unpublished
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
                    //In V1 of the API, "tags" and "tags_list" are used two different ways in different calls.
                    //In the List all User's Articles, the "tag_list" is the array and the "tags" is the list.
                    //I am only using the array so I have to rename the field to match my class architechture.

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
            //Calls the Dev.to API to get a list of all of the user's articles

            string apiKey = Program.DevtoAuth();

            if (apiKey != null)
            {
                string url = "https://dev.to/api/articles/me/all";
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
            //Prepares an article for posting to Dev.to by replacing the image paths with the Github permalink to the image instead.
            //This means that you won't have to upload images to Dev.to to have them in your article.
            //The images must be in Github before running this prep command.

            Console.WriteLine("The images must be uploaded to Github for this feature to work.\nPlease confirm that the images have been uploaded to Github before continuing");
            Console.Write("[Y] Yes [N] No :");
            ConsoleKey confirm = Console.ReadKey().Key;

            if(confirm != ConsoleKey.Y)
            {
                Console.WriteLine("Please upload to Github and try again.");
                return;
            }

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
            //Gets any article in Dev.to from the URL of the article.

            if (url.StartsWith("https://dev.to/"))
            {
                try
                {
                    url = url.Substring(14);
                }
                catch (Exception e)
                {
                    Console.WriteLine(new Error($"Couldn't parse Url\n{e.Message}"));
                    return;
                }
            }
            else
            {
                Console.WriteLine(new Error("Invalid Url\nUrl must start with \"https://dev.to/\""));
                return;
            }
            
            try
            {
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
            catch (Exception e)
            {
                Console.Write(new Error(e.Message));
            }
        }

        public static async Task<string> GetArticleByUrlRequest(string url)
        {
            //Calls the Dev.to API to get the article by URL

            string apiKey = Program.DevtoAuth();

            if (apiKey != null)
            {
                url = "https://dev.to/api/articles" + url;
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
            //Post a new article to Dev.to from a Markdown file

            string body = "";

            try
            {
                string body_markdown = File.ReadAllText(article_path);

                body = $"{{\"title\": \"{title},\"published\": \"{published}\",\"main_image\": \"{main_image}\",\"series\": \"{series}\",\"body_markdonw\": \"{body_markdown}\",\"tags\":[";

                foreach (var tag in tags)
                {
                    body += $"\"{tag}\",";
                }
                
                body = body.Substring(0, body.Length-2) + "]}";
            }
            catch (Exception e)
            {
                Console.WriteLine(new Error($"Issue building request body: {e.Message}"));
                return;
            }
            
            try
            {
                string result = PostArticleRequest(body).GetAwaiter().GetResult();
            
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
            catch (Exception e)
            {
                Console.Write(new Error(e.Message));
            }
        }

        public static async Task<string> PostArticleRequest(string body)
        {
            //Calls the Dev.to API to post a new article

            string apiKey = Program.DevtoAuth();

            if (apiKey != null)
            {
                string url = "https://dev.to/api/articles";
                string responseJSON = "";
                var response = new HttpResponseMessage();

                try
                {
                    var request = new HttpRequestMessage() {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Post,
                    };
                    request.Headers.Add("api_key", apiKey);
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");
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

        public static void UpdateArticle(string url, string title, string article_path, string main_image, string series, bool published, string[] tags)
        {
            //Updates an existing article using the URL and a Markdown file.

            string body = "";
            Article originalArticle = new Article();

            //Parsing the URL to get the username and the slug
            if (url.StartsWith("https://dev.to/"))
            {
                try
                {
                    url = url.Substring(14);
                }
                catch (Exception e)
                {
                    Console.WriteLine(new Error($"Couldn't parse Url\n{e.Message}"));
                    return;
                }
            }
            else
            {
                Console.WriteLine(new Error("Invalid Url\nUrl must start with \"https://dev.to/\""));
                return;
            }

            //Use the username and the slug to get the article's ID number
            try
            {
                string getArticle = GetArticleByUrlRequest(url).GetAwaiter().GetResult();
            
                if (getArticle.StartsWith("{\"error\""))
                {
                    Error error = JsonSerializer.Deserialize<Error>(getArticle);
                    Console.WriteLine(error);
                    return;
                }
                else
                {
                    originalArticle = JsonSerializer.Deserialize<Article>(getArticle);
                }
            }
            catch (Exception e)
            {
                Console.Write(new Error(e.Message));
                return;
            }

            //Build the body of the request with the command arguments
            try
            {
                string body_markdown = File.ReadAllText(article_path);

                body = $"{{\"title\": \"{title},\"published\": \"{published}\",\"main_image\": \"{main_image}\",\"series\": \"{series}\",\"body_markdonw\": \"{body_markdown}\",\"tags\":[";

                foreach (var tag in tags)
                {
                    body += $"\"{tag}\",";
                }
                
                body = body.Substring(0, body.Length-2) + "]}";
            }
            catch (Exception e)
            {
                Console.WriteLine(new Error($"Issue building request body: {e.Message}"));
                return;
            }
            
            //Use the ID number and the body to update the aritcle
            try
            {
                string result = UpdateArticleRequest(originalArticle.id, body).GetAwaiter().GetResult();
            
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
            catch (Exception e)
            {
                Console.Write(new Error(e.Message));
            }
        }

        public static async Task<string> UpdateArticleRequest(int id,string body)
        {
            //Calls the Dev.to API to update an existing article
            
            string apiKey = Program.DevtoAuth();

            if (apiKey != null)
            {
                string url = $"https://dev.to/api/articles/{id}";
                string responseJSON = "";
                var response = new HttpResponseMessage();

                try
                {
                    var request = new HttpRequestMessage() {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Put,
                    };
                    request.Headers.Add("api_key", apiKey);
                    request.Content = new StringContent(body, Encoding.UTF8, "application/json");
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
    }
}