namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbhotel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Booking", "statuscreditid", c => c.Int());
            CreateIndex("dbo.Booking", "statuscreditid");
            AddForeignKey("dbo.Booking", "statuscreditid", "dbo.StatusCredit", "statuscreditid");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Booking", "statuscreditid", "dbo.StatusCredit");
            DropIndex("dbo.Booking", new[] { "statuscreditid" });
            DropColumn("dbo.Booking", "statuscreditid");
        }
    }
}
