using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace SampleProject.Pages
{
    public class ModelBindingTestModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [Display(Name = "First number")]
        public int FirstNumber { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Second number")]
        public int SecondNumber { get; set; }

        [Display(Name = "Result")]
        public int Result { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var _msg = "Result: {0}";
            TempData.Info(string.Format(_msg, FirstNumber * SecondNumber));
        }
    }
}
