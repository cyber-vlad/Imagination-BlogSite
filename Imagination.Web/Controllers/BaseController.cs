using Imagination.Domain.Entities.UserInfo;
using Imagination.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Imagination.Web.Controllers
{
    public class BaseController : Controller
    {
        [AllowAnonymous]
        public async Task<UserClaim> GetUserClaims()
        {
            try
            {
                var claimPrincipal = User as ClaimsPrincipal;
                var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;

                UserClaim userClaim = new UserClaim();

                if (claimIdentity.Claims.Count() != 0)
                {
                    var IdClaim = (from c in claimIdentity.Claims
                                   where c.Type == ClaimTypes.NameIdentifier
                                   select c).Single();
                    var UsernameClaim = (from c in claimIdentity.Claims
                                         where c.Type == ClaimTypes.Name
                                         select c).Single();
                    var EmailClaim = (from c in claimIdentity.Claims
                                      where c.Type == ClaimTypes.Email
                                      select c).Single();
                    var PictureClaim = (from c in claimIdentity.Claims
                                        where c.Type == "PhotoUrl"
                                        select c).Single();
                    var RoleClaim = (from c in claimIdentity.Claims
                                        where c.Type == "UserRole"
                                        select c).Single();

                    userClaim.Id = int.Parse(IdClaim.Value);
                    userClaim.Username = UsernameClaim.Value;
                    userClaim.Email = EmailClaim.Value;
                    userClaim.PhotoUrl = PictureClaim.Value;
                    userClaim.UserRole = (UserRole)Enum.Parse(typeof(UserRole), RoleClaim.Value);


                }
                else
                {
                    userClaim.Id = 0;
                    userClaim.Username = string.Empty;
                    userClaim.Email = string.Empty;
                    userClaim.PhotoUrl = string.Empty;
                    userClaim.UserRole = UserRole.None;
                }

                return userClaim;
            }
            catch
            {
                UserClaim userClaim = new UserClaim()
                {
                    Id = 0,
                    Username = string.Empty,
                    Email = string.Empty,
                    PhotoUrl = string.Empty,
                    UserRole = UserRole.None,
                };

                return userClaim;
            }
        }
    }
}
