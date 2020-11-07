using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2.Models
{
    public class Category : ObservableCollection<string>
    {
        public Category()
        {
            Add("Internet");
            Add("Hardwere");
            Add("Softwere");
        }

    }
}
