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

namespace IOOP_Assignment
{
    public partial class Student_Menu : Form
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());
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
                    "24 May 2021",
                    "8:00 p.m.",
                    "2 hours",
                });
            resDataGridView.Rows.Add(
                new object[]
                {
                    "2",
                    "Blackthorn",
                    "B02",
                    "3",
                    "27 May 2021",
                    "10:00 a.m.",
                    "1 hour",
                });
            resDataGridView.Rows.Add(
                new object[]
                {
                    "3",
                    "Cedar",
                    "C03",
                    "2",
                    "30 June 2021",
                    "08:30 a.m.",
                    "3 hours",
                });
            resDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colStudentNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Student_Menu_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select Name from [dbo].[User] where Email='" + User.loginEmail + "'", con);
            User.name = cmd.ExecuteScalar().ToString();
            lblName.Text = User.name;
            SqlCommand cmd2 = new SqlCommand("select TPNumber from [dbo].[User]  where Email='" 
                + User.loginEmail + "'", con);
            User.tpNumber = cmd2.ExecuteScalar().ToString();
            SqlCommand cmd3 = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where StudentID ='"
                + User.tpNumber + "'", con);
            int activeReservation = Convert.ToInt32(cmd3.ExecuteScalar().ToString());
            lblHi.Text = "Hi " + User.name + ",";
            lblActiveRes.Text = activeReservation.ToString();
            con.Close();
        }

        private void shapeClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(0);
        }

        private void btnRoomRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(1);
        }

        private void btnEditRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(2);
        }

        private void shapeMinimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
