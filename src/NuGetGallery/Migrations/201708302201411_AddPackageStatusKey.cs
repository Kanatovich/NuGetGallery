namespace NuGetGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPackageStatusKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packages", "PackageStatusKey", c => c.Int());

            Sql(@"
WHILE EXISTS (SELECT * FROM dbo.Packages WHERE Deleted = 1 AND PackageStatusKey IS NULL)
BEGIN
UPDATE TOP (1000) dbo.Packages
SET PackageStatusKey = 1
WHERE Deleted = 1 AND PackageStatusKey IS NULL
END
");

            Sql("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'nci_wi_Packages_PackageStatusKeyListed' AND object_id = OBJECT_ID('Packages')) CREATE INDEX [nci_wi_Packages_PackageStatusKeyListed] ON [dbo].[Packages] ([PackageStatusKey] ASC, [Listed] ASC) INCLUDE ([Description], [FlattenedDependencies], [IsPrerelease], [PackageRegistrationKey], [Tags], [Version]) WITH (ONLINE = ON)");

            Sql("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'nci_wi_Packages_IsListedPackageStatusKey' AND object_id = OBJECT_ID('Packages')) CREATE INDEX [nci_wi_Packages_IsListedPackageStatusKey] ON [dbo].[Packages] ([IsLatest] ASC, [PackageStatusKey] ASC) INCLUDE ([PackageRegistrationKey], [Tags], [Title]) WITH (ONLINE = ON)");

            Sql("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'nci_wi_Packages_IsLatestStablePackageStatusKey' AND object_id = OBJECT_ID('Packages')) CREATE INDEX [nci_wi_Packages_IsLatestStablePackageStatusKey] ON [dbo].[Packages] ([IsLatestStable] ASC, [PackageStatusKey] ASC) INCLUDE ([Description], [FlattenedAuthors], [Listed], [PackageRegistrationKey], [Published], [Tags]) WITH (ONLINE = ON)");

            Sql("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'nci_wi_Packages_SemVerLevelKeyIsLatestPackageStatusKey' AND object_id = OBJECT_ID('Packages')) CREATE INDEX [nci_wi_Packages_SemVerLevelKeyIsLatestPackageStatusKey] ON [dbo].[Packages] ([SemVerLevelKey] ASC, [IsLatest] ASC, [PackageStatusKey] ASC) INCLUDE ([PackageRegistrationKey]) WITH (ONLINE = ON)");

            Sql("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'nci_wi_Packages_SemVerLevelKeyPackageStatusKey' AND object_id = OBJECT_ID('Packages')) CREATE INDEX [nci_wi_Packages_SemVerLevelKeyPackageStatusKey] ON [dbo].[Packages] ([SemVerLevelKey] ASC, [PackageStatusKey] ASC) INCLUDE ([IsLatest], [IsLatestSemVer2]) WITH (ONLINE = ON)");

            Sql("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'nci_wi_Packages_PackageStatusKeyIsPrereleasePackageStatusKey' AND object_id = OBJECT_ID('Packages')) CREATE INDEX [nci_wi_Packages_PackageStatusKeyIsPrereleasePackageStatusKey] ON [dbo].[Packages] ([SemVerLevelKey] ASC, [IsPrerelease] ASC, [PackageStatusKey] ASC) INCLUDE ([PackageRegistrationKey], [Description], [Tags]) WITH (ONLINE = ON)");
        }
        
        public override void Down()
        {
            DropIndex(table: "Packages", name: "nci_wi_Packages_PackageStatusKeyListed");

            DropIndex(table: "Packages", name: "nci_wi_Packages_IsListedPackageStatusKey");

            DropIndex(table: "Packages", name: "nci_wi_Packages_IsLatestStablePackageStatusKey");

            DropIndex(table: "Packages", name: "nci_wi_Packages_SemVerLevelKeyIsLatestPackageStatusKey");

            DropIndex(table: "Packages", name: "nci_wi_Packages_SemVerLevelKeyPackageStatusKey");

            DropIndex(table: "Packages", name: "nci_wi_Packages_PackageStatusKeyIsPrereleasePackageStatusKey");

            DropColumn("dbo.Packages", "PackageStatusKey");
        }
    }
}
