using Rocket.API;

namespace GarageLite
{ 
    public class Config : IRocketPluginConfiguration
    {
        public bool AllowCarsSavingUnderwater;
        public int MaxGarage;
        public string icon;
        public string DiscordWebHook;
        public string DiscordWebHookIcon;
        public string DiscordWebHookName;
        public MQSPlugin.DatabaseType Database;

        public void LoadDefaults()
        {
            AllowCarsSavingUnderwater = false;
            MaxGarage = 3;
            icon = "https://i.imgur.com/4wLZNsz.png";
            DiscordWebHook = "https://canary.discord.com/api/webhooks/111/11";
            DiscordWebHookIcon = "https://cdn.discordapp.com/avatars/807474843469611059/a_d5c8540bda187272055cee53219e99d3.gif?size=1024";
            DiscordWebHookName = "GarageLite";
            Database = MQSPlugin.DatabaseType.JSON;
        }

        
    }
}
