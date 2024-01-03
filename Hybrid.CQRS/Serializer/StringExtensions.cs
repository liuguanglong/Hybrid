using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Client.Serializer
{
    /// <summary>
    /// Copied from https://github.com/jquense/StringUtils
    /// </summary>
    public static class StringExtensions
    {
        public static string StripIndent(this string str) => StringUtils.StripIndent(str);

        public static IEnumerable<string> ToWords(this string str) => StringUtils.ToWords(str);

        public static string ToUpperFirst(this string str) => StringUtils.ToUpperFirst(str);

        public static string ToLowerFirst(this string str) => StringUtils.ToLowerFirst(str);

        public static string Capitalize(this string str) => StringUtils.Capitalize(str);

        public static string ToCamelCase(this string str) => StringUtils.ToCamelCase(str);

        public static string ToConstantCase(this string str) => StringUtils.ToConstantCase(str);

        public static string ToUpperCase(this string str) => StringUtils.ToUpperCase(str);

        public static string ToLowerCase(this string str) => StringUtils.ToLowerCase(str);


        public static string ToPascalCase(this string str) => StringUtils.ToPascalCase(str);


        public static string ToKebabCase(this string str) => StringUtils.ToKebabCase(str);


        public static string ToSnakeCase(this string str) => StringUtils.ToSnakeCase(str);
    }
}
