using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Models.FrontEndDataObject
{
    public class MaterialTable
    {
        public string title { get; set; }
        public MaterialTableColumn[] columns { get; set; }
        public dynamic[] data { get; set; }
    }
}
