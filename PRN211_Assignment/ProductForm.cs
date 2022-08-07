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
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
            LoadProductData();
        }
        private void LoadProductData()
        {
            using (var db = new PRN_ProductDBContext()){
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = db.Products.ToList();
            }
        }

        private string getCategory(int cateId)
        {
            string category = "";
            using (var db = new PRN_ProductDBContext())
            {
                category = db.Categories.Find(cateId).Name;
            }
            return category;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this item?", "Delete confirm", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    var pid = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    using (var db = new PRN_ProductDBContext())
                    {
                        var deleteP = db.Products.Find(pid);
                        if (deleteP != null)
                        {
                            db.Products.Remove(deleteP);
                            db.SaveChanges();
                            LoadProductData();
                        }
                    }
                }
            }
            
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductAddForm productAddForm = new();
            productAddForm.ShowDialog();
            LoadProductData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                using (var db = new PRN_ProductDBContext())
                {
                    var product = db.Products.Find(dataGridView1.SelectedRows[0].Cells[0].Value);
                    ProductUpdateForm productUpdateForm = new(product);
                    productUpdateForm.ShowDialog();
                    LoadProductData();
                }
            }
        }
    }
}
