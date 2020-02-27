using System;

namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        public enum eEnergySourceType
        {
            Soler = 1,
            Octan95 = 2,
            Octan96 = 3,
            Octan98 = 4,
            Electric = 5,
            FuelGeneral = 6
        }

        protected float m_CurrentEnergyCapacity;
        protected float m_MaxEnergyCapacity;

        public Engine(float i_MaxEnergySourceCapacity, float i_CurrentEnergySourceAmount)
        {
            m_MaxEnergyCapacity = i_MaxEnergySourceCapacity;
            UpdateCurrentEnergyCapacity(i_CurrentEnergySourceAmount);
        }

        public float MaxEnergyCapacity
        {
            get { return m_MaxEnergyCapacity; }
            set { m_MaxEnergyCapacity = value; }
        }

        public float CalculateEnergyPercentage()
        {
            float energyPercentage = (m_CurrentEnergyCapacity / m_MaxEnergyCapacity) * 100;
            return energyPercentage;
        }

        public void UpdateCurrentEnergyCapacity(float i_CurrentEnergyCapacity)
        {
            if (i_CurrentEnergyCapacity > m_MaxEnergyCapacity || i_CurrentEnergyCapacity < 0) 
            {
                string errorMsg = string.Format(
                                            @"
Energy source capacity limits are between {0} to {1}!
",
                                            0,
                                            m_MaxEnergyCapacity);
                throw new ValueOutOfRangeException(0, m_MaxEnergyCapacity, errorMsg);
            }
            else
            {
                m_CurrentEnergyCapacity = i_CurrentEnergyCapacity;
            }
        }

        public abstract void RefillEnergy(float i_EnergyNeed, eEnergySourceType i_EnergyType);
    }
}
