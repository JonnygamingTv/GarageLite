using Rocket.API;

namespace GarageLite
{ 
    public class Config : IRocketPluginConfiguration
    {
        public bool AllowCarsSavingUnderwater;

        public void LoadDefaults()
        {
            AllowCarsSavingUnderwater = false;
        }
    }
}
