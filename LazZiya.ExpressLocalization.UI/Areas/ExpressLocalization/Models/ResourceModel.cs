using LazZiya.ExpressLocalization.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Models
{
    public class ResourceInputModel
    {
        [ExRequired]
        [Display(Name = "Key")]
        public string Key { get; set; }
        
        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }

    public class ResourceListItem
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Key")]
        public string Key { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public ICollection<string> Cultures { get; set; }
    }

    public class ResourceSearchModel
    {
        public int? ID { get; set; }
        public string Key { get; set; }
        public string Comment { get; set; }
    }
}
