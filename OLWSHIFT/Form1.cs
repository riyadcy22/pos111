using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;


namespace OLWSHIFT
{
    public partial class Form1 : Form
    { 
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public string _pass, _user;

        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            NotifyCriticalItems();
          //  MyDashBoard();


        }

        public void NotifyCriticalItems()
        {
            string critical = "";
            int i = 0;
            cn.Open();
            cm = new SqlCommand("select * from vwCriticalItems", cn);
            dr = cm.ExecuteReader();
            while (dr.Read()) 
            {
                i++;
                critical += i + dr["pdesc"].ToString() + Environment.NewLine;
            }
            dr.Close();
            cn.Close();

            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.error;
            popup.TitleText = "CRITICAL ITEM(S)";
            popup.ContentText = critical;
            popup.Popup();
        }



        private void btnBrand_Click(object sender, EventArgs e)
        {
            frmBrandList frm = new frmBrandList();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            frmCategoryList frm = new frmCategoryList();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.LoadCategory();
            frm.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmProductList frm = new frmProductList();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);   
            frm.BringToFront();
            frm.LoadRecord();
            frm.Show();

        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            frmStockIn frm = new frmStockIn();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);  
            frm.BringToFront();
            frm.Show();
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            //frmPOS frm = new frmPOS();
            //frm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmUserAccount frm = new frmUserAccount(this);
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.txtUser1.Text = _user;
            frm.BringToFront();
            frm.Show();
        }

        private void btnSalesHistory_Click(object sender, EventArgs e)
        {
            frmSoldItems frm = new frmSoldItems();  
            frm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmRecords frm = new frmRecords();
            frm.TopLevel = false;
            frm.LoadCriticalItems();
            frm.LoadInventory();
            frm.CancelledOrders();
            frm.LoadStockInHistory();
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("LOGOUT APPLICATION?","CONFIRM",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
            {
                this.Hide();
                frmSecurity f = new frmSecurity();
                f.ShowDialog(); 
            } 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            frmStore f = new frmStore();
            f.LoadRecords();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyDashBoard();
        }


        public void MyDashBoard()
        {
            frmDashboard f = new frmDashboard();
            f.TopLevel = false;
            panel4.Controls.Add(f);
            f.lblDailySales.Text = dbcon.DailySales().ToString("#,##0.00");
            f.lblProduct.Text = dbcon.Productline().ToString("#,##0");
            f.lblStockOnHand.Text = dbcon.StockOnHand().ToString("#,##0");
            f.lblCritical.Text = dbcon.CriticalItems().ToString("#,##0");
            f.BringToFront();
            f.Show();
        }

        private void btnVendor_Click(object sender, EventArgs e)
        {
            frmVendorList frm = new frmVendorList();
            frm.TopLevel = false;
            frm.LoadRecords();
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            frmModifierList frm = new frmModifierList();
            frm.TopLevel = false;
            panel4.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnAdjustment_Click(object sender, EventArgs e)
        {
            frmAdjustment f = new frmAdjustment(this);
            f.LoadRecords();
            f.txtUser.Text = lblUser.Text;
            f.ReferenceNo();
            f.ShowDialog();
        }
    }
}

