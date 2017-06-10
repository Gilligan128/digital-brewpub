# Digital Brewpub
A Digital Brewery tracker for Craftsmanship

## Use Cases

### MVP
- Allows one to store craft brewery's they have visited along with some notes about each.
- Allows for multiple users.  
- Allows users to search for brewery's by city or zipcode and includes displays notes from other users for a selected brewery.
  - Assumptions
    - This can list breweries not included by users.

### Nifty Extras (budget allowing)
- Allows logged in users to chat with each other.

## Delivery Principles
- Continuous Integration (with myself)
  - Using dotnet cli to script building the app.
  - frequent commits
  - each commit will build and run the tests.
- Test-driven Development
  - fast unit tests at the controller level. These may span multiple unmocked classes. (likely no service layer for this simple an app)
  - functional tests at teh controller level to test subsystem interaction.
- Pair Programming
  - My pair is a rubber duck (or my gray cat, as her time allows. She is, however, a keyboard hog)
  
  
 ## Architecture
  - Feature-Focusd
    - One of the first things I will do is organize the project by features (Search, Manage-Pubs, Chat)instead of Models/Controllers/Views. I almost uniformly advocate for this on all projects.
    - This will require a new viewengine, last I checked.
  - OAuth
    - I don't like managing logins and OAuth lets me focus on the strategic domain (well, er, there is kinda one)
  - Entity Framework
    - Entity Framework usually plays well with Dot Net stuff right out the door and is good enough for an app this simple.
    - Alternative would be a document db but I ahve not used those enough to claim mastery.
    - Another alternative which I have mastered is NHibernate, but where is the fun in that.
    - I will not be using repositories but will likely use stubbed query objects kept as siple as possible (meaning most filtering will be done in memory)
      that are tested by the Functional tests (or integration tests)
    - I leverage Entity Framework migrations.
  - SQL Server 2016. Because it supports temporal queries. 
    - There are few thigns I feel should be in a default client architecture for a project. Historical data is one of them.
  - Metrics
    - Metrics is the other thing I believe belong in almost every client project.
    - If this were for a client and it was feasible, I would recommend Application Insights for this app.
   
   # Developer Setup
   
   ## Prerequisites
   - Dotnet CLI
   - SQL Server 2016
   - ASPNET_CORE_ENVIRONMENT variable set to "Development" 
   
   ## Setup
   On the Command Line:
   - CD Digital-BrewPub
   - dotnet restore
   - CD Digital.BrewPub.Test
   - dotnet test (this will build the database and migrate it)
   - dotnet run
   
