using FluentValidation;
using Grocery.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Service.FluentValidators.Order
{
    public class BasketItemDtoValidator : AbstractValidator<BasketItemDto>
    {
        public BasketItemDtoValidator()
        {
            RuleFor(item => item.Id).NotEmpty().WithMessage("Id is required.");

            RuleFor(item => item.ProductName).NotEmpty().WithMessage("Product name is required.");

            RuleFor(item => item.Price)
                .NotEmpty().WithMessage("Price is required.")
                .ExclusiveBetween(0.1m,decimal.MaxValue).WithMessage("Price must be greater than zero.");
            // or   .Must(value => value > 0.1m).WithMessage("Price must be greater than zero.");

            RuleFor(item => item.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .InclusiveBetween(1, int.MaxValue).WithMessage("Quantity must be at least one item.");

            RuleFor(item => item.PictureUrl).NotEmpty().WithMessage("Picture URL is required.");

            RuleFor(item => item.Brand).NotEmpty().WithMessage("Brand is required.");

            RuleFor(item => item.Type).NotEmpty().WithMessage("Type is required.");
        }
    }
}
