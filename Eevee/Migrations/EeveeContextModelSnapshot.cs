﻿// <auto-generated />
using System;
using Eevee.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eevee.Migrations
{
    [DbContext(typeof(EeveeContext))]
    partial class EeveeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Eevee.Models.AccountType", b =>
                {
                    b.Property<int>("AccountTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountTypeID");

                    b.ToTable("AccountType");
                });

            modelBuilder.Entity("Eevee.Models.AdvertiserType", b =>
                {
                    b.Property<int>("AdvertiserTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdvertiserTypeID");

                    b.ToTable("AdvertiserType");
                });

            modelBuilder.Entity("Eevee.Models.Album", b =>
                {
                    b.Property<int>("AlbumID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArtistID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AlbumID");

                    b.HasIndex("ArtistID");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("Eevee.Models.Artist", b =>
                {
                    b.Property<int>("ArtistID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Listens")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WordVec")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtistID");

                    b.ToTable("Artist");
                });

            modelBuilder.Entity("Eevee.Models.Genre", b =>
                {
                    b.Property<int>("GenreID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WordVec")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenreID");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("Eevee.Models.History", b =>
                {
                    b.Property<int>("HistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Progress")
                        .HasColumnType("real");

                    b.Property<int>("SongID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("HistoryID");

                    b.HasIndex("SongID");

                    b.HasIndex("UserID");

                    b.ToTable("History");
                });

            modelBuilder.Entity("Eevee.Models.Instrument", b =>
                {
                    b.Property<int>("InstrumentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("GenreID")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InstrumentManufacturerID")
                        .HasColumnType("int");

                    b.Property<int>("InstrumentTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InstrumentID");

                    b.HasIndex("GenreID");

                    b.HasIndex("InstrumentManufacturerID");

                    b.HasIndex("InstrumentTypeID");

                    b.ToTable("Instrument");
                });

            modelBuilder.Entity("Eevee.Models.InstrumentManufacturer", b =>
                {
                    b.Property<int>("InstrumentManufacturerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InstrumentManufacturerID");

                    b.ToTable("InstrumentManufacturer");
                });

            modelBuilder.Entity("Eevee.Models.InstrumentType", b =>
                {
                    b.Property<int>("InstrumentTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InstrumentTypeID");

                    b.ToTable("InstrumentType");
                });

            modelBuilder.Entity("Eevee.Models.Note", b =>
                {
                    b.Property<int>("NoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Filepath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InstrumentTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NoteID");

                    b.HasIndex("InstrumentTypeID");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Eevee.Models.Playlist", b =>
                {
                    b.Property<int>("PlaylistID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PlaylistID");

                    b.HasIndex("UserID");

                    b.ToTable("Playlist");
                });

            modelBuilder.Entity("Eevee.Models.PlaylistSongAssignment", b =>
                {
                    b.Property<int>("PlaylistSongAssignmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PlaylistID")
                        .HasColumnType("int");

                    b.Property<int>("SongID")
                        .HasColumnType("int");

                    b.HasKey("PlaylistSongAssignmentID");

                    b.HasIndex("PlaylistID");

                    b.HasIndex("SongID");

                    b.ToTable("PlaylistSongAssignment");
                });

            modelBuilder.Entity("Eevee.Models.Song", b =>
                {
                    b.Property<int>("SongID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlbumID")
                        .HasColumnType("int");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Filepath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FreqVec")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenreID")
                        .HasColumnType("int");

                    b.Property<int>("Listens")
                        .HasColumnType("int");

                    b.Property<string>("Lyrics")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WordVec")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SongID");

                    b.HasIndex("AlbumID");

                    b.HasIndex("GenreID");

                    b.ToTable("Song");
                });

            modelBuilder.Entity("Eevee.Models.SongInstrumentAssignment", b =>
                {
                    b.Property<int>("SongInstrumentAssignmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("InstrumentID")
                        .HasColumnType("int");

                    b.Property<int>("SongID")
                        .HasColumnType("int");

                    b.HasKey("SongInstrumentAssignmentID");

                    b.ToTable("SongInstrumentAssignment");
                });

            modelBuilder.Entity("Eevee.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreferenceVector")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Eevee.Models.UserAccountTypeAssignment", b =>
                {
                    b.Property<int>("UserAccountTypeAssignmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountTypeID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserAccountTypeAssignmentID");

                    b.HasIndex("AccountTypeID");

                    b.HasIndex("UserID");

                    b.ToTable("UserAccountTypeAssignment");
                });

            modelBuilder.Entity("Eevee.Models.Album", b =>
                {
                    b.HasOne("Eevee.Models.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eevee.Models.History", b =>
                {
                    b.HasOne("Eevee.Models.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eevee.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eevee.Models.Instrument", b =>
                {
                    b.HasOne("Eevee.Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eevee.Models.InstrumentManufacturer", "InstrumentManufacturer")
                        .WithMany()
                        .HasForeignKey("InstrumentManufacturerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eevee.Models.InstrumentType", "InstrumentType")
                        .WithMany()
                        .HasForeignKey("InstrumentTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eevee.Models.Note", b =>
                {
                    b.HasOne("Eevee.Models.InstrumentType", "InstrumentType")
                        .WithMany()
                        .HasForeignKey("InstrumentTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eevee.Models.Playlist", b =>
                {
                    b.HasOne("Eevee.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eevee.Models.PlaylistSongAssignment", b =>
                {
                    b.HasOne("Eevee.Models.Playlist", "Playlist")
                        .WithMany()
                        .HasForeignKey("PlaylistID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eevee.Models.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eevee.Models.Song", b =>
                {
                    b.HasOne("Eevee.Models.Album", "Album")
                        .WithMany()
                        .HasForeignKey("AlbumID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eevee.Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eevee.Models.UserAccountTypeAssignment", b =>
                {
                    b.HasOne("Eevee.Models.AccountType", "AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eevee.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
