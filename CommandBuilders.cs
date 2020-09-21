using System.CommandLine;
using System.CommandLine.Invocation;

namespace devto_dotnetcore_cli
{
    class CommandBuilders
    {
        public static Command buildCmdNewAPI()
        {
            var cmd = new Command("new-api-key", "Adds new Dev.to API key to this tool as a JSON settings file");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--api-key", "-a"},
                description: "Your Dev.to API Key\nRequired"
            ));

            cmd.Handler = CommandHandler.Create<string>(CommandFunctions.NewAPIKey);

            return cmd;
        }

        public static Command buildCmdListAllArticles()
        {
            var cmd = new Command("list-all-articles", "Lists all of your Dev.to articles, published and unpublished");

            cmd.Handler = CommandHandler.Create(CommandFunctions.ListAllArticles);

            return cmd;
        }

        public static Command buildCmdPrepArticle()
        {
            var cmd = new Command("prep-article", "Prepares a Markdown article for posting to Dev.to by replacing image links to point to GitHub repo");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--article-path", "-p"},
                description: "The path to the article's Markdown file\nRequired"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--github-user", "-u"},
                description: "Your Github Username\nRequired"
            ));
            cmd.AddOption(new Option<string> (
                aliases: new string[] {"--github-repo", "-r"},
                description: "The name of the article's Github repo\nRequired"
            ));
            cmd.AddOption(new Option<string> (
                aliases: new string[] {"--images-path", "-i"},
                description: "The path within the repo to the images\nRequired"
            ));

            cmd.Handler = CommandHandler.Create<string,string,string,string>(CommandFunctions.PrepArticle);

            return cmd;
        }

        public static Command buildCmdGetArticleByUrl()
        {
            var cmd = new Command("get-article-by-url", "Gets any Dev.to article using the URL of that article");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--url", "-u"},
                description: "The Url of the article\nRequired"
            ));

            cmd.Handler = CommandHandler.Create<string>(CommandFunctions.GetArticleByUrl);

            return cmd;
        }

        public static Command buildCmdPostArticle()
        {
            var cmd = new Command("post-article", "Posts a new article to Dev.to");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--title", "-t"},
                description: "Title of the new article\nRequired"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--article-path", "-path"},
                description: "Path to the Markdown file of the article\nRequired"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--main_image", "-i"},
                getDefaultValue: () => "",
                description: "Url to image to be used as a header image on the article\nOptional"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--series", "-s"},
                getDefaultValue: () => "",
                description: "Name of the series the article belongs to\nOptional"
            ));
            cmd.AddOption(new Option<bool>(
                aliases: new string[] {"--published", "-pub"},
                getDefaultValue: () => true,
                description: "Whether or not the article should be published by this request\nOptional: Defaults to true"
            ));
            cmd.AddOption(new Option<string[]>(
                aliases: new string[] {"--tags", "-tags"},
                getDefaultValue: () => new string[] {},
                description: "Tags to attach to the article\nOptional"
            ));

            cmd.Handler = CommandHandler.Create<string,string,string,string,bool,string[]>(CommandFunctions.PostArticle);

            return cmd;
        }

        public static Command buildCmdUpdateArticle()
        {
            var cmd = new Command("update-article", "Posts a new article to Dev.to");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--url", "-u"},
                description: "Url of the article to be updated\nRequired"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--title", "-t"},
                description: "Title of the new article\nRequired"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--article-path", "-path"},
                description: "Path to the Markdown file of the article\nRequired"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--main_image", "-i"},
                getDefaultValue: () => "",
                description: "Url to image to be used as a header image on the article\nOptional"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--series", "-s"},
                getDefaultValue: () => "",
                description: "Name of the series the article belongs to\nOptional"
            ));
            cmd.AddOption(new Option<bool>(
                aliases: new string[] {"--published", "-pub"},
                getDefaultValue: () => true,
                description: "Whether or not the article should be published by this request\nOptional: Defaults to true"
            ));
            cmd.AddOption(new Option<string[]>(
                aliases: new string[] {"--tags", "-tags"},
                getDefaultValue: () => new string[] {},
                description: "Tags to attach to the article\nOptional"
            ));

            cmd.Handler = CommandHandler.Create<string,string,string,string,bool,string[]>(CommandFunctions.PostArticle);

            return cmd;
        }
    }
}