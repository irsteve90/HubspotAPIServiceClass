using Hubspot_API_Service.Constants;
using Hubspot_API_Service.Dto;
using Hubspot_API_Service.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Hubspot_API_Service.API
{

    public class HsContactApi : IHsContactApi
    {
        private readonly HttpClient _httpClient;
        private HbSerializer _serializer;
        public HsContactApi(string hapikey)
        {
            HsApiEndpoints.hapikey = hapikey;
            _serializer = new HbSerializer();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(HsApiEndpoints.HbBaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<(HttpStatusCode statusCode, string msg, long vid)> CreateOrUpdateUserContact(object contact, string email)
        {
            string msg = "ok";
            requestedHubspotVidModel returnedData = null;
            var serializedContact = _serializer.SerializeEntity<object>(contact);
            string endpoint = HsApiEndpoints.Hubspot_CreateOrUpdate(email);

            var response = await _httpClient.PostAsync(endpoint, new StringContent(serializedContact, Encoding.Unicode, "application/json")).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                msg = "Error: issue Creating/Updating hubspot contact, ensure properties exists";

            if (response.StatusCode == HttpStatusCode.Conflict)
                msg = "Error: The email to update differs from the one provided in the model";

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    returnedData = JsonConvert.DeserializeObject<requestedHubspotVidModel>(await response.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    msg = "Error deserialising created/updated contact";
                }
            }

            return (response.StatusCode, msg, returnedData.vid);
        }

        public async Task<(HttpStatusCode statusCode, string msg)> CreateOrUpdateUserContacts<T>(List<T> contacts)
        {
            string msg = "ok";
            _serializer.batchMode = true;
            var serializedContact = _serializer.SerializeEntities(contacts);

            string endpoint = HsApiEndpoints.Hubspot_CreateOrUpdateBatch;

            var response = await _httpClient.PostAsync(endpoint, new StringContent(serializedContact, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                msg = "Error: issue Creating/Updating hubspot contacts, ensure properties exists";

            return (response.StatusCode, msg);
        }

        public async Task<(HttpStatusCode statusCode, string msg)> Delete(long id)
        {
            string msg = "ok";
            _serializer.batchMode = true;
 
            string endpoint = HsApiEndpoints.Hubspot_Delete(id);
            var response = await _httpClient.DeleteAsync(endpoint).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                msg = "Error: issue Deleting contact";

            return (response.StatusCode, msg);
        }
    }
}
