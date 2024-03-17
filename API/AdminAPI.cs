using System.Net;
using System.Xml.Schema;
using MarketplacBoostifySolutione.Models.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using BoostifySolution.API;
using BoostifySolution.Data;
using BoostifySolution.Entities;
using BoostifySolution.Global;
using BoostifySolution.Global.Enums.Common;
using BoostifySolution.Models.Admin;
using BoostifySolution.Models.Global;
using BoostifySolution.Models.Users;
using boostifysolution1.Models.Admin;
using boostifysolution1.Entities;

namespace BoostifySolution.API
{
    [Route("api/Admin")]
    public class AdminAPI : BaseAPI
    {
        public AdminAPI(ApplicationDbContext db, UserManager<UserLogins> um, SignInManager<UserLogins> sm, RoleManager<IdentityRole<int>> rm, IStringLocalizer<BaseAPI> l, IHttpContextAccessor cx) : base(db, um, sm, rm, l, cx)
        {
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers(int pageIndex, int pageSize, string sortField, string sortOrder, string? filterUserName, int? filterAccountStatus, int? filterDateAdded, DateTime? filterDateAddedStart, DateTime? filterDateAddedEnd, int? filterCountry, int? filterLanguage)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null)
                {
                    return ReturnUnauthorizedStatus();
                }

                var userQuery = _db.Users
                .Include(x => x.AdminStaff)
                .AsQueryable();

                if (ca.AdminStaffType == (int)AdminStaffTypes.Staff)
                {
                    userQuery = userQuery.Where(x => x.AdminStaffId == ca.AdminStaffId);
                }
                else if (ca.AdminStaffType == (int)AdminStaffTypes.Leader)
                {
                    var staffIds = await _db.LeaderAdminStaffs.Where(x => x.AdminStaffId == ca.AdminStaffType).Select(x => x.AssociatedAdminStaffId).ToListAsync();

                    userQuery = userQuery.Include(x => x.AdminStaff).Where(x => staffIds.Contains(x.AdminStaffId) || x.AdminStaffId == ca.AdminStaffId);
                }


                switch (sortField)
                {
                    case "userName":
                        if (sortOrder == "asc")
                        {
                            userQuery = userQuery.OrderBy(x => x.FullName);
                        }
                        else
                        {
                            userQuery = userQuery.OrderBy(x => x.FullName);
                        }
                        break;
                    case "dateAdded":
                        if (sortOrder == "asc")
                        {
                            userQuery = userQuery.OrderBy(x => x.DateAdded);
                        }
                        else
                        {
                            userQuery = userQuery.OrderBy(x => x.DateAdded);
                        }
                        break;
                    case "country":
                        if (sortOrder == "asc")
                        {
                            userQuery = userQuery.OrderBy(x => x.Country);
                        }
                        else
                        {
                            userQuery = userQuery.OrderBy(x => x.Country);
                        }
                        break;
                    case "language":
                        if (sortOrder == "asc")
                        {
                            userQuery = userQuery.OrderBy(x => x.Language);
                        }
                        else
                        {
                            userQuery = userQuery.OrderBy(x => x.Language);
                        }
                        break;
                    case "accountStatus":
                        if (sortOrder == "asc")
                        {
                            userQuery = userQuery.OrderBy(x => x.AccountStatus);
                        }
                        else
                        {
                            userQuery = userQuery.OrderBy(x => x.AccountStatus);
                        }
                        break;
                    default:
                        userQuery = userQuery.OrderByDescending(x => x.DateAdded);
                        break;
                }

                if (filterUserName != null)
                {
                    userQuery = userQuery.Where(x => x.FullName.Contains(filterUserName));
                }

                if (filterAccountStatus != null)
                {
                    userQuery = userQuery.Where(x => x.AccountStatus == filterAccountStatus);
                }

                if (filterCountry != null)
                {
                    userQuery = userQuery.Where(x => x.Country == filterCountry);
                }

                if (filterLanguage != null)
                {
                    userQuery = userQuery.Where(x => x.Language == filterLanguage);
                }

                if (filterDateAdded != null)
                {
                    var todayDate = DateTime.UtcNow.ToMalaysiaDateTime();
                    var startDate = todayDate.StartOfDay();
                    var endDate = todayDate.EndOfDay();

                    switch (filterDateAdded)
                    {
                        case (int)DateFilters.Today:
                            break;
                        case (int)DateFilters.Yesterday:
                            startDate = todayDate.AddDays(-1).StartOfDay();

                            break;
                        case (int)DateFilters.Last7Days:
                            startDate = todayDate.AddDays(-6).StartOfDay(); // Start date is 7 days ago

                            break;
                        case (int)DateFilters.ThisMonth:
                            startDate = startDate.AddDays(-1);

                            break;
                        case (int)DateFilters.LastMonth:
                            var firstDayOfLastMonth = todayDate.AddMonths(-1).FirstDayOfTheMonth().StartOfDay();
                            var lastDayOfLastMonth = todayDate.AddMonths(-1).LastDayOfTheMonth().EndOfDay();

                            startDate = firstDayOfLastMonth;
                            endDate = lastDayOfLastMonth;

                            break;
                        case (int)DateFilters.Custom:
                            startDate = filterDateAddedStart.GetValueOrDefault(DateTime.UtcNow).StartOfDay();
                            endDate = filterDateAddedEnd.GetValueOrDefault(DateTime.UtcNow).EndOfDay();

                            break;
                    }

                    userQuery = userQuery.Where(x => x.DateAdded >= startDate.FromMalaysiaDateTimeToUTC() && x.DateAdded < endDate.FromMalaysiaDateTimeToUTC());
                }

                var userCount = await userQuery.CountAsync();

                var users = await userQuery.Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

