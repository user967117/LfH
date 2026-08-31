using LawFirmsHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Repositories;

public class ChatRepository : Repository<Chat>, IChatRepository
{
    private readonly AppDbContext _context;
    public ChatRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Chats
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    
    public async Task<List<Chat>> GetByLeadAsync(Guid leadId, CancellationToken cancellationToken)
    {
        return await _context.Chats.Where(c => c.LeadId == leadId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}