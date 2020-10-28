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
    public partial class CustomMessageBox : Form
    {
        string message = "";
        public CustomMessageBox(string msg)
        {
            InitializeComponent();
            this.message =msg;
        }

        private void CustomMessageBox_Load(object sender, EventArgs e)
        {
            lblMessage.Text = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
