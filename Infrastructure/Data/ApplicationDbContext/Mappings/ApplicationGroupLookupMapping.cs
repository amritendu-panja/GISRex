using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ApplicationDbContext.Mappings
{
	public class ApplicationGroupLookupMapping : IEntityTypeConfiguration<ApplicationGroupLookup>
	{
		public void Configure(EntityTypeBuilder<ApplicationGroupLookup> builder)
		{
			builder
				.HasKey(u => u.GroupId);
			builder
				.Property(u => u.GroupId)
				.ValueGeneratedOnAdd();			
				
		}
	}
}
