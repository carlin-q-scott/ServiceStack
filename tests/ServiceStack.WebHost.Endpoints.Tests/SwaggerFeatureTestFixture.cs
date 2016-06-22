using System;
using System.Collections.Generic;
using NUnit.Framework;
using ServiceStack.Api.Swagger;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;

namespace ServiceStack.WebHost.Endpoints.Tests
{
    [TestFixture]
    public abstract class SwaggerFeatureTestFixture
    {
        protected const string BaseUrl = "http://localhost:8024";
        protected const string ListeningOn = BaseUrl + "/";

        public class SwaggerFeatureAppHostHttpListener
            : AppHostHttpListenerBase
        {

            public SwaggerFeatureAppHostHttpListener()
                : base("Swagger Feature Tests", typeof(SwaggerFeatureServiceTests).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                Plugins.Add(new SwaggerFeature());

                SetConfig(new EndpointHostConfig
                {
                    DebugMode = true //Show StackTraces for easier debugging
                });
            }
        }

        protected SwaggerFeatureAppHostHttpListener appHost;

        [TestFixtureSetUp]
        public void OnTestFixtureSetUp()
        {
            appHost = new SwaggerFeatureAppHostHttpListener();
            appHost.Init();
            appHost.Start(ListeningOn);
        }

        [TestFixtureTearDown]
        public void OnTestFixtureTearDown()
        {
            appHost.Dispose();
        }
    }

    [ServiceHost.Api("Service Description")]
    [Route("/swagger/{Name}", "GET", Summary = @"GET Summary", Notes = "GET Notes")]
    [Route("/swagger/{Name}", "POST", Summary = @"POST Summary", Notes = "POST Notes")]
    public class SwaggerFeatureRequest
    {
        [ApiMember(Name = "Name", Description = "Name Description",
            ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swaggerGetList/{Name}", "GET")]
    public class SwaggerGetListRequest : IReturn<List<SwaggerFeatureResponse>>
    {
        public string Name { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swaggerGetArray/{Name}", "GET")]
    public class SwaggerGetArrayRequest : IReturn<SwaggerFeatureResponse[]>
    {
        public string Name { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swaggerModels/{UrlParam}", "POST")]
    public class SwaggerModelsRequest : IReturn<SwaggerFeatureResponse>
    {
        [ApiMember(Name = "UrlParam", Description = "URL parameter",
            ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string UrlParam { get; set; }

        [ApiMember(Name = "RequestBody", Description = "The request body",
            ParameterType = "body", DataType = "SwaggerModelsRequest", IsRequired = true)]
        [System.ComponentModel.Description("Name description")]
        public string Name { get; set; }

        [System.ComponentModel.Description("NestedModel description")]
        public SwaggerNestedModel NestedModel { get; set; }

        public List<SwaggerNestedModel2> ListProperty { get; set; }

        public SwaggerNestedModel3[] ArrayProperty { get; set; }

        public byte ByteProperty { get; set; }

        public long LongProperty { get; set; }

        public float FloatProperty { get; set; }

        public double DoubleProperty { get; set; }

        public decimal DecimalProperty { get; set; }

        public DateTime DateProperty { get; set; }
    }

    public class SwaggerNestedModel
    {
        [System.ComponentModel.Description("NestedProperty description")]
        public bool NestedProperty { get; set; }
    }

    public class SwaggerNestedModel2
    {
        [System.ComponentModel.Description("NestedProperty2 description")]
        public bool NestedProperty2 { get; set; }
    }

    public class SwaggerNestedModel3
    {
        [System.ComponentModel.Description("NestedProperty3 description")]
        public bool NestedProperty3 { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swagger2/NameIsNotSetRequest", "GET")]
    public class NameIsNotSetRequest
    {
        [ApiMember]
        public string Name { get; set; }
    }


    [ServiceHost.Api("test")]
    [Route("/swg3/conference/count", "GET")]
    public class MultipleTestRequest : IReturn<int>
    {
        [ApiMember]
        public string Name { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swg3/conference/{Name}/conferences", "POST")]
    [Route("/swgb3/conference/{Name}/conferences", "POST")]
    public class MultipleTest2Request : IReturn<object>
    {
        [ApiMember]
        public string Name { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swg3/conference/{Name}/conferences", "DELETE")]
    public class MultipleTest3Request : IReturn<object>
    {
        [ApiMember]
        public string Name { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swg3/conference", "GET")]
    public class MultipleTest4Request : IReturn<object>
    {
        [ApiMember]
        public string Name { get; set; }
    }

    public class NullableResponse
    {
        [System.ComponentModel.Description("NestedProperty2 description")]
        public bool NestedProperty2 { get; set; }

        public int? Optional { get; set; }
    }

    [ServiceHost.Api]
    [Route("/swgnull/", "GET")]
    public class NullableInRequest : IReturn<NullableResponse>
    {
        [ApiMember]
        public int? Position { get; set; }
    }

    public class NullableService : ServiceInterface.Service
    {
        public object Get(NullableInRequest request)
        {
            return null;
        }
    }


    public class SwaggerFeatureResponse
    {
        public bool IsSuccess { get; set; }
    }

    public class MultipleTestRequestService : ServiceInterface.Service
    {
        public object Get(MultipleTestRequest request)
        {
            return null;
        }

        public object Post(MultipleTest2Request request)
        {
            return null;
        }

        public object Delete(MultipleTest3Request request)
        {
            return null;
        }
    }
    public class MultipleTest2RequestService : ServiceInterface.Service
    {
        public object Get(MultipleTest4Request request)
        {
            return null;
        }
    }


    public class SwaggerFeatureService : ServiceInterface.Service
    {
        public object Get(SwaggerFeatureRequest request)
        {
            return new SwaggerFeatureResponse { IsSuccess = true };
        }

        public object Post(SwaggerFeatureRequest request)
        {
            return new SwaggerFeatureResponse { IsSuccess = true };
        }

        public object Get(NameIsNotSetRequest request)
        {
            return 0;
        }

        public object Post(SwaggerModelsRequest request)
        {
            return new SwaggerFeatureResponse { IsSuccess = true };
        }

        public object Get(SwaggerGetListRequest request)
        {
            return new List<SwaggerFeatureResponse> { new SwaggerFeatureResponse { IsSuccess = true } };
        }

        public object Get(SwaggerGetArrayRequest request)
        {
            return new[] { new SwaggerFeatureResponse { IsSuccess = true } };
        }
    }
}
