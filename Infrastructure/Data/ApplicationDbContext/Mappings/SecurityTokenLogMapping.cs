﻿using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
    public class SecurityTokenLogMapping : IEntityTypeConfiguration<SecurityTokenLog>
    {
        public void Configure(EntityTypeBuilder<SecurityTokenLog> builder)
        {
            builder.HasKey(e => e.LogId);
            builder.Property(e => e.LogId)
                .ValueGeneratedOnAdd();
            builder
                .HasOne(e => e.User)
                .WithMany(u => u.SecurityTokenLogs)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
