# Grocery-Backend

→ Project created to be maintainable, reusable, testable ,scalable, Solid , DRY principle, union architecture note [ can changed later to be a clean architecture with CQRS ] , auto mapper , Stripe Payment , Fluent Validators for DTOs , fluent api for domain classes , Redis for basket module and cashing requests , Rate limiter requests, unit testing like Xunit , InmemoryDatabase   Specification pattern , Mail Services ,  routing , ProjectmetaData like routing of endpoints  ,  standard response used  , 3 databases used

I've made it available. github : [   ]I would be happier to hear feedback or suggestions from you. 😍 💖

project consist from 5 layers 

layer 1 ⇒ Grocery  API

- Controllers
- Errors [  APIExceptionResponse , ApiResponse , Validation response ]
- Extensions
- Helpers [ like  attributes, mapping profiles]
- Middlewares [ like Exception middleware ]

layer 2 ⇒ Service layer

- Caching Response
- DTOs
- Fluent validators
- MailServices
- OrderServices
- PaymentServices
- TokenServices
- ServiceDependancies

Layer 3 ⇒ Repository Layer

- Data for GroceryDatabase [ Configuration of domain classes , DataSeed, Migrations, DBContext]
- Identity for IdentityGroceryDatabase [DBContext  , DataSeed, Migrations]
- Repositories Implementation
    - Basket repository
    - Generic Repository
- UnitOfWork
- SpecificationEvaluator

Layer 4 Domain layer

- Entities
- Grocery MetaData
- IRepositories
- IServices
    - IMailService
    - IOrderService
    - IPaymentService
    - IResponseCaching
    - ITokenService
    - 
- IUnitOfWorks
- Specifications

Layer 5 Testing Layer  basic testing:

- Xunit tool
- InMemoryDatabase
