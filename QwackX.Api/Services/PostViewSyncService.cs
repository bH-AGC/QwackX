using CommandQuerySeparation.Results;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Repositories;

namespace QwackX.Api.Services;
public class PostViewSyncService : BackgroundService
{
    private readonly ILogger<PostViewSyncService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PostViewCache _postViewCache;

    public PostViewSyncService(ILogger<PostViewSyncService> logger, 
        IServiceScopeFactory scopeFactory,
        PostViewCache postViewCache)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _postViewCache = postViewCache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    { 
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                await SyncToDatabase();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur dans le PostViewSyncService.");
        }
    }

    public async Task SyncToDatabase()
    {
        var views = _postViewCache.GetViewsAndClear();

        if (views.Count == 0)
        {
            _logger.LogInformation("Aucune vue à synchroniser.");
            return;
        }

        var postViews = new List<PostView>();

        foreach (var (postId, userIds) in views)
        {
            foreach (var (userId, viewedAt) in userIds)
            {
                postViews.Add(new PostView
                {
                    PostId = postId,
                    UserId = userId,
                    ViewedAt = viewedAt
                });
            }
        }
        
        var bulkInsertCommand = new BulkInsertPostsViews(postViews);
        
        var postViewRepository = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IPostViewRepository>();
        
        Result result = postViewRepository.Execute(bulkInsertCommand);

        if (result.IsFailure)
        {
            _logger.LogError($"Erreur lors de l'insertion en masse : {result.ErrorMessage}");
        }
        else
        {
            _logger.LogInformation("Vues insérées en masse dans la base de données.");
        }

        _logger.LogInformation("Synchronisation des vues en DB terminée.");
    }


    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PostViewSyncService s'arrête. Sauvegarde finale en cours...");
        await SyncToDatabase();
        await base.StopAsync(cancellationToken);
    }
}
