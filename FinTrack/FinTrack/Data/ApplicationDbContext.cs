using AuthenticationServer.Models;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using ResourceServer.Models;

namespace FinTrack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Override OnModelCreating to configure entity properties and relationships.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure the UserRole entity as a join table for User and Role.
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite primary key.

            //Defines the many-to-many relationship between User and Role.
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRole)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Configure one-to-one relationship between User and UserDetail
            modelBuilder.Entity<UserDetail>()
                .HasOne(ud => ud.User)
                .WithOne(u => u.UserDetail)
                .HasForeignKey<UserDetail>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserInvestment is the one to many child of user Expense
            modelBuilder.Entity<UserInvestment>()
                .HasOne(ui => ui.UserExpense)
                .WithMany(ue => ue.UserInvestments)
                .HasForeignKey(ui => ui.UserExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            //UserExpense is a one to many child of User
            modelBuilder.Entity<UserExpense>()
                .HasOne(ue=>ue.User)
                .WithMany(u=>u.UserExpenses)
                .HasForeignKey(ue=>ue.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //UserIncome is a one to many child of User
            modelBuilder.Entity<UserIncome>()
               .HasOne(ue => ue.User)
               .WithMany(u => u.UserIncomes)
               .HasForeignKey(ue => ue.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision
            modelBuilder.Entity<UserDetail>()
                .Property(e => e.AnnualSalary)
                .HasPrecision(18, 2);

            
            modelBuilder.Entity<UserInvestment>()
                .Property(e => e.InvestmentAmount)
                .HasPrecision(18, 2);

            // Add check constraints for investment type
            modelBuilder.Entity<UserInvestment>()
                .ToTable(t => t.HasCheckConstraint("CK_InvestmentType", 
                    "[InvestmentType] IN ('Stock', 'SIP')"));

            // Seed initial data for Roles, Users, Clients, and UserRoles.
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Admin Role" },
                new Role { Id = 2, Name = "Editor", Description = " Editor Role" },
                new Role { Id = 3, Name = "User", Description = "User Role" }
            );
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    ClientId = "Client1",
                    Name = "Client Application 1",
                    ClientURL = "https://client1.com"
                },
                new Client
                {
                    Id = 2,
                    ClientId = "Client2",
                    Name = "Client Application 2",
                    ClientURL = "https://client2.com"
                }
            );
        }
        
        // DbSet representing the Users table.
        public DbSet<User> Users { get; set; }
        // DbSet representing the Roles table.
        public DbSet<Role> Roles { get; set; }
        // DbSet representing the Clients table.
        public DbSet<Client> Clients { get; set; }
        // DbSet representing the UserRoles join table.
        public DbSet<UserRole> UserRoles { get; set; }
        // DbSet representing the SigningKeys table.
        public DbSet<SigningKey> SigningKeys { get; set; }
        // DbSet representing the UserDetails table.
        public DbSet<UserDetail> UserDetails { get; set; }
        // DbSet representing the UserInvestments table.
        public DbSet<UserInvestment> UserInvestments { get; set; }
        public DbSet<UserExpense> UserExpenses { get; set; }
        public DbSet<UserIncome> UserIncomes { get; set; }
    }
}