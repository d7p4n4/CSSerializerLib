using CSAc4yClass.Class;
using CSPersistentAttributeLib;
using CSGUIDLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CSAc4yAnnotationsFW;

namespace CSSerializerLib
{
    public class Generate
    {
        //Date: 2019. 11. 09. 15:40
        public void SerializeClasses(Type anyType, string PATH)
        {
            string _guidValue = "";
            Persistent persistent = null;
            Boolean isPersistent = false;
            Ac4yEmbedded embedded = null;
            try
            {
                persistent = (Persistent)anyType.GetCustomAttribute(typeof(Persistent), true);
                if (persistent != null)
                {
                    isPersistent = true;
                }
                GUID _guid = (GUID)anyType.GetCustomAttributes(typeof(GUID), true).First();
                _guidValue = _guid.getGuid();
            }
            catch (Exception _exception)
            {
                Console.WriteLine(_exception.Message);
            }
            PropertyInfo[] _propInf = anyType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            Ac4yClass _ac4yClass1 = new Ac4yClass(anyType.Name);
            _ac4yClass1.Namespace = anyType.Namespace;
            _ac4yClass1.Ancestor = anyType.BaseType.Name;
            _ac4yClass1.GUID = _guidValue;
            foreach (var _prop in _propInf)
            {

                if (_prop.PropertyType.ToString().StartsWith("System.Collections.Generic."))
                {
                    string type = _prop.PropertyType.ToString();
                    string outType = type.Substring(0, type.IndexOf("`")).Replace("System.Collections.Generic.", "");
                    string innerType = type.Substring(type.IndexOf("`"));
                    string finalInnerType = innerType.Substring(innerType.LastIndexOf(".") + 1).Replace("]", "");
                    Console.WriteLine(finalInnerType);

                    _ac4yClass1.PropertyList.Add(new Ac4yProperty(_prop.Name, outType + "<" + finalInnerType + ">"));
                }
                else
                {
                    //ezt szeretném debuggolni h lássam mit ír a string-be
                    IEnumerable<CustomAttributeData> attributes = _prop.CustomAttributes;
                    foreach(var attribute in attributes)
                    {
                        string type = attribute.AttributeType.ToString();
                    }
                    _ac4yClass1.PropertyList.Add(new Ac4yProperty(_prop.Name, _prop.PropertyType.Name));
                }
            }
            SerializeAsXml2TextFile(_ac4yClass1.GetType(), _ac4yClass1, _ac4yClass1.Name, PATH, isPersistent);
        }
        static void SerializeAsXml2TextFile(Type aType, Object aObject, String aObjectName, String aPath, Boolean isPersistent)
        {
            XmlSerializer _xmlSerializer = new XmlSerializer(aType);
            TextWriter textWriter = null;
            if (isPersistent)
            {
                textWriter = new StreamWriter(aPath + aObjectName + "@" + aType.Name + "Persistent.xml");
            }
            else
            {
                textWriter = new StreamWriter(aPath + aObjectName + "@" + aType.Name + ".xml");
            }
            _xmlSerializer.Serialize(textWriter, aObject);
            textWriter.Close();
        }

    }
}
