using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using static Abstracta.JmeterDsl.JmeterDsl;

namespace TodoApi.LoadTests
{
    [TestClass]
    public class JMeterDslTests
    {
        private const string BaseUrl = "http://localhost:5128";

        [TestMethod]
        public void Basic_Get_Tasks_Load()
        {
            var stats = TestPlan(
                ThreadGroup(10, 50,
                    HttpSampler($"{BaseUrl}/tasks")
                ),
                JtlWriter("jtls")
            ).Run();


            Assert.IsTrue(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(5),
                $"P99 too slow: {stats.Overall.SampleTimePercentile99}");
            Assert.IsTrue(stats.Overall.SamplesCount > 0);
        }

        [TestMethod]
        public void Mixed_100_Users_Crud()
        {
            var json = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);

            var stats = TestPlan(
                ThreadGroup(100, 5,
                    // GET
                    HttpSampler($"{BaseUrl}/tasks"),

                    // POST (create)
                    HttpSampler($"{BaseUrl}/tasks")
                        .Post("{\"id\":0,\"title\":\"lt\",\"description\":\"x\",\"status\":\"CREATED\"}", json),

                    // PUT (update id=1 as a simple representative)
                    HttpSampler($"{BaseUrl}/tasks/1")
                        .Method(HttpMethod.Put.Method)
                        .ContentType(json)
                        .Body("{\"id\":1,\"title\":\"lt-upd\",\"description\":\"x\",\"status\":\"IN_PROGRESS\"}"),

                    // DELETE (id=1)
                    HttpSampler($"{BaseUrl}/tasks/1")
                        .Method(HttpMethod.Delete.Method)
                ),
                JtlWriter("jtls-mixed")
            ).Run();

            Assert.IsTrue(stats.Overall.SampleTimePercentile99 < TimeSpan.FromSeconds(5),
                $"P99 too slow: {stats.Overall.SampleTimePercentile99}");
            Assert.IsTrue(stats.Overall.SamplesCount > 0);
        }
    }
}
