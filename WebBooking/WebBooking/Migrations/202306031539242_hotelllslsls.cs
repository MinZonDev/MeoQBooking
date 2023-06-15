namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotelllslsls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "TypePaymentVN", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Booking", "TypePaymentVN");
        }
    }
}
