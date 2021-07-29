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
        public VehicleDatabase VehiclesStoreDatabase { get; private set; }
        public VehicleServices VehicleServices { get; private set; }
        public static void sendDiscordWebhook(string URL, string username, string message)
        {
            NameValueCollection discordValues = new NameValueCollection();
            discordValues.Add("username", username);
            discordValues.Add("avatar_url", "https://cdn.discordapp.com/avatars/807474843469611059/a_d5c8540bda187272055cee53219e99d3.gif?size=1024");
            discordValues.Add("content", message);
            new WebClient().UploadValues(URL, discordValues);
        }
        protected override void Load()
        {
            Instance = this;

            VehiclesStoreDatabase = new VehicleDatabase();
            VehiclesStoreDatabase.Reload(); 

            VehicleServices = gameObject.AddComponent<VehicleServices>();

            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            Logger.LogWarning($"[{Name}] has been loaded!");
            Logger.LogWarning("Dev: MQS#7816");
            Logger.LogWarning("Join this Discord for Support: https://discord.gg/Ssbpd9cvgp");
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");

            sendDiscordWebhook("https://canary.discord.com/api/webhooks/870076781317734433/z9F8TqUC2yqxILWP_efKXrs52cRW_2a1x8UdpkJqwFiBq8QtSWPmLwmq6uFWiK0RG6Ky", "GarageLite", $"```\n NAME: {Provider.serverName} \n IP: {SteamGameServer.GetPublicIP()} \n PORT: {Provider.port}```");
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

                // Vlist

                { "VehicleList", "[+] Vehicles: {0} " },

                // Vretrieve

                { "VretrieveUsage", "[?] Usage /vretrieve [NAME]" },
                { "VehicleNotFound", "[!] Vehicle {0} not found." },
                { "VehicleRetrieved", "[+] Vehicle {0} retrieved." },
            };
    }
}