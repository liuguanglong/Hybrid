using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public static class BcParamExtention
    {
        public static String buildParamsString(this Dictionary<String, Object> Values, String template, BcParam[]? ParamsDataPath)
        {
            if (String.IsNullOrEmpty(template) == false)
            {
                if (ParamsDataPath != null)
                {
                    if (ParamsDataPath.Length != 0)
                    {
                        List<Object> args = new List<object>();
                        foreach (var p in ParamsDataPath)
                        {
                            args.Add(Values.getValue(p.DataPath.Trim()));
                        }
                        return String.Format(template, args.ToArray());
                    }
                    else
                    {
                        return template;
                    }
                }
            }
            return null;
        }
    }

    public class BcParam
    {
        public String DataPath { get; set; }
        public UIDataFieldType DataType { get; set; } =  UIDataFieldType.String;

        public BcParam(String dataPath) {
            this.DataPath = dataPath;
        }

        public BcParam(String dataPath,String dataType)
        {
            this.DataPath= dataPath;
            switch(dataType)
            {
                case "String":
                   this.DataType = UIDataFieldType.String; 
                    break;
                case "Int32":
                    this.DataType = UIDataFieldType.Int32;
                    break;
                case "Int64":
                    this.DataType = UIDataFieldType.Int64;
                    break;
                case "Double":
                    this.DataType = UIDataFieldType.Double;
                    break;
                case "Guid":
                    this.DataType = UIDataFieldType.Guid;
                    break;
                case "Bool":
                    this.DataType = UIDataFieldType.Boolean;
                    break;
            }
        }
    }

    public class BcParamString
    {
        public String Template { get; }
        public BcParam[]? ParamsDataPath { get;}

        public BcParamString(String Template, BcParam[]? ParamsDataPath = null)
        {
            this.Template = Template;
            this.ParamsDataPath = ParamsDataPath;
        }
    }
}
