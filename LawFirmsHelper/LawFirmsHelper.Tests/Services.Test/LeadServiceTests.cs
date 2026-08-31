using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using NSubstitute;

namespace LawFirmHelper.Tests;

public class LeadServiceTests
{
    private const string Name = "Test Lead";
    private const string Email = "testlead@gmail.com";
    private const string Description = "Test Lead Description";
    private const string Phone = "+35988888888";
    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenFirmDoesNoExist()
    {
        //arrange
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
        
        // assert
        Assert.Contains(request.FirmId.ToString(), result.Message);
        
        await leadRepo.DidNotReceiveWithAnyArgs().GetCountAsync(default, default);
        await leadRepo.DidNotReceiveWithAnyArgs().AddAsync(default!);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenLeadsLimitExceeded()
    {
        // arrange
        var firmRepo = Substitute.For<IFirmRepository>();
        var leadRepo = Substitute.For<ILeadRepository>();

        var service = new LeadService(leadRepo, firmRepo);

        var request = new CreateLeadRequest
        {
            FirmId = Guid.NewGuid()
        };
        var cancellationToken = CancellationToken.None;

        var fakeFirm = new Firm
        {
            Id = request.FirmId,
            Subscription = new Subscription
            {
                Plan = new Plan { MaxLeads = 10 }
            }
        };
        
        firmRepo.GetFirmWithLeadsAsync(request.FirmId, cancellationToken)
            .Returns(fakeFirm);

        leadRepo.GetCountAsync(request.FirmId, cancellationToken)
            .Returns(15);
        
        // act
        var result = await Assert.ThrowsAsync<ValidationException>(() => 
            service.CreateAsync(request, cancellationToken));
            
        // assert
        Assert.Contains("does not have enough leads", result.Message);
        await leadRepo.DidNotReceiveWithAnyArgs().AddAsync(default!);
    }

    [Fact]
    public async Task CreateAsync_ShouldTCreateLead_WhenFirmExists_AndLimitNotExceeded()
    {
        // arrange
        var firmRepo = Substitute.For<IFirmRepository>();
        var leadRepo = Substitute.For<ILeadRepository>();
        var service = new LeadService(leadRepo, firmRepo);

        var request = new CreateLeadRequest
        {
            FirmId = Guid.NewGuid(),
            Name = Name,
            Email = Email,
            Description = Description,
            Phone = Phone
        };
        var cancellationToken = CancellationToken.None;

        var fakeFirm = new Firm
        {
            Id = request.FirmId,
            Subscription = new Subscription
            {
                Plan = new Plan { MaxLeads = 100 }
            }
        };
        
        firmRepo.GetFirmWithLeadsAsync(request.FirmId, cancellationToken)
            .Returns(fakeFirm);
        leadRepo.GetCountAsync(request.FirmId, cancellationToken)
            .Returns(9);
        
        // act
        var result = await service.CreateAsync(request, cancellationToken);
        
        // assert
        Assert.NotNull(result);
        Assert.Equal(request.FirmId, result.FirmId);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.Phone, result.Phone);
    }
}