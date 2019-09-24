using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace ConsoleApp
{
    public class ClassWithMethods
    {
        //The sum from the card
        public double CartSum { get; set; }

        //the whole sum with discounts
        public double WholeSumWithDiscounts { get; set; }
        
        //how many items are inside of the cart
        public int CartCounter { get; set; }

        //The list with the prices from the products
        public List<double> CartSumPrices = new List<double>();

        //discount numbers
        public List<double> Dicounts = new List<double>();

        //here are the lists that are send to the kvpList with the values inside depending on the  condition
        //the list for parts
        List<Part> choosedPartProducts = new List<Part>();

        //the list for modules
        List<Module> choosedModuleProducts = new List<Module>();

        //the list for configurations
        List<Configuration> choosedConfigurationsProducts = new List<Configuration>();

        //this is the list where are stored ale elements that user wants to buy
        List<KeyValuePair<string, Enum>> kvpList = new List<KeyValuePair<string, Enum>>();


        public void WelcomeMessage()
        {
            //Here we are "clearing the lists which are sent with values in kvpList"
            //because when we make recursion(when the user enters 2 to go at the beggining)
            //to prevent double showing of the elements
            choosedPartProducts.Clear();
            choosedModuleProducts.Clear();
            choosedConfigurationsProducts.Clear();

            Console.WriteLine();
            Console.WriteLine("Enter the number to choose what do you want to buy by us");

            Console.WriteLine("1) Parts");
            Console.WriteLine("2) Modules ");
            Console.WriteLine("3) Configurations");
            Console.WriteLine("4) Exit if all limits of the products that we have you buyed");


            bool isItNumber = int.TryParse(Console.ReadLine(), out int choosingNumberProduct);

            //callint the CallSpecificProduct  GENERIC METHOD with three types od list
            //depending on the user input
            if (isItNumber == true)
            {
                switch (choosingNumberProduct)
                {
                    case 1:
                        CallSpecificProduct(Db.Parts);
                        break;
                    case 2:
                        CallSpecificProduct(Db.Modules);
                        break;
                    case 3:
                        CallSpecificProduct(Db.Configurations);
                        break;
                    case 4:
                        var partCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.PartType");
                        var moduleCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ModuleType");
                        var configurationCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ConfigurationType");

                        if(partCounting >= 10 && moduleCounting >= 5 && configurationCounting <= 1)
                        {
                            TheFinalMethodAfterBuying(kvpList , "doesnt matter now");
                        }

                        else
                        {
                            Console.WriteLine("You still cant use this option");
                            Console.WriteLine("You can exit NOW by pressing 4) Continue to check out If you buyed one part and module minimum");
                            WelcomeMessage();
                        }
                        break;
                    default:
                        Console.WriteLine("You need to input 1 , 2  or 3");
                        WelcomeMessage();
                        break;

                }
            }
            else
            {
                Console.WriteLine("You need to input numbers.Letters are not allowed");
                WelcomeMessage();
            }
        }


        //here the user choose what he want to see,from the "PRODUCT" he choosed
        public void CallSpecificProduct<T>(List<T> listOfProducts) where T : Item
        {

            Console.WriteLine();
            Console.WriteLine("Choose the filtering here");
            Console.WriteLine("1) Show all products");
            Console.WriteLine("2) Show all products depending on their price ");
            Console.WriteLine("3) Show all products depending on their type");

            bool isItNumber = int.TryParse(Console.ReadLine(), out int choosingNumberProduct);

            //after the choose from the user,we call another methods
            if (isItNumber == true)
            {
                switch (choosingNumberProduct)
                {
                    case 1:
                        PrintingProducts(listOfProducts);
                        break;
                    case 2:
                        PrintingProductsByThePrice(listOfProducts);
                        break;
                    case 3:
                        PrintingProductsByTheirType(listOfProducts);
                        break;
                    default:
                        CallSpecificProduct(listOfProducts);
                        break;
                }
            }

            else
            {
                CallSpecificProduct(listOfProducts);
            }
        }
        //vtora greask dolu vrtese for loop niz modulite

        //This is the method for showing all the items in the specific "product". based on that
        //which type the product it self is.
        public void PrintingProducts<T>(List<T> products) where T : Item
        {
            var partCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.PartType");
            var moduleCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ModuleType");
            var configurationCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ConfigurationType");

           //get the type of the product
            Type myListElementType = products.GetType().GetGenericArguments().Single();
            Console.WriteLine(myListElementType);

            //1
            if (myListElementType.ToString() == "Entities.Part")
            {
                if (partCounting >= 10)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Sorry but You already buyed {partCounting} parts. 10 parts is our limit");
                    Console.WriteLine("Maybe you want to buy something else");
                    Console.WriteLine();
                    WelcomeMessage();
                }
                else
                {
                    for (int i = 0; i < Db.Parts.Count; i++)
                    {
                        Console.WriteLine(Db.Parts[i].Name);
                        choosedPartProducts.Add(Db.Parts[i]);
                    }
                    choosingProduct(choosedPartProducts, "printingAllProductsOption");
                }

            }

            //2
            if (myListElementType.ToString() == "Entities.Module")
            {
                if (moduleCounting >= 2)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Sorry but You already buyed {moduleCounting} modules. 5 modules is our limit");
                    Console.WriteLine("maybe you want to buy something else");
                    WelcomeMessage();
                }

                else
                {
                    for (int i = 0; i < Db.Modules.Count; i++)
                    {
                        Console.WriteLine(Db.Modules[i].Type);
                        choosedModuleProducts.Add(Db.Modules[i]);
                    }
                    choosingModule(choosedModuleProducts, "printingAllProductsOption");
                }
            }

            //3
            if (myListElementType.ToString() == "Entities.Configuration")
            {

                if(configurationCounting >= 1)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Sorry but You already buyed {configurationCounting} configuration. One configuration is our limit");
                    Console.WriteLine("mayme you want to buy something else");
                    WelcomeMessage();
                }

                else
                {
                    for (int i = 0; i < Db.Configurations.Count; i++)
                    {
                        Console.WriteLine(Db.Configurations[i].Title);
                        choosedConfigurationsProducts.Add(Db.Configurations[i]);
                    }
                    choosingConfiguration(choosedConfigurationsProducts, "printingAllProductsOption");
                }
            }

        }

        //Showing the the items of some product,based on their price
        public void PrintingProductsByThePrice<T>(List<T> products) where T : Item
        {
            choosedPartProducts.Clear();
            choosedModuleProducts.Clear();
            choosedConfigurationsProducts.Clear();

            var partCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.PartType");
            var moduleCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ModuleType");
            var configurationCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ConfigurationType");

            Console.WriteLine("Enter the minimum price for the product");

            bool isItNumberLowPrice = int.TryParse(Console.ReadLine(), out int lowPrice);

            Console.WriteLine("Enter the maximum price for the product");
            bool isItNumberHighPrice = int.TryParse(Console.ReadLine(), out int highPrice);

            //if both of the numbers the user enterd are type of number go forward
            if (isItNumberLowPrice == true && isItNumberHighPrice == true)
            {
                var filteredProductsByTheirPrice = from price in products where price.Price >= lowPrice && price.Price <= highPrice select price;
                Type myListElementType = products.GetType().GetGenericArguments().Single();

                //1
                if (myListElementType.ToString() == "Entities.Part")
                {

                    if(partCounting >= 10)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Sorry but You already buyed {partCounting} parts. 10 parts is our limit");
                        Console.WriteLine("Maybe you want to buy something else");
                        Console.WriteLine();
                        WelcomeMessage();
                    }
                    else
                    {
                        var countingIfThereIsNullPriceRange = Db.Parts.Count(x => x.Price >= lowPrice && x.Price <= highPrice);

                        if(countingIfThereIsNullPriceRange == 0)
                        {
                            Console.WriteLine("We dont have such a price range in our store for parts.Please try again");
                            PrintingProductsByThePrice(products);
                        }

                        else
                        {
                            for (int i = 0; i < Db.Parts.Count; i++)
                            {
                                if (Db.Parts[i].Price >= lowPrice && Db.Parts[i].Price <= highPrice)
                                {
                                    //enter the products in the list,that correspond to the
                                    //prices that user entered.and then call 
                                    choosedPartProducts.Add(Db.Parts[i]);
                                }
                            }
                            choosingProduct(choosedPartProducts, "printingByThePriceOption");
                        }
                    }
                }


                //2
                if (myListElementType.ToString() == "Entities.Module")
                {
                    if (moduleCounting >= 5)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Sorry but You already buyed {partCounting} parts. 10 parts is our limit");
                        Console.WriteLine("Maybe you want to buy something else");
                        Console.WriteLine();
                        WelcomeMessage();
                    }

                    else
                    {
                        
                        var countingIfThereIsNullPriceRange = Db.Modules.Count(x => x.Price >= lowPrice && x.Price <= highPrice);

                        if(countingIfThereIsNullPriceRange == 0)
                        {
                            Console.WriteLine("We dont have such a price range in our store for modules.Please try again");
                            PrintingProductsByThePrice(products);
                        }

                        else
                        {
                            for (int i = 0; i < Db.Modules.Count; i++)
                            {
                                if (Db.Modules[i].Price >= lowPrice && Db.Modules[i].Price <= highPrice)
                                {
                                    choosedModuleProducts.Add(Db.Modules[i]);
                                }
                            }
                            choosingModule(choosedModuleProducts, "printingByThePriceOption");
                        }
                    }
                }

                //3
                if (myListElementType.ToString() == "Entities.Configuration")
                {

                    if(configurationCounting >= 1)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Sorry but You already buyed {configurationCounting} configuration. One configuration is our limit");
                        Console.WriteLine("mayme you want to buy something else");
                        WelcomeMessage();
                    }

                    else
                    {

                        var countingIfThereIsNullPriceRange = Db.Configurations.Count(x => x.Price >= lowPrice && x.Price <= highPrice);

                        if(countingIfThereIsNullPriceRange == 0)
                        {
                            Console.WriteLine("We dont have such a price range in our store for confugurations.Please try again");
                            PrintingProductsByThePrice(products);
                        }

                        else
                        {
                            for (int i = 0; i < Db.Configurations.Count; i++)
                            {
                                if (Db.Configurations[i].Price >= lowPrice && Db.Configurations[i].Price <= highPrice)
                                {
                                    choosedConfigurationsProducts.Add(Db.Configurations[i]);
                                }
                            }
                            choosingConfiguration(choosedConfigurationsProducts, "printingByThePriceOption");
                        }
                      
                    }

                }
   
            }

            else
            {
                Console.WriteLine("You should input numbers");
                PrintingProductsByThePrice(products);
            }
        }

        // Showing the types of some product
        public void PrintingProductsByTheirType<T>(List<T> products)
        {

            choosedPartProducts.Clear();
            choosedModuleProducts.Clear();
            choosedConfigurationsProducts.Clear();

            var partCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.PartType");
            var moduleCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ModuleType");
            var configurationCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ConfigurationType");


            Type myListElementType = products.GetType().GetGenericArguments().Single();

            Console.WriteLine("Choose the type of parts you want to buy");

            //1
            if (myListElementType.ToString() == "Entities.Part")
            {
                List<int> theListWithIndexers = new List<int>();
                Console.WriteLine();

                //if there are more than 10 buyed parts,go back at the beggining
                if(partCounting >= 10)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Sorry but You already buyed {partCounting} parts. 10 parts is our limit");
                    Console.WriteLine("Maybe you want to buy something else");
                    Console.WriteLine();
                    WelcomeMessage();
                }

                else
                {
                    int counter = 1;
                    foreach (string str in Enum.GetNames(typeof(PartType)))
                    {
                        Console.WriteLine($"{counter} -- {str}");
                        //adding the indexers in this list,later we will know is it the right number from the product
                        theListWithIndexers.Add(counter);
                        counter++;
          
                    }
                }

                bool isItNumber = int.TryParse(Console.ReadLine(), out int choosedNumberForType);

                //if the input is of type number, and if  that number is in the "theListWithIndexers" list,
                //then the input is right,the user entered number,that is showed as option on the product
                //and the order can proceed.

                if (isItNumber && theListWithIndexers.Contains(choosedNumberForType))
                {
                    for (int i = 0; i < Db.Parts.Count; i++)
                    {
                        if (Db.Parts[i].Type == (PartType)choosedNumberForType - 1)
                        {
                            Console.WriteLine($"{Db.Parts[i].Name}");
                            choosedPartProducts.Add(Db.Parts[i]);
                        }
                    }
                    choosingProduct(choosedPartProducts, "printingByTheTypeOption");
                }

                //if not go back
                else
                {
                    Console.WriteLine("You need yo enter the right  numbers  , without enetering letters!!");
                    PrintingProductsByTheirType(products);
                }
            }

            //same logic down
            
            //2
            if (myListElementType.ToString() == "Entities.Module")
            {
                Console.WriteLine();

                List<int> theListWithIndexers = new List<int>();

                if (moduleCounting >= 5)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Sorry but You already buyed {partCounting} parts. 10 parts is our limit");
                    Console.WriteLine("Maybe you want to buy something else");
                    Console.WriteLine();
                    WelcomeMessage();
                }

                else
                {
                    int counter = 1;

                    foreach (string str in Enum.GetNames(typeof(ModuleType)))
                    {
                        Console.WriteLine($"{counter} -- {str}");
                        theListWithIndexers.Add(counter);
                        counter++;
                    }
                }


                bool isItNumber = int.TryParse(Console.ReadLine(), out int choosedNumberForType);


                if (isItNumber && theListWithIndexers.Contains(choosedNumberForType))
                {
                    for (int i = 0; i < Db.Modules.Count; i++)
                    {
                        if (Db.Modules[i].Type == (ModuleType)choosedNumberForType - 1)
                        {
                            Console.WriteLine($"{i} -- {Db.Modules[i].Type}");
                            choosedModuleProducts.Add(Db.Modules[i]);
                        }
                    }
                    choosingModule(choosedModuleProducts, "printingByTheTypeOption");
                }

                else
                {
                    Console.WriteLine("You need yo enter the right numbers without entering letters!!");
                    PrintingProductsByTheirType(products);
                }
            }

            

            if (myListElementType.ToString() == "Entities.Configuration")
            {
                Console.WriteLine();

                List<int> theListWithIndexers = new List<int>();

                if (configurationCounting >= 1)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Sorry but You already buyed {configurationCounting} configuration. 1 configuration is our limit");
                    Console.WriteLine("Maybe you want to buy something else");
                    Console.WriteLine();
                    WelcomeMessage();
                }

                else
                {
                    int counter = 1;

                    foreach (string str in Enum.GetNames(typeof(ConfigurationType)))
                    {
                        Console.WriteLine($"{counter} -- {str}");
                        theListWithIndexers.Add(counter);
                        counter++;
                    }
                }


                bool isItNumber = int.TryParse(Console.ReadLine(), out int choosedNumberForType);

                if (isItNumber && theListWithIndexers.Contains(choosedNumberForType))
                {
                    for (int i = 0; i < Db.Configurations.Count; i++)
                    {
                        if (Db.Configurations[i].Type == (ConfigurationType)choosedNumberForType - 1)
                        {
                            Console.WriteLine($"{i} -- {Db.Configurations[i].Title}");
                            choosedConfigurationsProducts.Add(Db.Configurations[i]);
                        }
                    }
                    choosingConfiguration(choosedConfigurationsProducts, "printingByTheTypeOption");
                }

                else
                {
                    Console.WriteLine("You need yo enter the right numbers without entering letters!!");
                    PrintingProductsByTheirType(products);
                }
            }


        }

        //the method for parts
        //here T inherits from Part,and we are able to print the properties we need.
        //we become here in "listOfProducts" items with type NAME and type ENUM
        //and we are inserting that items in the kvpList

        //aslo we become and
        //string value(later we know with that value which method we need to call back
        //to go at the right filter option as the user want
        public void choosingProduct<T>(List<T> listOfProducts, string typeOfFiltering) where T : Part
        {
            List<int> theListWithIndexers = new List<int>();

            Console.Clear();
            Console.WriteLine();

            for (int i = 0; i < listOfProducts.Count; i++)
            {
                Console.WriteLine(i + " " + listOfProducts[i].Name);
                theListWithIndexers.Add(i);
            }

            Console.WriteLine();
            Console.WriteLine("Enter the number from the product you want to buy");
            Console.WriteLine();

            bool isItNumber = int.TryParse(Console.ReadLine(), out int choosedNumberForType);

            
            if (isItNumber == true && theListWithIndexers.Contains(choosedNumberForType))
            {
                for (int i = 0; i < listOfProducts.Count; i++)
                {

                    if (choosedNumberForType == i)
                    {
                        Console.WriteLine($"{listOfProducts[i].Name} with type -- {listOfProducts[i].Type} is added to the cart ");
                        CartCounter++;
                        Console.WriteLine($"You have now {CartCounter} items in the cart");
                        //here we are inserting the items that the user wants.
                        kvpList.Add(new KeyValuePair<string, Enum>(listOfProducts[i].Name, listOfProducts[i].Type));
                        //after we insert that element,that the user wanted
                        //we write and the sum from that element in the cart
                        CartSum += listOfProducts[i].Price;
                        CartSumPrices.Add(listOfProducts[i].Price);
                        Dicounts.Add(listOfProducts[i].Discount);
                    }
                }
                TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
            }

            else
            {
                choosingProduct(listOfProducts, typeOfFiltering);
            }

        }


        //the method for modules
        public void choosingModule<T>(List<T> listOfProducts, string typeOfFiltering) where T : Module
        {
            List<int> theListWithIndexers = new List<int>();

            Console.Clear();
            Console.WriteLine();

            for (int i = 0; i < listOfProducts.Count; i++)
            {
                Console.WriteLine(i + " " + listOfProducts[i].Type);
                theListWithIndexers.Add(i);
            }

            Console.WriteLine("Enter the number from the product you want to buy");

            bool isItNumber = int.TryParse(Console.ReadLine(), out int choosedNumberForType);

            if (isItNumber == true && theListWithIndexers.Contains(choosedNumberForType))
            {
                for (int i = 0; i < listOfProducts.Count; i++)
                {

                    if (choosedNumberForType == i)
                    {
                        Console.WriteLine($"{listOfProducts[i].Type} with type -- {listOfProducts[i].Type} is added to the cart ");
                        CartCounter++;
                        Console.WriteLine($"You have now {CartCounter} items in the cart");
                        kvpList.Add(new KeyValuePair<string, Enum>(listOfProducts[i].Type.ToString(), listOfProducts[i].Type));
                        CartSum += listOfProducts[i].Price;
                        CartSumPrices.Add(listOfProducts[i].Price);
                        Dicounts.Add(listOfProducts[i].Discount);
                        //quit when you find it,dont continue the for each iteration
                        break;
                    }

                }
                TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
            }

            else
            {
                choosingModule(listOfProducts, typeOfFiltering);
            }
        }

        //the method for cofigurations
        public void choosingConfiguration<T>(List<T> listOfProducts, string typeOfFiltering) where T : Configuration
        {
            List<int> theListWithIndexers = new List<int>();

            Console.Clear();
            Console.WriteLine();

            for (int i = 0; i < listOfProducts.Count; i++)
            {
                Console.WriteLine(i + " " + listOfProducts[i].Title);
                theListWithIndexers.Add(i);
            }

            Console.WriteLine("Enter the number from the product you want to buy");

            bool isItNumber = int.TryParse(Console.ReadLine(), out int choosedNumberForType);

            if (isItNumber == true && theListWithIndexers.Contains(choosedNumberForType))
            {
                for (int i = 0; i < listOfProducts.Count; i++)
                {
                    if (choosedNumberForType == i)
                    {
                        Console.WriteLine($"{listOfProducts[i].Title} with type -- {listOfProducts[i].Type} is added to the cart ");
                        CartCounter++;
                        Console.WriteLine($"You have now {CartCounter} items in the cart");
                        kvpList.Add(new KeyValuePair<string, Enum>(listOfProducts[i].Title.ToString(), listOfProducts[i].Type));
                        CartSum += listOfProducts[i].Price;
                        CartSumPrices.Add(listOfProducts[i].Price);
                        Dicounts.Add(listOfProducts[i].Discount);
                        break;
                    }
                }
                TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
            }

            else
            {
                choosingConfiguration(listOfProducts, typeOfFiltering);
            }
        }


        public void TheFinalMethodAfterBuying(List<KeyValuePair<string, Enum>> kvpList, string typeOfFiltering)
        {
            Type myListElementType = kvpList.GetType().GetGenericArguments().Single();

            var partCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.PartType");
            var moduleCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ModuleType");
            var configurationCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ConfigurationType");


            Console.WriteLine();
            Console.WriteLine("Choose the numbers to proceed");
            Console.WriteLine("1) Coninue Shooping");
            Console.WriteLine("2) Choose something else");
            Console.WriteLine("3) See Cart");
            Console.WriteLine("4) Continue to Check Out");

            bool isItNumber = int.TryParse(Console.ReadLine(), out int choosedNumberForType);

            if (isItNumber == true)
            {
                switch (choosedNumberForType)
                {
                    case 1:
                        for (int i = 0; i < kvpList.Count; i++)
                        {
                            //1 we are checking here if the value in kvpList(there we puted all enums from the elements"
                            //is equal to the actual enum type from the "enums folder"
                            //if it is depending on user input we call back the methods with specific list
                            if (kvpList[kvpList.Count - 1].Value.GetType().ToString() == "Entities.PartType")
                            {
                                if (partCounting >= 10)
                                {
                                    //the reusable method where we are notifying the user that the limit is already taken/
                                    //and goes right at the beggining
                                    GoingBack(partCounting, "Parts");
                                    TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
                                    break;
                                }
                                else
                                {
                                    //depending on the value of the parametar,we sent here
                                    //we will call again that method when the user want that
                                    if (typeOfFiltering == "printingByThePriceOption")
                                    {
                                        PrintingProductsByThePrice(Db.Parts);
                                        break;
                                    }

                                    if (typeOfFiltering == "printingByTheTypeOption")
                                    {
                                        PrintingProductsByTheirType(Db.Parts);
                                        break;
                                    }

                                    else
                                    {
                                        choosingProduct(Db.Parts, typeOfFiltering);
                                        break;
                                    }
                                }
                            }


                            //2
                            if (kvpList[kvpList.Count - 1].Value.GetType().ToString() == "Entities.ModuleType")
                            {

                                if (moduleCounting >= 5)
                                {
                                    GoingBack(moduleCounting, "Modules");
                                    TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
                                    break;
                                }
                                else
                                {
                                    if (typeOfFiltering == "printingByThePriceOption")
                                    {
                                        PrintingProductsByThePrice(Db.Modules);
                                        break;
                                    }

                                    if (typeOfFiltering == "printingByTheTypeOption")
                                    {
                                        PrintingProductsByTheirType(Db.Modules);
                                        break;
                                    }

                                    if (typeOfFiltering == "printingAllProductsOption")
                                    {
                                        choosingModule(Db.Modules, typeOfFiltering);
                                        break;
                                    }
                                }

                            }

                            //3
                            if (kvpList[kvpList.Count - 1].Value.GetType().ToString() == "Entities.ConfigurationType")
                            {

                                if (configurationCounting >= 1)
                                {
                                    GoingBack(configurationCounting, "Configuration");
                                    TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
                                    break;
                                }

                                else
                                {
                                    if (typeOfFiltering == "printingByThePriceOption")
                                    {
                                        PrintingProductsByThePrice(Db.Configurations);
                                        break;
                                    }

                                    if (typeOfFiltering == "printingByTheTypeOption")
                                    {
                                        PrintingProductsByTheirType(Db.Configurations);
                                        break;
                                    }

                                    if (typeOfFiltering == "printingAllProductsOption")
                                    {
                                        choosingConfiguration(Db.Configurations, typeOfFiltering);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        WelcomeMessage();
                        break;
                        //printhe the elements that are buyed
                    case 3:
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("These are the products that you buyed");
                        foreach (KeyValuePair<string, Enum> kvp in kvpList)
                        {
                            Console.WriteLine($"PRODUCT     {kvp.Key,-20} | TYPE      {kvp.Value,-20}");

                        }
                        WelcomeMessage();
                        break;
                    case 4:
                        TheSumFromTheCart();
                        break;
                    default:
                        Console.WriteLine("You need to enter the right numbers");
                        TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
                        break;
                }//tuka zavrsuva switch
            }//tuka zavrsuva ifo za da vlezi vo switch
            else
            {
                Console.WriteLine("You are not allowed to enter letters");
                TheFinalMethodAfterBuying(kvpList, typeOfFiltering);
            }
        }//tuka zavrsuva metodo

        public void TheSumFromTheCart()
        {
            var partCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.PartType");
            var moduleCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ModuleType");
            var configurationCounting = kvpList.Count(x => x.Value.GetType().ToString() == "Entities.ConfigurationType");

            if (partCounting < 1)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("you need to buy minimum 1 part");
                Console.WriteLine();
                WelcomeMessage();
            }

            if(moduleCounting < 1)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("you need to buy minimum 1 module");
                Console.WriteLine();
                WelcomeMessage();
            }

            else
            {
                Console.WriteLine($"Your bill is {CartSum} $ and you buyed the followed {CartCounter} items");
                int showingPriceCounter = 0;
                int showingDiscountCounter = 0;

                Console.WriteLine();
                Console.WriteLine("You are lucky because we have discounts");

                double justForScope = 0;

                Console.WriteLine();

                for (int i = 0; i < kvpList.Count; i++)
                {
                    var calculatedDiscountFromTheSum = CartSumPrices[showingPriceCounter] * Dicounts[showingDiscountCounter];
                    var newPriceWithOrWithoutDiscountCalculation = CartSumPrices[showingPriceCounter] - (CartSumPrices[showingPriceCounter] * Dicounts[showingDiscountCounter]);
                    Console.WriteLine($"PRODUCT   {kvpList[i].Key,-20} | TYPE   {kvpList[i].Value,-12} | NORMAL PRICE     {CartSumPrices[showingPriceCounter],-4} | DISCOUNT    {Dicounts[showingDiscountCounter],-4} |  DICOUNT SUM {calculatedDiscountFromTheSum,-4} | THE PRICE(WITH OR WITHOUT DISCOUNT)     {newPriceWithOrWithoutDiscountCalculation,-4}");
                    showingPriceCounter++;
                    showingDiscountCounter++;
                    WholeSumWithDiscounts += newPriceWithOrWithoutDiscountCalculation;
                    justForScope = WholeSumWithDiscounts;
                }

                Console.WriteLine();
                Console.WriteLine($"Your bill with discounts now is {justForScope} $ and you saved {CartSum - justForScope} $");
                ReceiptOptions();
            }

        }


        public void ReceiptOptions()
        {
            Console.WriteLine();
            Console.WriteLine("You want the receipt to be send through");
            Console.WriteLine("1) SMS");
            Console.WriteLine("2) Email");
            Console.WriteLine("3) Post");
            Console.WriteLine("4) SMS, Email , Post");

            bool isItNumber = int.TryParse(Console.ReadLine(), out int choosingNumberProduct);

            if(isItNumber)
            {
                switch(choosingNumberProduct)
                {
                    case 1:
                        ReceiptHanlder("SMS");
                        break;
                    case 2:
                        ReceiptHanlder("Email");
                        break;
                    case 3:
                        ReceiptHanlder("Post");
                        break;
                    case 4:
                        ReceiptHanlder("SMS Email Post");
                        break;
                    default:
                        ReceiptOptions();
                        break;
                }
            }

            else
            {
                Console.WriteLine();
                Console.WriteLine("You need to enter numbers");
                ReceiptOptions();
            }
        }

        public void ReceiptHanlder(params string[] typeOfSending)
        {
            Console.WriteLine("Processing the transaction...");
            Thread.Sleep(3000);

            Console.WriteLine();
            foreach (var item in typeOfSending)
            {
                Console.WriteLine($"The receipt is sended through {item}");
            }
        }
        public void GoingBack(int counter , string whichType)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Sorry but You already buyed {counter} {whichType}. {counter} is our limit");
            Console.WriteLine("maybe you want to buy something else");
            Console.WriteLine();
        }
    }
}
