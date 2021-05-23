using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void regButton_Click(object sender, EventArgs e)
        {
            ValidationVariables registration = new ValidationVariables();

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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            ValidationVariables login = new ValidationVariables();

            login.loginEmail = emailTxtBox.Text;
            login.loginPass = passwordTxtBox.Text;

            //Validate Login Details
            LoginValidator validator = new LoginValidator();
            ValidationResult result = validator.Validate(login);

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
                MessageBox.Show("Operation Completed!");
            }
        }

        private void showPwToggle_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {
            if (showPwToggle.Checked)
            {
                passwordTxtBox.UseSystemPasswordChar = true;
            }
            else
            {
                passwordTxtBox.UseSystemPasswordChar = false;
            }
        }
    }
}
