namespace DBSoft.EPM.DAL.CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionTypeIndex : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.Transaction", name: "IX_Transaction_EveTransactionID", newName: "IX_EveTransactionID");
            CreateIndex("dbo.Transaction", "TransactionType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transaction", new[] { "TransactionType" });
            RenameIndex(table: "dbo.Transaction", name: "IX_EveTransactionID", newName: "IX_Transaction_EveTransactionID");
        }
    }
}
