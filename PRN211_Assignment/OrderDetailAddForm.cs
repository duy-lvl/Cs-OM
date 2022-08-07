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
    public partial class OrderDetailAddForm : Form
    {
        public OrderDetailAddForm()
        {
            InitializeComponent();
            
            LoadProduct();
            UpdatePrice();
            
        }
        public OrderDetail OrderDetail { get; set; }
        private void LoadProduct()
        {
            using (var db = new PRN_ProductDBContext())
            {
                cboProduct.DataSource = db.Products.ToList();
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "ID";
                //cboProduct.SelectedItem = db.Products.FirstOrDefault();
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var orderDetail = new OrderDetail()
            {
                //OrderId = Convert.ToInt32(txtOrderID.Text),
                ProductId = (int)cboProduct.SelectedValue,
                Quantity = (int)numQuantity.Value,
                Price = Convert.ToInt32(txtPrice.Text),
            };
            //db.OrderDetails.Add(orderDetail);
            //db.SaveChanges();
            this.OrderDetail = orderDetail;
            this.Close();
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            using (var db = new PRN_ProductDBContext())
            {
                var selected = cboProduct.SelectedIndex;
                var products = db.Products.ToList();
                var product = products[selected];
                txtPrice.Text = ((int)numQuantity.Value * product.Price).ToString();
            }
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        
    }
}
