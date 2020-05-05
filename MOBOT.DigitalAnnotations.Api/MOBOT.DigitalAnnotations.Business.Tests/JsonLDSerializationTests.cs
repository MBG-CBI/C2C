using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOBOT.DigitalAnnotations.Business.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MOBOT.DigitalAnnotations.Business.Tests
{
    [TestClass]
    public class JsonLDSerializationTests
    {
        private JsonSerializerSettings _jSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _jSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
            };
        }

        [TestMethod]
        public void DigitalAnnotation_CanSerializeAsJsonLD()
        {
            // Arrange: Given expected result
            var expected = @"{
                ""@context"": ""http://www.w3.org/ns/anno.jsonld"",
                ""id"": ""http://MyTestUri.com"",
                ""type"": ""Annotation"",
                ""target"": {
                        ""id"": ""http://MyImageSource.com#xywh=1,2,100,200"",
                        ""type"": ""image"",
                        ""format"": ""image/jpeg""
                    },
                ""body"": {
                    ""type"": ""TextualBody"",
                    ""value"": ""My Test Value"",
                    ""format"": ""text/plain""
                }
            }";

            // Arrange: Given a valid Digital Annotation Object
            var annotation = new WebAnnotation
            {
                Body = new TextualBody
                {
                    Value = "My Test Value"
                },
                Id = "http://MyTestUri.com",
                Target = new WebAnnotationTarget
                {
                    Source = "http://MyImageSource.com",
                    CoordinateX = 1,
                    CoordinateY = 2,
                    Width = 100,
                    Height = 200
                }
            };

            // Act: When serializing...
            var actual = JsonConvert.SerializeObject(annotation, _jSettings);
            var x = expected.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", String.Empty).Replace(" ", string.Empty);

            // Assert: the serialized string should meet json+ld specifications
            Assert.AreEqual(x, actual.Replace(" ", string.Empty));
        }
    }
}
