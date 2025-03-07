using System.Data;
using System.Data.Common;
using CommandQuerySeparation.Results;
using Microsoft.Data.SqlClient;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Repositories;

namespace QwackX.Api.Domain.Services;

public class PostViewService : BaseService, IPostViewRepository
{
    public PostViewService(DbConnection dbConnection) : base(dbConnection) { }
    
    public Result Execute(BulkInsertPostsViews command)
    {
        try
        {
            DataTable postViewTable = CreatePostViewTable(command.PostViews);
        
            try
            {
                using var sqlCommand = CreateSqlCommand("[AppUserSchema].[PostsViewsBulkInsert]", postViewTable);

                // Ajout du paramètre de sortie pour récupérer le nombre de lignes affectées
                var rowsAffectedParameter = new SqlParameter
                {
                    ParameterName = "@RowsAffected",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(rowsAffectedParameter);
            
                int responseMessage = sqlCommand.ExecuteNonQuery();

                // Récupérer la valeur du paramètre de sortie
                int rowsAffected = (int)rowsAffectedParameter.Value;

                return rowsAffected > 0
                    ? Result.Success()
                    : Result.Failure($"Aucune ligne insérée : {rowsAffected}");
            }
            catch (SqlException ex)
            {
                return Result.Failure($"Erreur lors de l'insertion en masse : {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Erreur générale : {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erreur lors de l'insertion en masse : {ex.Message}");
        }
    }

    
    private SqlCommand CreateSqlCommand(string storedProcedureName, DataTable postViewTable)
    {
        var sqlCommand = new SqlCommand
        {
            CommandText = storedProcedureName,
            CommandType = CommandType.StoredProcedure,
            Connection = (SqlConnection)DbConnection
        };
        
        var parameter = new SqlParameter
        {
            ParameterName = "@PostViews",
            SqlDbType = SqlDbType.Structured,
            Value = postViewTable,
            TypeName = "[AppUserSchema].PostViewType"
        };

        sqlCommand.Parameters.Add(parameter);
        return sqlCommand;
    }
    
    private DataTable CreatePostViewTable(List<PostView> postViews)
    {
        var postViewTable = new DataTable();
        postViewTable.Columns.Add("PostId", typeof(int));
        postViewTable.Columns.Add("UserId", typeof(int));
        postViewTable.Columns.Add("ViewedAt", typeof(DateTime));
        
        foreach (var postView in postViews)
        {
            postViewTable.Rows.Add(postView.PostId, postView.UserId, postView.ViewedAt);
        }
        return postViewTable;
    }
}
