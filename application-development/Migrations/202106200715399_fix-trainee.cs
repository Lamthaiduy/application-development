namespace application_development.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixtrainee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Programming", c => c.String());
            DropColumn("dbo.AspNetUsers", "Pogramming");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Pogramming", c => c.String());
            DropColumn("dbo.AspNetUsers", "Programming");
        }
    }
}
