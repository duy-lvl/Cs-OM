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
    public partial class OrderUpdateForm : Form
    {
        public OrderUpdateForm(Order order)
        {
            InitializeComponent();
            LoadData(order);
            this.order = order;
            this.order.OrderDetails = order.OrderDetails;
            this.order.Payments = order.Payments;
            this.DetailID.Visible = false;
            this.PaymentID.Visible = false;
        }
        private Order order;
        private void LoadData(Order order)
        {
            dgvPayment.AutoGenerateColumns = false;
            dgvOrderDetail.AutoGenerateColumns = false;
            using (var db = new PRN_ProductDBContext())
            {
                dgvPayment.DataSource = db.Payments.Where(p => p.OrderId == order.Id).ToList();
                
                this.orderDetails = db.OrderDetails.Where(od => od.OrderId == order.Id).ToList();
                dgvOrderDetail.DataSource = this.orderDetails;//db.OrderDetails.Where(od => od.OrderId == order.Id).ToList();
                numId.Value = order.Id;
                txtCusName.Text = order.CustomerName;
                txtAddress.Text = order.Address;
                numStatus.Value = (int)order.Status;
                dtpOrderDate.Value = (DateTime) order.OrderDate;
            }
        }

        private void btnUpdateOrderDetail_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetail.SelectedRows.Count > 0)
            {
                OrderDetail orderDetail1 = new()
                {
                    ProductId = (int)dgvOrderDetail.SelectedRows[0].Cells["ProductID"].Value,
                    Quantity = (int)dgvOrderDetail.SelectedRows[0].Cells["Quantity"].Value,
                    Id = (int)dgvOrderDetail.SelectedRows[0].Cells["DetailID"].Value,
                };

                OrderDetailUpdateForm orderDetailUpdateForm = new(orderDetail1);
                orderDetailUpdateForm.ShowDialog();
                var orderDetail = orderDetailUpdateForm.orderDetail;
                if (orderDetail != null)
                {
                    this.order.OrderDetails.Where(od => od.Id == orderDetail.Id).FirstOrDefault().ProductId = orderDetail.ProductId;
                    this.order.OrderDetails.Where(od => od.Id == orderDetail.Id).FirstOrDefault().Quantity = orderDetail.Quantity;
                    this.order.OrderDetails.Where(od => od.Id == orderDetail.Id).FirstOrDefault().Price = orderDetail.Price;

                    dgvOrderDetail.DataSource = order.OrderDetails.Where(od => od.Quantity > 0).ToList();


                }
            }
            
        }
        public List<OrderDetail> orderDetails { get; set; }
        private void btnDeleteOrderDetail_Click(object sender, EventArgs e)
        {
                /*int detailId = (int)dgvOrderDetail.SelectedRows[0].Cells[0].Value;
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this?", "Delete confirm", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    using (var db = new PRN_ProductDBContext())
                    {
                        var order = db.Orders.Find((int)numId.Value);
                        int orderId = order.Id;
                        List<OrderDetail> orderDetails = db.OrderDetails.Where(od => od.OrderId == orderId).ToList();
                        var result = orderDetails.Remove(db.OrderDetails.Find(detailId));
                        this.order.OrderDetails = orderDetails;
                        dgvOrderDetail.DataSource = this.order.OrderDetails.ToList();
                    }

                }*/
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

        private void btnAddOrderDetail_Click(object sender, EventArgs e)
        {
            OrderDetailAddForm orderDetailAddForm = new();
            orderDetailAddForm.ShowDialog();
            //dgvOrderDetail.ClearSelection();
            OrderDetail orderDetail = orderDetailAddForm.OrderDetail;
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
                    order.OrderDetails.Add(orderDetail);
                }
            }
            
            dgvOrderDetail.DataSource = order.OrderDetails.ToList();

        }

        private void btnDeletePayment_Click(object sender, EventArgs e)
        {
            /*if (dgvPayment.SelectedRows.Count > 0)
            {
                int paymentId = (int)dgvPayment.SelectedRows[0].Cells[0].Value;
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this?", "Delete confirm", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    using (var db = new PRN_ProductDBContext())
                    {
                        var order = db.Orders.Find((int)numId.Value);
                        int orderId = order.Id;
                        List<Payment> payments = db.Payments.Where(od => od.OrderId == orderId).ToList();
                        var result = payments.Remove(db.Payments.Find(paymentId));
                        this.order.Payments = payments;
                        dgvPayment.DataSource = this.order.Payments.ToList();
                    }
                    //
                    
                }
            }*/
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

        private void btnUpdateOrder_Click(object sender, EventArgs e)
        {
            if (checkValidate() != "")
            {
                MessageBox.Show(checkValidate(), "Warning");
            }
            else
            {
                Order order = new()
                {
                    Id = (int)numId.Value,
                    CustomerName = txtCusName.Text,
                    Address = txtAddress.Text,
                    OrderDate = dtpOrderDate.Value,
                    Status = (int)numStatus.Value,
                    TotalAmount = Convert.ToInt32(txtTotal.Text),
                };
                using (var db = new PRN_ProductDBContext())
                {
                    db.Orders.Update(order);
                    db.SaveChanges();

                    int orderID = order.Id;
                    List<Payment> paymentList = (List<Payment>)dgvPayment.DataSource;
                    foreach (var payment in db.Payments.Where(p => p.OrderId == orderID).ToList())
                    {
                        db.Payments.Remove(payment);
                        db.SaveChanges();
                    }

                    foreach (var payment in paymentList)//.Where(p => p.Id == 0).ToList())
                    {
                        
                        {
                            //payment.OrderId = orderID;
                            Payment pay = new()
                            {
                                PayTime = payment.PayTime,
                                PayType = payment.PayType,
                                Amount = payment.Amount,
                                OrderId = orderID,
                            };
                            db.Payments.Add(pay);
                            db.SaveChanges();
                        }
                        
                        
                        
                    }
                    List<OrderDetail> orderDetailList = (List<OrderDetail>)dgvOrderDetail.DataSource;
                    foreach (var detail in db.OrderDetails.Where(d => d.OrderId == orderID).ToList())
                    {
                        db.OrderDetails.Remove(detail);
                        db.SaveChanges();
                    }
                    foreach (var orderDetail in orderDetailList)
                    {
                        
                        {
                            //orderDetail.OrderId = orderID;
                            OrderDetail detail = new()
                            {
                                OrderId = orderID,
                                ProductId = orderDetail.ProductId,
                                Quantity = orderDetail.Quantity,
                                Price = orderDetail.Price,
                            };
                            db.OrderDetails.Add(detail);
                            db.SaveChanges();
                        }
                        
                        
                    }
                    this.Close();
                }
            }
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
            else if (((List<OrderDetail>)dgvOrderDetail.DataSource).Count == 0)
            {
                return "please input order detail!";
            }
            else if (((List<Payment>)dgvPayment.DataSource).Count == 0)
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
                    Id = (int)dgvPayment.SelectedRows[0].Cells["PaymentID"].Value,
                    PayTime = (DateTime)dgvPayment.SelectedRows[0].Cells["PayTime"].Value,
                    Amount = (double)dgvPayment.SelectedRows[0].Cells["Amount"].Value,
                    PayType = dgvPayment.SelectedRows[0].Cells["PayType"].Value.ToString(),
                };
                OrderPaymentUpdateForm orderPaymentUpdateForm = new OrderPaymentUpdateForm(payment);
                orderPaymentUpdateForm.ShowDialog();
                var pay = orderPaymentUpdateForm.payment;
                if (pay != null)
                {
                    this.order.Payments.Where(p => p.Id == pay.Id).FirstOrDefault().PayType = pay.PayType;
                    this.order.Payments.Where(p => p.Id == pay.Id).FirstOrDefault().Amount = pay.Amount;
                    this.order.Payments.Where(p => p.Id == pay.Id).FirstOrDefault().PayTime = pay.PayTime;
                    /*dgvPayment.SelectedRows[0].Cells["PayTime"].Value = pay.PayTime;
                    dgvPayment.SelectedRows[0].Cells["PayType"].Value = pay.PayType;
                    dgvPayment.SelectedRows[0].Cells["Amount"].Value = pay.Amount;*/

                }
                //dgvOrderDetail.ClearSelection();
                //dgvOrderDetail.DataSource = this.order.OrderDetails.Where(od => od.Quantity > 0).ToList();
                dgvPayment.DataSource = this.order.Payments.ToList();
            }
            //=============
            
        }
    }
}
