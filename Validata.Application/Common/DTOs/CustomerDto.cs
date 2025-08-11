namespace Validata.Application.Common.DTOs;

public record CustomerDto(Guid Id, string FirstName, string LastName, string Address, string PostalCode);
