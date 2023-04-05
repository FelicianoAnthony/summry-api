# Summry API
---

- This is the backend for [summry_frontend](https://github.com/Anto-Gary/summry_frontend) repo
- This is the future data source for [shopify_scraper](https://github.com/Anto-Gary/shopify_scraper) repo

### Folder Structure 
---
1. appsettings.{{environment}}.json files
    * these files contain constants used across the API, DB connection strings, secrets, etc.
    * different files for different environments
    * secrets will be removed once there is a deployment process that handles secret substitution


2. ./ApiModels 
    - DTO layer (transforms query from DB into JSON response)


3. ./Constants
    - values should never change & are derived from the appsettings.{{environment}}.json file used on startup


4. ./Controllers 
    - routes
    - controllers call service (of a similar name) which calls repository (of a similar name)


5. ./Entities
    - classes that map to DB tables (the mapping happens in ./Repositories/SummryContext.cs)


6. ./Helpers
    - convenience methods that are shared by different parts of API (alternative would be storing them in service)


7. ./Middlewares
    - custom authorization check
    - catches all exceptions encountered within API & returns them according to [ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-7.0) docs


8. ./Migrations 
    - EF Core migrations for SummryContext

9. ./Properties 
    - used when debugging in Visual Studio

10. ./Repositories
    - [Unit of work](https://dev.to/moe23/step-by-step-repository-pattern-and-unit-of-work-with-asp-net-core-5-3l92) pattern implemented
    - uses [LINQKit](https://github.com/scottksmith95/LINQKit) to build queries 
    - only thing happening here is building query based on where clauses & joins
    - called by service of a similar name

11. ./Services
    - called by a controller of a similar name and calls repository of a similar name
    - business logic resides here
    


### How to run 
---

1. ```cs
    dotnet restore
   ```

2. ```cs
    dotnet run SummryApi 
   ```



### Ports 
---

1. listens on https://localhost:7256

2. swagger on https://localhost:7256/swagger/index.html






### GOTCHAS
---
1. code to "validate store url can be scraped by scraper" works but is commented out to make development easier
    StoreService.ConvertToEntities
        > if url isnt a shopify store, it's silently skipped


#### TO CONSIDER
StoreService.ConvertToEntities
    > need to create a UserSummryStoreService class...