                var aul = new AdminUsersList()
                {
                    UsersList = new List<AdminUserListDetails>(),
                    ItemsCount = userCount,
                    DateFilterOptions = Utility.GetEnumAsDropdownOptions(typeof(DateFilters)),
                    AccountStatusOptions = Utility.GetEnumAsDropdownOptions(typeof(UserAccountStatuses)),
                    CountryOptions = Utility.GetEnumAsDropdownOptions(typeof(Countries)),
                    LanguageOptions = Utility.GetEnumAsDropdownOptions(typeof(Languages)),
                    CurrencyOptions = Utility.GetEnumAsDropdownOptions(typeof(CurrencyTypes))
                };

                foreach (var x in users)
                {
                    var currencySymbol = Global.Utility.GetCurrencySymbol(x.Currency);

                    var nu = new AdminUserListDetails()
                    {
                        UserId = x.UserId,
                        UserName = x.FullName,
                        AccountStatus = ((UserAccountStatuses)x.AccountStatus).GetDescription(),
                        Wallet = $"{currencySymbol}{x.WalletAmount:0.00}",
                        DateAdded = x.DateAdded.ToLocalTime().ToString("dd/MM/yyyy"),
                        Country = ((Countries)x.Country).GetDescription(),
                        Language = ((Languages)x.Language).GetDescription()
                    };

                    if (ca.AdminStaffType == (int)AdminStaffTypes.FullAdmin || ca.AdminStaffType == (int)AdminStaffTypes.Leader)
                    {
                        nu.AdminStaffName = x.AdminStaff.FullName;
                    }

                    aul.UsersList.Add(nu);
                }

