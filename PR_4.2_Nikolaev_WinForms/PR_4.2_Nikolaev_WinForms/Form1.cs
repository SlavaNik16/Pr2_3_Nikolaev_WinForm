using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR_4._2_Nikolaev_WinForms
{
    public partial class Form1 : Form
    {
        private decimal n, m;
        private int[,] mas;
        public Form1()
        {
            InitializeComponent();
        }

        private void butInput_Click(object sender, EventArgs e)
        {

            //Берем Строки(n) и Столбцы(m) из numericUpDown
            n = Nnum.Value;
            m = Mnum.Value;
            //Очищаем DataGridView чтобы не добавлялись доп.ячейки
            DataGrid.Columns.Clear();
            mas = new int[(int)n, (int)m];
            //Цикл добавление столбцов
            for (int i = 0; i <= m; i++)
            {
                DataGrid.Columns.Add("", "");
                if (i > 0) DataGrid.Columns[i].HeaderText = (i - 1).ToString();
                DataGrid.Columns[i].Width = 40;

            }
            //Цикл добавление строк
            for (int j = 0; j < n; j++)
            {
                DataGrid.Rows.Add(j.ToString());
                DataGrid.Rows[j].Cells[0].ReadOnly = true; //Запрещаем изменять 
            }
            //Заполняем DataGridView по умолчанию каким-то знаком,
            //чтобы не вызывало исключений
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {

                   DataGrid.Rows[i].Cells[j+1].Style.BackColor = (i + j) % 2 == 0 ? Color.Yellow : Color.Orange;
                   DataGrid.Rows[i].Cells[j + 1].Value = "!";
                    
                }
            }
            //Отключаем сортировку
            foreach (DataGridViewColumn column in DataGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //Активируем кнопку вывода
            butOutput.Enabled = true;
        }

        private void butExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        //Запрещаем вводить буквы и другие знаки в ячейки
        private void DataGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            
            if (DataGrid.CurrentCell.ColumnIndex == 1)
            {
                e.Control.KeyPress += new KeyPressEventHandler(DataGrid_KeyPress);
            }
        }
        private void DataGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
        //
        private void butOutput_Click(object sender, EventArgs e)
        {
            int k = 0;
            int p;
            //Очищаем ListBox
            ResultListArray.Items.Clear();
            //Делаем проверку на то, что пользователь вел все ячейки значением
            p = Validate();
            if (p == 0)
            {
                //Заполняем массив и ищем нечетный элемент на четных ячейках
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        mas[i, j] = int.Parse(DataGrid.Rows[i].Cells[j + 1].Value.ToString());
                        if (mas[i, j] % 2 != 0 && (i + j) % 2 == 0)
                        {
                            k++;
                            //Поля которые выполняются по условию мы закрашиваем красным
                            DataGrid.Rows[i].Cells[j + 1].Style.ForeColor = Color.Red;
                            string r = $"{i},{j}";
                            ResultListArray.Items.Add(r);
                        }
                        else
                        {
                            //остальное оставляем черным
                            DataGrid.Rows[i].Cells[j + 1].Style.ForeColor = Color.Black;
                        }
                        
                    }
                }
                //Заполняем количество таких ячеек в TextBox
                CountBox.Text = k.ToString();
            }
            else
            {
                MessageBox.Show("Заполните все ячейки!!!", "Ошибка",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int Validate()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (DataGrid.Rows[i].Cells[j + 1].Value == "!")
                    {
                        return 1;
                    }
                }
               
            }
            return 0;
        }


    }
}
