using Lexica.Core.Services;
using Lexica.EF.Models;
using Lexica.EF.Config;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lexica.EF
{
    public class LexicaContext : DbContext
    {
        public DbSet<MaintainingHistoryTable> MaintainingHistoryRecords => Set<MaintainingHistoryTable>();

        public DbSet<LearningHistoryTable> LearningHistoryRecords => Set<LearningHistoryTable>();

        public LexicaContext() : base() { }

        public LexicaContext(DbContextOptions<LexicaContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigService<DatabaseSettings> appSettings = ConfigService<DatabaseSettings>.Get(
                    "database", Assembly.GetExecutingAssembly()
                );
                string? connectionString = appSettings.Get().ConnectionString;

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        private void CreateWordsTables(ModelBuilder modelBuilder)
        {
            var maintainingHistoryTable = modelBuilder.Entity<MaintainingHistoryTable>()
                .ToTable("maintaining_history", "modes");
            var learningHistoryTable = modelBuilder.Entity<LearningHistoryTable>()
                .ToTable("learning_history", "modes");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateWordsTables(modelBuilder);
        }
    }
}
