using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OSP.SudokuSolver.WebApp.Models;

namespace OSP.SudokuSolver.WebApp.Tester
{
    public class WebApiClient
    {
        protected readonly string _endpoint;

        public WebApiClient(string endpoint)
        {
            _endpoint = endpoint;
        }

        public T GetList<T>(int top = 0, int skip = 0)
        {
            using (var httpClient = NewHttpClient())
            {
                var endpoint = _endpoint + "?";
                var parameters = new List<string>();

                if (top > 0)
                    parameters.Add(string.Concat("$top=", top));

                if (skip > 0)
                    parameters.Add(string.Concat("$skip=", skip));

                endpoint += string.Join("&", parameters);

                var response = httpClient.GetAsync(endpoint).Result;

                response.EnsureSuccessStatusCode();

                var obj = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                return obj;
            }
        }

        public T GetItem<T>(int id)
        {
            return GetItem<T>(id, null);
        }

        public T GetItem<T>(int id, int? squareid)
        {
            using (var httpClient = NewHttpClient())
            {
                var requestUri = _endpoint + id;

                if (squareid.HasValue)
                    requestUri += "/" + squareid.Value;

                var response = httpClient.GetAsync(requestUri).Result;

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public string Post<T>(T data)
        {
            return Post<T>(null, data);
        }

        public string Post<T>(System.Nullable<int> id, T data)
        {
            using (var httpClient = NewHttpClient())
            {
                var requestMessage = GetHttpRequestMessage<T>(data);

                var requestUri = _endpoint;

                if (id.HasValue)
                    requestUri += id;

                var result = httpClient.PostAsync(requestUri, requestMessage.Content).Result;

                return result.Content.ToString();
            }
        }


        public string Put<T>(int id, T data)
        {
            using (var httpClient = NewHttpClient())
            {
                var requestMessage = GetHttpRequestMessage<T>(data);

                var result = httpClient.PutAsync(_endpoint + id, requestMessage.Content).Result;

                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public string Delete(int id)
        {
            using (var httpClient = NewHttpClient())
            {
                var result = httpClient.DeleteAsync(_endpoint + id).Result;

                return result.Content.ToString();
            }
        }

        protected System.Net.Http.HttpRequestMessage GetHttpRequestMessage<T>(T data)
        {
            //var mediaType = new MediaTypeHeaderValue("application/json");
            //var jsonSerializerSettings = new JsonSerializerSettings();
            //jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());

            //var jsonFormatter = new JsonNetFormatter(jsonSerializerSettings);

            // var requestMessage = new HttpRequestMessage<T>(data, mediaType, new MediaTypeFormatter[] { jsonFormatter });

            //var requestMessage = new HttpRequestMessage(HttpMethod.Post,  HttpRequestMessage<T>(data, mediaType, new MediaTypeFormatter[] { jsonFormatter });

            // return requestMessage;

            var message = new HttpRequestMessage();
            //message.Content = data.ToString();

            return message;
        }

        protected HttpClient NewHttpClient()
        {
            return new HttpClient();
        }
    }
}