using LawFirmsHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Message>> GetByLeadIdAsync(Guid chatId, CancellationToken cancellationToken = default)
    {
        return await _context.Messages.Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}