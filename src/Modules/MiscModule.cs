﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Diagnostics;

namespace crackdotnet.Modules
{
    [Name("misc")]
    public class ExampleModule : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Alias("s")]
        [Summary("make me say some shit u want")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task Say([Remainder]string text)
            => ReplyAsync(text);

        [Command("Ping")]
        [Summary("Show the Gateway latency to Discord.")]
        [Remarks("ping")]
        public async Task Command_PingAsync()
        {
            var sw = Stopwatch.StartNew();
            var initial = await ReplyAsync("pingin...").ConfigureAwait(false);
            var restTime = sw.ElapsedMilliseconds.ToString();
            await ReplyAsync(restTime + " ms");
        }

        [Group("set"), Name("user modification")]
        [RequireContext(ContextType.Guild)]
        public class Set : ModuleBase
        {
            [Command("nick"), Priority(1)]
            [Summary("change your nickname to what u want")]
            [RequireUserPermission(GuildPermission.ChangeNickname)]
            public Task Nick([Remainder]string name)
                => Nick(Context.User as SocketGuildUser, name);

            [Command("nicksomeone"), Priority(0)]
            [Summary("change another user's nickname to what u want")]
            [RequireUserPermission(GuildPermission.ManageNicknames)]
            public async Task Nick(SocketGuildUser user, [Remainder]string name)
            {
                await user.ModifyAsync(x => x.Nickname = name);
                await ReplyAsync($"{user.Mention} i changed your name to **{name}**");
            }

            
        }
    }
}
