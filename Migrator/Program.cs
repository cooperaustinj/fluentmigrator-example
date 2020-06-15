using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Migrations;

namespace Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var profile = "";
            if (args.Length == 1)
            {
                profile = args[0];
            }
            else if (args.Length > 1)
            {
                throw new ArgumentException("Too many arguments");
            }
            var serviceProvider = CreateServices(profile);

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        private static IServiceProvider CreateServices(string profile)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .Configure<RunnerOptions>(cfg => cfg.Profile = profile)
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddSqlServer()
                    // Set the connection string
                    .WithGlobalConnectionString("Server=localhost; Database=Application; User Id=SA; Password=securePassword123!")
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CreateLogTable).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}
