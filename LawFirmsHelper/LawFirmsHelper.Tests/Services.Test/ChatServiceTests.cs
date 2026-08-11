using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using Moq;
using NSubstitute;

namespace TestProject1;

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
}