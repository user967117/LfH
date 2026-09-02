using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using NSubstitute;

namespace LawFirmHelper.Tests;

public class MessageServiceTest
{
    [Fact]
    public async Task CreateAsync_WhenChatDoesNotExist()
    {
        // arrange
        var chatRepo = Substitute.For<IChatRepository>();
        var messageRepo = Substitute.For<IMessageRepository>();
        var aiService = Substitute.For<IAiAssistantService>();
        var leadRepo = Substitute.For<ILeadRepository>();
        
        var service = new MessageService(messageRepo, chatRepo, aiService,  leadRepo);

        var request = new CreateMessageRequest
        {
            ChatId = Guid.NewGuid(),
            Text = "TestText 12134"
        };
        
        var cancelationToken = CancellationToken.None;
        
        chatRepo.GetByIdAsync(request.ChatId, cancelationToken)
            .Returns((Chat)null!);
        
        // act
        var result = await service.CreateAsync(request, cancelationToken);
        
        // assert
        Assert.Null(result);
        
        await messageRepo.DidNotReceiveWithAnyArgs().AddAsync(default!);
        await messageRepo.DidNotReceiveWithAnyArgs().SaveChangesAsync(default!);
    }

    [Fact]
    public async Task CreateAsync_WhenChatExists()
    {
        // arrange
        var chatRepo = Substitute.For<IChatRepository>();
        var messageRepo = Substitute.For<IMessageRepository>();
        var aiService = Substitute.For<IAiAssistantService>();
        var leadRepo = Substitute.For<ILeadRepository>();
        var service = new MessageService(messageRepo, chatRepo, aiService,  leadRepo);

        var request = new CreateMessageRequest
        {
            ChatId = Guid.NewGuid(),
            Text = "TestText 12134"
        };
        
        var cancelationToken = CancellationToken.None;
        
        var fakeLeadId = Guid.NewGuid();
        var fakeFirmId = Guid.NewGuid();

        var fakeChat = new Chat
        {
             Id = request.ChatId,
             LeadId = fakeLeadId
        };

        var fakeLead = new Lead
        {
            Id = fakeLeadId,
            FirmId = fakeFirmId
        };
        
        chatRepo.GetByIdAsync(request.ChatId, cancelationToken)
            .Returns(fakeChat);
        leadRepo.GetByIdAsync(fakeLeadId, cancelationToken)
            .Returns(fakeLead);
        
        var expectedAiResponse = "Вітаю!";
        aiService.GetNextResponseAsync(fakeFirmId, Arg.Any<Guid>(),Arg.Any<List<Message>>(), cancelationToken)
            .Returns(expectedAiResponse);
        
        // act
        var result = await service.CreateAsync(request, cancelationToken);
        
        // assert
        Assert.NotNull(result);
        Assert.Equal(expectedAiResponse, result.Text);

        Received.InOrder(() =>
            {
                chatRepo.GetByIdAsync(request.ChatId, cancelationToken);
                messageRepo.AddAsync(Arg.Is<Message>(x => x.ChatId == request.ChatId));
                messageRepo.SaveChangesAsync(cancelationToken);
                
                leadRepo.GetByIdAsync(fakeLeadId, cancelationToken);
                messageRepo.GetMessageByChatIdAsync(request.ChatId, 0, 50, cancelationToken);
                
                aiService.GetNextResponseAsync(fakeFirmId, Arg.Any<Guid>(), Arg.Any<List<Message>>(), cancelationToken);
                
                messageRepo.AddAsync(Arg.Is<Message>(x => x.Text == expectedAiResponse));
                messageRepo.SaveChangesAsync(cancelationToken);
            });

    }
}