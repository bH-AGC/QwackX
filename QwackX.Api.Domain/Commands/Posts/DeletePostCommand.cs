using System.Windows.Input;
using CommandQuerySeparation.Commands;

namespace QwackX.Api.Domain.Commands;

public class DeletePostCommand : ICommandDefinition
{
    public int PostId { get; }

    public DeletePostCommand(int id)
    {
        PostId = id;
    }
}