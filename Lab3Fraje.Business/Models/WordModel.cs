using MongoDB.Bson.Serialization.Attributes;

namespace Lab3Fraje.Business.Models
{
    [BsonIgnoreExtraElements]
    public class WordModel
    {
        [BsonElement]
        public string[] Translations { get; set; }

        [BsonElement]
        public int FromLanguage { get; }

        [BsonElement]
        public int ToLanguage { get; }
        public WordModel(params string[] translations)
        {
            Translations = translations;
            FromLanguage = 0;
            ToLanguage = 1;
        }
    }
}
