# Meet.API
 
A Web based API for conducting Meetups

## Tools

- C#
- .NET 6
- EntityFramework
- NLog

> in order to run this project do the following
1. Set the JWT Key [Info on JWT](https://jwt.io/introduction) to something either in appsettings.json OR UserSecrets [docs](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) ``` JSON "JwtKey": "SET_THIS_TO_SOMETHING_DONT_SHARE" ``` 
2. you would need to run in Visual Studio Package Manager : ```BASH add-migration MigrationName ``` or Run ```CLI dotnet ef migrations add MigrationName ```
3. You should now be able to build and run :thumbsup:

![Screenshot 2023-02-05 at 16-53-12 Swagger UI](https://user-images.githubusercontent.com/20805058/216850654-a5bd38d5-0fae-493b-b3e1-1aff1edea460.png)

### Entity Framework Migrations Commands and Info
<details><summary>More Info ...</summary> 

Commands for DB Migrations

The Microsoft [Docs](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)

1. Create a Migration
___

At the very first time, you defined the initial domain classes. 
At this point, there is no database for your application which can store the data from your domain classes. 
So, firstly, you need to create a migration

> using the Package Manager Console in Visual Studio
```bash
PM> add-migration MigrationName
```

> using the CLI (any terminal / command line) and dotnet
```bash
> dotnet ef migrations add MigrationName
```

2. Creating or Updating the Database
___

```bash
PM> Update-Database 
```

```bash
> dotnet ef database update 
```

3. Removing a Migration
___

```bash
PM> remove-migration
```

```bash
> dotnet ef migrations remove
```

4. Reverting a Migration
___

```bash
PM> Update-database MigrationName 
```

```bash
> dotnet ef database update MigrationName
```

5. Generating a SQL Script
___

Use the following command to generate a SQL script for the database. 

```bash
PM> script-migration
```

```bash
> dotnet ef migrations script
```
</details>

