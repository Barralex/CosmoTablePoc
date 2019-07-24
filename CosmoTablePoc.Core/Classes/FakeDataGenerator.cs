using Bogus;
using CosmoTablePoc.Core.Models;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmoTablePoc.Core.Classes
{
    public static class FakeDataGenerator
    {

        public static List<MetadataEntity> GenerateData(int amount)
        {
    
            var metadataList = new List<MetadataEntity>();

            var options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };

            using (var pbar = new ProgressBar(amount, "Genereting Fake Data", options))
            {

                Parallel.For(0, amount, i =>
                {
                    lock (metadataList)
                    {
                        metadataList.Add(FakeRow());
                        pbar.Tick();
                    }

                });

            }

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
