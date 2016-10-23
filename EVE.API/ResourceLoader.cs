namespace DBSoft.EVEAPI
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    public class ResourceLoader
    {
        public static IEnumerable<T> LoadResource<T>(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    return null;
                }
                var reader = new StreamReader(stream);
                return JsonConvert.DeserializeObject<List<T>>(reader.ReadToEnd());
            }
        }
    }
}