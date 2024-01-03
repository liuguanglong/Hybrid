namespace Hybrid.Web.FormEngine
{
    public static class Util
    {
       public static Type? getType(String name)
        {
            var selectedType = Type.GetType(name);
            return selectedType;
        }
    }
}
