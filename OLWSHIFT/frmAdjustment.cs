﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace OLWSHIFT
{
    public partial class frmAdjustment : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        DBConnection db = new DBConnection();
        Form1 f;
        int _qty = 0;
        public frmAdjustment(Form1 f)
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = db.MyConnection();
            this.f = f; 
        }

        private void frmAdjustment_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        public void ReferenceNo()
        {
            Random rnd = new Random();
            txtRef.Text = rnd.Next().ToString();
        }

        public void LoadRecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("Select p.pcode, p.barcode, p.pdesc, b.brand, c.category, p.price, p.qty from tblProduct as p inner join tblBrand as b on b.id = p.bid inner join tblCategory as c on c.id = p.cid where p.pdesc like '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                LoadRecords();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                txtPcode.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtDesc.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() + " " + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() + " " + dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                _qty = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //validation for empty fields


                if(int.Parse(txtQty.Text) > _qty)
                {
                    MessageBox.Show("STOCK ON HAND QUANTITY SHOULD BE GREATER THEN FROM ADJSUTMENT QTY.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //update stock
                if(cboCommand.Text == "REMOVE FROM INVENTORY")
                {
                    SqlStatement ("update tblproduct set qty = (qty - " + int.Parse(txtQty.Text) + ") where pcode like '" + txtPcode.Text + "'");
                }else if(cboCommand.Text == "ADD TO INVENTORY")
                {
                    SqlStatement("update tblproduct set qty = (qty + " + int.Parse(txtQty.Text) + ") where pcode like '" + txtPcode.Text + "'");

                }

                SqlStatement("insert into tbladjustment(referenceno, pcode, qty, action, remarks, sdate, [user]) values('" + txtRef.Text + "','" + txtPcode.Text + "','" + int.Parse(txtQty.Text) + "','" + cboCommand.Text + "','" + txtRemarks.Text + "','" + DateTime.Now.ToShortDateString() + "','" + txtUser.Text + "')");
                MessageBox.Show("STOCK HAS BEEN SUCCESSFULLY ADJUSTED.", "PROCESS COMPLETED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRecords();
                Clear();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Clear()
        {
            txtDesc.Clear();
            txtPcode.Clear();
            txtQty.Clear();
            txtRef.Clear();
            txtRemarks.Clear();
            cboCommand.Text = "";
            ReferenceNo();
        }

        public void SqlStatement(string _sql)
        {
            cn.Open();
            cm = new SqlCommand(_sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }

    }
}
