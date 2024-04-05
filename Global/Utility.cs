using System.ComponentModel;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using BoostifySolution.Data;
using BoostifySolution.Entities;
using BoostifySolution.Global.Enums.Common;
using BoostifySolution.Models.Global;
using BoostifySolution.Models.Users;

namespace BoostifySolution.Global
{
    public static partial class Utility
    {
        private const string passPhrase = "BoostifySolution";
        private const int keysize = 256;


        //Copied from https://www.codementor.io/cerkit/giving-an-enum-a-string-value-using-the-description-attribute-6b4fwdle0 
        /// <summary>
        /// Function : Retrieve a description of an enum value
        /// <code> 
        /// Returns : The description string if it exists. If not, it returns null
        /// </code>
        /// </summary>
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);
                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }
                        break;
                    }
                }
            }
            return description;
        }

        //Copied from https://stackoverflow.com/questions/4367723/get-enum-from-description-attribute
        /// <summary>
        /// Function : Retrieves the value of an enumerated type based on the description string of that value
        /// <code> 
        /// Returns : The value of the enumerated type that matches the description string.
        /// </code>
        /// </summary>
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        /// <summary>
        /// Function : Malaysia start of the day in UTC 
        /// Returns : New DateTime value representing the Start of the day.
        /// </code>
        /// </summary>
        public static DateTime StartOfDayUTC(this DateTime utcInDate)
        {
            var newDate = utcInDate.AddDays(-1);
            return new DateTime(newDate.Year, newDate.Month, newDate.Day, 16, 0, 0);
        }

        /// <summary>
        /// Function : Malaysia end of the day in UTC 
        /// Returns : New DateTime value representing the Start of the day.
        /// </code>
        /// </summary>
        public static DateTime EndOfDayUTC(this DateTime utcInDate)
        {
            return new DateTime(utcInDate.Year, utcInDate.Month, utcInDate.Day, 15, 59, 59);
        }


        /// <summary>
        /// Function : Start of the day for the current DateTime (eg. 12:00AM or 00:00)
        /// Returns : New DateTime value representing the Start of the day.
        /// </code>
        /// </summary>
        public static DateTime StartOfDay(this DateTime inDate)
        {
            return inDate.Date;
        }

        /// <summary>
        /// Function : End of the day for the current DateTime (eg. 1159pm or 23:59)
        /// </code>
        /// <code> 
        /// Returns : New DateTime value representing the end of the day.
        /// </code>
        /// </summary>
        public static DateTime EndOfDay(this DateTime inDate)
        {
            return inDate.Date.AddDays(1).AddMinutes(-1);
        }

        /// <summary>
        /// Funtion : takes DateTime and returns the starting date of the week (eg. Sunday)
        /// <code> 
        /// Returns : the starting date of the week it belongs to.
        /// </code>
        /// </summary>
        public static DateTime StartOfWeek(this DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Funtion : takes DateTime and returns the ending date of the week (eg : Saturday)
        /// <code> 
        /// Returns : the ending date of the week it belongs to.
        /// </code>
        /// </summary>
        public static DateTime EndOfWeek(this DateTime dt)
        {
            int diff = (7 + (DayOfWeek.Saturday - dt.DayOfWeek)) % 7;
            return dt.AddDays(1 * diff).Date;
        }

        // public static DateTime GetWeekRanges(DateTime inputDate)
        // {
        //     Func<DateTime, int> weekProjector =
        //         d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
        //         d,
        //         CalendarWeekRule.FirstFourDayWeek,
        //         DayOfWeek.Sunday);
        // }

        /// <summary>
        /// Funtion : Calculates the age of a person based on their date of birth
        /// <code> 
        /// Returns : Age in Int
        /// </code>
        /// </summary>
        public static int CalculateAge(DateTime dob)
        {
            var currentYear = DateTime.UtcNow.Year;

            return currentYear - dob.Year;
        }

        public static TimeZoneInfo _malaysiaSystemTimeZone;
        public static TimeZoneInfo MalaysiaSystemTimeZone
        {
            get
            {
                if (_malaysiaSystemTimeZone == null)
                {
                    _malaysiaSystemTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                }
                return _malaysiaSystemTimeZone;
            }
        }

        /// <summary>
        /// Funtion : Takes in a DateTime value and returns a new DateTime value
        /// <code> 
        /// Returns : New DateTime value that is converted to the Malaysian time zone
        /// </code>
        /// </summary>
        public static DateTime ToMalaysiaDateTime(this DateTime dt)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dt, MalaysiaSystemTimeZone);
        }

        /// <summary>
        /// Funtion : Converts a DateTime value from one zone to another
        /// <code> 
        /// Returns : Datetime in UTC Time zone
        /// </code>
        /// </summary>
        public static DateTime FromMalaysiaDateTimeToUTC(this DateTime dt)
        {
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dt, MalaysiaSystemTimeZone);
        }

        /// <summary>
        /// Funtion : Takes in a DateTime value and returns the 1st day of the month (eg. 01/01/2023)
        /// <code> 
        /// Returns : New DateTime value that represents the 1st day of the month
        /// </code>
        /// </summary>
        public static DateTime FirstDayOfTheMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// Funtion : Takes in a DateTime value and returns the last day of the month (eg. 31/01/2023).
        /// <code> 
        /// Returns : New DateTime value that represents the last day of the month
        /// </code>
        /// </summary>
        public static DateTime LastDayOfTheMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        /// <summary>
        /// Funtion : The method checks if the count is greater than 1
        /// <code> 
        /// Returns : if it is greater than 1, returns true, otherwise returns false.
        /// </code>
        /// </summary>
        public static string Pluralise(this string singularWord, int count, string charToAdd)
        {
            return count > 1 ? $"{singularWord}{charToAdd}" : singularWord;
        }

        /// <summary>
        /// Funtion : Create a list of dropdown options
        /// <code> 
        /// Returns : Generate a list of dropdown options for an enumeration type in a user interface
        /// </code>
        /// </summary>
        public static List<DropdownOptions> GetEnumAsDropdownOptions(Type enumType)
        {
            var ddo = new List<DropdownOptions>();

            var enumDict = Enum.GetValues(enumType).Cast<object>()
               .ToDictionary(x => (int)x, x => x.ToString());

            foreach (var e in enumDict)
            {
                var enumField = enumType.GetField(e.Value);
                var valueAttributes = enumField.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var enumDescription = ((DescriptionAttribute)valueAttributes[0]).Description;

                var opt = new DropdownOptions()
                {
                    Id = e.Key,
                    Text = enumDescription
                };
                ddo.Add(opt);
            }

            return ddo.OrderBy(i => i.Text).ToList();
        }

        /// <summary>
        /// Funtion : Calculate the number of distance in meters
        /// <code> 
        /// Returns : Converts the distance from meters and returns as string.
        /// </code>
        /// </summary>
        public static string FormatDistance(this double distance)
        {
            return string.Format("{0:0.0}km", distance / 1000);
        }

        public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<UserLogins, IdentityRole<int>>
        {
            private readonly ApplicationDbContext _db;

            public CustomUserClaimsPrincipalFactory(
                ApplicationDbContext db,
                UserManager<UserLogins> userManager,
                RoleManager<IdentityRole<int>> roleManager,
                IOptions<IdentityOptions> optionsAccessor)
                    : base(userManager, roleManager, optionsAccessor)
            {
                _db = db;
            }

            public override async Task<ClaimsPrincipal> CreateAsync(UserLogins user)
            {
                IList<string> loginUserRole = await base.UserManager.GetRolesAsync(user);
                var principal = await base.CreateAsync(user);

                if (loginUserRole.Contains(UserRoles.User.ToString()))
                {
                    var userEntity = await _db.Users
                               .Where(x => x.UserLoginId == user.Id)
                               .FirstAsync();

                    CurrentUserObj cu = new()
                    {
                        UserId = userEntity.UserId,
                        UserName = userEntity.FullName,
                        Currency = userEntity.Currency
                    };

                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(Constants.Common.CurrentUserClaimKey, JsonConvert.SerializeObject(cu)));
                }
                else
                {
                    var adminStaff = await _db.AdminStaffs
                           .Include(x => x.UserLogin)
                           .Where(x => x.UserLoginId == user.Id)
                               .FirstAsync();

                    CurrentAdminObj ca = new()
                    {
                        AdminStaffId = adminStaff.AdminStaffId,
                        UserName = adminStaff.FullName,
                        AdminStaffType = adminStaff.AdminStaffType,
                        RequirePasswordChange = adminStaff.RequirePasswordChange,
                        AdminStaffEmail = adminStaff.UserLogin.Email
                    };

                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(Constants.Common.CurrentAdminClaimKey, JsonConvert.SerializeObject(ca)));
                }

                return principal;
            }
        }

        /// <summary>
        /// Funtion : Convert string object to URL format
        /// <code> 
        /// Returns : The modified string as a query string, which can be used in a URL.
        /// </code>
        /// </summary>
        public static string JsonToQueryString(string jsonQuery)
        {
            string str = "?";
            str += jsonQuery.Replace(":", "=").Replace("{", "").
                        Replace("}", "").Replace(",", "&").
                            Replace("\"", "");
            return str;
        }

        public static string GetCurrencySymbol(int currencyType)
        {
            switch (currencyType)
            {
                case (int)CurrencyTypes.MYR:
                    return "RM";
                case (int)CurrencyTypes.Yen:
                    return "";
                case (int)CurrencyTypes.Rupiah:
                    return "Rp";
                default:
                    return "RM";
            }
        }



        [GeneratedRegex("[^0-9]")]
        public static partial Regex NumberOnly();

        [GeneratedRegex("[^a-zA-Z0-9\\s]")]
        public static partial Regex AlphaNumbericWSpace();

        [GeneratedRegex("/^[a-zA-Z0-9._]*$/")]
        public static partial Regex StoreUsernamePattern();
    }
}

