# entity-framework-net6-banking

## EF Migration
1. To run migrations on VisualStudio you need to go to Tools ->  Nuget Package Manager -> Package Manager Console
2. On top of the Package Manager Console set as Default Project the one that has the db context
3. Commands: 
   - ```get-help entityframework```
   - ```cls``` 
     > clears the console
   - ```add-migration InitialMigration``` 
     > 'InitialMigration' is migration's name
   - ```update-database```
     > makes the changes on the database - if does not exist, it will create it locally
   - ```script-migration```
     > separation of cotrols by scripting migrations - generates an .sql file
   - ```Scaffold-DbContext -provider Microsoft.EntityFrameworkCore.SqlServer -connection "Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Banking_EF6"```
     > generates a SQL script from the DbContext, bypasses any migrations
   - ```remove-migration```
     > It will remove the last migration, if it is not commited on the database yet
4. ***Up*** means the changes are about to make, ***Down*** means the changes are undo whenever migration being rollback
   > An empty migration can be created and filled up with custom queries
5. Migration cannot be removed if it is already updated on database. To revert a change on db we just need to update the databse with the previous migration of the current one we want to remove.
   - ```update-database '1234567890_LastMigration'```
     > 1234567890_LastMigration is an example of migration's name
   
   
## Tools
1. EF Core Power Tools: 
   - Location: VisualStudio -> Tools -> ManageExtensions -> search by name of extension
   - Utility: Right click on .csproj which has the dbContext -> Click on EF Core Power Tools -> Chose between different functionalities
2. SQL Server Object Explorer: Visual Studio -> View -> SQL Server Object Explorer
3. Make Diagrams using: https://app.diagrams.net/
