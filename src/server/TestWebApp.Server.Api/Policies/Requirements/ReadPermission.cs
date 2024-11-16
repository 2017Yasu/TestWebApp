using Microsoft.AspNetCore.Authorization;

namespace TestWebApp.Server.Api.Policies.Requirements;

public class ReadPermission() : IAuthorizationRequirement
{
}
