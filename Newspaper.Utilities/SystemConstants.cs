﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Utilities
{
    public class SystemConstants
    {
        public const string MainConnectionString = "NewspaperConnect";
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
