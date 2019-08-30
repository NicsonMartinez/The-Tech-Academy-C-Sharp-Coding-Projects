using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarInsuranceQuoteDrill.Models
{
    //NOTE: Here I devoted an entire class to create a 'GenerateQuote' method, that the
    //      HomeController class can call. It is declared public so ouside classes can have 
    //      access to this class.
    public class QuoteGenerator
    {
        //NOTE: Here I declaired the 'GenerateQuote', public so that classes outside of the 'QuoteGenerator'
        //      class can call it. It was also declared static because this method isn't meant to be called
        //      from a 'QuoteGenerator' object instantiation (We wouldn't declare an instance of this object
        //      because it doesn't change states, it simply has a behaviour). Instead we are would call this 
        //      method by the class name, eg. 'QuoteGenerator.GenerateQuote(user)' ans that would return a 
        //      decimal.
        public static decimal GenerateQuote(UserQuote user)
        {
            int userAge;
            double quoteTotal = 0;
            double baseTotal = 50;

            //NOTE: All 'user.DateOfBirth', 'user.CarYear', and 'user.SpeedingTicketNum' are object properties
            //      that were declared 'Nullable' which means normally those types cannot be equal to 'null',
            //      but we allowed them to be (by declaring them 'Nullable'). Below you will see that I used 
            //      the 'null-coalescing' operator '??' to help c# know what to do with nullable data types.

            //NOTE: The null-coalescing operator ?? returns the value of its left-hand operand if it 
            //      isn't null; otherwise, it evaluates the right-hand operand and returns its result.
            DateTime userDateOfBirth = user.DateOfBirth ?? DateTime.Now;

            if (userDateOfBirth == DateTime.Now)
            {
                throw new Exception();
            }
            else
            {
                userAge = CalculateAge(userDateOfBirth);
            }

            if (userAge < 18) //If the user is under 18, add $100 to the monthly total.
            {
                quoteTotal = baseTotal + 100;
            }
            else if (userAge > 18 && userAge < 25) //If the user is under 25 (but over 18), add $25 to the monthly total.
            {
                quoteTotal = baseTotal + 25;
            }
            else if (userAge > 100) //If the user is over 100, add $25 to the monthly total.
            {
                quoteTotal = baseTotal + 25;
            }

            int userCarYear = user.CarYear ?? 0;
            //1769 CUGNOT STEAMER is the oldest recognized automobile lol.
            //Don't skip 2 years ahead, we're not in the future.
            if (userCarYear <= 1768 || userCarYear > DateTime.Now.Year + 1)
            {
                throw new Exception();
            }
            else if (userCarYear < 2000 || userCarYear > 2015) //If the car's year is before 2000 or after 2015, add $25 to the monthly total.
            {
                quoteTotal = +25;
            }

            //If the car's Make is a Porsche, add $25 to the price.
            if (user.CarMake == "Porsche")
            {
                quoteTotal = +25;
            }

            //If the car's Make is a Porsche and its model is a 911 Carrera, add an additional $25 to the price.
            if (user.CarMake == "Porsche" && user.CarModel == "911 Carrera")
            {
                quoteTotal = +25;
            }

            //Add $10 to the monthly total for every speeding ticket the user has
            int userSpeedingTicketNum = user.SpeedingTicketNum ?? 0;
            if (userSpeedingTicketNum < 0)
            {
                userSpeedingTicketNum = 0;
            }
            quoteTotal = userSpeedingTicketNum * 10;


            //If the user has ever had a DUI, add 25 % to the total.
            if (user.DUI == "Yes")
            {
                quoteTotal = +(quoteTotal * .25);
            }

            //If it's full coverage, add 50% to the total.
            if (user.FullCoverageOrLiability == "Full Coverage")
            {
                quoteTotal = +(quoteTotal * .50);
            }

            decimal quoteTotalInDecimal = Convert.ToDecimal(quoteTotal);

            return quoteTotalInDecimal;
        }

        //NOTE: Here I created a method, 'CalculateAge' and made it private because it is only
        //      meant to be used in this class ('QuoteGenerator'). It was declared static becuase
        //      it is only meant to be called by this class and not an instantiation of this class..
        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }
}