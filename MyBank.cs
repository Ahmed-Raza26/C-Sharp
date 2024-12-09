using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BAM
{
    abstract class MyBank
    {
        public static List<Account> accounts = new List<Account>();
        public static int accountnumincreament = 7000001;
        public static CultureInfo myculture = new CultureInfo("ur-PK");

        public abstract Account findaccount(int accountid);
    }

    abstract class Account : MyBank
    {
        public string? Owner { get; set; }
        public int AccountNum { get; set; }
        public decimal Balance { get; set; }
        public string? Contact { get; set; }
        public string? Pin { get; set; }

        public virtual void AddAccount(string owner, string contact, string pin, decimal initialBalance)
        {
            this.AccountNum = accountnumincreament;
            this.Owner = owner;
            this.Contact = contact;
            this.Pin = pin;
            this.Balance = initialBalance;
        }

        public override Account findaccount(int accountid)
        {
            return accounts.Find(a => a.AccountNum == accountid);
        }

        public virtual void AccountCreation()
        {
            bool isValid = false;
            Console.WriteLine("\nEnter the following details to create an account\n");

            while (!isValid)
            {
                string? name;
                do
                {
                    Console.WriteLine("Enter your name (minimum 3 characters, only alphabets allowed):");
                    name = Console.ReadLine();
                    if (Regex.IsMatch(name, @"^[A-Za-z\s'-]{3,}$"))
                        break;
                    Console.WriteLine("\nInvalid name! Only alphabetic characters allowed, with a minimum length of 3.");
                } while (true);

                string? contact;
                do
                {
                    Console.WriteLine("\nEnter your Contact number (format: 03XXXXXXXXX or 03XX-XXXXXXX):");
                    contact = Console.ReadLine();
                    if (Regex.IsMatch(contact, @"^03\d{9}$|^03\d{2}-\d{7}$"))
                        break;
                    Console.WriteLine("\nInvalid contact number! Please enter a valid Pakistani number starting with 03.");
                } while (true);

                string? pin;
                do
                {
                    Console.WriteLine("Create a 4-digit pin:");
                    pin = Console.ReadLine();
                    if (Regex.IsMatch(pin, @"^\d{4}$"))
                        break;
                    Console.WriteLine("\nInvalid Pin! Please enter a valid Pin up to 4 digits.");
                } while (true);

                decimal initialAmount;
                while (true)
                {
                    Console.WriteLine("\nDeposit at least 1000 or above to initialize your account balance:");
                    if (decimal.TryParse(Console.ReadLine(), out initialAmount) && initialAmount >= 1000)
                        break;
                    else
                        Console.WriteLine("\nInvalid input. Amount must be at least 1000 or above.");
                }

                Console.WriteLine("\nPlease confirm! Details you entered are correct? (y/n):");
                string? confirm = Console.ReadLine();
                if (confirm.ToLower() == "y")
                {
                    
                    AddAccount(name, contact, pin, initialAmount);
                    accounts.Add(this);
                    accountnumincreament++;

                    Console.WriteLine("\nAccount has been successfully created.");
                    Console.WriteLine($"\nYour Account Number is {this.AccountNum}");

                    isValid = true;
                }
                else
                {
                    Console.WriteLine("\nOops! Please re-enter the details.");
                }
            }
        }

        public virtual void common(Action<Account> logic)
        {
            bool isaccount = false;
            while (!isaccount)
            {
                Console.WriteLine("\nEnter your Account Number:");
                if (int.TryParse(Console.ReadLine(), out int accountNum))
                {
                    Account account = findaccount(accountNum);
                    if (account != null)
                    {
                        isaccount = true;
                        bool ispin = false;
                        while (!ispin)
                        {
                            Console.WriteLine("\nEnter your Pin:");
                            string? pin = Console.ReadLine();
                            if (pin == account.Pin)
                            {
                                ispin = true;
                                logic(account);
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid Pin!");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNo such account found! Returning to the main menu\n");
                        isaccount = true;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid account number format.\n");
                }
            }
        }

        public abstract void deposit();
        public abstract void withdraw();
        public abstract void ShowBalance();
    }

    class user : Account
    {
       

        public override void deposit()
        {
            common(account =>
            {
                Console.WriteLine("\nEnter a deposit amount:");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount >= 500)
                {
                    account.Balance += amount;
                    Console.WriteLine($"\nAmount added successfully! Your new balance is {account.Balance.ToString("C2", myculture)}.");
                }
                else
                {
                    Console.WriteLine("\nInvalid input! Amount must be at least 500 or above.");
                }
            });
        }

        public override void withdraw()
        {
            common(account =>
            {
                Console.WriteLine("\nEnter the amount you want to withdraw:");
                if (decimal.TryParse(Console.ReadLine(), out decimal amountWithdraw) && amountWithdraw > 0)
                {
                    if (account.Balance > amountWithdraw && account.Balance - amountWithdraw >= 1000)
                    {
                        account.Balance -= amountWithdraw;
                        Console.WriteLine("\nMoney withdrawn successfully!");
                        Console.WriteLine($"\nYour current balance is: {account.Balance.ToString("C2", myculture)}");
                    } else if (account.Balance - amountWithdraw < 1000)
                    {
                        Console.WriteLine($"\nyou can't withdraw this amount! you should maintain aleast 1000 in your account.\nyour current Balance is {account.Balance.ToString("C2",myculture)}");
                    }
                    else
                    {
                        Console.WriteLine("\nInsufficient Balance!");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid format.");
                }
            });
        }

        public override void ShowBalance()
        {
            common(account =>
            {
                Console.WriteLine($"\nDear Mr./Ms. {account.Owner.ToUpper()}, your current balance is : {account.Balance.ToString("C2", myculture)}\n");
            });
        }
    }
}
