using System.Collections.Generic;

namespace GarageLite
{
    public class VehicleDatabase
    {
        private DataStorage<List<VehicleInfo>> DataStorage { get; set; }
        public List<VehicleInfo> Data { get; private set; }
        public VehicleDatabase()
        {
            DataStorage = new DataStorage<List<VehicleInfo>>(MQSPlugin.Instance.Directory, "Vehicles.json");
        }
        public void Reload()
        {
            Data = DataStorage.Read();
            if (Data == null)
            {
                Data = new List<VehicleInfo>();
                DataStorage.Save(Data);
            }
        }

        public void AddVehicle(VehicleInfo vehicle)
        {
            Data.Add(vehicle);
            DataStorage.Save(Data);
        }

        public bool RetrieveVehicle(string name, string id)
        {
            var flag = Data.RemoveAll(x => x.Name.Equals(name) && x.PlayerId.Equals(id));
            if (flag > 0)
            {
                DataStorage.Save(Data);
                return true;
            }
            return false;
        }
    }
}
