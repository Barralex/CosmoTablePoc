using Bogus;
using CosmoTablePoc.Core.Models;
using System;
using System.Collections.Generic;

namespace CosmoTablePoc.Core.Classes
{
    public static class FakeDataGenerator
    {

        public static List<MetadataEntity> GenerateData(int amount)
        {
            Console.WriteLine("Genereting Fake Data");

            var metadataList = new List<MetadataEntity>();

            for (int i = 0; i < amount; i++)
            {
                metadataList.Add(FakeRow());
            }

            Console.WriteLine("Fake Data generated");

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
