using BMBSOFT.GIS.CORE.Extensions;
using System.Collections.Generic;
using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace BMBSOFT.GIS.CORE.Helper
{
    public class UtilHelper
    {
        public static string BuildPathWithParentPath(string targetFolder, string parentPath, string childPath)
        {
            return $"{targetFolder}/{parentPath}/{childPath}";
        }

        public static string GetPath(string filePreview)
        {
            string pattern = @"[/\\]{1,2}|\\\\";
            string replacement = "\\";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(filePreview, replacement);

            return result;
        }
        public static void CopyProperties(object dstObj, object srcObj)
        {
            Type typeDst = dstObj.GetType();
            Type typeSrc = srcObj.GetType();

            PropertyInfo[] propertiesSrc = typeSrc.GetProperties();

            foreach (PropertyInfo propertySrc in propertiesSrc)
            {
                PropertyInfo propertyDst = typeDst.GetProperty(propertySrc.Name);
                if (propertyDst != null)
                {
                    if (propertySrc.GetValue(srcObj) != null)
                    {
                        propertyDst.SetValue(dstObj, propertySrc.GetValue(srcObj));
                    }
                }
            }
        }
        public static string ConvertToUnSign(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD)).ToLower();
        }

        public static string SenderRemoveOfsSpecialCharacter(string input)
        {

            input = input.Replace("&", "va");
            input = input.Replace("%", "per");
            input = input.Replace(",", ".");
            input = input.Replace("~", ".");
            input = input.Replace("`", ".");
            input = input.Replace("_", ".");
            input = input.Replace("?", ".");
            input = input.Replace("}", ".");
            input = input.Replace("{", ".");
            input = input.Replace("'", ".");
            input = input.Replace("<", ".");
            input = input.Replace(">", ".");
            input = input.Replace("/", ".");
            input = input.Replace(@"\", ".");
            return input;

        }
        public static List<FilterExtensions.FilterParams> GetFilterParams(object model)
        {
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var filterParams = new List<FilterExtensions.FilterParams>();
            string[] ignoreNames = new string[] { "PageIndex", "PageSize", "Sorting" };
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(model);
                if (!ignoreNames.Contains(property.Name) && value != null && !string.IsNullOrEmpty(value.ToString())
                    && (!double.TryParse(value.ToString(), out double d) || double.TryParse(value.ToString(), out double result) && result != 0)
                )
                {
                    filterParams.Add(new FilterExtensions.FilterParams()
                    {
                        ColumnName = property.Name,
                        FilterValue = value.ToString(),
                    });
                }
            }

            return filterParams;
        }
    }
}
