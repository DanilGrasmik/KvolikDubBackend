﻿// <auto-generated />
using System;
using System.Collections.Generic;
using KvolikDubBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KvolikDubBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230118081557_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("KvolikDubBackend.Models.Entities.AnimeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AgeLimit")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<int>("EpisodesAmount")
                        .HasColumnType("integer");

                    b.Property<int>("ExitStatus")
                        .HasColumnType("integer");

                    b.Property<List<string>>("Frames")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Genres")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameEng")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PrimarySource")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ReleaseBy")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ReleaseFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TrailerUrl")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Animes");
                });
#pragma warning restore 612, 618
        }
    }
}
