using System;
using System.IO;
using System.Runtime.Serialization;

namespace DataConvert
{
    public class Serializer
    {
        private IFormatter formatter;

        public Serializer(IFormatter formatter)
        {
            this.formatter = formatter;
        }

        public byte[] Serialize(object obj)
        {
            try
            {
                if (obj != null)
                {

                    using (MemoryStream ms = new MemoryStream())
                    {
                        formatter.Serialize(ms, obj);
                        return ms.ToArray();
                    }
                }
                return null;
            }
            catch { return null; }
        }

        public void Serialize(Stream stream,object obj)
        {           
            try
            {
                if (obj != null)
                {
                    formatter.Serialize(stream, obj);
                }
            }
            catch { }
        }

        public T Deserialize<T>(Stream stream)
        {
            try
            {
                return (T)formatter.Deserialize(stream);
            }
            catch(Exception e)
            {
                return default(T);
            }
        }
    }
}
