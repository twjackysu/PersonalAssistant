using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using PersonalAssistant.Models.FrontEndDataObject;

namespace PersonalAssistant.Extension
{
    public static class TypeExt
    {
        public static MaterialTableColumn[] GetMaterialTableColumns(this Type type)
        {
            return type.GetProperties().Where(x => x.Name != "OwnerID" && x.Name != "ID")
                .Select(x => new MaterialTableColumn
                { 
                    title = Regex.Replace(x.Name, "([a-z])([A-Z])", "$1 $2") ,
                    field = x.Name
                }).ToArray();
        }
    }
}
