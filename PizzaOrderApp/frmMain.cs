using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Reflection;

namespace PizzaOrderApp
{
    public partial class frmMain : Form
    {
        //Datatable object
        private DataTable dtPizzas;

        // Connect to database
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ASUS\Documents\PizzaOrderApp\PizzaOrderApp\LoginDB.mdf;Integrated Security=True;Connect Timeout=30");
        
        //Global Tax variable
        Double iTax = 15;

        //Initialize mainform
        public frmMain()
        {
            InitializeComponent();
        }

        //Methods and Functions
        public void SaveData()
        {
            Dtime();

            try
            {

                // Save entered data
                con.Open();
                String query = "INSERT INTO Order_Systems (Table_No, Customer_Name, Phone_Number, Order_Date, Order_Time, Net_Total) VALUES ('" + table_NoTextBox.Text + "','" + customer_NameTextBox.Text + "','" + phone_NumberTextBox.Text + "','" + order_DateTextBox.Text + "', '" + order_TimeTextBox.Text + "', '" + net_TotalTextBox.Text + "')";
                SqlDataAdapter SDA = new SqlDataAdapter(query, con);
                SDA.SelectCommand.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Saved Successfully!");
                this.Validate();
                this.order_SystemsBindingSource.EndEdit();
            }
            catch (SqlException)
            {
                
                throw new Exception ("Please make sure that phone and table numbers contains no letters or symbols");

            }
            finally
            {
                con.Close();
            }
        }
        private void Dtime()
        {
            // Set date and time
            DateTime iDate = DateTime.Now;
            order_DateTextBox.Text = iDate.ToString("dd/MM/yyyy");
            order_TimeTextBox.Text = iDate.ToString("HH:mm");
        }
        private DataTable GetData()
        {
            dtPizzas = new DataTable();
            dtPizzas.Columns.Add("Pizza", typeof(string));

            // Add data
            dtPizzas.Rows.Add("Bbq");
            dtPizzas.Rows.Add("Chicken");
            dtPizzas.Rows.Add("Regina");
            dtPizzas.Rows.Add("California");
            dtPizzas.Rows.Add("Steak");
            dtPizzas.Rows.Add("Tropical");
            dtPizzas.Rows.Add("Salami");

            return dtPizzas;

        }
        protected virtual void OnLoad()
        {

            // Set default value
            qtyTextBox.Text = "0";
            customer_NameTextBox.Text = "";
            item_PriceTextBox.Text = "0";
            sub_TotalTextBox.Text = "0";
            basketTotal.Text = "0";
            taxTextBox.Text = "15%";
            phone_NumberTextBox.Text = "";
            net_TotalTextBox.Text = "0";
            sub_TotalTextBox.ReadOnly = true;
            basketTotal.ReadOnly=true;
            taxTextBox.ReadOnly = true;
            net_TotalTextBox.ReadOnly = true;
            basket_Summary.ReadOnly = true;
            txtReceipt.ReadOnly = true;
            txtReceipt.BackColor = System.Drawing.SystemColors.Window;
            order_SystemsDataGridView.ReadOnly = true;

            // Load data in listbox
            pizzaList.DataSource = GetData();
            pizzaList.DisplayMember = "Pizza";

            // Clear datagridview
            order_SystemsDataGridView.DataSource = null;
        } 
        protected virtual void ValidateData()
        {
            this.Validate();
            this.order_SystemsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.loginDBDataSet);
        }
        protected virtual void ResetFields()
        {
            // Clear fields
            qtyTextBox.Text = "0";
            item_PriceTextBox.Text = "0";
            lbl_Search.Text = "";
            sub_TotalTextBox.Text = "";
            taxTextBox.Text = "";
            net_TotalTextBox.Text = "";
            customer_NameTextBox.Text = "";
            phone_NumberTextBox.Text = "";
            table_NoTextBox.Text = "";
            basket_Summary.Text = "";
            basketTotal.Text = "0";
            sub_TotalTextBox.Text = "0";
            taxTextBox.Text = "15%";

            order_SystemsDataGridView.DataSource = null;
        }
        protected virtual void DisplaySlip()
        {
            // Clear receipt
            txtReceipt.Text = "";

            // Open tab on click
            tabControl1.SelectedTab = Receipt;

            // View scrollbar
            txtReceipt.ScrollBars = ScrollBars.Vertical;

            // Display Receipt
            txtReceipt.AppendText("\t\t" + "       PIZZA HUB");
            txtReceipt.AppendText("\t\t\t" + "==============================================" + Environment.NewLine);
            txtReceipt.AppendText(" " + Environment.NewLine);

            txtReceipt.AppendText("Name:" + "\t" + customer_NameTextBox.Text + "\t" + "Phone No: " + phone_NumberTextBox.Text + Environment.NewLine);
            txtReceipt.AppendText(Environment.NewLine + "Table:" + "\t" + table_NoTextBox.Text + Environment.NewLine);
            txtReceipt.AppendText("Date:" + "\t" + order_DateTextBox.Text + Environment.NewLine);
            txtReceipt.AppendText("Time" + "\t" + order_TimeTextBox.Text + Environment.NewLine);

            txtReceipt.AppendText(Environment.NewLine + "Qty " + "\t" + "Pizza Type " + "\t" + "Item Price " + Environment.NewLine);
            txtReceipt.AppendText(Environment.NewLine + basket_Summary.Text + Environment.NewLine);

            txtReceipt.AppendText(Environment.NewLine + "\t" + "Order Sub Total:" + "\t" + String.Format("{0:0.00}", Double.Parse(sub_TotalTextBox.Text)) + Environment.NewLine);
            txtReceipt.AppendText(Environment.NewLine + "\t" + "Tax on Order:" + "\t" + taxTextBox.Text + Environment.NewLine);
            txtReceipt.AppendText(Environment.NewLine + "\t" + "Net Total:     " + "\t" + "R" + net_TotalTextBox.Text + Environment.NewLine);

            txtReceipt.AppendText("\t\t\t" + "==============================================" + Environment.NewLine);
            txtReceipt.AppendText("\t\t" + "       RECEIPT" + Environment.NewLine);
        }
        protected virtual void OnSave()
        {

            validateInputs(customer_NameTextBox.Text, phone_NumberTextBox.Text, table_NoTextBox.Text);
            SaveData();

        }
        protected virtual void validateInputs (string name, string number, string table)
        {
            if (name == "")
            {
                throw new Exception ("Please enter customer name");
            }
            else if (number == "")
            {
                throw new Exception ("Please enter customer phone number");
            }
            else if (table == "")
            {
                throw new Exception ("Please enter table number");
            }
        }
        protected virtual void validateNum(string num)
        {
                if (!Regex.IsMatch(num, @"^\d+$"))
                {
                    throw new Exception("Incorrect input. Quantity or item price fields may not contain letters");
                } 
        }
        protected virtual void Calculation()
        {
            // Exception
            try
            {
                // Calculate total
                Double ItemQty;
                Double Price;
                Double SubTotal;
                Double Total;
                Double NetTax;
                Double BillTotal;

                ItemQty = Double.Parse(qtyTextBox.Text);
                Price = Double.Parse(item_PriceTextBox.Text);
                SubTotal = Double.Parse(sub_TotalTextBox.Text);
                Total = SubTotal;

                sub_TotalTextBox.Text = System.Convert.ToString(Total);
                basketTotal.Text = System.Convert.ToString(Total);

                NetTax = ((Total) * iTax) / 100;
                BillTotal = Total + NetTax;

                net_TotalTextBox.Text = System.Convert.ToString(BillTotal);

                item_PriceTextBox.Text = String.Format("{0:0.00}", Double.Parse(item_PriceTextBox.Text));
                sub_TotalTextBox.Text = String.Format("{0:0.00}", Double.Parse(sub_TotalTextBox.Text));
                net_TotalTextBox.Text = "R" + String.Format("{0:0.00}", Double.Parse(net_TotalTextBox.Text));
                basketTotal.Text = "R" + String.Format("{0:0.00}", Double.Parse(basketTotal.Text));

                taxTextBox.Text = "15%";
            }

            catch
            {
                MessageBox.Show("Calculation already proccessed. Please reset fields");
            }
        }
        protected virtual void AddData()
        {
            string selection;

            if (pizzaList.SelectedIndex != -1)
            {
                Double ItemPrice;
                ItemPrice = Double.Parse(item_PriceTextBox.Text);
                DataRowView row = pizzaList.SelectedItem as DataRowView;
                selection = row["Pizza"].ToString();
                basket_Summary.Text = basket_Summary.Text + "x" + qtyTextBox.Text + "\t" + selection + "\t" + "\t" + String.Format("{0:0.00}", ItemPrice) + Environment.NewLine;
            }

            Double ItemQty;
            Double Price;
            Double SubTotal;
            Double Total;
            Double TotalResult;

            ItemQty = Double.Parse(qtyTextBox.Text);
            Price = Double.Parse(item_PriceTextBox.Text);
            SubTotal = Double.Parse(sub_TotalTextBox.Text);
            TotalResult = ItemQty * Price;
            Total = TotalResult + SubTotal;

            sub_TotalTextBox.Text = System.Convert.ToString(Total);
            basketTotal.Text = System.Convert.ToString(Total);
        }
        protected virtual void SearchText()
        {
            // Filter data in listbox
            DataView dvPizzas = dtPizzas.DefaultView;
            dvPizzas.RowFilter = "Pizza LIKE '%" + lbl_Search.Text + "%'";
        }
        protected virtual void TotalPopup()
        {
            try
            {
                validateInputs(customer_NameTextBox.Text, phone_NumberTextBox.Text, table_NoTextBox.Text);
                SaveData();
                
                Calculation();
                MessageBox.Show("Your Total is: " + net_TotalTextBox.Text);

            }
            catch (Exception e)

            {
                MessageBox.Show(e.Message);
            }

        }
        protected virtual void ViewData()
        {
            con.Open();
            String query = "SELECT * FROM Order_Systems";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            order_SystemsDataGridView.DataSource = dt;
            con.Close();
        }
        protected virtual void CheckForUpdate()
        {
            WebRequest wr = WebRequest.Create(new Uri("https://www.dropbox.com/s/ij6ebeuru5eqt34/Version.txt?dl=0"));
            WebResponse ws = wr.GetResponse();
            StreamReader sr = new StreamReader(ws.GetResponseStream());
            string CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string NewVersion = sr.ReadToEnd();

            if (CurrentVersion.Contains(NewVersion))
            {
                MessageBox.Show("Already up to date");
            } else
            {
                MessageBox.Show(CurrentVersion);
                Updater objFrmUpdate = new Updater();
                objFrmUpdate.Show();
            }
        }

        //Events
        private void FrmMain_Load(object sender, EventArgs e)
        {
            OnLoad();
            Dtime();
        }
        private void Order_SystemsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            ValidateData();
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {

            Dtime();
            ResetFields();

        }
        private void BtnReceipt_Click(object sender, EventArgs e)
        {
            DisplaySlip();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                OnSave();

            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {

                validateNum(qtyTextBox.Text);
                validateNum(item_PriceTextBox.Text);
                Calculation();
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                validateNum(qtyTextBox.Text);
                validateNum(item_PriceTextBox.Text);
                AddData();
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Lbl_Search_TextChanged(object sender, EventArgs e)
        {
            SearchText();
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            try
            {

                validateNum(qtyTextBox.Text);
                validateNum(item_PriceTextBox.Text);
                TotalPopup();
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }
        private void BtnView_Click(object sender, EventArgs e)
        {
            ViewData();
        }

    }
}
