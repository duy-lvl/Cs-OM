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
    public partial class AddDetailUpdateForm : Form
    {
        public AddDetailUpdateForm(OrderDetail orderDetail)
        {
            InitializeComponent();
            LoadProduct((int)orderDetail.ProductId);
            LoadData(orderDetail);
        }
        public OrderDetail orderDetail { get; set; }
        private void LoadData(OrderDetail orderDetail)
        {
            cboProduct.SelectedValue = orderDetail.ProductId;
            numQuantity.Value = (int)orderDetail.Quantity;
        }
        private void LoadProduct(int productId)
        {
            using (var db = new PRN_ProductDBContext())
            {
                cboProduct.DataSource = db.Products.Where(p => p.Status > 0).ToList();
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "ID";

                cboProduct.SelectedValue = productId;
            }
        }
        private void UpdatePrice()
        {
            using (var db = new PRN_ProductDBContext())
            {
                int selected = cboProduct.SelectedIndex;
                var products = db.Products.Where(p => p.Status > 0).ToList();
                var product = products[selected];
                txtPrice.Text = ((int)numQuantity.Value * product.Price).ToString();
            }
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            orderDetail = new()
            {
                //Id = (int)numDetailID.Value,
                ProductId = (int)cboProduct.SelectedValue,
                Quantity = (int)numQuantity.Value,
                Price = Convert.ToInt32(txtPrice.Text),

            };
            this.orderDetail = orderDetail;
            this.Close();
        }
    }
}
