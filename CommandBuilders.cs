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
                description: "Your Dev.to API Key"
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

        public static Command buildCmdPrepArticle() {
            var cmd = new Command("prep-article", "Prepares a Markdown article for posting to Dev.to by replacing image links to point to GitHub repo");
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--article-path", "-p"},
                description: "The name of the article's Markdown file"
            ));
            cmd.AddOption(new Option<string>(
                aliases: new string[] {"--github-user", "-u"},
                description: "Your Github Username"
            ));
            cmd.AddOption(new Option<string> (
                aliases: new string[] {"--github-repo", "-r"},
                description: "The name of the article's Github repo"
            ));
            cmd.AddOption(new Option<string> (
                aliases: new string[] {"--images-path", "-i"},
                description: "The path within the repo to the images"
            ));
            cmd.Handler = CommandHandler.Create<string,string,string,string>(CommandFunctions.PrepArticle);

            return cmd;
        }
    }
}