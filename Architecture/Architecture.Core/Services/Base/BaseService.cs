﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private string GetParameterString(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return string.Empty;
            }
            var list = new List<string>();
            foreach (var item in parameters)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return "?" + string.Join("&", list);
        }

        protected async Task<T> GetFromService<T>(string url, ParseType parseType = ParseType.JSON, int timeOutMinutes = 7, Dictionary<string, string> parameters = null)
        {
            ResultException = null;
            ResultStatusCode = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    client.Timeout = TimeSpan.FromMinutes(timeOutMinutes);

                    HttpResponseMessage response = null;

                    Debug.WriteLine("Service request: " + GetParameterString(parameters));

                    response = await client.GetAsync(url + GetParameterString(parameters));

                    ResultStatusCode = response.StatusCode;

                    if (ResultStatusCode == HttpStatusCode.OK)
                    {
                        if (parseType == ParseType.XML)
                        {
                            var reader = XmlReader.Create((await response.Content.ReadAsStreamAsync()));
                            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                        }
                        else
                        {
                            string data = await response.Content.ReadAsStringAsync();

                            var deserializerSettings = new JsonSerializerSettings()
                            {
                                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                DateParseHandling = DateParseHandling.DateTimeOffset,
                                NullValueHandling = NullValueHandling.Ignore
                            };


                            return JsonConvert.DeserializeObject<T>(data, deserializerSettings);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Debug.WriteLine("Unauthorized");
                    }
                    else
                    {
                        Debug.WriteLine("Response code:" + response.StatusCode.ToString());
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                ResultException = ex;
                Debug.WriteLine("PostToService exception: " + ex);
                return default(T);
            }
        }

        protected async Task<T> PostToService<T>(string url, object postObject, ParseType parseType = ParseType.JSON, int timeOutMinutes = 7)
        {
            ResultException = null;
            ResultStatusCode = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    client.Timeout = TimeSpan.FromMinutes(timeOutMinutes);

                    HttpResponseMessage response = null;

                    string postString = JsonConvert.SerializeObject(postObject);

                    Debug.WriteLine("Service request: " + url);

                    response = await client.PostAsync(url, new StringContent(postString, Encoding.UTF8, "application/json"));

                    ResultStatusCode = response.StatusCode;

                    if (ResultStatusCode == HttpStatusCode.OK)
                    {
                        if (parseType == ParseType.XML)
                        {
                            var reader = XmlReader.Create((await response.Content.ReadAsStreamAsync()));
                            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                        }
                        else
                        {
                            string data = await response.Content.ReadAsStringAsync();

                            var deserializerSettings = new JsonSerializerSettings()
                            {
                                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                DateParseHandling = DateParseHandling.DateTimeOffset,
                                NullValueHandling = NullValueHandling.Ignore
                            };

                            return JsonConvert.DeserializeObject<T>(data, deserializerSettings);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Debug.WriteLine("Unauthorized");
                    }
                    else
                    {
                        Debug.WriteLine("Response code:" + response.StatusCode.ToString());
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                ResultException = ex;
                Debug.WriteLine("PostToService exception: " + ex);
                return default(T);
            }
        }

        protected async Task<Stream> GetStream(string url, object postObject, int timeOutMinutes = 7)
        {
            ResultException = null;
            ResultStatusCode = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    client.Timeout = TimeSpan.FromMinutes(timeOutMinutes);

                    HttpResponseMessage response = null;

                    string postString = JsonConvert.SerializeObject(postObject);

                    Debug.WriteLine("Service request: " + url);

                    response = await client.PostAsync(url, new StringContent(postString, Encoding.UTF8, "application/json"));

                    ResultStatusCode = response.StatusCode;

                    if (ResultStatusCode == HttpStatusCode.OK)
                    {
                        return await response.Content.ReadAsStreamAsync();
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Debug.WriteLine("Unauthorized");
                    }
                    else
                    {
                        Debug.WriteLine("Response code:" + response.StatusCode.ToString());
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                ResultException = ex;
                Debug.WriteLine("PostToService exception: " + ex);
                return null;
            }
        }

        public async Task<byte[]> GetFile(string url, int timeOutMinutes = 7)
        {
            ResultException = null;
            ResultStatusCode = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(timeOutMinutes);

                    HttpResponseMessage response = null;

                    response = await client.GetAsync(new Uri(url));

                    ResultStatusCode = response.StatusCode;

                    byte[] data = default(byte[]);

                    if (ResultStatusCode == HttpStatusCode.OK)
                    {
                        data = await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        Debug.WriteLine("Response code:" + response.StatusCode.ToString());
                    }

                    return data;
                }
            }
            catch (Exception ex)
            {
                ResultException = ex;
                Debug.WriteLine("GetFileAsync exception: " + ex);
                return default(byte[]);
            }
        }

        public HttpStatusCode? ResultStatusCode;
        public Exception ResultException;
    }

    public enum ParseType
    {
        JSON,
        XML
    }
}
