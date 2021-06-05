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
using FluentValidation.Results;
using IOOP_Assignment.Validator;

namespace IOOP_Assignment
{
    public partial class Form1 : Form
    {
        private string userRole;
        
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());

        public string UserRole { get => userRole; set => userRole = value; }

        public Form1()
        {
            InitializeComponent();
            lblInvalidLogin.Hide();
        }

        private void regButton_Click(object sender, EventArgs e)
        {
            User registration = new User();

            registration.name = nameTxtBox.Text;
            registration.tpNumber = tpTxtBox.Text;
            registration.regEmail = regEmailTxtBox.Text;
            registration.regPass = regPassTxtBox.Text;
            registration.confPass = confPassTxtBox.Text;

            //Validate Registration Data
            RegistrationValidator validator = new RegistrationValidator();
            ValidationResult result = validator.Validate(registration);

            if (!result.IsValid)
            {
                foreach(ValidationFailure failure in result.Errors)
                {
                    bunifuSnackbar1.Show(this, failure.ErrorMessage,
                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 4000);
                }
            }
            else
            {
                MessageBox.Show("Operation Completed");
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 1;
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageIndex = 0;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Passwords should contain at least 8 characters, at least a lowercase and an uppercase.",
                "Password Format", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void showPwToggle_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (showPwToggle.Checked)
            {
                passwordTxtBox.PasswordChar = '\0';
            }
            else
            {
                passwordTxtBox.PasswordChar = '●';
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User login = new User();

            login.loginEmail = emailTxtBox.Text;
            login.loginPass = passwordTxtBox.Text;

            //Validate Login Details
            LoginValidator validator = new LoginValidator();
            ValidationResult result = validator.Validate(login);
            con.Open();
            if (!result.IsValid)
            {
                foreach (ValidationFailure failure in result.Errors)
                {
                    bunifuSnackbar1.Show(this, failure.ErrorMessage,
                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 4000);
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select count(*) from [dbo].[User] where Email='" + login.loginEmail 
                    + "' and Password='" + login.loginPass + "'", con);
                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (count > 0)
                {
                    SqlCommand cmd2 = new SqlCommand("select Role from [dbo].[User] where email = '" 
                        + login.loginEmail + "'", con);
                    UserRole = cmd2.ExecuteScalar().ToString();
                    lblInvalidLogin.Hide();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    lblInvalidLogin.Show();
                con.Close();
            }
        }
    }
}
