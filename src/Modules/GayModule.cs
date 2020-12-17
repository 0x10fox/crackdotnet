using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
//using static crackdotnet.CommandHandler;

namespace crackdotnet.Modules
{
    [Name("gay")]
    public class GayModule : ModuleBase<SocketCommandContext>
    {
        [Group("shiftycoin"), Name("shiftycoin")]
        public class shiftycoin : ModuleBase
        {
            [Command("amount")]
            [Summary("show the amount of shiftycoin you have")]
            public async Task Command_CoinAmountAsync()
            {
                string id = Context.Message.Author.Id.ToString();
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject users = (JObject)parsed["users"];
                await ReplyAsync("you have " + users[id] + " shiftycoin");
            }
            [Command("totalcoin")]
            [Summary("show the amount of shiftycoin awarded to all users combined")]
            public async Task Command_TotalCoinAsync()
            {
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject users = (JObject)parsed["users"];
                JObject totalareas = (JObject)parsed["totalareas"];
                await ReplyAsync("there are " + totalareas["totalcoin"] + " shiftycoin in circulation");
            }
            [Command("pay")]
            [Summary("pay a user")]
            public async Task Command_PayAsync(int a, [Remainder]SocketGuildUser user)
            {
                string id1 = Context.Message.Author.Id.ToString();
                string id2 = user.Id.ToString();
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject users = (JObject)parsed["users"];
                if ((int)users[id1] >= a && a >= 0)
                {
                    users[id1] = ((int)users[id1]) - a;
                    if (!scjson.Contains(id2))
                    {
                        users.Property("start").AddAfterSelf(new JProperty(id2, a));
                        System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed.ToString());
                        Console.WriteLine("new user added, " + id2);
                    }
                    else
                    {
                        users[id2] = ((int)users[id2]) + a;
                        System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed.ToString());
                    }
                    await ReplyAsync("you have paid " + user.Mention + " " + a + " shiftycoin.");
                }
                else
                {
                    await ReplyAsync("you dont have enough coin");
                }
                

            }

            [Command("payid")]
            [Summary("pay a user via their id")]
            public async Task Command_PayIdAsync(int a, string id2)
            {
                string id1 = Context.Message.Author.Id.ToString();
                //string id2 = user.Id.ToString();
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject users = (JObject)parsed["users"];
                if ((int)users[id1] >= a && a >= 0)
                {
                    
                    if (!scjson.Contains(id2))
                    {
                        await ReplyAsync("user not registered");
                    }
                    else
                    {
                        users[id1] = ((int)users[id1]) - a;
                        users[id2] = ((int)users[id2]) + a;
                        System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed.ToString());
                    }
                }
                else
                {
                    await ReplyAsync("you dont have enough coin");
                }
                await ReplyAsync("you have paid the user " + a + " shiftycoin.");

            }
            [Command("useramount")]
            [Summary("show the amount of shiftycoin a given user id has")]
            public async Task Command_UserCoinAmountAsync(string a)
            {
                //string id = Context.Message.Author.Id.ToString();
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject users = (JObject)parsed["users"];
                await ReplyAsync("this person has " + users[a] + " shiftycoin");
            }
        }
        [Group("business"), Name("business")]
        public class business : ModuleBase
        {
            [Command("create")]
            [Summary("create a business")]
            public async Task Command_BusinessCreateAsync(string newid)
            {
                string id = Context.Message.Author.Id.ToString();
                string bujson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\business.json");
                JObject parsed = JObject.Parse(bujson);
                JObject buid = (JObject)parsed["buid"];
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed1 = JObject.Parse(scjson);
                JObject business = (JObject)parsed1["business"];
                if (bujson.Contains(newid.ToUpper()))
                {
                    await ReplyAsync("the business " + newid.ToUpper() + " already exists.");

                }
                else
                {
                    buid.Property("start").AddAfterSelf(new JProperty(newid.ToUpper(), id));
                    business.Property("start").AddAfterSelf(new JProperty(newid.ToUpper(), 0));
                    System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\business.json", parsed.ToString());
                    System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed1.ToString());
                    await ReplyAsync("the business " + newid.ToUpper() + " has been created.");
                    Console.WriteLine("new business added, " + newid.ToUpper());
                }
            }
            [Command("pay")]
            [Summary("add money to a business' balance")]
            public async Task Command_BusinessPayAsync(string id2, int a)
            {
                string id1 = Context.Message.Author.Id.ToString();
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject users = (JObject)parsed["users"];
                JObject business = (JObject)parsed["business"];
                if ((int)users[id1] >= a && a >= 0)
                {
                    
                    if (!scjson.Contains(id2.ToUpper()))
                    {
                        await ReplyAsync("the business " + id2.ToUpper() + " does not exist.");
                    }
                    else
                    {
                        users[id1] = ((int)users[id1]) - a;
                        business[id2.ToUpper()] = ((int)business[id2.ToUpper()]) + a;
                        System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed.ToString());
                        await ReplyAsync("you have paid " + id2.ToUpper() + " " + a + " shiftycoin.");
                    }
                    
                }
                else
                {
                    await ReplyAsync("you dont have enough coin");
                }
            }
            [Command("balance")]
            [Summary("show the balance of a given business")]
            public async Task Command_BusinessAmountAsync(string a)
            {
                //string id = Context.Message.Author.Id.ToString();
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed = JObject.Parse(scjson);
                JObject business = (JObject)parsed["business"];
                await ReplyAsync("this business has " + (int)business[a.ToUpper()] + " shiftycoin");
            }
            [Command("withdraw")]
            [Summary("withdraw an amount from a business you own. must be registered as business owner.")]
            public async Task Command_BusinessWithdrawAsync(string busid, int a)
            {
                string id = Context.Message.Author.Id.ToString();
                string bujson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\business.json");
                JObject parsed = JObject.Parse(bujson);
                JObject buid = (JObject)parsed["buid"];
                string scjson = System.IO.File.ReadAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json");
                JObject parsed1 = JObject.Parse(scjson);
                JObject business = (JObject)parsed1["business"];
                JObject users = (JObject)parsed1["users"];

                if (id == ((string)buid[busid.ToUpper()]))
                {
                    if ((int)business[busid.ToUpper()] >= a && a >= 0)
                    {
                        users[id] = ((int)users[id]) + a;
                        business[busid.ToUpper()] = ((int)business[busid.ToUpper()]) - a;
                        System.IO.File.WriteAllText(@"C:\Users\sdani\OneDrive\Documents\crackdotnet\src\shiftycoins.json", parsed1.ToString());
                        await ReplyAsync("you have withdrawn " + a + " shiftycoin from your business.");
                    }
                    else
                    {
                        await ReplyAsync("your business does not have enough shiftycoin.");
                    }
                }
                else
                {
                    await ReplyAsync("either this business does not exist, or you do not own it.");
                }
            }
        }
    }
}
