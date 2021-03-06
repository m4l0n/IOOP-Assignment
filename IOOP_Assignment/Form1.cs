using FluentValidation.Results;
using IOOP_Assignment.Validator;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOOP_Assignment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lblInvalidLogin.Hide();
            lblRegSuccess.Hide();
        }

        private void regButton_Click(object sender, EventArgs e)
        {
            User.name = nameTxtBox.Text;
            User.tpNumber = tpTxtBox.Text;
            User.regEmail = regEmailTxtBox.Text;
            User.regPass = regPassTxtBox.Text;
            User.confPass = confPassTxtBox.Text;
            User registration = new User();
            //Validate Registration Data
            RegistrationValidator validator = new RegistrationValidator();
            ValidationResult result = validator.Validate(registration);
            if (!result.IsValid)    //Data invalid format
            {
                foreach (ValidationFailure failure in result.Errors)
                {
                    bunifuSnackbar1.Show(this, failure.ErrorMessage,
                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 4000);
                }
            }
            else   //Data valid format
            {
                registerUser();
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Passwords should contain at least 8 characters, at least a lowercase and an uppercase.",
                "Password Format", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void showPwToggle_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            passwordTxtBox.PasswordChar = showPwToggle.Checked ? '\0' : '●';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User.loginEmail = emailTxtBox.Text;
            User.loginPass = passwordTxtBox.Text;
            User login = new User();
            //Validate Login Details
            LoginValidator validator = new LoginValidator();
            ValidationResult result = validator.Validate(login);
            if (!result.IsValid)    //Data invalid format
            {
                foreach (ValidationFailure failure in result.Errors)
                {
                    bunifuSnackbar1.Show(this, failure.ErrorMessage,
                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 4000);
                }
            }
            else    //Data valid format
            {
                loginUser();
            }
        }

        private void btnRedirectRegister_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 1;
        }

        private void btnRedirectLogin_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 0;
        }

        private void shapeClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void shapeMinimise_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        /// <summary>
        /// This method is used to register the user into the system
        /// </summary>
        private async void registerUser()
        {
            int result2;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("insert into [dbo].[User] values (@tpNum,@name,@email,@pass," +
                    "'Student')", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@tpNum", User.tpNumber);
                    cmd.Parameters.AddWithValue("@name", User.name);
                    cmd.Parameters.AddWithValue("email", User.regEmail);
                    cmd.Parameters.AddWithValue("@pass", User.regPass);
                    result2 = cmd.ExecuteNonQuery();
                }
            }
            if (result2 != 0)
            {
                lblRegSuccess.Show();
                await Task.Delay(2000);
                bunifuPages1.PageIndex = 0;
            }
            else
            {
                MessageBox.Show("Unable to Register! Please contact the system administration for support", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// This method is used to validate the login credentials.
        /// </summary>
        private void loginUser()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                int count;
                using (SqlCommand cmd = new SqlCommand("select count(*) from [dbo].[User] where Email='" + User.loginEmail
                + "' and Password='" + User.loginPass + "'", con))
                {
                    count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                if (count > 0)
                {
                    using (SqlCommand cmd2 = new SqlCommand("select Role from [dbo].[User] where email = '"
                        + User.loginEmail + "'", con))
                    {
                        User.userRole = cmd2.ExecuteScalar().ToString();
                    }
                    lblInvalidLogin.Hide();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else lblInvalidLogin.Show();
            }
        }

        private void passwordTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }
    }
}