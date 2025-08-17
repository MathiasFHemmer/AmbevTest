# Ambev Sales CRUD:

## Design Philosophy

The system is divided into 6 projects, each solving a different problem inside software development:

1. *WebApi* > Offers a system friendly way of interacting with the domain application, by letting external entities (users, other systems, etc..) execute commands and queries. This layer offers action contracts with required or optional parameters that must be supplied to the domain in order to execute actions. It standardize the way others interact with the domain.

2. *Application* > Next to the WebApi, and much like it, reflects domain operations to external usage. This layer orchestrates the execution of the domain logic, making sure each operation is well defined, well documented, well tested and the information transactions contracts are valid for each specific operation. It also makes sure business logic is properly applied to domain models.

The Application layer presented here follows a Mediator Patterns, an entity that understands and delegates commands for each operation. This helps standardizing validation behavior and input/output contracts. 

3. *Domain* > This is the foundation of the business, a set of well defined entities that follow a specific purpose inside a domain. Here we can find Data Types with specific rules that must be followed to make sure no invariant is violated. _Policies_ and _Specifications_ define a variable number of *business conditions and rules* that those data types must enforce to make sure the domain operations are correct. _Events_ are raised by certain operations done the domain, and _Repositories_ offers contract bindings through interfaces that guarantee data access and data integrity.

*Domain Entities* implement operations on important business concepts, exposing just enough to developers to be able to interact with them, but hiding away details and irrelevant data from consumers. It tells what it can and cant do with the data related to it, but it hides how it does it, making sure no operation a developer does to it renders the domain entity in a invalid state.

4. *ORM* > Abstracts away specifications on how to handle data consume. It implements the functionality necessary on the _Repositories_, as well as define data access strategies and any other necessities from the data access layer.

5. *Common* > Represents common data and/or behavior that are no intrinsically tied to any specific layer. Some data objects might be used by every layer and have the same shape, or some logic can be abstract away and placed here.

6.  *IoC* > This is used to bind together the whole application, gluing all the necessary parts, services, making sure every dependency is properly set up, to run the application. 

### The Problem

"...You will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:"

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

#### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items


### The Solution

The _Sale Module_ represents Sales operations inside the system. This consists of the _Sale_ and _Sale Item_ entities and related business operations.

#### Sale

A _Sale_ is an Aggregate Root that contains sale information and sales operations. Sale Items represent a collection of products in this sale. All domain related operations on the _SaleItem_ are done by using the product id as a reference.

| Property         | Type                            | Default                             | Description                                                        |
| ---------------- | ------------------------------- | ----------------------------------- | ------------------------------------------------------------------ |
| `DiscountPolicy` | `IDiscountPolicy?`              | `QuantityBasedDiscountPolicy`       | Discount policy applied to this sale; can be null for no discount. |
| `SaleNumber`     | `string`                        | `Empty`                             | Sale number, must not be null or empty.                            |
| `SaleDate`       | `DateTime`                      | `UtcNow`                            | Date of the sale; defaults to current UTC time.                    |
| `CustomerId`     | `Guid`                          | `Empty`                             | Identifier of the customer.                                        |
| `CustomerName`   | `string`                        | `Empty`                             | Name of the customer.                                              |
| `BranchId`       | `Guid`                          | `Empty`                             | Identifier of the branch.                                          |
| `BranchName`     | `string`                        | `Empty`                             | Name of the branch.                                                |
| `TotalAmount`    | `decimal`                       | `0`                                 | Total amount of the sale; must be greater than zero.               |
| `Status`         | `SaleStatus`                    | `Unknown`                           | Current status of the sale (Pending, Completed, Cancelled, etc.).  |
| `SaleItems`      | `List<SaleItem>`                | `Empty`                             | Collection of active items in the sale (`Confirmed` items only).   |
| `CreatedAt`      | `DateTime`                      | `default(DateTime) `                | Timestamp when the sale was created.                               |
| `UpdatedAt`      | `DateTime?`                     | `Empty`                             | Timestamp of the last update to the sale.                          |
| `CompletedAt`    | `DateTime?`                     | `Empty`                             | Timestamp when the sale was marked completed.                      |

Sales can operate in range of ways:

