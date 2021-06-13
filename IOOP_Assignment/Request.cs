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
    class Request
    {
        //Declare connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());

        private string roomType;
        private string date;
        private string time;
        private int numStudents;
        private string duration;
        private int reservationID;
        private string studentID;

        public string RoomType { get => roomType; set => roomType = value; }
        public string Date { get => date; set => date = value; }
        public string Time { get => time; set => time = value; }
        public int NumStudents { get => numStudents; set => numStudents = value; }
        public string StudentID { get => studentID; set => studentID = value; }
        public string Duration { get => duration; set => duration = value; }
        public int ReservationID { get => reservationID; set => reservationID = value; }

        public List<Request> getReqData()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from [dbo].Request", con);
            int activeReq = (Convert.ToInt32(cmd.ExecuteScalar()));
            if (activeReq != 0)
            {
                SqlCommand cmd2 = new SqlCommand("select StudentID, [Room Type], " +
                    "format(Date, 'dd/MM/yyyy') as Date, " +
                    "CONVERT(varchar(10), CAST(Time as Time),0) as Time, " +
                    "[Number of Students], Duration, ReservationID from [dbo].Request", con);
                List<Request> reqList = new List<Request>();
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Request req = new Request
                        {
                            RoomType = (Convert.ToString(reader["Room Type"])),
                            Date = (Convert.ToString(reader["Date"])),
                            Time = (Convert.ToString(reader["Time"])),
                            NumStudents = (Convert.ToInt32(reader["Number of Students"])),
                            StudentID = (Convert.ToString(reader["StudentID"])),
                            Duration = (Convert.ToString(reader["Duration"])),
                            ReservationID = (Convert.ToInt32(reader["ReservationID"]))
                        };
                        reqList.Add(req);
                    }
                }
                return reqList;
            }
            else
            {
                return null;
            }
        }
        
    }
}
