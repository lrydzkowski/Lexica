﻿// <auto-generated />
using System;
using Lexica.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lexica.EF.Migrations
{
    [DbContext(typeof(LexicaContext))]
    partial class LexicaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Lexica.EF.Models.EntryTable", b =>
                {
                    b.Property<long>("RecId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("rec_id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("EntryId")
                        .HasColumnName("entry_id")
                        .HasColumnType("integer");

                    b.Property<long>("SetId")
                        .HasColumnName("set_id")
                        .HasColumnType("bigint");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnName("translation")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnName("word")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("RecId");

                    b.HasIndex("SetId");

                    b.ToTable("entry","words");
                });

            modelBuilder.Entity("Lexica.EF.Models.ImportHistoryTable", b =>
                {
                    b.Property<long>("ImportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("import_id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("ExecutedDate")
                        .HasColumnName("executed_date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("SetId")
                        .HasColumnName("set_id")
                        .HasColumnType("bigint");

                    b.HasKey("ImportId");

                    b.HasIndex("SetId")
                        .IsUnique();

                    b.ToTable("import_history","words");
                });

            modelBuilder.Entity("Lexica.EF.Models.MaintainingHistoryTable", b =>
                {
                    b.Property<long>("OperationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("operation_id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("EntryRecId")
                        .HasColumnName("entry_rec_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsTranslation")
                        .HasColumnName("is_translation")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsWord")
                        .HasColumnName("is_word")
                        .HasColumnType("boolean");

                    b.Property<long>("NumOfCorrectAnswers")
                        .HasColumnName("num_of_correct_answers")
                        .HasColumnType("bigint");

                    b.Property<long>("NumOfMistakes")
                        .HasColumnName("num_of_mistakes")
                        .HasColumnType("bigint");

                    b.HasKey("OperationId");

                    b.HasIndex("EntryRecId")
                        .IsUnique();

                    b.ToTable("maintaining_history","modes");
                });

            modelBuilder.Entity("Lexica.EF.Models.SetTable", b =>
                {
                    b.Property<long>("SetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("set_id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Namespace")
                        .IsRequired()
                        .HasColumnName("namespace")
                        .HasColumnType("character varying(400)")
                        .HasMaxLength(400);

                    b.HasKey("SetId");

                    b.ToTable("set","words");
                });

            modelBuilder.Entity("Lexica.EF.Models.EntryTable", b =>
                {
                    b.HasOne("Lexica.EF.Models.SetTable", "Set")
                        .WithMany("Entries")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Lexica.EF.Models.ImportHistoryTable", b =>
                {
                    b.HasOne("Lexica.EF.Models.SetTable", "Set")
                        .WithOne("Import")
                        .HasForeignKey("Lexica.EF.Models.ImportHistoryTable", "SetId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Lexica.EF.Models.MaintainingHistoryTable", b =>
                {
                    b.HasOne("Lexica.EF.Models.EntryTable", "Entry")
                        .WithOne("History")
                        .HasForeignKey("Lexica.EF.Models.MaintainingHistoryTable", "EntryRecId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
