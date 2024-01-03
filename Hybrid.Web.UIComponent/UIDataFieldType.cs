namespace Hybrid.Web.UIComponent
{
    public enum UIDataFieldType
    {
        String,
        Int32,
        Int64,
        Double,
        DateTime,
        Guid,
        Boolean
    }

    public static class Extensions
    {
        public static Type ToType(this UIDataFieldType vType)
        {
            switch (vType)
            {
                case UIDataFieldType.String:
                    return typeof(string);
                case UIDataFieldType.Int32:
                    return typeof(Int32);
                case UIDataFieldType.Int64:
                    return typeof(Int64);
                case UIDataFieldType.Double:
                    return typeof(double);
                case UIDataFieldType.Boolean:
                    return typeof(bool);
                case UIDataFieldType.Guid:
                    return typeof(String);
                default:
                    return typeof(string);
            }
        }
    }
}
