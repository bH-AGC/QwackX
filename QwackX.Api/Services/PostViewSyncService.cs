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
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
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

    
// foreach (var userId in userIds)
// {
//     var command = new IncrementViewsPosts(postId, userId);
//     var result = _postViewRepository.Execute(command);  // S'assurer que la méthode Execute() gère correctement les erreurs.
//
//     if (result.IsFailure)
//     {
//         _logger.LogError($"Erreur lors de l'ajout de la vue pour PostId {postId} et UserId {userId}: {result.ErrorMessage}");
//     }
//     else
//     {
//         _logger.LogInformation($"Vue ajoutée pour PostId {postId} et UserId {userId}");
//     }
// }
    
// private DataTable CreatePostViewDataTable(List<PostView> postViews)
// {
//     var dataTable = new DataTable();
//     dataTable.Columns.Add("PostId", typeof(int));
//     dataTable.Columns.Add("UserId", typeof(int));
//
//     foreach (var postView in postViews)
//     {
//         var row = dataTable.NewRow();
//         row["PostId"] = postView.PostId;
//         row["UserId"] = postView.UserId;
//         dataTable.Rows.Add(row);
//     }
//
//     return dataTable;
// }
