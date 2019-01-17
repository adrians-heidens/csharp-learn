using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace JsonNetParse
{
    /// <summary>
    /// An example of custom contract resolver which overwrites property
    /// name to a data type name thus ignoring attribute settings.
    /// </summary>
    /// <remarks>
    /// There are few other methods which creates JsonProperty which should
    /// be considered for overriding.
    /// </remarks>
    class MyContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var p = base.CreateProperty(member, memberSerialization);
            p.PropertyName = member.Name;
            return p;
        }
    }
}
