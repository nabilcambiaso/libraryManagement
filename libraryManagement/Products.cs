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
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms.Design;

namespace libraryManagement
{
    public partial class Products : Form
    {

        /*variables and declarations*/
        private OpenFileDialog file = new OpenFileDialog();
        int IDAdmin;
        private string imgName = "";
        private string imageLocation = "";
        private int parsed;
        private float floatParsed;
        private int selectedCell;
        private int selectedRow;
        CustomMessageBox msgs;
        /*cnx*/
        cnxDataContext cnx = new cnxDataContext();
        /*store ancien price and quantity value in case of entering a non numeric value
         * i use it globally so i can reinsert it in case of error in the datagridviewErrorHandler
         */
        private float ancienPrice;
        private int ancienQuantity;

        /** custome functions*/

        //ClearFields
        private void clearFields()
        {
            txtLibelle.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtRef.Clear();
            txtfLibelle.Clear();
            txtfPrice.Clear();
            txtfQuantity.Clear();
            txtfRef.Clear();
            pictureBox1.Image = Image.FromFile("./images/product.png");
            imgName = "";
            imageLocation = "";
            lblPrice.Visible = false;
            lblQuantity.Visible = false;
        }

        //reload Employee DataGridView
        private void loadData()
        {
            var q = from x in cnx.PRODUCTs
                    select new
                    {
                        Reference = x.REFERENCE,
                        Name=x.NAME,
                        Price=x.PRICE,
                        Quantity=x.QUANTITY
                    };
            dataGridView1.DataSource = q.ToList();
            countProducts(q.Count());
        }
        
        //filter
        private void filter()
        {
            
            try
            {
                
                if (!int.TryParse(txtfQuantity.Text, out parsed) && txtfQuantity.Text!="")
                {
                    msgs = new CustomMessageBox("Enter a numeric Value !");
                    msgs.ShowDialog();
                    return;
                }
                if (!float.TryParse(txtfPrice.Text, out floatParsed) && txtfPrice.Text!="")
                {
                    msgs = new CustomMessageBox("Enter a numeric Value !");
                    msgs.ShowDialog();
                    return;
                }

                if (txtfQuantity.Text == "" && txtfPrice.Text != "")
                {
                   var q = from x in cnx.PRODUCTs
                            where x.REFERENCE.StartsWith(txtfRef.Text.ToString())
                                && x.NAME.StartsWith(txtfLibelle.Text.ToString())
                                && x.PRICE<=float.Parse(txtfPrice.Text)
                            select new {
                                Reference = x.REFERENCE,
                                Name = x.NAME,
                                Price = x.PRICE,
                                Quantity = x.QUANTITY
                            };
                    dataGridView1.DataSource = q.ToList();
                    countProducts(q.Count());
                }
                else if(txtfQuantity.Text != "" && txtfPrice.Text == "")
                {
                    var q = from x in cnx.PRODUCTs
                            where x.REFERENCE.StartsWith(txtfRef.Text.ToString())
                                && x.NAME.StartsWith(txtfLibelle.Text.ToString())
                                && x.QUANTITY<= float.Parse(txtfQuantity.Text)
                            select new
                            {
                                Reference = x.REFERENCE,
                                Name = x.NAME,
                                Price = x.PRICE,
                                Quantity = x.QUANTITY
                            };
                    dataGridView1.DataSource = q.ToList();
                    countProducts(q.Count());
                }
                else if (txtfQuantity.Text == "" && txtfPrice.Text == "")
                {
                    var q = from x in cnx.PRODUCTs
                            where x.REFERENCE.StartsWith(txtfRef.Text.ToString())
                                && x.NAME.StartsWith(txtfLibelle.Text.ToString())
                            select new
                            {
                                Reference = x.REFERENCE,
                                Name = x.NAME,
                                Price = x.PRICE,
                                Quantity = x.QUANTITY
                            };
                    dataGridView1.DataSource = q.ToList();
                    countProducts(q.Count());
                }
                else
                {
                    var q = from x in cnx.PRODUCTs
                            where x.REFERENCE.StartsWith(txtfRef.Text.ToString()) && x.QUANTITY<= int.Parse(txtfQuantity.Text)
                                && x.PRICE<= float.Parse(txtfPrice.Text) && x.NAME.StartsWith(txtfLibelle.Text.ToString())
                            select new
                            {
                                Reference = x.REFERENCE,
                                Name = x.NAME,
                                Price = x.PRICE,
                                Quantity = x.QUANTITY
                            };
                    dataGridView1.DataSource = q.ToList();
                    countProducts(q.Count());
                }


            }
            catch { }
        }

        //count
        private void countProducts(int count)
        {
            lblCountProducts.Text=count.ToString();
        }
        //--------------------------------------
        public Products(int idAdmin)
        {
            InitializeComponent();
            this.IDAdmin = idAdmin;
        }

        private void Products_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void txtfLibelle_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void txtfPrice_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void txtfQuantity_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Size = new System.Drawing.Size(20,15);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            clearFields();
            loadData();
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Size = new System.Drawing.Size(20, 15);
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Size = new System.Drawing.Size(28, 20);         
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //the reference
                string code = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();

