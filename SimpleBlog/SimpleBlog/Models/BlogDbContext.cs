namespace SimpleBlog.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class BlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public BlogDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static BlogDbContext Create()
        {
            return new BlogDbContext();
        }
    }
}