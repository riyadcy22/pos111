﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;

namespace OLWSHIFT
{
    public partial class frmChart : Form
    {
        SqlConnection cn;
        DBConnection db = new DBConnection();
        public frmChart()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = db.MyConnection();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadChartSold(string sql)
        {
            SqlDataAdapter da;
            cn.Open();
            da = new SqlDataAdapter(sql, cn);
            DataSet ds = new DataSet();
            da.Fill(ds,"SOLD");
            chart1.DataSource = ds.Tables["SOLD"];
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Doughnut;


            series.Name = "SOLD ITEMS";
            chart1.Series[0].XValueMember = "pdesc";
            //chart1.Series[0]["PieLabelStyle"] = "Outside";
            //chart1.Series[0].BorderColor = System.Drawing.Color.Gray;  
            chart1.Series[0].YValueMembers = "total";
            chart1.Series[0].LabelFormat = "{#,##0.00}";
            chart1.Series[0].IsValueShownAsLabel = true;
            cn.Close(); 

        }
    }
}
