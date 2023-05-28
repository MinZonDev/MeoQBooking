namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotellls1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Birthday", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Birthday");
        }
    }
}
