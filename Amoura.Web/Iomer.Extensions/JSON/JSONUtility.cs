using System.Web.Script.Serialization;

namespace Iomer.Extensions.JSON
{
    public static class JSONUtility<T>
    {
        public static string Stringify(T value)
        {
            var sb = new System.Text.StringBuilder();
            var serializer = new JavaScriptSerializer();
            serializer.Serialize(value, sb);
            return sb.ToString();
        }
        public static T GetData(string s)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedObject = serializer.Deserialize<T>(s);
            return deserializedObject;
        }
    }
}
