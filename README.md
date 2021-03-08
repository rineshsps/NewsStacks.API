##API documentation
Swagger for api documentation

## Mapping
Automapper

##Database
MS SQL

##Unit tests
xUnit

## ORM
EFCore database firt approach.
Choose Database project  as defaulut in Pakcage manager console & run below script

PM> Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=news;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -Force  -OutputDir Models 

How to run API & Functions 

Choose mutltiple start up projects
Set > API, Functions are second project
