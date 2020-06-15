using FluentMigrator;

namespace Migrations.Profiles.InitialData
{
    [Profile("InitialData")]
    public class CreateDevSeedData : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Log").Row(new
            {
                Text = "test text",
            });
        }

        public override void Down()
        {
        }
    }
}