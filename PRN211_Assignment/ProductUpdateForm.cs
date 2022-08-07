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
    public partial class ProductUpdateForm : Form
    {
        public ProductUpdateForm(Product product)
        {
            InitializeComponent();
            LoadUpdateData(product);
        }
        private void LoadUpdateData(Product product)
        {
            numProductId.Value = product.Id;
            txtName.Text = product.Name;
            numPrice.Value = (decimal)product.Price;
            dtpCreatedDate.Value = (DateTime)product.CreatedDate;
            numStatus.Value = (int)product.Status;
            using (var db = new PRN_ProductDBContext())
            {
                cboCategory.DataSource = db.Categories.Where(c => c.Status > 0).ToList();
                cboCategory.DisplayMember = "Name";
                cboCategory.ValueMember = "ID";
                cboCategory.SelectedValue = product.CategoryId;
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkInput() != "")
            {
                MessageBox.Show(checkInput(), "Invalid input");
            }
            else
            {
                var product = new Product()
                {
                    Id = (int)numProductId.Value,
                    Name = txtName.Text,
                    Price = (double)numPrice.Value,
                    CreatedDate = (DateTime)dtpCreatedDate.Value,
                    CategoryId = (int)cboCategory.SelectedValue,
                    Status = (int)numStatus.Value,
                };
                using (var db = new PRN_ProductDBContext())
                {
                    db.Products.Update(product);
                    db.SaveChanges();
                    this.Close();
                }
            }
            
        }

        private string checkInput()
        {
            if (txtName.Text == "")
            {
                return "Please input product name!";
            }
            return "";
        }
    }
}
