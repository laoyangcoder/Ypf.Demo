﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ypf.Xncf.FileCenterModul.Models;

namespace Ypf.Xncf.FileCenterModul.Migrations.Migrations.SqlServer
{
    [DbContext(typeof(FileCenterModulSenparcEntities_SqlServer))]
    [Migration("20210108155711_AddTenantId")]
    partial class AddTenantId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Ypf.Xncf.FileCenterModul.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("AddTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("AdditionNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdminRemark")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("Blue")
                        .HasColumnType("int");

                    b.Property<bool>("Flag")
                        .HasColumnType("bit");

                    b.Property<int>("Green")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Red")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Ypf_FileCenterModul_Color");
                });
#pragma warning restore 612, 618
        }
    }
}
