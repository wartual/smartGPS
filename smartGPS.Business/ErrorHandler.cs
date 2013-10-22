using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business
{
    public class ErrorHandler
    {
        public enum SignUpErrors { Database, UsernameAlreadyTaken, Success }
        public enum SignInErrors { Database, Failed, Success }
    }
}