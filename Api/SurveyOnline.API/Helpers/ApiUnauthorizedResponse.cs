using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.API.Helpers
{
    public class ApiUnauthorizedResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; }

        public ApiUnauthorizedResponse(ModelStateDictionary modelState, string message)
            : base(401, message)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }

        public ApiUnauthorizedResponse(ModelStateDictionary modelState)
           : base(401)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }

        public ApiUnauthorizedResponse(IdentityResult identityResult)
           : base(401)
        {
            Errors = identityResult.Errors
                .Select(x => x.Code + " - " + x.Description).ToArray();
        }

        public ApiUnauthorizedResponse(string message)
           : base(401, message)
        {
        }
    }
}
