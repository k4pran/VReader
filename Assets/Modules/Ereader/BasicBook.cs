using System.Collections.Generic;
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
        private float centreMargin;
        private float width;
        private float height;

        private float pageMarginTop;
        private float pageMarginBottom;
        private float pageMarginRight;
        private float pageMarginLeft;

        public BasicBook(string bookPath){
            this.bookPath = bookPath;
            this.pages = new List<Page>();
            this.pageIndLeft = 0;
            this.pageIndRight = 1;
            this.pageIndFocused = 0;
            this.bookPosition = new Vector3(0, 0, 0);
            this.centreMargin = 5;
            this.pageMarginTop = 10;
            this.pageMarginBottom = 10;
            this.pageMarginRight = 10;
            this.pageMarginLeft = 10;
            this.linesPerPage = 25;
        }

        public override void Display(){
            GetCurrentPage().Tmp.rectTransform.position = new Vector3(
                bookPosition.x - (width / 2) - centreMargin, bookPosition.y, bookPosition.z);
            GetCurrentPageRight().Tmp.rectTransform.position = new Vector3(
                bookPosition.x + (width / 2) + centreMargin, bookPosition.y, bookPosition.z);
            
            GetCurrentPage().Tmp.rectTransform.sizeDelta = new Vector2(width, height);
            GetCurrentPageRight().Tmp.rectTransform.sizeDelta = new Vector2(width, height);    
            
            AddPageRect(GetCurrentPage());
            AddPageRect(GetCurrentPageRight(), false);
            
            GetCurrentPage().Enable();
            GetCurrentPageRight().Enable();
        }

        public void LoadBook(){
            BookDotText dracula = new BookDotText(bookPath);
            string[] lines = dracula.Lines;
            StringBuilder sb = new StringBuilder();
            int lineNum = 0;
            int pageNum = 1;
            GameObject gameObj = new GameObject("page" + pageNum);
            TextMeshProUGUI tmp = gameObj.AddComponent<TextMeshProUGUI>();
            foreach(string line in lines){

                sb.Append(line);
                sb.Append("\n");

                if (lineNum == linesPerPage) {
                    gameObj = new GameObject("page" + pageNum);
                    tmp = gameObj.AddComponent<TextMeshProUGUI>();
                    TMP_TextEventHandler textHandler = gameObj.AddComponent<TMP_TextEventHandler>();
                    AddPage(ConstructPage(tmp, textHandler, "page" + pageNum, pageNum, sb.ToString()));
                    pageNum++;
                    lineNum = 0;
                    sb = new StringBuilder();
                }
                lineNum++;
            }
        }
        protected Page ConstructPage(TextMeshProUGUI tmp, TMP_TextEventHandler textHandler, string objName, int pageNum, string text){
            Page page = new Page(tmp, textHandler, objName, pageNum);
            Vector2 preferredValues = tmp.GetPreferredValues(text);
            page.PreferredVals = preferredValues;
            tmp.text = text;
            return page;
        }        

        
        public override void AddPage(Page page){
            Vector2 dims = page.Tmp.GetPreferredValues(page.Tmp.text);
            width = dims.x > width ? dims.x  : width;
            height = dims.y > height ? dims.y : height;
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

        public override Page GetCurrentPage(){
            return pages.ElementAt(PageIndLeft);
        }

        public void AddPageRect(Page page, bool isLeft=true){
                        
            if (page.Tmp.transform.childCount == 0){

                float mRightPlusCenter = isLeft ? pageMarginRight + centreMargin : pageMarginRight;
                float mLeftPlusCenter = !isLeft ? pageMarginLeft + centreMargin : pageMarginLeft;

                float xPosWithOffset = isLeft
                    ? bookPosition.x - (width / 2) - centreMargin / 2
                    : bookPosition.x + (width / 2) + centreMargin / 2;
                                
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 cubeVec3 = new Vector3(
                    width + mLeftPlusCenter + mRightPlusCenter, 
                    height + pageMarginTop + pageMarginBottom, 
                    0.001f);
                cube.transform.localScale = cubeVec3;
                cube.transform.localPosition = new Vector3(
                    xPosWithOffset,
                    page.Tmp.transform.localPosition.y, 
                    page.Tmp.transform.localPosition.z + 0.1f);
                cube.transform.parent = page.Tmp.transform;
            }
        }

        public Page GetCurrentPageRight(){
            if (PageIndRight < pages.Count){
                return pages.ElementAt(PageIndRight);
            }
            return Page.GetBlank(); // if book ends on a left page pad with blank page.
        }
        
        public override void Back(){

            if (pageIndLeft == 0){
                pageIndFocused = 0;
                return;
            }

            GetCurrentPage().Disable();
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
            
            GetCurrentPage().Disable();
            GetCurrentPageRight().Disable();
            
            pageIndLeft += 2;
            pageIndRight += 2;

            pageIndFocused = pageIndLeft;
            
            Display();
        }
        
        public override void GoTo(int pageNumber){ // todo - check if correct
            GetCurrentPage().Disable();
            GetCurrentPageRight().Disable();

            pageNumber--;
            // Seek to beginning
            if (pageNumber < 0){
                pageNumber = 0;
            }
            // Seek to end
            else if (pageNumber > pages.Count - 1){
                pageNumber = pages.Count - 1;
            }

            if (pageNumber % 2 == 0){
                pageIndLeft = pageNumber;
                pageIndRight = pageNumber + 1;
            }
            else{
                pageIndRight = pageNumber;
                pageIndLeft = pageNumber - 1;
            }

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

        public float Width{
            get{ return width; }
        }

        public float Height{
            get{ return height; }
        }
    }
}