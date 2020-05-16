using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public static class JsonExtends
    {

        public static string LogJson(this object obj)
        {
            var settings = JsonSerializer.CreateDefault();
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new LimitPropsContractResolver(new[] { "ID", "CreateDate", "ModifyDate" })
            }); ;
        }
        public static T JsonTo<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }

    public class LimitPropsContractResolver : DefaultContractResolver
    {
        string[] props = null;

        public LimitPropsContractResolver(string[] props)
        {
            this.props = props;
        }

        protected override IList<JsonProperty> CreateProperties(Type type,

        MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list =
            base.CreateProperties(type, memberSerialization);
            return list.Where(p => !props.Contains(p.PropertyName)).ToList();
        }
    }
}