                //make it writable
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;


                //show the image of the product
                var prod = (from x in cnx.PRODUCTs where x.REFERENCE == code select x).Single();
                if (prod.images != "" && prod.images != "null")
                    pictureBox2.Image = Image.FromFile(Application.StartupPath + @"\images\" + prod.images);
                else
                {
                    pictureBox2.Image = Image.FromFile(Application.StartupPath + @"\images\product.png");
                }

                //validate delete action
                if (e.ColumnIndex == 0)
                {
                    if (MessageBox.Show("Are You sure you want to remove this product !", "Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        var q = (from x in cnx.PRODUCTs where x.REFERENCE == code select x).Single();
                        cnx.PRODUCTs.DeleteOnSubmit(q);
                        cnx.SubmitChanges();
                        msgs = new CustomMessageBox("Product Removed Successfully !");
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

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedCell = e.ColumnIndex;
                selectedRow = e.RowIndex;
                string code = dataGridView1.Rows[selectedRow].Cells[1].Value.ToString();
                string newValue = dataGridView1.Rows[selectedRow].Cells[selectedCell].EditedFormattedValue.ToString();
                var q = (from x in cnx.PRODUCTs where x.REFERENCE == code select x).Single();

                switch (selectedCell)
                {
                    case 2:
                        q.NAME = newValue;
                        cnx.SubmitChanges();
                        loadData();
                        msgs = new CustomMessageBox("Updated Successfully!");
                        msgs.ShowDialog();
                        ; break;
                    case 3:
                        ancienPrice = float.Parse(dataGridView1.Rows[selectedRow].Cells[selectedCell].Value.ToString());
                        float isFloat=-1;
                        if (float.TryParse(newValue, out isFloat))
                        {
                            q.PRICE = float.Parse(newValue);
                            cnx.SubmitChanges();
                            loadData();
                            msgs = new CustomMessageBox("Updated Successfully!");
                            msgs.ShowDialog();
                        }
                        else
                        {
                            msgs = new CustomMessageBox("Enter A numeric Value !");
                            msgs.ShowDialog();
                        }
                        ;break;
                    case 4:
                        ancienQuantity = int.Parse(dataGridView1.Rows[selectedRow].Cells[selectedCell].Value.ToString());
                         int isInt=-1;
                        if (!int.TryParse(newValue,out isInt))
                        {
                            msgs = new CustomMessageBox("Enter A numeric Value !");
                            msgs.ShowDialog();
                        }
                        else
                        {
                            q.QUANTITY = int.Parse(newValue);
                            cnx.SubmitChanges();
                            loadData();
                            msgs = new CustomMessageBox("Updated Successfully!");
                            msgs.ShowDialog();
                        }
                        ;break;
                    default: ; break;
                }
            }
            catch{}
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
             if(e.ColumnIndex==0)
           {
               dataGridView1.Cursor = Cursors.Hand;
           }
           else
           {
               dataGridView1.Cursor = Cursors.Default;
           }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (selectedCell == 3)
                dataGridView1.Rows[selectedRow].Cells[selectedCell].Value = ancienPrice;
            if (selectedCell == 4)
                dataGridView1.Rows[selectedRow].Cells[selectedCell].Value = ancienQuantity;
            msgs = new CustomMessageBox("Enter A numeric Value !");
            msgs.ShowDialog();
        }

        private void buttonAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLibelle.Text == "" || txtPrice.Text == "" || txtQuantity.Text == "" || txtRef.Text == "")
                {
                    msgs = new CustomMessageBox("All fields Must be Filled");
                    lblPrice.Visible = true;

                    msgs.ShowDialog();
                    return;
                }
                if (!int.TryParse(txtQuantity.Text, out parsed))
                {
                    msgs = new CustomMessageBox("Enter a numeric Value !");
                    msgs.ShowDialog();
                    lblQuantity.Visible = true;
                    return;
                }

                if (!float.TryParse(txtPrice.Text, out floatParsed))
                {
                    msgs = new CustomMessageBox("Enter a numeric Value !");
                    msgs.ShowDialog();
                    return;
                }
                var q = (from x in cnx.PRODUCTs where x.REFERENCE == txtRef.Text select x).Count();
                if (q > 0)
                {
                    msgs = new CustomMessageBox("Reference Already Exists !");
                    msgs.ShowDialog();
                    return;
                }
                else
                {
                    PRODUCT pr = new PRODUCT();
                    pr.REFERENCE = txtRef.Text;
                    pr.NAME = txtLibelle.Text;
                    pr.PRICE = float.Parse(txtPrice.Text);
                    pr.QUANTITY = int.Parse(txtQuantity.Text);
                    pr.IDADMINS = IDAdmin;
                    pr.images = imgName;
                    pr.ADDDATE = DateTime.Now;
                    cnx.PRODUCTs.InsertOnSubmit(pr);
                    if (imgName != "")
                        File.Copy(imageLocation, Application.StartupPath + @"\images\" + pr.REFERENCE + imgName);
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

        private void buttonAddImage_Click(object sender, EventArgs e)
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

    }
}
