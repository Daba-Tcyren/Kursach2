using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Курсовая_Работа
{
    public class File
    {
        private string path = "BaseData1.txt";

        public void Record()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    writer.WriteLine("--Product--");
                    foreach (var product in Data.base_Product)
                    {
                        writer.WriteLine(product.ToString('|'));
                    }
                    writer.WriteLine("--Invoice--");
                    foreach (var invoice in Data.base_Invoice)
                    {
                        writer.WriteLine(invoice.ToString('|'));
                    }
                    writer.WriteLine("--Order--");
                    foreach (var order in Data.base_Order)
                    {
                        writer.WriteLine(order.ToString('|'));
                    }
                    writer.WriteLine("--Counterparty--");
                    foreach (var counterparty in Data.base_Counterparty)
                    {
                        writer.WriteLine(counterparty.ToString('|'));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Read()
        {
            try
            {
                using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                {
                    string line;
                    int key = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "--Product--")
                        {
                            key = 1;
                            continue;
                        }
                        if (line == "--Invoice--")
                        {
                            key = 2;
                            continue;
                        }
                        if (line == "--Order--")
                        {
                            key = 3;
                            continue;
                        }
                        if (line == "--Counterparty--")
                        {
                            key = 4;
                            continue;
                        }

                        switch (key)
                        {
                            case 1:
                                string[] productParts = line.Split('|');
                                Product product = new Product(productParts[0], (ProductGroup)Enum.Parse(typeof(ProductGroup), productParts[1]),
                                                            (productParts[2]), (productParts[3]),
                                                            productParts[4], (Unit)Enum.Parse(typeof(Unit), productParts[5]),
                                                            (productParts[6]), (productParts[7]),
                                                            productParts[8], productParts[9]);
                                Data.base_Product.Add(product);
                                break;
                            case 2:
                                string[] invoiceParts = line.Split('|');
                                string[] counterpartyParts = invoiceParts[0].Split('$');
                                Bank bank = new Bank(counterpartyParts[6], counterpartyParts[7]);
                                Counterparty counterparty = new Counterparty(counterpartyParts[0], counterpartyParts[1], counterpartyParts[2],
                                                                             counterpartyParts[3], counterpartyParts[4], counterpartyParts[5],
                                                                             bank, counterpartyParts[8]);
                                List<ReceivedProduct> receivedProducts = new List<ReceivedProduct>();
                                for (int i = 2; i < invoiceParts.Length; i += 11)
                                {                                   
                                    Product productInvoice = new Product(invoiceParts[i], (ProductGroup)Enum.Parse(typeof(ProductGroup), invoiceParts[i + 1]),
                                                                         invoiceParts[i + 2], invoiceParts[i + 3], invoiceParts[i+4], (Unit)Enum.Parse(typeof(Unit), invoiceParts[i + 5]),
                                                                         invoiceParts[i + 6], invoiceParts[i + 7], invoiceParts[i + 8], invoiceParts[i + 9]);
                                    ReceivedProduct receivedProduct = new ReceivedProduct( productInvoice,invoiceParts[i + 10]) ;
                                    receivedProducts.Add(receivedProduct);
                                }
                                Invoice invoice = new Invoice(counterparty, invoiceParts[1], receivedProducts);
                                Data.base_Invoice.Add(invoice);
                                break;
                            case 3:
                                string[] orderParts = line.Split('|');
                                counterpartyParts = orderParts[0].Split('$');
                                bank = new Bank(counterpartyParts[6], counterpartyParts[7]);
                                counterparty = new Counterparty(counterpartyParts[0], counterpartyParts[1], counterpartyParts[2],
                                                                             counterpartyParts[3], counterpartyParts[4], counterpartyParts[5],
                                                                             bank, counterpartyParts[8]);
                                List<SoldProduct> soldProducts = new List<SoldProduct>();
                                for (int i = 2; i < orderParts.Length; i += 12)
                                {
                                    Product productOrder = new Product(orderParts[i], (ProductGroup)Enum.Parse(typeof(ProductGroup), orderParts[i + 1]),
                                                                         orderParts[i + 2], orderParts[i + 3], orderParts[i + 4], (Unit)Enum.Parse(typeof(Unit), orderParts[i + 5]),
                                                                         orderParts[i + 6], orderParts[i + 7], orderParts[i + 8], orderParts[i + 9]);
                                    SoldProduct soldProduct = new SoldProduct ( productOrder,  orderParts[i + 10],  orderParts[i + 11]);
                                    soldProducts.Add(soldProduct);
                                }
                                Order order = new Order(counterparty, (orderParts[1]), soldProducts);
                                Data.base_Order.Add(order);
                                break;
                            case 4:
                                string[] counterpartyParts1 = line.Split('|');
                                bank = new Bank(counterpartyParts1[6], counterpartyParts1[7]);
                                counterparty = new Counterparty(counterpartyParts1[0], counterpartyParts1[1], counterpartyParts1[2],
                                                                             counterpartyParts1[3], counterpartyParts1[4], counterpartyParts1[5],
                                                                             bank, counterpartyParts1[8]);
                                Data.base_Counterparty.Add(counterparty);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}
