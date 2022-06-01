using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3Fraje.Business.Models
{
    [BsonIgnoreExtraElements]
    public class WordList
    {
        private WordListModel _wordListModel = new WordListModel();

        private static DataAccess _dataAccess = new DataAccess();
        public string Name { get { return _wordListModel.Name; } }
        public string[] Languages { get { return _wordListModel.Languages; } }

        public WordList(string name, string languageOne, string languageTwo)
        {
            _wordListModel.Name = name;
            _wordListModel.Languages = new string[] { languageOne, languageTwo };

            SaveNew();

        }
        private WordList(WordListModel model)
        {
            _wordListModel = model;
        }

        public void addWord(string languageOneWord, string languageTwoWord)
        {
            var word = new WordModel(new string[] { languageOneWord, languageTwoWord });
            _wordListModel.Words.Add(word);
            Save();
        }

        public void removeWords(string language, string[] wordsToRemove)
        {
            var languageIndex = Array.IndexOf(_wordListModel.Languages, language);

            var wordsToBeRemaing = new List<WordModel>();
            if (languageIndex >= 0)
            {
                foreach (var word in _wordListModel.Words)
                {
                    if (!wordsToRemove.Any(wordToRemove => wordToRemove == word.Translations[languageIndex]))
                    {
                        wordsToBeRemaing.Add(word);
                    }
                }
            }


            _wordListModel.Words = wordsToBeRemaing;
            Save();
        }

        public static string[] GetLists()
        {
            var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            var _database = dbClient.GetDatabase("Lab3Fraje_GlosMaster");
            var _collection = _database.GetCollection<WordListModel>("WordLists");

            var list = _collection.Find(_ => true).ToList();
            return list.Select(doc => doc.Name).ToArray();
        }

        public static WordList loadList(string wordListName)
        {
            var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            var _database = dbClient.GetDatabase("Lab3Fraje_GlosMaster");
            var _collection = _database.GetCollection<WordListModel>("WordLists");

            var list = _collection.Find(doc => doc.Name == wordListName).FirstOrDefault();
            if (list == null)
            {
                return null;
            }

            var wordList = new WordList(list);

            return wordList;
        }

        public bool Remove(int translation, string[] words)
        {
            var check = true;
            Save();
            return check;
        }

        public int Count()
        {
            return _wordListModel.Words.Count;
        }

        public void List(int sortByTranslation, Action<string[]> showTranslations)
        {

            var words = _wordListModel.Words;


            var translations = words.Select(word => word.Translations);

            var sortedTranslations = translations.OrderBy(translation => translation[sortByTranslation]).ToList();


            sortedTranslations.ForEach(showTranslations);

        }

        public WordModel GetWordToPractise()
        {
            var numberOfWords = Count();

            Random rnd = new Random();
            var rndWordNumber = rnd.Next(0, numberOfWords);
            var _wordModel = _wordListModel.Words[rndWordNumber];

            return _wordModel;
        }

        private void SaveNew()
        {
            _dataAccess.AddWordListModel(_wordListModel);
        }

        public void Save()
        {
            _dataAccess.UpdateWordList(_wordListModel);
        }
    }
}
