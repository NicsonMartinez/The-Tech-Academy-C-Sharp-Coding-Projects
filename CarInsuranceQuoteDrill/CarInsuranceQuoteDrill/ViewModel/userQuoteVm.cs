using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarInsuranceQuoteDrill.ViewModel
{
    public class UserQuoteVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<decimal> GeneratedQuote { get; set; }
        public Nullable<System.DateTime> DateAndTime { get; set; }
    }
}