# Meet.API
 
A Web based API for conducting Meetups

:information_source::triangular_flag_on_post: Built for Software Developers, as a demo and practice for me. :triangular_flag_on_post::information_source:

## Tools

- C#
- .NET 6
- EntityFramework
- NLog

## Running From Visual Studio / VS Code

This is not intended for production (end users), it is meant as demo or example of creating a RESTful Web API in C# and .NET.

> in order to run this project do the following ...
1. Set the JWT Key [Info on JWT](https://jwt.io/introduction) to something either in appsettings.json OR UserSecrets 
[docs](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) `"JwtKey": "SET_THIS_TO_SOMETHING_DONT_SHARE"` 
> :warning: If you get an error like **'IDX10653: The encryption algorithm 'http://www.w3.org/2001/04/xmldsig-more#hmac-sha256' 
requires a key size of at least '128' bits. Key '[PII of type 'Microsoft.IdentityModel.Tokens.SymmetricSecurityKey' is hidden. 
For more details, see https://aka.ms/IdentityModel/PII.]', is of size: '8'. (Parameter 'key')''** your JWT Key may need to be longer in length :warning:
2. you would need to run in Visual Studio Package Manager : `add-migration MigrationName` or Run `dotnet ef migrations add MigrationName`
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

