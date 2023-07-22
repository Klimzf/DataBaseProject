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

namespace DataBaseProject
{
    public partial class SELECT : Form
    {
        private SqlConnection sqlConnection;
        public SELECT(SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.sqlConnection = sqlConnection;
        }

        SelectWindow SW = new SelectWindow();


        private async Task LoadSelAsync(string _a, string[] _b, SqlConnection _c)
        {

            SqlDataReader sqlreader = null;
            SqlCommand getSelCommand = new SqlCommand($"SELECT {_a} FROM church", _c);

            try
            {
                sqlreader = await getSelCommand.ExecuteReaderAsync();
                while (await sqlreader.ReadAsync()) 
                {
                    string[] cas = new string[_b.Length];
                    for (int j = 0; j < _b.Length; j++)
                    {            
                        cas[j] = Convert.ToString(sqlreader[_b[j]]);
                    }
                    SW.ChangeList(cas);
                }      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlreader != null && !sqlreader.IsClosed)
                {
                    sqlreader.Close();
                }
            }
        }
    

        private string[] GetPar()
        {
            string[] strings = checkedListBox1.CheckedItems.Cast<string>().ToArray();
            return strings;
        }
        private string GetSP(string[] a)
        {
            if (a.Length > 1)
            {
                string Par = a[0];
                for (int i = 1; i < a.Length; i++)
                {
                    Par += ",";
                    Par += a[i];
                }
                return Par;
            }
            else if (a.Length == 1) 
            {
                string Par = a[0];
                return Par;
            }
            else
            {
                MessageBox.Show("Вы не выбрали поля, по которым будет осуществлен поиск", "Be careful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "*";
            }
        }

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



        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SW.CreateList(GetPar());
            await LoadSelAsync(GetSP(GetPar()), GetPar(), sqlConnection);
            Close();           
            SW.Show();
        }

      
    }
}
