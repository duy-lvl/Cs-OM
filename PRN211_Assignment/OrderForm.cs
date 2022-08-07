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
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
            LoadOrderData();
        }
        private void LoadOrderData()
        {
            using (var db = new PRN_ProductDBContext())
            {
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = db.Orders.ToList();
            }
        }
        private void btnOrderDetail_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                OrderDetailForm odf = new OrderDetailForm(orderID);
                odf.ShowDialog();
                LoadOrderData();
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this order?", "Delete confirm", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    using (var db = new PRN_ProductDBContext())
                    {
                        var order = db.Orders.Find(dataGridView1.SelectedRows[0].Cells[0].Value);
                        if (order != null)
                        {
                            var orderDetails = db.OrderDetails.Where(od => od.OrderId == order.Id).ToList();
                            var payments = db.Payments.Where(p => p.OrderId == order.Id).ToList();
                            foreach (var pay in payments)
                            {
                                db.Payments.Remove(pay);
                                db.SaveChanges();
                            }
                            foreach (var detail in orderDetails)
                            {
                                db.OrderDetails.Remove(detail);
                                db.SaveChanges();
                            }
                            db.Orders.Remove(order);
                            db.SaveChanges();
                        }
                    }
                    LoadOrderData();
                }
            }
            
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OrderAddForm orderAddForm = new OrderAddForm();
            orderAddForm.ShowDialog();
            LoadOrderData();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                PaymentForm paymentForm = new PaymentForm(orderID);
                paymentForm.ShowDialog();
                LoadOrderData();
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                using (var db = new PRN_ProductDBContext())
                {
                    var order = db.Orders.Find((int)dataGridView1.SelectedRows[0].Cells[0].Value);
                    order.OrderDetails = db.OrderDetails.Where(od => od.OrderId == order.Id).ToList();
                    order.Payments = db.Payments.Where(pm => pm.OrderId == order.Id).ToList();
                    OrderUpdateForm orderUpdateForm = new OrderUpdateForm(order);
                    orderUpdateForm.ShowDialog();
                    LoadOrderData();
                }
            }
            
        }
    }
}
