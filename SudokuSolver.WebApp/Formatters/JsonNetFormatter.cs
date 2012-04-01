using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OSP.SudokuSolver.WebApp.Formatters
{
    public class JsonNetFormatter : MediaTypeFormatter
    {
        private JsonSerializerSettings _jsonSerializerSettings;

        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();

            // Fill out the mediatype and encoding we support
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            Encoding = new UTF8Encoding(false, true);
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(IKeyValueModel))
            {
                return false;
            }

            return true;
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        //protected override Task<object> OnReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        //{
        //    // Create a serializer
        //    JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

        //    // Create task reading the content
        //    return Task.Factory.StartNew(() =>
        //   {
        //       using (StreamReader streamReader = new StreamReader(stream, Encoding))
        //       {
        //           using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
        //           {
        //               return serializer.Deserialize(jsonTextReader, type);
        //           }
        //       }
        //   });
        //}

        //protected override Task OnWriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        //{
        //    // Create a serializer
        //    JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

        //    // Create task writing the serialized content
        //    return Task.Factory.StartNew(() =>
        //    {
        //        using (JsonTextWriter jsonTextWriter = new JsonTextWriter(new StreamWriter(stream, Encoding)) { CloseOutput = false })
        //        {
        //            serializer.Serialize(jsonTextWriter, value);
        //            jsonTextWriter.Flush();
        //        }
        //    });
        //}

        protected override System.Threading.Tasks.Task<object> OnReadFromStreamAsync(Type type, System.IO.Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            var task = Task<object>.Factory.StartNew(() =>
            {
                var settings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };

                var sr = new StreamReader(stream);
                var jreader = new JsonTextReader(sr);

                var ser = new JsonSerializer();
                ser.Converters.Add(new IsoDateTimeConverter());

                object val = ser.Deserialize(jreader, type);
                return val;
            });

            return task;
        }

        protected override System.Threading.Tasks.Task OnWriteToStreamAsync(Type type, object value, System.IO.Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders, FormatterContext formatterContext, System.Net.TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var settings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };

                string json = JsonConvert.SerializeObject(value, Formatting.Indented,
                                                          new JsonConverter[1] { new IsoDateTimeConverter() });

                byte[] buf = System.Text.Encoding.Default.GetBytes(json);
                stream.Write(buf, 0, buf.Length);
                stream.Flush();
            });

            return task;
        }
    }
}
