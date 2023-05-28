namespace WebBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Hotels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Booking",
                c => new
                    {
                        bookingid = c.Int(nullable: false, identity: true),
                        roomid = c.Int(nullable: false),
                        userid = c.String(maxLength: 128),
                        customername = c.String(maxLength: 500),
                        email = c.String(maxLength: 250),
                        phone = c.String(maxLength: 10),
                        identiyid = c.String(maxLength: 12),
                        birthday = c.DateTime(),
                        bookingdate = c.DateTime(),
                        numberpeople = c.Int(),
                        checkin = c.DateTime(),
                        checkout = c.DateTime(),
                        total = c.Decimal(storeType: "money"),
                        statusid = c.Int(),
                    })
                .PrimaryKey(t => t.bookingid)
                .ForeignKey("dbo.Room", t => t.roomid, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.statusid)
                .Index(t => t.roomid)
                .Index(t => t.statusid);
            
            CreateTable(
                "dbo.Bill",
                c => new
                    {
                        billid = c.Int(nullable: false, identity: true),
                        bookingid = c.Int(nullable: false),
                        date = c.DateTime(),
                        serviceid = c.Int(),
                        number = c.Int(),
                        total = c.Decimal(storeType: "money"),
                        statuscreditid = c.Int(),
                        creditid = c.Int(),
                        totalprice = c.Decimal(storeType: "money"),
                    })
                .PrimaryKey(t => t.billid)
                .ForeignKey("dbo.Booking", t => t.bookingid, cascadeDelete: true)
                .ForeignKey("dbo.Credit", t => t.creditid)
                .ForeignKey("dbo.Servive", t => t.serviceid)
                .ForeignKey("dbo.StatusCredit", t => t.statuscreditid)
                .Index(t => t.bookingid)
                .Index(t => t.serviceid)
                .Index(t => t.statuscreditid)
                .Index(t => t.creditid);
            
            CreateTable(
                "dbo.Credit",
                c => new
                    {
                        creditid = c.Int(nullable: false),
                        creditname = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.creditid);
            
            CreateTable(
                "dbo.Servive",
                c => new
                    {
                        serviceid = c.Int(nullable: false),
                        categoryid = c.Int(),
                        servicename = c.String(maxLength: 250),
                        price = c.Decimal(storeType: "money"),
                    })
                .PrimaryKey(t => t.serviceid);
            
            CreateTable(
                "dbo.StatusCredit",
                c => new
                    {
                        statuscreditid = c.Int(nullable: false, identity: true),
                        statuscreditname = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.statuscreditid);
            
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        roomid = c.Int(nullable: false, identity: true),
                        categoryid = c.Int(),
                        roomname = c.String(maxLength: 150),
                        roomdes = c.String(),
                        price = c.Decimal(storeType: "money"),
                        promotion = c.Decimal(storeType: "money"),
                        image = c.String(maxLength: 500),
                        image1 = c.String(),
                        image2 = c.String(),
                        image3 = c.String(),
                        image4 = c.String(),
                        image5 = c.String(),
                        image6 = c.String(),
                        sizeRoom = c.Int(nullable: false),
                        maxpeople = c.Int(),
                        view = c.String(),
                        bed = c.Int(),
                    })
                .PrimaryKey(t => t.roomid)
                .ForeignKey("dbo.Category", t => t.categoryid)
                .Index(t => t.categoryid);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        categoryid = c.Int(nullable: false, identity: true),
                        categoryname = c.String(maxLength: 150),
                        categorydes = c.String(),
                        image = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.categoryid);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        statusid = c.Int(nullable: false),
                        statusname = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.statusid);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Booking", "statusid", "dbo.Status");
            DropForeignKey("dbo.Room", "categoryid", "dbo.Category");
            DropForeignKey("dbo.Booking", "roomid", "dbo.Room");
            DropForeignKey("dbo.Bill", "statuscreditid", "dbo.StatusCredit");
            DropForeignKey("dbo.Bill", "serviceid", "dbo.Servive");
            DropForeignKey("dbo.Bill", "creditid", "dbo.Credit");
            DropForeignKey("dbo.Bill", "bookingid", "dbo.Booking");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Room", new[] { "categoryid" });
            DropIndex("dbo.Bill", new[] { "creditid" });
            DropIndex("dbo.Bill", new[] { "statuscreditid" });
            DropIndex("dbo.Bill", new[] { "serviceid" });
            DropIndex("dbo.Bill", new[] { "bookingid" });
            DropIndex("dbo.Booking", new[] { "statusid" });
            DropIndex("dbo.Booking", new[] { "roomid" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Status");
            DropTable("dbo.Category");
            DropTable("dbo.Room");
            DropTable("dbo.StatusCredit");
            DropTable("dbo.Servive");
            DropTable("dbo.Credit");
            DropTable("dbo.Bill");
            DropTable("dbo.Booking");
        }
    }
}
