using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class RolsList
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public List<string> Actions { get; set; }
    }
}
