using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libraryManagement
{
    public partial class LoginCs : Form
    {
        /*
         * connexion to database
         */
        private cnxDataContext cnx = new cnxDataContext();
          
        /*
         * customize design
         */
        // show hide password
        private void showHidePassword()
        {
            if (txtPassword.UseSystemPasswordChar)
                txtPassword.UseSystemPasswordChar = false;
            else
                txtPassword.UseSystemPasswordChar = true;
        }

        //change eye of hide show
        private void showHideEye()
        {
            if (txtPassword.UseSystemPasswordChar)
                pictureBox2.Image = Image.FromFile("./images/show.png");
            else
                pictureBox2.Image = Image.FromFile("./images/hide.png");

        }

        //customized messageBox
        CustomMessageBox msgs;



        public LoginCs()
        {
            InitializeComponent();
        }

        private void LoginCs_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            showHidePassword();
            showHideEye();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtUserName.Text=="")
            {
                msgs = new CustomMessageBox("User Name Invalid !");
                msgs.ShowDialog();
            }
            else if(txtPassword.Text=="")
            {
                msgs = new CustomMessageBox("Password Can't be Empty !");
                msgs.ShowDialog();
            }
            else
            {
                try{
                     var q = (from x in cnx.LOGINs where x.USERNAME == txtUserName.Text && x.PASSWORDS == txtPassword.Text select x).Single();
                if(q!=null)
                {
                    HomePage home = new HomePage(q.ADMINS.Value,q.USERNAME,q.images,q.ID);
                    home.BringToFront();
                    home.Show();
                    this.Hide();
                    
                }
                }
                catch
                {
                    msgs=new CustomMessageBox("User Name Or Password Incorrect !");
                    msgs.ShowDialog();
                }
               
            }
             

            
        }
    }
}
