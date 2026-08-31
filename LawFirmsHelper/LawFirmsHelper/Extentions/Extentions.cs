using System.Security.Claims;
using LawFirmsHelper;
using LawFirmsHelper.Requests;
using LawFirmsHelper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest;
using RegisterRequest = Microsoft.AspNetCore.Identity.Data.RegisterRequest;

namespace LawFirmsHelper.Extentions;

public static class Extentions
{
    public static WebApplication UseEndpointExtensions(this WebApplication app)
    {
        app.MapPost("/api/auth/register", async (RegisterRequest model, IAuthenticationService authService) =>
        {
            var result = await authService.RegisterAsync(model.Email, model.Password);
            return result.Succeeded ? Results.Ok() : Results.BadRequest();
        });

        app.MapPost("/api/auth/login", async (LoginRequest model, IAuthenticationService authService, CancellationToken cancellationToken) =>
        {
            var token = await authService.LoginAsync(model.Email, model.Password, cancellationToken);
            return token != null ? Results.Ok(new {token}) : Results.BadRequest();
        });


        app.MapPost("/api/firms", async (CreateFirmRequest request, IUserContextService userContextService,
            IFirmService firmService, CancellationToken cancellationToken) =>
        {
          var firm = await firmService.CreateAsync(request, cancellationToken);
          return Results.Ok(firm);
        }).RequireAuthorization();

        app.MapPost("/api/agents",
            async (CreateAgentRequest request, IAgentService agentService, CancellationToken cancellationToken) =>
            {
                var agent = await agentService.CreateAsync(request, cancellationToken);
                return Results.Ok(agent); 
            });

        app.MapGet("/api/firms/{firmId}/agents", async (Guid firmId, IAgentService agentService, CancellationToken cancellationToken) =>
        {
            var agents = await agentService.GetAllByFirmIdAsync(firmId, cancellationToken);
            return Results.Ok(agents);
        });


        app.MapPut("api/firms/{firmId:guid}/subscription", async (Guid firmId, UpdateSubscriptionRequest request,
            IUserContextService userContextService, ISubscriptionService subService, CancellationToken cancellationToken) =>
        {
            var userContext = userContextService.GetContext();
            var userId = userContext.UserId;

            var command = new AddOrUpdateSubscriptionCommand
            {
                FirmId = firmId,
                OwnerId = userId,
                PlanId = request.PlanId,
            };
            
            var success = await subService.AddOrUpdateAsync(command, cancellationToken);
            return success ? Results.Ok() : Results.BadRequest();
        }).RequireAuthorization();
        
        app.MapPost("/api/leads", async (CreateLeadRequest request, ILeadService leadService, CancellationToken cancellationToken) =>
        {
            var lead = await leadService.CreateAsync(request, cancellationToken);
            return Results.Ok(lead);
        });

        app.MapPost("/api/messages", async (CreateMessageRequest request, IMessageService messageService, CancellationToken cancellationToken) =>
        {
            var responce = await messageService.CreateAsync(request, cancellationToken);
            return Results.Ok(responce);
        });

        app.MapPost("/api/chats", async (CreateChatRequest request, IChatService chatService, CancellationToken cancellationToken) =>
        {
            var response = await chatService.CreateAsync(request, cancellationToken);
            return Results.Ok(response);
        });

        app.MapGet("/api/chats/{chatId:guid}/messages", async (
            Guid chatId, 
            [AsParameters] SearchRequest request,
            IChatService chatService, 
            CancellationToken cancellationToken) =>
        {
            var history = await chatService.GetChatHistoryAsync(chatId, request, cancellationToken);
            return Results.Ok(history);
        });
        
        
        
        return app;
    }
}