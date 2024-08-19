﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PCM.Data;

#nullable disable

namespace PCM.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240819062353_InitialCreate4")]
    partial class InitialCreate4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PCM.Models.Collection", b =>
                {
                    b.Property<Guid>("CollectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomBoolean1Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomBoolean2Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomBoolean3Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomDate1Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomDate2Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomDate3Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomInt1Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomInt2Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomInt3Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomMultilineText1Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomMultilineText2Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomMultilineText3Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomString1Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomString2Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomString3Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TotalItems")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CollectionId");

                    b.HasIndex("UserId");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("PCM.Models.Comment", b =>
                {
                    b.Property<Guid>("CommentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentID");

                    b.HasIndex("ItemId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PCM.Models.Item", b =>
                {
                    b.Property<Guid>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CollectionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CollectionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomBoolean1Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomBoolean2Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomBoolean3Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomDate1Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomDate2Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomDate3Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomInt1Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomInt2Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomInt3Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomMultilineText1Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomMultilineText2Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomMultilineText3Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomString1Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomString2Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomString3Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ItemId");

                    b.HasIndex("CollectionId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PCM.Models.ItemLikeCount", b =>
                {
                    b.Property<Guid>("ItemLikeCountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.HasKey("ItemLikeCountId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemLikeCounts");
                });

            modelBuilder.Entity("PCM.Models.Like", b =>
                {
                    b.Property<Guid>("LikeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("VisitorUserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LikeID");

                    b.HasIndex("ItemId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("PCM.Models.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.HasIndex("ItemId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PCM.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PCM.Models.Collection", b =>
                {
                    b.HasOne("PCM.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PCM.Models.Comment", b =>
                {
                    b.HasOne("PCM.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("PCM.Models.Item", b =>
                {
                    b.HasOne("PCM.Models.Collection", "Collection")
                        .WithMany("Items")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("PCM.Models.ItemLikeCount", b =>
                {
                    b.HasOne("PCM.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("PCM.Models.Like", b =>
                {
                    b.HasOne("PCM.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("PCM.Models.Tag", b =>
                {
                    b.HasOne("PCM.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("PCM.Models.Collection", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
