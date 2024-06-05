# BloodBanking
## Project: Blood Donation Database System to manage donor registrations, blood stock, and donation records

### This technology stack and architecture provide a scalable, maintainable, and secure solution for the Blood Banking Management System, including 
### This project has been developed using the following technologies and design patterns:
### 1 - Technology Stack:
        Backend Framework: .NET 8
        Architecture: Clean Architecture with Domain-Driven Design (DDD)
        Database Access: Utilizing Entity Framework Core (for ORM)
        Repository Pattern: Implemented to provide a standard approach to data access
        Email: MimeKit for sending emails
        Domain Events: MediatR for handling domain events
        PDF Generation: iText for generating PDF documents
        Testing: xUnit for unit testing

### 2 - Business Rules
        Prevent duplicate donor registration using the same email.
        Minors cannot donate, but they can be registered.
        Donors must weigh at least 50kg.
        Women can donate every 90 days.
        Men can donate every 60 days.
        The amount of blood donated must be between 420ml and 470ml.
