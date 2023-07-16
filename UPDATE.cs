using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace DataBaseProject
{
    public partial class UPDATE : Form
    {
        private SqlConnection sqlConnection = null;

        private int id;
        #region Проверка входных данных
        private object CheckInt(string _a, string _b)
        {
            int result;
            if (int.TryParse(_a, out result))
            {
                return Convert.ToInt32(_a);
            }
            else
            {
                MessageBox.Show($"Вы не ввели данные для поля \"{_b}\" или ввели неверные данные.\nБудет установлено значение \"1\"", "Attention!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 999999999;
            }
        }

        private object CheckStr(string _a, string _b)
        {
            if (!string.IsNullOrWhiteSpace(_a))
            {
                return _a;
            }
            else
            {
                MessageBox.Show($"Вы не ввели данные для поля \"{_b}\" или ввели неверные данные.\nБудет установлено значение \"None\"", "Attention!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "None";
            }
        }
        #endregion
        public UPDATE(SqlConnection connection, int id)
        {
            InitializeComponent();

            sqlConnection = connection;
            this.id = id;
        }

        private async void UPDATE_Load(object sender, EventArgs e)
        {
            SqlCommand getChurchDateCommand = new SqlCommand("SELECT id, ChurchName, ChurchAge, ChurchFoundingDate, ChurchCity, ChurchInfo FROM church WHERE id = @id", sqlConnection);

            getChurchDateCommand.Parameters.AddWithValue("id", id);

            SqlDataReader sqlReader = null;
            try
            {
                sqlReader = await getChurchDateCommand.ExecuteReaderAsync();
                
                while (await sqlReader.ReadAsync())
                {
                    textBox1.Text = Convert.ToString(sqlReader["ChurchName"]);

                    textBox2.Text = Convert.ToString(sqlReader["ChurchAge"]);

                    textBox3.Text = Convert.ToString(sqlReader["ChurchFoundingDate"]);

                    textBox4.Text = Convert.ToString(sqlReader["ChurchCity"]);

                    richTextBox1.Text = Convert.ToString(sqlReader["ChurchInfo"]);
                }
            }
            catch (Exception ex)
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

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand updateChurchCommand = new SqlCommand("UPDATE church SET ChurchName = @ChurchName, ChurchAge = @ChurchAge, ChurchFoundingDate = @ChurchFoundingDate, ChurchCity = @ChurchCity, ChurchInfo = @ChurchInfo WHERE id = @id", sqlConnection);
            object rChkA = CheckInt(textBox2.Text, label2.Text);
            object rChkFD = CheckInt(textBox3.Text, label3.Text);
            object rChkN = CheckStr(textBox1.Text, label1.Text);
            object rChkC = CheckStr(textBox4.Text, label4.Text);
            object rChkI = CheckStr(richTextBox1.Text, label5.Text);
            updateChurchCommand.Parameters.AddWithValue("id", id);
            updateChurchCommand.Parameters.AddWithValue("ChurchName", rChkN);
            updateChurchCommand.Parameters.AddWithValue("ChurchAge", rChkA);
            updateChurchCommand.Parameters.AddWithValue("ChurchFoundingDate", rChkFD);
            updateChurchCommand.Parameters.AddWithValue("ChurchCitY", rChkC);
            updateChurchCommand.Parameters.AddWithValue("ChurchInfo", rChkI);

            try
            {
                await updateChurchCommand.ExecuteNonQueryAsync();

                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
