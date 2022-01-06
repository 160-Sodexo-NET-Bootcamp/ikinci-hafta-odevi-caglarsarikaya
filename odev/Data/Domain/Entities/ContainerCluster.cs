using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Entities
{
    public class ContainerCluster
    {
        public int Id { get; set; } 
        public int ContainerId { get; set; } 
        public int GroupId { get; set; } 
        public double LocationX { get; set; }
        public double LocationY { get; set; }
    }
}
