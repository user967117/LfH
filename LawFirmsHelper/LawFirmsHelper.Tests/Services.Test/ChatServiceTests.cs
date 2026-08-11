using LawFirmsHelper.Models;
using LawFirmsHelper.Repositories;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using Moq;

namespace TestProject1;

public class ChatServiceTests
{
    [Fact]
    public async Task GetChatHistoryAsync_ShouldReturnChatHistory()
    {
        // arrange
        var chatRepo = new Mock<IChatRepository>();
        var leadRepo = new Mock<ILeadRepository>();
        var messageRepo = new Mock<IMessageRepository>();
        
        var service = new ChatService(chatRepo.Object, leadRepo.Object, messageRepo.Object);
        
        var chatId = Guid.NewGuid();
        var request = new SearchRequest{Offset =  0, Limit = 50};
        
        messageRepo.Setup(repo => repo.GetMessageByChatIdAsync(chatId, request.Offset, request.Limit, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Message>());
        
        // act
        var result = await service.GetChatHistoryAsync(chatId, request);
        
        // assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}