﻿using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLWSHIFT
{
    public partial class frmCancelDetails : Form
    {
        frmSoldItems f;
        public frmCancelDetails(frmSoldItems frm)
        {
            InitializeComponent();
            f = frm;
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cboAction_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try { 
          
                if ((cboAction.Text != String.Empty) && (txtQty.Text != String.Empty) && (txtReason.Text != String.Empty))
                {
             
                    if (int.Parse(txtQty.Text) >= int.Parse(txtCancelQty.Text))
                    {
                  
                        frmVoid f = new frmVoid(this);

                  
                        f.ShowDialog();
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void RefreshList()
        {
            f.LoadRecord();
        }


        private void frmCancelDetails_Load(object sender, EventArgs e)
        {

        }

        private void txtPCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
