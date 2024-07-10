using Unity.VisualScripting;

namespace LocalNetworking
{
    public static class Util
    {
        public static string PackJson(string json)
        {
            string newJson = json.Trim('"');
            newJson = json.Replace('"', '\'');
            return newJson;
        }

         public static string UnpackJson(string data)
        {
            if (!data.Contains("/end/") || !data.Contains("{"))
            {
                return null;
            }
            string[] splits = data.Split("/end/");
            string d = splits[0];
            if (splits.Length > 1 && !splits[0].Contains("{"))
            {
                if (splits[1].Contains("/end/"))
                {
                    d = splits[1];
                }
                else
                {
                    return null;
                }
            }
            
            string newJson = d.Trim('"');
            newJson = newJson.Replace('\'', '"');
            return newJson;
        }
    }
}