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
        
        var service = new MessageService(messageRepo, chatRepo);

        var request = new CreateMessageRequest
        {
            ChatId = Guid.NewGuid(),
            Text = "TestText 12134"
        };
        
        var cancelationToken = CancellationToken.None;
        
        chatRepo.GetByIdAsync(request.ChatId, cancelationToken)
            .Returns((List<Chat>)null!);
        
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
        var service = new MessageService(messageRepo, chatRepo);

        var request = new CreateMessageRequest
        {
            ChatId = Guid.NewGuid(),
        };
        
        var cancelationToken = CancellationToken.None;

        var fakeChat = new List<Chat>
        {
            new Chat { Id = request.ChatId }
        };
        
        chatRepo.GetByIdAsync(request.ChatId, cancelationToken)
            .Returns(fakeChat);
        
        // act
        var result = await service.CreateAsync(request, cancelationToken);
        
        // assert
        Assert.NotNull(result);

        Received.InOrder(() =>
            {
                chatRepo.GetByIdAsync(request.ChatId, cancelationToken);
                messageRepo.AddAsync(Arg.Is<Message>(x => x.ChatId == request.ChatId));
                messageRepo.SaveChangesAsync(cancelationToken);
            });

    }
}