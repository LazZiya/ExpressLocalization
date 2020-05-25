using System.ComponentModel.DataAnnotations;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Models
{
    public class TranslationItem
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name ="Value")]
        public string Value { get; set; }

        [Display(Name = "Culture")]
        public string CultureName { get; set; }
    }
}
