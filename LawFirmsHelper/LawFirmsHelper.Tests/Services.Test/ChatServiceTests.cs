using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using NSubstitute;

namespace LawFirmHelper.Tests;

public class ChatServiceTests
{
    [Fact]
    public async Task GetChatHistoryAsync_ShouldReturnChatHistory()
    {
        // arrange
        var chatRepo = Substitute.For<IChatRepository>();
        var leadRepo = Substitute.For<ILeadRepository>();
        var messageRepo = Substitute.For<IMessageRepository>();
        
        var service = new ChatService(chatRepo, leadRepo, messageRepo);
        
        var chatId = Guid.NewGuid();
        var request = new SearchRequest{Offset =  0, Limit = 50};

        var fakeActor = new Actor
        {
            Id = Guid.NewGuid(),
            Type = ActorType.Lead
        };

        var fakeMessage = new List<Message>
        {
            new Message
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                Text = "Hello World!",
                CreatedAt = DateTime.UtcNow,
                Actor = fakeActor
            }
        };
        
        messageRepo.GetMessageByChatIdAsync(chatId, request.Offset, request.Limit, Arg.Any<CancellationToken>())
            .Returns(fakeMessage);
        
        // act
        var result = await service.GetChatHistoryAsync(chatId, request);
        
        // assert
        Assert.NotNull(result);
        Assert.Single(result);
        
        var responseItem = result.First();
        
        Assert.Equal(fakeMessage[0].Id, responseItem.Id);
        Assert.Equal(fakeMessage[0].Text, responseItem.Text);
        Assert.Equal(fakeMessage[0].CreatedAt, responseItem.CreatedAt);
        
        Assert.Equal(fakeActor.Id, responseItem.ActorId);
        Assert.Equal(fakeActor.Type, responseItem.ActorType);
    }

    [Fact]
    public async Task GetChatHistoryAsync_ShouldReturnNull_WhenNoMessagesExist()
    {
        // arrange
        var chatRepo = Substitute.For<IChatRepository>();
        var leadRepo = Substitute.For<ILeadRepository>();
        var messageRepo = Substitute.For<IMessageRepository>();
        
        var service = new ChatService(chatRepo, leadRepo, messageRepo);
        
        var chatId = Guid.NewGuid();
        
        var searchRequest = new SearchRequest{Offset =  0, Limit = 50};

        messageRepo.GetMessageByChatIdAsync(chatId, searchRequest.Offset, searchRequest.Limit,
                Arg.Any<CancellationToken>())
            .Returns(new List<Message>());
        
        //act
        var result = await service.GetChatHistoryAsync(chatId, searchRequest);
        
        //assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenLeadDoesntExist()
    {
        // arrange
        var leadRepo = Substitute.For<ILeadRepository>();
        var messageRepo = Substitute.For<IMessageRepository>();
        var chatRepo = Substitute.For<IChatRepository>();
        
        var service = new ChatService(chatRepo, leadRepo, messageRepo);

        var request = new CreateChatRequest
        {
            LeadId = Guid.NewGuid()
        };

        leadRepo.GetByIdAsync(request.LeadId, Arg.Any<CancellationToken>())
            .Returns((Lead)null);
        
        // act
        var result = await service.CreateAsync(request);
        
        // assert
        Assert.Null(result);
        
        await chatRepo.DidNotReceive().AddAsync(Arg.Any<Chat>());
        await chatRepo.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateChatAndReturnResponse_WhenLeadExists()
    {
        // arrange
        var leadRepo = Substitute.For<ILeadRepository>();
        var messageRepo = Substitute.For<IMessageRepository>();
        var chatRepo = Substitute.For<IChatRepository>();
        
        var service = new ChatService(chatRepo, leadRepo, messageRepo);

        var request = new CreateChatRequest
        {
            LeadId = Guid.NewGuid()
        };

        var fakeLead = new Lead
        {
            Id = request.LeadId
        };
        
        leadRepo.GetByIdAsync(request.LeadId, Arg.Any<CancellationToken>())
            .Returns(fakeLead);
        
        // act 
        var result = await service.CreateAsync(request);
        
        // assert
        Assert.NotNull(result);
        
        await chatRepo.Received().AddAsync(Arg.Any<Chat>());
        await chatRepo.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    
    
}