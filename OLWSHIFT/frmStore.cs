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
    public partial class frmStore : Form
    {

        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        DBConnection db = new DBConnection();
        public frmStore()
        {
            InitializeComponent();
            cn = new SqlConnection(db.MyConnection());

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadRecords()
        {
            cn.Open();
            cm = new SqlCommand("select * from tblstore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                txtAddress.Text = dr["address"].ToString();
                txtStore.Text = dr["store"].ToString();

            }else
            {
                txtStore.Clear();
                txtAddress.Clear();

            }
            dr.Close();
            cn.Close();

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               if (MessageBox.Show("SAVE STORE DETAILS","CONFIRM",MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    int count;
                    cn.Open();
                    cm = new SqlCommand("select count(*) from tblstore", cn);
                    count = int.Parse(cm.ExecuteScalar().ToString());
                    cn.Close();
                    if (count > 0)
                    {
                        cn.Open();
                        cm = new SqlCommand("update tblstore set store=@store, address=@address",cn);
                        cm.Parameters.AddWithValue("@store", txtStore.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();

                    }
                    else
                    {
                        cn.Open();
                        cm = new SqlCommand("insert into tblstore (store, address) values (@store,@address)",cn);
                        cm.Parameters.AddWithValue("@store", txtStore.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();

                    }

                    MessageBox.Show("SHOW DETAILS HAS BEEN SUCCESSFULLY SAVED!", "SAVE RECORD",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }


            }catch(Exception ex) 
            {
            cn.Close();
            MessageBox.Show(ex.Message,"WARNING",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        


    }
}
