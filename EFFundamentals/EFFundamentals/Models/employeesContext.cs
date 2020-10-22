using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFFundamentals.Models
{
    public partial class employeesContext : DbContext
    {
        public employeesContext()
        {
        }

        public employeesContext(DbContextOptions<employeesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CurrentDeptEmp> CurrentDeptEmp { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<DeptEmp> DeptEmp { get; set; }
        public virtual DbSet<DeptEmpLatestDate> DeptEmpLatestDate { get; set; }
        public virtual DbSet<DeptManager> DeptManager { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Salaries> Salaries { get; set; }
        public virtual DbSet<Titles> Titles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;database=employees;user=gonzalo;password=swordfish", x => x.ServerVersion("8.0.21-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrentDeptEmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("current_dept_emp");

                entity.Property(e => e.DeptNo)
                    .IsRequired()
                    .HasColumnName("dept_no")
                    .HasColumnType("char(4)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EmpNo).HasColumnName("emp_no");

                entity.Property(e => e.FromDate)
                    .HasColumnName("from_date")
                    .HasColumnType("date");

                entity.Property(e => e.ToDate)
                    .HasColumnName("to_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DeptNo)
                    .HasName("PRIMARY");

                entity.ToTable("departments");

                entity.HasIndex(e => e.DeptName)
                    .HasName("dept_name")
                    .IsUnique();

                entity.Property(e => e.DeptNo)
                    .HasColumnName("dept_no")
                    .HasColumnType("char(4)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DeptName)
                    .IsRequired()
                    .HasColumnName("dept_name")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<DeptEmp>(entity =>
            {
                entity.HasKey(e => new { e.EmpNo, e.DeptNo })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("dept_emp");

                entity.HasIndex(e => e.DeptNo)
                    .HasName("dept_no");

                entity.Property(e => e.EmpNo).HasColumnName("emp_no");

                entity.Property(e => e.DeptNo)
                    .HasColumnName("dept_no")
                    .HasColumnType("char(4)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FromDate)
                    .HasColumnName("from_date")
                    .HasColumnType("date");

                entity.Property(e => e.ToDate)
                    .HasColumnName("to_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.DeptNoNavigation)
                    .WithMany(p => p.DeptEmp)
                    .HasForeignKey(d => d.DeptNo)
                    .HasConstraintName("dept_emp_ibfk_2");

                entity.HasOne(d => d.EmpNoNavigation)
                    .WithMany(p => p.DeptEmp)
                    .HasForeignKey(d => d.EmpNo)
                    .HasConstraintName("dept_emp_ibfk_1");
            });

            modelBuilder.Entity<DeptEmpLatestDate>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("dept_emp_latest_date");

                entity.Property(e => e.EmpNo).HasColumnName("emp_no");

                entity.Property(e => e.FromDate)
                    .HasColumnName("from_date")
                    .HasColumnType("date");

                entity.Property(e => e.ToDate)
                    .HasColumnName("to_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<DeptManager>(entity =>
            {
                entity.HasKey(e => new { e.EmpNo, e.DeptNo })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("dept_manager");

                entity.HasIndex(e => e.DeptNo)
                    .HasName("dept_no");

                entity.Property(e => e.EmpNo).HasColumnName("emp_no");

                entity.Property(e => e.DeptNo)
                    .HasColumnName("dept_no")
                    .HasColumnType("char(4)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FromDate)
                    .HasColumnName("from_date")
                    .HasColumnType("date");

                entity.Property(e => e.ToDate)
                    .HasColumnName("to_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.DeptNoNavigation)
                    .WithMany(p => p.DeptManager)
                    .HasForeignKey(d => d.DeptNo)
                    .HasConstraintName("dept_manager_ibfk_2");

                entity.HasOne(d => d.EmpNoNavigation)
                    .WithMany(p => p.DeptManager)
                    .HasForeignKey(d => d.EmpNo)
                    .HasConstraintName("dept_manager_ibfk_1");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmpNo)
                    .HasName("PRIMARY");

                entity.ToTable("employees");

                entity.Property(e => e.EmpNo)
                    .HasColumnName("emp_no")
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(14)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasColumnType("enum('M','F')")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.HireDate)
                    .HasColumnName("hire_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasColumnType("varchar(16)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Salaries>(entity =>
            {
                entity.HasKey(e => new { e.EmpNo, e.FromDate })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("salaries");

                entity.Property(e => e.EmpNo).HasColumnName("emp_no");

                entity.Property(e => e.FromDate)
                    .HasColumnName("from_date")
                    .HasColumnType("date");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.ToDate)
                    .HasColumnName("to_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.EmpNoNavigation)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.EmpNo)
                    .HasConstraintName("salaries_ibfk_1");
            });

            modelBuilder.Entity<Titles>(entity =>
            {
                entity.HasKey(e => new { e.EmpNo, e.Title, e.FromDate })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("titles");

                entity.Property(e => e.EmpNo).HasColumnName("emp_no");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FromDate)
                    .HasColumnName("from_date")
                    .HasColumnType("date");

                entity.Property(e => e.ToDate)
                    .HasColumnName("to_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.EmpNoNavigation)
                    .WithMany(p => p.Titles)
                    .HasForeignKey(d => d.EmpNo)
                    .HasConstraintName("titles_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
