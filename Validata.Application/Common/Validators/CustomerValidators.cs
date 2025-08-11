using FluentValidation;
using Validata.Application.Customers.Commands;

namespace Validata.Application.Common.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20);
        }
    }

    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20);
        }
    }
}
