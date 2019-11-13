using CSNeedToSerializeAttributeLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSSerializerLib
{
    public class Serialize
    {
        //2019. 11. 09. 18:20

        public void SerializeMethod(string path, Type _anyType)
        {
            Generate _generate = new Generate();

            System.Reflection.Assembly a = System.Reflection.Assembly.Load(_anyType.Namespace);
            var allTypes = a.GetTypes();

            foreach (var type in allTypes)
            {
                Object[] serializeAttribute = type.GetCustomAttributes(typeof(NeedToSerialize), true);

                if (serializeAttribute.Length > 0)
                {
                    _generate.SerializeClasses(type, path);
                }
            }
        }
    }
}
