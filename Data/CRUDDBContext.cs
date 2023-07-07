using CRUD.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{
    public class CRUDDBContext : DbContext
    {
        public CRUDDBContext(DbContextOptions options) : base(options)
        {
        }
         
        public DbSet<Employee> Employees { get; set; }
    }
}
