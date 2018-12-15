﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace Ereader{
    public class BasicBook : Book {
        
        private string bookPath;
        private List<Page> pages;
        private int linesPerPage;
        private int pageIndLeft;
        private int pageIndRight;
        private int pageIndFocused;
        protected Vector3 bookPosition;

        private Page prevPageLeft;
        private Page prevPageRight;
        private Page nextPageLeft;
        private Page nextPageRight;

        public BasicBook(string bookPath){
            this.bookPath = bookPath;
            pages = new List<Page>();
            pageIndLeft = -1; // Back side of front cover
            pageIndRight = 0;
            pageIndFocused = 0;
            bookPosition = new Vector3(0, 0, 0);
            linesPerPage = 25;
        }

        public override void Display(){
            
            if (prevPageLeft != null) prevPageLeft.Disable();
            if (prevPageRight != null) prevPageRight.Disable();
            if (nextPageLeft != null) nextPageLeft.Disable();
            if (nextPageRight != null) nextPageRight.Disable();

            prevPageLeft = PageIndLeft - 2 >= 0 ? Pages[PageIndLeft - 2] : null;
            prevPageRight = PageIndRight - 2 >= 0 ? Pages[pageIndRight - 2] : null;
            nextPageLeft = PageIndLeft + 2 < Pages.Count && PageIndLeft + 2 >= 0 ? Pages[pageIndLeft + 2] : null;
            nextPageRight = PageIndRight + 2 < Pages.Count && PageIndRight + 2 >= 0 ? Pages[pageIndRight + 2] : null;
            
            if (prevPageLeft != null) prevPageLeft.Enable();
            if (prevPageRight != null) prevPageRight.Enable();
            if (nextPageLeft != null) nextPageLeft.Enable();
            if (nextPageRight != null) nextPageRight.Enable();
            
            GetCurrentPageLeft().Enable();
            GetCurrentPageRight().Enable();
        }

        public void LoadBook(){
            BookDotText dracula = new BookDotText(bookPath);
            string[] lines = dracula.Lines;
            StringBuilder sb = new StringBuilder();
            int lineNum = 0;
            int pageNum = 1;
            GameObject gameObj = new GameObject("tmp" + pageNum);
            TextMeshProUGUI tmp = gameObj.AddComponent<TextMeshProUGUI>();
            foreach(string line in lines){

                sb.Append(line);
                sb.Append("\n");

                if (lineNum == linesPerPage) {
                    gameObj = new GameObject("tmp" + pageNum);
                    tmp = gameObj.AddComponent<TextMeshProUGUI>();
                    AddPage(ConstructPage(tmp, "tmp" + pageNum, pageNum, sb.ToString()));
                    pageNum++;
                    lineNum = 0;
                    sb = new StringBuilder();
                }
                lineNum++;
            }
            Display();
        }
        protected Page ConstructPage(TextMeshProUGUI tmp, string objName, int pageNum, string text){
            Page page = new Page(tmp, objName, pageNum);
            Vector2 preferredValues = tmp.GetPreferredValues(text);
            page.PreferredVals = preferredValues;
            tmp.text = text;
            return page;
        }        
        
        public override void AddPage(Page page){
            pages.Add(page);
        }

        public override void Remove(Page page){
            pages.Remove(page);
        }

        public override void Remove(int index){
            pages.RemoveAt(index);
        }

        public override Page PageAt(int index){
            return pages.ElementAt(index);
        }

        public override Page GetCurrentPageLeft(){
            if (PageIndLeft < pages.Count && PageIndLeft >= 0){
                return pages.ElementAt(PageIndLeft);
            }
            return Page.GetBlank();
        }

        public Page GetCurrentPageRight(){
            if (PageIndRight < pages.Count && PageIndRight >= 0){
                return pages.ElementAt(PageIndRight);
            }
            return Page.GetBlank(); // if book ends on a left page pad with blank page.
        }
        
        public override void Back(){

            if (pageIndLeft == 0){
                pageIndFocused = 0;
                return;
            }

            GetCurrentPageLeft().Disable();
            GetCurrentPageRight().Disable();
            
            pageIndLeft -= 2;
            pageIndRight -= 2;

            pageIndFocused = pageIndLeft;
            
            Display();
        }

        public override void Next(){
            
            if (pageIndRight == PageCount() - 1){
                pageIndFocused = PageCount() - 1;
                return;
            }
            
            GetCurrentPageLeft().Disable();
            GetCurrentPageRight().Disable();
            
            pageIndLeft += 2;
            pageIndRight += 2;

            pageIndFocused = pageIndLeft;
            
            Display();
        }
        
        public override void GoTo(int pageNumber){ 
            
            GetCurrentPageLeft().Disable();
            GetCurrentPageRight().Disable();

            pageIndLeft = pageNumber - 1;
            pageIndRight = pageNumber;

            pageIndFocused = pageNumber;
            Display();
        }

        public override int CurrentPageNum(){
            return PageIndFocused + 1;
        }

        public override int PageCount(){
            return pages.Count;
        }

        public string BookPath{
            get{ return bookPath; }
            set{ bookPath = value; }
        }

        public List<Page> Pages{
            get{ return pages; }
        }

        public int LinesPerPage{
            get{ return linesPerPage; }
            set{ linesPerPage = value; }
        }

        public int PageIndLeft{
            get{ return pageIndLeft; }
        }

        public int PageIndRight{
            get{ return pageIndRight; }
        }

        public int PageIndFocused{
            get{ return pageIndFocused; }
        }

        public Vector3 BookPosition{
            get{ return bookPosition; }
        }
    }
}