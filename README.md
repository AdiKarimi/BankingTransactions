# BankingTransactions
Banking with Microservices architecture and .NET Core  
 
The application is build based on the microservices architecture. The high level design of Back-end architecture is: 
* Identity Microservice - Authenticates user based on username, password and issues a JWT Bearer token which contains Claims-based identity information in it. 
* Transaction Microservice - Handles account transactions like Get balance, deposit, withdraw
* API Gateway - Acts as a center point of entry to the back-end application, Provides data aggregation and communication path to microservices. 
*An API gateway takes all API calls from clients, then routes them to the appropriate microservice with request routing, composition, and protocol translation. Typically, it handles a request by invoking multiple microservices and aggregating the results, to determine the best path. It can translate between web protocols and web. 
 
Security : JWT Token based Authentication: JWT Token based authentication is implementated to secure the WebApi services. Identity Microservice acts as a Auth server and issues a valid token after validating the user credentitals. The API Gateway sends the token to the client. The client app uses the token for the subsequent request. 
Technologies 
    C#.NET 
    ASP.NET WEB API Core 
    SQL Server 
Opensource Tools Used 
    Automapper (For object-to-object mapping) 
    Entity Framework Core (For Data Access) 
    Swashbucke (For API Documentation) 
    XUnit (For Unit test case) 
    Ocelot (For API Gateway Aggregation) 
 
End-points configured and accessible through API Gateway 
 
    Route: "/user/authenticate" [HttpPost] - To authenticate user and issue a token 
    Route: "/account/balance" [HttpGet] - To retrieve account balance. 
    Route: "/account/deposit" [HttpPost] - To deposit amount in an account. 
    Route: "/account/withdraw" [HttpPost] - To withdraw amount from an account. 
 
End-points implemented at the Microservice level 
 
    Route: "/api/user/authenticate" [HttpPost]- To authenticate user and issue a token 
    Route: "/api/account/balance" [HttpGet]- To retrieve account balance. 
    Route: "/api/account/deposit" [HttpPost]- To deposit amount in an account. 
    Route: "/api/account/withdraw" [HttpPost]- To withdraw amount from an account. 
 
Solution Structure 
* Banking.Identity  o Handles the authentication part using username, password as input parameter and issues a JWT Bearer token with Claims-Identity info in it.
* Banking.Transactions  o Supports three http methods 'Balance', 'Deposit' and 'Withdraw'. Receives http request for these methods. o Handles exception through a middleware o Reads Identity information from the Authorization Header which contains the Bearer token o Calls the appropriate function in the 'Transaction' framework 
o Returns the transaction response result back to the client
* Banking.Framework  o Defines the interface for the repository (data) layer and service (business) layer o Defines the domain model (Business Objects) and Entity Model (Data Model) o Defines the business exceptions and domain model validation o Defines the required data types for the framework 'Struct', 'Enum', 'Consants' o Implements the business logic to perform the required account transactions o Implements the data logic to read and update the data from and to the SQL database o Performs the task of mapping the domain model to entity model and vice versa o Handles the db update concurrency conflict o Registers the Interfaces and its Implementation in to Service Collection through dependency injection
* Banking.Gateway  o Validates the incoming Http request by checking for authorized JWT token in it. o Reroute the Http request to a downstream service. 
* Banking.Client o A console client app that connects to Api Gateway, can be used to login with username, password and perform transactions like 'Balance', 'Deposit' and 'Withdraw' against a account. 
Exception Handling 
A Middleware is written to handle the exceptions and it is registered in the startup to run as part of http request. Every http request, passes through this exception handling middleware and then executes the Web API controller action method. 
* If the action method is successful then the success response is send back to the client.
* If any exception is thrown by the action method, then the exception is caught and handled by the Middleware and appropriate response is sent back to the client. 
Swagger: API Documentation 
Swashbuckle Nuget package added to the "Banking.Transaction Microservice" and Swagger Middleware configured in the startup.cs for API documentation. when running the WebApi service, the swagger UI can be accessed through the swagger endpoint "/swagger". 
To run the application 
*  Run the scirpt against SQL server to create the necessary tables and sample data  
*  Open the solution (.sln) in Visual Studio 2019  
*  Configure the SQL connection string in Banking.Transaction -> Appsettings.json file  
*  Configure the AppInsights Instrumentation Key in Banking.Transaction -> Appsettings.json file. I comment out the AppInsight related code in Startup.cs file as I didn’t have a key and didn't require log.  

Check the Banking.Identity -> UserService.cs file for Identity info. User details are hard coded for few accounts in Identity service which can be used to run the app. For instance, you can use the following credential to log in as a client: 
 
Account Number  Currency   Username  Password 1234567 USD johnsmith john1234 
 
* Run the following projects in the solution  
* Banking.Identity 
* Banking.Transaction 
* Banking.Gateway 
* Banking.Client 
* Gateway host and port should be configured correctly in the  Banking.Client 
* Banking.Idenity and Banking.Transaction service host and port should be configured correctly in the gateway -> configuration.json 

----------------------------------------------------------------------
# SQL

USE [master]
GO

IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'BankingTransactions')
BEGIN
CREATE DATABASE [BankingTransactions]
END
GO 

USE [BankingTransactions]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountSummary](
	[AccountNumber] [int] NOT NULL,
	[Balance] [decimal](19,2) NOT NULL,
	[Currency] [varchar](3) NOT NULL
	CONSTRAINT [PK_AccountSummary] PRIMARY KEY CLUSTERED([AccountNumber])
 );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountTransaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountTransaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [int] NOT NULL,
	[Date] DATETIME CONSTRAINT [DF_AccountTransaction_Date] DEFAULT (getutcdate()) NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[TransactionType] [varchar](10) NOT NULL,
	[Amount] [decimal](19,2) NOT NULL
	CONSTRAINT [PK_AccountTransaction] PRIMARY KEY CLUSTERED ([TransactionId] ASC),
	CONSTRAINT [FK_AccountTransaction_AccountNumber] FOREIGN KEY ([AccountNumber]) REFERENCES [dbo].[AccountSummary] ([AccountNumber]) 
 );
END
GO

IF NOT EXISTS (SELECT * FROM dbo.AccountSummary WHERE AccountNumber = 3628101)
BEGIN
    Insert Into dbo.AccountSummary(AccountNumber, Balance, Currency) 
	Values(1234567, 12000, 'USD')
END
GO

IF NOT EXISTS (SELECT * FROM dbo.AccountSummary WHERE AccountNumber = 3637897)
BEGIN
    Insert Into dbo.AccountSummary(AccountNumber, Balance, Currency) 
	Values(7654321, 7400, 'USD')
END
GO

IF NOT EXISTS (SELECT * FROM dbo.AccountSummary WHERE AccountNumber = 3648755)
BEGIN
    Insert Into dbo.AccountSummary(AccountNumber, Balance, Currency) 
	Values(1859624, 19860, 'USD')
END
GO
