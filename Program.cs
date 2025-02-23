﻿using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System.Net.Http;

//TODO: Write Readme & How to's on all commands

namespace devto_dotnetcore_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand();

            foreach (var c in (BuildCommands()))
            {
                rootCommand.AddCommand(c);
            }
            
            rootCommand.InvokeAsync(args).Wait();
        }

        public static List<Command> BuildCommands ()
        {
            string commandsJSONPath = $"{Globals.CONTENT_PATH}\\commands.json";

            var commandList = new List<Command>();
            var commandsJSON = File.ReadAllText(commandsJSONPath);
            List<CommandInfo> commands = JsonSerializer.Deserialize<List<CommandInfo>>(commandsJSON);
            Type type = typeof(CommandBuilders);
            MethodInfo method;

            foreach (var c in commands)
            {
                method = type.GetMethod(c.builderFunction);
                commandList.Add((Command)method.Invoke(null, null));
            }

            return commandList;
        }

        public static string DevtoAuth ()
        {
            string jsonString;
            DevtoAPIKey apiKey;
            string apiKeyJSONPath = $"{Globals.CONTENT_PATH}\\devto-api-key.json";

            try
            {
                jsonString = File.ReadAllText(apiKeyJSONPath);
                apiKey = JsonSerializer.Deserialize<DevtoAPIKey>(jsonString);

            }
            catch (FileNotFoundException)
            {
                return "No saved API key found. Please run the 'new-api-key' command to save an API key.";
            }
            catch (Exception e)
            {
                return $"Type: {e.InnerException}\nMessage:{e.Message}";
            }

            return apiKey.key;
        }
    }
}
