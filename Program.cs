using System;
using BAM;
namespace BankAccount
{
    class Program
    {
        public static void Main()
        {
            bool start = false;
            Console.WriteLine("WELCOME!\n");

            while (!start)
            {
                BAM.Account u = new BAM.user();

                try
                {
                    Console.WriteLine("\nSelect From The Following Option\n1.Account Creation\n2.Deposit Money\n3.Witthdraw\n4.Check Balance\n5.Exit\n");
                    int selection = Convert.ToInt32(Console.ReadLine());
                    switch (selection)
                    {
                        case 1:
                            u.AccountCreation();
                            break;
                        case 2:
                            u.deposit();
                            break;
                        case 3:
                            u.withdraw();
                            break;
                        case 4:
                            u.ShowBalance();
                            break;
                        case 5:
                            Console.WriteLine("\nExit The Program");
                            start = true;
                            Console.WriteLine("Press Any key to Close the Console");
                            Console.ReadKey();
                             
                           
                            break;
                        default:
                            Console.WriteLine("\nInvalid Selection!");
                            break;
                    }

                }
                catch (Exception e)
                { Console.WriteLine("\nInvalid Input!" + e.ToString()); }
            }
        }
    }

}