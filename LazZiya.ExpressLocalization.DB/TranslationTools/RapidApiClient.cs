using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

namespace LazZiya.ExpressLocalization.DB.TranslationTools
{
    public class RapidApiClient : IXLTranslateApiClient
    {
        private readonly IConfiguration Configuration;
        private readonly HttpClient Client;
        private readonly string RapidApiKey;

        public RapidApiClient(IConfiguration configuration)
        {
            Configuration = configuration;
            RapidApiKey = Configuration["x-rapidapi-key"];
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("x-rapidapi-key", RapidApiKey);
        }

        public async Task<string> TranslateAsync(TranslationProvider provider, string source, string target, string text, string format)
        {
            switch (provider)
            {
                case TranslationProvider.Google: return await GoogleTranslateAsync(source, target, text, format);
                case TranslationProvider.Yandex: return await YandexTranslateAsync(source, target, text, format);
                case TranslationProvider.MyMemory: return await MyMemoryTranslateAsync(source, target, text);
                case TranslationProvider.Systran: return await SystranTranslateAsync(source, target, text);
                default: throw new NullReferenceException(nameof(provider));
            }
        }

        public async Task<string> YandexTranslateAsync(string source, string target, string text, string format)
        {
            var yandexkey = Configuration["yandex-translate-api-key"];

            if (string.IsNullOrWhiteSpace(yandexkey))
            {
                throw new NullReferenceException(nameof(yandexkey));
            }

            var result = await Client.GetAsync($"https://translate.yandex.net/api/v1.5/tr.json/translate?key={yandexkey}&text={text}&lang={source}-{target}&format={format}");
            var _txt = await result.Content.ReadAsStringAsync();
            return _txt;
        }

        public async Task<string> GoogleTranslateAsync(string source, string target, string text, string format)
        {
            var dummyresult = "{ \"data\":{ \"translations\":[ { \"translatedText\":\"Hosgeldiniz\" } ]  }  }";
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
            var gData = JsonConvert.DeserializeObject<GoogleTranslateResult>(dummyresult);
#else
            var gData = JsonSerializer.Deserialize(dummyresult, typeof(GoogleTranslateResult)) as GoogleTranslateResult;
#endif

            return gData.data.translations[0].translatedText;

            Client.DefaultRequestHeaders.Add("x-rapidapi-host", "google-translate1.p.rapidapi.com");

            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("format", format));
            list.Add(new KeyValuePair<string, string>("source", source));
            list.Add(new KeyValuePair<string, string>("target", target));
            list.Add(new KeyValuePair<string, string>("q", text));

            var appContent = new FormUrlEncodedContent(list);

            var result = await Client.PostAsync("https://google-translate1.p.rapidapi.com/language/translate/v2", appContent);
            var _txt = await result.Content.ReadAsStringAsync();

            return _txt;
        }

        public async Task<string> MyMemoryTranslateAsync(string source, string target, string text)
        {
            Client.DefaultRequestHeaders.Add("x-rapidapi-host", "translated-mymemory---translation-memory.p.rapidapi.com");

            var result = await Client.GetAsync($"https://translated-mymemory---translation-memory.p.rapidapi.com/api/get?langpair={source}|{target}&q={text}");
            var _txt = await result.Content.ReadAsStringAsync();
            return _txt;
        }

        public async Task<string> SystranTranslateAsync(string source, string target, string text)
        {
            Client.DefaultRequestHeaders.Add("x-rapidapi-host", "systran-systran-platform-for-language-processing-v1.p.rapidapi.com");

            var result = await Client.GetAsync($"https://systran-systran-platform-for-language-processing-v1.p.rapidapi.com/translation/text/translate?source={source}&target={target}&input={text}");
            var _txt = await result.Content.ReadAsStringAsync();
            return _txt;
        }
    }
}
