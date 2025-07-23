using FastEndpoints;
using JetBrains.Annotations;

namespace Workbench.Models;

public class ApplicationRequestBase
{
    public Guid ApplicationId { get; set; }
}

[UsedImplicitly]
public class ApplicationRequestBaseValidator : Validator<ApplicationRequestBase>
{
    public ApplicationRequestBaseValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
    }
}