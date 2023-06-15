namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotelllslsll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "paymentdate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Booking", "paymentdate");
        }
    }
}
