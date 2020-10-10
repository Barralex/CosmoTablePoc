# CosmoTablePoc
Proof of concept about Azure Cosmo BD API Table. Connection, fake data and seeding.

## Getting Started
Just clone the project and compile to download all the nugget packages.

In Settings.json file update the "StorageConnectionString" with your own Table API Primary Connection String.

If you don't have a Azure Account you can use for free [Azure Cosmos Emulator](https://aka.ms/cosmosdb-emulator) in order to test the application.

## Running the app

The app ask you for a number to create a list of fake data and insert into the Cosmo Azure Table. At the end the app will show how much time and 
RU/s (Request unit per second) took the whole operation.

## Built With

* [Bogus](https://github.com/bchavez/Bogus) - A simple and sane fake data generator for C#
* [Microsoft.Azure.Cosmos.Table](https://www.nuget.org/packages/Microsoft.Azure.Cosmos.Table) - Microsoft Azure Cosmos Table Standard Library

## Authors

* **Luis Barral** - *All work* - [Barralex](https://github.com/Barralex)

## License

This project is licensed under the MIT License
