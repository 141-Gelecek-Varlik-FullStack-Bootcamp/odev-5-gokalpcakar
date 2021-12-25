using Icarus.DB.Entities.DataContext;
using System;
using System.Linq;

namespace Icarus.API.Services
{
    public class PrintWelcomeJob : IPrintWelcomeJob
    {
        public void PrintWelcome()
        {
            using(var context = new IcarusContext())
            {
                var users = context.User.Where(u => u.IsActive && !u.IsDeleted && u.Idate < DateTime.Now.AddDays(-1)).ToList();
                foreach (var user in users)
                {
                    //sendWelcomeMail(user.Email);
                    Console.WriteLine($"Welcome to the Icarus, Your Id:{user.Id} - Mail:{user.Email}!");
                }
            }
        }

        public void CleanUserTable()
        {
            using(var context = new IcarusContext())
            {
                var users = context.User.Where(u => !u.IsActive && u.IsDeleted);
                foreach (var user in users)
                    Console.WriteLine($" Removed user: {user.Email}. ({DateTime.Now})");
                context.User.RemoveRange(users);
                context.SaveChanges();
            }
        }
    }
}
