namespace Hubspot_API_Service.Constants
{
    public static class HsApiEndpoints
    {
        public static string hapikey = null;
        public const string HbBaseUrl = "https://api.hubapi.com";

        //CREATE OR UPDATE HUBSPOT USER 
        public static string Hubspot_CreateOrUpdate(string Email)
        {
            return $"/contacts/v1/contact/createOrUpdate/email/{Email}/?hapikey={hapikey}";
        }

        public static string Hubspot_CreateOrUpdateBatch = $"/contacts/v1/contact/batch/?hapikey={hapikey}";

        public static string Hubspot_Delete(long id)
        {
            return $"/contacts/v1/contact/vid/{id}?hapikey={hapikey}";
        }
    }
}
