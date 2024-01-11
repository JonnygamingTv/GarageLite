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
        public List<string> Aliases => new List<string> { "garageadd", "vadd", "gadd", "vehicleadd" };
        public List<string> Permissions => new List<string> { "garagelite.add" };
        public List<VehicleInfo> GetVehicles(string id)
        {
            return MQSPlugin.Instance.VehiclesStoreDatabase.Data.Where(x => x.PlayerId.Equals(id)).ToList();
        }

        public void Execute(IRocketPlayer caller, string[] args)
        {
            string icon = MQSPlugin.Instance.Configuration.Instance.icon;

            UnturnedPlayer player = (UnturnedPlayer)caller;

            

            InteractableVehicle vehicle = null;
            if (player.CurrentVehicle)
            {
                vehicle = player.CurrentVehicle;
            }else
            {
                RaycastInfo thingLocated = TraceRay(player, 2048f, RayMasks.VEHICLE);
                if (thingLocated != null) if (thingLocated.vehicle) vehicle = thingLocated.vehicle;
            }

            if (args.Length == 0 || args.Length > 1)
            {
                ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VaddUsage"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);

            }

            if (args.Length == 1)
            {
                List<VehicleInfo> vehiclename = GetVehicles(player.Id);
                if (vehiclename.Count > MQSPlugin.Instance.Configuration.Instance.MaxGarage)
                {
                    ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VaddFull"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                    return;
                }
                foreach (var car in vehiclename)
                {
                    if (args[0].Equals(car.Name))
                    {
                        ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VaddAnotherCarName"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                        return;
                    }
                }
                if (vehicle != null) 
                {
                    if (player.CSteamID == vehicle.lockedOwner)
                    {
                        {
                            if (vehicle.health != 0 && (!vehicle.isUnderwater || MQSPlugin.Instance.Configuration.Instance.AllowCarsSavingUnderwater))
                            {
                                /*int x = vehicle.tires.Length;
                                bool[] Tires = new bool[x];
                                for (byte i = 0; i < x; i++) Tires[i] = vehicle.tires[i].isAlive;
                                x = vehicle.trunkItems.getItemCount();
                                ushort[] trunkItems = new ushort[x];
                                for (byte i = 0; i < x; i++) trunkItems[i] = vehicle.trunkItems.getItem(i).item.id;*/
                                vehicle.forceRemoveAllPlayers();

                                MQSPlugin.Instance.VehicleServices.RegisterVehicle(player.Id, args[0], vehicle.id,
                                vehicle.health, vehicle.batteryCharge, vehicle.fuel, vehicle.trunkItems);

                                VehicleManager.askVehicleDestroy(vehicle);
                                UnityEngine.Object.DestroyImmediate(vehicle,true);

                                ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VehicleSaved", args[0], vehicle.id), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);

                            }
                        }
                    }
                    else
                    {
                        ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VaddMustBeLocked"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                    }
                } else
                {
                    ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VaddNotFound"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, icon, true);
                }
                
            }
        }

        public RaycastInfo TraceRay(UnturnedPlayer player, float distance, int masks)
        {
            return DamageTool.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), distance, masks);
        }

    }
}