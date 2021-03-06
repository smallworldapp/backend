﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SmallWorld.Database.Entities;
using System;

namespace SmallWorld.Database.Migrations
{
    [DbContext(typeof(SmallWorldContext))]
    [Migration("20171016195920_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("SmallWorld.Database.Context.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CredentialsId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<Guid>("Guid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int?>("ResetTokenId");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CredentialsId");

                    b.HasIndex("ResetTokenId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Guid");

                    b.HasKey("Id");

                    b.ToTable("Application");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.ApplicationQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<int?>("ApplicationId");

                    b.Property<Guid>("Guid");

                    b.Property<string>("Question")
                        .IsRequired();

                    b.Property<string>("Subtext")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("ApplicationQuestion");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Guid");

                    b.Property<byte[]>("Hash");

                    b.Property<byte[]>("Salt");

                    b.HasKey("Id");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Description", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Goals")
                        .IsRequired();

                    b.Property<Guid>("Guid");

                    b.Property<string>("Introduction")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Description");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<DateTime>("Created");

                    b.Property<Guid>("Guid");

                    b.Property<bool>("IsSent");

                    b.Property<DateTime?>("Sent");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.EmailRecipient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int?>("EmailId");

                    b.Property<Guid>("Guid");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("EmailId");

                    b.ToTable("EmailRecipient");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.FaqItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .IsRequired();

                    b.Property<int?>("DescriptionId");

                    b.Property<Guid>("Guid");

                    b.Property<string>("Question")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.ToTable("FaqItem");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Identity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid>("Guid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Identity");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Identity");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.MfroMigration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Identifier");

                    b.HasKey("Id");

                    b.ToTable("__MfroMigrationsHistory");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Pair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Guid");

                    b.Property<int?>("InitiatorId");

                    b.Property<int>("Outcome");

                    b.Property<int?>("PairingId");

                    b.Property<int?>("ReceiverId");

                    b.Property<int?>("WorldId");

                    b.HasKey("Id");

                    b.HasIndex("InitiatorId");

                    b.HasIndex("PairingId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("WorldId");

                    b.ToTable("Pairs");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Pairing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<Guid>("Guid");

                    b.Property<bool>("IsComplete");

                    b.Property<string>("Message");

                    b.Property<int>("Type");

                    b.Property<int?>("WorldId");

                    b.HasKey("Id");

                    b.HasIndex("WorldId");

                    b.ToTable("Pairings");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.PairingSettings", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Enabled");

                    b.Property<Guid>("Guid");

                    b.Property<DateTime>("MostRecent");

                    b.Property<long>("Period");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.ToTable("PairingSettings");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.ResetToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Expiration");

                    b.Property<Guid>("Guid");

                    b.HasKey("Id");

                    b.ToTable("ResetToken");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.World", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<int?>("ApplicationId");

                    b.Property<int?>("BackupUserId");

                    b.Property<int?>("DescriptionId");

                    b.Property<Guid>("Guid");

                    b.Property<string>("Identifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("Privacy");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("BackupUserId");

                    b.HasIndex("DescriptionId");

                    b.ToTable("Worlds");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Member", b =>
                {
                    b.HasBaseType("SmallWorld.Database.Context.Identity");

                    b.Property<bool>("HasEmailValidation");

                    b.Property<bool>("HasLeft");

                    b.Property<bool>("HasPrivacyValidation");

                    b.Property<Guid>("JoinToken");

                    b.Property<Guid>("LeaveToken");

                    b.Property<bool>("OptOut");

                    b.Property<int?>("WorldId");

                    b.Property<int>("_Deprecated_Status")
                        .HasColumnName("Status");

                    b.HasIndex("WorldId");

                    b.ToTable("Member");

                    b.HasDiscriminator().HasValue("Member");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Account", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.Credentials", "Credentials")
                        .WithMany()
                        .HasForeignKey("CredentialsId");

                    b.HasOne("SmallWorld.Database.Context.ResetToken", "ResetToken")
                        .WithMany()
                        .HasForeignKey("ResetTokenId");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.ApplicationQuestion", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.Application")
                        .WithMany("Questions")
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.EmailRecipient", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.Email")
                        .WithMany("Recipients")
                        .HasForeignKey("EmailId");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.FaqItem", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.Description")
                        .WithMany("Faq")
                        .HasForeignKey("DescriptionId");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Pair", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.Member", "Initiator")
                        .WithMany("Pairs1")
                        .HasForeignKey("InitiatorId");

                    b.HasOne("SmallWorld.Database.Context.Pairing", "Pairing")
                        .WithMany("Pairs")
                        .HasForeignKey("PairingId");

                    b.HasOne("SmallWorld.Database.Context.Member", "Receiver")
                        .WithMany("Pairs2")
                        .HasForeignKey("ReceiverId");

                    b.HasOne("SmallWorld.Database.Context.World", "World")
                        .WithMany("Pairs")
                        .HasForeignKey("WorldId");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Pairing", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.World", "World")
                        .WithMany("Pairings")
                        .HasForeignKey("WorldId");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.PairingSettings", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.World", "World")
                        .WithOne("PairingSettings")
                        .HasForeignKey("SmallWorld.Database.Context.PairingSettings", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmallWorld.Database.Context.World", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.Account", "Account")
                        .WithMany("Worlds")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmallWorld.Database.Context.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.HasOne("SmallWorld.Database.Context.Identity", "BackupUser")
                        .WithMany()
                        .HasForeignKey("BackupUserId");

                    b.HasOne("SmallWorld.Database.Context.Description", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId");
                });

            modelBuilder.Entity("SmallWorld.Database.Context.Member", b =>
                {
                    b.HasOne("SmallWorld.Database.Context.World", "World")
                        .WithMany("Members")
                        .HasForeignKey("WorldId");
                });
#pragma warning restore 612, 618
        }
    }
}
