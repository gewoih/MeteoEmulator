using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace MeteoEmulator.Utils
{
    public class CustomCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var res = base.CreateProperty(member, memberSerialization);

            if (res.PropertyName != null && res.PropertyName.ToLower().Equals("id"))
                res.PropertyName = "ID";

            return res;
        }
    }
}
