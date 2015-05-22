namespace EasyLearning.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class download : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Studies", "VideoName", c => c.String());
            AddColumn("dbo.Studies", "VideoType", c => c.String());
            AddColumn("dbo.Studies", "NoteName", c => c.String());
            AddColumn("dbo.Studies", "NoteType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Studies", "NoteType");
            DropColumn("dbo.Studies", "NoteName");
            DropColumn("dbo.Studies", "VideoType");
            DropColumn("dbo.Studies", "VideoName");
        }
    }
}
