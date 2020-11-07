using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2.Models
{
    public class Settings
    {
        public int Take { get; set; }
        public List<string> Status { get; set; }
        public Settings()
        {
            Status = new List<string>();
        }

    }
}
