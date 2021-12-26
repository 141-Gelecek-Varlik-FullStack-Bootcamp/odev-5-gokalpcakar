using Icarus.DB.Entities.DataContext;
using System;
using System.Linq;

namespace Icarus.API.Jobs
{
    public class PrintWelcomeJob : IPrintWelcomeJob
    {
        public void PrintWelcome()
        {
            using (var context = new IcarusContext())
            {
                var users = context.User.Where(u => u.IsActive && !u.IsDeleted && u.Idate < DateTime.Now.AddDays(-1)).ToList();
                foreach (var user in users)
                {
                    //sendWelcomeMail(user.Email);
                    Console.WriteLine($"Welcome to the Icarus, {user.UserName}.");
                }
            }
        }

        public void CleanUserTable()
        {
            using (var context = new IcarusContext())
            {
                var users = context.User.Where(u => !u.IsActive && u.IsDeleted);

                foreach (var user in users)
                {
                    Console.WriteLine($"{user.Name} adlı kullanıcı silinmiştir");
                }

                context.User.RemoveRange(users);
                context.SaveChanges();
            }
        }

        public void CleanProductTable()
        {
            using (var context = new IcarusContext())
            {
                var products = context.Product.Where(u => !u.IsActive && u.IsDeleted);

                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Name} adlı ürün silinmiştir");
                }

                context.Product.RemoveRange(products);
                context.SaveChanges();
            }
        }
    }
}
