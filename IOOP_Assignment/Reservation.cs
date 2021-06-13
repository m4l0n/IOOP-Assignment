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
    class Reservation
    {
        //Declare connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());

        private string roomType;
        private string date;
        private string time;
        private int numStudents;
        private string duration;
        private string studentID;
        private string roomNumber;
        private string resID;

        public string RoomType { get => roomType; set => roomType = value; }
        public string Date { get => date; set => date = value; }
        public string Time { get => time; set => time = value; }
        public int NumStudents { get => numStudents; set => numStudents = value; }
        public string StudentID { get => studentID; set => studentID = value; }
        public string Duration { get => duration; set => duration = value; }
        public string RoomNumber { get => roomNumber; set => roomNumber = value; }
        public string ResID { get => resID; set => resID = value; }

        public List<Reservation> getResData()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where StudentID ='"
                + User.tpNumber + "'", con);
            int activeReservation = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            if (activeReservation != 0) //only executes if there's record of reservation found for the user
            {
                /*
                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                DataTable dt = new DataTable(); //Creates a DataTable in the memory
                da.Fill(dt);    //Fills the DataTable with the result from SQL Query
                con.Close();
                List<Reservation> resList = new List<Reservation>();
                resList = (from DataRow dr in dt.Rows
                           select new Reservation()
                           {
                               roomType = dr["Room Type"].ToString(),
                               roomNumber = dr["Room Number"].ToString(),
                               date = dr["Date"].ToString(),
                               time = dr["Time"].ToString(),
                               numStudents = Convert.ToInt32(dr["Number of Students"]),
                               duration = dr["Duration"].ToString(),
                           }).ToList(); //LINQ returns a list of object
            */
                SqlCommand cmd2 = new SqlCommand("Select [Room Type], [Room Number], [Number of Students], " +
                    "format(Date, 'dd/MM/yyyy') as Date, " +
                    "CONVERT(varchar(10), CAST(Time as Time),0) as Time, " +
                    "Duration from [dbo].[Reservation] where StudentID='" + User.tpNumber + "'", con);
                List<Reservation> resList = new List<Reservation>();
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Reservation res = new Reservation
                        {
                            RoomType = (Convert.ToString(reader["Room Type"])),
                            RoomNumber = (Convert.ToString(reader["Room Number"])),
                            Date = (Convert.ToString(reader["Date"])),
                            Time = (Convert.ToString(reader["Time"])),
                            NumStudents = (Convert.ToInt32(reader["Number of Students"])),
                            Duration = (Convert.ToString(reader["Duration"]))
                        };
                        resList.Add(res);
                    }
                }              
                return resList;
            }
            else
            {
                return null;
            }
            con.Close();
        }

        /*public List<Reservation> getDailyRes()
        {
            SqlCommand cmd = new SqlCommand("select count(StudentID) from [dbo].[Reservation]", con);
            int activeReservation = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            SqlCommand cmd2 = new SqlCommand("Select [Room Type], [Room Number], [Number of Students], " +
                "format(Date, 'dd/MM/yyyy') as Date, " +
                "CONVERT(varchar(10), CAST(Time as Time),0) as Time, " +
                "Duration from [dbo].[Reservation] where StudentID='" + User.tpNumber + "'", con);
            if (activeReservation != 0) //only executes if there's record of reservation found for the user
            {

            }
        }
        */
        public List<Reservation> getDailyReport(string d, string rt)
        {
            date = d;
            roomType = rt;
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from [dbo].[Reservation]", con);
            int activeRes = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            con.Close();
            SqlCommand cmd2;
            if (activeRes != 0)
            {
                if (rt != "All")
                {
                    cmd2 = new SqlCommand("select [Room Type], format(Date, 'dd/MM/yyyy') as Date, " +
                        "CONVERT(varchar(10), CAST(Time as Time),0) as Time, [Number of Students], Duration," +
                        "ReservationID, StudentID, [Room Number] from [dbo].Reservation " +
                        "where [Room Type]='" + rt + "' and Date='" + d + "'", con);
                }
                else
                {
                    cmd2 = new SqlCommand("select [Room Type], format(Date, 'dd/MM/yyyy') as Date, " +
                        "CONVERT(varchar(10), CAST(Time as Time),0) as Time, [Number of Students], Duration," +
                        "ReservationID, StudentID, [Room Number] from [dbo].Reservation " +
                        "where Date='" + d + "'", con);
                }
                List<Reservation> resList = new List<Reservation>();
                con.Open();
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Reservation dailyReport = new Reservation
                        {
                            RoomType = (Convert.ToString(reader["Room Type"])),
                            RoomNumber = (Convert.ToString(reader["Room Number"])),
                            Date = (Convert.ToString(reader["Date"])),
                            Time = (Convert.ToString(reader["Time"])),
                            NumStudents = (Convert.ToInt32(reader["Number of Students"])),
                            Duration = (Convert.ToString(reader["Duration"])),
                            StudentID = (Convert.ToString(reader["StudentID"])),
                            ResID = (Convert.ToString(reader["ReservationID"]))
                        };
                        resList.Add(dailyReport);
                    }
                }
                return resList;
            }
            else
            {
                return null;
            }
        }
    }
}
