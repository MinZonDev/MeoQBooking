namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotellllllssss : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Booking", "birthday", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Booking", "birthday", c => c.DateTime(nullable: false));
        }
    }
}
