using System;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        public enum eLiscenseType
        {
            A = 1,
            A1 = 2,
            B1 = 3,
            B2 = 4
        }

        private const int k_NumOfTires = 2;
        private const int k_MaxFuelTrunkCapacity = 6;
        private const float k_MaxTireAirPressure = 30; ////we should check MaxAirPressure when we're adding new Tire
        private const Engine.eEnergySourceType k_FuelType = Engine.eEnergySourceType.Octan96;
        private const float k_MaxBatteryTime = 1.8F;
        private eLiscenseType m_LiscenseType;
        private int m_EngineCapacity;

        public Motorcycle(string i_ModelName, string i_LiscenseNum, Engine.eEnergySourceType i_EnergySource, float i_CurrentEnergySourceAmount, eLiscenseType i_MotorCycleLicenseType, int i_MotorcycleEngineCapacity) : base(i_ModelName, i_LiscenseNum, k_NumOfTires)
        {
            m_LiscenseType = i_MotorCycleLicenseType;
            m_EngineCapacity = i_MotorcycleEngineCapacity;
            if (i_EnergySource == Engine.eEnergySourceType.Electric)
            {
                VehicleEngine = new ElectricalEngine(k_MaxBatteryTime, i_CurrentEnergySourceAmount);
            }
            else
            {
                VehicleEngine = new FuelEngine(k_FuelType, k_MaxFuelTrunkCapacity, i_CurrentEnergySourceAmount);
            }

            UpdateCurrentEnergyPercentage();
        }

        public override void AddTireToVehicle(string i_ManufacturerName, float i_CurrentAirPressure)
        {
            Tire newTire = new Tire(i_ManufacturerName, k_MaxTireAirPressure);
            newTire.UpdateCurrentAirPressure(i_CurrentAirPressure);
            VehicleTires.Add(newTire);
        }

        public override string ToString()
        {
            return string.Format(
                            @"The vehicle type is : Motorcycle
{0}
The liscense type is: {1}
The engine capacity is: {2}",
                            base.ToString(), 
                            m_LiscenseType,
                             m_EngineCapacity);
        }
    }
}
