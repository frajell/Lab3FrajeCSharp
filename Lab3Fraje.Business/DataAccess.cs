using Lab3Fraje.Business.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Lab3Fraje.Business
{
    public class DataAccess
    {
        private IMongoDatabase _database;
        private IMongoCollection<WordListModel> _collection;
        public DataAccess()
        {
            var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            _database = dbClient.GetDatabase("Lab3Fraje_GlosMaster");
            _collection = _database.GetCollection<WordListModel>("WordLists");
        }

        public void AddWordListModel(WordListModel wordList)
        {
            _collection.InsertOne(wordList);
        }

        public void UpdateWordList(WordListModel wordListModel)
        {
            var filter = Builders<WordListModel>.Filter.Eq(wml => wml.ID, wordListModel.ID);
            _collection.ReplaceOne(filter, wordListModel);
        }
        public void AddWord(WordListModel wordList)
        {
            _collection.InsertOne(wordList);
        }
    }
}
