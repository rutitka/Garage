using System;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        public enum eCarColor
        {
            Grey = 1,
            Blue = 2,
            White = 3,
            Black = 4
        }

        public enum eNumOfDoors
        {
            Two = 1,
            Three = 2,
            Four = 3,
            Five = 4
        }

        private const int k_NumOfTires = 4;
        private const float k_MaxFuelTrunkCapacity = 45;
        private const float k_MaxBatteryTime = 3.2F;
        private const float k_MaxTireAirPressure = 32; ////we should check MaxAirPressure when we're adding new Tire
        private const Engine.eEnergySourceType k_FuelType = Engine.eEnergySourceType.Octan98;
        private eCarColor m_Color;
        private eNumOfDoors m_NumOfDoors;

        public Car(string i_ModelName, string i_LiscenseNum, Engine.eEnergySourceType i_EnergySource, float i_CurrentEnergySourceAmount, eNumOfDoors i_NumOfDoors, eCarColor i_CarColor) : base(i_ModelName, i_LiscenseNum, k_NumOfTires)
        {
            m_Color = i_CarColor;
            m_NumOfDoors = i_NumOfDoors;
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
                @"The type vehicle is : Car
{0}
The fuel type is: {1}
The Car color is: {2}
Num of door in the car is: {3}",
                base.ToString(),
                k_FuelType,
                m_Color,
                m_NumOfDoors);
        }
    }
}
