using System.Linq.Expressions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using NSubstitute;

namespace LawFirmHelper.Tests;

public class LeadServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenFirmDoesNoExist()
    {
        var firmRepo = Substitute.For<IFirmRepository>();
        var leadRepo = Substitute.For<ILeadRepository>();
        
        var service = new LeadService(leadRepo, firmRepo);

        var request = new CreateLeadRequest
        {
            FirmId = Guid.NewGuid()
        };
        var cancellationToken = CancellationToken.None;
        
        firmRepo.GetFirmWithLeadsAsync(request.FirmId, cancellationToken)
            .Returns((Firm)null!);
        
        // act
        var result = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request, cancellationToken));
        
        // arrange
        Assert.Contains(request.FirmId.ToString(), result.Message);
        
        await leadRepo.DidNotReceiveWithAnyArgs().GetCountAsync(default, default);
        await leadRepo.DidNotReceiveWithAnyArgs().AddAsync(default!);
    }
}