                return Ok(new APIJsonReturnObject(aul));

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject("There was a problem when loading users. Please refresh and try again."));
            }
        }

        [HttpGet("Users/{userId}")]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null)
                {
                    return ReturnUnauthorizedStatus();
                }

                var userQuery = _db.Users
                .Include(x => x.UserLogin)
                .Where(x => x.UserId == userId)
                .AsQueryable();

                if (ca.AdminStaffType == (int)AdminStaffTypes.Staff)
                {
                    userQuery = userQuery.Where(x => x.AdminStaffId == ca.AdminStaffId);
                }

                var user = await userQuery.FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new APIJsonReturnObject("User not found. Please select a different user."));
                }

                var walletTransactions = await _db.WalletTransactions
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateAdded)
                .ToListAsync();

                var userTasks = await _db.UserTasks
                .Include(x => x.Task)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateAdded)
                .ToListAsync();

                var currencySymbol = Global.Utility.GetCurrencySymbol(user.Currency);

                var ud = new AdminUserDetails()
                {
                    Name = user.FullName,
                    LoginUsername = user.UserLogin.UserName,
                    Currency = user.Currency,
                    Language = user.Language,
                    WalletAmount = $"{currencySymbol}{user.WalletAmount:0.00}",
                    DateAdded = user.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yyyy"),
                    WalletTransactionList = new List<WalletTransactionListDetails>(),
                    UserTaskList = new List<UserTaskListDetail>(),
                    AccountStatus = ((UserAccountStatuses)user.AccountStatus).GetDescription(),
                    ShowPopupMessage = user.ShowPopupMessage,
                    PopupMessage = user.PopupMessage,
                    PopupMessageTitle = user.PopupMessageTitle,
                    Country = user.Country
                };

                foreach (var x in walletTransactions)
                {
                    var wtld = new WalletTransactionListDetails()
                    {
                        WalletTransactionId = x.WalletTransactionId,
                        Amount = $"{currencySymbol}{x.Amount:0.00}",
                        Type = ((WalletTransactionTypes)x.WalletTransactionType).GetDescription(),
                        Status = ((WalletTransactionStatuses)x.Status).GetDescription(),
                        DateAdded = x.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yyyy hh:mmtt")
                    };

                    ud.WalletTransactionList.Add(wtld);
                }

                foreach (var x in userTasks)
                {
                    var utld = new UserTaskListDetail()
                    {
                        UserTaskId = x.UserTaskId,
                        TaskTitle = x.Task.TaskTitle,
                        Status = ((UserTaskStatuses)x.Status).GetDescription(),
                        DateAdded = x.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yy hh:mmtt"),
                        QuantityPurchased = x.QuantityPurchased != null ? x.QuantityPurchased.ToString() : "N/A",
                        DateCompleted = x.CompletedDateTime != null ? x.CompletedDateTime.Value.ToMalaysiaDateTime().ToString("dd/MM/yy hh:mmtt") : "N/A",
                        Category = ((TaskCategories)x.Task.TaskCategory).GetDescription()
                    };

                    var commission = $"{x.CommissionPercentage:0}%";

                    if (x.CommissionAmount != null)
                    {
                        commission = $"{commission}(RM{x.CommissionAmount.GetValueOrDefault():0.00})";
                    }

                    utld.Commission = commission;

                    ud.UserTaskList.Add(utld);
                }

                return Ok(new APIJsonReturnObject(ud));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject("There was a problem when loading the user details. Please refresh and try again."));
            }
        }

        [HttpGet("Staffs")]
        public async Task<IActionResult> GetAdminStaffs(int pageIndex, int pageSize, string sortField, string sortOrder, int? filterAdminLeader, int? filterAdminType)
        {
            var ca = CurrentAdmin;

            if (ca == null || ca.AdminStaffType == (int)AdminStaffTypes.Staff)
            {
                return ReturnUnauthorizedStatus();
            }

            var adminStaffQuery = _db.AdminStaffs.Include(x => x.UserLogin).AsQueryable();

            if (filterAdminLeader != null || ca.AdminStaffType == (int)AdminStaffTypes.Leader)
            {
                int leaderStaffId;

                if (filterAdminLeader != null)
                {
                    leaderStaffId = filterAdminLeader.Value;
                }
                else
                {
                    leaderStaffId = ca.AdminStaffId;
                }

                var staffIds = await _db.LeaderAdminStaffs.Where(x => x.AdminStaffId == leaderStaffId).Select(x => x.AssociatedAdminStaffId).ToListAsync();

                adminStaffQuery = adminStaffQuery.Where(x => staffIds.Contains(x.AdminStaffId));
            }

            switch (sortField)
            {
                case "name":
                    if (sortOrder == "asc")
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.FullName);
                    }
                    else
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.FullName);
                    }
                    break;
                case "status":
                    if (sortOrder == "asc")
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.Status);
                    }
                    else
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.Status);
                    }
                    break;
                case "adminType":
                    if (sortOrder == "asc")
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.AdminStaffType);
                    }
                    else
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.AdminStaffType);
                    }
                    break;
                case "dateAdded":
                    if (sortOrder == "asc")
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.DateAdded);
                    }
                    else
                    {
                        adminStaffQuery = adminStaffQuery.OrderBy(x => x.DateAdded);
                    }
                    break;
                default:
                    adminStaffQuery = adminStaffQuery.OrderBy(x => x.FullName);
                    break;
            }

            if (filterAdminType != null)
            {
                adminStaffQuery = adminStaffQuery.Where(x => x.AdminStaffType == filterAdminType);
            }

            var adminStaffs = await adminStaffQuery.ToListAsync();

            var asl = new AdminStaffsList()
            {
                StaffsList = new List<AdminStaffListDetails>(),
                ItemsCount = adminStaffs.Count,
                CountryOptions = Utility.GetEnumAsDropdownOptions(typeof(Countries)),
                AdminTypeOptions = Utility.GetEnumAsDropdownOptions(typeof(AdminStaffTypes)),
                AdminLeaderOptions = new List<DropdownOptions>()
            };

            var adminStaffIds = adminStaffs.Select(x => x.AdminStaffId);

            var leaderAdminStaffDict = await _db.LeaderAdminStaffs
                .Include(x => x.AdminStaff)
                .Where(x => adminStaffIds.Contains(x.AssociatedAdminStaffId))
                .ToDictionaryAsync(x => x.AssociatedAdminStaffId, x => x);

            foreach (var x in adminStaffs)
            {
                var asld = new AdminStaffListDetails()
                {
                    AdminStaffId = x.AdminStaffId,
                    Name = x.FullName,
                    Status = ((AdminStaffStatuses)x.Status).GetDescription(),
                    DateAdded = x.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yyyy"),
                    AdminType = ((AdminStaffTypes)x.AdminStaffType).GetDescription(),
                    Email = x.UserLogin.Email
                };

                if (leaderAdminStaffDict.ContainsKey(x.AdminStaffId))
                {
                    asld.Leader = leaderAdminStaffDict[x.AdminStaffId].AdminStaff.FullName;
                };

                asl.StaffsList.Add(asld);
            }

            var leaders = await _db.AdminStaffs
            .Where(x => x.Status == (int)AdminStaffStatuses.Active && x.AdminStaffType == (int)AdminStaffTypes.Leader)
            .OrderBy(x => x.FullName)
            .ToListAsync();

            foreach (var x in leaders)
            {
                var ddo = new DropdownOptions()
                {
                    Id = x.AdminStaffId,
                    Text = x.FullName
                };

                asl.AdminLeaderOptions.Add(ddo);
            }

            return Ok(new APIJsonReturnObject(asl));
        }

        [HttpGet("Staffs/{adminStaffId}")]
        public async Task<IActionResult> GetAdminStaffDetails(int adminStaffId)
        {
            var ca = CurrentAdmin;

            if (ca == null || ca.AdminStaffType == (int)AdminStaffTypes.Staff)
            {
                return ReturnUnauthorizedStatus();
            }

            var staff = await _db.AdminStaffs
            .Include(x => x.UserLogin)
            .Where(x => x.AdminStaffId == adminStaffId)
            .FirstAsync();

            var staffUsers = await _db.Users
            .Where(x => x.AdminStaffId == staff.AdminStaffId)
            .ToListAsync();

            var leader = await _db.LeaderAdminStaffs
            .Include(x => x.AdminStaff)
            .Where(x => x.AssociatedAdminStaffId == adminStaffId)
            .FirstOrDefaultAsync();

            var asd = new AdminStaffDetails()
            {
                Name = staff.FullName,
                LoginUsername = staff.UserLogin.UserName,
                AdminType = staff.AdminStaffType,
                Status = ((AdminStaffStatuses)staff.Status).GetDescription(),
                Accounts = staffUsers.Count,
                DateAdded = staff.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yyyy"),
                UsersList = new List<AdminStaffUserListDetails>(),
                Leader = leader != null ? leader.AdminStaff.FullName : ""
            };

            foreach (var x in staffUsers)
            {
                var currencySymbol = Global.Utility.GetCurrencySymbol(x.Currency);

                var asuld = new AdminStaffUserListDetails()
                {
                    UserId = x.UserId,
                    Name = x.FullName,
                    WalletAmount = $"{currencySymbol}{x.WalletAmount:0.00}",
                    DateAdded = x.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yyyy")
                };

                asd.UsersList.Add(asuld);
            }

            return Ok(new APIJsonReturnObject(asd));
        }

        [HttpGet("Tasks")]
        public async Task<IActionResult> GetTasks(int pageIndex, int pageSize, string sortField, string sortOrder, string? filterTaskTitle, string? filterProductName, int? filterTaskCategory, int? filterCountry, int? filterLanguage, int? filterTaskStatus)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var taskQuery = _db.Tasks
            .OrderByDescending(x => x.DateAdded)
            .AsQueryable();

            switch (sortField)
            {
                case "taskTitle":
                    if (sortOrder == "asc")
                    {
                        taskQuery = taskQuery.OrderBy(x => x.TaskTitle);
                    }
                    else
                    {
                        taskQuery = taskQuery.OrderBy(x => x.TaskTitle);
                    }
                    break;
                case "productName":
                    if (sortOrder == "asc")
                    {
                        taskQuery = taskQuery.OrderBy(x => x.ProductName);
                    }
                    else
                    {
                        taskQuery = taskQuery.OrderBy(x => x.ProductName);
                    }
                    break;
                case "status":
                    if (sortOrder == "asc")
                    {
                        taskQuery = taskQuery.OrderBy(x => x.Status);
                    }
                    else
                    {
                        taskQuery = taskQuery.OrderBy(x => x.Status);
                    }
                    break;
                case "category":
                    if (sortOrder == "asc")
                    {
                        taskQuery = taskQuery.OrderBy(x => x.TaskCategory);
                    }
                    else
                    {
                        taskQuery = taskQuery.OrderBy(x => x.TaskCategory);
                    }
                    break;
                case "language":
                    if (sortOrder == "asc")
                    {
                        taskQuery = taskQuery.OrderBy(x => x.Language);
                    }
                    else
                    {
                        taskQuery = taskQuery.OrderBy(x => x.Language);
                    }
                    break;
                case "dateAdded":
                    if (sortOrder == "asc")
                    {
                        taskQuery = taskQuery.OrderBy(x => x.DateAdded);
                    }
                    else
                    {
                        taskQuery = taskQuery.OrderBy(x => x.DateAdded);
                    }
                    break;
                default:
                    taskQuery = taskQuery.OrderBy(x => x.TaskTitle);
                    break;
            }

            if (filterTaskTitle != null)
            {
                taskQuery = taskQuery.Where(x => x.TaskTitle.Contains(filterTaskTitle));
            }

            if (filterProductName != null)
            {
                taskQuery = taskQuery.Where(x => x.ProductName.Contains(filterProductName));
            }

            if (filterTaskCategory != null)
            {
                taskQuery = taskQuery.Where(x => x.TaskCategory == filterTaskCategory);
            }

            if (filterLanguage != null)
            {
                taskQuery = taskQuery.Where(x => x.TaskCategory == filterLanguage);
            }

            if (filterCountry != null)
            {
                taskQuery = taskQuery.Where(x => x.TaskCategory == filterCountry);
            }

            if (filterTaskStatus != null)
            {
                taskQuery = taskQuery.Where(x => x.Status == filterTaskStatus);
            }

            var tasks = await taskQuery.ToListAsync();

            var atl = new AdminTasksList()
            {
                TasksList = new List<AdminTaskListDetails>(),
                ItemsCount = tasks.Count,
                TaskCategoryOptions = Utility.GetEnumAsDropdownOptions(typeof(TaskCategories)),
                PlatformOptions = Utility.GetEnumAsDropdownOptions(typeof(Platforms)),
                CountryOptions = Utility.GetEnumAsDropdownOptions(typeof(Countries)),
                LanguageOptions = Utility.GetEnumAsDropdownOptions(typeof(Languages)),
                TaskStatusOptions = Utility.GetEnumAsDropdownOptions(typeof(TaskStatuses)),
            };

            foreach (var x in tasks)
            {
                var newTaskListDetails = new AdminTaskListDetails()
                {
                    TaskId = x.TaskId,
                    TaskTitle = x.TaskTitle,
                    ProductName = x.ProductName,
                    Status = ((TaskStatuses)x.Status).GetDescription(),
                    DateAdded = x.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yy"),
                    Language = ((Languages)x.Language).GetDescription(),
                    Country = ((Countries)x.Country).GetDescription(),
                    Category = ((TaskCategories)x.TaskCategory).GetDescription(),
                    ProductPrice = x.ProductPrice.ToString("0.00")
                };

                atl.TasksList.Add(newTaskListDetails);
            }

            return Ok(new APIJsonReturnObject(atl));
        }

        [HttpGet("Tasks/{taskId}")]
        public async Task<IActionResult> GetTaskDetails(int taskId)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var task = await _db.Tasks.Where(x => x.TaskId == taskId).FirstAsync();

            var atd = new AdminTaskDetails()
            {
                TaskTitle = task.TaskTitle,
                ProductName = task.ProductName,
                ProductDescription = task.ProductDescription,
                ProductMainImageURL = task.ProductMainImageURL,
                ProductImagesURL = task.ProductImagesURL,
                ProductPrice = task.ProductPrice,
                ProductRating = task.ProductRating,
                StoreName = task.StoreName,
                StoreThumbnailURL = task.StoreThumbnailURL,
                Status = ((TaskStatuses)task.Status).GetDescription(),
                StatusId = task.Status,
                Language = task.Language,
                Platform = task.Platform,
                TaskCategory = task.TaskCategory,
            };

            return Ok(new APIJsonReturnObject(atd));
        }

        [HttpPut("Users/{userId}/UserTasks/{userTaskId}/UpdateStatus")]
        public async Task<IActionResult> UpdateUserTaskStatus(int userTaskId, int userId, [FromBody] AdminUpdateStatusRequest data)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            try
            {
                var userTask = await _db.UserTasks
                .Include(x => x.Task)
                .Include(x => x.User)
                .Where(x => x.UserTaskId == userTaskId && x.UserId == userId).FirstAsync();

                if (data.NewStatus == 4) //4 means wants to remove task
                {
                    _db.UserTasks.Remove(userTask);
                }
                else
                {
                    userTask.Status = data.NewStatus;

                    if (data.NewStatus == (int)UserTaskStatuses.Approved)
                    {
                        var commAmount = (userTask.Task.ProductPrice * (userTask.CommissionPercentage / 100)) * data.Quantity.GetValueOrDefault(1);

                        userTask.CommissionAmount = commAmount;
                        userTask.QuantityPurchased = data.Quantity.GetValueOrDefault(1);

                        var newWalletTransaction = new WalletTransactions()
                        {
                            UserId = userId,
                            Amount = commAmount,
                            WalletTransactionType = (int)WalletTransactionTypes.Commission,
                            Status = (int)WalletTransactionStatuses.Successful,
                            DateAdded = DateTime.UtcNow,
                        };

                        _db.WalletTransactions.Add(newWalletTransaction);

                        var user = userTask.User;

                        user.WalletAmount = user.WalletAmount + newWalletTransaction.Amount;

                        _db.Users.Update(user);
                    }
                    else //rejected
                    {
                        //do nothing
                    }

                    _db.UserTasks.Update(userTask);
                }

                await _db.SaveChangesAsync();

                return Ok(new APIJsonReturnObject(null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.InternalServerError, "There was a problem when trying to update task status. Please refresh and try again.")));
            }
        }

        [HttpPut("Users/{userId}/AddPopupMessage")]
        public async Task<IActionResult> AddPopupMessage(int userId, [FromBody] AdminAddPopupMessageDetails data)
        {
            var user = await _db.Users.Where(x => x.UserId == userId).FirstAsync();

            user.ShowPopupMessage = data.ShowPopupMessage;
            user.PopupMessageTitle = data.PopupMessageTitle;
            user.PopupMessage = data.PopupMessage;

            if (!data.ShowPopupMessage)
            {
                user.PopupMessageTitle = null;
                user.PopupMessage = null;
            }

            _db.Users.Update(user);

            await _db.SaveChangesAsync();

            return Ok(new APIJsonReturnObject(null));
        }


        [HttpPut("Tasks/{taskId}")]
        public async Task<IActionResult> SaveTaskDetails(int taskId, [FromBody] AdminTaskDetails data)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var task = new Tasks();

            if (taskId > 0)
            {
                task = await _db.Tasks.Where(x => x.TaskId == taskId).FirstAsync();

            }
            else
            {
                task.DateAdded = DateTime.UtcNow;
                task.Status = (int)TaskStatuses.Active;
            }

            task.TaskTitle = data.TaskTitle;
            task.ProductName = data.ProductName;
            task.ProductDescription = data.ProductDescription;
            task.ProductMainImageURL = data.ProductMainImageURL;
            task.ProductImagesURL = data.ProductImagesURL;
            task.ProductPrice = data.ProductPrice;
            task.ProductRating = data.ProductRating;
            task.StoreName = data.StoreName;
            task.StoreThumbnailURL = data.StoreThumbnailURL;
            task.Language = data.Language;
            task.TaskCategory = data.TaskCategory;
            task.Platform = data.Platform;
            task.Country = data.Country;

            if (taskId > 0)
            {
                _db.Tasks.Update(task);
            }
            else
            {
                _db.Tasks.Add(task);
            }

            await _db.SaveChangesAsync();

            return Ok(new APIJsonReturnObject(null));
        }


        [HttpGet("TasksDropdown")]
        public async Task<IActionResult> GetTasksDropdown(int country, int language)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var tasks = await _db.Tasks
            .Where(x => x.Country == country && x.Language == language)
            .OrderBy(x => x.TaskCategory)
            .ThenBy(x => x.TaskTitle)
            .ToListAsync();

            var dropdownOptions = new List<DropdownOptions>();

            foreach (var x in tasks)
            {
                var ddo = new DropdownOptions()
                {
                    Text = $"({((ShortFormTaskCategories)x.TaskCategory).GetDescription()}-{((ShortFormCountries)x.Country).GetDescription()}-{((ShortFormLanguages)x.Language).GetDescription()}) {x.TaskTitle}",
                    Id = x.TaskId
                };

                dropdownOptions.Add(ddo);
            }

            return Ok(new APIJsonReturnObject(dropdownOptions));
        }

        [HttpPut("Users/{userId}/FreezeAccount")]
        public async Task<IActionResult> FreezeUserAccount(int userId)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var users = await _db.Users.Where(x => x.UserId == userId).FirstAsync();

            if (users.AccountStatus == ((int)UserAccountStatuses.Frozen))
            {
                users.AccountStatus = (int)UserAccountStatuses.Active;
            }
            else
            {
                users.AccountStatus = (int)UserAccountStatuses.Frozen;
            }

            _db.Users.Update(users);

            await _db.SaveChangesAsync();

            return Ok(new APIJsonReturnObject(null));
        }

        [HttpPost("Users")]
        public async Task<IActionResult> AddNewUser([FromBody] AdminNewUserDetails data)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null)
                {
                    return ReturnUnauthorizedStatus();
                }

                var existingLogin = await _um.FindByNameAsync(data.LoginUsername);

                if (existingLogin != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new APIJsonReturnObject("Login username has been used. Please use another."));
                }

                var newUser = new UserLogins
                {
                    UserName = data.LoginUsername,
                    Email = data.LoginUsername
                };

                var createdUser = await CreateNewUserLoginAsync(newUser, data.Password);

                await AddUserRoleAsync(newUser, UserRoles.User);

                var user = new Users()
                {
                    UserLoginId = newUser.Id,
                    FullName = data.Name,
                    Email = data.LoginUsername,
                    Currency = data.Currency,
                    AccountStatus = (int)UserAccountStatuses.Active,
                    WalletAmount = data.WalletAmount,
                    DateAdded = DateTime.UtcNow,
                    Language = data.Language,
                    AdminStaffId = ca.AdminStaffId,
                    Country = data.Country
                };

                _db.Users.Add(user);

                if (data.WalletAmount > 0)
                {
                    var newWalletTransaction = new WalletTransactions()
                    {
                        User = user,
                        Amount = data.WalletAmount,
                        WalletTransactionType = (int)WalletTransactionTypes.Deposit,
                        Status = (int)WalletTransactionStatuses.Successful,
                        DateAdded = DateTime.UtcNow
                    };

                    _db.WalletTransactions.Add(newWalletTransaction);
                }

                await _db.SaveChangesAsync();

                return Ok(new APIJsonReturnObject(null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject("There was a problem when trying to create a new user. Please refresh and try again."));
            }
        }

        [HttpGet("UserLeads")]
        public async Task<IActionResult> GetUserLeads(int pageIndex, int pageSize, string sortField, string sortOrder, string? filterLeadName, int? filterLeadStatus, int? filterDateAdded, DateTime? filterDateAddedStart, DateTime? filterDateAddedEnd)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var userLeadsQuery = _db.UserLeads
            .OrderByDescending(x => x.DateAdded)
            .AsQueryable();

            switch (sortField)
            {
                case "leadName":
                    if (sortOrder == "asc")
                    {
                        userLeadsQuery = userLeadsQuery.OrderBy(x => x.Name);
                    }
                    else
                    {
                        userLeadsQuery = userLeadsQuery.OrderBy(x => x.Name);
                    }
                    break;
                case "leadStatus":
                    if (sortOrder == "asc")
                    {
                        userLeadsQuery = userLeadsQuery.OrderBy(x => x.LeadStatus);
                    }
                    else
                    {
                        userLeadsQuery = userLeadsQuery.OrderBy(x => x.LeadStatus);
                    }
                    break;
                case "dateAdded":
                    if (sortOrder == "asc")
                    {
                        userLeadsQuery = userLeadsQuery.OrderBy(x => x.DateAdded);
                    }
                    else
                    {
                        userLeadsQuery = userLeadsQuery.OrderBy(x => x.DateAdded);
                    }
                    break;
                default:
                    userLeadsQuery = userLeadsQuery.OrderBy(x => x.DateAdded);
                    break;
            }

            if (filterLeadName != null)
            {
                userLeadsQuery = userLeadsQuery.Where(x => x.Name.Contains(filterLeadName));
            }

            if (filterLeadStatus != null)
            {
                userLeadsQuery = userLeadsQuery.Where(x => x.LeadStatus == filterLeadStatus);
            }

            if (filterDateAdded != null)
            {
                var todayDate = DateTime.UtcNow.ToMalaysiaDateTime();
                var startDate = todayDate.StartOfDay();
                var endDate = todayDate.EndOfDay();

                switch (filterDateAdded)
                {
                    case (int)DateFilters.Today:
                        break;
                    case (int)DateFilters.Yesterday:
                        startDate = todayDate.AddDays(-1).StartOfDay();

                        break;
                    case (int)DateFilters.Last7Days:
                        startDate = todayDate.AddDays(-6).StartOfDay(); // Start date is 7 days ago

                        break;
                    case (int)DateFilters.ThisMonth:
                        startDate = startDate.AddDays(-1);

                        break;
                    case (int)DateFilters.LastMonth:
                        var firstDayOfLastMonth = todayDate.AddMonths(-1).FirstDayOfTheMonth().StartOfDay();
                        var lastDayOfLastMonth = todayDate.AddMonths(-1).LastDayOfTheMonth().EndOfDay();

                        startDate = firstDayOfLastMonth;
                        endDate = lastDayOfLastMonth;

                        break;
                    case (int)DateFilters.Custom:
                        startDate = filterDateAddedStart.GetValueOrDefault(DateTime.UtcNow).StartOfDay();
                        endDate = filterDateAddedEnd.GetValueOrDefault(DateTime.UtcNow).EndOfDay();

                        break;
                }

                userLeadsQuery = userLeadsQuery.Where(x => x.DateAdded >= startDate.FromMalaysiaDateTimeToUTC() && x.DateAdded < endDate.FromMalaysiaDateTimeToUTC());
            }

            var userLeads = await userLeadsQuery.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            var admins = await _db.AdminStaffs
            .Where(x => x.Status == (int)AdminStaffStatuses.Active)
            .OrderBy(x => x.FullName)
            .ToListAsync();

            var auld = new AdminUserLeadsList()
            {
                UserLeadsList = new List<AdminUserLeadsListDetails>(),
                ItemsCount = await userLeadsQuery.CountAsync(),
                DateAddedOptions = Utility.GetEnumAsDropdownOptions(typeof(DateFilters)),
                LeadStatusOptions = Utility.GetEnumAsDropdownOptions(typeof(LeadStatuses)),
                AdminOptions = new List<DropdownOptions>(),
                LanguageOptions = Utility.GetEnumAsDropdownOptions(typeof(Languages))
            };

            foreach (var x in admins)
            {
                var ddo = new DropdownOptions()
                {
                    Id = x.AdminStaffId,
                    Text = x.FullName
                };

                auld.AdminOptions.Add(ddo);
            }

            foreach (var x in userLeads)
            {
                var newUserListDetails = new AdminUserLeadsListDetails()
                {
                    UserLeadId = x.UserLeadId,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    DateAdded = x.DateAdded.ToMalaysiaDateTime().ToString("dd/MM/yyyy hh:mmtt"),
                    Country = ((Countries)x.Country).GetDescription(),
                    LeadStatus = ((LeadStatuses)x.LeadStatus).GetDescription()
                };

                auld.UserLeadsList.Add(newUserListDetails);
            }

            return Ok(new APIJsonReturnObject(auld));
        }

        [HttpPut("UserLeads/ConvertToUser")]
        public async Task<IActionResult> ConvertLeadToUser([FromBody] AdminLeadConvertToUserRequest data)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null)
                {
                    return ReturnUnauthorizedStatus();
                }

                var userLeads = await _db.UserLeads
                .Where(x => data.UserLeadIds.Contains(x.UserLeadId) && x.LeadStatus == (int)LeadStatuses.Lead)
                .ToListAsync();

                foreach (var userLead in userLeads)
                {
                    var existingLogin = await _um.FindByNameAsync(userLead.Email);

                    if (existingLogin != null)
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest, new APIJsonReturnObject("Login username has been used. Please use another."));
                    }

                    var newUser = new UserLogins
                    {
                        UserName = userLead.Email,
                        Email = userLead.Email
                    };

                    var createdUser = await CreateNewUserLoginAsync(newUser, data.Password);

                    await AddUserRoleAsync(newUser, UserRoles.User);

                    var user = new Users()
                    {
                        UserLoginId = newUser.Id,
                        FullName = userLead.Name,
                        Email = userLead.Email,
                        AccountStatus = (int)UserAccountStatuses.Active,
                        WalletAmount = data.WalletAmount,
                        DateAdded = DateTime.UtcNow,
                        Language = data.Language,
                        AdminStaffId = data.AdminStaffId,
                        Country = userLead.Country
                    };

                    if (userLead.Country == (int)Countries.Malaysia)
                    {
                        user.Currency = (int)CurrencyTypes.MYR;
                        user.Language = (int)Languages.English;
                    }
                    else if (userLead.Country == (int)Countries.Indonesia)
                    {
                        user.Currency = (int)CurrencyTypes.Rupiah;

                    }
                    else if (userLead.Country == (int)Countries.India)
                    {
                        user.Currency = (int)CurrencyTypes.Rupee;
                    }


                    _db.Users.Add(user);

                    if (data.WalletAmount > 0)
                    {
                        var newWalletTransaction = new WalletTransactions()
                        {
                            User = user,
                            Amount = data.WalletAmount,
                            WalletTransactionType = (int)WalletTransactionTypes.Deposit,
                            Status = (int)WalletTransactionStatuses.Successful,
                            DateAdded = DateTime.UtcNow
                        };

                        _db.WalletTransactions.Add(newWalletTransaction);
                    }

                    userLead.LeadStatus = (int)LeadStatuses.Assigned;

                    _db.UserLeads.Update(userLead);

                    await _db.SaveChangesAsync();

                }

                return Ok(new APIJsonReturnObject(null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject("There was a problem when trying to create a new user. Please refresh and try again."));
            }
        }

        [HttpPost("Users/{userId}/AddTask")]
        public async Task<IActionResult> AddUserTask(int userId, [FromBody] AdminAddTaskDetails data)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null)
                {
                    return ReturnUnauthorizedStatus();
                }

                var task = await _db.Tasks.Where(x => x.TaskId == data.TaskId).FirstAsync();

                var newTask = new UserTasks()
                {
                    UserId = userId,
                    TaskId = data.TaskId,
                    DateAdded = DateTime.UtcNow,
                    Status = (int)UserTaskStatuses.NewTask,
                    CommissionPercentage = data.Commission
                };

                _db.UserTasks.Add(newTask);

                await _db.SaveChangesAsync();

                return Ok(new APIJsonReturnObject(null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject("There was a problem when trying to add task to user. Please refresh and try again."));
            }
        }

        [HttpPut("Users/{userId}/Wallet/{walletTransactionId}/Update")]
        public async Task<IActionResult> UpdateWalletTransaction(int userId, int walletTransactionId, [FromBody] AdminUpdateStatusRequest data)
        {
            var walletTransaction = await _db.WalletTransactions
            .Include(x => x.User)
            .Where(x => x.UserId == userId && x.WalletTransactionId == walletTransactionId)
            .FirstAsync();

            var user = walletTransaction.User;

            walletTransaction.Status = data.NewStatus;

            _db.WalletTransactions.Update(walletTransaction);

            if (data.NewStatus == (int)WalletTransactionStatuses.Successful)
            {
                user.WalletAmount = user.WalletAmount - walletTransaction.Amount;

                _db.Users.Update(user);
            }

            await _db.SaveChangesAsync();

            return Ok(new APIJsonReturnObject(null));
        }

        [HttpPost("Users/{userId}/AddWalletCredit")]
        public async Task<IActionResult> AddUserWalletCredit(int userId, [FromBody] AdminAddCreditDetails data)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null)
                {
                    return ReturnUnauthorizedStatus();
                }

                var user = await _db.Users
                .Where(x => x.UserId == userId)
                .FirstAsync();

                user.WalletAmount = user.WalletAmount + data.Amount;

                _db.Users.Update(user);

                var addWalletTransaction = new WalletTransactions()
                {
                    UserId = user.UserId,
                    Amount = data.Amount,
                    WalletTransactionType = data.WalletTransactionType,
                    Status = (int)WalletTransactionStatuses.Successful,
                };

                if (data.CustomDateAdded == null)
                {
                    addWalletTransaction.DateAdded = DateTime.UtcNow;
                }
                else
                {
                    addWalletTransaction.DateAdded = data.CustomDateAdded.Value.FromMalaysiaDateTimeToUTC();
                }

                _db.WalletTransactions.Add(addWalletTransaction);

                await _db.SaveChangesAsync();

                return Ok(new APIJsonReturnObject(null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject("There was a problem when trying to add credit to user. Please refresh and try again."));
            }
        }

        [HttpPost("Staffs")]
        public async Task<IActionResult> AddNewStaff([FromBody] AdminNewStaffRequest data)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null && ca.AdminStaffType == (int)AdminStaffTypes.Staff)
                {
                    return ReturnUnauthorizedStatus();
                }

                var existingLogin = await _um.FindByNameAsync(data.LoginUsername);

                if (existingLogin != null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new APIJsonReturnObject("Login username has been used. Please use another."));
                }

                var newUser = new UserLogins
                {
                    UserName = data.LoginUsername,
                    Email = data.LoginUsername
                };

                var createdUser = await CreateNewUserLoginAsync(newUser, data.Password);

                await AddUserRoleAsync(newUser, UserRoles.Admin);

                var newAdminStaff = new AdminStaffs()
                {
                    UserLoginId = newUser.Id,
                    FullName = data.Name,
                    Status = (int)AdminStaffStatuses.Active,
                    DateAdded = DateTime.UtcNow,
                    RequirePasswordChange = true
                };

                if (ca.AdminStaffType == (int)AdminStaffTypes.FullAdmin)
                {
                    newAdminStaff.AdminStaffType = data.AdminType.Value;
                }
                else
                {
                    newAdminStaff.AdminStaffType = (int)AdminStaffTypes.Staff;
                }

                _db.AdminStaffs.Add(newAdminStaff);

                await _db.SaveChangesAsync();

                var newLeaderAdminStaffs = new LeaderAdminStaffs()
                {
                    AssociatedAdminStaffId = newAdminStaff.AdminStaffId
                };

                if (data.AdminLeaderStaffId != null)
                {
                    newLeaderAdminStaffs.AdminStaffId = data.AdminLeaderStaffId.Value;
                }
                else
                {
                    newLeaderAdminStaffs.AdminStaffId = ca.AdminStaffId;
                }

                _db.LeaderAdminStaffs.Add(newLeaderAdminStaffs);

                await _db.SaveChangesAsync();

                return Ok(new APIJsonReturnObject(null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject("There was a problem when trying to add credit to user. Please refresh and try again."));
            }
        }

        //POST: api/Admin/SignIn
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> UserSignIn([FromBody] UserSignInRequest data)
        {
            try
            {
                var user = await _um.FindByNameAsync(data.Username.Trim());

                if (user != null)
                {
                    IList<string> loginUserRoles = await _um.GetRolesAsync(user);

                    if (new List<string> { UserRoles.Admin.ToString(), UserRoles.FullAdmin.ToString() }.Intersect(loginUserRoles).Any())
                    {
                        var loginUser = await _sm.PasswordSignInAsync(data.Username, data.Password, true, false);

                        if (loginUser.Succeeded)
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

                            return Ok(new APIJsonReturnObject(ca));
                        }
                        else if (loginUser.IsLockedOut)
                        {
                            return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.Unauthorized, "Account has been locked out. Please contact support.")));
                        }
                        else
                        {
                            return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.Forbidden, "Incorrect email and password combination")));
                        }
                    }
                }

                return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.BadRequest, "Incorrect email and password combination")));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.InternalServerError, "A problem occured when trying to sign you in. Please refresh the page and try again.")));
            }
        }

        [HttpPut("Users/{userId}/ResetPassword")]
        public async Task<IActionResult> ResetUserPassword(int userId)
        {
            try
            {
                var ca = CurrentAdmin;

                if (ca == null)
                {
                    return ReturnUnauthorizedStatus();
                }

                var userQuery = _db.Users
                .AsQueryable();

                if (ca.AdminStaffType == (int)AdminStaffTypes.Staff)
                {
                    userQuery = userQuery.Where(x => x.AdminStaffId == ca.AdminStaffId);
                }

                var user = await userQuery.FirstOrDefaultAsync();

                if (user == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new APIJsonReturnObject("User not found. Please select a different user."));
                }

                var userLogin = await _um.FindByIdAsync(user.UserLoginId.ToString());

                var passwordResetToken = await _um.GeneratePasswordResetTokenAsync(userLogin);

                var reset = await _um.ResetPasswordAsync(userLogin, passwordResetToken, "tempPassword123");

                return Ok(new APIJsonReturnObject(null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.InternalServerError, "A problem occured when trying to sign you in. Please refresh the page and try again.")));
            }
        }

        [HttpGet("DemoTasks/{taskId}")]
        public async Task<IActionResult> GetDemoTaskDetails(int taskId)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var task = await _db.Tasks
                .Where(x => x.TaskId == taskId)
                .FirstAsync();


            var comments = await _db.ProductReviews
                .OrderBy(x => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            Random rnd = new Random();

            var productImages = new List<string>();

            productImages.Add(task.ProductMainImageURL);
            productImages.AddRange(task.ProductImagesURL.Split(",").ToList());

            var currencySymbol = Global.Utility.GetCurrencySymbol(CurrentUser.Currency);

            var utd = new UserTaskDetails()
            {
                TaskTitle = task.TaskTitle,
                ProductName = task.ProductName,
                ProductDescription = task.ProductDescription,
                ProductMainImageURL = task.ProductMainImageURL,
                ProductImagesURL = productImages,
                ProductPrice = $"{currencySymbol}{task.ProductPrice:0.00}",
                ProductRating = task.ProductRating,
                StoreName = task.StoreName,
                StoreThumbnailURL = task.StoreThumbnailURL,
                ProductReviewsList = new List<ProductReviewListDetails>(),
                ProductRatingCount = rnd.Next(80, 90),
                ProductSoldCount = rnd.Next(100, 110),
                ShippingFee = $"{currencySymbol}0.00",
                Vouchers = new List<string>() { $"{currencySymbol}1.00 OFF", $"{currencySymbol}2.00 OFF", $"{currencySymbol}3.00 OFF" },
                QuantityRemaining = rnd.Next(200, 300)
            };

            var orderdComments = comments.OrderByDescending(x => x.DateAdded).ToList();

            foreach (var x in orderdComments)
            {
                var nprd = new ProductReviewListDetails()
                {
                    ReviewerName = x.ReviewerName,
                    Rating = x.Rating * 100 / 5,
                    Comment = x.Comment,
                    DateAdded = $"{x.DateAdded.ToLocalTime().ToString("yyyy-MM-dd HH:mm")} | Variation: Standard",
                };

                utd.ProductReviewsList.Add(nprd);
            }

            return Ok(new APIJsonReturnObject(utd));
        }

        //PUT: api/Admin/UpdatePassword
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdateAdminPassword([FromBody] UserLoginRequest data)
        {
            var ca = CurrentAdmin;

            if (ca == null)
            {
                return ReturnUnauthorizedStatus();
            }

            var adminStaff = await _db.AdminStaffs
                .Where(x => x.AdminStaffId == ca.AdminStaffId)
                .FirstAsync();

            var user = await _um.FindByIdAsync(adminStaff.UserLoginId.ToString());

            var passwordResetToken = await _um.GeneratePasswordResetTokenAsync(user);

            var reset = await _um.ResetPasswordAsync(user, passwordResetToken, data.Password);

            adminStaff.RequirePasswordChange = false;

            _db.AdminStaffs.Update(adminStaff);

            await _db.SaveChangesAsync();

            return Ok(new APIJsonReturnObject(null));
        }

        //POST: api/Users/SignOut
        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await _sm.SignOutAsync();

            return Ok(new APIJsonReturnObject(null));
        }
    }
}