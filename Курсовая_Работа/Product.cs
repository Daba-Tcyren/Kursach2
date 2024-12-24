using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая_Работа
{
    public enum ProductGroup
    {
        undefined,
        Vegetables,
        Fruits,
        Confectionery,
        MeatProducts,
        DairyProducts,
        BakeryProducts,
        Beverages,
        CannedGoods,
        Seafood,
        Groceries,
        FrozenFoods,
        Spices,
        OilsAndFats,
        Sweets,
        PreparedFoods        
    }
    public enum Unit
    {
        undefined,
        Kilogram,
        Piece,
        Pack,
        Liter,
        Gram,
        Milliliter,
        Box,
        Bottle,
        Can,
        Bag
    }
    public class Product
    {
        private string productName;
        private ProductGroup productGroup;
        private DateTime deliveryDate;
        private int shelfLife;
        private string manufacturer;
        private Unit unit;
        private int quantity;
        private decimal purchasePrice;
        private string barcode;
        private string photoPath;

        public string ProductName
        {
            get { return productName; }
            set 
            {
                if (value == "") throw new ArgumentException("Не введено название товара!");
                else productName = value; 
            }
        }
        public ProductGroup ProductGroup
        {
            get { return productGroup; }
            set 
            {
                if (value == ProductGroup.undefined) throw new ArgumentException("Не введено группа товара");
                else productGroup = value;
            }
        }
        public string DeliveryDate
        {
            get { return deliveryDate.ToShortDateString(); }
            set 
            {
                if(value == null) throw new ArgumentNullException("Не введено дата поставки!");
                if (!DateTime.TryParse(value, out DateTime s)) throw new ArgumentException("Проверьте выставленную дату");
                if (DateTime.Parse(value) > DateTime.Today) throw new ArgumentException("Дата поставки некорректна. Она превышает нынешнюю дату!");
                else deliveryDate = DateTime.Parse(value);
            }
        }
        public string ShelfLife
        {
            get { return Convert.ToString(shelfLife); }
            set 
            {
                if (value == "") throw new ArgumentException("Не введено срок хранения товара!");
                if (!int.TryParse(value, out int s)) throw new ArgumentException("Введите число в срок хранения товара!");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Срок хранения товара должно быть положительной!");
                else shelfLife = Convert.ToInt32(value);
            }
        }
        public string Manufacturer
        {
            get { return manufacturer; }
            set 
            {
                if (value == "") throw new ArgumentException("Не введено производитель товара!");
                else manufacturer = value; 
            }
        }
        public Unit Unit
        {
            get { return unit; }
            set 
            {
                if (value == Unit.undefined) throw new ArgumentException("Не введено учетная единица!");
                else unit = value;
            }
        }
        public string Quantity
        {
            get { return Convert.ToString(quantity); }
            set 
            {
                if (value == "") throw new ArgumentException("Не введите в количество товара!");
                if (!int.TryParse(value, out int s)) throw new ArgumentException("Введите число в количество товара!");
                if (Convert.ToInt32(value) == 0) throw new ArgumentException("Количество товаров не должно быть равно нулю!");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Количество товаров должно быть положительной");
                else quantity = Convert.ToInt32(value); 
            }
        }
        public string PurchasePrice
        {
            get { return Convert.ToString(purchasePrice); }
            set 
            {
                if (value == "") throw new ArgumentException("Не введите в закупочная цена!");
                if (!decimal.TryParse(value, out decimal s)) throw new ArgumentException("Введите число в закупочная цена!");
                if (Convert.ToInt32(value) == 0) throw new ArgumentException("Цена не должно быть равно нулю!");
                if (Convert.ToDecimal(value) < 0) throw new ArgumentException("Цена должна быть положительной!");
                else purchasePrice = Convert.ToDecimal(value); 
            }
        }
        public string Barcode
        {
            get { return barcode; }
            set 
            { 
                if (value == "") throw new ArgumentException("Не введен штрих-код!");
                else barcode = value;
            }
        }
        public string PhotoPath
        {
            get { return photoPath; }
            set 
            { 
                if (value == "") throw new ArgumentException("Не введен Фото товара!");
                else photoPath = value;
            }
        }
        public Product(string productName, ProductGroup productGroup, string deliveryDate,
                       string shelfLife, string manufacturer, Unit unit,
                       string quantity, string purchasePrice, string barcode, string photoPath)
        {
            ProductName = productName;
            ProductGroup = productGroup;
            DeliveryDate = deliveryDate;
            ShelfLife = shelfLife;
            Manufacturer = manufacturer;
            Unit = unit;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            Barcode = barcode;
            PhotoPath = photoPath;
        }
        public string ToString(char c)
        {
            return $"{ProductName}{c}{ProductGroup}{c}{DeliveryDate}{c}{ShelfLife}{c}{Manufacturer}{c}{Unit}{c}{Quantity}{c}{PurchasePrice}{c}{Barcode}{c}{PhotoPath}";
        }
        public string ToStringInfo()
        {
            return $"Товар: {ProductName}, группа товара: {ProductGroup}, Дата поставки: {DeliveryDate}, срок годности: {ShelfLife}, учетная единица: {Unit}, Количество: {quantity}, закупочная цена: {PurchasePrice}";
        }
    }
}
