using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShadowSharp
{
    public class CardDeck
    {
        [JsonIgnore]
        internal string LangCode { get; set; }

        [JsonIgnore]
        public string DeckHash { get; internal set; }

        [JsonIgnore]
        public string DeckLink { get { return $"https://shadowverse-portal.com/deck/{DeckHash}?lang={LangCode}"; } }

        [JsonIgnore]
        public string DeckImage { get { return $"https://shadowverse-portal.com/image/{DeckHash}?lang={LangCode}"; } }

        [JsonProperty("deck_format")]
        public int DeckFormat { get; private set; }

        [JsonProperty("clan", NullValueHandling = NullValueHandling.Ignore)]
        public ClanType ClanType { get; private set; }

        [JsonProperty("cards")]
        public IEnumerable<Card> Cards { get; private set; }
    }

    public class ApiResponse
    {
        [JsonProperty("data_headers")]
        public DataHeader Header { get; private set; }

        [JsonProperty("data")]
        public string PayloadData { get; private set; }

        [JsonProperty("errors")]
        public IEnumerable<Error> Errors { get; private set; }
    }

    public class DataHeader
    {
        [JsonProperty("udid")]
        public bool UdId { get; private set; }

        [JsonProperty("viewer_id")]
        public int ViewerId { get; private set; }

        [JsonProperty("sid")]
        public string SId { get; private set; }

        [JsonProperty("servertime")]
        public DateTimeOffset ServerTime { get; private set; }

        [JsonProperty("result_code")]
        public int ResultCode { get; private set; }
    }

    public class Error
    {
        [JsonProperty("")]
        public string EEEEEEEEEH { get; set; }
    }

    public class DeckImport
    {
        /// <summary>
        /// Always says "Sucessfully imported the deck" in Japanese
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; private set; }

        [JsonProperty("clan", NullValueHandling = NullValueHandling.Ignore)]
        public ClanType ClanType { get; private set; }

        [JsonProperty("hash")]
        public string DeckHash { get; private set; }
    }

    public class Card
    {
        [JsonProperty("card_id")]
        public long Id { get; private set; }

        [JsonProperty("foil_card_id")]
        public long FoilId { get; private set; }

        [JsonProperty("card_set_id")]
        public long SetId { get; private set; }

        [JsonProperty("card_name")]
        public string Name { get; private set; }

        [JsonProperty("is_foil")]
        private int Is_Foil
        {
            set => IsFoil = (value != 0)? true : false;
        }

        [JsonIgnore]
        public bool IsFoil { get; private set; }

        [JsonProperty("char_type")]
        public CharType CharType { get; private set; }

        [JsonProperty("clan", NullValueHandling = NullValueHandling.Ignore)]
        public ClanType ClanType { get; private set; }

        [JsonProperty("tribe_name")]
        public string TraitName { get; private set; }

        [JsonProperty("skill")]
        public string Skill { get; private set; }

        [JsonProperty("skill_option")]
        public string SkillOption { get; private set; }

        [JsonProperty("skill_disc")]
        public string SkillDescription { get; private set; }

        [JsonProperty("org_skill_disc")]
        public string RawSkillDescription { get; private set; }

        [JsonProperty("evo_skill_disc")]
        public string EvolvedSkillDescription { get; private set; }

        [JsonProperty("org_evo_skill_disc")]
        public string RawEvolvedSkillDescription { get; private set; }

        [JsonProperty("cost")]
        public int Cost { get; private set; }

        [JsonProperty("atk")]
        public int Attack { get; private set; }

        [JsonProperty("life")]
        public int Life { get; private set; }

        [JsonProperty("evo_atk")]
        public int EvolvedAttack { get; private set; }

        [JsonProperty("evo_life")]
        public int EvolvedLife { get; private set; }

        [JsonProperty("rarity")]
        public RarityLevel Rarity { get; private set; }


        /// <summary>
        /// Vials gained when you liquify the card
        /// </summary>
        [JsonProperty("get_red_ether")]
        public int LiquifyVials { get; private set; }

        /// <summary>
        /// Vials needed to create the card
        /// </summary>
        [JsonProperty("use_red_ether", NullValueHandling = NullValueHandling.Ignore)]
        public int? CreateVials { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("evo_description")]
        public string EvolvedDescription { get; private set; }

        [JsonProperty("cv")]
        public string CardVoiceActor { get; private set; }

        [JsonProperty("copyright")]
        public string Copyright { get; private set; }

        [JsonProperty("base_card_id")]
        public long BaseId { get; private set; }

        [JsonProperty("tokens")]
        public string Tokens { get; private set; }

        [JsonProperty("normal_card_id")]
        public long NormalCardId { get; private set; }

        [JsonProperty("format_type")]
        public int FormatType { get; private set; }

        [JsonProperty("restricted_count")]
        public int RestrictedCount { get; private set; }
    }

    public enum RarityLevel
    {
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Legendary = 4
    }

    public enum CharType
    {
        Follower = 1,
        Amulet = 2,
        CountdownAmulet = 3,
        Spell = 4
    }

    public enum ClanType
    {
        Neutral = 0,
        ForestCraft = 1,
        SwordCraft = 2,
        RuneCraft = 3,
        DragonCraft = 4,
        ShadowCraft = 5,
        BloodCraft = 6,
        HavenCraft = 7,
        PortalCraft = 8
    }
}
