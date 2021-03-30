using Microsoft.EntityFrameworkCore;
using PolygonProp.DAL.DataContext.Models;

#nullable disable

namespace PolygonProp.DAL.DataContext
{
    public partial class PolygonContext : DbContext
    {
        public PolygonContext()
        {
        }

        public PolygonContext(DbContextOptions<PolygonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Polygon> Polygons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Polygon;Integrated security=True", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Polygon>(entity =>
            {
                entity.ToTable("Polygon");

                //entity.Property(e => e.Id)

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnType("geometry");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
