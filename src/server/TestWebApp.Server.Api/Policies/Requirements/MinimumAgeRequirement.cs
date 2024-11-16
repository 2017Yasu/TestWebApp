using Microsoft.AspNetCore.Authorization;

namespace TestWebApp.Server.Api.Policies.Requirements;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}
