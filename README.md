**AGRB.Optio.API**
<br>
AGRB.Optio.API is a powerful RESTful API project developed for the optimization and management of Bank Transactions. <br>
This API provides multi-functional operations and facilitates the automation of data management and analysis processes.

## Project Description
AGRB.Optio.API is designed to support various aspects of Transacions management, including Analys, planning,
resource allocation, and financial tracking.<br> The API offers endpoints for managing data related cases, fields,
Transactions, and resources, allowing users to streamline their operations and make informed decisions based on real-time
data analysis.<br> By integrating AGRB.Optio.API into your Buisnes workflow, you can enhance efficiency, reduce costs, and improve overall productivity.<br>

## Getting Started
Here you can find instructions to set up and configure the project on your
local machine for development and testing purposes.<br>

## Prerequisites
To set up the project, you need to have the following tools installed:<br>

**.NET Core SDK (version 8.0 or later)**<br>
```sh
Visual Studio or Visual Studio Code
```
```sh
SQL Server or another compatible database
```
```sh
Mongo Server (Version 7.0 or later)
```

## Installation
Clone the repository to your local machine:<br>
```sh
git clone https://github.com/guga2002/AGRB.Optio.API.git
```
Navigate to the project directory:<br>
```sh
cd AGRB.Optio.API
```

<br>**Install the required packages:**<br>
```sh
dotnet restore
```
<br>**Configuration:**<br>
Configure the appsettings.json file with your database connection settings.<br>
Run the database migrations:
```sh
dotnet ef database update
```

## Running the Application
You can run the application using the following command:
```sh
dotnet run
```
<br>

The project will be available at Localhost 
```sh
http://localhost:5000 or port check in appseting.json
```

## Usage
You can use any API client to interact with the API
```sh
Postman
```
```sh
 Swagger
```

## Some Endpoints

- GET /api/Merchants - Retrieve a list of all Merchants.<br>
- POST /api/Merchants - Create a new Merchant.<br>
- GET /api/Transactions - Retrieve a list of all Transactions.<br>
- GET /api/Statistics - Retrive statistic details.<br>
- GET /api/ExchangeRate - Retrieve a list of all ExchangeRate.<br>
- POST /api/Transaction - Create a new Transaction.<br>
  For detailed API documentation and further usage examples, refer to the API documentation available at
  http://localhost:5000/swagger.

## Relational models: <br>
![image](https://github.com/guga2002/AGRB.Optio.API/assets/74540934/f3a2aacf-49ce-4567-acbd-ae820ffef948)

## Used Architecture:

![image](https://github.com/guga2002/AGRB.Optio.API/assets/74540934/acc022c0-ae1e-4d78-99ee-185dd8bad84a)
<br>
- **onion Architecture**
- **UniteOfWork Pattern**
- **solid Pricncipes**
- **DI for IOC** ( Inversion of controll)
- **Restfull principes**
## License
This project is licensed under the MIT License 
- contanct me  for licence : **aapkhazava22@gmail.com**

 ## Contributing
If you would like to contribute to the project, please:
- **fork the repository**
- **submit a pull request**
 <br> for major changes,please open an issue first to discuss what you would like to change.<br>
## Contact
For any questions or suggestions, please contact:
- **Author**: Guga Apkhazava , Raisa Badalova
- **Email**: guram.apkhazava908@ens.tsu.ge
- **SecondEmail**: raisa.badalova132@ens.tsu.ge


