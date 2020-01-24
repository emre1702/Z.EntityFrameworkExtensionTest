using EntityFrameworkExtensionTest.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using Z.EntityFramework.Extensions;

namespace EntityFrameworkExtensionTest
{
    class Program
    {
        private const string _licenseName = "LICENSENAME";
        private const string _licenseKey = "LICENSEKEY";

        public static string ConnectionString => "Server=localhost;Database=EntityFrameworkExtensionTest;User ID=testuser;Password=testpw;";

        static void Main()
        {
            #region Z.EntityFramework license 
            LicenseManager.AddLicense(_licenseName, _licenseKey);
            #endregion Z.EntityFramework license 

            #region Migrate Database
            using (var dbContextToMigrate = new TestDbContext())
            {
                dbContextToMigrate.Database.Migrate();

                var connection = (NpgsqlConnection)dbContextToMigrate.Database.GetDbConnection();
                connection.Open();
                connection.ReloadTypes();
            }
            #endregion

            var testEntity1 = new TestEntity { TheEnum = Enum.TestEnum.Test2 };
            var testEntity2 = new TestEntity { TheEnum = Enum.TestEnum.Test3 };

            using var dbContext = new TestDbContext();
            dbContext.TestEntities.Add(testEntity1);
            dbContext.TestEntities.Add(testEntity2);

            dbContext.BulkSaveChanges();

            Console.ReadKey();

        }
    }
}
