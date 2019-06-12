using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzaOrderApp
{
    public partial class Updater : Form
    {
        public Updater()
        {
            InitializeComponent();
        }
        protected virtual void Update()
        {
            MessageBox.Show("Updated successfully!");
        }
        protected virtual void Later()
        {
            this.Hide();
        }
        private void BtnYes_Click(object sender, EventArgs e)
        {
            Update();
            Later();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            Later();
        }
    }
}
