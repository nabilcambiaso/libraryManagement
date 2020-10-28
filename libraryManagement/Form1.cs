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
    public partial class HomePage : Form
    {
        /*const and variables*/
        //check if the user is an admin or a regular employee
        private bool isAdmin = false;
        private string isUser = "";
        private string isImage = "";
        private int isId = -1;
        private Form activeForm = null;


        /**************
         * 
         * custom functions , variables and more
         * 
         **************/

        //customize design
        private void customizeDesign()
        {
            mediaSubMenuPanel.Visible = false;
            equalizerSubMenuPanel.Visible = false;
            toolsSubMenuPanel.Visible = false;
            playlistMangementSubMenuPanel.Visible = false;
           
        }

        //hide submenus
        private void hideSubMenu()
        {
            if (mediaSubMenuPanel.Visible)
                mediaSubMenuPanel.Visible = false;
            if (equalizerSubMenuPanel.Visible)
                equalizerSubMenuPanel.Visible = false;
            if (toolsSubMenuPanel.Visible)
                toolsSubMenuPanel.Visible = false;
            if (playlistMangementSubMenuPanel.Visible)
                playlistMangementSubMenuPanel.Visible = false;
        }

        //show submenu
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                //call hide all submenus
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        //forms in panel as childforms
        private void openChildForm(Form childFormName)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childFormName;
            childFormName.TopLevel = false;
            childFormName.FormBorderStyle = FormBorderStyle.None;
            childFormName.Dock = DockStyle.Fill;
            panelChildContainer.Controls.Add(childFormName);
            panelChildContainer.Tag = childFormName;
            childFormName.BringToFront();
            childFormName.Show();
        }

        



        public HomePage(bool adminState,string userName,string img,int ID)
        {
            InitializeComponent();
            this.isAdmin = adminState;
            this.isUser = userName;
            this.isImage = img;
            this.isId = ID;
            lblUserName.Text = userName;
            if(img!=null)
                pictureBoxUserImage.Image = Image.FromFile(Application.StartupPath + @"\images\"+img);
        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //call custom functions
            //invisible submenu panels
            customizeDesign();
            if (isAdmin == false)
                btnSettings.Visible = false;
            

        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            showSubMenu(mediaSubMenuPanel);
        }

        private void btnPlaylistMangement_Click(object sender, EventArgs e)
        {
            showSubMenu(playlistMangementSubMenuPanel);
        }

        private void btnEqualizer_Click(object sender, EventArgs e)
        {
            showSubMenu(equalizerSubMenuPanel);
        }

        private void btnTools_Click(object sender, EventArgs e)
        {
            showSubMenu(toolsSubMenuPanel);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //--
            openChildForm(new Dashboard());
            //--
            hideSubMenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //------
            openChildForm(new Products(isId));
            //------
            hideSubMenu();
        }

        private void panelLogo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            //------
            openChildForm(new SettingsCs());
            //------
            hideSubMenu();
        }

        private void HomePage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard ds = new Dashboard(txtSearch.Text);
            openChildForm(ds);
        }
    }
}
