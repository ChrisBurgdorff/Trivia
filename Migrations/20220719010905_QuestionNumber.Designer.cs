﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalRDemo.Data;

#nullable disable

namespace SignalRDemo.Migrations
{
    [DbContext(typeof(SignalRDemoDbContext))]
    [Migration("20220719010905_QuestionNumber")]
    partial class QuestionNumber
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SignalRDemo.Models.AnswerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("QuestionModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionModelId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("SignalRDemo.Models.ChatModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("SignalRDemo.Models.GameModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SignalRDemo.Models.PlayerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ConnectionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentQuestion")
                        .HasColumnType("int");

                    b.Property<int?>("GameModelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameModelId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SignalRDemo.Models.QuestionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GameModelId")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameModelId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("SignalRDemo.Models.AnswerModel", b =>
                {
                    b.HasOne("SignalRDemo.Models.QuestionModel", null)
                        .WithMany("IncorrectAnswers")
                        .HasForeignKey("QuestionModelId");
                });

            modelBuilder.Entity("SignalRDemo.Models.PlayerModel", b =>
                {
                    b.HasOne("SignalRDemo.Models.GameModel", null)
                        .WithMany("Players")
                        .HasForeignKey("GameModelId");
                });

            modelBuilder.Entity("SignalRDemo.Models.QuestionModel", b =>
                {
                    b.HasOne("SignalRDemo.Models.GameModel", null)
                        .WithMany("Questions")
                        .HasForeignKey("GameModelId");
                });

            modelBuilder.Entity("SignalRDemo.Models.GameModel", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("SignalRDemo.Models.QuestionModel", b =>
                {
                    b.Navigation("IncorrectAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}