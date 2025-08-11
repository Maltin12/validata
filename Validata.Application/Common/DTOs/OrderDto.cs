namespace Validata.Application.Common.DTOs;

public record OrderItemOutDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);
public record OrderOutDto(Guid Id, DateTime OrderDate, decimal TotalPrice, List<OrderItemOutDto> Items);
