using Microsoft.EntityFrameworkCore;
using OnlineShopInfrastructe;
using OnlineShopModels;

namespace OnlineShopDB
{
    public class OnlineShopDbContext : DbContext, IDbAdapter
    {
        public DbSet<Client> DbClients { get; set; }
        public DbSet<Product> DbProducts { get; set; }
        IEnumerable<Client> IDbAdapter.GetClients() => DbClients.Include(c=>c.Products);
        IEnumerable<Product> IDbAdapter.GetProducts() => DbProducts; 

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

        void IDbAdapter.AddClient(Client client)
        {
            SaveChanges();
            DbClients.Add(client);
            SaveChanges();
        }

        void IDbAdapter.RemoveClient(string email)
        {
            SaveChanges();
            var client = DbClients.FirstOrDefault(c => c.Email == email);
            if (client is null) return;
            DbClients.Remove(client);
            SaveChanges();
        }

        void IDbAdapter.UpdateClient(Client client)
        {
            SaveChanges();
            var c = DbClients.FirstOrDefault(c => c.Email == client.Email);
            if (c is null) return;
            c.Name = client.Name;
            c.Suname = client.Suname;
            c.Patronymic = client.Patronymic;
            c.Phone = client.Phone;
            SaveChanges();
        }

        void IDbAdapter.AddProduct(Product product)
        {
            SaveChanges();
            DbProducts.Add(product);
            SaveChanges();
        }

        void IDbAdapter.RemoveProduct(int productId)
        {
            SaveChanges();
            var product = DbProducts.FirstOrDefault(p => p.Id == productId);
            if (product is null) return;
            DbProducts.Remove(product);
            SaveChanges();
        }

        void IDbAdapter.UpdateProduct(Product product)
        {
            SaveChanges();
            var p = DbProducts.FirstOrDefault(p => p.Id == product.Id);
            if (p is null) return;
            p.Name = product.Name;
            p.Kod = product.Kod;
            SaveChanges();
        }
    }
}