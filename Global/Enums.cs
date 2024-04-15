using System.ComponentModel;

namespace BoostifySolution.Global
{
    namespace Enums
    {
        namespace Common
        {
            public enum UserRoles
            {
                [Description("Admin")]
                Admin = 0,
                [Description("Full Admin")]
                FullAdmin = 1,
                [Description("User")]
                User = 2
            }

            public enum AdminStaffTypes
            {
                [Description("Staff")]
                Staff = 0,
                [Description("Full Admin")]
                FullAdmin = 1,
                [Description("Leader")]
                Leader = 2
            }

            public enum AdminStaffStatuses
            {
                [Description("Disabled")]
                Disabled = 0,
                [Description("Active")]
                Active = 1
            }

            public enum DateFilters
            {
                [Description("Today")]
                Today = 0,
                [Description("Yesterday")]
                Yesterday = 1,
                [Description("Last 7 Days")]
                Last7Days = 2,
                [Description("This Month")]
                ThisMonth = 3,
                [Description("Last Month")]
                LastMonth = 4,
                [Description("Custom Range")]
                Custom = 5
            }

            public enum CurrencyTypes
            {
                [Description("RM")]
                MYR = 0,
                [Description("¥")]
                Yen = 1,
                [Description("Rp")]
                Rupiah = 2
            }

            public enum Languages
            {
                [Description("English")]
                English = 0,
                [Description("Malay")]
                Malay = 1,
                [Description("Chinese")]
                Chinese = 2,
                [Description("Japanese")]
                Japanese = 3
            }

            public enum ShortFormLanguages
            {
                [Description("Eng")]
                English = 0,
                [Description("Ms")]
                Malay = 1,
                [Description("Chi")]
                Chinese = 2,
                [Description("Jp")]
                Japan = 3
            }

            public enum UserAccountStatuses
            {
                [Description("Frozen")]
                Frozen = 0,
                [Description("Active")]
                Active = 1
            }

            public enum WalletTransactionStatuses
            {
                [Description("Pending")]
                Pending = 0,
                [Description("Successful")]
                Successful = 1,
                [Description("Unsuccessful")]
                Unsuccessful = 2
            }

            public enum WalletTransactionTypes
            {
                [Description("Deposit")]
                Deposit = 0,
                [Description("Withdraw")]
                Withdraw = 1,
                [Description("Commission")]
                Commission = 2
            }

            public enum UserTaskStatuses
            {
                [Description("New Task")]
                NewTask = 0,
                [Description("Pending")]
                Pending = 1,
                [Description("Approved")]
                Approved = 2,
                [Description("Rejected")]
                Rejected = 3,
                [Description("Submitted")]
                Submitted = 4,
            }

            public enum TaskStatuses
            {
                [Description("Inactive")]
                Inactive = 0,
                [Description("Active")]
                Active = 1
            }
            public enum TaskCategories
            {
                [Description("Normal")]
                Normal = 0,
                [Description("Overtime")]
                Overtime = 1,
                [Description("Frozen Account")]
                FrozenAccount = 2,
            }

            public enum ShortFormTaskCategories
            {
                [Description("Nl")]
                Normal = 0,
                [Description("Ot")]
                Overtime = 1,
                [Description("Fz")]
                FrozenAccount = 2,
            }

            public enum Platforms
            {
                [Description("Shopee")]
                Shopee = 0,
                [Description("Lazada")]
                Lazada = 1,
                [Description("Amazon")]
                Amazon = 2,
            }

            public enum Countries
            {
                [Description("Malaysia")]
                Malaysia = 0,
                [Description("Indonesia")]
                Indonesia = 1,
                [Description("Japan")]
                Japan = 2,
            }

            public enum ShortFormCountries
            {
                [Description("My")]
                Malaysia = 0,
                [Description("Indo")]
                Indonesia = 1,
                [Description("Jp")]
                Japan = 2,
            }

            public enum LeadStatuses
            {
                [Description("Lead")]
                Lead = 0,
                [Description("Assigned")]
                Assigned = 1
            }
        }
    }
}
