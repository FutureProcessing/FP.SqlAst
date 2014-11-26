namespace Tests {
    using System;
    using System.Collections.Generic;
    using FP.SqlAst;
    using FP.SqlAst.Generators;
    using NUnit.Framework;

    [TestFixture]
    public class MsSqlGeneratorTest {
        private MsSqlGenerator generator;
        private Dictionary<string, object> parameters;

        [SetUp]
        public void SetUp() {
            this.generator = new MsSqlGenerator();
            this.parameters = new Dictionary<string, object>();
        }

        [Test]
        public void ShouldGenerateForSimpleSelectWithAllColumns() {
            // arrange
            var query = new SelectQuery();
            query.From = new Table("test");
            query.Columns.Add(new AllColumns());

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT * FROM test"));
        }

        [Test]
        public void ShouldGenerateSelectWithOneColumn() {
            // arrange
            var query = new SelectQuery();
            query.From = new Table("test");
            query.Columns.Add(new NamedColumn("Id"));

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT [Id] FROM test"));
        }

        [Test]
        public void ShouldGenerateSelectWithTwoColumns() {
            // arrange
            var query = new SelectQuery();
            query.From = new Table("test");
            query.Columns.Add(new NamedColumn("Id"));
            query.Columns.Add(new NamedColumn("Title"));

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT [Id], [Title] FROM test"));
        }

        [Test]
        public void ShouldGenerateSelectWithSubqueryAsFrom() {
            // arrange
            var subQuery = new SelectQuery();
            subQuery.From = new Table("test");
            subQuery.Columns.Add(new NamedColumn("Id"));
            subQuery.Columns.Add(new NamedColumn("Title"));

            var query = new SelectQuery();
            query.From = subQuery;
            query.Columns.Add(new NamedColumn("Id"));

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT [Id] FROM (SELECT [Id], [Title] FROM test) as x"));
        }

        [Test]
        public void ShouldGenerateSelectWithRowNumberColumn() {
            // arrange
            var query = new SelectQuery();
            query.From = new Table("test");
            query.Columns.Add(new RowNumberColumn("__RowNumber") {
                OrderBy = {
                    new NamedColumn("Id")
                }
            });

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT ROW_NUMBER() OVER (ORDER BY [Id]) AS __RowNumber FROM test"));
        }

        [Test]
        public void ShouldGenerateSelectWithRowNumberBetween() {
            // arrange
            var subQuery = new SelectQuery();
            subQuery.From = new Table("test");
            subQuery.Columns.Add(new RowNumberColumn("__RowNumber") {
                OrderBy = {
                    new NamedColumn("Id")
                }
            });
            subQuery.Columns.Add(new NamedColumn("Id"));
            subQuery.Columns.Add(new NamedColumn("Title"));

            var query = new SelectQuery();
            query.From = subQuery;
            query.Columns.Add(new NamedColumn("Id"));
            query.Columns.Add(new NamedColumn("Title"));
            query.Where = new RowNumberInRangeCondition("__RowNumber", 5, 10);

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT [Id], [Title] FROM (SELECT ROW_NUMBER() OVER (ORDER BY [Id]) AS __RowNumber, [Id], [Title] FROM test) as x WHERE [__RowNumber] BETWEEN @p0 AND @p1"));
            Assert.That(this.parameters["@p0"], Is.EqualTo(5));
            Assert.That(this.parameters["@p1"], Is.EqualTo(10));
        }

        [Test]
        public void ShouldGenerateSelectWithRowNumberGreaterOrEqualThan() {
            // arrange
            var subQuery = new SelectQuery();
            subQuery.From = new Table("test");
            subQuery.Columns.Add(new RowNumberColumn("__RowNumber") {
                OrderBy = {
                    new NamedColumn("Id")
                }
            });
            subQuery.Columns.Add(new NamedColumn("Id"));
            subQuery.Columns.Add(new NamedColumn("Title"));

            var query = new SelectQuery();
            query.From = subQuery;
            query.Columns.Add(new NamedColumn("Id"));
            query.Columns.Add(new NamedColumn("Title"));
            query.Where = new RowNumberInRangeCondition("__RowNumber", 5, null);

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT [Id], [Title] FROM (SELECT ROW_NUMBER() OVER (ORDER BY [Id]) AS __RowNumber, [Id], [Title] FROM test) as x WHERE [__RowNumber] >= @p0"));
            Assert.That(this.parameters["@p0"], Is.EqualTo(5));            
        }

        [Test]
        public void ShouldGenerateSelectWithRowNumberLessOrEqualThan() {
            // arrange
            var subQuery = new SelectQuery();
            subQuery.From = new Table("test");
            subQuery.Columns.Add(new RowNumberColumn("__RowNumber") {
                OrderBy = {
                    new NamedColumn("Id")
                }
            });
            subQuery.Columns.Add(new NamedColumn("Id"));
            subQuery.Columns.Add(new NamedColumn("Title"));

            var query = new SelectQuery();
            query.From = subQuery;
            query.Columns.Add(new NamedColumn("Id"));
            query.Columns.Add(new NamedColumn("Title"));
            query.Where = new RowNumberInRangeCondition("__RowNumber", null, 5);

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT [Id], [Title] FROM (SELECT ROW_NUMBER() OVER (ORDER BY [Id]) AS __RowNumber, [Id], [Title] FROM test) as x WHERE [__RowNumber] <= @p0"));
            Assert.That(this.parameters["@p0"], Is.EqualTo(5));
        }

        private string Generate(SelectQuery query) {
            var sql = this.generator.Generate(query, this.parameters);

            Console.WriteLine("Generated SQL:\n{0}", sql);

            return sql;
        }
    }
}