using System.Text.Json.Serialization;

namespace MauiAppShellDemo.Models
{

    public class Dog
    {
        [JsonPropertyName("weight")]
        public Weight Weight { get; set; }

        [JsonPropertyName("height")]
        public Height Height { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bred_for")]
        public string BredFor { get; set; }

        [JsonPropertyName("breed_group")]
        public string BreedGroup { get; set; }

        [JsonPropertyName("life_span")]
        public string LifeSpan { get; set; }

        [JsonPropertyName("temperament")]
        public string Temperament { get; set; }

        [JsonPropertyName("origin")]
        public string Origin { get; set; }

        [JsonPropertyName("reference_image_id")]
        public string ReferenceImageId { get; set; }

        [JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("history")]
        public string History { get; set; }
    }

    public class Height
    {
        [JsonPropertyName("imperial")]
        public string Imperial { get; set; }

        [JsonPropertyName("metric")]
        public string Metric { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Weight
    {
        [JsonPropertyName("imperial")]
        public string Imperial { get; set; }

        [JsonPropertyName("metric")]
        public string Metric { get; set; }
    }
}
