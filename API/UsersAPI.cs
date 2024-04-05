using System.Net;
using MarketplacBoostifySolutione.Models.Global;
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
using BoostifySolution.Models.Users;
using boostifysolution1.Models.Users;
using boostifysolution1.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BoostifySolution.API
{
    [Route("api/Users")]
    public class UsersAPI : BaseAPI
    {
        public UsersAPI(ApplicationDbContext db, UserManager<UserLogins> um, SignInManager<UserLogins> sm, RoleManager<IdentityRole<int>> rm, IStringLocalizer<BaseAPI> l, IHttpContextAccessor cx) : base(db, um, sm, rm, l, cx)
        {
        }

        [HttpGet("{userId}/Wallet")]
        public async Task<IActionResult> GetWalletDetails(int userId)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            var user = await _db.Users.Where(x => x.UserId == userId).FirstAsync();

            var walletTransactions = await _db.WalletTransactions
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateAdded)
                .ToListAsync();

            var currencySymbol = "RM";

            switch (user.Currency)
            {
                case (int)CurrencyTypes.MYR:
                    currencySymbol = "RM";
                    break;
                case (int)CurrencyTypes.Yen:
                    currencySymbol = "₹";
                    break;
            }

            var uwd = new UserWalletDetails()
            {
                WalletAmount = $"{currencySymbol}{user.WalletAmount:0.00}",
                WalletAmountDecimal = user.WalletAmount,
                WalletTransactions = new List<WalletTransactionListDetails>(),
                WalletLastUpdated = DateTime.Now.ToString("dd/MM/yyyy hh:mmtt"),
                ShowPopupMessage = user.ShowPopupMessage,
                PopupMessageTitle = user.PopupMessageTitle,
                PopupMessage = user.PopupMessage
            };

            foreach (var x in walletTransactions)
            {
                var amountSymbol = "+";

                if (x.WalletTransactionType == (int)WalletTransactionTypes.Withdraw)
                {
                    amountSymbol = "-";
                }

                var wtld = new WalletTransactionListDetails()
                {
                    WalletTransactionId = x.WalletTransactionId,
                    Amount = $"{amountSymbol}{currencySymbol}{x.Amount:0.00}",
                    Type = ((WalletTransactionTypes)x.WalletTransactionType).GetDescription(),
                    Status = ((WalletTransactionStatuses)x.Status).GetDescription(),
                    DateAdded = x.DateAdded.ToLocalTime().ToString("dd/MM/yyyy hh:mmtt")
                };

                uwd.WalletTransactions.Add(wtld);
            }

            return Ok(new APIJsonReturnObject(uwd));
        }

        [HttpPost("{userId}/Wallet/Deposit")]
        public async Task<IActionResult> DepositWallet(int userId)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            return Ok(new APIJsonReturnObject("Unable to process deposit. Please contact your support admin"));
        }

        [HttpPost("{userId}/Wallet/Withdraw")]
        public async Task<IActionResult> WithdrawWallet(int userId, [FromBody] UserWithdrawWalletDetails data)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            try
            {
                var walletTransactions = new WalletTransactions()
                {
                    UserId = userId,
                    Amount = data.WithdrawAmount,
                    WalletTransactionType = (int)WalletTransactionTypes.Withdraw,
                    Status = (int)WalletTransactionStatuses.Pending,
                    DateAdded = DateTime.UtcNow,
                    BankName = data.BankName,
                    BankAccountNumber = data.BankAccountNumber,
                    AccountHolderName = data.AccountHolderName
                };

                _db.WalletTransactions.Add(walletTransactions);

                await _db.SaveChangesAsync();

                var currencySymbol = Global.Utility.GetCurrencySymbol(CurrentUser.Currency);


                var wtld = new WalletTransactionListDetails()
                {
                    WalletTransactionId = walletTransactions.WalletTransactionId,
                    Amount = $"-{currencySymbol}{walletTransactions.Amount:0.00}",
                    Type = ((WalletTransactionTypes)walletTransactions.WalletTransactionType).GetDescription(),
                    Status = ((WalletTransactionStatuses)walletTransactions.Status).GetDescription(),
                    DateAdded = walletTransactions.DateAdded.ToLocalTime().ToString("dd/MM/yyyy hh:mmtt")
                };

                return Ok(new APIJsonReturnObject(wtld));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.InternalServerError, "A problem occured when trying to withdraw.")));
            }
        }

        [HttpGet("{userId}/Home")]
        public async Task<IActionResult> GetHomeDetails(int userId)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            var tasks = await _db.UserTasks
                .Include(x => x.Task)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateAdded)
                .ToListAsync();

            var user = await _db.Users.Where(x => x.UserId == userId).FirstAsync();

            var currencySymbol = Global.Utility.GetCurrencySymbol(user.Currency);

            var uhd = new UserHomeDetails()
            {
                WalletAmount = $"{currencySymbol}{user.WalletAmount:0.00}",
                WalletLastUpdated = DateTime.Now.ToString("dd/MM/yyyy hh:mmtt"),
                UserName = user.FullName,
                UserTasks = new List<UserTasksListDetails>(),
                CompletedUserTasks = new List<UserTasksListDetails>(),
                UserSubmittedTasks = new List<UserTasksListDetails>(),
                AccountStatus = user.AccountStatus
            };


            var newTasks = tasks.Where(x => x.Status == (int)UserTaskStatuses.NewTask).ToList();

            var submittedTaskStatuses = new List<int> { (int)UserTaskStatuses.Pending, (int)UserTaskStatuses.Submitted };
            var submittedTasks = tasks.Where(x => submittedTaskStatuses.Contains(x.Status)).ToList();

            var completedTaskStatuses = new List<int>() { (int)UserTaskStatuses.Approved, (int)UserTaskStatuses.Rejected };
            var completedTasks = tasks.Where(x => completedTaskStatuses.Contains(x.Status)).ToList();

            foreach (var x in newTasks)
            {
                var utld = new UserTasksListDetails()
                {
                    UserTaskId = x.UserTaskId,
                    TaskTitle = x.Task.TaskTitle,
                    ProductName = x.Task.ProductName,
                    Status = ((UserTaskStatuses)x.Status).GetDescription(),
                    DateAdded = x.DateAdded.ToLocalTime().ToString("dd/MM/yyyy"),
                    TaskAmount = $"{x.CommissionPercentage:0}%",
                    Platform = x.Task.Platform,
                    Language = x.Task.Language
                };

                uhd.UserTasks.Add(utld);
            }

            foreach (var x in submittedTasks)
            {
                var utld = new UserTasksListDetails()
                {
                    UserTaskId = x.UserTaskId,
                    TaskTitle = x.Task.TaskTitle,
                    ProductName = x.Task.ProductName,
                    Status = ((UserTaskStatuses)x.Status).GetDescription(),
                    DateAdded = x.DateAdded.ToLocalTime().ToString("dd/MM/yyyy"),
                    TaskAmount = $"{x.CommissionPercentage:0}%",
                    Platform = x.Task.Platform,
                    Language = x.Task.Language
                };

                uhd.UserSubmittedTasks.Add(utld);
            }

            foreach (var x in completedTasks)
            {
                var utld = new UserTasksListDetails()
                {
                    UserTaskId = x.TaskId,
                    TaskTitle = x.Task.TaskTitle,
                    ProductName = x.Task.ProductName,
                    Status = ((UserTaskStatuses)x.Status).GetDescription(),
                    DateAdded = x.DateAdded.ToLocalTime().ToString("dd/MM/yyyy"),
                    TaskAmount = $"{x.CommissionPercentage:0}%"
                };

                uhd.CompletedUserTasks.Add(utld);
            }

            return Ok(new APIJsonReturnObject(uhd));
        }

        [HttpGet("{userId}/Profile")]
        public async Task<IActionResult> GetProfileDetails(int userId)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            var user = await _db.Users
            .Include(x => x.AdminStaff)
            .Where(x => x.UserId == userId)
            .FirstAsync();

            var upd = new UserProfileDetails()
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Language = user.Language,
                LanguageOptions = Global.Utility.GetEnumAsDropdownOptions(typeof(Languages)),
                Currency = ((CurrencyTypes)user.Currency).GetDescription(),
                AccountStatus = ((UserAccountStatuses)user.AccountStatus).GetDescription(),
                AccountAdmin = user.AdminStaff.FullName,
                DateJoined = user.DateAdded.ToLocalTime().ToString("dd/MM/yyyy")
            };

            return Ok(new APIJsonReturnObject(upd));
        }

        [HttpPut("{userId}/Profile")]
        public async Task<IActionResult> SaveProfileDetails(int userId, [FromBody] UserProfileDetails data)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            var users = await _db.Users.Where(x => x.UserId == userId).FirstAsync();

            users.FullName = data.FullName;
            users.PhoneNumber = data.PhoneNumber;
            users.Email = data.Email;

            _db.Users.Update(users);

            await _db.SaveChangesAsync();

            return Ok(new APIJsonReturnObject(null));
        }

        [HttpGet("{userId}/UserTasks/{userTaskid}")]
        public async Task<IActionResult> GetUserTaskDetails(int userId, int userTaskId)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            var acceptedTaskStatuses = new List<int> { (int)UserTaskStatuses.NewTask, (int)UserTaskStatuses.Pending, (int)UserTaskStatuses.Submitted };

            var userTask = await _db.UserTasks
                .Include(x => x.Task)
                .Include(x => x.User)
                .Where(x => x.UserId == userId && x.UserTaskId == userTaskId && acceptedTaskStatuses.Contains(x.Status))
                .FirstOrDefaultAsync();

            if (userTask == null)
            {
                return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.BadRequest, "Task has already been completed")));
            }

            userTask.Status = (int)UserTaskStatuses.Pending;

            _db.UserTasks.Update(userTask);

            await _db.SaveChangesAsync();

            var commentsQuery = _db.ProductReviews   
                .Take(10);

            var task = userTask.Task;

            if (task.Platform == (int)Platforms.Amazon)
            {
                commentsQuery = commentsQuery.Where(x => x.Language == (int)Languages.Japanese);
            }

            commentsQuery = commentsQuery.OrderBy(x => Guid.NewGuid());

            var comments = await commentsQuery.ToListAsync();

            Random rnd = new Random();

            var productImages = new List<string>();

            productImages.Add(task.ProductMainImageURL);
            productImages.AddRange(task.ProductImagesURL.Split(",").ToList());

            var currencySymbol = Global.Utility.GetCurrencySymbol(CurrentUser.Currency);

            var productPrice = $"{currencySymbol}{task.ProductPrice:0.00}";

            if (task.Country == (int)Countries.Japan)
            {
                productPrice = $"{task.ProductPrice:N0}";
            }

            var beforeDiscountPrice = task.ProductPrice / 0.75M;

            var utd = new UserTaskDetails()
            {
                TaskTitle = task.TaskTitle,
                ProductName = task.ProductName,
                ProductDescription = task.ProductDescription,
                ProductMainImageURL = task.ProductMainImageURL,
                ProductImagesURL = productImages,
                ProductPrice = productPrice,
                ProductRating = task.ProductRating,
                ProductRatingWidth = task.ProductRating * 100 / 5,
                StoreName = task.StoreName,
                StoreThumbnailURL = task.StoreThumbnailURL,
                ProductReviewsList = new List<ProductReviewListDetails>(),
                ProductRatingCount = rnd.Next(80, 90),
                ProductSoldCount = rnd.Next(100, 110),
                ShippingFee = $"{currencySymbol}0.00",
                Vouchers = new List<string>() { $"{currencySymbol}1.00 OFF", $"{currencySymbol}2.00 OFF", $"{currencySymbol}3.00 OFF" },
                QuantityRemaining = rnd.Next(200, 300),
                BeforeDiscountPrice = $"{currencySymbol}{beforeDiscountPrice:0.00}",
                DiscountPercentage = "25%",
                SupportItemsList = new List<SupportItemListDetails>()
            };

            var orderdComments = comments.OrderByDescending(x => x.DateAdded).ToList();

            foreach (var x in orderdComments)
            {
                var nprd = new ProductReviewListDetails()
                {
                    ReviewerName = x.ReviewerName,
                    Rating = x.Rating * 100 / 5,
                    Comment = x.Comment,
                    DateAdded = $"{x.DateAdded.ToLocalTime().ToString("yyyy-MM-dd HH:mm")}",
                    Variation = "Standard"
                };

                utd.ProductReviewsList.Add(nprd);
            }

            if (task.Country == (int)Countries.Japan && task.Language == (int)Languages.Japanese)
            {
                var supportItems = await _db.SupportItems
                .OrderBy(x => Guid.NewGuid())
                .Take(7)
                .ToListAsync();

                foreach (var x in supportItems)
                {
                    var nsild = new SupportItemListDetails()
                    {
                        ProductName = x.ProductName,
                        ProductImageURL = x.ProductImageURL,
                        ProductRating = x.ProductRating,
                        ProductRatingCount = x.ProductRatingCount,
                        ProductRatingWidth = x.ProductRating * 100 / 5,
                        ProductPrice = $"{currencySymbol}{x.ProductPrice:N0}",
                    };

                    utd.SupportItemsList.Add(nsild);
                }
            }

            return Ok(new APIJsonReturnObject(utd));
        }

        [HttpPut("{userId}/UserTasks/{userTaskId}")]
        public async Task<IActionResult> UpdateUserTask(int userId, int userTaskId, [FromBody] UserSubmitTaskDetails data)
        {
            var cu = CurrentUser;

            if (cu == null && cu.UserId != userId)
            {
                return ReturnUnauthorizedStatus();
            }

            var acceptedTaskStatuses = new List<int> { (int)UserTaskStatuses.NewTask, (int)UserTaskStatuses.Pending, (int)UserTaskStatuses.Submitted };

            var userTask = await _db.UserTasks
               .Include(x => x.Task)
               .Include(x => x.User)
                .Where(x => x.UserId == userId && x.UserTaskId == userTaskId && acceptedTaskStatuses.Contains(x.Status))
               .FirstOrDefaultAsync();

            if (userTask == null)
            {
                return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.BadRequest, "Task has already been completed")));
            }

            userTask.Status = data.NewStatus;
            userTask.QuantityPurchased = data.Quantity;
            userTask.CompletedDateTime = DateTime.UtcNow;

            _db.UserTasks.Update(userTask);

            if (userTask.Task.TaskCategory == (int)TaskCategories.Overtime)
            {
                var user = await _db.Users.Where(x => x.UserId == userId).FirstAsync();

                user.ShowPopupMessage = true;
                user.PopupMessageTitle = "Unable to access wallet";
                user.PopupMessage = "There is a task that was not completed in the set time. Please contact your support admin.";

                _db.Users.Update(user);
            }
            else if (userTask.Task.TaskCategory == (int)TaskCategories.FrozenAccount)
            {
                var user = await _db.Users.Where(x => x.UserId == userId).FirstAsync();

                user.ShowPopupMessage = true;
                user.PopupMessageTitle = "Your account has been suspended";
                user.PopupMessage = "Your account has been frozen due to weird activity detected. Please contact your support admin.";

                _db.Users.Update(user);
            }


            await _db.SaveChangesAsync();

            return Ok(new APIJsonReturnObject(null));
        }

        //POST: api/Users/SignIn
        [HttpPost("SignIn")]
        public async Task<IActionResult> UserSignIn([FromBody] UserSignInRequest data)
        {
            try
            {
                var user = await _um.FindByNameAsync(data.Username.Trim());

                if (user != null)
                {
                    IList<string> loginUserRoles = await _um.GetRolesAsync(user);

                    if (new List<string> { UserRoles.User.ToString() }.Intersect(loginUserRoles).Any())
                    {
                        var loginUser = await _sm.PasswordSignInAsync(data.Username, data.Password, true, false);

                        if (loginUser.Succeeded)
                        {
                            var userEntity = await _db.Users
                            .Where(x => x.UserLoginId == user.Id)
                                .FirstAsync();

                            CurrentUserObj cu = new()
                            {
                                UserId = userEntity.UserId,
                                UserName = userEntity.FullName,
                                Currency = userEntity.Currency,
                                AccountStatus = userEntity.AccountStatus,
                            };

                            switch (userEntity.Language)
                            {
                                case (int)Languages.Malay:
                                    cu.Language = "ms-MY";
                                    break;
                                case (int)Languages.English:
                                    cu.Language = "en-GB";
                                    break;
                                case (int)Languages.Chinese:
                                    cu.Language = "zh";
                                    break;
                                case (int)Languages.Japanese:
                                    cu.Language = "ja-JP";
                                    break;
                            }

                            return Ok(new APIJsonReturnObject(cu));
                        }
                        else if (loginUser.IsLockedOut)
                        {
                            return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.Unauthorized, "Account has been locked out. Please contact support.")));
                        }
                        else
                        {
                            return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.Forbidden, "Incorrect email and password combination. Please try again.")));
                        }
                    }
                }

                return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.BadRequest, "Incorrect email and password combination. Please try again.")));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.InternalServerError, "A problem occured when trying to sign you in. Please refresh the page and try again.")));
            }
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> UserSignUp([FromBody] UserSignUpRequest data)
        {
            if (data.Name.IsNullOrEmpty() || data.PhoneNumber.IsNullOrEmpty() || data.Email.IsNullOrEmpty())
            {
                return BadRequest(new APIJsonReturnObject(null, new APIJsonReturnObject.ErrorObject(HttpStatusCode.BadRequest, "Please make sure all fields are filled before submitting")));
            }

            var newUserLead = new UserLeads()
            {
                Name = data.Name,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                Country = data.Country,
                DateAdded = DateTime.UtcNow,
                LeadStatus = (int)LeadStatuses.Lead
            };

            _db.UserLeads.Add(newUserLead);

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