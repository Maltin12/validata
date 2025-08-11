## Assumptions Made During Design & Implementation

1. **Customer Data Uniqueness**  
   - Customers are uniquely identified by a system-generated `Guid` ID, not by name or address.  
   - The same name can be used by different customers.

2. **Order and Item Relationship**  
   - Each order must contain at least one item.  
   - Items are linked to products through a foreign key relationship.  
   - `UnitPrice` in `OrderItem` is stored at the time of order creation to preserve historical pricing even if the product price changes later.

3. **Product Entity**  
   - Products have unique names.  
   - Price is stored as `decimal(18,2)` for precision.

4. **Persistence & Database**  
   - Entity Framework Core is used for persistence.  
   - Repository & Unit of Work patterns are used for data access abstraction.  
   - All database IDs are GUIDs.

5. **CQRS Separation**  
   - Commands are used for create/update/delete operations.  
   - Queries are used for read-only operations.

6. **Order Iteration**  
   - Orders can be retrieved per customer, ordered by `OrderDate` ascending.

7. **Domain Logic Priority**  
   - Business logic resides in the domain layer, not in controllers.

8. **Error Handling**  
   - If an entity is not found, the API returns `404 Not Found`.  
   - Updates return `204 No Content` if successful.

9. **Testing**  
   - NUnit is used for unit testing (basic coverage provided).

10. **API Documentation**  
    - XML comments are enabled for API documentation generation (Swagger/OpenAPI).
