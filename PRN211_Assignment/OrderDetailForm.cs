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
    public partial class OrderDetailForm : Form
    {
        public OrderDetailForm(int orderID)
        {
            InitializeComponent();
            LoadOrderDetail(orderID);
        }
        private void LoadOrderDetail(int orderID)
        {
            using (var db = new PRN_ProductDBContext())
            {
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = db.OrderDetails.Where(od => od.OrderId == orderID).ToList();
            }
        }
    }
}
