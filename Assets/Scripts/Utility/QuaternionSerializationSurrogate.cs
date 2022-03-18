using System.Runtime.Serialization;
using UnityEngine;

namespace Utility
{
    public class QuaternionSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Quaternion q = (Quaternion) obj;
            
            info.AddValue("w", q.w);
            info.AddValue("x", q.x);
            info.AddValue("y", q.y);
            info.AddValue("z", q.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion q = (Quaternion) obj;

            q.w = (float) info.GetValue("w", typeof(float));
            q.x = (float) info.GetValue("x", typeof(float));
            q.y = (float) info.GetValue("y", typeof(float));
            q.z = (float) info.GetValue("z", typeof(float));
            
            return q;
        }
    }
}