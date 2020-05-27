using System.Collections.Generic;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization.UI.Areas.ExpressLocalization.TranslationTools
{
    public interface IXLTranslateApiClient
    {
        Task<string> GoogleTranslateAsync(string source, string target, string text, string format);
        Task<string> YandexTranslateAsync(string source, string target, string text, string format);
        Task<string> MyMemoryTranslateAsync(string source, string target, string text);
        Task<string> SystranTranslateAsync(string source, string target, string text);
    }
}
