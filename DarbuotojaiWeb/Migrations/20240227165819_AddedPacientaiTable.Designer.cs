﻿// <auto-generated />
using System;
using DarbuotojaiWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DarbuotojaiWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240227165819_AddedPacientaiTable")]
    partial class AddedPacientaiTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DarbuotojaiWeb.Models.Darbuotojas", b =>
                {
                    b.Property<int>("DarbuotojasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DarbuotojasId"));

                    b.Property<string>("Adresas")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("GimimoData")
                        .HasColumnType("datetime2");

                    b.Property<string>("Pavarde")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Statusas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vardas")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.HasKey("DarbuotojasId");

                    b.ToTable("Darbuotojai");
                });

            modelBuilder.Entity("DarbuotojaiWeb.Models.DarbuotojasPareiga", b =>
                {
                    b.Property<int>("DarbuotojasId")
                        .HasColumnType("int");

                    b.Property<int>("PareigaId")
                        .HasColumnType("int");

                    b.HasKey("DarbuotojasId", "PareigaId");

                    b.HasIndex("PareigaId");

                    b.ToTable("DarbuotojasPareigos");
                });

            modelBuilder.Entity("DarbuotojaiWeb.Models.Pacientas", b =>
                {
                    b.Property<int>("PacientasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PacientasId"));

                    b.Property<string>("Adresas")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("AsmensKodas")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<int>("DarbuotojasId")
                        .HasColumnType("int");

                    b.Property<string>("Pavarde")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Statusas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vardas")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.HasKey("PacientasId");

                    b.HasIndex("DarbuotojasId");

                    b.ToTable("Pacientai");
                });

            modelBuilder.Entity("DarbuotojaiWeb.Models.Pareiga", b =>
                {
                    b.Property<int>("PareigaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PareigaId"));

                    b.Property<string>("Pavadinimas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PareigaId");

                    b.ToTable("Pareigos");

                    b.HasData(
                        new
                        {
                            PareigaId = 1,
                            Pavadinimas = "Administratorius(-ė)"
                        },
                        new
                        {
                            PareigaId = 2,
                            Pavadinimas = "Analitikas(-ė)"
                        },
                        new
                        {
                            PareigaId = 3,
                            Pavadinimas = "Bendrosios praktikos slaugytoja"
                        },
                        new
                        {
                            PareigaId = 4,
                            Pavadinimas = "Laborantas(-ė)"
                        },
                        new
                        {
                            PareigaId = 5,
                            Pavadinimas = "Vadovas(-ė)"
                        },
                        new
                        {
                            PareigaId = 6,
                            Pavadinimas = "Apskaitininkas(-ė)"
                        },
                        new
                        {
                            PareigaId = 7,
                            Pavadinimas = "Programuotojas(-a)"
                        });
                });

            modelBuilder.Entity("DarbuotojaiWeb.Models.DarbuotojasPareiga", b =>
                {
                    b.HasOne("DarbuotojaiWeb.Models.Darbuotojas", "Darbuotojas")
                        .WithMany("DarbuotojasPareigos")
                        .HasForeignKey("DarbuotojasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DarbuotojaiWeb.Models.Pareiga", "Pareiga")
                        .WithMany("DarbuotojasPareigos")
                        .HasForeignKey("PareigaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Darbuotojas");

                    b.Navigation("Pareiga");
                });

            modelBuilder.Entity("DarbuotojaiWeb.Models.Pacientas", b =>
                {
                    b.HasOne("DarbuotojaiWeb.Models.Darbuotojas", "Darbuotojas")
                        .WithMany("Pacientai")
                        .HasForeignKey("DarbuotojasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Darbuotojas");
                });

            modelBuilder.Entity("DarbuotojaiWeb.Models.Darbuotojas", b =>
                {
                    b.Navigation("DarbuotojasPareigos");

                    b.Navigation("Pacientai");
                });

            modelBuilder.Entity("DarbuotojaiWeb.Models.Pareiga", b =>
                {
                    b.Navigation("DarbuotojasPareigos");
                });
#pragma warning restore 612, 618
        }
    }
}
