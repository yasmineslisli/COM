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
    [Migration("20221009170447_Add Etat for prevision")]
    partial class AddEtatforprevision
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DropDown.Models.ActionProj", b =>
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

                    b.HasIndex(new[] { "ProjetId" }, "IX_ActionProjs_ProjetId");

                    b.ToTable("ActionProjs");
                });

            modelBuilder.Entity("DropDown.Models.Dd", b =>
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

                    b.Property<int>("Ddid")
                        .HasColumnType("int")
                        .HasColumnName("DDId");

                    b.Property<string>("IndiceTrn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("indiceTRN");

                    b.Property<int?>("NumDossier")
                        .HasColumnType("int")
                        .HasColumnName("numDossier");

                    b.Property<int>("NumTrn")
                        .HasColumnType("int")
                        .HasColumnName("numTRN");

                    b.Property<int>("PrévisionId")
                        .HasColumnType("int");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<string>("Trn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TRN");

                    b.Property<int?>("Valeur")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Ddid" }, "IX_Details_DDId");

                    b.HasIndex(new[] { "PrévisionId" }, "IX_Details_PrévisionId");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("DropDown.Models.Dr", b =>
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

                    b.Property<int>("Drid")
                        .HasColumnType("int")
                        .HasColumnName("DRId");

                    b.Property<int>("ExerciceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ActionProjId" }, "IX_Objectifs_ActionProjId");

                    b.HasIndex(new[] { "Drid" }, "IX_Objectifs_DRId");

                    b.HasIndex(new[] { "ExerciceId" }, "IX_Objectifs_ExerciceId");

                    b.ToTable("Objectifs");
                });

            modelBuilder.Entity("DropDown.Models.Prévision", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("date");

                    b.Property<bool>("Etat")
                        .HasColumnType("bit")
                        .HasColumnName("Etat");

                    b.Property<int>("Nombre")
                        .HasColumnType("int");

                    b.Property<int>("ObjectifId")
                        .HasColumnType("int")
                        .HasColumnName("objectifId");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<int?>("Valeur")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ObjectifId" }, "IX_Prévisions_objectifId");

                    b.ToTable("Prévisions");
                });

            modelBuilder.Entity("DropDown.Models.Profil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Profil");
                });

            modelBuilder.Entity("DropDown.Models.Programme", b =>
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

            modelBuilder.Entity("DropDown.Models.Projet", b =>
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

                    b.HasIndex(new[] { "ProgrammeId" }, "IX_Projets_ProgrammeId");

                    b.ToTable("Projets");
                });

            modelBuilder.Entity("DropDown.Models.Réalisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("date");

                    b.Property<int>("Nombre")
                        .HasColumnType("int");

                    b.Property<int>("ObjectifId")
                        .HasColumnType("int")
                        .HasColumnName("objectifId");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<int>("Valeur")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ObjectifId" }, "IX_Réalisations_objectifId");

                    b.ToTable("Réalisations");
                });

            modelBuilder.Entity("DropDown.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("date");

                    b.Property<int>("Nombre")
                        .HasColumnType("int");

                    b.Property<int>("ObjectifId")
                        .HasColumnType("int")
                        .HasColumnName("objectifId");

                    b.Property<int>("Superficie")
                        .HasColumnType("int");

                    b.Property<int?>("Valeur")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ObjectifId" }, "IX_Stocks_objectifId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("DropDown.Models.Structure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Structure");
                });

            modelBuilder.Entity("DropDown.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Cin")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CIN");

                    b.Property<string>("ConfirmPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfilId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Statut")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StructureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProfilId");

                    b.HasIndex("StructureId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DropDown.Models.ActionProj", b =>
                {
                    b.HasOne("DropDown.Models.Projet", "Projet")
                        .WithMany("ActionProjs")
                        .HasForeignKey("ProjetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projet");
                });

            modelBuilder.Entity("DropDown.Models.Detail", b =>
                {
                    b.HasOne("DropDown.Models.Dd", "Dd")
                        .WithMany("Details")
                        .HasForeignKey("Ddid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DropDown.Models.Prévision", "Prévision")
                        .WithMany("Details")
                        .HasForeignKey("PrévisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dd");

                    b.Navigation("Prévision");
                });

            modelBuilder.Entity("DropDown.Models.Objectif", b =>
                {
                    b.HasOne("DropDown.Models.ActionProj", "ActionProj")
                        .WithMany("Objectifs")
                        .HasForeignKey("ActionProjId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DropDown.Models.Dr", "Dr")
                        .WithMany("Objectifs")
                        .HasForeignKey("Drid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DropDown.Models.Exercice", "Exercice")
                        .WithMany("Objectifs")
                        .HasForeignKey("ExerciceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActionProj");

                    b.Navigation("Dr");

                    b.Navigation("Exercice");
                });

            modelBuilder.Entity("DropDown.Models.Prévision", b =>
                {
                    b.HasOne("DropDown.Models.Objectif", "Objectif")
                        .WithMany("Prévisions")
                        .HasForeignKey("ObjectifId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Objectif");
                });

            modelBuilder.Entity("DropDown.Models.Projet", b =>
                {
                    b.HasOne("DropDown.Models.Programme", "Programme")
                        .WithMany("Projets")
                        .HasForeignKey("ProgrammeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Programme");
                });

            modelBuilder.Entity("DropDown.Models.Réalisation", b =>
                {
                    b.HasOne("DropDown.Models.Objectif", "Objectif")
                        .WithMany("Réalisations")
                        .HasForeignKey("ObjectifId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Objectif");
                });

            modelBuilder.Entity("DropDown.Models.Stock", b =>
                {
                    b.HasOne("DropDown.Models.Objectif", "Objectif")
                        .WithMany("Stocks")
                        .HasForeignKey("ObjectifId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Objectif");
                });

            modelBuilder.Entity("DropDown.Models.User", b =>
                {
                    b.HasOne("DropDown.Models.Profil", "Profil")
                        .WithMany()
                        .HasForeignKey("ProfilId");

                    b.HasOne("DropDown.Models.Structure", "structure")
                        .WithMany()
                        .HasForeignKey("StructureId");

                    b.Navigation("Profil");

                    b.Navigation("structure");
                });

            modelBuilder.Entity("DropDown.Models.ActionProj", b =>
                {
                    b.Navigation("Objectifs");
                });

            modelBuilder.Entity("DropDown.Models.Dd", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("DropDown.Models.Dr", b =>
                {
                    b.Navigation("Objectifs");
                });

            modelBuilder.Entity("DropDown.Models.Exercice", b =>
                {
                    b.Navigation("Objectifs");
                });

            modelBuilder.Entity("DropDown.Models.Objectif", b =>
                {
                    b.Navigation("Prévisions");

                    b.Navigation("Réalisations");

                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("DropDown.Models.Prévision", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("DropDown.Models.Programme", b =>
                {
                    b.Navigation("Projets");
                });

            modelBuilder.Entity("DropDown.Models.Projet", b =>
                {
                    b.Navigation("ActionProjs");
                });
#pragma warning restore 612, 618
        }
    }
}
