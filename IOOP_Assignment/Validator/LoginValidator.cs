using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOOP_Assignment.Validator
{
    public class LoginValidator : AbstractValidator<User>
    {
        public LoginValidator()
        {
            CascadeMode = CascadeMode.Stop;
            //Login Email Rule
            RuleFor(p => p.loginEmail)
                .NotEmpty().WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("A valid email address is required.");
            //Login Password Rule
            RuleFor(p => p.loginPass)
                .NotEmpty().WithMessage("Password cannot be empty!")
                .MinimumLength(8).WithMessage("Password must contain 8 or more characters!")
                .Matches("[A-Z]").WithMessage("Password must contain one or more capital letters!")
                .Matches("[a-z]").WithMessage("Password must contain one or more lowercase letters!")
                .Matches(@"\d").WithMessage("Password must contain one or more digits.");
        }

    }
}
