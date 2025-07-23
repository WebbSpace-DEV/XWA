using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace XWA.Core.Converters;

public static class JsonConverter
{
    public static string Beautify(string json)
    {
        return JValue.Parse(json).ToString(Formatting.Indented);
    }
}

public static class JsonConverters<TEntity> where TEntity : class
{
    public static string Serialize(TEntity model)
    {
        JsonSerializerSettings settings = new()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        return JsonConvert.SerializeObject(model, settings);
    }

    public static TEntity Deserialize(string json)
    {
        JsonSerializerSettings settings = new()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include
        };

#pragma warning disable CS8603 // Possible null reference return.
        return JsonConvert.DeserializeObject<TEntity>(json, settings);
#pragma warning restore CS8603 // Possible null reference return.
    }

    public static string ListSerialize(IList<TEntity> model)
    {
        JsonSerializerSettings settings = new()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        return JsonConvert.SerializeObject(model, settings);
    }

    public static IList<TEntity> ListDeserialize(string json)
    {
        JsonSerializerSettings settings = new()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include
        };

#pragma warning disable CS8603 // Possible null reference return.
        return JsonConvert.DeserializeObject<IList<TEntity>>(json, settings);
#pragma warning restore CS8603 // Possible null reference return.
    }
}

public class NullSetterContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty prop = base.CreateProperty(member, memberSerialization);
        if (!prop.Writable)
        {
            if (member is PropertyInfo propertyInfo)
            {
                if (propertyInfo.GetSetMethod() is null)
                {
                    prop.ShouldSerialize = i => false;
                    prop.ShouldDeserialize = i => false;
                    prop.Ignored = true;
                }
            }
        }

        return prop;
    }
}