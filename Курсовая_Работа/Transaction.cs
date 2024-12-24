using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая_Работа
{
     public class Transaction
     {
        private Counterparty counterparty;
        private DateTime date;
        public Counterparty Counterparty
        {
            get { return counterparty; }
            set { counterparty = value; }
        }
        public string Date
        {
            get { return date.ToShortDateString(); }
            set {
                if (value == null) throw new ArgumentNullException("Не введена дата операции!");
                if (!DateTime.TryParse(value, out DateTime s)) throw new ArgumentException("Проверьте выставленную дату");
                if (DateTime.Parse(value) > DateTime.Today) throw new ArgumentException("Дата операции некорректна. Она превышает нынешнию дату!");
                else date = DateTime.Parse(value);
            }
        }
        public Transaction(Counterparty counterparty, string date)
        {
            Counterparty = counterparty;
            Date = date;
        }
        public virtual string ToString(char c)
        {
            return $"{counterparty.ToString(c)}{c}{date}";
        }
    }
    public class Order: Transaction
    {
        private List<SoldProduct> soldProducts;
       
        public List<SoldProduct> SoldProducts
        {
            get { return soldProducts; }
            set 
            { 
                if(value == null) throw new ArgumentException("Пустой список заказов!");
                if (value.Count == 0) throw new ArgumentException("Пустой список заказов!");
                else soldProducts = value; 
            }
        }

        public Order(Counterparty counterparty, string date, List<SoldProduct> soldProducts): base(counterparty, date)
        {
            SoldProducts = soldProducts;
        }

        public override string ToString(char c) 
        {
            string result = Counterparty.ToString('$') + '|' + Date;
            foreach (SoldProduct soldProduct in soldProducts) result += c + soldProduct.ToString(c); 
            return result;
        }
        public string ToStringInfo()
        {
            string result = "";
            foreach(SoldProduct soldProduct in SoldProducts)
            {
                result += soldProduct.ToStringInfo();
                result += "\n";
            }
            return result;
        }
    }
    
    public class Invoice: Transaction
    {
        private List<ReceivedProduct> receivedProducts;
        public List<ReceivedProduct> ReceivedProducts
        {
            get { return receivedProducts; }
            set 
            { 
                if(value == null) throw new ArgumentException("Пустой список поставок!");
                if(value.Count == 0) throw new ArgumentException("Пустой список поставок!");
                else receivedProducts = value; 
            }
        }
        public Invoice(Counterparty counterparty, string date, List<ReceivedProduct> receivedProducts): base(counterparty, date)
        {
            ReceivedProducts= receivedProducts;
        }
        public override string ToString(char c)
        {             
            string result = Counterparty.ToString('$') + '|' + Date;
            foreach(ReceivedProduct receivedProduct in ReceivedProducts) result += c + receivedProduct.ToString(c);            
            return result;
        }
        public string ToStringInfo()
        {
            string result = "";
            foreach (ReceivedProduct receivedProduct in receivedProducts)
            {
                result += receivedProduct.ToStringInfo();
                result += "\n";
            }
            return result;
        }
    }

    public class ReceivedProduct
    {
        private Product product;
        private int quantity;
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }
        public string Quantity
        {
            get { return Convert.ToString(quantity); }
            set
            {
                if (value == null) throw new ArgumentNullException("Не введено количество товара в список!");
                if (!int.TryParse(value, out quantity)) throw new ArgumentException("Введите число в количество товара в список!");
                if (Convert.ToInt32(value) == 0) throw new ArgumentException("Количество товаров не должно быть равно нулю!");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Количество товаров должно быть положительной!");
                else quantity = Convert.ToInt32(value);
            }
        }
        public ReceivedProduct(Product product, string quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public virtual string ToString(char c)
        {
            return $"{Product.ToString(c)}{c}{Quantity}";
        }
        public virtual string ToStringInfo()
        {
            return $"Продукт: {product.ProductName} - Количество: {quantity}";
        }
    }
    public class SoldProduct: ReceivedProduct
    {
        private decimal sellingPrice;
        public string SellingPrice
        {
            get { return Convert.ToString(sellingPrice); }
            set 
            {
                if (value == "") throw new ArgumentException("Не введено закупочная цена!");
                if (!decimal.TryParse(value, out decimal s)) throw new ArgumentException("Введите число в закупочная цена!");
                if (Convert.ToDecimal(value) < 0) throw new ArgumentException("Цена должна быть положительной!");
                else sellingPrice = Convert.ToDecimal(value);
            }
        }
        public SoldProduct(Product product, string quantity, string sellingPrice): base(product, quantity)
        {
            SellingPrice = sellingPrice;
        }
        public override string ToString(char c)
        {
            return $"{Product.ToString(c)}{c}{Quantity}{c}{SellingPrice}";
        }
        public override string ToStringInfo()
        {
            return $"Продукт: {Product.ProductName} - Количество: {Quantity} - Отпускная цена: {SellingPrice}"; 
        }
    }
}
