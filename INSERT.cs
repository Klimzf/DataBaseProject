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

namespace DataBaseProject
{
    public partial class INSERT : Form
    {
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
                MessageBox.Show($"Вы ввели неверную данные для поля \"{_b}\".\nБудет установлено значение \"1\"", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show($"Вы ввели неверную данные для поля \"{_b}\".\nБудет установлено значение \"None\"", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "None";
            }
        }
        #endregion
        private SqlConnection sqlConnection;
        public INSERT(SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.sqlConnection = sqlConnection;
        }

       
        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand TableLong = new SqlCommand("SELECT COUNT(*) FROM church", sqlConnection); // Подсчет количества строк в нашей таблице 
            object rChkA = CheckInt(textBox2.Text, label2.Text);
            object rChkFD = CheckInt(textBox3.Text, label3.Text);
            object rChkN = CheckStr(textBox1.Text, label1.Text);
            object rChkC = CheckStr(textBox4.Text, label4.Text);
            object rChkI = CheckStr(richTextBox1.Text, label5.Text);
            object rChkId = (object)TableLong.ExecuteScalar();
            SqlCommand InsertChurchCommand = new SqlCommand("INSERT INTO church (id, ChurchName, ChurchAge, ChurchFoundingDate, ChurchCity, ChurchInfo) VALUES (@id, @ChurchName, @ChurchAge, @ChurchFoundingDate, @ChurchCity, @ChurchInfo)", sqlConnection);

            InsertChurchCommand.Parameters.AddWithValue("id", rChkId);
            InsertChurchCommand.Parameters.AddWithValue("ChurchName", rChkN);
            InsertChurchCommand.Parameters.AddWithValue("ChurchAge", rChkA);
            InsertChurchCommand.Parameters.AddWithValue("ChurchFoundingDate", rChkFD);
            InsertChurchCommand.Parameters.AddWithValue("ChurchCity", rChkC);
            InsertChurchCommand.Parameters.AddWithValue("ChurchInfo", rChkI);

            try
            {
                await InsertChurchCommand.ExecuteNonQueryAsync();

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
