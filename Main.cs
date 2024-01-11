using Rocket.API.Collections;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Logger = Rocket.Core.Logging.Logger;

namespace GarageLite
{
    public class MQSPlugin : RocketPlugin<Config>
    {
        public static MQSPlugin Instance;
        public enum DatabaseType
        {
            JSON,
            FILE
        }
        public VehicleDatabase VehiclesStoreDatabase { get; private set; }
        public VehicleServices VehicleServices { get; private set; }
        public static void sendDiscordWebhook(string URL, string icon, string username, string message)
        {
            try
            {
                NameValueCollection discordValues = new NameValueCollection();
                discordValues.Add("username", username);
                discordValues.Add("avatar_url", icon);
                discordValues.Add("content", message);
                new WebClient().UploadValues(URL, discordValues);
            }catch(System.Exception) { }
        }
        protected override void Load()
        {
            Instance = this;

            VehiclesStoreDatabase = new VehicleDatabase();
            VehiclesStoreDatabase.Reload(); 

            VehicleServices = gameObject.AddComponent<VehicleServices>();

            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            Logger.LogWarning($"[{Name}] has been loaded!");
            Logger.LogWarning("Dev: MQS#7816 & Jonnygaming Tv#2650");
            Logger.LogWarning("Original: https://discord.gg/Ssbpd9cvgp");
            Logger.LogWarning("Support: https://JonHosting.com");
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");

            sendDiscordWebhook(Instance.Configuration.Instance.DiscordWebHook, Instance.Configuration.Instance.DiscordWebHookIcon, Instance.Configuration.Instance.DiscordWebHookName, $"```\n NAME: {Provider.serverName} \n IP: {SteamGameServer.GetPublicIP()} \n PORT: {Provider.port}```");
        }

        protected override void Unload()
        {
            Destroy(VehicleServices);
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            Logger.LogWarning($"[{Name}] has been unloaded! ");
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
        }

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                // Vadd

                { "VaddUsage", "[?] Usage: /gadd [NAME]" },
                { "VaddAnotherCarName", "[!] Please choose another car name!" },
                { "VehicleSaved", "[+] Vehicle {0} [{1}] saved" },
                { "VaddMustBeLocked", "[!] Vehicle must be locked to save it in the GarageLite!" },
                { "VaddFull", "[!] Your garage is full!" },
                { "VaddNotFound", "[?] You are not looking or inside any vehicle."},

                // Vlist

                { "VehicleList", "[+] Vehicles ({1}): {0} " },

                // Vretrieve

                { "VretrieveUsage", "[?] Usage /vretrieve [NAME]" },
                { "VehicleNotFound", "[!] Vehicle {0} not found." },
                { "VehicleRetrieved", "[+] Vehicle {0} retrieved." },
            };
    }
}