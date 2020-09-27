using Lexica.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Lexica.EF
{
    public class LexicaContext : DbContext
    {
        public DbSet<EntryTable> Entries { get; set; }

        public DbSet<ImportHistoryTable> ImportExecutions { get; set; }

        public DbSet<SetTable> Sets { get; set; }

        public LexicaContext() : base() { }

        public LexicaContext(DbContextOptions<LexicaContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=192.168.0.20;Port=5500;Database=Lexica;Username=Lexica;Password=9Dt7pv4Uwi6JPHREploR");
        }

        private void CreateWordsTables(ModelBuilder modelBuilder)
        {
            var entryTable = modelBuilder.Entity<EntryTable>()
                .ToTable("Entry", "words");
            entryTable.HasOne<SetTable>(x => x.Set)
                .WithMany(y => y.Entries)
                .HasForeignKey(x => x.SetId)
                .OnDelete(DeleteBehavior.NoAction);

            var importTable = modelBuilder.Entity<ImportHistoryTable>()
                .ToTable("ImportHistory", "words");
            importTable.HasOne<SetTable>(x => x.Set)
                .WithOne(y => y.Import)
                .OnDelete(DeleteBehavior.NoAction);

            var setTable = modelBuilder.Entity<SetTable>()
                .ToTable("Set", "words");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateWordsTables(modelBuilder);
        }
    }
}
