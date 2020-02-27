using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    public class GarageManagerSoftware
    {
        private Garage m_MyGarage = new Garage();

        public void RunGarageManagerSoftware()
        {
            string menuMsg = string.Format(@"Welcome To Garage Manager Software
please choose your request Activity:

To enter new vehicle into the garage ........... press 1 
To show vehicle lisencing list ................. press 2
To change vehicle status ....................... press 3
To inflate vehicle tires to max air pressure ... press 4
To refuel vehicle .............................. press 5
To charge electrical vehicle ................... press 6
To show vehicle full details ................... press 7
To Exit ........................................ press 8");
            RunGarageSoftwareMenu(menuMsg);
        }

        public void RunGarageSoftwareMenu(string i_MenuMsg)
        {
            bool userChoseToExit = false;
            do
            {   
                string userInput = null;
                do
                {
                    Console.Clear();
                    PrintUserInputMsgToConsoleAndGetNewInput(ref userInput, i_MenuMsg);
                }
                while (!IsLegalUserChoice("^[1-8]+$", userInput));

                try
                {
                    switch (userInput)
                    {
                        case "1":
                            AssembleNewVehicleInfoAndEnterToGarage();
                            break;
                        case "2":
                            DisplayVehicleLicenseNumbers();
                            break;
                        case "3":
                            ChangeVehicleGarageStatus();
                            break;
                        case "4":
                            GetLicencingNumAndInflateVehicleTires();
                            break;
                        case "5":
                            LoadEnergyVehicle(Engine.eEnergySourceType.FuelGeneral);
                            break;
                        case "6":
                            LoadEnergyVehicle(Engine.eEnergySourceType.Electric);
                            break;
                        case "7":
                            DisplayVehicleInfo();
                            break;
                        case "8":
                            userChoseToExit = true;
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ValueOutOfRangeException vEx)
                {
                    Console.WriteLine(vEx.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Parsing error!");
                }
                finally
                {
                    System.Console.WriteLine("{0}Press any key to continue...", Environment.NewLine);
                    System.Console.ReadKey();
                }
            }
            while (!userChoseToExit);
        }

        public void AssembleNewVehicleInfoAndEnterToGarage()
        {
            string licenseNum = null;
            GetLicenseNum(ref licenseNum);
            bool isVehicleAlreadyInGarage = m_MyGarage.CheckIfVehicleIsInGarage(licenseNum);
            if (isVehicleAlreadyInGarage == false)
            {
                string modelName = null, vehicleOwnersPhone = null, ownersName = null;
                Vehicle.eVehicleType vehicleType;
                Engine.eEnergySourceType energySourceType = 0;
                Motorcycle.eLiscenseType motorcycleLicenseType = 0;
                int motorcycleEngineCapacity = 0;
                Car.eCarColor carColor = 0;
                Car.eNumOfDoors carNumOfDoors = 0;
                float currentEnergyAmount = 0;
                bool isTrunkCooling = false;
                float cargoCapacity = 0;
                getValidName(ref ownersName, "owner");
                GetVehicleOwnersPhone(ref vehicleOwnersPhone);
                getValidName(ref modelName, "model");
                GetVehicleType(out vehicleType);
                if (vehicleType != Vehicle.eVehicleType.Truck)
                {
                    GetEnergySourceType(out energySourceType);
                }

                GetCurrenEnergyAmount(out currentEnergyAmount);

                switch (vehicleType)
                {
                    case Vehicle.eVehicleType.Motorcycle:
                        GetMotorcycleLicenseType(out motorcycleLicenseType);
                        GetMotorcycleEngineCapacity(out motorcycleEngineCapacity);
                        break;
                    case Vehicle.eVehicleType.Car:
                        GetCarColor(out carColor);
                        GetNumOfDoors(out carNumOfDoors);
                        break;
                    case Vehicle.eVehicleType.Truck:
                        isTrunkCooling = AskUserIfTrunkIsCooling();
                        GetCargoCapacity(out cargoCapacity);
                        break;
                }

                m_MyGarage.AddNewVehicleToGarage(ownersName, vehicleOwnersPhone, modelName, licenseNum, vehicleType, currentEnergyAmount, motorcycleLicenseType, motorcycleEngineCapacity, carColor, carNumOfDoors, isTrunkCooling, cargoCapacity, energySourceType);

                try
                {
                    InsertNewVehicleEnergyAmountAndTiresInfo(licenseNum, currentEnergyAmount);
                }
                catch (ValueOutOfRangeException vaEx)
                {
                    Console.WriteLine(vaEx.Message);
                    GetCurrenEnergyAmount(out currentEnergyAmount);
                    InsertNewVehicleEnergyAmountAndTiresInfo(licenseNum, currentEnergyAmount);
                }

                Console.WriteLine("Vehicle entered succesfully!");
            }
            else
            {
                Console.WriteLine("vehicle already exist in garage! vehicle status is now 'repair'");
            }
        }

        public void InsertNewVehicleEnergyAmountAndTiresInfo(string i_LicenseNum, float i_CurrentEnergyAmount)
        {
            int numOfTires = m_MyGarage.GetNumOfTires(i_LicenseNum);
            for (int numOfLoop = 0; numOfLoop < numOfTires; numOfLoop++)
            {
                try
                {
                    AskForTireInfoAndUpdateInVehicleInfo(i_LicenseNum, numOfTires, numOfLoop + 1);
                }
                catch (ValueOutOfRangeException valEx)
                {
                    numOfLoop--;
                    Console.WriteLine(valEx.Message);
                }
            }
        }

        public void AskForTireInfoAndUpdateInVehicleInfo(string i_LicenseNum, int i_NumOfTires, int i_NumOfTire)
        {
            string manufacturerName = null;
            int numOfTire = i_NumOfTire;
            string airPressureToCheck = null;
            float currentAirPressure;
            string msg1 = string.Format(@"Please Enter Tire {0} manufacturer name: ", numOfTire);
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref manufacturerName, msg1);
            }
            while (!Regex.IsMatch(manufacturerName, "^[a-z, ,A-Z]+$") || manufacturerName.Length > 20);

            string msg2 = string.Format(@"Please Enter Tire {0} current air pressure:", numOfTire);
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref airPressureToCheck, msg2);
            }
            while (!Regex.IsMatch(airPressureToCheck, "^[0-9]+$") || airPressureToCheck.Length > 2);

            float.TryParse(airPressureToCheck, out currentAirPressure);
            m_MyGarage.AddNewTireInVehicle(i_LicenseNum, manufacturerName, currentAirPressure);
        }

        public void DisplayVehicleLicenseNumbers()
        {
            string userInput = null;
            int vehicleStatusAsInt;
            List<string> LicenseNumsList;
            Garage.eVehicleInGarageStatus VehicleStatusToFilterBy;
            string menuMsg = string.Format(@"
To display general licensing num list ....... press 0
To display vehicles under 'repair' status ... press 1
To display vehicles under 'Fixed' status .... press 2
To display vehicles under 'paid' status ..... press 3");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userInput, menuMsg);
            }
            while (!IsLegalUserChoice("^[0-3]+$", userInput));

            int.TryParse(userInput, out vehicleStatusAsInt);
            VehicleStatusToFilterBy = (Garage.eVehicleInGarageStatus)vehicleStatusAsInt;
            LicenseNumsList = m_MyGarage.DisplayLicenseNumbersByStatusInGarage(VehicleStatusToFilterBy);
            PrintLicenseNumsList(LicenseNumsList, VehicleStatusToFilterBy);
        }

        public void ChangeVehicleGarageStatus()
        {
            string LicenseNum = null;
            int statusAsInt;
            Garage.eVehicleInGarageStatus vehicleNewStatus = 0;
            string userRequestedStatus = null;
            string msg = string.Format(@"Choose Vehicle new status:
Repair ......... press 1
Fixed .......... press 2
Paid ........... Press 3");
            GetLicenseNum(ref LicenseNum);
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userRequestedStatus, msg);
            }
            while (!IsLegalUserChoice("^[1-3]+$", userRequestedStatus));

            int.TryParse(userRequestedStatus, out statusAsInt);
            vehicleNewStatus = (Garage.eVehicleInGarageStatus)statusAsInt;
            m_MyGarage.ChangeCarStatus(LicenseNum, vehicleNewStatus);
        }

        public void LoadEnergyVehicle(Engine.eEnergySourceType i_EnergyType)
        {
            string LicenseNum = null;
            string userChoice = null;
            string fuelAmountInput = null;
            int convertStringToInt;
            float fuelAmountToRefuel = 0;
            Engine.eEnergySourceType energySourceToCharge;
            GetLicenseNum(ref LicenseNum);
            if (i_EnergyType != Engine.eEnergySourceType.Electric)
            {
                string msg1 = string.Format(@"please choose Fuel type:
Soler ......... Press 1
Octan 95 ...... Press 2
Octan 96 ...... Press 3
Octan 98 ...... Press 4");
                do
                {
                    PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg1);
                }
                while (!IsLegalUserChoice("^[1-3]+$", userChoice));

                int.TryParse(userChoice, out convertStringToInt);
                energySourceToCharge = (Engine.eEnergySourceType)convertStringToInt;
            }
            else
            {
                energySourceToCharge = i_EnergyType;
            }

            string msg2 = string.Format(@"Enter fuel amount to refuel: ");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref fuelAmountInput, msg2);
            }
            while (!Regex.IsMatch(fuelAmountInput, "^[0-9]+$"));

            float.TryParse(fuelAmountInput, out fuelAmountToRefuel);
            m_MyGarage.RefillEnergyVehicleInGarage(LicenseNum, energySourceToCharge, fuelAmountToRefuel);
        }

        public void DisplayVehicleInfo()
        {
            string requestedLicenseNum = null;
            GetLicenseNum(ref requestedLicenseNum);
            m_MyGarage.GetVehicleInfo(requestedLicenseNum);
            string vehicleInfo = m_MyGarage.GetVehicleInfo(requestedLicenseNum);
            Console.WriteLine(string.Format(
                                        @"Vehicle Info:
{0}", 
                                        vehicleInfo));
        }

        public void GetLicencingNumAndInflateVehicleTires()
        {
            string LicenseNum = null;
            GetLicenseNum(ref LicenseNum);
            m_MyGarage.InflateTireInVehicleToMax(LicenseNum);
        }

        public void PrintLicenseNumsList(List<string> i_LicensingListToPrint, Garage.eVehicleInGarageStatus i_VehicleStatusToFilterBy)
        {
            if (i_LicensingListToPrint.Count == 0)
            {
                Console.WriteLine("There are no vehicles under status " + "'" + i_VehicleStatusToFilterBy + "' in garage");
            }

            Console.WriteLine(i_VehicleStatusToFilterBy + " Vehicles in garage licensing list");
            foreach (string LicenseNum in i_LicensingListToPrint)
            {
                Console.WriteLine(LicenseNum);
            }
        }

        public void getValidName(ref string o_GeneralName, string i_StringToPrint)
        {
            do
            {
                if (o_GeneralName != null)
                {
                    WrongInputMsg();
                }

                Console.WriteLine(string.Format(@"Please insert {0} name(letters only) :", i_StringToPrint));
                o_GeneralName = Console.ReadLine();
            }
            while (!Regex.IsMatch(o_GeneralName, "^[a-z, ,A-Z]+$") || o_GeneralName.Length > 20);
        }

        public void GetVehicleOwnersPhone(ref string o_VehicleOwnersPhone)
        {
            do
            {
                if (o_VehicleOwnersPhone != null)
                {
                    WrongInputMsg();
                }

                Console.WriteLine("Please insert vehicle owners Phone (10 digits) :");
                o_VehicleOwnersPhone = Console.ReadLine();
            }
            while (!Regex.IsMatch(o_VehicleOwnersPhone, "^[0-9]+$") || o_VehicleOwnersPhone.Length != 10);
        }

        public void GetLicenseNum(ref string o_LiscenseNum)
        {
            do
            {
                if (o_LiscenseNum != null)
                {
                    WrongInputMsg();
                }

                Console.WriteLine("Please insert vehicle Liscense number (between 3 to 10 characters) :");
                o_LiscenseNum = Console.ReadLine();
            }
            while (o_LiscenseNum.Length > 10 || o_LiscenseNum.Length < 3);
        }

        public void GetVehicleType(out Vehicle.eVehicleType o_VehicleType)
        {
            string userChoice = null;
            int userChoiceAsInt;
            string msg = string.Format(@"Please choose the vehicle type:
For private car .... press 1
For Motorcycle ..... press 2
For Truck .......... press 3");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
            }
            while (!IsLegalUserChoice("^[1-3]+$", userChoice));
            int.TryParse(userChoice, out userChoiceAsInt);
            o_VehicleType = (Vehicle.eVehicleType)userChoiceAsInt;
        }

        public void GetEnergySourceType(out Engine.eEnergySourceType o_EnergySourceType)
        {
            string userChoice = null;
            int userChoiceAsInt;
            string msg = string.Format(@"Please choose the Energy source type:
For Electric engine .... press 1
For Fuel engine    ..... press 2");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
            }
            while (!IsLegalUserChoice("^[1-2]+$", userChoice));

            int.TryParse(userChoice, out userChoiceAsInt);
            if (userChoiceAsInt == 1)
            {
                o_EnergySourceType = Engine.eEnergySourceType.Electric;
            }
            else
            {
                o_EnergySourceType = Engine.eEnergySourceType.FuelGeneral;
            }
        }

        public void GetCurrenEnergyAmount(out float o_CurrentEnergyAmount)
        {
            string msg = string.Format(@"Please enter current energy amount : ");
            string userChoice = null;
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
                float.Parse(userChoice);
            }
            while (!Regex.IsMatch(userChoice, "[0-9]+$") && userChoice.Length > 3);

            float.TryParse(userChoice, out o_CurrentEnergyAmount);
        }

        public void GetMotorcycleLicenseType(out Motorcycle.eLiscenseType o_MotorcycleLicenseType)
        {
            string userChoice = null;
            int userChoiceAsInt;
            string msg = string.Format(@"Please choose your Motorcycle license type:
A .... press 1
A1 ... press 2
B1 ... Press 3
B2 ... Press 4");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
            }
            while (!IsLegalUserChoice("^[1-4]+$", userChoice));

            int.TryParse(userChoice, out userChoiceAsInt);
            o_MotorcycleLicenseType = (Motorcycle.eLiscenseType)userChoiceAsInt;
        }

        public void GetMotorcycleEngineCapacity(out int o_MotorcycleEngineCapacity)
        {
            string userChoice = null;
            string msg = string.Format(@"Please enter Motorcycle engine capacity (at least 100):");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
                if(int.TryParse(userChoice, out o_MotorcycleEngineCapacity) == false)
                {
                    throw new FormatException();
                }    
            }
            while (!Regex.IsMatch(userChoice, "[0-9]+$") || userChoice.Length < 3);
        }

        public void GetCarColor(out Car.eCarColor o_CarColor)
        {
            string userChoice = null;
            int userChoiceAsInt;
            string msg = string.Format(@"Please choose your Car color:
Grey .... press 1
Blue .... press 2
White ... Press 3
Black ... Press 4");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
            }
            while (!IsLegalUserChoice("^[1-4]+$", userChoice));

            int.TryParse(userChoice, out userChoiceAsInt);
            o_CarColor = (Car.eCarColor)userChoiceAsInt;
        }

        public void GetNumOfDoors(out Car.eNumOfDoors o_CarNumOfDoors)
        {
            string userChoice = null;
            int userChoiceAsInt;
            string msg = string.Format(@"Please select num of doors:
Two .... press 1
Three .. press 2
Four ... Press 3
Five ... Press 4");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
            }
            while (!IsLegalUserChoice("^[1-4]+$", userChoice));

            int.TryParse(userChoice, out userChoiceAsInt);
            o_CarNumOfDoors = (Car.eNumOfDoors)userChoiceAsInt;
        }

        public bool AskUserIfTrunkIsCooling()
        {
            string userChoice = null;
            bool isCooling = false;
            string msg = string.Format(@"Is trunk cooling? (Y/N)");
            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
            }
            while (userChoice.Length != 1 || (!userChoice.Equals("Y") && (!userChoice.Equals("N") && (!userChoice.Equals("n") && (!userChoice.Equals("y"))))));
            if (userChoice == "n" || userChoice == "N")
            {
                isCooling = false;
            }
            else
            {
                isCooling = true;
            }

            return isCooling;
        }

        public void GetCargoCapacity(out float o_CargoCapacity)
        {
            string userChoice = null;
            string msg = string.Format(@"Please insert Truck cargo capacity (between 1000 to 10000) : ");

            do
            {
                PrintUserInputMsgToConsoleAndGetNewInput(ref userChoice, msg);
                if(float.TryParse(userChoice, out o_CargoCapacity) == false)
                {
                    throw new FormatException();
                }
            }
            while (!Regex.IsMatch(userChoice, "[0-9]+$") || userChoice.Length > 5 || userChoice.Length < 4);
        }

        public void PrintUserInputMsgToConsoleAndGetNewInput(ref string o_UserChoice, string i_Msg)
        {
            if (o_UserChoice != null)
            {
                WrongInputMsg();
            }

            Console.WriteLine(i_Msg);
            o_UserChoice = Console.ReadLine();
        }

        public void WrongInputMsg()
        {
            Console.WriteLine("Wrong input!");
            Thread.Sleep(1000);
            Console.Clear();
        }

        public bool IsLegalUserChoice(string i_StringPattern, string i_UserChoice)
        {
            bool isLegalChoice = Regex.IsMatch(i_UserChoice, i_StringPattern) && i_UserChoice.Length == 1;
            return isLegalChoice;
        }
    }
}
