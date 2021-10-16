using System;
using System.Collections.Generic;
using System.Text;

namespace ManaberManagement.Utilities
{
    public class SystemConstants
    {
        public const string MainConnectionString = "MemberManagementConnext";
        public const string CartSession = "CartSession";

        public class AppSettings
        {
            public const string Token = "JWT";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 4;
            public const int NumberOfLatestProducts = 6;
        }

        public class ProductConstants
        {
            public const string NA = "N/A";
        }
    }
}
