﻿using Microsoft.Azure.Cosmos.Table;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CosmoTablePoc.Core
{
    public class TableManagement<T> where T : TableEntity
    {

        private double RequestUnitsConsumed { get; set; }

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

            Console.WriteLine("");

            return table;
        }

        public async Task BulkInsertEntityAsync(CloudTable table, List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("Entities");

            try
            {

                Stopwatch stopwatch = Stopwatch.StartNew();

                var options = new ProgressBarOptions
                {
                    ProgressCharacter = '─',
                    ProgressBarOnBottom = true
                };

                using (var pbar = new ProgressBar(entities.Count, "Inserting Fake Data into Cosmo DB Table API...", options))
                {

                    var tasks = entities.Select(entity => InsertEntityAsync(table, entity,
                        new Progress<double>(operationResult =>
                        {
                            pbar.Tick();
                            RequestUnitsConsumed += operationResult;
                        })
                        ));

                    await Task.WhenAll(tasks);

                }

                stopwatch.Stop();

                Console.WriteLine($"The insert operation for {entities.Count} entities consumed {RequestUnitsConsumed} request units");
                Console.WriteLine($"The insert operation for {entities.Count} entities took {stopwatch.Elapsed}");

            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }

        }

        private async Task InsertEntityAsync(CloudTable table, T entity, IProgress<double> completionNotification)
        {
            double requestCharge = 0;

            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

            if (result.RequestCharge.HasValue)
            {
                requestCharge = result.RequestCharge.Value;
            }
            completionNotification.Report(requestCharge);
        }

    }
}
