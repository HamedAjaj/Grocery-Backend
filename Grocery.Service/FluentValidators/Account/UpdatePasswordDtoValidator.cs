using FluentValidation;
using Grocery.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Service.FluentValidators.Account
{
    public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email address is required.").EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.CurrentPassword).NotEmpty()
                .WithMessage("Current password is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("The new password and confirmation password do not match.");
        }
    }
}
