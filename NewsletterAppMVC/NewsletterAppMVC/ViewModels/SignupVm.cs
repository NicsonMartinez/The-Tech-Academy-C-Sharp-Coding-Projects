using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsletterAppMVC.ViewModels
{
    //NOTE: A View Model class naming convention is to end it with 'Vm'. 
    public class SignupVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

    }
}