using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GarageLite
{

    public class Vretrieve : IRocketCommand
    {
        string icon = "https://i.imgur.com/4wLZNsz.png";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "gretrieve";
        public string Help => "";
        public string Syntax => throw new NotImplementedException();
        public List<string> Aliases => new List<string> { "garageretrieve", "vretrieve" };
        public List<string> Permissions => new List<string> { "garagelite.retrieve" };
        public VehicleInfo RetrieveVehicle(string name, string ownerId)
        {
            var find = MQSPlugin.Instance.VehiclesStoreDatabase.Data.FirstOrDefault(x => x.PlayerId == ownerId && x.Name == name);
            return find;
        }
        public void Execute(IRocketPlayer caller, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (args.Length == 0)
            {
                ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VretrieveUsage"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                return;
            }

            if (args.Length == 1)
            {
                var vehicle = args[0];
                
                if (!MQSPlugin.Instance.VehicleServices.HasVehicles(player.Id, vehicle))
                {
                    ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VehicleNotFound", vehicle), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                }

                if (MQSPlugin.Instance.VehicleServices.HasVehicles(player.Id, vehicle))
                {
                    if (vehicle != null)
                    {
                        string id = RetrieveVehicle(vehicle, player.Id).VehicleId.ToString();

                        var car = Convert.ToUInt16(id);

                        ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VehicleRetrieved", vehicle), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);

                        Vector3 point = player.Player.transform.position + player.Player.transform.forward * 6 + player.Player.transform.up * 6;

                        InteractableVehicle MyCar = 
                        VehicleManager.spawnLockedVehicleForPlayerV2(car, point, player.Player.transform.rotation, player.Player);

                        string health = RetrieveVehicle(vehicle, player.Id).VehicleHealth.ToString();
                        string battery = RetrieveVehicle(vehicle, player.Id).VehicleBattery.ToString();
                        string fuel = RetrieveVehicle(vehicle, player.Id).VehicleFuel.ToString();

                        VehicleManager.sendVehicleHealth(MyCar, Convert.ToUInt16(health));
                        VehicleManager.sendVehicleBatteryCharge(MyCar, Convert.ToUInt16(battery));
                        VehicleManager.sendVehicleFuel(MyCar, Convert.ToUInt16(fuel));

                        MQSPlugin.Instance.VehicleServices.QuitVehicle(vehicle, player.Id);
                    }
                }
            }
        }
    }
}