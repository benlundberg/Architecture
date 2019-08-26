using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Architecture.Core
{
    public class BaseService
    {
        private HttpClient GetHttpClient(int timeout)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            httpClient.Timeout = TimeSpan.FromMinutes(timeout);

            // Here we can put some tokens or shared secrets 

            return httpClient;
        }

        protected async Task<ResponseData> MakeRequestAsync(string url, HttpMethod httpMethod, object param = null, ParseType parseType = ParseType.JSON, int timeout = 7)
        {
            try
            {
                using (var client = GetHttpClient(timeout))
                {
                    var requestMessage = new HttpRequestMessage(httpMethod, new Uri(url));

                    if (param != null)
                    {
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
                    }

                    var response = await client.SendAsync(requestMessage);

                    if (response.StatusCode == HttpStatusCode.Moved)
                    {
                        requestMessage.RequestUri = response.Headers.Location;

                        response = await client.SendAsync(requestMessage);
                    }

                    var responseData =  new ResponseData()
                    {
                        ResultStatusCode = response.StatusCode
                    };

                    if (parseType == ParseType.JSON || parseType == ParseType.XML)
                    {
                        responseData.Data = await response.Content.ReadAsStringAsync();
                    }
                    else if (parseType == ParseType.BYTES)
                    {
                        responseData.Data = await response.Content.ReadAsByteArrayAsync();
                    }
                    else if (parseType == ParseType.STREAM)
                    {
                        responseData.Data = await response.Content.ReadAsStreamAsync();
                    }

                    return responseData;
                }
            }
            catch (Exception ex)
            {
                ex.Print();
                throw;
            }
        }

        protected async Task<T> MakeRequestAsync<T>(string url, HttpMethod httpMethod, object param = null, ParseType parseType = ParseType.JSON, int timeout = 7)
        {
            try
            {
                var resultData = await MakeRequestAsync(url, httpMethod, param, parseType, timeout);

                if (resultData.ResultStatusCode == HttpStatusCode.OK)
                {
                    if (parseType == ParseType.JSON)
                    {
                        // Return object parsed from Json
                        return JsonConvert.DeserializeObject<T>(resultData.Data?.ToString(), new JsonSerializerSettings()
                        {
                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                            DateParseHandling = DateParseHandling.DateTimeOffset,
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    }
                    else if (parseType == ParseType.XML)
                    {
                        // Return object parsed from XML
                        return (T)new XmlSerializer(typeof(T)).Deserialize(XmlReader.Create(resultData.Data?.ToString()));
                    }
                    else
                    {
                        return (T)resultData.Data;
                    }
                }
                else if (resultData.ResultStatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Not Authorized");
                }
                else
                {
                    throw new Exception($"Something went wrong with the request: {resultData.ResultStatusCode}");
                }
            }
            catch (Exception ex)
            {
                ex.Print();
                throw;
            }
        }

        protected static string GetUrl(string method)
        {
            return ServiceConfig.WEB_SERVICE_BASE_ADDRESS + method;
        }
    }

    public class ResponseData
    {
        public object Data { get; set; }
        public HttpStatusCode? ResultStatusCode { get; set; }

        public void EnsureSuccess()
        {
            if (ResultStatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Something went wrong with the request:" + ResultStatusCode);
            }
        }
    }

    public enum ParseType
    {
        JSON,
        XML,
        STREAM,
        BYTES
    }
}
