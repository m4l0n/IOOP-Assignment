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

        public string RoomType { get => roomType; set => roomType = value; }
        public string Date { get => date; set => date = value; }
        public string Time { get => time; set => time = value; }
        public int NumStudents { get => numStudents; set => numStudents = value; }
        public string StudentID { get => studentID; set => studentID = value; }
        public string Duration { get => duration; set => duration = value; }
        public string RoomNumber { get => roomNumber; set => roomNumber = value; }

        public List<Reservation> getResData()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where StudentID ='"
                + User.tpNumber + "'", con);
            int activeReservation = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            SqlCommand cmd2 = new SqlCommand("Select [Room Type], [Room Number], [Number of Students], " +
                "format(Date, 'dd/MM/yyyy') as Date, " +
                "CONVERT(varchar(10), CAST(Time as Time),0) as Time, " +
                "Duration from [dbo].[Reservation] where StudentID='" + User.tpNumber + "'", con);
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
                List<Reservation> resList = new List<Reservation>();
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Reservation res = new Reservation();
                        res.RoomType = (Convert.ToString(reader["Room Type"]));
                        res.RoomNumber = (Convert.ToString(reader["Room Number"]));
                        res.Date = (Convert.ToString(reader["Date"]));
                        res.Time = (Convert.ToString(reader["Time"]));
                        res.NumStudents = (Convert.ToInt32(reader["Number of Students"]));
                        res.Duration = (Convert.ToString(reader["Duration"]));
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
    }
}
