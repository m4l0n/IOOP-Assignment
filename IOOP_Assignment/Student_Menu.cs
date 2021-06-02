using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOOP_Assignment
{
    public partial class Student_Menu : Form
    {
        public Student_Menu()
        {
            InitializeComponent();
        }

        private void Student_Menu_Shown(object sender, EventArgs e)
        {
            resDataGridView.Rows.Add(
                new object[]
                {
                    "1",
                    "Amber",
                    "A01",
                    "5",
                    "24 May 2021, 08:00 p.m.",
                    "2 hours",
                });
            resDataGridView.Rows.Add(
                new object[]
                {
                    "2",
                    "Blackthorn",
                    "B02",
                    "3",
                    "27 May 2021, 10:00 a.m.",
                    "1 hour",
                });
            resDataGridView.Rows.Add(
                new object[]
                {
                    "3",
                    "Cedar",
                    "C03",
                    "2",
                    "30 June 2021, 08:30 a.m.",
                    "3 hours",
                });
            resDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["resID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["roomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["roomNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["numStudents"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["dateTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["length"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(0);
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(1);
        }

        private void reportBtn_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(2);
        }

        private void bunifuShapes1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuShapes2_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void bunifuLabel19_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
