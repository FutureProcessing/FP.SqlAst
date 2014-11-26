namespace Tests {
    using System;
    using System.Collections.Generic;
    using FP.SqlAst;
    using FP.SqlAst.Generators;
    using NUnit.Framework;

    [TestFixture]
    public class PostgreSqlGeneratorTest {
        private PostgreSqlGenerator generator;
        private Dictionary<string, object> parameters;

        [SetUp]
        public void SetUp() {
            this.generator = new PostgreSqlGenerator();
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
            Assert.That(sql, Is.EqualTo("SELECT * FROM \"test\""));
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
            Assert.That(sql, Is.EqualTo("SELECT \"Id\" FROM \"test\""));
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
            Assert.That(sql, Is.EqualTo("SELECT \"Id\", \"Title\" FROM \"test\""));
        }

        [Test]
        public void ShouldGenerateSelectWithSubqueryAsFrom() {
            // arrange
            var subQuery = new SelectQuery();
            subQuery.From = new Table("test");
            subQuery.Columns.Add(new NamedColumn("Id"));
            subQuery.Columns.Add(new NamedColumn("Title"));
            subQuery.Alias = "x";

            var query = new SelectQuery();
            query.From = subQuery;
            query.Columns.Add(new NamedColumn("Id"));

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT \"Id\" FROM (SELECT \"Id\", \"Title\" FROM \"test\") as \"x\""));
        }

        [Test]
        public void ShouldGenerateSelectWithGroupBy() {
            // arrange
            var query = new SelectQuery() {
                Columns =
                {
                    new NamedColumn("CategoryId")
                },
                From = new Table("table"),
                GroupBy =
                {
                    new NamedColumn("CategoryId")
                }
            };

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT \"CategoryId\" FROM \"table\" GROUP BY \"CategoryId\""));
        }

        [Test]
        public void ShouldGenerateFunctionCall() {
            // arrange
            var query = new SelectQuery() {
                Columns =
                {
                    new FunctionCall()
                    {
                        FunctionName = "Avg",
                        Arguments =
                        {
                            new NamedColumn("Value")
                        }
                    }
                },
                From = new Table("table"),
            };

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT Avg(\"Value\") FROM \"table\""));
        }

        [Test]
        public void ShouldGenerateColumnWithAlias() {
            // arrange
            var query = new SelectQuery() {
                Columns =
                {
                    new AliasColumn
                    {
                        InnerColumn = new NamedColumn("Value"),
                        Alias = "MyName"
                    }

                },
                From = new Table("table"),
            };

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT (\"Value\") as \"MyName\" FROM \"table\""));
        }

        [Test]
        public void ShouldGenerateSelectWithJoin() {
            // arrange
            var query = new SelectQuery() {
                Columns =
                {
                   new NamedColumn("table2", "Id")

                },
                From = new Table("table1"),
                Joins =
                {
                    new Join
                    {
                        Type = "Left",
                        RecordSet = new Table("table2"),
                        Condition = new BinaryCondition()
                        {
                            Left = new ColumnReference("table1", "Id"),
                            Operator = "=",
                            Right = new ColumnReference("table2","FkId1")
                        }
                    }
                }
            };

            // act
            var sql = this.Generate(query);

            // assert
            Assert.That(sql, Is.EqualTo("SELECT \"table2\".\"Id\" FROM \"table1\" LEFT JOIN \"table2\" ON \"table1\".\"Id\" = \"table2\".\"FkId1\""));
        }

        private string Generate(SelectQuery query) {
            var sql = this.generator.Generate(query, this.parameters);

            Console.WriteLine("Generated SQL:\n{0}", sql);

            return sql;
        }
    }
}