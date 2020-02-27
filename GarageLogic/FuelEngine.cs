using System;

namespace Ex03.GarageLogic
{
    public class FuelEngine : Engine
    {
        private eEnergySourceType m_EnergyType;

        public FuelEngine(eEnergySourceType i_EnergySource, float i_MaxEnergySourceCapacity, float i_CurrentEnergySourceAmount) : base(i_MaxEnergySourceCapacity, i_CurrentEnergySourceAmount)
        {
            m_EnergyType = i_EnergySource;
        }

        public override void RefillEnergy(float i_EnergyNeed, eEnergySourceType i_EnergyType)
        {
            float newCapacity = m_CurrentEnergyCapacity + i_EnergyNeed;
            if (i_EnergyType != m_EnergyType)
            {
                string errorMsg = string.Format(
                                            @"
Operation faild! your engine energy source is {0}
", 
                                            m_EnergyType);
                throw new ArgumentException(errorMsg);
            }
            else
            {
                if (newCapacity > m_MaxEnergyCapacity || i_EnergyNeed < 0) 
                {
                    string errorMsg = string.Format(
                                                @"
Refuel amount limits are between {0} to {1}!
", 
                                                0, 
                                                m_MaxEnergyCapacity);
                    throw new ValueOutOfRangeException(0, m_MaxEnergyCapacity - m_CurrentEnergyCapacity, errorMsg);
                }
                else
                {
                    m_CurrentEnergyCapacity = newCapacity;
                }
            }
        }

        public override string ToString()
        {
            return string.Format(
                            @"The energy type is: Fuel engine
The Fuel type is:  {0}  
The max energy capacity is:  {1}
The current energy capacity is: {2}",
                            m_EnergyType,
                            m_MaxEnergyCapacity,
                            m_CurrentEnergyCapacity);
        }
    }
}
