using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Json;
using TodoApi.Models;

namespace TodoApi.Tests;

[TestClass]
public class TasksApiTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task Integration_Create_Task()
    {
        var task = new TaskItem { Title = "CreateTest", Description = "Desc" };
        var response = await _client.PostAsJsonAsync("/tasks", task);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.IsNotNull(created);
        Assert.AreEqual("CreateTest", created!.Title);
    }

    [TestMethod]
    public async Task Integration_Create_Task_Title_Is_Null()
    {
        var task = new TaskItem { Description = "Desc" };
        var response = await _client.PostAsJsonAsync("/tasks", task);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Integration_Get_All_Tasks()
    {
        await _client.PostAsJsonAsync("/tasks", new TaskItem { Title = "GetTest" });
        var response = await _client.GetAsync("/tasks");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var tasks = await response.Content.ReadFromJsonAsync<IEnumerable<TaskItem>>();
        Assert.IsNotNull(tasks);
        Assert.IsTrue(tasks!.Any());
    }

    [TestMethod]
    public async Task Integration_Get_Task_By_Id()
    {
        var createResp = await _client.PostAsJsonAsync("/tasks", new TaskItem { Title = "GetByIdTest" });
        var created = await createResp.Content.ReadFromJsonAsync<TaskItem>();
        var response = await _client.GetAsync($"/tasks/{created!.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var task = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.IsNotNull(task);
        Assert.AreEqual("GetByIdTest", task!.Title);
    }

    [TestMethod]
    public async Task Integration_Get_Task_By_Id_Id_Is_Zero()
    {
        var createResp = await _client.PostAsJsonAsync("/tasks", new TaskItem { Title = "GetByIdTest" });
        await createResp.Content.ReadFromJsonAsync<TaskItem>();
        var response = await _client.GetAsync($"/tasks/0");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Integration_Update_Task()
    {
        var createResp = await _client.PostAsJsonAsync("/tasks", new TaskItem { Title = "UpdateOld" });
        var created = await createResp.Content.ReadFromJsonAsync<TaskItem>();
        var updated = new TaskItem { Title = "UpdateNew", Description = "NewDesc", Status = TodoApi.Models.TaskStatus.IN_PROGRESS };
        var response = await _client.PutAsJsonAsync($"/tasks/{created!.Id}", updated);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<TaskItem>();
        Assert.AreEqual("UpdateNew", result!.Title);
    }

    [TestMethod]
    public async Task Integration_Update_Task_Id_Is_Zero()
    {
        var createResp = await _client.PostAsJsonAsync("/tasks", new TaskItem { Title = "UpdateOld" });
        await createResp.Content.ReadFromJsonAsync<TaskItem>();
        var updated = new TaskItem { Title = "UpdateNew", Description = "NewDesc", Status = TodoApi.Models.TaskStatus.IN_PROGRESS };
        var response = await _client.PutAsJsonAsync($"/tasks/0", updated);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Integration_Delete_Task()
    {
        var createResp = await _client.PostAsJsonAsync("/tasks", new TaskItem { Title = "DeleteTest" });
        var created = await createResp.Content.ReadFromJsonAsync<TaskItem>();
        var response = await _client.DeleteAsync($"/tasks/{created!.Id}");
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task Integration_Delete_Task_Id_Is_Zero()
    {
        var createResp = await _client.PostAsJsonAsync("/tasks", new TaskItem { Title = "DeleteTest" });
        await createResp.Content.ReadFromJsonAsync<TaskItem>();
        var response = await _client.DeleteAsync($"/tasks/0");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}