using System;
using NUnit.Framework;

namespace SimpleJSON.Test
{
    [TestFixture]
    public class TestParse
    {
        public static string JsonStringEqualsInsteadOfColon = @"
        {
            ""integer"": {
                ""negative"" = 1
        }}";
        public static string JsonStringMissingComma = @"
        {
            ""integer"": {
                ""negative"": 1,
                ""positive"": 1
        }}";
        public static string JsonStringMissingClosingBracket = @"
        {
            ""integer"": {
                ""negative"": 1,
                ""positive"": 1
            },
            ""floating"": 1.0";
        public static string JsonStringMissingOpeningBracket = @"
        
            ""integer"": {
                ""negative"": 1,
                ""positive"": 1
        }}";
        public static string JsonObjectStringWithAllTypes = @"
        {
            ""string"": {
                ""normal"": ""this is a string"",
                ""special"": "":,[]{}\""\\\t\n\r\b\u0041\f\m\/""
            },
            ""integer"": {
                ""positive"": 1,
                ""explicitPositive"": +1,
                ""negative"" : -1
            },
            ""floating"": {
                ""positive"": 3.14,
                ""explicitPositive"": +3.14,
                ""negative"": -3.14
            },
            ""double"": {
                ""positive"": 3.14159265359,
                ""explicitPositive"": +3.14159265359,
                ""negative"": -3.14159265359
            },
            ""exponential"": {
                ""positive"": 3E4,
                ""explicitPositive"": 3E+4,
                ""negative"": 3E-4
            },
            ""boolean"": {
                ""positive"": true,
                ""negative"": false
            },
            ""array"": {
                ""empty"": [],
                ""populated"": [1, 1.0, null, ""string"", false, {}]
            },
            ""null"": null
        }";
        public static string JsonObjectStringWithAllNull = @"
        {
            ""string"": null,
            ""integer"": null,
            ""floating"": null,
            ""exponential"": null,
            ""boolean"": null,
            ""emptyArray"": null,
            ""null"": null
        }";

        [Test]
        public void Parse_EqualsInsteadOfColon_ThrowsException()
        {
            // assert
            Assert.Throws<Exception>(() => JSON.Parse(JsonStringEqualsInsteadOfColon));
        }

        [Test]
        public void Parse_MissingClosingBracket_ThrowsException()
        {
            // assert
            Assert.Throws<Exception>(() => JSON.Parse(JsonStringMissingClosingBracket));
        }

        [Test]
        public void Parse_MissingOpeningBracket_ThrowsException()
        {
            // assert
            Assert.Throws<Exception>(() => JSON.Parse(JsonStringMissingOpeningBracket));
        }

        [Test]
        public void Parse_MissingComma_ThrowsException()
        {
            // assert
            Assert.Throws<Exception>(() => JSON.Parse(JsonStringMissingComma));
        }

        [Test]
        public void Parse_EmptyJsonObject_NotNull()
        {
            // act
            var node = JSON.Parse("{}");

            // assert
            Assert.IsNotNull(node);
        }

        [Test]
        public void Parse_EmptyString_ReturnsNull()
        {
            // act
            var node = JSON.Parse("");

            // assert
            Assert.IsNull(node);
        }

        // ... (remaining tests unchanged)

        [Test]
        public void Parse_ObjectWithIdenticalItems_Overwrite()
        {
            // arrange
            const string jsonString = @"
            {
                ""value"": ""first"",
                ""value"": ""second""
            }";

            // act
            var node = JSON.Parse(jsonString);

            // assert
            Assert.AreEqual("second", node["value"].Value);
        }
    }
}
