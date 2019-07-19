using CosmoTablePoc.Core;
using CosmoTablePoc.Core.Classes;
using CosmoTablePoc.Core.Constants;
using CosmoTablePoc.Core.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace CosmosTablePoc
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () => await RunApplication()).GetAwaiter().GetResult();
        }

        private static async Task RunApplication()
        {
            Console.Write("Azure Cosmos DB Table - Enter bulk size: ");

            int bulkSize = int.Parse(Console.ReadLine());
            var tableManagment = new TableManagement<MetadataEntity>();

            CloudTable table = await tableManagment.CreateOrReferenceTableAsync(TableConstants.TableName);
            await tableManagment.BulkInsertEntityAsync(table, FakeDataGenerator.GenerateData(bulkSize));

        }

    }

}
