using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GarageLite
{

    public class Code : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "gadd";
        public string Help => "";
        public string Syntax => throw new NotImplementedException();
        public List<string> Aliases => new List<string> { "garageadd", "vadd" };
        public List<string> Permissions => new List<string> { "garagelite.add" };
        public List<VehicleInfo> GetVehicles(string id)
        {
            return MQSPlugin.Instance.VehiclesStoreDatabase.Data.Where(x => x.PlayerId.Equals(id)).ToList();
        }

        public void Execute(IRocketPlayer caller, string[] args)
        {
            string icon = "https://i.imgur.com/4wLZNsz.png";

            UnturnedPlayer player = (UnturnedPlayer)caller;

            RaycastInfo thingLocated = TraceRay(player, 2048f, RayMasks.VEHICLE);

            InteractableVehicle vehicle = thingLocated.vehicle;

            if (args.Length == 0 || args.Length > 1)
            {
                ChatManager.serverSendMessage($"[?] Usage: /gadd [NAME]", Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);

            }

            if (args.Length == 1)
            {
                var vehiclename = GetVehicles(player.Id);

                foreach (var car in vehiclename)
                {
                    if (args[0].Equals(car.Name))
                    {
                        ChatManager.serverSendMessage("[!] Please choose another car name!", Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                        return;
                    }
                }

                
                if (vehicle != null) 
                {
                    if (player.CSteamID == vehicle.lockedOwner)
                    {
                        if (vehicle.health != 0 && !vehicle.isUnderwater)
                        {
                            MQSPlugin.Instance.VehicleServices.RegisterVehicle(player.Id, args[0], vehicle.id,
                            vehicle.health, vehicle.batteryCharge, vehicle.fuel);

                            VehicleManager.askVehicleDestroy(vehicle);

                            ChatManager.serverSendMessage($"[+] Vehicle {args[0]} [{vehicle.id}] saved", Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                        }
                    }

                    else if (player.CSteamID != vehicle.lockedOwner)
                    {
                        ChatManager.serverSendMessage("[!] Vehicle must be locked to save it in the GarageLite!", Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                    }
                }
                
            }
        }

        public RaycastInfo TraceRay(UnturnedPlayer player, float distance, int masks)
        {
            return DamageTool.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), distance, masks);
        }

    }
}