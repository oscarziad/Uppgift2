using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2.Models
{
    public class Status : ObservableCollection<string>
    {
        public Status()
        {
            Add("Pending");
            Add("Active");
            Add("Closed");


        }

    }
}
