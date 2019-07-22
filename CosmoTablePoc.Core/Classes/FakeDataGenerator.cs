using Bogus;
using CosmoTablePoc.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CosmoTablePoc.Core.Classes
{
    public static class FakeDataGenerator
    {

        public static List<MetadataEntity> GenerateData(int amount)
        {
            Console.Write("Genereting Fake Data... ");

            var metadataList = new List<MetadataEntity>();

            using (var progress = new ProgressBar())
            {
                for (int i = 0; i < amount; i++)
                {
                    metadataList.Add(FakeRow());
                    progress.Report((double)i / amount);
                    Thread.Sleep(20);
                }
            }

            Console.WriteLine("Done.");

            return metadataList;
        }

        private static MetadataEntity FakeRow()
        {

            var faker = new Faker("en");
            var fileName = faker.Lorem.Word();

            return new MetadataEntity(faker.Random.Number(1, 3).ToString(), fileName)
            {
                Tag = faker.Lorem.Word(),
                Class = faker.Lorem.Word(),
                FileName = fileName,
                Type = faker.Lorem.Word(),
                Description = faker.Lorem.Sentence(),
                CountryCode = faker.Address.CountryCode()
            };
        }

    }
}
