using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Курсовая_Работа
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            File file = new File();
            file.Read();

            // Заполняем данные 
            GridWareHouse.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            for (int i = 0; i < Data.base_Product.Count; i++)
            {
                GridWareHouse.Rows.Add();
                GridWareHouse.Rows[i].Cells[0].Value = Data.base_Product[i].ProductName;
                GridWareHouse.Rows[i].Cells[1].Value = EnumToString(Data.base_Product[i].ProductGroup);
                GridWareHouse.Rows[i].Cells[2].Value = Data.base_Product[i].DeliveryDate;
                GridWareHouse.Rows[i].Cells[3].Value = Data.base_Product[i].ShelfLife;
                GridWareHouse.Rows[i].Cells[4].Value = Data.base_Product[i].Manufacturer;
                GridWareHouse.Rows[i].Cells[5].Value = EnumToString(Data.base_Product[i].Unit);
                GridWareHouse.Rows[i].Cells[6].Value = Data.base_Product[i].Quantity;
                GridWareHouse.Rows[i].Cells[7].Value = Data.base_Product[i].PurchasePrice;
                GridWareHouse.Rows[i].Cells[8].Value = Image.FromFile(Data.base_Product[i].Barcode);
                GridWareHouse.Rows[i].Cells[9].Value = Image.FromFile(Data.base_Product[i].PhotoPath);
            }
            GridTrsnsaction.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            foreach (Invoice invoice in Data.base_Invoice)
            {
                GridTrsnsaction.Rows.Add("Накладная", invoice.Counterparty.FirmName, invoice.Date, "\n" + invoice.ToStringInfo());
            }
            foreach (Order order in Data.base_Order)
            {
                GridTrsnsaction.Rows.Add("Заказ", order.Counterparty.FirmName, order.Date,"\n" + order.ToStringInfo());
            }
            GridCounterparty.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            foreach (Counterparty counterparty in Data.base_Counterparty)
            {
                GridCounterparty.Rows.Add(counterparty.FirmName, counterparty.Country, counterparty.DirectorName, counterparty.LegalAddress, counterparty.Phone,
                                          counterparty.Email, counterparty.Bank.BankName, counterparty.Bank.BankAccount, counterparty.INN);
            }
        }

        // 1 закладка - товары
        string fileBarcode = "";
        string filePhote = "";
        int rowIndexProduct = 0;
        private void buttonAddProduct_Click(object sender, EventArgs e)
        {
            buttonAddProduct.Visible = false;
            buttonRemoveProduct.Visible = true;
            panelProduct.Visible = true;

            textBoxNameProduct.Text = "";
            comboBoxProductGroup.SelectedIndex = 0;
            dateTimePickerDeliveryDate.Text = "";
            textBoxShelfLife.Text = "";
            textBoxManufactory.Text = "";
            comboBoxUnit.SelectedIndex = 0;
            textBoxCountProduct.Text = "";
            textBoxPurchasePrice.Text = "";

            pictureBoxBarcode.Image = null;
            pictureBoxPhote.Image = null;
            rowIndexProduct = GridWareHouse.Rows.Count;
            fileBarcode = "";
            filePhote = "";
        }
        private void buttonSaveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                Product newProduct = new Product(textBoxNameProduct.Text, (ProductGroup)(comboBoxProductGroup.SelectedIndex ), dateTimePickerDeliveryDate.Text,
                                                 (textBoxShelfLife.Text), textBoxManufactory.Text, (Unit)(comboBoxUnit.SelectedIndex ),
                                                 textBoxCountProduct.Text, textBoxPurchasePrice.Text, fileBarcode, filePhote);
                if (rowIndexProduct == GridWareHouse.Rows.Count)
                {
                    Data.base_Product.Add(newProduct);
                    GridWareHouse.Rows.Add();
                }
                else 
                { 
                    Data.base_Product.RemoveAt(rowIndexProduct);
                    Data.base_Product.Insert(rowIndexProduct, newProduct);
                    
                    if(rowIndexProduct == GridWareHouse.Rows.Count)
                    {
                        GridWareHouse.Rows.Add();
                        GridWareHouse.ReadOnly = false;
                    }
                }

                GridWareHouse.Rows[rowIndexProduct].Cells[0].Value = newProduct.ProductName;
                GridWareHouse.Rows[rowIndexProduct].Cells[1].Value = EnumToString(newProduct.ProductGroup);
                GridWareHouse.Rows[rowIndexProduct].Cells[2].Value = newProduct.DeliveryDate;
                GridWareHouse.Rows[rowIndexProduct].Cells[3].Value = newProduct.ShelfLife;
                GridWareHouse.Rows[rowIndexProduct].Cells[4].Value = newProduct.Manufacturer;
                GridWareHouse.Rows[rowIndexProduct].Cells[5].Value = EnumToString(newProduct.Unit);
                GridWareHouse.Rows[rowIndexProduct].Cells[6].Value = newProduct.Quantity;
                GridWareHouse.Rows[rowIndexProduct].Cells[7].Value = newProduct.PurchasePrice;
                GridWareHouse.Rows[rowIndexProduct].Cells[8].Value = System.Drawing.Image.FromFile(newProduct.Barcode);
                GridWareHouse.Rows[rowIndexProduct].Cells[9].Value = System.Drawing.Image.FromFile(newProduct.PhotoPath);

                buttonAddProduct.Visible = true;
                panelProduct.Visible = false;

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonRemoveProduct_Click(object sender, EventArgs e)
        {
            buttonAddProduct.Visible = true;
            panelProduct.Visible = false;
        }
        private void btnPhotoPath_Click(object sender, EventArgs e)
        {
            if (openFileDialogPhotePath.ShowDialog() == DialogResult.OK && openFileDialogPhotePath.FileName != "")
            {
                filePhote = openFileDialogPhotePath.FileName.ToString();
                pictureBoxPhote.Image = System.Drawing.Image.FromFile(openFileDialogPhotePath.FileName);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialogBarCode.ShowDialog() == DialogResult.OK && openFileDialogBarCode.FileName != "")
            {
                fileBarcode = openFileDialogBarCode.FileName;
                pictureBoxBarcode.Image = System.Drawing.Image.FromFile(openFileDialogBarCode.FileName);
            }
        }
        private void GridWareHouse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                buttonDeleteProduct.Visible = true;
                buttonRemoveProduct.Visible = false;
                buttonAddProduct.Visible = false;        
                rowIndexProduct = e.RowIndex;
                labelTitleProduct.Text = "Информация товара";
                panelProduct.Visible = true;

                textBoxNameProduct.Text = GridWareHouse.Rows[rowIndexProduct].Cells[0].Value.ToString();
                comboBoxProductGroup.SelectedIndex = comboBoxProductGroup.Items.IndexOf(GridWareHouse.Rows[rowIndexProduct].Cells[1].Value);
                dateTimePickerDeliveryDate.Text = GridWareHouse.Rows[rowIndexProduct].Cells[2].Value.ToString();
                textBoxShelfLife.Text = GridWareHouse.Rows[rowIndexProduct].Cells[3].Value.ToString();
                textBoxManufactory.Text = GridWareHouse.Rows[rowIndexProduct].Cells[4].Value.ToString();
                comboBoxUnit.SelectedIndex = comboBoxUnit.Items.IndexOf(GridWareHouse.Rows[rowIndexProduct].Cells[5].Value);
                textBoxCountProduct.Text = GridWareHouse.Rows[rowIndexProduct].Cells[6].Value.ToString();
                textBoxPurchasePrice.Text = GridWareHouse.Rows[rowIndexProduct].Cells[7].Value.ToString();
                foreach(Product product in Data.base_Product)
                {
                    if(product.ProductName == textBoxNameProduct.Text && product.ProductGroup == (ProductGroup)(comboBoxProductGroup.SelectedIndex) && product.DeliveryDate == dateTimePickerDeliveryDate.Text &&
                        product.ShelfLife == textBoxShelfLife.Text && product.Manufacturer == textBoxManufactory.Text && product.Unit == (Unit)(comboBoxUnit.SelectedIndex) && product.Quantity == textBoxCountProduct.Text && product.PurchasePrice == textBoxPurchasePrice.Text)
                    {
                        fileBarcode = product.Barcode;
                        pictureBoxBarcode.Image = (GridWareHouse.Rows[rowIndexProduct].Cells[8].Value as Bitmap);
                        filePhote = product.PhotoPath;
                        pictureBoxPhote.Image = (GridWareHouse.Rows[rowIndexProduct].Cells[9].Value as Bitmap);
                    }
                }
            }
        }
        private void buttonDeleteProduct_Click(object sender, EventArgs e)
        {
            buttonDeleteProduct.Visible = false;
            buttonAddProduct.Visible = true;
            panelProduct.Visible = false;

            if(rowIndexProduct != GridWareHouse.Rows.Count)
            {
                Data.base_Product.RemoveAt(rowIndexProduct);
                GridWareHouse.Rows.RemoveAt(rowIndexProduct);
            }
        }
        private string EnumToString(Enum enumType)
        {
            switch (enumType)
            {
                case Unit.undefined: return "Неопределено";
                case Unit.Kilogram: return "Килограмм";
                case Unit.Piece: return "Штука";
                case Unit.Pack: return "Упаковка";
                case Unit.Liter: return "Литр";
                case Unit.Gram: return "Грамм";
                case Unit.Milliliter: return "Миллилитр";
                case Unit.Box: return "Коробка";
                case Unit.Bottle: return "Бутылка";
                case Unit.Can: return "Банка";
                case Unit.Bag: return "пакетик";
                case ProductGroup.undefined: return "Неопределено";
                case ProductGroup.Vegetables: return "Овощи";
                case ProductGroup.Fruits: return "Фрукты";
                case ProductGroup.Confectionery: return "Кондитерские изделия";
                case ProductGroup.MeatProducts: return "Мясные изделия";
                case ProductGroup.DairyProducts: return "Молочные продукты";
                case ProductGroup.BakeryProducts: return "Хлебобулочные изделия";
                case ProductGroup.Beverages: return "Напитки";
                case ProductGroup.CannedGoods: return "Консервы";
                case ProductGroup.Seafood: return "Морепродукты";
                case ProductGroup.Groceries: return "Бакалея";
                case ProductGroup.FrozenFoods: return "Замороженные продукты";
                case ProductGroup.Spices: return "Специи";
                case ProductGroup.OilsAndFats: return "масла и жиры";
                case ProductGroup.Sweets: return "Сладости";
                case ProductGroup.PreparedFoods: return "Полуфабрикаты";

                default: return "Неопределено";
            }
        }

        // 2 закладка - операции
        int modetransaction;
        private void buttonAddTrans_Click(object sender, EventArgs e)
        {
            buttonAddTrans.Visible = false;
            panelMenu.Visible = true;

            List<string> listCP = new List<string> { "" };
            foreach (Counterparty CP in Data.base_Counterparty)
            {
                listCP.Add(CP.ToStringInfo());
            };
            comboBoxCounterpartyTransaction.DataSource = listCP;
            comboBoxCounterpartyTransaction.SelectedIndex = 0;
            dateTimTransaction.Text = "";
        }
        private void GridTrsnsaction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                buttonAddTrans.Visible = false;
                buttonLeaveTransaction.Visible = true;
                buttondeleteTransaction.Visible = true;

                rowIndexTransaction = e.RowIndex;                
            }            
        }

        private void buttonLeaveTransaction_Click(object sender, EventArgs e)
        {
            buttonAddTrans.Visible = true;
            buttonLeaveTransaction.Visible = false;
            buttondeleteTransaction.Visible = false;
        }

        private void buttondeleteTransaction_Click(object sender, EventArgs e)
        {
            buttonAddTrans.Visible = true;
            buttonLeaveTransaction.Visible = false;
            buttondeleteTransaction.Visible = false;

            
            if (rowIndexTransaction != GridTrsnsaction.Rows.Count)
            {
                int delIndex = -10;
                if (GridTrsnsaction.Rows[rowIndexTransaction].Cells[0].Value.ToString() == "Накладная") 
                {                   
                    foreach(Invoice Invoice in Data.base_Invoice)
                    {                        
                        if(Invoice.Counterparty.FirmName == GridTrsnsaction.Rows[rowIndexTransaction].Cells[1].Value.ToString() &&
                            Invoice.Date == GridTrsnsaction.Rows[rowIndexTransaction].Cells[2].Value.ToString() &&
                            "\n" +  Invoice.ToStringInfo() == GridTrsnsaction.Rows[rowIndexTransaction].Cells[3].Value.ToString())
                        {
                            //Invoice DelInvoice = Invoice;
                            delIndex = Data.base_Invoice.IndexOf(Invoice);
                        }
                    }
                    if (delIndex >= 0) Data.base_Invoice.RemoveAt(delIndex);
                }
                
                if (GridTrsnsaction.Rows[rowIndexTransaction].Cells[0].Value.ToString() == "Заказ") 
                { 
                    foreach(Order order in Data.base_Order)
                    {
                        if(order.Counterparty.FirmName == GridTrsnsaction.Rows[rowIndexTransaction].Cells[1].Value.ToString() &&
                            order.Date == GridTrsnsaction.Rows[rowIndexTransaction].Cells[2].Value.ToString() &&
                            "\n" + order.ToStringInfo() == GridTrsnsaction.Rows[rowIndexTransaction].Cells[3].Value.ToString())
                        {
                            delIndex =  Data.base_Order.IndexOf(order);
                        }
                    }
                    if (delIndex >= 0) Data.base_Order.RemoveAt(delIndex);
                }
                GridTrsnsaction.Rows.RemoveAt(rowIndexTransaction);
            }
        }
        // новый КонтраАгент
        private void buttonAddNewCp_Click(object sender, EventArgs e)
        {
            groupBoxCP.Visible = true;
            textBoxFirmNameCP.Text = "";
            textBoxCountryCP.Text = "";
            textBoxFullName.Text = "";
            textBoxAddress.Text = "";
            maskedTextBoxPhone.Text = "";
            textBoxEmailCP.Text = "";
            textBoxBankNameCP.Text = "";
            textBoxBankAccCP.Text = "";
            textBoxINNCP.Text = "";

            rowIndexCP = GridCounterparty.Rows.Count;
        }
        private void button5_Click(object sender, EventArgs e) // закрытие
        {
            groupBoxCP.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)// Добавление нового контрагента
        {
            try
            {                
                Counterparty newCP = new Counterparty(textBoxFirmNameCP.Text, textBoxCountryCP.Text, textBoxFullName.Text, textBoxAddress.Text, maskedTextBoxPhone.Text , textBoxEmailCP.Text, new Bank(textBoxBankNameCP.Text, textBoxBankAccCP.Text), textBoxINNCP.Text);

                int rowIndexCP = GridCounterparty.Rows.Count;
                Data.base_Counterparty.Add(newCP);
                GridCounterparty.Rows.Add();
                

                GridCounterparty.Rows[rowIndexCP].Cells[0].Value = newCP.FirmName;
                GridCounterparty.Rows[rowIndexCP].Cells[1].Value = newCP.Country;
                GridCounterparty.Rows[rowIndexCP].Cells[2].Value = newCP.DirectorName;
                GridCounterparty.Rows[rowIndexCP].Cells[3].Value = newCP.LegalAddress;
                GridCounterparty.Rows[rowIndexCP].Cells[4].Value = newCP.Phone;
                GridCounterparty.Rows[rowIndexCP].Cells[5].Value = newCP.Email;
                GridCounterparty.Rows[rowIndexCP].Cells[6].Value = newCP.Bank.BankName;
                GridCounterparty.Rows[rowIndexCP].Cells[7].Value = newCP.Bank.BankAccount;
                GridCounterparty.Rows[rowIndexCP].Cells[8].Value = newCP.INN;

                List<string> listCP = new List<string> { "" };
                foreach (Counterparty CP in Data.base_Counterparty)
                {
                    listCP.Add(CP.ToStringInfo());
                };
                comboBoxCounterpartyTransaction.DataSource = listCP;
                comboBoxCounterpartyTransaction.SelectedIndex = 0;
                groupBoxCP.Visible = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        
        // накладная
       
        private void buttonAddInvoice_Click(object sender, EventArgs e)
        {
            panelMenu.Visible = false;
            panelTransaction.Visible = true;

            modetransaction = 0;
            labelCP.Text = "Выберите поставщика:";
            buttonAddNewCp.Text = "Добавить нового поставщика";
            labelDate.Text = "Дата поставки:";

            groupBoxCP.Text = "Данные поставщика";
            groupBoxInvoice.Visible = true;
            receivedProducts.Clear();
            gridReceivedProducts.Rows.Clear();
            groupBoxInvoice.Text = $"Товар №1";
            rowsListProducts = 0;

            textBoxNameProductInvoice.Text = "";
            comboBoxGroupInvoice.SelectedIndex = 0;
            textBoxShelfLifeINvoice.Text = "";
            textBoxManufactInvoice.Text = "";
            comboBoxUnitInvoice.SelectedIndex = 0;
            textBoxCountInvoice.Text = "";
            textBoxPriceInvoice.Text = "";
            fileBarcode = "";
            filePhote = "";
            pictureBox1.Image = null;
            pictureBox2.Image = null;
        }
        
        private void buttonRemoveInvoice_Click(object sender, EventArgs e)
        {
            groupBoxInvoice.Visible = false;
            panelOrder.Visible = false;
            panelTransaction.Visible = false;
            buttonAddTrans.Visible = true;
        }
        
        int rowsListProducts = 0;
        int rowIndexTransaction = 0;
        List<ReceivedProduct> receivedProducts = new List<ReceivedProduct>();
        private void gridReceivedProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                buttonDeleteProductList.Visible = true;
                groupBoxInvoice.Text = $"Товар №{e.RowIndex + 1}";
                rowsListProducts = e.RowIndex;

                Product product = receivedProducts[rowsListProducts].Product;

                textBoxNameProductInvoice.Text = product.ProductName;
                comboBoxGroupInvoice.SelectedIndex = comboBoxGroupInvoice.Items.IndexOf(EnumToString(product.ProductGroup));
                textBoxShelfLifeINvoice.Text = product.ShelfLife;
                textBoxManufactInvoice.Text = product.Manufacturer;
                comboBoxUnitInvoice.SelectedIndex = comboBoxUnitInvoice.Items.IndexOf(EnumToString(product.Unit));
                textBoxCountInvoice.Text = product.Quantity;
                textBoxPriceInvoice.Text = product.PurchasePrice;
                fileBarcode = product.Barcode;
                filePhote = product.PhotoPath;
                pictureBox1.Image = Image.FromFile(fileBarcode);
                pictureBox2.Image = Image.FromFile(filePhote);
            }
        }
        private void buttonDeleteProductInvoice_Click(object sender, EventArgs e)
        {
            if (rowsListProducts != gridReceivedProducts.Rows.Count)
            {
                receivedProducts.RemoveAt(rowsListProducts);
                gridReceivedProducts.Rows.RemoveAt(rowsListProducts);
            }
            groupBoxInvoice.Text = $"Товар №{gridReceivedProducts.Rows.Count + 1}";

            textBoxNameProductInvoice.Text = "";
            comboBoxGroupInvoice.SelectedIndex = 0;
            textBoxShelfLifeINvoice.Text = "";
            textBoxManufactInvoice.Text = "";
            comboBoxUnitInvoice.SelectedIndex = 0;
            textBoxCountInvoice.Text = "";
            textBoxPriceInvoice.Text = "";
            fileBarcode = "";
            filePhote = "";
            rowsListProducts = gridReceivedProducts.Rows.Count;
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            buttonDeleteProductList.Visible = false;
        }
        private void buttonBarcodeInvoice_Click(object sender, EventArgs e)
        {
            if (openFileDialogBarCode.ShowDialog() == DialogResult.OK && openFileDialogBarCode.FileName != "")
            {
                fileBarcode = openFileDialogBarCode.FileName;
                pictureBox1.Image = System.Drawing.Image.FromFile(openFileDialogBarCode.FileName);
            }
        }

        private void buttonPhotoInvoice_Click(object sender, EventArgs e)
        {
            if (openFileDialogPhotePath.ShowDialog() == DialogResult.OK && openFileDialogPhotePath.FileName != "")
            {
                filePhote = openFileDialogPhotePath.FileName;
                pictureBox2.Image = System.Drawing.Image.FromFile(openFileDialogPhotePath.FileName);
            }
        }
        
        private void buttonNextProduct_Click(object sender, EventArgs e)
        {
            try
            {                   
                Product newProduct = new Product(textBoxNameProductInvoice.Text, (ProductGroup)(comboBoxGroupInvoice.SelectedIndex), dateTimTransaction.Text,
                                                 (textBoxShelfLifeINvoice.Text), textBoxManufactInvoice.Text, (Unit)(comboBoxUnitInvoice.SelectedIndex),
                                                 textBoxCountInvoice.Text, textBoxPriceInvoice.Text, fileBarcode, filePhote);
                
                
                if(rowsListProducts != gridReceivedProducts.Rows.Count)
                {
                    receivedProducts.RemoveAt(rowsListProducts);
                }
                else
                {
                    gridReceivedProducts.Rows.Add();
                }
                receivedProducts.Add(new ReceivedProduct(newProduct, newProduct.Quantity));

                gridReceivedProducts.Rows[rowsListProducts].Cells[0].Value = newProduct.ProductName;
                gridReceivedProducts.Rows[rowsListProducts].Cells[1].Value = newProduct.Quantity;


                groupBoxInvoice.Text = $"Товар №{receivedProducts.Count + 1}";

                textBoxNameProductInvoice.Text = "";
                comboBoxGroupInvoice.SelectedIndex = 0;
                textBoxShelfLifeINvoice.Text = "";
                textBoxManufactInvoice.Text = "";
                comboBoxUnitInvoice.SelectedIndex = 0;
                textBoxCountInvoice.Text = "";
                textBoxPriceInvoice.Text = "";
                fileBarcode = "";
                filePhote = "";
                rowsListProducts = gridReceivedProducts.Rows.Count;
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                buttonDeleteProductList.Visible = false;

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

                
        private void buttonSaveTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCounterpartyTransaction.SelectedIndex == 0) throw new ArgumentException("Выберите поставщика или введите нового!");
                if (modetransaction == 0) // накладная
                {
                    rowIndexProduct = GridWareHouse.Rows.Count;
                    rowIndexTransaction = GridTrsnsaction.Rows.Count;

                    Invoice invoice = new Invoice(Data.base_Counterparty[comboBoxCounterpartyTransaction.SelectedIndex - 1], dateTimTransaction.Text, receivedProducts);

                    for (int i = 0; i < receivedProducts.Count; i++)
                    {
                        Data.base_Product.Add(receivedProducts[i].Product);
                        GridWareHouse.Rows.Add();
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[0].Value = receivedProducts[i].Product.ProductName;
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[1].Value = EnumToString(receivedProducts[i].Product.ProductGroup);
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[2].Value = receivedProducts[i].Product.DeliveryDate;
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[3].Value = receivedProducts[i].Product.ShelfLife;
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[4].Value = receivedProducts[i].Product.Manufacturer;
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[5].Value = EnumToString(receivedProducts[i].Product.Unit);
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[6].Value = receivedProducts[i].Product.Quantity;
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[7].Value = receivedProducts[i].Product.PurchasePrice;
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[8].Value = System.Drawing.Image.FromFile(receivedProducts[i].Product.Barcode);
                        GridWareHouse.Rows[i + rowIndexProduct].Cells[9].Value = System.Drawing.Image.FromFile(receivedProducts[i].Product.PhotoPath);
                    }
                    Data.base_Invoice.Add(invoice);

                    GridTrsnsaction.Rows.Add();
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[0].Value = "Накладная";
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[1].Value = invoice.Counterparty.FirmName;
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[2].Value = invoice.Date;
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[3].Value = "\n" + invoice.ToStringInfo();

                    groupBoxInvoice.Visible = false;
                    buttonAddTrans.Visible = true;
                    panelTransaction.Visible = false;

                }
                if (modetransaction == 1) // заказ
                { 
                    if (soldProducts.Count == 0) throw new ArgumentException("Список товаров в заказ пустой!");

                    rowsListProductsOrder = dataGridView1.Rows.Count;
                    rowIndexTransaction = GridTrsnsaction.Rows.Count;

                    Order order = new Order(Data.base_Counterparty[comboBoxCounterpartyTransaction.SelectedIndex - 1], dateTimTransaction.Text, soldProducts);

                    // изменения в складе
                    for (int i = 0; i < soldProducts.Count; i++)
                    {
                        int IndexBaseProduct = Data.base_Product.IndexOf(soldProducts[i].Product);
                        if (soldProducts[i].Quantity == soldProducts[i].Product.Quantity) Data.base_Product.RemoveAt(IndexBaseProduct);
                        else
                        {
                            int remains = Convert.ToInt32(soldProducts[i].Product.Quantity) - Convert.ToInt32(soldProducts[i].Quantity);
                            Data.base_Product[IndexBaseProduct].Quantity = Convert.ToString(remains);
                        }
                    }
                    GridWareHouse.Rows.Clear();
                    for (int i = 0; i < Data.base_Product.Count; i++)
                    {
                        GridWareHouse.Rows.Add();
                        GridWareHouse.Rows[i].Cells[0].Value = Data.base_Product[i].ProductName;
                        GridWareHouse.Rows[i].Cells[1].Value = EnumToString(Data.base_Product[i].ProductGroup);
                        GridWareHouse.Rows[i].Cells[2].Value = Data.base_Product[i].DeliveryDate;
                        GridWareHouse.Rows[i].Cells[3].Value = Data.base_Product[i].ShelfLife;
                        GridWareHouse.Rows[i].Cells[4].Value = Data.base_Product[i].Manufacturer;
                        GridWareHouse.Rows[i].Cells[5].Value = EnumToString(Data.base_Product[i].Unit);
                        GridWareHouse.Rows[i].Cells[6].Value = Data.base_Product[i].Quantity;
                        GridWareHouse.Rows[i].Cells[7].Value = Data.base_Product[i].PurchasePrice;
                        GridWareHouse.Rows[i].Cells[8].Value = Image.FromFile(Data.base_Product[i].Barcode);
                        GridWareHouse.Rows[i].Cells[9].Value = Image.FromFile(Data.base_Product[i].PhotoPath);
                    }

                    Data.base_Order.Add(order);


                    GridTrsnsaction.Rows.Add();
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[0].Value = "Заказ";
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[1].Value = order.Counterparty.FirmName;
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[2].Value = order.Date;
                    GridTrsnsaction.Rows[rowIndexTransaction].Cells[3].Value = "\n" + order.ToStringInfo();

                    panelOrder.Visible = false;
                    buttonAddTrans.Visible = true;
                    panelTransaction.Visible = false;
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // заказ
        int rowsListProductsOrder = 0; // для отслеживания в таблице по товары склада
        private void buttonAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Data.base_Product.Count == 0) throw new ArgumentException("Склад пустой! Невозможно оформить заказ!");
                panelMenu.Visible = false;
                panelTransaction.Visible = true;
                panelOrder.Visible = true ;
                mod = 0;
                modetransaction = 1;
                labelCP.Text = "Выберите заказчика:";
                buttonAddNewCp.Text = "Добавить нового заказчика";
                labelDate.Text = "Дата заказа:";
                groupBoxCP.Text = "Данные заказщика";

                rowsOrderList = 0;
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                rowsListProductsOrder = 0;
                foreach(Product product in Data.base_Product)
                {
                    dataGridView2.Rows.Add(product.ProductName, product.DeliveryDate, EnumToString(product.ProductGroup), product.ShelfLife, product.Manufacturer,
                                            EnumToString(product.Unit), product.Quantity, product.PurchasePrice);
                }
                for(int i = 0; i < Data.base_Product.Count; i++)
                {
                    dataGridView2.Rows[i].Cells[8].Value = Image.FromFile(Data.base_Product[i].Barcode);
                    dataGridView2.Rows[i].Cells[9].Value = Image.FromFile(Data.base_Product[i].PhotoPath);                
                }
                dataGridView1.Rows.Clear();
                panel1.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                soldProducts.Clear();
                textBox4.Text = "";
                textBox3.Text = "";
                dateTimePicker1.Text = "";
                comboBox2.SelectedIndex = 0;
                textBox5.Text = "";
                textBox1.Text = "";
                comboBox1.SelectedIndex = 0;
                textBox2.Text = "";
                pictureBox4.Image = null;
                pictureBox3.Image = null;
                fileBarcode = "";
                filePhote = "";

                
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) // таблица склада
        {
            if(e.RowIndex >= 0)
            {
                buttonCompleteOrder.Visible = true;
                mod = 0;

                rowsListProductsOrder = e.RowIndex;

                rowsOrderList = soldProducts.Count;

                panel1.Visible = true;
                pictureBox3.Visible = true;
                pictureBox4.Visible = true;
                label50.Visible = true;
                textBox6.Visible = true;

                textBox6.Text = "";

                textBox4.Text = Data.base_Product[rowsListProductsOrder].ProductName;
                textBox3.Text = Data.base_Product[rowsListProductsOrder].Quantity;
                dateTimePicker1.Text = Data.base_Product[rowsListProductsOrder].DeliveryDate;
                comboBox2.SelectedIndex = comboBox2.Items.IndexOf(EnumToString(Data.base_Product[rowsListProductsOrder].ProductGroup));
                textBox5.Text = Data.base_Product[rowsListProductsOrder].ShelfLife;
                textBox1.Text = Data.base_Product[rowsListProductsOrder].Manufacturer;
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(EnumToString(Data.base_Product[rowsListProductsOrder].Unit));
                textBox2.Text = Data.base_Product[rowsListProductsOrder].PurchasePrice;
                pictureBox4.Image = Image.FromFile(Data.base_Product[rowsListProductsOrder].Barcode);
                pictureBox3.Image = Image.FromFile(Data.base_Product[rowsListProductsOrder].PhotoPath);

            }
        }        
        int rowsOrderList = 0; // для отслеживания индекса таблицы по товарам в заказ
        List<SoldProduct> soldProducts = new List<SoldProduct>();
        int mod = 0;
        private void buttonCompleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (panel1.Visible == false) throw new ArgumentException("Выберите товар из таблицы!");
                Product product;
                if(mod == 2) product = soldProducts[rowsOrderList].Product; 
                else  product = Data.base_Product[rowsListProductsOrder]; 
                if(DateTime.Parse(dateTimTransaction.Text) < DateTime.Parse(product.DeliveryDate)) throw new ArgumentException("Дата заказа должка быть позже дата поставки товара!");
                SoldProduct soldProduct = new SoldProduct(product, textBox6.Text, "1");                 
                if (Convert.ToInt32(textBox6.Text) > Convert.ToInt32(soldProduct.Product.Quantity)) throw new ArgumentException("Количество товара в заказ не должен превышать количество товара на складе!");
                if (mod == 0)
                {
                    foreach (SoldProduct soldProduct1 in soldProducts)
                    {
                        if (soldProduct.Product.Equals(soldProduct1.Product))
                        {
                            mod = 1;
                            int result = Convert.ToInt32(soldProduct.Quantity) + Convert.ToInt32(soldProduct1.Quantity);
                            if (result > Convert.ToInt32(soldProduct.Product.Quantity)) result = Convert.ToInt32(soldProduct1.Product.Quantity);
                            soldProduct.Quantity = Convert.ToString(result);
                            rowsOrderList = soldProducts.IndexOf(soldProduct1);
                        }
                    }
                }
                
                
                decimal price = 1.05M * Convert.ToDecimal(soldProduct.Quantity) * Convert.ToDecimal(soldProduct.Product.PurchasePrice);
                soldProduct.SellingPrice = Convert.ToString(price);                
                
                if(rowsOrderList != dataGridView1.Rows.Count || mod == 1 || mod == 2)
                {
                    soldProducts.RemoveAt(rowsOrderList);
                    soldProducts.Insert(rowsOrderList, soldProduct);
                }
                else
                {
                    dataGridView1.Rows.Add();
                    soldProducts.Add(soldProduct);
                }
                                

                dataGridView1.Rows[rowsOrderList].Cells[0].Value = soldProducts[rowsOrderList].Product.ProductName;
                dataGridView1.Rows[rowsOrderList].Cells[1].Value = soldProducts[rowsOrderList].Quantity;
                dataGridView1.Rows[rowsOrderList].Cells[2].Value = soldProducts[rowsOrderList].SellingPrice;

                panel1.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                label50.Visible = false;
                textBox6.Visible = false;
                buttonRemoveProductOrder.Visible = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
        private void buttonRemoveProductOrder_Click(object sender, EventArgs e) // удаление из таблицы заказов
        {
            buttonRemoveProductOrder.Visible = false;
            panel1.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            label50.Visible = false;
            textBox6.Visible = false;

            if (rowsOrderList != dataGridView1.Rows.Count)
            {
                soldProducts.RemoveAt(rowsOrderList);
                dataGridView1.Rows.RemoveAt(rowsOrderList);
            }
        }  
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e) // таблица товаров в заказ
        {
            if(e.RowIndex >= 0)
            {
                rowsOrderList = e.RowIndex;
                mod = 2;
                panel1.Visible = true;
                pictureBox3.Visible = true;
                pictureBox4.Visible = true;
                label50.Visible = true;
                textBox6.Visible = true;
                buttonRemoveProductOrder.Visible = true;

                textBox6.Text = soldProducts[rowsOrderList].Quantity;

                textBox4.Text = soldProducts[rowsOrderList].Product.ProductName;
                textBox3.Text = soldProducts[rowsOrderList].Product.Quantity;
                dateTimePicker1.Text = soldProducts[rowsOrderList].Product.DeliveryDate;
                comboBox2.SelectedIndex = comboBox2.Items.IndexOf( EnumToString(soldProducts[rowsOrderList].Product.ProductGroup));
                textBox5.Text = soldProducts[rowsOrderList].Product.ShelfLife;
                textBox1.Text = soldProducts[rowsOrderList].Product.Manufacturer;
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(EnumToString(soldProducts[rowsOrderList].Product.Unit));
                textBox2.Text = soldProducts[rowsOrderList].Product.PurchasePrice;
                pictureBox4.Image = Image.FromFile(soldProducts[rowsOrderList].Product.Barcode);
                pictureBox3.Image = Image.FromFile(soldProducts[rowsOrderList].Product.PhotoPath);
            }
        }       

        // 3 закладка - контрагенты
        int rowIndexCP = 0;
        private void BtnAddCounterparty_Click(object sender, EventArgs e)
        {
            InputCounterparty.Visible = true;
            btnRemoveCP.Visible = true;
            BtnAddCounterparty.Visible = false;

            tBFirmName.Text = "";
            tBCountry.Text = "";
            tBFullNameDirector.Text = "";
            tBAddress.Text = "";
            tBPhone.Text = "";
            tBEmail.Text = "";
            tBNameBank.Text = "";
            tBBancAcc.Text = "";
            tBINN.Text = "";
            label51.Text = ""; 
            label52.Text = "";
            label53.Text =  "Добавление контрагента";
            rowIndexCP = GridCounterparty.Rows.Count;

        }

        private void btnRemoveCP_Click(object sender, EventArgs e)
        {
            InputCounterparty.Visible = false;
            BtnAddCounterparty.Visible = true;
        }

        private void buttonSaveCounterparty_Click(object sender, EventArgs e)
        {
            try
            {
                Bank b = new Bank(tBNameBank.Text, tBBancAcc.Text);
                Counterparty newCP = new Counterparty(tBFirmName.Text, tBCountry.Text, tBFullNameDirector.Text, tBAddress.Text, tBPhone.Text, tBEmail.Text, b, tBINN.Text);

                if (rowIndexCP == GridCounterparty.Rows.Count)
                {
                    Data.base_Counterparty.Add(newCP);
                    GridCounterparty.Rows.Add();
                }
                else 
                {
                    Data.base_Counterparty.RemoveAt(rowIndexCP);
                    Data.base_Counterparty.Insert(rowIndexCP, newCP);
                    if (rowIndexCP == GridCounterparty.Rows.Count)
                    {
                        GridCounterparty.Rows.Add();
                        GridCounterparty.ReadOnly = false;
                    }
                }

                GridCounterparty.Rows[rowIndexCP].Cells[0].Value = newCP.FirmName;
                GridCounterparty.Rows[rowIndexCP].Cells[1].Value = newCP.Country;
                GridCounterparty.Rows[rowIndexCP].Cells[2].Value = newCP.DirectorName;
                GridCounterparty.Rows[rowIndexCP].Cells[3].Value = newCP.LegalAddress;
                GridCounterparty.Rows[rowIndexCP].Cells[4].Value = newCP.Phone;
                GridCounterparty.Rows[rowIndexCP].Cells[5].Value = newCP.Email;
                GridCounterparty.Rows[rowIndexCP].Cells[6].Value = newCP.Bank.BankName;
                GridCounterparty.Rows[rowIndexCP].Cells[7].Value = newCP.Bank.BankAccount;
                GridCounterparty.Rows[rowIndexCP].Cells[8].Value = newCP.INN;

                InputCounterparty.Visible = false;
                BtnAddCounterparty.Visible = true;
            }
            catch (ArgumentException ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tBBancAcc_TextChanged(object sender, EventArgs e)
        {
            label51.Text = $"{tBBancAcc.Text.Count()}/9";
        }

        private void tBINN_TextChanged(object sender, EventArgs e)
        {
            label52.Text = $"{tBINN.Text.Count()}/10";
        }

        private void GridCounterparty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                label53.Text = "Информация о контрагенте";
                btnRemoveCP.Visible = false;
                button3.Visible = true;
                BtnAddCounterparty.Visible = false;
                InputCounterparty.Visible = true;

                rowIndexCP = e.RowIndex;

                tBFirmName.Text = GridCounterparty.Rows[rowIndexCP].Cells[0].Value.ToString();
                tBCountry.Text = GridCounterparty.Rows[rowIndexCP].Cells[1].Value.ToString();
                tBFullNameDirector.Text = GridCounterparty.Rows[rowIndexCP].Cells[2].Value.ToString();
                tBAddress.Text = GridCounterparty.Rows[rowIndexCP].Cells[3].Value.ToString();
                tBPhone.Text = GridCounterparty.Rows[rowIndexCP].Cells[4].Value.ToString();
                tBEmail.Text = GridCounterparty.Rows[rowIndexCP].Cells[5].Value.ToString();
                tBNameBank.Text = GridCounterparty.Rows[rowIndexCP].Cells[6].Value.ToString();
                tBBancAcc.Text = GridCounterparty.Rows[rowIndexCP].Cells[7].Value.ToString();
                tBINN.Text = GridCounterparty.Rows[rowIndexCP].Cells[8].Value.ToString();
            }            
        }
        private void button3_Click(object sender, EventArgs e) // удаление из таблицы
        {
            InputCounterparty.Visible = false;
            button3.Visible = false;
            BtnAddCounterparty.Visible = true;
            if (rowIndexCP != GridCounterparty.Rows.Count)
            {
                Data.base_Counterparty.RemoveAt(rowIndexCP);
                GridCounterparty.Rows.RemoveAt(rowIndexCP);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            File file = new File();
            file.Record();
        }
    }    
}
