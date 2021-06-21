using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace IOOP_Assignment
{
    internal class Request
    {
        private int requestID;
        private string roomType;
        private string date;
        private string time;
        private int numStudents;
        private string duration;
        private int reservationID;
        private string studentID;

        /// <value>Property <c>RoomType</c> represents the room type used when making a request to modify reservation</value>
        public string RoomType { get => roomType; set => roomType = value; }

        /// <value>Property <c>Date</c> represents the Date used when making a request to modify reservation</value>
        public string Date { get => date; set => date = value; }

        /// <value>Property <c>Time</c> represents the Time used when making a request to modify reservation</value>
        public string Time { get => time; set => time = value; }

        /// <value>Property <c>NumStudents</c> represents the Number of Students used when making a request to
        /// modify reservation</value>
        public int NumStudents { get => numStudents; set => numStudents = value; }

        /// <value>Property <c>StudentID</c> represents the TPNumber of the user who is modifying the reservation</value>
        public string StudentID { get => studentID; set => studentID = value; }

        /// <value>Property <c>Duration</c> represents the Duration used when making a request to modify reservation</value>
        public string Duration { get => duration; set => duration = value; }

        /// <value>Property <c>ReservationID</c> represents the ReservationID of the reservation that the student
        /// intend to modify</value>
        public int ReservationID { get => reservationID; set => reservationID = value; }

        /// <value>Property <c>RequestID</c> represents the RequestID used when making a request to modify reservation</value>
        public int RequestID { get => requestID; set => requestID = value; }

        /// <summary>
        /// This method is used to obtain the list of Requests awaiting librarian's actions from the Database
        /// </summary>
        /// <returns>A list of Request objects which include all the Request data, each object representing a row from the
        /// SQL result</returns>
        public List<Request> getReqData()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(*) from [dbo].Request", con);
                int activeReq = (Convert.ToInt32(cmd.ExecuteScalar()));
                if (activeReq != 0)
                {
                    SqlCommand cmd2 = new SqlCommand("select RequestID, StudentID, [Room Type], " +
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
                                RequestID = (Convert.ToInt32(reader["RequestID"])),
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

        /// <summary>
        /// This method is the algorithm to automatically assign a room number according to the selected room type, based on
        /// availability on the selected date
        /// </summary>
        /// <param name="req">Request object</param>
        /// <returns>The room number which is available on the selected date</returns>
        public string assignRoom(Request req)   //Method to assign a Room Number
        {
            string query = "SELECT SUBSTRING((select ',' + [Room Number] AS 'data()' FROM[dbo].Reservation " +
                "where [Room Type] = '" + req.RoomType + "' and Date = '" + req.Date + "' " +
                "FOR XML PATH('')),2,9999) AS [Room Numbers]";  //Concatenate all rows in result into a single string
            string assignedRoom;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    string takenRooms = cmd.ExecuteScalar().ToString();
                    takenRooms = takenRooms.Replace(",", "','");
                    string query2 = "SELECT TOP 1 [Room Number] FROM [dbo].Room WHERE [Room Number] NOT IN " +
                        "('" + takenRooms + "') and [Room Type]='" + req.RoomType + "'";
                    using (SqlCommand cmd2 = new SqlCommand(query2, con))
                    {
                        assignedRoom = cmd2.ExecuteScalar().ToString();
                    }
                }
                return assignedRoom;
            }
        }

        /// <summary>
        /// This method is used to add a Request record from the students into the Database
        /// </summary>
        /// <param name="req">Request object</param>
        /// <returns>The number of rows updated in the database</returns>
        public int addRequest(Request req)
        {
            string query = "INSERT INTO [dbo].Request VALUES(@roomtype, @date, @time, @student, @duration, @reservationid," +
                " @studentid)";
            int result;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@roomtype", req.RoomType);
                    cmd.Parameters.AddWithValue("@date", req.Date);
                    cmd.Parameters.AddWithValue("@time", req.Time);
                    cmd.Parameters.AddWithValue("@student", req.NumStudents);
                    cmd.Parameters.AddWithValue("@duration", req.Duration);
                    cmd.Parameters.AddWithValue("@reservationid", req.ReservationID);
                    cmd.Parameters.AddWithValue("@studentid", User.tpNumber);

                    result = cmd.ExecuteNonQuery(); //Add Request to Request Table
                }
            }
            return result;
        }

        /// <summary>
        /// This method is used to delete a Reservation record from the Database
        /// </summary>
        /// <param name="req">Request object containing ReservationID</param>
        /// <returns>The number of rows updated in the database</returns>
        public int deleteReservation(Request req)
        {
            string query = "DELETE FROM [dbo].Request WHERE [ReservationID]=@reservationid";
            string query2 = "DELETE FROM [dbo].Reservation WHERE [ReservationID]=@reservationid";
            int result;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    //Deletes any existing records from the table first due to Foreign Key constraints
                    cmd.Parameters.AddWithValue("@reservationid", req.ReservationID);
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd2 = new SqlCommand(query2, con))
                {
                    cmd2.Parameters.AddWithValue("@reservationid", req.ReservationID);
                    result = cmd2.ExecuteNonQuery(); //Delete reservation from Reservation Table
                }
            }
            return result;
        }
    }
}