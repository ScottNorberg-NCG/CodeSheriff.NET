using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.Data;

internal class ApplicationDbContext : DbContext
{
    public virtual DbSet<CveInfo> CveInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CveInfo>(entity =>
        {
            entity.HasKey("CveId");

            entity.HasMany(e => e.CveReferences)
                .WithOne(r => r.CveInfo)
                .HasForeignKey(e => e.CveId);

            entity.HasMany(e => e.ParsedCpes)
                .WithOne(r => r.CveInfo)
                .HasForeignKey(e => e.CveId);
        });

        modelBuilder.Entity<CveReference>(entity =>
        {
            entity.HasKey("Id");
        });

        modelBuilder.Entity<ParsedCpe>(entity =>
        {
            entity.HasKey("Id");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
        base.OnConfiguring(optionsBuilder);
    }

    public void RebuildDatabase()
    {
        this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
        this.Database.Migrate();
    }
}
