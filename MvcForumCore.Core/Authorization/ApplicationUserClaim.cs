using System;
using Microsoft.AspNetCore.Identity;

namespace MvcForumCore.Authorization
{
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
    }
}
