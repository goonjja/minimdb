﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiniMdb.Backend.Data;
using MiniMdb.Backend.Shared;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MiniMdb.Backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MiniMdb.Backend.Models.MediaTitle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("AddedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Plot")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("ReleaseDate")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<long>("UpdatedAt")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("Type");

                    b.ToTable("Titles");

                    b.HasDiscriminator<int>("Type");
                });

            modelBuilder.Entity("MiniMdb.Backend.Models.Movie", b =>
                {
                    b.HasBaseType("MiniMdb.Backend.Models.MediaTitle");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("MiniMdb.Backend.Models.Series", b =>
                {
                    b.HasBaseType("MiniMdb.Backend.Models.MediaTitle");

                    b.HasDiscriminator().HasValue(1);
                });
#pragma warning restore 612, 618
        }
    }
}
