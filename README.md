# BloodBanking

## Project: Blood Donation Database System to manage donor registrations, blood stock, and donation records

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

### 3 - 

### Insert User  
![inserir](https://github.com/HenriqueLopesDeSouza/BloodBanking/assets/43977679/d72922fc-58bf-4ed1-8b7f-0013af02648a)

### Registrar Donation 
![donation1](https://github.com/HenriqueLopesDeSouza/BloodBanking/assets/43977679/ae04f2db-0e24-4b23-be55-3f96f466886f)

### Email 
![blookdown](https://github.com/HenriqueLopesDeSouza/BloodBanking/assets/43977679/3e84bcbe-4fd7-4602-a041-8d54bdeb6785)

### Pdf 
![pdf](https://github.com/HenriqueLopesDeSouza/BloodBanking/assets/43977679/4de28394-0b34-4440-b3fc-da7eb504993a)

### Swagger
![image](https://github.com/HenriqueLopesDeSouza/BloodBanking/assets/43977679/467870ed-7fd3-4d37-800f-fe5bfefc1459)


### Directory Structure
![image](https://github.com/HenriqueLopesDeSouza/BloodBanking/assets/43977679/d5a39075-0cd3-4217-8835-177b606a55c7)
