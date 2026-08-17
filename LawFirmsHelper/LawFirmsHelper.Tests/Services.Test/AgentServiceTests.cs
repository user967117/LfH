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
}