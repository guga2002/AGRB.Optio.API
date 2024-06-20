**AGRB.Optio.API**
AGRB.Optio.API is a powerful RESTful API project developed for the optimization and management of agribusiness. 
This API provides multi-functional operations and facilitates the automation of data management and analysis processes.

**Project Description**
AGRB.Optio.API is designed to support various aspects of agribusiness management, including crop planning,
resource allocation, and financial tracking. The API offers endpoints for managing data related to farms, fields,
crops, and resources, allowing users to streamline their operations and make informed decisions based on real-time
data analysis. By integrating AGRB.Optio.API into your agribusiness workflow, you can enhance efficiency, reduce costs, and improve overall productivity.

**Getting Started**
Here you can find instructions to set up and configure the project on your
local machine for development and testing purposes.

**Prerequisites**
To set up the project, you need to have the following tools installed:

**.NET Core SDK (version 8.0 or later)**
Visual Studio or Visual Studio Code
SQL Server or another compatible database

**Installation**
Clone the repository to your local machine:
git clone https://github.com/guga2002/AGRB.Optio.API.git
Navigate to the project directory:
cd AGRB.Optio.API

**Install the required packages:**
dotnet restore

**Configuration:**
Configure the appsettings.json file with your database connection settings.
Run the database migrations:
dotnet ef database update

**Running the Application**
You can run the application using the following command:
dotnet run

The project will be available at **http://localhost:5000** or the port specified in the appsettings.json file.

**Usage**
You can use any API client (e.g., Postman) to interact with the API. Below are some example endpoints you can use:

GET /api/Merchants - Retrieve a list of all Merchants.
POST /api/Merchants - Create a new Merchant.
GET /api/Transactions - Retrieve a list of all Transactions.
GET /api/Statistics - Retrive statistic details.
GET /api/ExchangeRate - Retrieve a list of all ExchangeRate.
POST /api/Transaction - Create a new Transaction.
For detailed API documentation and further usage examples, refer to the API documentation available at
http://localhost:5000/swagger.

**Contributing**
If you would like to contribute to the project, please fork the repository and submit a pull request. For major changes, 
please open an issue first to discuss what you would like to change.
