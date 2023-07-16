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
using System.Configuration;
using System.Xml;

namespace DataBaseProject
{
    public partial class WorkingWithRemoteDB : Form
    {
        private SqlConnection sqlConnection = null;
        public WorkingWithRemoteDB()
        {
            InitializeComponent();
        }

        private async void WorkingWithRemoteDB_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ChurchCS"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();

            listView1.GridLines = true;

            listView1.FullRowSelect = true;

            listView1.View = View.Details;

            listView1.Columns.Add("id");
            listView1.Columns.Add("ChurchName");
            listView1.Columns.Add("ChurchAge");
            listView1.Columns.Add("ChurchFoundingDate");
            listView1.Columns.Add("ChurchCity");
            listView1.Columns.Add("ChurchInfo");

            await LoadChurchAsync();

        }

        private void WorkingWithRemoteDB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }


        internal void ChangeList(string[] _a)
        {
            ListViewItem lv = new ListViewItem(_a);
            listView1.Items.Add(lv);
        }

        private async Task LoadChurchAsync() // SELECT
        {
            SqlDataReader sqlReader = null;

            SqlCommand getChurchCommand = new SqlCommand("SELECT * FROM church", sqlConnection);      

            try
            {
                sqlReader = await getChurchCommand.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[]
                    {
                        Convert.ToString(sqlReader["id"]),
                        Convert.ToString(sqlReader["ChurchName"]),
                        Convert.ToString(sqlReader["ChurchAge"]),
                        Convert.ToString(sqlReader["ChurchFoundingDate"]),
                        Convert.ToString(sqlReader["ChurchCity"]),
                        Convert.ToString(sqlReader["ChurchInfo"])
                    });

                    listView1.Items.Add(item);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                {
                    sqlReader.Close();
                }
            }
        }

        private async void toolStripButton5_Click(object sender, EventArgs e) // Функционал кнопки "Обновить"
        {
            listView1.Items.Clear();

            await LoadChurchAsync();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) // Функционал кнопки INSERT
        {
            INSERT insert = new INSERT(sqlConnection);

            insert.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                UPDATE update = new UPDATE(sqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text));

                update.Show();
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

        }

        private async void toolStripButton3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку", "Удаление строки", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (listView1.SelectedItems.Count > 0)
            {
                switch (res)
                {
                    case DialogResult.OK:
                        SqlCommand deleteChurchCommand = new SqlCommand("DELETE FROM church WHERE id = @id", sqlConnection);

                        deleteChurchCommand.Parameters.AddWithValue("id", Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await deleteChurchCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        listView1.Items.Clear();

                        await LoadChurchAsync();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("WorkingWithDB-Klim, 2023", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SELECT select = new SELECT(sqlConnection);
            select.Show();
        }

        
    }
}
