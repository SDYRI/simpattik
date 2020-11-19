using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;

namespace TasikmalayaKota.Simpatik.Web.Models
{
    public class Simpat1kContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public Simpat1kContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*****************************************************************************
            *  Identifikasi Primary Key
            *****************************************************************************/
            builder.Entity<mUserModel>().HasKey(e => new { e.IdUser });


            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<mUserModel> mUserModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(Configuration.GetConnectionString("SimpatikConnection"));
    }
}
