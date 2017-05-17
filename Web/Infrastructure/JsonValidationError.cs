using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Web.Infrastructure
{
    public class JsonValidationError : JsonResult
    {
        private readonly IList<string> errors = new List<string>();

        public JsonValidationError(IEnumerable<KeyValuePair<string, ModelState>> modelState)
        {
            BuildErrors(modelState);
        }

        private void BuildErrors(IEnumerable<KeyValuePair<string, ModelState>> modelState)
        {
            foreach (KeyValuePair<string, ModelState> state in modelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    foreach (ModelError err in state.Value.Errors)
                    {
                        errors.Add(err.ErrorMessage);
                    }
                }
            }
        }

        private string Message
        {
            get { return errors.Count > 0 ? errors[0] : string.Empty; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException();
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.StatusCode = 400;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            response.Write(Message);
        }
    }
}