using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Models.FrontEndDataObject
{
    public class MaterialTable<T>
    {
        public MaterialTableColumn[] columns { get; set; }
        public T[] data { get; set; }
    }
}
