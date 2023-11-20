using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class MainForm : Form
    {
        private int gCol, gRow, lItem;

        string SourceName;

        private void MainForm_Load(object sender, EventArgs e)
        {
            gCol = -1;
            gRow = -1;
            lItem = -1;
            SourceName = "";
            dataGridView1.ColumnCount = 5;
            dataGridView1.RowCount = 5;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 16);

            dataGridView1.MultiSelect = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowDrop = true;
            listBox1.AllowDrop = true;
            listBox2.AllowDrop = true;

            listBox1.Font = new Font("Microsoft Sans Serif", 16);
            listBox2.Font = new Font("Microsoft Sans Serif", 16);

            label1.TextAlign = ContentAlignment.MiddleCenter;
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label3.TextAlign = ContentAlignment.MiddleCenter;

            Random Rnd = new Random();
            for (int r = 0; r < 5; r++)
                for (int c = 0; c < 5; c++)
                    dataGridView1.Rows[r].Cells[c].Value = Rnd.Next(108);
            this.OnResize(null);
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo cellPosoition = dataGridView1.HitTest(e.X, e.Y);

                if (cellPosoition.RowIndex >= 0 && cellPosoition.ColumnIndex >= 0)
                {
                    SourceName = (sender as DataGridView).Name;
                    gCol = cellPosoition.ColumnIndex;
                    gRow = cellPosoition.RowIndex;

                    lItem = -1;

                    if (dataGridView1.Rows[cellPosoition.RowIndex].Cells[cellPosoition.ColumnIndex].Value != null)
                    {
                        string text = dataGridView1.Rows[cellPosoition.RowIndex].Cells[cellPosoition.ColumnIndex].Value.ToString();

                        if (text != "")
                            dataGridView1.DoDragDrop(text, DragDropEffects.Move);
                    }
                }
            }
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            if ((sender as Control).Name == SourceName)
                return;

            string value = e.Data.GetData(DataFormats.Text).ToString();
            Point cursorPosition = dataGridView1.PointToClient(new Point(e.X, e.Y));
            System.Windows.Forms.DataGridView.HitTestInfo cellPosition = dataGridView1.HitTest(cursorPosition.X, cursorPosition.Y);

            if (cellPosition.ColumnIndex != -1 && cellPosition.RowIndex != -1)
            {
                if (dataGridView1[cellPosition.ColumnIndex, cellPosition.RowIndex].Value == "")
                {
                    dataGridView1[cellPosition.ColumnIndex, cellPosition.RowIndex].Value = value;

                    if (lItem >= 0)
                        (this.Controls[SourceName] as ListBox).Items.RemoveAt(lItem);
                }
            }

        }


        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int itemPosition = (sender as ListBox).SelectedIndex;

                if (itemPosition != -1)
                {
                    SourceName = (sender as ListBox).Name;
                    gCol = -1;
                    gRow = -1;
                    lItem = itemPosition;
                    string text = (sender as ListBox).Items[itemPosition].ToString();

                    if (text != "")
                        (sender as ListBox).DoDragDrop(text, DragDropEffects.Move);
                }

            }

        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            if ((sender as Control).Name == SourceName)
                return;

            string value = e.Data.GetData(DataFormats.Text).ToString();
            Point cursorPosition = (sender as ListBox).PointToClient(new Point(e.X, e.Y));
            int itemPosition = (sender as ListBox).IndexFromPoint(cursorPosition);

            if (itemPosition == -1)
                (sender as ListBox).Items.Add(value);
            else
                (sender as ListBox).Items.Insert(itemPosition, value);

            if (gRow >= 0 && gCol >= 0)
                dataGridView1.Rows[gRow].Cells[gCol].Value = "";

            if (lItem >= 0)
                (this.Controls[SourceName] as ListBox).Items.RemoveAt(lItem);
        }

        private bool isSimple(int a)
        {
            int count = 0;
            if (a == 0) return false;

            for (int i = 1; i <= a; i++)
                if (a % i == 0) count++;

            if (count > 2) return false;
            else return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int L1 = 0;
            for (int i = 0; i < listBox1.Items.Count; i++)
                if (isSimple(Convert.ToInt32(listBox1.Items[i].ToString()))) L1++;

            int DG = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value.ToString() != "")
                        DG++;
                }

            int L2 = 0;
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                int c = Convert.ToInt32(listBox2.Items[i].ToString());
                if (isSimple(c)) L2++;
            }

            if (L1 == 0 && DG == 0 && L2 == listBox2.Items.Count)
                MessageBox.Show(
                "Вы справились  с заданием",
                "Победа",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show(
                "Вы не справились  с заданием",
                "Миссия проваплена успешнаААААААААААААААААААААА!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            DataGridView.HitTestInfo cellPosoition = dataGridView1.HitTest(e.X, e.Y);

            if (cellPosoition.RowIndex >= 0 && cellPosoition.ColumnIndex >= 0)
            {
                SourceName = (sender as DataGridView).Name;
                gCol = cellPosoition.ColumnIndex;
                gRow = cellPosoition.RowIndex;

                lItem = -1;

                if (dataGridView1.Rows[cellPosoition.RowIndex].Cells[cellPosoition.ColumnIndex].Value != null && dataGridView1.Rows[cellPosoition.RowIndex].Cells[cellPosoition.ColumnIndex].Value != "")
                {
                    string text = dataGridView1.Rows[cellPosoition.RowIndex].Cells[cellPosoition.ColumnIndex].Value.ToString();

                    if (isSimple(int.Parse(text)) == true)
                    {
                        listBox2.Items.Add(text);
                        dataGridView1.Rows[gRow].Cells[gCol].Value = "";

                    }
                    else
                    {
                        listBox1.Items.Add(text);
                        dataGridView1.Rows[gRow].Cells[gCol].Value = "";

                    }
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            listBox1.Top = 25;
            listBox1.Left = 5;
            listBox1.Width = (int)(this.ClientSize.Width * 0.25) - listBox1.Left;
            listBox1.Height = this.ClientSize.Height - listBox1.Top - 45;

            //nonoxeHne dataGridViewl u ero pasmepbl

            dataGridView1.Top = listBox1.Top;
            dataGridView1.Left = listBox1.Left * 2 + listBox1.Width;
            dataGridView1.Width = (int)(this.ClientSize.Width * 0.5) - listBox1.Left * 2;
            dataGridView1.Height = listBox1.Height;

            //nonoxeHne listBox2 un ero pasmepsl

            listBox2.Top = listBox1.Top;
            listBox2.Left = dataGridView1.Left + dataGridView1.Width + listBox1.Left;
            listBox2.Width = listBox1.Width;
            listBox2.Height = listBox1.Height;

            button1.Top = listBox1.Top + listBox1.Height + 5;
            button1.Left = dataGridView1.Left;
            button1.Width = dataGridView1.Width;
            button1.Height = this.ClientSize.Height - button1.Top - 5;

            //nonoxexHne labell

            label1.Top = 3;
            label1.Left = listBox1.Left + listBox1.Width / 2 - label1.Width / 2;

            //nonoxeHne label?

            label2.Top = label2.Top;
            label2.Left = dataGridView1.Left + dataGridView1.Width / 2 - label2.Width / 2;

            //nonoxeHne label3

            label3.Top = label3.Top;
            label3.Left = listBox2.Left + listBox2.Width / 2 - label2.Width / 2;

            //umkn ans nepebopa Bcex cTonbLoB Tabauupl

            foreach (System.Windows.Forms.DataGridViewColumn Col in dataGridView1.Columns)
                Col.Width = (dataGridView1.Width + 2) / 5; //pacHeT WUpWUHbLI CTONOLOB
                                                           //
            foreach (System.Windows.Forms.DataGridViewRow Row in dataGridView1.Rows)
                Row.Height = (dataGridView1.Height + 2) / 5; //pacHeT WUpWUHbLI CTONOLOB
        }


    }
}
