using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOOP_Assignment.Validator
{
    public class RegistrationValidator : AbstractValidator<User>
    {
        public RegistrationValidator()
        {
            //Stop as soon as the validator fails
            CascadeMode = CascadeMode.Stop;
            //Name Rule
            RuleFor(p => User.name)
                .NotEmpty().WithMessage("Name cannot be empty!")
                .Matches("^[a-zA-Z ,.'-]+$").WithMessage("Name format is not valid!");
            //TP Number Rule
            RuleFor(p => User.tpNumber)
                .NotEmpty().WithMessage("TP Number cannot be empty!")
                .Matches("\\ATP0\\d{5}\\z").WithMessage("TP Number format is invalid.");
            //Reg Email Rule
            RuleFor(p => User.regEmail)
                .NotEmpty().WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("A valid email address is required.");
            //Reg Password Rule
            RuleFor(p => User.regPass)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(8).WithMessage("Password must contain 8 or more characters!")
                .Matches("[A-Z]").WithMessage("Password must contain one or more capital letters!")
                .Matches("[a-z]").WithMessage("Password must contain one or more lowercase letters!")
                .Matches(@"\d").WithMessage("Password must contain one or more digits.");
            //Confirm Password Rule
            RuleFor(p => User.confPass)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(p => User.regPass).WithMessage("Passwords should match!");
        }
    }
}