| Operation              | Inputs                                                                                      | Description                                                                                             |
| ---------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| `Create`               | `string saleNumber, Guid customerId, string customerName, Guid branchId, string branchName` | Creates a new `Sale` instance with the provided identifiers and names; sets `CreatedAt` and `Status`.   |
| `UpdateDiscountPolicy` | `IDiscountPolicy? discountPolicy`                                                           | Updates the sale’s discount policy and recalculates discounts for associated items.                     |
| `Cancel`               | None                                                                                        | Cancels the sale if not already cancelled; updates `Status` and `UpdatedAt`.                            |
| `Complete`             | None                                                                                        | Completes the sale; updates `Status`, `UpdatedAt`, and `CompletedAt`; throws if already cancelled.      |
| `AddItem`              | `Guid productId, string productName, uint quantity, decimal unitPrice`                      | Adds a new item to the sale, applies discount policy, enforces quantity limits, and recalculates total. |
| `GetItem`              | `Guid productId`                                                                            | Retrieves a sale item by `productId` if it is confirmed; returns null otherwise.                        |
| `UpdateItemQuantity`   | `Guid productId, uint newQuantity`                                                          | Updates the quantity of an existing sale item, recalculates discounts, and updates total.               |
| `CancelItem`           | `Guid productId`                                                                            | Cancels a specific item in the sale and recalculates the total.                                         |

Those operations enforce the following rules:

_Complete_:
- Cannot complete a Sale in Cancelled status.

_UpdateDiscountPolicy_:
- Must recalculate each active item discount value and total sale value if successful.

