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
                description: "Your Dev.to API Key - Required"
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
                description: "The path to the article's Markdown file - Required"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--github-user", "-u"},
                description: "Your Github Username - Required"
            ));
            cmd.AddOption(new Option<string> (
                aliases: new string[] {"--github-repo", "-r"},
                description: "The name of the article's Github repo - Required"
            ));
            cmd.AddOption(new Option<string> (
                aliases: new string[] {"--images-path", "-i"},
                description: "The path within the repo to the images - Required"
            ));

            cmd.Handler = CommandHandler.Create<string,string,string,string>(CommandFunctions.PrepArticle);

            return cmd;
        }

        public static Command buildCmdGetArticleByUrl()
        {
            var cmd = new Command("get-article-by-url", "Gets any Dev.to article using the URL of that article");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--url", "-u"},
                description: "The URL of the article - Required"
            ));

            cmd.Handler = CommandHandler.Create<string>(CommandFunctions.GetArticleByUrl);

            return cmd;
        }

        public static Command buildCmdPostArticle()
        {
            var cmd = new Command("post-article", "Posts a new article to Dev.to");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--title", "-t"},
                description: "The title of the new article - Required"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--article-path", "-p"},
                description: "The path to the article's Markdown file - Required"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--main_image", "-i"},
                getDefaultValue: () => "",
                description: "The URL of the image to be used as a header image of the article - Optional"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--series", "-s"},
                getDefaultValue: () => "",
                description: "The name of the series the article belongs to - Optional"
            ));
            cmd.AddOption(new Option<bool>(
                aliases: new string[] {"--published"},
                getDefaultValue: () => true,
                description: "Choose whether to publish the article now or not - Optional"
            ));
            cmd.AddOption(new Option<string[]>(
                aliases: new string[] {"--tags", "-tags"},
                getDefaultValue: () => new string[] {},
                description: "A list of tags to attach to the article - Optional"
            ));

            cmd.Handler = CommandHandler.Create<string,string,string,string,bool,string[]>(CommandFunctions.PostArticle);

            return cmd;
        }

        public static Command buildCmdUpdateArticle()
        {
            var cmd = new Command("update-article", "Posts a new article to Dev.to");

            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--url", "-u"},
                description: "Url of the article to be updated - Required"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--article-path", "-p"},
                description: "Path to the Markdown file of the article - Required"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--title", "-t"},
                description: "Title of the new article - Optional"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--main-image", "-i"},
                getDefaultValue: () => "",
                description: "Url to image to be used as a header image on the article - Optional"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--series", "-s"},
                getDefaultValue: () => "",
                description: "Name of the series the article belongs to - Optional"
            ));
            cmd.AddOption(new Option<bool>(
                aliases: new string[] {"--published"},
                getDefaultValue: () => true,
                description: "Whether or not the article should be published by this request - Optional"
            ));
            cmd.AddOption(new Option<string[]>(
                aliases: new string[] {"--tags", "-tags"},
                getDefaultValue: () => new string[] {},
                description: "Tags to attach to the article - Optional"
            ));

            cmd.Handler = CommandHandler.Create<string,string,string,string,string,bool,string[]>(CommandFunctions.UpdateArticle);

            return cmd;
        }
    }
}