using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class HttpResponse
   {
      public string Data { get; set; }
      public int Code { get; set; }

      public HttpResponse()
      {
         Code = 200;
         Data = "";
      }
      public HttpResponse(int code)
      {
         Code = code;
         Data = "";
      }
      public HttpResponse( int code, string data )
      {
         Code = code;
         Data = data;         
      }
      
   }
}
