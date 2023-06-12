﻿// <auto-generated />
using System;
using DropDown.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DropDown.Migrations
{
    [DbContext(typeof(DropDownContext))]
    [Migration("20220824183856_add stock prev real details")]
    partial class addstockprevrealdetails
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DropDown.Models.Cascade.ActionProj", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjetId");

                    b.ToTable("ActionProjs");
                });

            modelBuilder.Entity("DropDown.Models.Cascade.Programme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Programmes");
                });

            modelBuilder.Entity("DropDown.Models.Cascade.Projet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProgrammeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProgrammeId");

                    b.ToTable("Projets");
                });

            modelBuilder.Entity("DropDown.Models.DD", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DDs");
                });

            modelBuilder.Entity("DropDown.Models.Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DDId")
                        .HasColumnType("int");

                    b.Property<int>("PrévisionId")
                        .HasColumnType("int");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<string>("TRN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Valeur")
                        .HasColumnType("int");

                    b.Property<string>("indiceTRN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("numDossier")
                        .HasColumnType("int");

                    b.Property<int>("numTRN")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DDId");

                    b.HasIndex("PrévisionId");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("DropDown.Models.DR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DRs");
                });

            modelBuilder.Entity("DropDown.Models.Exercice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Annee")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Exercices");
                });

            modelBuilder.Entity("DropDown.Models.Objectif", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ActionProjId")
                        .HasColumnType("int");

                    b.Property<int>("DRId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciceId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ActionProjId");

                    b.HasIndex("DRId");

                    b.HasIndex("ExerciceId");

                    b.ToTable("Objectifs");
                });

            modelBuilder.Entity("DropDown.Models.Prévision", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nombre")
                        .HasColumnType("int");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<int>("Valeur")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int>("objectifId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("objectifId");

                    b.ToTable("Prévisions");
                });

            modelBuilder.Entity("DropDown.Models.Réalisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nombre")
                        .HasColumnType("int");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<int>("Valeur")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int>("objectifId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("objectifId");

                    b.ToTable("Réalisations");
                });

            modelBuilder.Entity("DropDown.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nombre")
                        .HasColumnType("int");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<int>("Valeur")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int>("objectifId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("objectifId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("DropDown.Models.Cascade.ActionProj", b =>
                {
                    b.HasOne("DropDown.Models.Cascade.Projet", "Projet")
                        .WithMany()
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projet");
                });

            modelBuilder.Entity("DropDown.Models.Cascade.Projet", b =>
                {
                    b.HasOne("DropDown.Models.Cascade.Programme", "Programme")
                        .WithMany()
                        .HasForeignKey("ProgrammeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Programme");
                });

            modelBuilder.Entity("DropDown.Models.Detail", b =>
                {
                    b.HasOne("DropDown.Models.DD", "DD")
                        .WithMany()
                        .HasForeignKey("DDId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DropDown.Models.Prévision", "Prévision")
                        .WithMany()
                        .HasForeignKey("PrévisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DD");

                    b.Navigation("Prévision");
                });

            modelBuilder.Entity("DropDown.Models.Objectif", b =>
                {
                    b.HasOne("DropDown.Models.Cascade.ActionProj", "ActionProj")
                        .WithMany()
                        .HasForeignKey("ActionProjId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DropDown.Models.DR", "DR")
                        .WithMany()
                        .HasForeignKey("DRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DropDown.Models.Exercice", "Exercice")
                        .WithMany()
                        .HasForeignKey("ExerciceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActionProj");

                    b.Navigation("DR");

                    b.Navigation("Exercice");
                });

            modelBuilder.Entity("DropDown.Models.Prévision", b =>
                {
                    b.HasOne("DropDown.Models.Objectif", "objectif")
                        .WithMany()
                        .HasForeignKey("objectifId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("objectif");
                });

            modelBuilder.Entity("DropDown.Models.Réalisation", b =>
                {
                    b.HasOne("DropDown.Models.Objectif", "objectif")
                        .WithMany()
                        .HasForeignKey("objectifId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("objectif");
                });

            modelBuilder.Entity("DropDown.Models.Stock", b =>
                {
                    b.HasOne("DropDown.Models.Objectif", "objectif")
                        .WithMany()
                        .HasForeignKey("objectifId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("objectif");
                });
#pragma warning restore 612, 618
        }
    }
}