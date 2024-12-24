using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая_Работа
{
    public class Bank
    {
        private string bankName;
        private string bankAccount;

        public string BankName
        {
            get { return bankName; }
            set 
            {
                if (value == "") throw new ArgumentException("Название банка не введено!");
                else bankName = value; 
            }
        }

        public string BankAccount
        {
            get { return bankAccount; }
            set 
            {
                if (value == "") throw new ArgumentException("Номер счета банка не введено!");
                if (value.Count() != 9) throw new ArgumentException("Длина счета должна быть равна 9!");
                if (!int.TryParse(value, out int num)) throw new ArgumentException("Номер счета банка должен состоять из чисел!");
                else bankAccount = value;
            }
        }

        public Bank(string bankName, string bankAccount)
        {
            BankName = bankName;
            BankAccount = bankAccount;
        }
    }

    public class Counterparty
    {
        private string firmName;
        private string country;
        private string directorName;
        private string legalAddress;
        private string phone;
        private string email;
        private string inn;
        private Bank bank;

        public string FirmName
        {
            get { return firmName; }
            set 
            {
                if (value == "") throw new ArgumentException("Название фирмы не введена!");
                else firmName = value; 
            }
        }
        public string Country
        {
            get { return country; }
            set 
            {
                if (value == "") throw new ArgumentException("Страна фирмы не введена!");
                else country = value; 
            }
        }
        public string DirectorName
        {
            get { return directorName; }
            set 
            {
                if (value == "") throw new ArgumentException("ФИО руководителя не введен!");
                else directorName = value;
            }
        }
        public string LegalAddress
        {
            get { return legalAddress; }
            set 
            {
                if (value == "") throw new ArgumentException("Юридический адрес не введен!");
                else legalAddress = value; 
            }
        }
        public string Phone
        {
            get { return phone; }
            set 
            {
                if (value == "") throw new ArgumentException($"Номер телефона не введен!");
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\+\d{4}-\(\d{3}\)-\d{4}-\d{4}$")) throw new ArgumentException("Номер телефона некорректно введен!\nДлина номера должна быть 14!");
                else phone = value; 
            }
        }
        public string Email
        {
            get { return email; }
            set 
            {
                if (value == "") throw new ArgumentException("E-mail не введен!");
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) throw new ArgumentException("E-mail введен неправильно!\nexample@example.com");
                else email = value;
            }
        }
        public string INN
        {
            get { return inn; }
            set 
            {
                if (inn == "") throw new ArgumentException("ИНН не введен!");
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{10}$")) throw new ArgumentException("ИНН должен состоят из 10 чисел!");
                else inn = value; 
            }
        }
        public Bank Bank
        {
            get { return bank; }
            set { bank = value; }
        }
        public Counterparty(string firmName, string country, string directorName,
                            string legalAddress, string phone, string email,
                            Bank bank, string inn)
        {
            FirmName = firmName;
            Country = country;
            DirectorName = directorName;
            LegalAddress = legalAddress;
            Phone= phone;
            Email = email;
            Bank = bank;
            INN = inn;
        }
        public string ToString(char c)
        {
            return $"{firmName}{c}{country}{c}{directorName}{c}{legalAddress}{c}{phone}{c}{email}{c}{bank.BankName}{c}{bank.BankAccount}{c}{inn}";
        }
        public string ToStringInfo()
        {
            return $"Фирма: {firmName}, страна: {country}, Руководитель: {directorName}.";
        }
    }
}
