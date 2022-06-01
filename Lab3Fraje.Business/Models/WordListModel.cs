using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Lab3Fraje.Business.Models
{

    public class WordListModel
    {

        [BsonId]
        public Guid ID { get; set; }

        public string Name { get; set; }


        public string[] Languages { get; set; }


        public List<WordModel> Words { get; set; }

        public WordListModel()
        {
            Words = new List<WordModel>();
            Languages = new string[] { };
        }
    }
}
