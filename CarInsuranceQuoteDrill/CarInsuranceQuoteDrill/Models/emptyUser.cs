using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarInsuranceQuoteDrill.Models
{
    public class EmptyUser
    {
        public static UserQuote EmptyUserMethod()
        {
            UserQuote user = new UserQuote();
            user.FirstName = null;
            user.LastName = null;
            user.EmailAddress = null;
            user.DateOfBirth = null;
            user.GeneratedQuote = null;
            user.CarYear = null;
            user.CarModel = null;
            user.CarModel = null;
            user.DUI = null;
            user.SpeedingTicketNum = null;
            user.FullCoverageOrLiability = null;
            user.GeneratedQuote = null;

            return user;
        }
        
    }
}