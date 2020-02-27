using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        public enum eVehicleType
        {
            Car = 1,
            Motorcycle = 2,
            Truck = 3
        }

        public enum eVehicleInGarageStatus
        {
            General = 0,
            Repair = 1,
            Fixed = 2,
            Paid = 3
        }

        private readonly Dictionary<string, VehicleInGarage> r_VehiclesList = new Dictionary<string, VehicleInGarage>();

        public bool CheckIfVehicleIsInGarage(string i_LicenseNum)
        {
            bool isInDictionary = true;
            isInDictionary = r_VehiclesList.ContainsKey(i_LicenseNum);
            if (isInDictionary)
            {
                ChangeCarStatus(i_LicenseNum, eVehicleInGarageStatus.Repair);
            }

            return isInDictionary;
        }

        public void AddNewVehicleToGarage(string i_VehicleOwnersName, string i_VehicleOwnersPhone, string i_ModelName, string i_LicenseNum, Vehicle.eVehicleType i_VehicleType, float i_CurrentEnergySourceAmount, Motorcycle.eLiscenseType i_MotorCycleLicenseType, int i_MotorcycleEngineCapacity, Car.eCarColor i_CarColor, Car.eNumOfDoors i_NumOfDoors, bool i_TrunkIsCooling, float i_TrunkCapacity, Engine.eEnergySourceType i_EnergySource)
        {
            Vehicle newVehicleWaitingForEntrance;
            newVehicleWaitingForEntrance = VehicleCreator.CreateNewVehicle(i_ModelName, i_LicenseNum, i_VehicleType, i_CurrentEnergySourceAmount, i_MotorCycleLicenseType, i_MotorcycleEngineCapacity, i_CarColor, i_NumOfDoors, i_TrunkIsCooling, i_TrunkCapacity, i_EnergySource);
            VehicleInGarage newVehicleInGarage = new VehicleInGarage(newVehicleWaitingForEntrance, i_VehicleOwnersName, i_VehicleOwnersPhone);
            r_VehiclesList.Add(newVehicleInGarage.VehicleInRepair.LicenseNumber, newVehicleInGarage);
        }

        public List<string> DisplayLicenseNumbersByStatusInGarage(eVehicleInGarageStatus i_StatusToDisplay)
        {
            List<string> vehicleLicenseNumList = new List<string>();
            foreach (KeyValuePair<string, VehicleInGarage> vehicle in r_VehiclesList)
            {
                if (i_StatusToDisplay == eVehicleInGarageStatus.General)
                {
                    vehicleLicenseNumList.Add(vehicle.Key);
                }
                else if (vehicle.Value.VehicleStatus == i_StatusToDisplay)
                {
                    vehicleLicenseNumList.Add(vehicle.Key);
                }
            }

            return vehicleLicenseNumList;
        }

        public void RefillEnergyVehicleInGarage(string i_LicenseNum, Engine.eEnergySourceType i_FuelType, float i_EnergyToCharge)
        {
            VehicleInGarage CurrentVehicle = FindVehicleInGarageByLicenseNum(i_LicenseNum);
            r_VehiclesList.TryGetValue(i_LicenseNum, out CurrentVehicle);
            CurrentVehicle.VehicleInRepair.GetVehicleEngine.RefillEnergy(i_EnergyToCharge, i_FuelType);
            CurrentVehicle.VehicleInRepair.UpdateCurrentEnergyPercentage();
        }

        public void ChangeCarStatus(string i_LicenseNum, eVehicleInGarageStatus i_NewStatus)
        {
            VehicleInGarage tempVehicle = FindVehicleInGarageByLicenseNum(i_LicenseNum);
            r_VehiclesList.TryGetValue(i_LicenseNum, out tempVehicle);
            tempVehicle.VehicleStatus = i_NewStatus;
        }

        public void AddNewTireInVehicle(string i_LicenseNum, string i_ManufacturerName, float i_CurrentAirPressure)
        {
            VehicleInGarage vehicleFromList = FindVehicleInGarageByLicenseNum(i_LicenseNum);
            vehicleFromList.VehicleInRepair.AddTireToVehicle(i_ManufacturerName, i_CurrentAirPressure);
        }

        public void InflateTireInVehicleToMax(string i_LicenseNum)
        {
            VehicleInGarage vehicleInGaraeFromList = FindVehicleInGarageByLicenseNum(i_LicenseNum);
            Vehicle vehicleFromList = vehicleInGaraeFromList.VehicleInRepair;
            foreach (Tire singleTire in vehicleFromList.VehicleTires)
            {
                singleTire.InflateTireToMax();
            }
        }

        public string GetVehicleInfo(string i_LicenseNum)
        {
            VehicleInGarage vehicleInGaraeFromList = FindVehicleInGarageByLicenseNum(i_LicenseNum);
            string vehicleInfo = vehicleInGaraeFromList.ToString();
            return vehicleInfo;
        }

        public VehicleInGarage FindVehicleInGarageByLicenseNum(string i_LicenseNum)
        {
            bool isInDictionary = true;
            VehicleInGarage currentVehicle = null;
            isInDictionary = r_VehiclesList.ContainsKey(i_LicenseNum);
            if (!isInDictionary)
            {
                string errorMsg = string.Format(
                                            @"
Vehicle License no. {0} is not in garage
",
                                            i_LicenseNum);
                throw new ArgumentException(errorMsg);
            }

            r_VehiclesList.TryGetValue(i_LicenseNum, out currentVehicle);
            return currentVehicle;
        }

        public int GetNumOfTires(string i_LicenseNum)
        {
            VehicleInGarage currentVehicle = FindVehicleInGarageByLicenseNum(i_LicenseNum);
            int numOfTires = currentVehicle.VehicleInRepair.NumOfTires;
            return numOfTires;
        }

        public class VehicleInGarage
        {
            private string m_OwnerName;
            private string m_OwnerPhone;
            private eVehicleInGarageStatus m_VehicleStatus;
            private Vehicle m_Vehicle;

            public VehicleInGarage(Vehicle i_NewVehicleInGarage, string i_VehicleOwnersName, string i_VehicleOwnersPhone)
            {
                m_OwnerName = i_VehicleOwnersName;
                m_OwnerPhone = i_VehicleOwnersPhone;
                m_Vehicle = i_NewVehicleInGarage;
                m_VehicleStatus = eVehicleInGarageStatus.Repair;
            }

            public Vehicle VehicleInRepair
            {
                get { return m_Vehicle; }
            }

            public eVehicleInGarageStatus VehicleStatus
            {
                get { return m_VehicleStatus; }
                set { m_VehicleStatus = value; }
            }

            public override string ToString()
            {
                Vehicle currentVehicle = null;

                if (m_Vehicle is Car)
                {
                    currentVehicle = (Car)m_Vehicle;
                }
                else if (m_Vehicle is Motorcycle)
                {
                    currentVehicle = (Motorcycle)m_Vehicle;
                }
                else if (m_Vehicle is Truck)
                {
                    currentVehicle = (Truck)m_Vehicle;
                }

                return string.Format(
                                @"The owner name is: {0}
The owner's phone is: {1}
The vehicle status is: {2}
{3}", 
                                m_OwnerName, 
                                m_OwnerPhone,
                                m_VehicleStatus, 
                                currentVehicle.ToString());
            }
        }
    }
}
