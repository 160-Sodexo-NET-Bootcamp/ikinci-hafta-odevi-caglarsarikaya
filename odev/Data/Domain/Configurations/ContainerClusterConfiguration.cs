using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Configurations
{
    public class ContainerClusterConfiguration : IEntityTypeConfiguration<ContainerCluster>
    {
        public void Configure(EntityTypeBuilder<ContainerCluster> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.ToTable("ContainerCluster");
        }
    }
}
