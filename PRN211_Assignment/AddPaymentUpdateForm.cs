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
    public partial class AddPaymentUpdateForm : Form
    {
        public AddPaymentUpdateForm(Payment payment)
        {
            InitializeComponent();
            LoadData(payment);
        }
        public Payment payment { get; set; }
        private void LoadData(Payment payment)
        {
            txtPaytype.Text = payment.PayType.ToString();
            numAmount.Value = (decimal)payment.Amount;
            dtpPaytime.Value = (DateTime)payment.PayTime;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkValidate() != "")
            {
                MessageBox.Show(checkValidate(), "Warning");
            }
            else
            {
                Payment payment = new Payment()
                {
                    PayTime = dtpPaytime.Value,
                    PayType = txtPaytype.Text,
                    Amount = (float)numAmount.Value,
                };
                this.payment = payment;
                this.Close();
            }
        }
        public string checkValidate()
        {
            if (txtPaytype.Text == "")
            {
                return "please input pay type!";
            }

            return "";
        }
    }
}
