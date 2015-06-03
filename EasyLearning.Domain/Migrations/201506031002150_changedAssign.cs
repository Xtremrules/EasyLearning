namespace EasyLearning.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedAssign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "ContentType", c => c.String());
            AddColumn("dbo.Assignments", "SaveName", c => c.String());
            AlterColumn("dbo.Assignments", "Score", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assignments", "Score", c => c.Int(nullable: false));
            DropColumn("dbo.Assignments", "SaveName");
            DropColumn("dbo.Assignments", "ContentType");
        }
    }
}
