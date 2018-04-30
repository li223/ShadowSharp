using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShadowSharp
{
    public class ShadowSharpClient
    {
        public event ClientErroredDelegate ClientErrored;

        internal static string LangCode { get; set; }

        private HttpClient httpClient = new HttpClient();

        public ShadowSharpClient(string lang = "en")
        {
            LangCode = lang;
            httpClient.BaseAddress = new Uri("https://shadowverse-portal.com/api/v1");
        }

        public async Task<IEnumerable<Card>> GetCardsAsync(ClanType? clan_type = null)
        {
            int? clan = null;
            if(clan_type != null)clan = (int)clan_type;
            var response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}/cards?format=json&lang={LangCode}&clan={clan}"));
            if (!response.IsSuccessStatusCode) return null;
            var cont = await response.Content.ReadAsStringAsync();
            var resdata = JsonConvert.DeserializeObject<ApiResponse>(cont);
            if (resdata.PayloadData.Errors.Count > 0)
            {
                ClientErrored?.Invoke(this, resdata.PayloadData.Errors);
                return null;
            }
            return resdata.PayloadData.Cards;
        }

        public async Task<CardDeck> GetCardDeckAsync(string deck_code)
        {
            var response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}/deck/import?format=json&lang={LangCode}&deck_code={deck_code}"));
            if (!response.IsSuccessStatusCode) return null;
            var resdata = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            if(resdata.PayloadData.Errors.Count > 0)
            {
                ClientErrored?.Invoke(this, resdata.PayloadData.Errors);
                return null;
            }
            response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}/deck?format=json&lang={LangCode}&hash={resdata.PayloadData.DeckHash}"));
            if (!response.IsSuccessStatusCode) return null;
            var content = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            if (content.PayloadData.Errors.Count > 0)
            {
                ClientErrored?.Invoke(this, resdata.PayloadData.Errors);
                return null;
            }
            var data = content.PayloadData.CardDeck;
            data.DeckHash = resdata.PayloadData.DeckHash;
            data.LangCode = LangCode;
            return data;
        }

        public async Task<string> GetClassImageAsync(ClanType clan, bool check = true)
        {
            if (check)
            {
                var response = await httpClient.GetAsync(new Uri($"https://shadowverse-portal.com/public/assets/image/deckbuilder/{LangCode}/classes/{(int)clan}/character.png?20180426b"));
                return (response.IsSuccessStatusCode) ? $"https://shadowverse-portal.com/public/assets/image/deckbuilder/{LangCode}/classes/{(int)clan}/character.png?20180426b" : null;
            }
            else return $"https://shadowverse-portal.com/public/assets/image/deckbuilder/{LangCode}/classes/{(int)clan}/character.png?20180426b";
        }
    }
    public delegate void ClientErroredDelegate(object sender, IEnumerable<Error> e);
}
