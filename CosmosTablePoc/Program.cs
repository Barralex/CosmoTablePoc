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

            CloudTable table = await TableManagement<MetadataEntity>.CreateOrReferenceTableAsync(TableConstants.TableName);
            await TableManagement<MetadataEntity>.BulkInsertEntityAsync(table, FakeDataGenerator.GenerateData(bulkSize));

        }

    }

}
