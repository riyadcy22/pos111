using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace OLWSHIFT
{

    public partial class frmProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmProductList flist;

        public frmProduct(frmProductList frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            flist = frm;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select category from tblcategory", cn);
            dr = cm.ExecuteReader();
            while(dr.Read()) 
            {
                cboCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void LoadBrand()
        {
            cboBrand.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select brand from tblBrand", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboBrand.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }


        private void frmProduct_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this product?", "Save Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string bid = "", cid = "";

                    // Open the connection once
                    cn.Open();

                    // Retrieve brand ID (bid) from tblBrand
                    cm = new SqlCommand("Select id from tblBrand where brand = @brand", cn);
                    cm.Parameters.AddWithValue("@brand", cboBrand.Text);
                    dr = cm.ExecuteReader();
                    if (dr.Read()) { bid = dr["id"].ToString(); }
                    dr.Close();

                    // Retrieve category ID (cid) from tblCategory
                    cm = new SqlCommand("Select id from tblCategory where category = @category", cn);
                    cm.Parameters.AddWithValue("@category", cboCategory.Text);
                    dr = cm.ExecuteReader();
                    if (dr.Read())
                    { 
                        cid = dr["id"].ToString();
                    }
                    dr.Close();

                    // Insert new product into tblProduct
                    cm = new SqlCommand("INSERT INTO tblProduct (pcode, barcode, pdesc, bid, cid, price, reorder) VALUES (@pcode, @barcode, @pdesc, @bid, @cid, @price, @reorder)", cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", bid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReorder.Text));

                    cm.ExecuteNonQuery();

                    // Show success message
                    MessageBox.Show("Product has been successfully saved.");

                    // Clear the form and reload product records
                    Clear();
                    flist.LoadRecord();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Ensure the connection is closed, even if an exception occurs
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }


        public void Clear()
        {
            txtPrice.Clear();
            txtPdesc.Clear();  
            txtBarcode.Clear();
            txtPcode.Clear();
            cboBrand.Text = "";
            cboCategory.Text = "";
            txtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;  


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you wan to update this product?", "Save Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string bid = "";
                    string cid = "";




                    cn.Open();
                    cm = new SqlCommand("SELECT id FROM tblbrand WHERE brand = @brand", cn);
                    cm.Parameters.AddWithValue("@brand", cboBrand.Text);
                    dr = cm.ExecuteReader();
                    if (dr.Read())
                    {
                        bid = dr["id"].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("SELECT id FROM tblCategory WHERE category = @category", cn);
                    cm.Parameters.AddWithValue("@category", cboCategory.Text);
                    dr = cm.ExecuteReader();
                    if (dr.Read())
                    {
                        cid = dr["id"].ToString();
                    }
                    dr.Close();
                    cn.Close();


                    cn.Open();
                    cm = new SqlCommand("UPDATE tblProduct set barcode = @barcode, pdesc = @pdesc, bid = @bid, cid = @cid, price = @price, reorder = @reorder WHERE pcode LIKE @pcode", cn); 
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", bid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReorder.Text));





                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product has been successfully updated.");
                    Clear();
                    flist.LoadRecord();
                    this.Dispose();

                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {
                //accept . character
            } else if (e.KeyChar ==8 ) {
                //accept backspace
            } else if ((e.KeyChar < 48) || (e.KeyChar > 57)) //ASCII CODE 48-57 between 0 - 9 
            {
                e.Handled = true;
            }
        }
    }
}

