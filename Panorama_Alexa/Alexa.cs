using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Panorama_Alexa
{
    public class Alexa
    {
        public static HttpListener listener;
        public static string url = "http://+:8000/";
        public static string urlsec = "https://+:8443/";

        #region Json class
        public class Application
        {
            public string applicationId { get; set; }
        }

        public class Attributes
        {
        }

        public class User
        {
            public string userId { get; set; }
        }

        public class Session
        {
            public bool @new { get; set; }
            public string sessionId { get; set; }
            public Application application { get; set; }
            public Attributes attributes { get; set; }
            public User user { get; set; }
        }

        public class Video
        {
            public List<string> codecs { get; set; }
        }

        public class Size
        {
            public string type { get; set; }
            public int pixelWidth { get; set; }
            public int pixelHeight { get; set; }
        }

        public class Current
        {
            public string mode { get; set; }
            public Video video { get; set; }
            public Size size { get; set; }
        }

        public class Configuration
        {
            public Current current { get; set; }
        }

        public class Viewport
        {
            public string type { get; set; }
            public string id { get; set; }
            public string shape { get; set; }
            public int dpi { get; set; }
            public string presentationType { get; set; }
            public bool canRotate { get; set; }
            public Configuration configuration { get; set; }
        }

        public class Experience
        {
            public int arcMinuteWidth { get; set; }
            public int arcMinuteHeight { get; set; }
            public bool canRotate { get; set; }
            public bool canResize { get; set; }
        }

        public class Viewport2
        {
            public List<Experience> experiences { get; set; }
            public string mode { get; set; }
            public string shape { get; set; }
            public int pixelWidth { get; set; }
            public int pixelHeight { get; set; }
            public int dpi { get; set; }
            public int currentPixelWidth { get; set; }
            public int currentPixelHeight { get; set; }
            public List<string> touch { get; set; }
            public Video video { get; set; }
        }

        public class AplextBackstack10
        {
        }

        public class Available
        {
            [JsonProperty("aplext:backstack:10")]
            public AplextBackstack10 AplextBackstack10 { get; set; }
        }

        public class Extensions
        {
            public Available available { get; set; }
        }

        public class SupportedInterfaces
        {
        }

        public class Device
        {
            public string deviceId { get; set; }
            public SupportedInterfaces supportedInterfaces { get; set; }
        }

        public class System
        {
            public Application application { get; set; }
            public User user { get; set; }
            public Device device { get; set; }
            public string apiEndpoint { get; set; }
            public string apiAccessToken { get; set; }
        }

        public class Context
        {
            public List<Viewport> Viewports { get; set; }
            public Viewport Viewport { get; set; }
            public Extensions Extensions { get; set; }
            public System System { get; set; }
        }

        public class WindTurbine
        {
            public string name { get; set; }
            public string value { get; set; }
            public string confirmationStatus { get; set; }
            public string source { get; set; }
        }

        public class Status
        {
            public string code { get; set; }
        }

        public class Value2
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Value
        {
            public Value value { get; set; }
        }

        public class ResolutionsPerAuthority
        {
            public string authority { get; set; }
            public Status status { get; set; }
            public List<Value> values { get; set; }
        }

        public class Resolutions
        {
            public List<ResolutionsPerAuthority> resolutionsPerAuthority { get; set; }
        }

        public class Farm
        {
            public string name { get; set; }
            public string value { get; set; }
            public Resolutions resolutions { get; set; }
            public string confirmationStatus { get; set; }
            public string source { get; set; }
        }

        public class Slots
        {
            public WindTurbine wind_turbine { get; set; }
            public Farm farm { get; set; }
        }

        public class Intent
        {
            public string name { get; set; }
            public string confirmationStatus { get; set; }
            public Slots slots { get; set; }
        }

        public class Request
        {
            public string type { get; set; }
            public string requestId { get; set; }
            public string locale { get; set; }
            public DateTime timestamp { get; set; }
            public Intent intent { get; set; }
            public string dialogState { get; set; }
        }

        public class Root
        {
            public string version { get; set; }
            public Session session { get; set; }
            public Context context { get; set; }
            public Request request { get; set; }
        }
        #endregion

        private bool m_start;
        public bool start
        {
            get
            {
                return m_start;
            }
            set
            {
                m_start = value;
                if (m_start)
                    StartServer();
                else
                    StopServer();
            }
        }

        private static string m_responseJson;
        public string responseJson
        {
            get
            {
                return m_responseJson;
            }
            set
            {
                m_responseJson = value;
            }
        }

        private static string m_childView;
        public string childView
        {
            get
            {
                return m_childView;
            }
            set
            {
                m_childView = value;
            }
        }

        private static string m_selectedFarm;
        public string selectedFarm
        {
            get
            {
                return m_selectedFarm;
            }
            set
            {
                m_selectedFarm = value;
            }
        }

        private static int m_selectedTurbine;
        public int selectedTurbine
        {
            get
            {
                return m_selectedTurbine;
            }
            set
            {
                m_selectedTurbine = value;
            }
        }

        private static int m_errorAlarm;
        public int errorAlarm
        {
            get
            {
                return m_errorAlarm;
            }
            set
            {
                m_errorAlarm = value;
            }
        }

        private static int m_warningAlarm;
        public int warningAlarm
        {
            get
            {
                return m_warningAlarm;
            }
            set
            {
                m_warningAlarm = value;
            }
        }

        private static int m_noticeAlarm;
        public int noticeAlarm
        {
            get
            {
                return m_noticeAlarm;
            }
            set
            {
                m_noticeAlarm = value;
            }
        }

        private static bool m_acquitAlarm;
        public bool acquitAlarm
        {
            get
            {
                return m_acquitAlarm;
            }
            set
            {
                m_acquitAlarm = value;
            }
        }

        private static string m_highestAlarm;
        public string highestAlarm
        {
            get
            {
                return m_highestAlarm;
            }
            set
            {
                m_highestAlarm = value;
            }
        }

        private void StartServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Prefixes.Add(urlsec);
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        }

        private void StopServer()
        {
            listener.Stop();
            listener.Close();
        }
        private static void ListenerCallback(IAsyncResult result)
        {
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                // Call EndGetContext to complete the asynchronous operation.
                HttpListenerContext context = listener.EndGetContext(result);
                HttpListenerRequest request = context.Request;

                //Read request stream
                Stream body = request.InputStream;
                Encoding encoding = request.ContentEncoding;
                StreamReader reader = new StreamReader(body, encoding);
                string s = reader.ReadToEnd();

                string reponseText = ParseCommand(s);


                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                response.AddHeader("Content-Type", "application/json;charset=UTF-8");


                StringBuilder responseBuild = new StringBuilder();
                responseBuild.Append("{");
                responseBuild.Append("\"version\": \"1.0\",");
                responseBuild.Append("\"response\": {");
                responseBuild.Append("\"outputSpeech\": {");
                responseBuild.Append("\"type\": \"PlainText\",");
                responseBuild.Append("\"text\": \"" + reponseText + "\",");
                responseBuild.Append("\"playBehavior\": \"REPLACE_ENQUEUED\"");
                responseBuild.Append("},");
                responseBuild.Append("\"shouldEndSession\": true");
                responseBuild.Append("}");
                responseBuild.Append("}");



                // Construct a response.
                //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>" + s;
                m_responseJson = responseBuild.ToString();
                byte[] buffer = Encoding.UTF8.GetBytes(responseBuild.ToString());
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();

                listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
            }
            catch
            {

            }
        }

        private static string ParseCommand(string s)
        {
            string response = "";
            bool farmFound = false, turbineFound = false;
            try
            {
                Root r = JsonConvert.DeserializeObject<Root>(s);
                if (r.request.intent.name == "Open_Wind_Turbine_Vue")
                {
                    m_childView = "Site_1";
                    switch (r.request.intent.slots.farm.value.ToLower())
                    {
                        case "nord":
                            {
                                m_selectedFarm = "north";
                                farmFound = true;
                                break;
                            }
                        case "non":
                            {
                                m_selectedFarm = "north";
                                farmFound = true;
                                break;
                            }
                        case "sud":
                            {
                                m_selectedFarm = "south";
                                farmFound = true;
                                break;
                            }
                        case "est":
                            {
                                m_selectedFarm = "est";
                                farmFound = true;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    int selectedTurbineIndex = Convert.ToInt32(r.request.intent.slots.wind_turbine.value);
                    if (selectedTurbineIndex <= 4 && selectedTurbineIndex > 0)
                    {
                        m_selectedTurbine = selectedTurbineIndex;
                        turbineFound = true;
                    }
                    if (farmFound && turbineFound)
                    {
                        response = "vue ouverte";
                    }
                    else if (!farmFound || !turbineFound)
                    {
                        response = "éolienne " + selectedTurbineIndex + "ferme" + r.request.intent.slots.farm.value + "non trouvée";
                    }
                }
                else if (r.request.intent.name == "Open_Datacenter_Site")
                {
                    m_childView = "Site_2";
                    response = "Site data center ouvert";
                }
                else if (r.request.intent.name == "Open_GTB_Site")
                {
                    m_childView = "Site_3";
                    response = "Site G.T.B. ouvert";
                }
                else if (r.request.intent.name == "Alarm_Count")
                {
                    response = "il y a " + m_noticeAlarm + " alarme de type évenement, " + m_warningAlarm + " alarme de type défaut et " + m_errorAlarm + " alarme de type erreur";
                }
                else if (r.request.intent.name == "Acquit_Alarm")
                {
                    m_acquitAlarm = true;
                    response = "Alarmes acquitées";
                }
                else if (r.request.intent.name == "Highest_Alarm_Priority")
                {
                    response = m_highestAlarm;
                }
                else
                {
                    response = "commande non trouvée";
                }
            }
            catch
            {

            }
            return response;
        }
    }
}