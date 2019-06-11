using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

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
            // TODO: This line of code loads data into the 'loginDBDataSet.Products' table. You can move, or remove it, as needed.
            this.productsTableAdapter.Fill(this.loginDBDataSet.Products);
            // TODO: This line of code loads data into the 'loginDBDataSet.Order_Items' table. You can move, or remove it, as needed.
            this.order_ItemsTableAdapter.Fill(this.loginDBDataSet.Order_Items);
            // TODO: This line of code loads data into the 'loginDBDataSet.Order_Systems' table. You can move, or remove it, as needed.
            this.order_SystemsTableAdapter.Fill(this.loginDBDataSet.Order_Systems);

            

            // Set default value
            qtyTextBox.Text = "0";
            customer_NameTextBox.Text = "";
            item_PriceTextBox.Text = "0";
            sub_TotalTextBox.Text = "0";
            basketTotal.Text = "0";
            taxTextBox.Text = "15%";
            phone_NumberTextBox.Text = "";

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

            // Display Receipt
            txtReceipt.AppendText("\t\t" + "       PIZZA HUB");
            txtReceipt.AppendText("\t\t\t" + "=================================================" + Environment.NewLine);
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

            txtReceipt.AppendText("\t\t\t" + "=================================================" + Environment.NewLine);
            txtReceipt.AppendText("\t\t" + "       RECEIPT" + Environment.NewLine);
        }
        protected virtual void OnSave()
        {
            //// Check if customer details are empty
            //if (customer_NameTextBox.Text == "")
            //{
            //    MessageBox.Show("Please enter customer name");
            //}
            //else if (phone_NumberTextBox.Text == "")
            //{
            //    MessageBox.Show("Please enter customer phone number");
            //}
            //else if (table_NoTextBox.Text == "")
            //{
            //    MessageBox.Show("Please enter table number");
            //}
            //else
            //{
            //    SaveData();
            //}

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
                net_TotalTextBox.Text = String.Format("{0:0.00}", Double.Parse(net_TotalTextBox.Text));
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

                //if (customer_NameTextBox.Text == "")
                //{
                //    throw new Exception("Please enter customer name");
                //}
                //else if (phone_NumberTextBox.Text == "")
                //{
                //    throw new Exception("Please enter customer phone number");
                //}
                //else if (table_NoTextBox.Text == "")
                //{
                //    throw new Exception("Please enter table number");
                //}
                //else
                //{
                //    SaveData();
                //}
                validateInputs(customer_NameTextBox.Text, phone_NumberTextBox.Text, table_NoTextBox.Text);
                SaveData();
                
                Calculation();
                MessageBox.Show("Your Total is: R" + net_TotalTextBox.Text);

            }
            catch (Exception e)

            {
                MessageBox.Show(e.Message);
            }

        }
        protected virtual void UpdateSystem()
        {
            MessageBox.Show("System already up to date");
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
        private void Button1_Click(object sender, EventArgs e)
        {

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
            Calculation();
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddData();
        }
        private void Lbl_Search_TextChanged(object sender, EventArgs e)
        {
            SearchText();
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            TotalPopup();
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            UpdateSystem();
        }
        private void BtnView_Click(object sender, EventArgs e)
        {
            ViewData();
        }

    }
}
