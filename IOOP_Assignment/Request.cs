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
        private string studentID;

        public string RoomType { get => roomType; set => roomType = value; }
        public string Date { get => date; set => date = value; }
        public string Time { get => time; set => time = value; }
        public int NumStudents { get => numStudents; set => numStudents = value; }
        public string StudentID { get => studentID; set => studentID = value; }
        public string Duration { get => duration; set => duration = value; }

        /*public List<Request> getReqData()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from [dbo].[Request]", con);
            int activeReq = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            if (activeReq != 0)
            {

            }
        }
        */
    }
}
