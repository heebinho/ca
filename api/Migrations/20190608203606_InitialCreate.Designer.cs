﻿// <auto-generated />
using System;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace api.Migrations
{
    [DbContext(typeof(TnRContext))]
    [Migration("20190608203606_InitialCreate")]
    partial class InitialCreate
    {
        /// <summary>
        /// BuildTargetModel
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085");

            modelBuilder.Entity("API.Model.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("API.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}