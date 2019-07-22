using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CosmoTablePoc.Core
{
    public class TableManagement<T> where T : TableEntity
    {
        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. " +
                    "Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. " +
                    "Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        public async Task<CloudTable> CreateOrReferenceTableAsync(string tableName)
        {
            string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;

            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            CloudTable table = tableClient.GetTableReference(tableName);

            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Created Table named: {0}", tableName);
            }
            else
            {
                Console.WriteLine("Table {0} already exists", tableName);
            }

            return table;
        }

        public async Task BulkInsertEntityAsync(CloudTable table, List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("Entities");

            try
            {
                double totalRequestCharge = 0;

                Stopwatch stopwatch = Stopwatch.StartNew();

                Console.Write("Inserting Fake Data into Cosmo DB Table API... ");

                using (var progress = new ProgressBar())
                {
                    int idx = 0;
                    foreach (var entity in entities)
                    {
                        totalRequestCharge += await InsertEntityAsync(table, entity);
                        progress.Report((double)idx / entities.Count);
                        Thread.Sleep(20);
                        idx++;
                    }

                }

                Console.WriteLine("Done.");

                stopwatch.Stop();

                Console.WriteLine($"The insert operation for {entities.Count} entities consumed {totalRequestCharge} request units");
                Console.WriteLine($"The insert operation for {entities.Count} entities took {stopwatch.Elapsed}");
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }

        }

        private async Task<double> InsertEntityAsync(CloudTable table, T entity)
        {
            double requestCharge = 0;

            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

            if (result.RequestCharge.HasValue)
            {
                requestCharge = result.RequestCharge.Value;
            }

            return requestCharge;
        }

    }
}
