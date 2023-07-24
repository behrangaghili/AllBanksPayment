﻿// <auto-generated />
using AspNetCore21Payment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace AspNetCore21Payment.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AspNetCore21Payment.Models.Epayment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Amount");

                    b.Property<string>("BankName")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(150);

                    b.Property<DateTime>("InsertDatetime");

                    b.Property<bool>("PaymentFinished");

                    b.Property<string>("ResCode")
                        .HasMaxLength(150);

                    b.Property<string>("RetrivalRefNo")
                        .HasMaxLength(100);

                    b.Property<long>("Rrn");

                    b.Property<string>("SystemTraceNo");

                    b.Property<string>("Token")
                        .HasMaxLength(150);

                    b.Property<int>("UserId");

                    b.HasKey("PaymentId");

                    b.ToTable("Epayments");
                });
#pragma warning restore 612, 618
        }
    }
}
