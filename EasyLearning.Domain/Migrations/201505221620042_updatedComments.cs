namespace EasyLearning.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedComments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "AppUserID", "dbo.Users");
            DropForeignKey("dbo.Replies", "AppUserID", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "AppUserID" });
            DropIndex("dbo.Replies", new[] { "AppUserID" });
            RenameColumn(table: "dbo.Comments", name: "AppUserID", newName: "AppUser_Id");
            RenameColumn(table: "dbo.Replies", name: "AppUserID", newName: "AppUser_Id");
            AlterColumn("dbo.Comments", "AppUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Replies", "AppUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "AppUser_Id");
            CreateIndex("dbo.Replies", "AppUser_Id");
            AddForeignKey("dbo.Comments", "AppUser_Id", "dbo.Users", "UserId");
            AddForeignKey("dbo.Replies", "AppUser_Id", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Replies", "AppUser_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "AppUser_Id", "dbo.Users");
            DropIndex("dbo.Replies", new[] { "AppUser_Id" });
            DropIndex("dbo.Comments", new[] { "AppUser_Id" });
            AlterColumn("dbo.Replies", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Comments", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.Replies", name: "AppUser_Id", newName: "AppUserID");
            RenameColumn(table: "dbo.Comments", name: "AppUser_Id", newName: "AppUserID");
            CreateIndex("dbo.Replies", "AppUserID");
            CreateIndex("dbo.Comments", "AppUserID");
            AddForeignKey("dbo.Replies", "AppUserID", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "AppUserID", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
