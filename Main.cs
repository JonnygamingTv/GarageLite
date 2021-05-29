using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Logger = Rocket.Core.Logging.Logger;

namespace GarageLite
{
    public class MQSPlugin : RocketPlugin<Config>
    {
        public static MQSPlugin Instance;
        public VehicleDatabase VehiclesStoreDatabase { get; private set; }
        public VehicleServices VehicleServices { get; private set; }
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

            };
    }
}