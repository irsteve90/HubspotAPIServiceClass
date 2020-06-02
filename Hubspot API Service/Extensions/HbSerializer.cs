using Hubspot_API_Service.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Hubspot_API_Service.Extensions
{
    class HbSerializer
    {
        public bool batchMode { get; set; } = false;

        public string SerializeEntity<T>(object model) where T : class
        {

            object entityModel = PrintTModelPropertyAndValue(model);
            if (entityModel != null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(entityModel);
            }

            return null;
        }

        public string SerializeEntities<T>(List<T> modelList)
        {
            List<object> objects = new List<object>();

            foreach (object model in modelList)
            {
                objects.Add(PrintTModelPropertyAndValue(model));
            }

            try
            {
                if (objects != null)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(objects);
                }
            }
            catch
            {
                throw new System.InvalidOperationException("Unable to serialize one of the objects provided");
            }

            return null;
        }


        public dynamic PrintTModelPropertyAndValue(object obj)
        {
            dynamic mapped = new ExpandoObject();
            mapped.properties = new List<hubSpotEntityDataModel>();
            bool emailProperty = false;

            var allProps = obj.GetType().GetProperties();

            try
            {
                foreach (var prop in allProps)
                {
                    if (prop.HasIgnoreDataMemberAttribute()) { continue; }

                    var propSerializedName = prop.GetPropSerializedName();

                    var propValue = prop.GetValue(obj);
                    var value = propValue.IsComplexType() ? propValue : propValue?.ToString();

                    var item = new hubSpotEntityDataModel
                    {
                        property = propSerializedName,
                        value = value
                    };

                    if (prop.GetPropSerializedName().ToLower() == "email")
                    {
                        emailProperty = true;
                        if (string.IsNullOrEmpty(value?.ToString()))
                        {
                            throw new System.InvalidOperationException("The field email, must not be empty");
                        }
                        if(batchMode)
                            mapped.email = value;
                    }

                    if (item.value == null)
                        continue;

                    mapped.properties.Add(item);


                }
                if (!emailProperty)
                {
                    throw new System.InvalidOperationException("The field 'email', isn't included in the model");
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }

            return mapped;
        }
    }

}
