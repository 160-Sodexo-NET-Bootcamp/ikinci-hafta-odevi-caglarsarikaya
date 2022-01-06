using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Entities
{
    public class ClusterMember
    {
        public ContainerCluster Cluster { get; set; }
        public int ClusterId { get; set; }

        public Container Container { get; set; }
        public int ContainerId  { get; set; }
    }
}
