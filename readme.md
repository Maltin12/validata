## Assumptions Made During Design & Implementation

1. **Customer Data Uniqueness**  
   - Customers are uniquely identified by a system-generated `Guid` ID, not by name or address.  
   - Duplicate names are allowed for different customers.

2. **Order and Item Relationship**  
   - Each order must contain at least one item.  
   - Items are linked to products via a foreign key relationship.  
   - `UnitPrice` in `OrderItem` is stored at the time of order creation to preserve historical pricing even if the product price changes later.

3. **Product Entity**  
   - Products are identified by a `Guid` ID.  
   - Product names are **not enforced as unique** at the database level.  
   - Price is stored as `decimal(18,2)` for precision.

4. **Persistence & Database**  
   - Entity Framework Core is used for persistence.  
   - Repository & Unit of Work patterns are implemented for data access abstraction.  
   - All database IDs are GUIDs.  
   - Read-only queries use `AsNoTracking` for performance.

5. **CQRS Separation**  
   - Commands are used for create, update, and delete operations.  
   - Queries are used for read-only operations.  
   - MediatR is used to implement the CQRS pattern.

6. **Order Iteration**  
   - Orders can be retrieved per customer, ordered by `OrderDate` ascending.

7. **Domain Logic Priority**  
   - Business logic resides in the domain layer, not in controllers.  
   - Controllers only orchestrate commands/queries.

8. **Error Handling**  
   - If an entity is not found, the API returns `404 Not Found`.  
   - Updates return `204 No Content` if successful.  
   - Validation errors return `400 Bad Request`.

9. **Testing**  
   - NUnit is used for unit testing with basic coverage for domain and application layers.

10. **API Documentation**  
    - XML comments are enabled for Swagger/OpenAPI generation.  
    - Summaries and parameter descriptions are provided for public API endpoints.
