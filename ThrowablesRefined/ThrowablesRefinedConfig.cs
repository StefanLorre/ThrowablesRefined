using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ThrowablesRefined
{
    public class ThrowablesRefinedConfig
    {
        public ThrowablesRefinedConfig()
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            using (XmlReader xmlReader = XmlReader.Create(ThrowablesRefinedConfig.FILE_NAME, xmlReaderSettings))
            {
                this.config.Load(xmlReader);
            }
        }
        private static string FILE_NAME = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/ThrowablesRefinedConfig.xml";
        public XmlDocument config = new XmlDocument();
    }
}
