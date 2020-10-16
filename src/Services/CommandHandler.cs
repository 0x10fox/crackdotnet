using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace crackdotnet
{
    public class CommandHandler
    {
        /**
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _provider;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config,
            IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }
        
        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;     // Ensure the message is from a user/bot
            if (msg == null) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return;     // Ignore self when checking commands
            
            var context = new SocketCommandContext(_discord, msg);     // Create the command context

            int argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);     // Execute the command

                if (!result.IsSuccess) Console.WriteLine("something sucks");    // If not successful, reply with the error.
                    //await context.Channel.SendMessageAsync(result.ToString());
                
            }
        
        }
        **/
        //private readonly ILogger<CommandHandlingService> _logger;
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _serviceProvider;

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

        public void Start()
        {
            _discord.MessageReceived += OnMessageReceivedAsync;
            //_logger.LogInformation("Started");
        }

        public void Stop()
        {
            _discord.MessageReceived -= OnMessageReceivedAsync;
            //_logger.LogInformation("Stopped");
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
        }
    }
}