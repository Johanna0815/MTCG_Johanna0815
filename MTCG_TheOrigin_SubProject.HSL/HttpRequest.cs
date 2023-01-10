//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MTCG_TheOrigin_SubProject.HSL
//{
//    public class HttpRequest
//    {
//        private StreamReader reader;

//        public HttpMethod Method { get; private set; }
//        //public string Method { get; private set; }     // Method als enum machen.  
//        public string Path { get; private set; }

//        // add on new darunter. 
//        public Dictionary<string, string> QueryParams = new();

//        public string ProtocolVersion { get; private set; }

//        public Dictionary<string, string> headers = new();

//        public string Content { get; private set; }
//        public HttpRequest(StreamReader reader)
//        {
//            this.reader = reader;
//        }



//        /// <summary>
//        /// Endpunkt und dann bei dem ? splitten und danach sind die QueryParameter
//        /// 
//        /// Key     Value
//        /// name =    A
//        /// 
//        /// </summary>


//        public void Parse()
//        {

//            // firrst line contains  HTTPMethod Path and Protocol
//            string line = reader.ReadLine();
//            if (line == null)
//            {
//                return;
//            }
//            Console.WriteLine(line);
//            var firstLineParts = line.Split(" ");
//            // Method = firstLineParts[0];
//            Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), firstLineParts[0]);

//            var path = firstLineParts[1];
//            var pathParts = path.Split('?');
//            if (pathParts.Length == 2) // wenn nur eins wäre, würde das Frageziechen fehlen. 
//            {
//                // queryParameters after the ?-char
//                var queryParams = pathParts[1].Split('&');
//                foreach (string queryParam in queryParams)
//                {
//                    var queryParamParts = queryParam.Split('=');
//                    if (queryParamParts.Length == 2)
//                        QueryParams.Add(queryParamParts[0], queryParamParts[1]);
//                    else
//                        QueryParams.Add(queryParamParts[0], null);
//                }
//            }

//            //  Path = pathParts[0].Split('/');

//            Path = pathParts[0];

//            ProtocolVersion = firstLineParts[2];

//            // headers
//            int contentLength = 0;
//            while ((line = reader.ReadLine()) != null)
//            {

//                Console.WriteLine(line);
//                if (line.Length == 0)
//                    break;

//                var headerParts = line.Split(": ");
//                headers[headerParts[0]] = headerParts[1];
//                if (headerParts[0] == "Content-Length")
//                    contentLength = int.Parse(headerParts[1]);
//                //Headers = new Dictionary<string, string>();
//            }

//            // read Http body (if existing)
//            Content = "";
//            if (contentLength > 0 && headers.ContainsKey("Content-Type"))
//            {
//                var data = new StringBuilder(200);
//                char[] buffer = new char[1024];
//                int bytesReadTotal = 0;
//                while (bytesReadTotal < contentLength)
//                {
//                    try
//                    {
//                        var bytesRead = reader.Read(buffer, 0, 1024);
//                        bytesReadTotal += bytesRead;
//                        if (bytesRead == 0) break;
//                        data.Append(buffer, 0, bytesRead);
//                    }
//                    // IOException occurs when there is a mismatch of the 'Content-Length'
//                    // becuase different encoding is used ? 
//                    catch (IOException) { break; }
//                }

//                Content = data.ToString();
//                Console.WriteLine(Content);

//            }



//            // content
//            // fix unten um die länge! 
//            /* Content = "";
//             while ((line = reader.ReadLine()) != null)
//             {
//                 Console.WriteLine(  line);
//                 Content += line + "/n";
//             }
//            */





//        }

//    }
//}
