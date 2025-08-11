using FluentValidation;
using Validata.Application.Orders.Commands;

namespace Validata.Application.Common.Validators.OrderValidators;

public class OrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public OrderValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Items).NotNull().NotEmpty();
        RuleForEach(x => x.Items).ChildRules(i =>
        {
            i.RuleFor(x => x.ProductId).NotEmpty();
            i.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}
