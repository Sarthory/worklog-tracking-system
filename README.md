# 🕧 Worklog Tracking System

## Description

This project was implemented as a coding challenge for BCC opportunity.
It consists in 2 layers:

- API, located at ./WorklogTrackingSystem.API
- Front, located at ./WorklogTrackingSystem.Front

The API serves the endpoints to manage Users and Worklogs.

The Front is responsible for conducting the user through the flow and showing Users , Worklogs and Worklog Entries.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 9+
- Node.js 18+
- Docker (optional)
- Database MSSQL 2022 (for production env)

## Running the Project in Development mode

Clone this repository in your local machine.

Run the following command on the solution API folder _(your_project_path/worklog-tracking-system/WorklogTrackingSystem.API)_:

- Powershell
  `$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet ef database update -c SqliteDbContext -p ../WorklogTrackingSystem.Infrastructure`

- Bash
  `ASPNETCORE_ENVIRONMENT="Development" dotnet ef database update -c SqliteDbContext -p ../WorklogTrackingSystem.Infrastructure`

This will create the SQLite database, tables, and associations.

Navigate to the Scripts folder _(your_project_path/worklog-tracking-system/WorklogTrackingSystem.Sripts)_:

- If you are using bash, execute the following script:

  `./bash_run_everything.sh`

- If you are using Powershell, execute the following script:

  `./pwsh_run_everything.ps1`

👉🏼 From the menu, choose "(D)evelopment" option.

The API and Front will start, and a browser window should pop up, loading the frontend.

## Running the Project in Production mode

Clone this repository in your local machine.

⚠️ **\*IMPORTANT\***

👉🏼 **Make sure that your MSSQL instance is running!**

👉🏼 **Make sure to replace "#CONNSTRING#" with YOUR MSSQL connection string!** 👇🏽

By updating the **_appsettings.Production.json_** file at API folder _(your_project_path/worklog-tracking-system/WorklogTrackingSystem.API)_:

Still in the API folder _(your_project_path/worklog-tracking-system/WorklogTrackingSystem.API)_, run the following entity framework command:

- Powershell
  `$env:ASPNETCORE_ENVIRONMENT="Production"; dotnet ef database update -c SqlServerDbContext -p ../WorklogTrackingSystem.Infrastructure`

- Bash
  `ASPNETCORE_ENVIRONMENT="Production" dotnet ef database update -c SqlServerDbContext -p ../WorklogTrackingSystem.Infrastructure`

This will create the SQL Server database, tables, and associations.

Change working directory to the Scripts folder _(your_project_path/worklog-tracking-system/WorklogTrackingSystem.Sripts)_:

- If you are using Powershell, execute the following script:

  `./pwsh_run_everything.ps1`

- If you are using bash, execute the following script:

  `./bash_run_everything.sh`

👉🏼 From the menu, choose "(P)roduction" option.

The API and Front will start, and a browser window should pop up, loading the frontend.

## Logging In and Using this Software

On the first execution, for both environments, the database will be seeded with 10000 users and their Worklog entries.

From these 10000 users, only 1 will be created with Admin role, and the rest with User role.

## All the seeded users have the same password: **1234**

- Logging in as Admin
  - Login: admin
  - Password: 1234

---

- Logging in as User
  - Login: _user(number from 2 to 10000)_ Ex.: user1000
  - Password: 1234

---

## Contact

**Felipe Sartori** - felipe@sartori.app

[Sartori Apps](https://www.sartori.app) - [Linkedin](#https://www.linkedin.com/in/ff-sartori/)
