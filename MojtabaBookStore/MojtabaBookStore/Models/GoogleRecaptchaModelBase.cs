using Microsoft.AspNetCore.Mvc;
using MojtabaBookStore.Attributes;

namespace MojtabaBookStore.Models
{
    public class GoogleRecaptchaModelBase
    {
        [GoogleRecaptchaValidation]
        [BindProperty(Name = "g-recaptcha-response")]
        public string GoogleRecaptchaResponse { get; set; }
    }
}
