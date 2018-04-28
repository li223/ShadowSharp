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

        internal string LangCode { get; set; }
        private HttpClient httpClient = new HttpClient();

        public ShadowSharpClient(string lang = "en")
        {
            this.LangCode = lang;
            httpClient.BaseAddress = new Uri("https://shadowverse-portal.com/api/v1");
        }

        public async Task<IEnumerable<Card>> GetCardsAsync(ClanType? clan_type = null)
        {
            int? clan = null;
            if(clan_type != null)clan = (int)clan_type;
            var response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}/cards?format=json&lang={this.LangCode}&clan={clan}"));
            if (!response.IsSuccessStatusCode) return null;
            var cont = await response.Content.ReadAsStringAsync();
            var resdata = JsonConvert.DeserializeObject<ApiResponse>(cont);
            if (resdata.Errors != null)
            {
                ClientErrored?.Invoke(this, resdata.Errors);
                return null;
            }
            var obj = ((JObject)resdata.PayloadData).SelectToken("cards");
            var data = JsonConvert.DeserializeObject<IEnumerable<Card>>(obj.ToString());
            return data;
        }

        public async Task<CardDeck> GetCardDeckAsync(string deck_code)
        {
            var response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}/deck/import?format=json&lang={this.LangCode}&deck_code={deck_code}"));
            if (!response.IsSuccessStatusCode) return null;
            var resdata = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            if(resdata.Errors != null)
            {
                ClientErrored?.Invoke(this, resdata.Errors);
                return null;
            }
            var didata = JsonConvert.DeserializeObject<DeckImport>((string)resdata.PayloadData);
            response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}/deck?format=json&lang={this.LangCode}&hash={didata.DeckHash}"));
            if (!response.IsSuccessStatusCode) return null;
            var content = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            if (content.Errors != null)
            {
                ClientErrored?.Invoke(this, resdata.Errors);
                return null;
            }
            var data = JsonConvert.DeserializeObject<CardDeck>((string)content.PayloadData);
            data.DeckHash = didata.DeckHash;
            data.LangCode = this.LangCode;
            return data;
        }
        
        public async Task<string> GetCardLinkAsync(long card_id)
        {
            var response = await httpClient.GetAsync(new Uri($"https://shadowverse-portal.com/card/{card_id}?lang={this.LangCode}"));
            return (response.IsSuccessStatusCode) ? $"https://shadowverse-portal.com/card/{card_id}?lang={this.LangCode}" : null;
        }

        public async Task<string> GetCardImageAsync(long card_id)
        {
            var response = await httpClient.GetAsync(new Uri($"https://shadowverse-portal.com/image/card/{this.LangCode}/C_{card_id}.png?20180426b"));
            return (response.IsSuccessStatusCode)? $"https://shadowverse-portal.com/image/card/{this.LangCode}/C_{card_id}.png?20180426b" : null;
        }

        public async Task<string> GetClassImageAsync(ClanType clan)
        {
            var response = await httpClient.GetAsync(new Uri($"https://shadowverse-portal.com/public/assets/image/deckbuilder/{this.LangCode}/classes/{(int)clan}/character.png?20180426b"));
            return (response.IsSuccessStatusCode) ? $"https://shadowverse-portal.com/public/assets/image/deckbuilder/{this.LangCode}/classes/{(int)clan}/character.png?20180426b" : null;
        }
    }
    public delegate void ClientErroredDelegate(object sender, IEnumerable<Error> e);
}
