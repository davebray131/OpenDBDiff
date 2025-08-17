using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Compare;
using OpenDBDiff.SqlServer.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Options;

namespace OpenDBDiff.Tests.Model.Tests
{
    [TestClass()]
    public class ColumnsTests
    {
        [TestMethod()]
        public void OriginHasExtraColumn_NothingSelected_ShouldDropExtraColumn()
        {
            var idStorage = 1;
            var getId = new Func<int>(() => ++idStorage);

            var originDatabase = new Database
            {
                Info = new DatabaseInfo()
                {
                    Collation = "SQL_Latin1_General_CP1_CI_AS",
                },
                Options = new SqlOption(),
                Id = getId()
            };
            var originTable = new Table(originDatabase)
            {
                Name = "Example",
                Id = getId()
            };
            var originColumn1 = new Column(originTable)
            {
                Name = "Test",
                Type = "int",
                Id = getId()
            };
            var originColumn2 = new Column(originTable)
            {
                Name = "Test2",
                Type = "varchar(20)",
                Id = getId()
            };
            var originColumn3 = new Column(originTable)
            {
                Name = "Test3",
                Type = "bigint",
                Id = getId()
            };
            originTable.Columns.Add(originColumn1);
            originTable.Columns.Add(originColumn3);
            originTable.Columns.Add(originColumn2);
            originDatabase.Tables.Add(originTable);


            var destinationDatabase = new Database
            {
                Info = new DatabaseInfo()
                {
                    Collation = "SQL_Latin1_General_CP1_CI_AS"
                },
                Id = getId(),
                Options = new SqlOption()
            };
            var destinationTable = new Table(destinationDatabase)
            {
                Name = "Example",
                Id = getId()
            };
            var destinationColumn1 = new Column(destinationTable)
            {
                Name = "Test",
                Type = "int",
                Id = getId()
            };
            var destinationColumn3 = new Column(destinationTable)
            {
                Name = "Test3",
                Type = "bigint",
                Id = getId()
            };
            destinationTable.Columns.Add(destinationColumn1);
            destinationTable.Columns.Add(destinationColumn3);
            destinationDatabase.Tables.Add(destinationTable);


            originTable.OriginalTable = (Table)originTable.Clone((Database)originTable.Parent);
            new CompareColumns().GenerateDifferences<Table>(originTable.Columns, destinationTable.Columns);

            var sqlList = originTable.ToSqlDiff(new List<ISchemaBase>());
            var sql = sqlList.ToSQL();
            Assert.AreEqual(originColumn2.ToSqlDrop(), sql);
        }
        [TestMethod()]
        public void OriginHasExtraColumn_NotChangedColumnSelected_ShouldBeEmptyScript()
        {
            var idStorage = 1;
            var getId = new Func<int>(() => ++idStorage);
            var originDatabase = new Database
            {
                Info = new DatabaseInfo()
                {
                    Collation = "SQL_Latin1_General_CP1_CI_AS"
                },
                Id = getId(),
                Options = new SqlOption()
            };
            var originTable = new Table(originDatabase)
            {
                Name = "Example",
                Id = getId()
            };
            var originColumn1 = new Column(originTable)
            {
                Name = "Test",
                Type = "int",
                Id = getId()
            };
            var originColumn2 = new Column(originTable)
            {
                Name = "Test2",
                Type = "varchar(20)",
                Id = getId()
            };
            var originColumn3 = new Column(originTable)
            {
                Name = "Test3",
                Type = "bigint",
                Id = getId()
            };
            originTable.Columns.Add(originColumn1);
            originTable.Columns.Add(originColumn3);
            originTable.Columns.Add(originColumn2);
            originDatabase.Tables.Add(originTable);


            var destinationDatabase = new Database
            {
                Info = new DatabaseInfo()
                {
                    Collation = "SQL_Latin1_General_CP1_CI_AS"
                },
                Id = getId(),
                Options = new SqlOption()
            };
            var destinationTable = new Table(destinationDatabase)
            {
                Name = "Example",
                Id = getId()
            };
            var destinationColumn1 = new Column(destinationTable)
            {
                Name = "Test",
                Type = "int",
                Id = getId()
            };
            var destinationColumn3 = new Column(destinationTable)
            {
                Name = "Test3",
                Type = "bigint",
                Id = getId()
            };
            destinationTable.Columns.Add(destinationColumn1);
            destinationTable.Columns.Add(destinationColumn3);
            destinationDatabase.Tables.Add(destinationTable);


            originTable.OriginalTable = (Table)originTable.Clone((Database)originTable.Parent);
            new CompareColumns().GenerateDifferences<Table>(originTable.Columns, destinationTable.Columns);

            var sqlList = originTable.ToSqlDiff(new List<ISchemaBase>() { originColumn3 });
            var sql = sqlList.ToSQL();
            Assert.AreEqual("", sql);
        }
        [TestMethod()]
        public void OriginHasExtraColumn_ExtraColumnSelected_ShouldBeDropColumnScript()
        {
            var idStorage = 1;
            var getId = new Func<int>(() => ++idStorage);
            var originDatabase = new Database
            {
                Info = new DatabaseInfo()
                {
                    Collation = "SQL_Latin1_General_CP1_CI_AS"
                },
                Id = getId(),
                Options = new SqlOption()
            };
            var originTable = new Table(originDatabase)
            {
                Name = "Example",
                Id = getId()
            };
            var originColumn1 = new Column(originTable)
            {
                Name = "Test",
                Type = "int",
                Id = getId()
            };
            var originColumn2 = new Column(originTable)
            {
                Name = "Test2",
                Type = "varchar(20)",
                Id = getId()
            };
            var originColumn3 = new Column(originTable)
            {
                Name = "Test3",
                Type = "bigint",
                Id = getId()
            };
            originTable.Columns.Add(originColumn1);
            originTable.Columns.Add(originColumn3);
            originTable.Columns.Add(originColumn2);
            originDatabase.Tables.Add(originTable);


            var destinationDatabase = new Database
            {
                Info = new DatabaseInfo()
                {
                    Collation = "SQL_Latin1_General_CP1_CI_AS"
                },
                Id = getId(),
                Options = new SqlOption()
            };
            var destinationTable = new Table(destinationDatabase)
            {
                Name = "Example",
                Id = getId()
            };
            var destinationColumn1 = new Column(destinationTable)
            {
                Name = "Test",
                Type = "int",
                Id = getId()
            };
            var destinationColumn3 = new Column(destinationTable)
            {
                Name = "Test3",
                Type = "bigint",
                Id = getId()
            };
            destinationTable.Columns.Add(destinationColumn1);
            destinationTable.Columns.Add(destinationColumn3);
            destinationDatabase.Tables.Add(destinationTable);


            originTable.OriginalTable = (Table)originTable.Clone((Database)originTable.Parent);
            new CompareColumns().GenerateDifferences<Table>(originTable.Columns, destinationTable.Columns);

            var sqlList = originTable.ToSqlDiff(new List<ISchemaBase>() { originColumn2 });
            var sql = sqlList.ToSQL();
            Assert.AreEqual(originColumn2.ToSqlDrop(), sql);
        }
    }
}
