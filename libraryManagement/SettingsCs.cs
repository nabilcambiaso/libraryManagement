using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace libraryManagement
{
    public partial class SettingsCs : Form
    {
        /*variables and declarations*/
        private  OpenFileDialog file = new OpenFileDialog();
        private string imgName = "";
        private string imageLocation = "";
        CustomMessageBox msgs;
        /*cnx*/
        cnxDataContext cnx = new cnxDataContext();


        /** custome functions*/

        //ClearFields
        private void clearFields()
        {
            txtUserName.Clear();
            txtPassword.Clear();
            imgName = "";
            imageLocation = "";
        }

        //reload Employee DataGridView
        private void loadData()
        {
            var q = from x in cnx.LOGINs where x.ADMINS==false
                    select new
                    {
                        Id = x.ID,
                        User_Name = x.USERNAME
                    };
            dataGridView1.DataSource = q.ToList();
        }







        //-----------------
        public SettingsCs()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtUserName.Text.Equals("")||txtUserName.Text==null)
                {
                    msgs = new CustomMessageBox("UserName can't be Empty !");
                    msgs.ShowDialog();
                    return;
                }
                if (txtPassword.Text=="")
                {
                    msgs = new CustomMessageBox("password can't be Empty !");
                    msgs.ShowDialog();
                    return;
                }
                var q = (from x in cnx.LOGINs where x.USERNAME == txtUserName.Text select x).Count();
                if(q>0)
                {
                    msgs = new CustomMessageBox("User Name Already Exists !");
                    msgs.ShowDialog();
                    return;
                }
                else
                {
                    LOGIN lg = new LOGIN();
                    lg.USERNAME = txtUserName.Text;
                    lg.PASSWORDS = txtPassword.Text;
                    lg.ADMINS = false;
                    lg.images = imgName;
                    cnx.LOGINs.InsertOnSubmit(lg);
                    File.Copy(imageLocation, Application.StartupPath + @"\images\"+ imgName);
                    cnx.SubmitChanges();
                    msgs = new CustomMessageBox("Added Successfully !");
                    msgs.ShowDialog();
                    clearFields();
                    loadData();                    
                }

            }
            catch 
            {
                msgs = new CustomMessageBox("Something Went Wrong !");
                msgs.ShowDialog();
              
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                file.Filter = ("jpg files|*.jpg | png files| *.png");
                file.Title = "Choose Image";
                file.FileName = "";
                file.ShowDialog();
                this.pictureBox1.Image = Image.FromFile(file.FileName);
                imageLocation = file.FileName;
                imgName = file.SafeFileName;
            }
            catch 
            {
                CustomMessageBox msgs = new CustomMessageBox("Error Loading Image!");
                msgs.ShowDialog();
            }
        }

        private void SettingsCs_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int code = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                if (e.ColumnIndex == 0)
                {
                    if(MessageBox.Show("Are You sure you want to remove this employee !","Remove",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning)==DialogResult.OK)
                    {
                        var q = (from x in cnx.LOGINs where x.ID == code select x).Single();
                        cnx.LOGINs.DeleteOnSubmit(q);
                        cnx.SubmitChanges();
                        msgs = new CustomMessageBox("Employee Removed Successfully !");
                        msgs.ShowDialog();
                        loadData();
                    }
                }
            }
            catch
            {
                msgs = new CustomMessageBox("Something Went Wrong !");
                msgs.ShowDialog();
            }
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
