using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Ereader{
    public class book_control : MonoBehaviour {
    
        // Use this for initialization
        void Start () {
            GameObject bookContainer = GameObject.Find("book001");
            TextMeshPro book = bookContainer.GetComponent<TextMeshPro>();

            string bookPath = "/Users/Shared/Unity/book_testing/Assets/dracula.txt";
            BookDotText txtBook = new BookDotText(bookPath);
	    
            book.text = string.Join("\n", txtBook.Lines);

            book.enableWordWrapping = true;
            book.overflowMode = TextOverflowModes.Page;
            book.enableAutoSizing = false;
            book.fontSize = 12;
            book.pageToDisplay = 100;
        }
	
        // Update is called once per frame
        void Update () {

        }
    }

    public class BookDotText{
        private string fileName;

        private string[] lines;

        private int paraCount;
        private int sentCount;
        private int lineCount;
        private int wordCount;
        private int charCount;

        public BookDotText(string fileName){
            this.fileName = fileName;

            readDotTxt();
        }

        private void readDotTxt(){

            this.lines = File.ReadAllLines(fileName);
            this.lineCount = lines.Length;

            bool seekPara = false;
            bool seekSent = false;
            bool seekWord = false;

            foreach(string line in lines){

                // Count paragraphs
                if (seekPara){
                    if (line.Length >= 2 && Char.IsWhiteSpace(line[0]) && Char.IsWhiteSpace(line[1])){
                        // Account for indented paragraphs
                        this.paraCount++;
                        seekPara = false;
                    }

                    if (line.Length == 0){
                        this.paraCount++;
                        seekPara = false;
                    }
                }

                // Count words and chars
                foreach(char c in line){
                    this.charCount++;
                    if (c == ' ' && seekWord){
                        this.wordCount++;
                        seekWord = false;
                    }

                    if (Char.IsLetter(c) || c == '\''){
                        // Flags when a new paragraph, sentence or line has started.
                        seekPara = true;
                        seekSent = true;
                        seekWord = true;
                    }
                    else if (c == '.' && seekSent){
                        this.sentCount++;
                        this.wordCount++;
                        seekSent = false;
                    }
                }
            }

            Console.WriteLine("Paragraphs: {0}", paraCount);
            Console.WriteLine("Sentences: {0}", sentCount);
            Console.WriteLine("Lines: {0}", lineCount);
            Console.WriteLine("words: {0}", wordCount);
            Console.WriteLine("chars: {0}", charCount);
        }

        public string[] Lines{
            get{ return lines; }
        }

        public int ParaCount{
            get{ return paraCount; }
        }

        public int SentCount{
            get{ return sentCount; }
        }

        public int LineCount{
            get{ return lineCount; }
        }

        public int WordCount{
            get{ return wordCount; }
        }

        public int CharCount{
            get{ return charCount; }
        }
    }
}