using System.Linq.Expressions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using NSubstitute;

namespace LawFirmHelper.Tests;

public class AgentServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentException_WhenFirmDoesNotExist()
    {
        // arrange
        var agentRepo = Substitute.For<IRepository<Agent>>();
        var firmRepo = Substitute.For<IFirmRepository>();
        
        var service = new AgentService(agentRepo, firmRepo);

        var request = new CreateAgentRequest
        {
            FirmId = Guid.NewGuid(),
            Name = "ChatDGDB"
        };

        firmRepo.AnyAsync(Arg.Any<Expression<Func<Firm, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(false);
        
        // act
        var expression = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));
        
        // assert 
        Assert.Contains(request.FirmId.ToString(), expression.Message);

        await agentRepo.DidNotReceiveWithAnyArgs().AddAsync(default!);
        await agentRepo.DidNotReceiveWithAnyArgs().SaveChangesAsync();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldCreateAgent_WhenFirmExists()
    {
        // arrange
        var agentRepo = Substitute.For<IRepository<Agent>>();
        var firmRepo = Substitute.For<IFirmRepository>();
        
        var service = new AgentService(agentRepo, firmRepo);
        
        var request = new CreateAgentRequest
        {
            FirmId = Guid.NewGuid(),
            Name = "ChatDGDB"
        };

        firmRepo.AnyAsync(Arg.Any<Expression<Func<Firm, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(true);
        
        // act
        var result = await service.CreateAsync(request);
        
        // assert
        Assert.NotNull(result);
        Assert.Equal(request.FirmId, result.FirmId);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(AgentStatus.Active, result.Status);
        
        Received.InOrder(() =>
        {
            firmRepo.AnyAsync(Arg.Any<Expression<Func<Firm, bool>>>(), Arg.Any<CancellationToken>());
            
            agentRepo.AddAsync(Arg.Is<Agent>(a => a.Name == request.Name && a.FirmId == request.FirmId && a.Status == AgentStatus.Active), 
                Arg.Any<CancellationToken>());
        });
    }

    [Fact]
    public async Task GetByFirmId_WhenNoAgentsFound()
    {
        // arrange
        var agentRepo = Substitute.For<IRepository<Agent>>();
        var firmRepo = Substitute.For<IFirmRepository>();
        var service = new AgentService(agentRepo, firmRepo);
        
        var targetId = Guid.NewGuid();

        agentRepo.AnyAsync(Arg.Any<Expression<Func<Agent, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(false);
        
        // act
        var result = await service.GetAllByFirmIdAsync(targetId);
        
        // assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByFirmId_WhenAgentsExist()
    {
        // arrange
        var agentRepo = Substitute.For<IRepository<Agent>>();
        var firmRepo = Substitute.For<IFirmRepository>();
        var service = new AgentService(agentRepo, firmRepo);
        
        var targetId = Guid.NewGuid();

        var fakeAgents = new List<Agent>
        {
            new Agent { Id = Guid.NewGuid(), Name = "agent47", FirmId = targetId, Status = AgentStatus.Active },
            new Agent { Id = Guid.NewGuid(), Name = "agent228", FirmId = targetId, Status = AgentStatus.Active}
        };

        agentRepo.GetWhereAsync(Arg.Any<Expression<Func<Agent, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(fakeAgents);
        
        // act 
        var result = await service.GetAllByFirmIdAsync(targetId);
        
        // assert
        Assert.NotNull(result);
        Assert.Equal(fakeAgents.Count, result.Count);
        
        var firstResponse = result.First(r => r.Id == fakeAgents[0].Id);
        Assert.Equal(fakeAgents[0].FirmId, firstResponse.FirmId);
        Assert.Equal(fakeAgents[0].Status, firstResponse.Status);
        Assert.Equal(fakeAgents[0].Name, firstResponse.Name);
    }
}