﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeopleSkillsApp.Models;

#nullable disable

namespace PeopleSkillsApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241022094931_RemoveSkillIds")]
    partial class RemoveSkillIds
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("PeopleSkillsApp.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkplaceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkplaceId");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "German",
                            WorkplaceId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Mark",
                            WorkplaceId = 2
                        },
                        new
                        {
                            Id = 3,
                            Name = "Daniel",
                            WorkplaceId = 3
                        });
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.PersonSkill", b =>
                {
                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkillId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.HasKey("PersonId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("PersonSkills");

                    b.HasData(
                        new
                        {
                            PersonId = 1,
                            SkillId = 1,
                            Level = 0
                        },
                        new
                        {
                            PersonId = 1,
                            SkillId = 2,
                            Level = 0
                        },
                        new
                        {
                            PersonId = 2,
                            SkillId = 1,
                            Level = 0
                        },
                        new
                        {
                            PersonId = 2,
                            SkillId = 3,
                            Level = 0
                        },
                        new
                        {
                            PersonId = 3,
                            SkillId = 4,
                            Level = 0
                        },
                        new
                        {
                            PersonId = 3,
                            SkillId = 5,
                            Level = 0
                        },
                        new
                        {
                            PersonId = 3,
                            SkillId = 3,
                            Level = 0
                        });
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Skills");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Level = 0,
                            Name = "C#"
                        },
                        new
                        {
                            Id = 2,
                            Level = 0,
                            Name = "Java"
                        },
                        new
                        {
                            Id = 3,
                            Level = 0,
                            Name = "Excel"
                        },
                        new
                        {
                            Id = 4,
                            Level = 0,
                            Name = "Python"
                        },
                        new
                        {
                            Id = 5,
                            Level = 0,
                            Name = "PowerPoint"
                        });
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.Workplace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Workplaces");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Estonia"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Germany"
                        },
                        new
                        {
                            Id = 3,
                            Name = "USA"
                        });
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.Person", b =>
                {
                    b.HasOne("PeopleSkillsApp.Models.Workplace", "Workplace")
                        .WithMany("Persons")
                        .HasForeignKey("WorkplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workplace");
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.PersonSkill", b =>
                {
                    b.HasOne("PeopleSkillsApp.Models.Person", "Person")
                        .WithMany("PersonSkills")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PeopleSkillsApp.Models.Skill", "Skill")
                        .WithMany("PersonSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.Person", b =>
                {
                    b.Navigation("PersonSkills");
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.Skill", b =>
                {
                    b.Navigation("PersonSkills");
                });

            modelBuilder.Entity("PeopleSkillsApp.Models.Workplace", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
