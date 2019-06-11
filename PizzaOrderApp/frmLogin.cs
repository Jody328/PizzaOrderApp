using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace PizzaOrderApp
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            DateTime iDate = DateTime.Now;
            string currentDate = iDate.ToString("dd/MM/yyyy");
            string currentTime = iDate.ToString("HH:mm");

            SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ASUS\Documents\PizzaOrderApp\PizzaOrderApp\Database\LoginDB.mdf;Integrated Security=True;Connect Timeout=30");
            string query = "Select * from tbl_Login Where Username = '" + txtUsername.Text.Trim() + "' and Password = '" + txtPassword.Text.Trim() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count == 1)
            {
                frmMain objFrmMain = new frmMain();
                this.Hide();
                objFrmMain.Show();
            }
            else
            {
                MessageBox.Show("Check your username and password");
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
