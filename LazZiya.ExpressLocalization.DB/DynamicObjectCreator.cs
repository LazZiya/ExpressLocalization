using LazZiya.ExpressLocalization.DB.Models;
using System.Globalization;
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

namespace LazZiya.ExpressLocalization.DB
{
    internal class DynamicObjectCreator
    {
        public static TResource DbResource<TResource>(string name)
            where TResource : class, IXLDbResource
        {
            // Create a dynamic resource entity to be added
            var dynResource = new { ID = 0, Key = name };
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
            // Serialize the dynamic entity to json
            var resourceJson = JsonConvert.SerializeObject(dynResource);

            // De-serialize json to TResource type
            // so we can add it to the database
            var res = JsonConvert.DeserializeObject<TResource>(resourceJson);
#else
            // Serialzie the dynamic entity to json
            var resourceJson = JsonSerializer.Serialize(dynResource);

            // De-serialzie json to TResource type
            // so we can add it to the database
            var res = JsonSerializer.Deserialize(resourceJson, typeof(TResource)) as TResource;
#endif
            return res;
        }

        public static TTranslation DbTranslation<TTranslation>(int resId, string val)
            where TTranslation : class ,IXLDbTranslation
        {
            var cultureId = CultureInfo.CurrentCulture.Name;

            // Create a dynamic resource entity to be added
            var dynTrans = new { ID = 0, ResourceID = resId, CultureID = cultureId, Value = val, IsActive = false };
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
            // Serialize the dynamic entity to json
            var transJson = JsonConvert.SerializeObject(dynTrans);

            // De-serialize json to TResource type
            // so we can add it to the database
            var trans = JsonConvert.DeserializeObject<TTranslation>(transJson);
#else
            // Serialize the dynamic entity to json
            var transJson = JsonSerializer.Serialize(dynTrans);

            // De-serialize json to TResource type
            // so we can add it to the database
            var trans = JsonSerializer.Deserialize(transJson, typeof(TTranslation)) as TTranslation;
#endif

            return trans;
        }
    }
}
