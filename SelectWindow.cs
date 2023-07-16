using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseProject
{
    public partial class SelectWindow : Form
    {
        private SqlConnection sqlConnection = null;
        public SelectWindow()
        {
            InitializeComponent();
        }
        internal async void CreateList(string[] _b)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ChurchCS"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();

            listView1.GridLines = true;

            listView1.FullRowSelect = true;

            listView1.View = View.Details;

            for (int i = 0; i < _b.Length; i++)
            {
                listView1.Columns.Add(_b[i]);
            }
        }

        internal void ChangeList(string[] _a)
        {
            ListViewItem lv = new ListViewItem(_a);
            listView1.Items.Add(lv);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
