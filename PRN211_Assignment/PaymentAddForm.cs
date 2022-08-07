using PRN211_Assigment.Repo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRN211_Assignment
{
    public partial class PaymentAddForm : Form
    {
        public PaymentAddForm()
        {
            InitializeComponent();
        }
        public Payment payment { get; set; }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isValidate())
            {
                Payment payment = new()
                {
                    Id = 0,
                    PayTime = dtpPayTime.Value,
                    PayType = txtPayType.Text,
                    Amount = (float)numAmount.Value,
                };
                this.payment = payment;
                this.Close();
            } else
            {
                MessageBox.Show("Please input pay type!");
            }
        }

        private bool isValidate()
        {
            if (txtPayType.Text == "")
            {
                return false;
            }
            return true;
        }
    }
}
