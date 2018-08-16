# Inventory

Simple project witn Angular Material modules frontend and .NET Core 2.1 WEB API backend.

The application periodically (at startup and then once a day) checks the DB for the presence of overdue products and sends their list to the email. 
Quartz Sheduler is used. Email is also sent when the product is sold.

Requirements: [Node.js > 8.0](https://nodejs.org/en/download/) and [.NET Core 2.1](https://www.microsoft.com/net/download)

Open project in VS2017. Rename file secrets.example.json to secrets.json and set appropriate data for SMTP server.

Run project.

A local database with test data will be created.

To recreate the database, uncomment line 93 (Database.EnsureDeleted();) of the Startup.cs file. 
