using System;
using System.Collections.Generic;
using System.Text;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.Models
{
    public class TranslationItemModel
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int KeyID { get; set; }
        public string CultureID { get; set; }
    }
}
