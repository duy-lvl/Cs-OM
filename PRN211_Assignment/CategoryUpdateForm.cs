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
    public partial class CategoryUpdateForm : Form
    {
        public CategoryUpdateForm(Category category)
        {
            InitializeComponent();
            LoadUpdateData(category);
        }

       public void LoadUpdateData(Category category)
        {
            numId.Value = category.Id;
            txtName.Text = category.Name.ToString();
            numStatus.Text = category.Status.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkInput() != "")
            {
                MessageBox.Show(checkInput(), "Invalid input");
            }
            else
            {
                using (var db = new PRN_ProductDBContext())
                {
                    var cate = new Category()
                    {
                        Id = (int)numId.Value,
                        Name = txtName.Text,
                        Status = (int)numStatus.Value,
                    };
                    db.Categories.Update(cate);
                    db.SaveChanges();
                    this.Close();
                }
            }
            
        }

        private string checkInput()
        {
            if (txtName.Text == "")
            {
                return "Please input name!";
            }
            

            return "";
        }
    }
}
