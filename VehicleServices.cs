using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GarageLite
{
    public class VehicleServices : MonoBehaviour
    {
        public Dictionary<string, List<VehicleInfo>> CodesCooldown { get; set; }

        public VehicleDatabase database => MQSPlugin.Instance.VehiclesStoreDatabase;

        void Awake()
        {
            CodesCooldown = new Dictionary<string, List<VehicleInfo>>();
        }

        void Start()
        {

        }

        void OnDestroy()
        {

        }

        
        public bool HasVehicles(string playerId, string name)
        {
            var hasVehicles = database.Data.FirstOrDefault(x => x.PlayerId.Equals(playerId) && x.Name == name);
            
            if (hasVehicles == null)
            {
                return false;
            }
            return true;
        }

        public void RegisterVehicle(string playerId, string name, ushort id, ushort health, ushort battery, ushort fuel, SDG.Unturned.Items items)
        {
            var vehicleinfo = new VehicleInfo
            {
                PlayerId = playerId,
                Name = name,
                VehicleId = id,
                VehicleHealth = health,
                VehicleBattery = battery,
                VehicleFuel = fuel,
                Items = items
            };

            database.AddVehicle(vehicleinfo);
        }

        public void QuitVehicle(string name, string id)
        { 
            database.RetrieveVehicle(name, id);
        }



    }
}
