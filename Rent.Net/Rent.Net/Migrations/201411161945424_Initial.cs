namespace Rent.Net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        PayerId = c.String(nullable: false, maxLength: 128),
                        PayeeId = c.String(nullable: false, maxLength: 128),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.ApplicationUsers", t => t.PayeeId)
                .ForeignKey("dbo.ApplicationUsers", t => t.PayerId)
                .Index(t => t.PayerId)
                .Index(t => t.PayeeId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        Approved = c.Boolean(nullable: false),
                        RequestId = c.Int(),
                        PayeeId = c.String(nullable: false, maxLength: 128),
                        PayerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.ApplicationUsers", t => t.PayeeId)
                .ForeignKey("dbo.ApplicationUsers", t => t.PayerId)
                .ForeignKey("dbo.Requests", t => t.RequestId)
                .Index(t => t.RequestId)
                .Index(t => t.PayeeId)
                .Index(t => t.PayerId);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.Requests", "PayerId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Requests", "PayeeId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Payments", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.Payments", "PayerId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Payments", "PayeeId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Payments", new[] { "PayerId" });
            DropIndex("dbo.Payments", new[] { "PayeeId" });
            DropIndex("dbo.Payments", new[] { "RequestId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Requests", new[] { "PayeeId" });
            DropIndex("dbo.Requests", new[] { "PayerId" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.Payments");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.Requests");
        }
    }
}
