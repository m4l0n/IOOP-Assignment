using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace IOOP_Assignment
{
    internal class Reservation
    {
        private string roomType;
        private string date;
        private string time;
        private int numStudents;
        private string duration;
        private string studentID;
        private string roomNumber;
        private int resID;
        public static string assignedRoom;

        /// <value>Property <c>RoomType</c> represents the Room Type used when making a reservation for rooms</value>
        public string RoomType { get => roomType; set => roomType = value; }

        /// <value>Property <c>RoomType</c> represents the Date used when making a reservation for rooms</value>
        public string Date { get => date; set => date = value; }

        /// <value>Property <c>RoomType</c> represents the room type used when making a reservation for rooms</value>
        public string Time { get => time; set => time = value; }

        /// <value>Property <c>RoomType</c> represents the Time used when making a reservation for rooms</value>
        public int NumStudents { get => numStudents; set => numStudents = value; }

        /// <value>Property <c>StudentID</c> represents the TPNumber of the user who is making a reservation for rooms</value>
        public string StudentID { get => studentID; set => studentID = value; }

        /// <value>Property <c>RoomType</c> represents the Duration used when making a reservation for rooms</value>
        public string Duration { get => duration; set => duration = value; }

        /// <value>Property <c>RoomType</c> represents the Room Number used when making a reservation for rooms</value>
        public string RoomNumber { get => roomNumber; set => roomNumber = value; }

        /// <value>Property <c>RoomType</c> represents the ReservationID used when making a reservation for rooms</value>
        public int ResID { get => resID; set => resID = value; }

        /// <summary>
        /// This method obtains the Reservation data of a Student from Database
        /// </summary>
        /// <returns>A list of Reservation objects which include Reservations data from Database. Each row represents
        /// an object</returns>
        public List<Reservation> getResData()
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
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                int activeReservation;
                using (SqlCommand cmd = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where StudentID ='"
                    + User.tpNumber + "' and Date >= getDate()", con))
                {
                    con.Open();
                    activeReservation = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                if (activeReservation != 0) //only executes if there's record of reservation found for the user
                {
                    List<Reservation> resList = new List<Reservation>();
                    using (SqlCommand cmd2 = new SqlCommand("Select ReservationID, [Room Type], [Room Number], " +
                        "[Number of Students], format(Date, 'dd/MM/yyyy') as Date, " +
                        "CONVERT(varchar(10), CAST(Time as Time),0) as Time, " +
                        "Duration from [dbo].[Reservation] where StudentID='" + User.tpNumber + "'and Date >= getDate()", con))
                    {
                        using (SqlDataReader reader = cmd2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Reservation res = new Reservation
                                {
                                    ResID = (Convert.ToInt32(reader["ReservationID"])),
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
                    }
                    return resList;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// This method obtains all the reservation data base on the selected date and room type to display on Daily Report
        /// </summary>
        /// <param name="d">string Date</param>
        /// <param name="rt">string roomType</param>
        /// <returns>A list of Reservation objects which include all Reservations data. Each row represents an object</returns>
        public List<Reservation> getDailyReport(string d, string rt)
        {
            date = d;
            roomType = rt;
            int activeRes;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select count(*) from [dbo].[Reservation]", con))
                {
                    activeRes = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                SqlCommand cmd2;
                if (activeRes != 0) //Checks if there are any reservations
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
                                ResID = (Convert.ToInt32(reader["ReservationID"]))
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

        /// <summary>
        /// This method is the algorithm to automatically assign a room number according to the selected room type, based on
        /// availability on the selected date
        /// </summary>
        /// <param name="res">Reservation object</param>
        /// <returns>The room number which is available on the selected date</returns>
        public string assignRoom(Reservation res)   //Method to assign a Room Number
        {
            string query = "select substring((select ',' + [Room Number] AS 'data()' FROM[dbo].Reservation " +
                "where [Room Type] = '" + res.RoomType + "' and Date = '" + res.Date + "' " +
                "FOR XML PATH('')),2,9999) AS [Room Numbers]";  //Concatenate all rows in result into a single string
            string assignedRoom;
            string takenRooms;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    takenRooms = cmd.ExecuteScalar().ToString();
                    takenRooms = takenRooms.Replace(",", "','");
                    string query2 = "select top 1 [Room Number] from [dbo].Room where [Room Number] NOT IN " +
                        "('" + takenRooms + "') and [Room Type]='" + res.RoomType + "'";
                    using (SqlCommand cmd2 = new SqlCommand(query2, con))
                    {
                        assignedRoom = cmd2.ExecuteScalar().ToString();
                    }
                }
                return assignedRoom;
            }
        }

        /// <summary>
        /// This method adds a reservation record from the user into the database
        /// </summary>
        /// <param name="res">Reservation object</param>
        /// <returns>The number of rows updated in the database</returns>
        public int addReservation(Reservation res)
        {
            string query = "insert into [dbo].Reservation values (@roomtype, @date, @time, @student, @duration, " +
                "@studentid, @assignedroom)";
            int result;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@roomtype", res.RoomType);
                    cmd.Parameters.AddWithValue("@date", res.Date);
                    cmd.Parameters.AddWithValue("@time", res.Time);
                    cmd.Parameters.AddWithValue("@student", res.NumStudents);
                    cmd.Parameters.AddWithValue("@duration", res.Duration);
                    cmd.Parameters.AddWithValue("@studentid", res.StudentID);
                    cmd.Parameters.AddWithValue("@assignedroom", Reservation.assignedRoom);

                    result = cmd.ExecuteNonQuery(); //Add Reservation into Reservation Table
                }
            }
            return result;
        }
    }
}