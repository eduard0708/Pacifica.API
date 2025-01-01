using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// this class is created to force all the recived request date will be treated as local date
//in local timezone of the server
namespace Pacifica.API.Middleware
{
    // 1. Create a Middleware class (DateTimeLocalTimeMiddleware.cs)
    public class DateTimeLocalTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public DateTimeLocalTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request contains a body (application/json)
            if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
            {
                // Read the body content
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                // Modify the request body by converting DateTime fields to local time
                var updatedBody = ConvertDatesToLocalTime(requestBody);

                // Rewrite the request body with the updated JSON data
                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(updatedBody));
            }

            await _next(context);
        }

        private string ConvertDatesToLocalTime(string requestBody)
        {
            // Deserialize and update the DateTime values to local time
            var jsonObject = JsonConvert.DeserializeObject<JObject>(requestBody);

            foreach (var property in jsonObject!.Properties())
            {
                if (property.Value.Type == JTokenType.Date)
                {
                    var localDate = ((DateTime)property.Value).ToLocalTime();
                    property.Value = localDate;
                }
            }

            return jsonObject.ToString();
        }
    }

}