_AddItem_
- Sale cannot have an Item in Confirmed state that relates to the same product being added.
- Item quantity to be added cannot be 0 or exceed [Restrictions Rule 2](#business-rules) amount of items.
- Must recalculate sales total if successful

_UpdateItemQuantity_
- Item quantity to be set cannot be 0 or exceed [Restrictions Rule 2](#business-rules) amount of items.
- Must recalculate sales total if successful

_CancelItem_
- Sale cannot be Completed
- Must recalculate sales total if successful

#### Sale Item
The _SaleItem_ entity represents an entry in a Sale, projecting  the product being sold, and data related to the sale.

| Property         | Type             | Default                  | Description                                                          |
| ---------------- | ---------------- | ------------------------ | -------------------------------------------------------------------- |
| `SaleId`         | `Guid`           | `Empty`                  | Identifier of the parent sale.                                       |
| `ProductId`      | `Guid`           | `Empty`                  | Identifier of the product; must not be empty.                        |
| `ProductName`    | `string`         | `Empty`                  | Name of the product; must not be empty.                              |
| `Quantity`       | `uint`           | `1`                      | Quantity of the product; must be greater than zero.                  |
| `UnitPrice`      | `decimal`        | `0`                      | Price per unit; must be greater than zero.                           |
| `Discount`       | `decimal`        | `0`                      | Discount applied (0–1); updated only via policy methods.             |
| `Status`         | `SaleItemStatus` | `Confirmed`              | Current status of the sale item (Confirmed, Cancelled, etc.).        |
| `CreatedAt`      | `DateTime`       | `UtcNow`                 | Creation timestamp of the sale item.                                 |
| `UpdatedAt`      | `DateTime?`      | `Empty`                  | Timestamp of the last update.                                        |
| `TotalPrice`     | `decimal`        | `0`                      | Total price after discount: `Quantity * UnitPrice * (1 - Discount)`. |
| `DiscountAmount` | `decimal`        | `0`                      | Amount saved due to discount: `(Quantity * UnitPrice) - TotalPrice`. |

Sale Items don't expose direct operations as they are manipulated by a _Sale_, but they do so through defined operations:

| Operation             | Inputs                            | Description                                                                       |
| --------------------- | --------------------------------- | --------------------------------------------------------------------------------- |
| `UpdateItemQuantity`  | `uint quantity`                   | Updates the quantity; throws if zero or item is cancelled.                        |
| `ApplyDiscountPolicy` | `IDiscountPolicy? discountPolicy` | Applies a discount policy to the item; clamps discount between 0 and 1.           |
| `Cancel`              | None                              | Cancels the item if not already cancelled; updates `Status` and `UpdatedAt`.      |

Those operations enforce the following rules:

Note 1:  _UpdateItemQuantity_ enforce not 0 rule, but the item limit on a sale is a _Sale_ business rule, and its enforce through _Policies_ instead of the _SaleItem_ handling it.
Note 2: `Discount` is in range 0 to 1 inclusive, this is used to calculate using the inverse value multiplied by the total amount. Ex: If the discount is 0.15, means the product has 15% of, and its calculated by doing `Quantity * UnitPrice * (1 - Discount)`: `1 * 100 * (1 - 0.15)` = `100*0.85` = `85`

_CancelItem_
- Sale cannot be Completed

_UpdateItemQuantity_
- Item cannot be Cancelled
- Item quantity to be added cannot be 0
- Must update total

_ApplyDiscountPolicy_
- Item cannot be Cancelled
- Discount must be in the range 0 (inclusive) to 1(inclusive)


## How To Run

### Configuring the Environment (Docker):

#### Dev Certificate:
Before running the program, you must ensure that you have a valid certificate to be used. The application will look for a certificate inside `{APPDATA}/ASP.NET/Https` folder. If any problems happen during this process, follow those steps to **clean and recreate** the developers certificate issued from dotnet. This will destroy existing certificates configurations!

- open a new terminal and run the following commands:
```
    // Removes any existing dev certificate
    dotnet dev-certs https --clean 
    // Ensures the direction the app looks for exists
    mkdir "%APPDATA%\ASP.NET\Https" 
    // Creates, registers and exports the certificate with the expected password
    dotnet dev-certs https -ep "%APPDATA%\ASP.NET\Https\app.pfx" -p "ambev@test"
```

- The *ambev.developerevaluation.webapi* Service in the _docker-compose.yml_ can be configure to expect any certificate in any location:
```
environment:
    # maps where the application will look for certificates. Make sure this points to the volume mapped folder bellow
    - ASPNETCORE_Kestrel__Certificates__Default__Path=/home/app/.aspnet/https/localhost.pfx
    # specifies the password for the certificate
    - ASPNETCORE_Kestrel__Certificates__Default__Password=test@1234
volumes:
    # maps the machine location to the location the application will look for certificates
    - ${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro
```

#### Ports:
The default ports mapped to the application are. Although http is mapped, the server is configured to redirect to https:
- 64247 HTTP
- 64248 HTTPS

####
1. Make sure to have Docker installed: https://www.docker.com/
2. Clone the project:
   git clone [the project](https://github.com/MathiasFHemmer/AmbevTest)
3. Navigate to the backend folder:
```
    cd backend
```
4. run:
```
    docker compose up -d --build
```
5. explore the api using swagger. In any browser, go to:
```
    https://localhost:64248/swagger/index.html
```


## Tests

No Integration or Functional tests have been written. Some important operations, such as Pagination, are yet to be secured by test cases.

Unit Tests have been extended to cover *most of*, if not all, business rules related rules on the [Restrictions Rule 2](#business-rules) requirements sections.

## Future Improvements

Some improvements can be done by extending the functionality of the api.

1. Currently there is no way to directly query for a _Sale_ by its id. This was done intentionally for now, as there is no benefit of query a single item (The ListSales will return all the necessary information)
2. There are no filters for Sales list. They can be easily implemented by 
    1. Defining a well documented set of rules to match fields on the entities (like the ones provide by documentation with suffixes or prefixes on request fields)
    2. Drill the filter props down from the _ListSalesRequests_ to the _ListSalesCommand_, all the way down to the _SaleRepository_ where the filters can be extracted and implemented in the appropriate method.
3. The same can be done in the ListSaleItems request.
4. The _SaleRepository_ method _GetByIdAsync_ is loading every _SaleItem_ entity and this can be optimized. Currently its sharing the behavior among other consumers that expect the _SaleItems_ property to be populated. A simple way would be exposing a property on the repository indicating wether or not to load related entities. A more DDD Compliant way would be exposing loading methods inside the _Sale_ entity, almost removing the coupling from the repository to the handler, but as we require it to update the entity already, it was left as is.
5. _CustomerRepository_ and _BranchRepository_ implementations are mocked to always return the request id of each entity. Validations are done if the provided entity does not exists, but for the operations describe here, we assumed they are correct. _CreateSaleHandlerTests_ can be referenced for the implementation details, but there are no tests covering missing customer or branch cases.


