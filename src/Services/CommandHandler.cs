using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace crackdotnet
{
    public class CommandHandler
    {
        
        //private readonly ILogger<CommandHandlingService> _logger;
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _serviceProvider;
        private int shiftycoinCounter = 0;
        static StringBuilder sb = new StringBuilder();
        static StringWriter sw = new StringWriter(sb);

        public CommandHandler(
            //ILogger<CommandHandler> logger,
            CommandService commandService,
            DiscordSocketClient discord,
            IServiceProvider serviceProvider)
        {
            //_logger = logger;
            _commandService = commandService;
            //_discordSocketClient = discordSocketClient;
            _serviceProvider = serviceProvider;
            _discord = discord;
            _discord.MessageReceived += OnMessageReceivedAsync;
        }


        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            //Console.WriteLine("we recieved");
            if (!(s is SocketUserMessage msg)) return;
            //if (!(s.Channel is SocketGuildChannel)) return;

            int argPos = 0;
            var context = new SocketCommandContext(_discord, msg);     // Create the command context
            if (msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                if (msg.Author.Id != _discord.CurrentUser.Id)
                {
                    var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
                    if (!result.IsSuccess) Console.WriteLine("sommen fuked");
                    if (result.IsSuccess) return;

                    switch (result)
                    {
                        case ExecuteResult execute:
                            Console.WriteLine(execute.Exception?.ToString());
                            return;
                        case ParseResult parse when parse.Error == CommandError.BadArgCount:
                            // Send Help Text
                            return;
                        default:
                            //await context.Channel.SendMessageAsync(result.ErrorReason);
                            if (result.ErrorReason == "Unknown command.") await context.Channel.SendMessageAsync(result.ErrorReason + " Did you accidentally put an unnecessary space somewhere?");
                            else await context.Channel.SendMessageAsync(result.ErrorReason);
                            return;
                    }
                }
            }
            else if (msg.Author.Id != _discord.CurrentUser.Id)
            {
                var id = context.Message.Author.Id;
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject users = (JObject)parsed["users"];
                JObject totalareas = (JObject)parsed["totalareas"];
                if (shiftycoinCounter < 100)
                {
                    shiftycoinCounter++;
                    //Console.WriteLine("counter plus");
                }
                else
                {
                    shiftycoinCounter = 0;


                    if (scjson.Contains(id.ToString()))
                    {
                        users[id.ToString()] = ((int)users[id.ToString()]) + 1;
                        totalareas["totalcoin"] = ((int)totalareas["totalcoin"]) + 1;
                        System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed.ToString());
                        //Console.WriteLine("upped value of user " + id.ToString());

                    }
                    else
                    {
                        users.Property("start").AddAfterSelf(new JProperty(id.ToString(), 1));
                        totalareas["totalcoin"] = ((int)totalareas["totalcoin"]) + 1;
                        System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed.ToString());
                        Console.WriteLine("new user added, " + id.ToString());
                    }
                }
            }
        }
    }
}