using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOOP_Assignment.Validator
{
    public class RegistrationValidator : AbstractValidator<ValidationVariables>
    {
        public RegistrationValidator()
        {
            //Stop as soon as the validator fails
            CascadeMode = CascadeMode.Stop;
            //Name Rule
            RuleFor(p => p.name)
                .NotEmpty().WithMessage("Name cannot be empty!")
                .Matches("^[a-zA-Z ,.'-]+$").WithMessage("Name format is not valid!");
            //TP Number Rule

            //Reg Email Rule
            RuleFor(p => p.regEmail)
                .NotEmpty().WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("A valid email address is required.");
            //Reg Password Rule
            RuleFor(p => p.regPass)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(8).WithMessage("Password must contain 8 or more characters!")
                .Matches("[A-Z]").WithMessage("Password must contain one or more capital letters!")
                .Matches("[a-z]").WithMessage("Password must contain one or more lowercase letters!")
                .Matches(@"\d").WithMessage("Password must contain one or more digits.");
            //Confirm Password Rule
            RuleFor(p => p.confPass)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(p => p.regPass).WithMessage("Passwords should match!");

        }


    }
}
