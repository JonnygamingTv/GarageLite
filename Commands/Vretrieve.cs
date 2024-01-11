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
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "gretrieve";
        public string Help => "";
        public string Syntax => throw new NotImplementedException();
        public List<string> Aliases => new List<string> { "garageretrieve", "vretrieve", "gretrieve", "vehicleretrieve" };
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
                ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VretrieveUsage"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, MQSPlugin.Instance.Configuration.Instance.icon, true);
                return;
            }

            if (args.Length == 1)
            {
                string vehicle = args[0];
                
                if (!MQSPlugin.Instance.VehicleServices.HasVehicles(player.Id, vehicle))
                {
                    ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VehicleNotFound", vehicle), Color.white, null, player.SteamPlayer(), EChatMode.SAY, MQSPlugin.Instance.Configuration.Instance.icon, true);
                }

                if (MQSPlugin.Instance.VehicleServices.HasVehicles(player.Id, vehicle))
                {
                    if (vehicle != null)
                    {
                        Vector3 point = player.Player.transform.position + player.Player.transform.forward * 6 + player.Player.transform.up * 6;

                        VehicleInfo Vinfo = RetrieveVehicle(vehicle, player.Id);
                        ushort id = Vinfo.VehicleId;
                        InteractableVehicle MyVehicle = VehicleManager.spawnLockedVehicleForPlayerV2(id, point, player.Player.transform.rotation, player.Player);//InteractableVehicle MyVehicle = new InteractableVehicle();
                        MyVehicle.id = id;
                        MyVehicle.transform.position = point;
                        MyVehicle.health = Vinfo.VehicleHealth;
                        MyVehicle.batteryCharge = Vinfo.VehicleBattery;
                        MyVehicle.fuel = Vinfo.VehicleFuel;
                        /*Items items = new Items(7);
                        for (int i = 0; i < Vinfo.Items.Length; i++)
                        {
                            Item it = new Item(Vinfo.Items[i],true);
                            items.tryAddItem(it);
                        }
                        MyVehicle.trunkItems = items;
                        for(int i=0;i<Vinfo.Tires.Length;i++)
                            MyVehicle.tires[i].isAlive = Vinfo.Tires[i];*/
                        MyVehicle.trunkItems = Vinfo.Items;
                        //VehicleManager.vehicles.Add(MyVehicle);
                        MyVehicle.updateEngine();
                        MyVehicle.updateVehicle();
                        
                        //VehicleManager.sendVehicleHealth(MyCar, health);
                        //VehicleManager.sendVehicleBatteryCharge(MyCar, battery);
                        //VehicleManager.sendVehicleFuel(MyCar, fuel);
                        //MyCar.trunkItems = Vinfo.Items;

                        MQSPlugin.Instance.VehicleServices.QuitVehicle(vehicle, player.Id);
                        ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("VehicleRetrieved", vehicle), Color.white, null, player.SteamPlayer(), EChatMode.SAY, MQSPlugin.Instance.Configuration.Instance.icon, true);
                    }
                }
            }
        }
    }
}