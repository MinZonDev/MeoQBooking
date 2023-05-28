namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hotelsss34 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Booking", "customername", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Booking", "email", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Booking", "phone", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Booking", "identiyid", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.Booking", "birthday", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Booking", "birthday", c => c.DateTime());
            AlterColumn("dbo.Booking", "identiyid", c => c.String(maxLength: 12));
            AlterColumn("dbo.Booking", "phone", c => c.String(maxLength: 10));
            AlterColumn("dbo.Booking", "email", c => c.String(maxLength: 250));
            AlterColumn("dbo.Booking", "customername", c => c.String(maxLength: 500));
        }
    }
}
