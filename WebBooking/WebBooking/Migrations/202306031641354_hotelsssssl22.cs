namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotelsssssl22 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Booking", "paymentdate");
            DropColumn("dbo.Booking", "TypePaymentVN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Booking", "TypePaymentVN", c => c.Int(nullable: false));
            AddColumn("dbo.Booking", "paymentdate", c => c.DateTime(nullable: false));
        }
    }
}
