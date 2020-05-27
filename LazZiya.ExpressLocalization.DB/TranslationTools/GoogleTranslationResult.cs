using System;
using System.Collections.Generic;
using System.Text;

namespace LazZiya.ExpressLocalization.DB.TranslationTools
{
    public class GoogleTranslateResult
    {
        public GoogleData data { get; set; }
    }

    public class GoogleData
    {
        public GoogleTranslation[] translations { get; set; }
    }

    public class GoogleTranslation
    {
        public string translatedText { get; set; }
    }

    public class GoogleTranslatedText
    {
        public string TranslatedText { get; set; }
    }
}
