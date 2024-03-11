using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MarketplacBoostifySolutione.Models.Global
{
    public class APIJsonReturnObject
    {
        public APIJsonReturnObject(Object data, ErrorObject errorObject = null)
        {
            Data = data;
            Error = errorObject;
        }

        [JsonProperty("data")]
        public Object Data { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public ErrorObject Error { get; set; }

        public class ErrorObject
        {
            public ErrorObject(HttpStatusCode statusCode, string message)
            {
                StatusCode = statusCode;
                Message = message;
            }

            public ErrorObject(List<ValidationResult> lvr)
            {
                StatusCode = HttpStatusCode.BadRequest;
                Message = "Please correct the following fields:";
                ValidationErrorMessages = new List<string>();

                foreach (var vr in lvr)
                {
                    ValidationErrorMessages.Add(vr.ErrorMessage);
                }
            }

            [JsonProperty("statusCode")]
            public HttpStatusCode StatusCode { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("validationErrors", NullValueHandling = NullValueHandling.Ignore)]
            public List<string> ValidationErrorMessages { get; set; }
        }
    }
}
