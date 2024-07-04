using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace library77.Models.Library;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorsBook> AuthorsBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookBooktype> BookBooktypes { get; set; }

    public virtual DbSet<BookGerne> BookGernes { get; set; }

    public virtual DbSet<Booktype> Booktypes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Gerne> Gernes { get; set; }

    public virtual DbSet<Logg> Loggs { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<ReaderRole> ReaderRoles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=1234;database=library", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_unicode_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("authors", tb => tb.HasComment("Таблица авторов"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("код записи")
                .HasColumnName("id");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasComment("отчество")
                .HasColumnName("fullname")
                .UseCollation("utf8mb3_general_ci");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasComment("фамилия")
                .HasColumnName("lastname")
                .UseCollation("utf8mb3_general_ci");
            entity.Property(e => e.Middlname)
                .HasMaxLength(50)
                .HasComment("фамилия")
                .HasColumnName("middlname")
                .UseCollation("utf8mb3_general_ci");
        });

        modelBuilder.Entity<AuthorsBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("authors_book")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorsId).HasColumnName("authors_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("book")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            //изменил publisherid на publisher
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.YearOfPublication).HasColumnName("year_of_publication");
        });

        modelBuilder.Entity<BookBooktype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("book_booktype")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.BookTypeId).HasColumnName("book_type_id");
        });

        modelBuilder.Entity<BookGerne>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("book_gernes")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.GernesId).HasColumnName("gernes_id");
        });

        modelBuilder.Entity<Booktype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("booktype")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ElectronicVersion).HasColumnName("electronic_version");
            entity.Property(e => e.PaperVersion).HasColumnName("paper_version");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("city")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gerne>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("gernes")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Logg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("logg")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.DateOfDelivery)
                .HasColumnType("timestamp")
                .HasColumnName("date_of_delivery");
            entity.Property(e => e.DateOfIssue)
                .HasColumnType("timestamp")
                .HasColumnName("date_of_issue");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("publisher")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reader");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateAdd).HasColumnName("date_add");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasColumnName("fullname")
                .UseCollation("utf8mb3_general_ci");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname")
                .UseCollation("utf8mb3_general_ci");
            entity.Property(e => e.LibraryCard)
                .HasMaxLength(50)
                .HasColumnName("library_card")
                .UseCollation("utf8mb3_general_ci");
            entity.Property(e => e.Middlname)
                .HasMaxLength(100)
                .HasColumnName("middlname")
                .UseCollation("utf8mb3_general_ci");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username")
                .UseCollation("utf8mb3_general_ci");
        });

        modelBuilder.Entity<ReaderRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reader_roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.RolesId).HasColumnName("roles_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles", tb => tb.HasComment("Все роли системы"));

            entity.Property(e => e.Id)
                .HasComment("код роли")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasComment("название роли")
                .HasColumnName("title");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_roles", tb => tb.HasComment("роли пользователей"));

            entity.Property(e => e.Id)
                .HasComment("код записи")
                .HasColumnName("id");
            entity.Property(e => e.RoleId)
                .HasComment("код роли")
                .HasColumnName("role_id");
            entity.Property(e => e.UserId)
                .HasComment("код пользователя")
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
