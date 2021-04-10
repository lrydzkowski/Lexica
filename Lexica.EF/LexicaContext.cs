﻿using Lexica.Core.Services;
using Lexica.EF.Models;
using Lexica.EF.Config;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lexica.EF
{
    public class LexicaContext : DbContext
    {
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
                string filePath = appSettings.Get().FilePath;

                optionsBuilder.UseSqlite($"Data Source={filePath}");
            }
        }

        private static void CreateWordsTables(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<LearningHistoryTable>()
                .ToTable("learning_history", "modes");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateWordsTables(modelBuilder);
        }
    }
}
