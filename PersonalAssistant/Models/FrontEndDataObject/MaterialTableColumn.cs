using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Models.FrontEndDataObject
{
    public class MaterialTableColumn
    {
        public string field { get; set; }
        public string type { get; set; }
        public Dictionary<int?, string> lookup { get; set; }
    }
}
