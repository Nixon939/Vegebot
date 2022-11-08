using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace EXDconverter
{
    public partial class Form1 : Form
    {
        private string filename = string.Empty;
        private DataTableCollection tableCollection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = openFileDialog1.ShowDialog();

                if(res==DialogResult.OK)
                {
                    filename = openFileDialog1.FileName;

                    Text = filename;
                    OpenExcelFile(filename);
                }
                else
                {
                    throw new Exception("Файл не выбран или поврежден!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OpenExcelFile(string path)
        {
            FileStream stream = File.Open(path,FileMode.Open,FileAccess.Read);

            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            //создание базы данных
            DataSet db = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable=(x)=> new ExcelDataTableConfiguration()
                {
                    UseHeaderRow =true //считываем ввернхюю колонку таблицы excel и отображать в datagrid view

                }
            });
            tableCollection= db.Tables; //присваиваем все листы 

            toolStripComboBox1.Items.Clear();//отчистка комбобокса

            foreach (DataTable tabe in tableCollection)
            {
                toolStripComboBox1.Items.Add(tabe.TableName);
            }
            toolStripComboBox1.SelectedIndex = 0;

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable table = tableCollection[Convert.ToString(toolStripComboBox1.SelectedItem)];
            dataGridView1.DataSource = table;
        }
    }
}
