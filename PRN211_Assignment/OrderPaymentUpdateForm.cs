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
    public partial class OrderPaymentUpdateForm : Form
    {
        public OrderPaymentUpdateForm(Payment payment)
        {
            InitializeComponent();
            LoadData(payment);
        }
        private void LoadData(Payment payment)
        {
            numId.Value = payment.Id;
            txtPaytype.Text = payment.PayType;
            numAmount.Value = (decimal)payment.Amount;
            dtpPaytime.Value = (DateTime)payment.PayTime;
        }
        public Payment payment { get; set; }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkInput() != "")
            {
                MessageBox.Show(checkInput(), "Warning");
            }
            else
            {
                Payment payment = new()
                {
                    Id = (int)numId.Value,
                    PayTime = dtpPaytime.Value,
                    PayType = txtPaytype.Text,
                    Amount = (float)numAmount.Value,
                };
                this.payment = payment;
                this.Close();
            }
        }

        private string checkInput()
        {
            if (txtPaytype.Text == "")
            {
                return "please input paytype!";
            }
            return "";
        }
    }
}
