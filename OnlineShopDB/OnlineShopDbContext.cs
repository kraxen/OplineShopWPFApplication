using Microsoft.EntityFrameworkCore;
using OnlineShopInfrastructe;
using OnlineShopVMLib;

namespace OnlineShopDB
{
    public class OnlineShopDbContext : DbContext, IDbAdapter
    {
        public DbSet<Client> DbClients { get; set; }
        public DbSet<Product> DbProducts { get; set; }
        IEnumerable<Client> IDbAdapter.Clients { get => DbClients.Include(c=>c.Products); set => throw new NotImplementedException(); }
        IEnumerable<Product> IDbAdapter.Products { get => DbProducts; set => throw new NotImplementedException(); }

        public OnlineShopDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=localdb.db");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasKey(c => new { c.Email });
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.Email);


            base.OnModelCreating(modelBuilder);
        }

        void IDbAdapter.SaveChanges()
        {
            SaveChanges();
        }

        IEnumerable<Client> IDbAdapter.Where(Func<Client, bool> func)
        {
            try
            {
                return DbClients.Where(func);
            }
            catch
            {
                return new List<Client>();
            }
        }

        IEnumerable<Product> IDbAdapter.Where(Func<Product, bool> func)
        {
            try
            {
                return DbProducts.Where(func);
            }
            catch
            {
                return new List<Product>();
            }
        }

        void IDbAdapter.AddClient(Client client)
        {
            DbClients.Add(client);
            SaveChanges();
        }

        void IDbAdapter.RemoveClient(Client client)
        {
            DbClients.Remove(client);
            SaveChanges();
        }

        void IDbAdapter.UpdateClient(Client client)
        {
            DbClients.Update(client);
            SaveChanges();
        }

        void IDbAdapter.AddProduct(Product product)
        {
            DbProducts.Add(product);
            SaveChanges();
        }

        void IDbAdapter.RemoveProduct(Product product)
        {
            DbProducts.Remove(product);
            SaveChanges();
        }

        void IDbAdapter.UpdateProduct(Product product)
        {
            DbProducts.Update(product);
            SaveChanges();
        }
    }
}