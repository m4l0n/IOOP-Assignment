namespace IOOP_Assignment
{
    public class User
    {
        /// <value>Property <c>name</c> represents the Name of the user</value>
        public static string name { get; set; }

        /// <value>Property <c>tpNumber</c> represents the TPNumber(StudentID) of the user</value>
        public static string tpNumber { get; set; }

        /// <value>Property <c>regEmail</c> represents the email used during registration</value>
        public static string regEmail { get; set; }

        /// <value>Property <c>regPass</c> represents the password used during registration</value>
        public static string regPass { get; set; }

        /// <value>Property <c>confPass</c> represents the password used to double-confirm during registration </value>
        public static string confPass { get; set; }

        /// <value>Property <c>loginEmail</c> represents the email used during login</value>
        public static string loginEmail { get; set; }

        /// <value>Property <c>loginPass</c> represents the password used during login</value>
        public static string loginPass { get; set; }

        /// <value>Property <c>userRole</c> represents the role of the user</value>
        public static string userRole { get; set; }
    }
}