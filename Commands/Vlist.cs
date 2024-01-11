using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GarageLite
{

    public class Vlist : IRocketCommand
    {

        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "glist";
        public string Help => "";
        public string Syntax => throw new NotImplementedException();
        public List<string> Aliases => new List<string> { "garagelist", "vlist", "glist", "vehiclelist" };
        public List<string> Permissions => new List<string> { "garagelite.list" };

        public List<VehicleInfo> GetVehicles(string id)
        {
            return MQSPlugin.Instance.VehiclesStoreDatabase.Data.Where(x => x.PlayerId.Equals(id)).ToList();
        }
 
        public void Execute(IRocketPlayer caller, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            List<VehicleInfo> vehicles = GetVehicles(caller.Id);
            string names = "|";

            foreach (var vehicle in vehicles)
            {
                names += $" {vehicle.Name} |";
            }

            ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VehicleList", names,vehicles.Count), Color.white, null, player.SteamPlayer(), EChatMode.SAY, MQSPlugin.Instance.Configuration.Instance.icon, true);
        }
    }
}