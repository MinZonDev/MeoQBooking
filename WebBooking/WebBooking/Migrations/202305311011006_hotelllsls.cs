namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotelllsls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Booking", "birthday", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Booking", "numberpeople", c => c.Int(nullable: false));
            AlterColumn("dbo.Booking", "checkin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Booking", "checkout", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Booking", "checkout", c => c.DateTime());
            AlterColumn("dbo.Booking", "checkin", c => c.DateTime());
            AlterColumn("dbo.Booking", "numberpeople", c => c.Int());
            AlterColumn("dbo.Booking", "birthday", c => c.DateTime());
        }
    }
}
