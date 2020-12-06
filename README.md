# crack.NET

this bot fucking sucks and i advise against its usage

## Building

make sure you have .NET Core 2.1.602 installed, and navigate to the src directory in the cloned repository. build using simply:

```
dotnet build
```
within command prompt, powershell, or whatever command processor you choose.
## Setup

you need to make a few changes before you run this thing. first off, rename _configexample.yml to _config.yml. Add your bot token to the space.
```
prefix: !
tokens:
    discord: your token here
```

then, rename shiftycoinsExample.json to shiftycoins.json. open CommandHandler.cs in a code editor of your choice, and change lines 74, 92, and 101 to match the directory of 
your shiftycoins json.
```
var id = context.Message.Author.Id;
                string scjson = System.IO.File.ReadAllText(@"path to your shiftycoins file");
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
                        System.IO.File.WriteAllText(@"path to your shiftycoin file", parsed.ToString());
                        //Console.WriteLine("upped value of user " + id.ToString());

                    }
                    else
                    {
                        users.Property("start").AddAfterSelf(new JProperty(id.ToString(), 1));
                        totalareas["totalcoin"] = ((int)totalareas["totalcoin"]) + 1;
                        System.IO.File.WriteAllText(@"path to your shiftycoin file", parsed.ToString());
                        Console.WriteLine("new user added, " + id.ToString());
                    }
                }
```
you will need to repeat the build step after you make this change.

## Running 
navigate to your src directory, and simply use:
```
dotnet run
```
inside a command processor of your choice.

## Contribution
why??? why would you want to contribute to this shit??? anyway if you really want to, you're welcome to open an issue, and if you have any idea how to fix your issue 
say how you would in the issue.
