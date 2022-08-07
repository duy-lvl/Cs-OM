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
    public partial class CategoryForm : Form
    {
        public CategoryForm()
        {
            InitializeComponent();
            LoadCategoryData();
        }
        private void LoadCategoryData()
        {
            using (var db = new PRN_ProductDBContext())
            {
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = db.Categories.ToList();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this item?", "Delete confirm", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    var cid = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    using (var db = new PRN_ProductDBContext())
                    {
                        var deleteC = db.Products.Find(cid);
                        if (deleteC != null)
                        {
                            db.Products.Remove(deleteC);
                            db.SaveChanges();
                            LoadCategoryData();
                        }
                    }
                }
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var category = new Category()
                {
                    Id = (int)dataGridView1.SelectedRows[0].Cells[0].Value,
                    Name = (string)dataGridView1.SelectedRows[0].Cells[1].Value,
                    Status = (int)dataGridView1.SelectedRows[0].Cells[2].Value,
                };
                CategoryUpdateForm categoryUpdateForm = new CategoryUpdateForm(category);
                categoryUpdateForm.ShowDialog();
                LoadCategoryData();
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryAddForm categoryAddForm = new();
            categoryAddForm.ShowDialog();
            LoadCategoryData();
        }
    }
}
