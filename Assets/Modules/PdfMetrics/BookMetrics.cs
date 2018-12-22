﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using Newtonsoft.Json;

namespace PdfMetrics {
    
    public class BookMetrics{

        private List<PageMetrics> pages;
        private float pageMaxWidth;
        private float pageMaxHeight;
        private int nbPages;
        private string author;
        private string title;
        private string date;
        private string publisher;

        public BookMetrics(){
            pages = new List<PageMetrics>();
            nbPages = 0;
            Author = "";
            title = "";
            date = "";
            publisher = "";
        }
        
        public void AddPage(PageMetrics page){
            pages.Add(page);
            nbPages++;

            if (page.Width > pageMaxWidth){
                pageMaxWidth = page.Width;
            }
            
            if (page.Height > pageMaxHeight){
                pageMaxHeight = page.Height;
            }
        }

        public void ToJson(string output){
            try{
                File.WriteAllText(output, JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented));
            }
            catch(Exception e){
                if (e is UnauthorizedAccessException || e is SecurityException){
                    e.Data.Add("WriteJsonError", "Failed to write json to path: " + output + 
                                                 " as you do not have the required permissions");
                    throw;
                }
                if (e is NotSupportedException){
                    e.Data.Add("WriteJsonError", "Failed to write json to path: " + output +
                                                 " as path is not in a valid format");
                    throw;
                }
                e.Data.Add("WriteJsonError", "Failed to write json to path: " + output);
                throw;
            }
        }

        public static BookMetrics FromJson(string input){
            if (File.Exists(input)){
                try{
                    return JsonConvert.DeserializeObject<BookMetrics>(File.ReadAllText(input)); // todo handle json issues
                }
                catch(Exception e){
                    if (e is UnauthorizedAccessException || e is SecurityException){
                        e.Data.Add("ReadJsonError", "Failed to read json to path: " + input + 
                                                     " as you do not have the required permissions");
                        throw;
                    }
                    if (e is NotSupportedException){
                        e.Data.Add("ReadJsonError", "Failed to read json to path: " + input +
                                                     " as path is not in a valid format");
                        throw;
                    }
                    e.Data.Add("ReadJsonError", "Failed to read json data from path: " + input);
                    throw;                    
                }
            }
            throw new Exception("Input file not found at path: " + input);
        }

        public float LineWidthProportion(LineMetrics line){
            return line.ContentWidth() / pageMaxWidth;
        }

        public float LineHeightProportion(LineMetrics line){
            return line.ContentHeight() / pageMaxHeight;
        }

        public List<PageMetrics> Pages{
            get{ return pages; }
        }

        public float PageMaxWidth{
            get{ return pageMaxWidth; }
            set{ pageMaxWidth = value; }
        }

        public float PageMaxHeight{
            get{ return pageMaxHeight; }
            set{ pageMaxHeight = value; }
        }

        public int NbPages{
            get{ return nbPages; }
        }

        public string Author{
            get{ return author; }
            set{ author = value; }
        }

        public string Title{
            get{ return title; }
            set{ title = value; }
        }

        public string Date{
            get{ return date; }
            set{ date = value; }
        }

        public string Publisher{
            get{ return publisher; }
            set{ publisher = value; }
        }
    }
}