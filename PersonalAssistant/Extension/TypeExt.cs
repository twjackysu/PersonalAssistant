using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PersonalAssistant.Data;
using PersonalAssistant.Models.FrontEndDataObject;

namespace PersonalAssistant.Extension
{
    public static class TypeExt
    {
        //MaterialTable json format example:
        //[
        //    { title: "Adı", field: "name" },
        //    { title: "Soyadı", field: "surname" },
        //    { title: "Doğum Yılı", field: "birthYear", type: "numeric" },
        //    {
        //      title: "Doğum Yeri",
        //      field: "birthCity",
        //      lookup: { 34: "İstanbul", 63: "Şanlıurfa" }
        //    }
        //]
        public static MaterialTableColumn[] GetMaterialTableColumns(this Type type, Dictionary<string, Dictionary<int?, string>> allForeignkey)
        {
            var allFieldRemoveOwnerIDandID = type.GetProperties().Where(x => x.Name != "OwnerID" && x.Name != "ID");

            return allFieldRemoveOwnerIDandID.Where(x => allForeignkey == null || x.PropertyType.IsEnum || !allForeignkey.Any(y => y.Key.Replace("ID", "") == x.Name))
                .Select(x => {
                    var result = new MaterialTableColumn()
                    {
                        field = x.Name
                    };
                    if (allForeignkey != null && allForeignkey.ContainsKey(x.Name))
                    {
                        result.lookup = allForeignkey[x.Name];
                    }
                    switch (x.PropertyType)
                    {
                        case Type type when type == typeof(decimal):
                            result.type = "currency";
                            break;
                        case Type type when type == typeof(DateTime):
                            result.type = "date";
                            break;
                    }
                    return result;
                }).ToArray();
        }
    }
}
