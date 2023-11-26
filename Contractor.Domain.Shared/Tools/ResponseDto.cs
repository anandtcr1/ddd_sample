using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Tools
{
    [Serializable]
    public class ResponseDto
    {
        public ResponseDto() { }

        public static ResponseDto Create(HttpStatusCode httpStatusCode, string result = null)
        {
            return Create((int)httpStatusCode, result ?? httpStatusCode.ToString());
        }

        public static ResponseDto Create(int statusCode, string result = null)
        {
            var response = new ResponseDto()
            {
                StatusCode = statusCode,
                ErrorResult = result,
                Errors = new Dictionary<string, List<string>>(),
            };
            return response;
        }

        public int StatusCode { get; set; }

        public object ErrorResult { get; set; }

        // key pair of key with list of its errors:
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();


        public ResponseDto AddError(string error, string key = "message")
        {
            if (this.Errors == null)
            {
                this.Errors = new Dictionary<string, List<string>>();
            }
            var keyErrors = this.Errors.ContainsKey(key) ? this.Errors[key] : null;
            if (keyErrors == null)
            {
                this.Errors.Add(key, new List<string>() { error });
            }
            else if (!keyErrors.Contains(error))
            {
                keyErrors.Add(error);
                this.Errors[key] = keyErrors;
            }
            //return calling object:
            return this;
        }

        public ResponseDto AddModelStateErrors(ModelStateDictionary modelState, string localizedMessage = "Validation Failed")
        {
            if (!modelState.IsValid)
            {
                this.ErrorResult = localizedMessage;
                var validationErrors = modelState.Keys
                        .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                        .ToList();
                foreach (var validationError in validationErrors)
                {
                    this.AddError(error: validationError.Message, key: validationError.Field);
                }
            }
            return this;
        }

        public ResponseDto ClearErrors()
        {
            this.Errors.Clear();
            return this;
        }

        public string FormatErrors()
        {
            string errors = "";
            if (Errors != null && Errors.Any())
            {
                foreach (var key in Errors.Keys)
                {
                    var keyErrors = Errors[key];
                    errors += string.Format("{0}: {1}", key, string.Join(", ", keyErrors));
                }
            }
            return errors;
        }
    }
}
