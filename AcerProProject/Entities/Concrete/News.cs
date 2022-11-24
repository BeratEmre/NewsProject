using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class News : IEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public int UserId { get; set; }
    }
}
