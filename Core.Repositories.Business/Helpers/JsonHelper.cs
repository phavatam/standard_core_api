using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Business.Helpers
{
    public static class JsonHelper
    {
        public static string GetJsonContentFromFile(string folderName, string jsonFileName)
        {
            var result = "";
            string codeBase = Assembly.GetCallingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string unescapedRepresentation = Uri.UnescapeDataString(uri.Path);
            using (StreamReader r = new StreamReader(Path.Combine(Path.GetDirectoryName(unescapedRepresentation), folderName, jsonFileName)))
            {
                result = r.ReadToEnd();
            };
            return result;
        }
        public static string GetPropertyValue(this string jsonData, string propertyName)
        {
            var result = "";
            return result;
        }
        public static string GetGroupDataByName(this string jsonData, string groupName)
        {
            var result = "";
            JObject group = JObject.Parse(jsonData);
            if (group != null)
            {
                result = group.GetValue(groupName).ToString();
            }
            return result;
        }
        public static string GetArrayData(this string jsonData, string arrayName)
        {
            var result = "";
            return result;
        }
    }
}
