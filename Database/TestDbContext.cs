using EntityFrameworkExtensionTest.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace EntityFrameworkExtensionTest.Database
{
    class TestDbContext : DbContext
    {
        private static string _connectionString;

        static TestDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TestEnum>();
        }

        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                   builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug)
                       .AddProvider(new CustomDBLogger())
               );

                optionsBuilder
                    .UseLoggerFactory(loggerFactory)
                    .UseNpgsql(Program.ConnectionString, options =>
                        options.EnableRetryOnFailure());
                    //.UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresEnum<TestEnum>();
        }
    }
}
