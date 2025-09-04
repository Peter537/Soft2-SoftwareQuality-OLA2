using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Tests;

[TestClass]
public class TaskRepositoryTests
{
    private InMemoryTaskRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _repository = new InMemoryTaskRepository();
    }

    [TestMethod]
    public async System.Threading.Tasks.Task Can_Create_Task()
    {
        var task = new TaskItem { Title = "Test", Description = "Desc" };
        var created = await _repository.CreateAsync(task);
        Assert.AreNotEqual(0, created.Id);
        Assert.AreEqual("Test", created.Title);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task Can_Get_All_Tasks()
    {
        await _repository.CreateAsync(new TaskItem { Title = "T1" });
        await _repository.CreateAsync(new TaskItem { Title = "T2" });
        var tasks = await _repository.GetAllAsync();
        Assert.AreEqual(2, tasks.Count());
    }

    [TestMethod]
    public async System.Threading.Tasks.Task Can_Update_Task()
    {
        var task = await _repository.CreateAsync(new TaskItem { Title = "Old" });
        var updated = new TaskItem { Title = "New", Description = "Updated" };
        var result = await _repository.UpdateAsync(task.Id, updated);
        Assert.IsNotNull(result);
        Assert.AreEqual("New", result.Title);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task Can_Delete_Task()
    {
        var task = await _repository.CreateAsync(new TaskItem { Title = "Delete" });
        var deleted = await _repository.DeleteAsync(task.Id);
        Assert.IsTrue(deleted);
        var get = await _repository.GetByIdAsync(task.Id);
        Assert.IsNull(get);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task Update_Nonexistent_Returns_Null()
    {
        var result = await _repository.UpdateAsync(999, new TaskItem());
        Assert.IsNull(result);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task Delete_Nonexistent_Returns_False()
    {
        var deleted = await _repository.DeleteAsync(999);
        Assert.IsFalse(deleted);
    }

    // Additional edge cases for coverage
    [TestMethod]
    public async System.Threading.Tasks.Task Create_With_Empty_Title()
    {
        var task = new TaskItem { Title = "", Description = "Desc" };
        var created = await _repository.CreateAsync(task); // No validation in repo, but in controller
        Assert.AreNotEqual(0, created.Id);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task Get_By_Invalid_Id_Returns_Null()
    {
        var task = await _repository.GetByIdAsync(-1);
        Assert.IsNull(task);
    }
}