
using Lexica.Core.IO;
using Lexica.Core.Services;
using Lexica.EF.Models;
using Lexica.EF.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lexica.EF
{
    public class LexicaContext : DbContext
    {
        public DbSet<EntryTable> Entries => Set<EntryTable>();

        public DbSet<ImportHistoryTable> ImportExecutions => Set<ImportHistoryTable>();

        public DbSet<SetTable> Sets => Set<SetTable>();

        public LexicaContext() : base() { }

        public LexicaContext(DbContextOptions<LexicaContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigService<DatabaseSettings> appSettings = ConfigService<DatabaseSettings>.Get(
                "database", Assembly.GetExecutingAssembly()
            );
            string? connectionString = appSettings.Get().ConnectionString;

            optionsBuilder.UseNpgsql(connectionString);
        }

        private void CreateWordsTables(ModelBuilder modelBuilder)
        {
            var entryTable = modelBuilder.Entity<EntryTable>()
                .ToTable("entry", "words");
            entryTable.HasOne<SetTable>(x => x.Set)
                .WithMany(y => y.Entries)
                .HasForeignKey(x => x.SetId)
                .OnDelete(DeleteBehavior.NoAction);

            var importTable = modelBuilder.Entity<ImportHistoryTable>()
                .ToTable("import_history", "words");
            importTable.HasOne<SetTable>(x => x.Set)
                .WithOne(y => y.Import)
                .OnDelete(DeleteBehavior.NoAction);

            var setTable = modelBuilder.Entity<SetTable>()
                .ToTable("set", "words");

            var maintainingHistoryTable = modelBuilder.Entity<MaintainingHistoryTable>()
                .ToTable("maintaining_history", "modes");
            maintainingHistoryTable.HasOne<EntryTable>(x => x.Entry)
                .WithOne(y => y.History)
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateWordsTables(modelBuilder);
        }
    }
}
