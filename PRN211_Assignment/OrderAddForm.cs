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
    public partial class OrderAddForm : Form
    {
        public OrderAddForm()
        {
            InitializeComponent();
        }
        //private List<OrderDetail> orderDetails;
        private Order order;
        private void btnAddOrderDetail_Click(object sender, EventArgs e)
        {
            OrderDetailAddForm orderDetailAddForm = new();
            orderDetailAddForm.ShowDialog();
            dgvOrderDetail.AutoGenerateColumns = false;
            //dgvOrderDetail.DataSource = orderDetailAddForm.detailList;
            if (order == null)
            {
                order = new Order();
            }
            var orderDetail = orderDetailAddForm.OrderDetail;
            if (orderDetail != null)
            {
                OrderDetail detail = order.OrderDetails.Where(od => od.ProductId == orderDetail.ProductId).FirstOrDefault();
                if (detail != null)
                {
                    order.OrderDetails.Where(od => od.ProductId == orderDetail.ProductId).First().Quantity += orderDetail.Quantity;
                    order.OrderDetails.Where(od => od.ProductId == orderDetail.ProductId).First().Price += orderDetail.Price;
                }
                else
                {
                    order.OrderDetails.Add(orderDetailAddForm.OrderDetail);
                }
            }
            
            dgvOrderDetail.DataSource = order.OrderDetails.Where(od => od.Quantity > 0).ToList();
        }

        private void btnAddOrderPayment_Click(object sender, EventArgs e)
        {
            PaymentAddForm paymentAddForm = new();
            paymentAddForm.ShowDialog();
            dgvPayment.AutoGenerateColumns = false;
            if (order == null)
            {
                order = new Order();
            }
            var payment = paymentAddForm.payment;
            if (payment != null)
            {
                order.Payments.Add(paymentAddForm.payment);
                
            }
            
            dgvPayment.DataSource = order.Payments.ToList();
        }

        private void btnUpdateOrderDetail_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetail.SelectedRows.Count > 0)
            {
                OrderDetail orderDetail1 = new()
                {
                    ProductId = (int)dgvOrderDetail.SelectedRows[0].Cells["ProductID"].Value,
                    Quantity = (int)dgvOrderDetail.SelectedRows[0].Cells["Quantity"].Value,
                    //Id = (int)dgvOrderDetail.SelectedRows[0].Cells["DetailID"].Value,
                };
                AddDetailUpdateForm addDetailUpdateForm = new(orderDetail1);

                //OrderDetailUpdateForm orderDetailUpdateForm = new(orderDetail);

                addDetailUpdateForm.ShowDialog();
                var orderDetail = addDetailUpdateForm.orderDetail;
                if (orderDetail != null)
                {
                    dgvOrderDetail.SelectedRows[0].Cells[0].Value = orderDetail.ProductId;
                    dgvOrderDetail.SelectedRows[0].Cells[1].Value = orderDetail.Quantity;
                    dgvOrderDetail.SelectedRows[0].Cells[2].Value = orderDetail.Price;
                }
                //dgvOrderDetail.ClearSelection();
                dgvOrderDetail.DataSource = this.order.OrderDetails.Where(od => od.Quantity > 0).ToList();

            }
        }

        private void btnDeleteOrderDetail_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetail.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this?", "Delete confirm", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    int removeProductId = (int)dgvOrderDetail.SelectedRows[0].Cells[0].Value;
                    var orderDetails = this.order.OrderDetails.ToList();
                    orderDetails.RemoveAt(dgvOrderDetail.SelectedRows[0].Index);
                    
                    this.order.OrderDetails = orderDetails;
                    dgvOrderDetail.DataSource = orderDetails.Where(od => od.Quantity > 0).ToList();
                }
            }
        }

        private void btnDeletePayment_Click(object sender, EventArgs e)
        {
            if (dgvPayment.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this?", "Delete confirm", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    //int removePayment = (int)dgvPayment.SelectedRows[0].Cells[0].Value;
                    var payments = this.order.Payments.ToList();
                    payments.RemoveAt(dgvPayment.SelectedRows[0].Index);

                    this.order.Payments = payments;
                    dgvPayment.DataSource = payments.ToList();
                }
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            if (checkValidate() != "")
            {
                MessageBox.Show(checkValidate(), "Warning");
            }
            else
            {
                Order order = new()
                {
                    CustomerName = txtCusName.Text,
                    Address = txtAddress.Text,
                    OrderDate = dtpOrderDate.Value,
                    Status = (int)numStatus.Value,
                    TotalAmount = Convert.ToInt32(txtTotal.Text),
                };
                using (var db = new PRN_ProductDBContext())
                {
                    db.Orders.Add(order);
                    db.SaveChanges();

                    List<Order> orders = db.Orders.ToList();
                    int orderID = orders[orders.Count - 1].Id;
                    List<Payment> paymentList = (List<Payment>)dgvPayment.DataSource;
                    foreach (var payment in paymentList)
                    {
                        payment.OrderId = orderID;
                        db.Payments.Add(payment);
                        db.SaveChanges();
                    }
                    List<OrderDetail> orderDetailList = (List<OrderDetail>)dgvOrderDetail.DataSource;
                    foreach(var orderDetail in orderDetailList)
                    {
                        orderDetail.OrderId = orderID;
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                    }
                    this.Close();
                }
            }
        }

        private string checkValidate()
        {
            if (txtCusName.Text == "")
            {
                return "Please input customer name!";
            }
            else if (txtAddress.Text == "")
            {
                return "please input customer address!";
            } 
            else if (dgvOrderDetail.DataSource == null) {
                return "please input order detail!";
            }
            else if (dgvPayment.DataSource == null)
            {
                return "please input payment!";
            }
            
            return "";
        }

        private void dgvOrderDetail_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dgvOrderDetail.DataSource != null)
            {
                float totalAmount = 0;
                List<OrderDetail> detailList = (List<OrderDetail>)dgvOrderDetail.DataSource;
                foreach (var orderDetail in detailList)
                {
                    totalAmount += (float)orderDetail.Price;
                }
                txtTotal.Text = totalAmount.ToString();
            }
        }

        private void btnUpdatePayment_Click(object sender, EventArgs e)
        {
            if (dgvPayment.SelectedRows.Count > 0)
            {
                Payment payment = new()
                {
                    PayTime = (DateTime)dgvPayment.SelectedRows[0].Cells["PayTime"].Value,
                    Amount = (double)dgvPayment.SelectedRows[0].Cells["Amount"].Value,
                    PayType = dgvPayment.SelectedRows[0].Cells["PayType"].Value.ToString(),
                };
                AddPaymentUpdateForm addPaymentUpdateForm = new(payment);

                //OrderDetailUpdateForm orderDetailUpdateForm = new(orderDetail);

                addPaymentUpdateForm.ShowDialog();
                var pay = addPaymentUpdateForm.payment;
                if (pay != null)
                {
                    dgvPayment.SelectedRows[0].Cells[0].Value = pay.PayTime;
                    dgvPayment.SelectedRows[0].Cells[2].Value = pay.PayType;
                    dgvPayment.SelectedRows[0].Cells[1].Value = pay.Amount;
                }
                //dgvOrderDetail.ClearSelection();
                //dgvOrderDetail.DataSource = this.order.OrderDetails.Where(od => od.Quantity > 0).ToList();
                dgvPayment.DataSource = this.order.Payments.ToList();
            }
        }
    }
}
