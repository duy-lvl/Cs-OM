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
    public partial class PaymentForm : Form
    {
        public PaymentForm(int orderID)
        {
            InitializeComponent();
            LoadPayment(orderID);
        }

        private void LoadPayment(int orderID)
        {
            using (var db = new PRN_ProductDBContext())
            {
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = db.Payments.Where(p => p.OrderId == orderID).ToList();
            }
        }
    }
}
