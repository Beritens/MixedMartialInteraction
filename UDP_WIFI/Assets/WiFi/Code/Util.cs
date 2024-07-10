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
            
            string newJson = splits[0].Trim('"');
            newJson = newJson.Replace('\'', '"');
            return newJson;
        }
    }
}