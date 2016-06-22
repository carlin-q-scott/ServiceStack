using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using ServiceStack.Api.Swagger;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace ServiceStack.WebHost.Endpoints.Tests
{
    [TestFixture]
    public class SwaggerFeatureSchemaTests : SwaggerFeatureTestFixture
    {
        IRestClient client = new JsonServiceClient(ListeningOn);

        [Ignore("JSchema doesn't know how to resolve referenced schemas"), Test]
        public void ResourcesShouldMatchSchema()
        {
            var schemaUrl = "https://raw.githubusercontent.com/OAI/OpenAPI-Specification/master/schemas/v1.2/resourceListing.json#";
            var schema = JSchema.Parse(client.Get<string>(schemaUrl));

            var resources = client.Get<JObject>("/resources");

            Assert.That(resources.IsValid(schema), "/resources doesn't return valid json according to {0}", schemaUrl);
        }
    }
}
