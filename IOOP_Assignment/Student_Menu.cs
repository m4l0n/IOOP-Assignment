﻿using System;
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
        //Declare connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());


        public Student_Menu()
        {
            InitializeComponent();
            //Converts DatePicker to TimePicker
            timeReserve.Format = DateTimePickerFormat.Custom;
            timeReserve.CustomFormat = "HH:mm tt";
            timeReserve.ShowUpDown = true;
            newTimeRes.Format = DateTimePickerFormat.Custom;
            newTimeRes.CustomFormat = "HH:mm tt";
            newTimeRes.ShowUpDown = true;
        }

        private void Student_Menu_Shown(object sender, EventArgs e)
        {
            Reservation res = new Reservation();
            List<Reservation> resList = res.getResData();   //returns resList as List of Objects
            if (resList != null)
            {
                foreach (var ress in resList)    //Fills the table with data
                {
                    resDataGridView.Rows.Add(
                        new object[]
                        {
                            "",
                            ress.RoomType,
                            ress.RoomNumber,
                            ress.NumStudents,
                            ress.Date,
                            ress.Time,
                            ress.Duration,
                        });
                }
                foreach (var ress in resList)    //Fills the table with data
                {
                    tableReservationEdit.Rows.Add(
                        new object[]
                        {
                            "",
                            ress.RoomType,
                            ress.RoomNumber,
                            ress.NumStudents,
                            ress.Date,
                            ress.Time,
                            ress.Duration,
                        });
                }
            }
            //Bunifu DataGridView Style Formatting
            resDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colStudentNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tableReservationEdit.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colResIDEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colRoomTypeEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colRoomNumEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colNumStudentsEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colDateEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colTimeEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colDurationEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void Student_Menu_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select Name from [dbo].[User] where Email='" + User.loginEmail + "'", con);
            User.name = cmd.ExecuteScalar().ToString();
            lblName.Text = User.name;
            lblName2.Text = User.name;
            lblName3.Text = User.name;
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
            this.Close();   //Close application
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(0);    //Redirect to Dashboard
        }

        private void btnRoomRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(1);    //Redirect to Room Reservation
        }
        private void btnEditRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(2);    //Redirect to Reservation Modification
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(3);    //Redirect to Reservation Preview
        }
        private void btnPreviewCancel_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(1);
        }
        private void shapeMinimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;   //Minimise Form
            }
        }
        private void resDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Auto Increment first column
            this.resDataGridView.Rows[e.RowIndex].Cells["colResID"].Value = (e.RowIndex + 1).ToString();
            this.tableReservationEdit.Rows[e.RowIndex].Cells["colResIDEdit"].Value = (e.RowIndex + 1).ToString();
        }

        private void chkboxDate_CheckedChanged(object sender, EventArgs e)
        {
            dateNewReserved.Enabled = chkboxDate.Checked;
        }

        private void chkboxRoom_CheckedChanged(object sender, EventArgs e)
        {
            comboNewRoom.Enabled = chkboxRoom.Checked;
        }

        private void chkboxStudent_CheckedChanged(object sender, EventArgs e)
        {
            txtNewStudent.Enabled = chkboxStudent.Checked;
        }

        private void chkboxTime_CheckedChanged(object sender, EventArgs e)
        {
            newTimeRes.Enabled = chkboxTime.Checked;
        }

        private void chkboxDuration_CheckedChanged(object sender, EventArgs e)
        {
            comboNewDuration.Enabled = chkboxDuration.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            comboDuration.SelectedItem = null;
            comboRoom.SelectedItem = null;
            comboStudentNo.SelectedItem = null;
        }

        private void comboRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboRoom.SelectedIndex)
            {
                case 0:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 10; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
                case 1:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 8; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
                case 2:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 4; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
                case 3:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 2; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
            }
        }
    }
}
