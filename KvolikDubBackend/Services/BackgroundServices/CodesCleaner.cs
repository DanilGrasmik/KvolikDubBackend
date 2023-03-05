using KvolikDubBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace KvolikDubBackend.Services.BackgroundServices;

public class CodesCleaner : BackgroundService
{
    private readonly TimeSpan _cleanMinutes;
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public CodesCleaner(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _cleanMinutes = TimeSpan.FromMinutes(configuration.GetValue<int>("CodesCleanFrequency"));
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                var expiredCodes = await context
                    .ConfirmCodes
                    .Where(code => code.ExpiredDate <= DateTime.UtcNow)
                    .ToListAsync(cancellationToken: stoppingToken);

                foreach (var code in expiredCodes)
                {
                    context.ConfirmCodes.Remove(code);
                }

                await context.SaveChangesAsync(stoppingToken);
                
                Console.WriteLine("/////////////////////////////");
                Console.WriteLine("Cleaned codes");
                Console.WriteLine("/////////////////////////////");

                await Task.Delay(_cleanMinutes, stoppingToken);
            }
        }
    }
}