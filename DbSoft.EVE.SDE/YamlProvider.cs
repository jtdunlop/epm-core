namespace DBSoft.EVE.SDE
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;

    public class YamlProvider : IYamlProvider
    {
        private readonly string _resource;
        private readonly Assembly _assembly;

        public YamlProvider(string resource)
        {
            _resource = resource;
            _assembly = Assembly.GetExecutingAssembly();
        }

        public Stream GetYamlStream()
        {
            var zipped = _assembly.GetManifestResourceStream(_resource);
            if ( zipped == null )
            {
                throw new NullReferenceException();
            }
            var archive = new ZipArchive(zipped);
            return archive.Entries.First().Open();
        }
    }
}