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
        string icon = "https://i.imgur.com/4wLZNsz.png";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "glist";
        public string Help => "";
        public string Syntax => throw new NotImplementedException();
        public List<string> Aliases => new List<string> { "garagelist", "vlist" };
        public List<string> Permissions => new List<string> { "garagelite.list" };

        public List<VehicleInfo> GetVehicles(string id)
        {
            return MQSPlugin.Instance.VehiclesStoreDatabase.Data.Where(x => x.PlayerId.Equals(id)).ToList();
        }
 
        public void Execute(IRocketPlayer caller, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            var vehicles = GetVehicles(caller.Id);
            string names = "|";

            foreach (var vehicle in vehicles)
            {
                names += $" {vehicle.Name} |";
            }

            ChatManager.serverSendMessage($"[+] Vehicles: {names} ", Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);

        }
    }
}