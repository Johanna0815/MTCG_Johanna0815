using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.HSL
{

    // public eventuell noch umändern ? // muss ja ins DLA Projekt o?
    internal class HttpRequest
    {

        
        private StreamReader reader;

        public Method Method { get; private set; }
        //public string Method { get; private set; }     // Method als enum machen.  
        public string Path
        {
            get;
            private set;
        }

        // add on new darunter. 
        public Dictionary<string, string>

        public string ProtocolVersion { get; private set; }

        public Dictionary<string, string> headers = new();

        public string Content { get; private set; } 
        public HttpRequest(StreamReader reader)
        {
            this.reader = reader;
        }



        /// <summary>
        /// Endpunkt und dann bei dem ? splitten und danach sind die QueryParameter
        /// 
        /// Key     Value
        /// name =    A
        /// 
        /// </summary>


        public void Parse()
        {

            // firrst line contains ..
            string line = reader.ReadLine();
            Console.WriteLine(line );
            var firstLineParts = line.Split(" ");
           // Method = firstLineParts[0];
            Method = (Method)Enum.Parse(typeof(Method), firstLineParts[0]; // noch mal kontrollieren !


            Path = firstLineParts[1];

            var pathParts = Path.Split("?");
            if (pathParts.Length == 2) // wenn nur eins wäre, würde das Frageziechen fehlen. 
            {
                var queryParams = pathParts[1].Split("&");
                foreach(string queryParam in queryParams)
                {
                    var queryParamParts = queryParam.Split('=');
                    if (queryParamParts.Length == 2)
                        QueryParams.Add(queryParamParts[0], queryParamParts[1]);
                    else
                        QueryParams.Add(queryParamParts[0], null);
                }
            }
            
            ProtocolVersion = firstLineParts[2];

            // headers
            while((line = reader.ReadLine())   != null  )
            {

                Console.WriteLine(line  );
                if (line.Length == 0)
                    break;

                var headerParts = line.Split(": ");
                headers[headerParts[0]] = headerParts[1];
                //Headers = new Dictionary<string, string>();
            }

            Content = "";
            int contentLength = int.Parse(headers["Content-Length"]);
            var data = new StringBuilder();
           

            if (contentLength > 0)
            {
                char[] buffer = new char[1024];

                int totalBytesRead = 0;
                while (totalBytesRead < contentLength)
                {
                   

                   var bytesRead = reader.Read(buffer, 0, 1024);
                    if(bytesRead == 0)
                    {
                        break;


                        totalBytesRead += bytesRead;
                        data.Append(buffer, 0, bytesRead);

                    }
                }
                Content = data.ToString();

            }

             // content
            // fix unten um die länge! 
           /* Content = "";
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(  line);
                Content += line + "/n";
            }
           */
          




        }

    }
}
