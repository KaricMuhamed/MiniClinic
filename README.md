How to start:

  Open Visual Studio.
  Select Open a Project or Solution.
  Navigate to the folder where you cloned the project and open the .sln file.

Install Dependencies
  Open NuGet Package Manager for the solution:
  Right-click on the solution in Solution Explorer.
  Select Manage NuGet Packages for Solution.
  Ensure the following packages are installed:
      Microsoft.EntityFrameworkCore.SqlServer
      Microsoft.EntityFrameworkCore.Tools
      Microsoft.EntityFrameworkCore.Design
  These packages are required for the Entity Framework migrations and database integration.

Configuration
    Before running the application, you need to update some settings in the appsettings.json file.
    
  Database Connection
  Update the connection string to match your local SQL Server setup:
  "ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MiniClinic;Trusted_Connection=true;TrustServerCertificate=true"
}

Replace YOUR_SERVER_NAME with the name of your SQL Server instance. If you're using SQL Server Express locally, it could be .\SQLEXPRESS.

API Token
To integrate with API Medic for API authentication, you'll need an API key.

Go to API Medic (https://apimedic.com/apitest) and obtain your API key.
Update the ApiToken field in appsettings.json:
"ApiToken": "YOUR_API_MEDIC_KEY"
Replace YOUR_API_MEDIC_KEY with your actual API key.

Running Migrations
After configuring your database connection, you need to run migrations to create the database tables.

Run Migrations with Entity Framework
    Open the Package Manager Console in Visual Studio:
    Tools > NuGet Package Manager > Package Manager Console.
    In the console, run the following command to apply the migrations:

    Update-Database
Running the Application
Once the database is set up, press F5 or click Run in Visual Studio to start the application.
The application will run locally on http://localhost:5000 (or another port if configured).
Default Admin Credentials
To log in as the admin:

Username: admin@example.ba
Password: adminpass
This will log you into the admin dashboard where you can manage users, clinics, and other application settings.
