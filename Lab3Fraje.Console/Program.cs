using Lab3Fraje.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3Fraje.ConsoleApp
{
    public class Program
    {
        static List<WordList> WordLists { get; set; }

        static void Main(string[] args)
        {
            WordLists = new List<WordList>();
            var practiceAttempts = 0;
            var correctAnswers = 0;

            if (args.Length > 0)
            {
                if (args[0].ToLower() == "-lists")
                {
                    Console.WriteLine("Show the wordlists");
                    Console.WriteLine("The database consists of the following lists:");
                    var wordLists = WordList.GetLists();
                    foreach (var wordList in wordLists)
                    {
                        Console.WriteLine(wordList);
                    }
                    return;
                }
                if (args[0].ToLower() == "-new")
                {
                    Console.WriteLine("Create a new list of words");
                    if (args.Length != 4)
                    {
                        Console.WriteLine(getHelpText());
                    }
                    else
                    {
                        var listName = args[1].ToLower();
                        var languageOne = args[2].ToLower();
                        var languageTwo = args[3].ToLower();
                        var wordList = new WordList(listName, languageOne, languageTwo);

                        while (true)
                        {
                            Console.WriteLine($"Enter word in {wordList.Languages[0]}");
                            var languageOneWord = Console.ReadLine().ToLower();
                            if (languageOneWord == "")
                            {
                                break;
                            }
                            Console.WriteLine($"Enter word in {wordList.Languages[1]}");
                            var languageTwoWord = Console.ReadLine().ToLower();
                            wordList.addWord(languageOneWord, languageTwoWord);
                            Console.WriteLine($"Added '{languageOneWord}' and '{languageTwoWord}' to the list '{wordList.Name}'.");
                        }

                    }
                    return;
                }
                if (args[0].ToLower() == "-add")
                {
                    if (args.Length != 2)
                    {
                        Console.WriteLine(getHelpText());
                    }
                    else
                    {
                        var listName = args[1].ToLower();
                        var wordList = WordList.loadList(listName);
                        if (wordList == null)
                        {
                            Console.WriteLine($"Could not find list '{listName}'");
                        }
                        else
                        {
                            while (true)
                            {
                                Console.WriteLine($"Enter word in {wordList.Languages[0]}");
                                var languageOneWord = Console.ReadLine().ToLower();
                                if (languageOneWord == "")
                                {
                                    break;
                                }
                                Console.WriteLine($"Enter word in {wordList.Languages[1]}");
                                var languageTwoWord = Console.ReadLine().ToLower();
                                wordList.addWord(languageOneWord, languageTwoWord);
                            }
                        }

                    }
                    return;
                }
                if (args[0].ToLower() == "-remove")
                {
                    Console.WriteLine("Remove words from a list");
                    if (args.Length < 4)
                    {
                        Console.WriteLine(getHelpText());
                    }

                    var listName = args[1].ToLower();
                    var language = args[2].ToLower();
                    var wordsToRemove = args.Skip(3);

                    var wordList = WordList.loadList(listName);
                    if (wordList == null)
                    {
                        Console.WriteLine($"Could not find list '{listName}'");
                    }
                    else
                    {
                        wordList.removeWords(language, wordsToRemove.ToArray());
                        Console.WriteLine($"Removed word(s) from list '{listName}'.");
                    }
                    return;
                }
                if (args[0].ToLower() == "-words")
                {
                    Console.WriteLine("Show words in a list");
                    var listName = args[1].ToLower();

                    var wordList = WordList.loadList(listName);
                    int sortByTranslation;
                    if (wordList == null)
                    {
                        Console.WriteLine($"Could not find list '{listName}'");
                    }
                    else
                    {
                        var language = args.Length >= 3 ? args[2].ToLower() : null;

                        if (language == null || language == wordList.Languages[0])
                        {

                            sortByTranslation = 0;
                            wordList.List(sortByTranslation, showTranslations);
                        }
                        if (language == wordList.Languages[1])
                        {
                            sortByTranslation = 1;
                            wordList.List(sortByTranslation, showTranslations);
                        }
                    }
                    return;
                }
                if (args[0].ToLower() == "-count")
                {
                    Console.WriteLine("Count words in a list");

                    var listName = args[1].ToLower();
                    var wordList = WordList.loadList(listName);
                    var numberOfWords = wordList.Count();

                    Console.WriteLine($"List '{listName}' consists of {numberOfWords} words.");
                    return;
                }

                if (args[0].ToLower() == "-practise")
                {
                    Console.WriteLine("Practise words!");
                    if (args.Length != 2)
                    {
                        Console.WriteLine(getHelpText());
                        return;
                    }

                    var listName = args[1].ToLower();
                    var wordList = WordList.loadList(listName);
                    if (wordList == null)
                    {
                        Console.WriteLine($"Could not find list '{listName}'");
                    }
                    else
                    {
                        practiceWords(wordList, practiceAttempts, correctAnswers);
                    }
                    return;
                }
                else
                {
                    Console.WriteLine(getHelpText());
                }
            }

            static void showTranslations(string[] translation)
            {
                Console.WriteLine($"{translation[0]}, {translation[1]}");
            }

            static WordList createWordList(string listName, string languageOne, string languageTwo)
            {
                return new WordList(listName, languageOne, languageTwo);
            }

            static void practiceWords(WordList wordList, int practiceAttempts, int correctAnswers)
            {
                Random rnd = new Random();
                var fromLanguage = rnd.Next(1);
                var practiseWordModel = wordList.GetWordToPractise();
                var wordToTranslateTo = practiseWordModel.Translations[0];

                var wordToTranlateFrom = practiseWordModel.Translations[fromLanguage];

                if (fromLanguage == 0)
                {
                    wordToTranslateTo = practiseWordModel.Translations[1];
                }

                Console.WriteLine($"Enter the translation for the word '{wordToTranlateFrom}':");
                var translationInput = Console.ReadLine().ToLower();
                if (translationInput == "")
                {
                    int percentCorrect = (int)Math.Round((double)(100 * correctAnswers) / practiceAttempts);

                    Console.WriteLine($"You did {practiceAttempts} attempts and had {correctAnswers} of the words right. That equals {percentCorrect} %.");
                    return;
                }

                practiceAttempts++;

                if (translationInput == wordToTranslateTo)
                {
                    Console.WriteLine("Correct!");
                    correctAnswers++;
                    practiceWords(wordList, practiceAttempts, correctAnswers);
                }
                else
                {
                    Console.WriteLine("Sorry, wrong answer.");
                    practiceWords(wordList, practiceAttempts, correctAnswers);
                }
            }

            static string getHelpText()
            {
                return "Use any of the following parameters: \n" +
                    "-lists \n" +
                    "-new <list name> <language 1> <language 2> \n" +
                    "-add <list name>\n" +
                    "-remove <list name> <language> <word 1> <word 2>... <word n>\n" +
                    "-words <list name> <sortByLanguage>\n" +
                    "-count <list name>\n" +
                    "-practise <listname>\n";
            }

        }
    }
}
