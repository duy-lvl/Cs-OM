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
    public partial class ProductAddForm : Form
    {
        public ProductAddForm()
        {
            InitializeComponent();
            LoadCategoryCbo();
        }
        private void LoadCategoryCbo()
        {
            using (var db = new PRN_ProductDBContext())
            {
                cboCategory.DataSource = db.Categories.Where(c => c.Status > 0).ToList();
                cboCategory.DisplayMember = "Name";
                cboCategory.ValueMember = "ID";
            }
        }
        private string checkInput()
        {
            if (txtName.Text == "")
            {
                return "Please input product name!";
            }
            else
            {
                using (var db = new PRN_ProductDBContext())
                {
                    if (db.Products.Find((int)numProductId.Value) != null)
                    {
                        return "Duplicate ID";
                    }
                }
            }
            return "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
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
                    db.Products.Add(product);
                    db.SaveChanges();
                    this.Close();
                }
            }
        }
    }
}
