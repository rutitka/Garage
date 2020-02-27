using System;

namespace Ex03.GarageLogic
{
    public class ElectricalEngine : Engine
    { 
        public ElectricalEngine(float i_MaxEnergySourceCapacity, float i_CurrentEnergySourceAmount) : base(i_MaxEnergySourceCapacity, i_CurrentEnergySourceAmount)
        {
        }

        public override void RefillEnergy(float i_EnergyToFill, eEnergySourceType i_EnergyType)
        {
            float newCapacity = m_CurrentEnergyCapacity + i_EnergyToFill;
            if (i_EnergyType != eEnergySourceType.Electric)
            {
                string errorMsg = string.Format(
                                            @"
Operation faild! you tried charge Electric battery with {0}
",
i_EnergyType);
                throw new ArgumentException(errorMsg);
            }
            else
            {
                if (newCapacity > m_MaxEnergyCapacity || i_EnergyToFill < 0)
                {
                    string errorMsg = string.Format(
                                        @"
Battery charging limits are between {0} to {1}!
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
                            @"The energy type is: Electrical engine
Energy source type: Electrical battery
The current energy capacity is: {0}
The max energy capacity is:  {1}",
m_CurrentEnergyCapacity,
m_MaxEnergyCapacity);
        }
    }
}
