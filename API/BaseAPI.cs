using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Net;
using System.Web;
using System.Text;
using BoostifySolution.Data;
using BoostifySolution.Entities;
using BoostifySolution.Global.Enums.Common;
using BoostifySolution.Models.Users;

namespace BoostifySolution.API
{
    public abstract class BaseAPI : Controller
    {
        protected readonly ApplicationDbContext _db;
        protected readonly UserManager<UserLogins> _um;
        protected readonly SignInManager<UserLogins> _sm;
        protected readonly RoleManager<IdentityRole<int>> _rm;
        protected readonly IStringLocalizer<BaseAPI> _l;
        protected IHttpContextAccessor _cx;

        protected CurrentUserObj CurrentUser
        {
            get
            {
                var user = HttpContext.User.Claims.Where(x => x.Type == Global.Constants.Common.CurrentUserClaimKey).FirstOrDefault();

                if (user != null)
                {
                    return JsonConvert.DeserializeObject<CurrentUserObj>(user.Value);
                }
                else
                {
                    return null;
                }
            }
        }

        protected CurrentAdminObj CurrentAdmin
        {
            get
            {
                var admin = HttpContext.User.Claims.Where(x => x.Type == Global.Constants.Common.CurrentAdminClaimKey).FirstOrDefault();

                if (admin != null)
                {
                    return JsonConvert.DeserializeObject<CurrentAdminObj>(admin.Value);
                }
                else
                {
                    return null;
                }
            }
        }

        public BaseAPI(ApplicationDbContext db, UserManager<UserLogins> um, SignInManager<UserLogins> sm, RoleManager<IdentityRole<int>> rm, IStringLocalizer<BaseAPI> l, IHttpContextAccessor cx)
        {
            _db = db;
            _um = um;
            _sm = sm;
            _rm = rm;
            _l = l;
            _cx = cx;
        }

        protected async Task<IdentityResult> AddUserRoleAsync(UserLogins ul, UserRoles ur)
        {
            var roleExist = await _rm.RoleExistsAsync(ur.ToString());

            if (!roleExist)
            {
                await _rm.CreateAsync(new IdentityRole<int>(ur.ToString()));
            }

            return await _um.AddToRoleAsync(ul, ur.ToString());
        }

        protected async Task<IdentityResult> CreateNewUserLoginAsync(UserLogins nul, string password)
        {
            return await _um.CreateAsync(nul, password);
        }

        protected StatusCodeResult ReturnUnauthorizedStatus()
        {
            return StatusCode((int)HttpStatusCode.Unauthorized);
        }

        protected async Task HandleException(Exception e, string dataString)
        {
            if (IsProductionEnvironment())
            {
                _db.ChangeTracker.Clear();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var ne = new Exceptions()
                {
                    Data = dataString,
                    ExceptionTitle = e.Message,
                    ExceptionType = e.GetType().ToString(),
                    ExceptionMessage = e.StackTrace,
                    DateAdded = DateTime.UtcNow,
                    UserId = 0
                };

                if (!string.IsNullOrEmpty(userId))
                {
                    ne.UserId = Int32.Parse(userId);
                }

                _db.Exceptions.Add(ne);

                await _db.SaveChangesAsync();
            }
        }

        private bool IsProductionEnvironment()
        {
            var environment = Environment.GetEnvironmentVariable("APP_ENVIRONMENT");
            return !string.IsNullOrEmpty(environment) && environment.Equals("Production", StringComparison.OrdinalIgnoreCase);
        }
    }

}