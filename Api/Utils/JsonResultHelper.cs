using System.Collections.Generic;
using System.Net;

namespace Utils
{
    public class JsonResultHelper
    {
        public JsonResultHelper()
        {
            Status = HttpStatusCode.OK;
            mensaje = mensaje;
            Errors = new List<string>();
        }

        public JsonResultHelper(HttpStatusCode status)
        {
            Status = status;
            mensaje = mensaje;
            Errors = new List<string>();
        }

        public ICollection<string> Errors { get; set; }
        public string mensaje { get; set; }

        public HttpStatusCode Status { get; set; }

        public object Data { get; set; }
    }

}