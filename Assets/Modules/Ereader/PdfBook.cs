using System.Text;
using PdfMetrics;
using TMPro;
using UnityEngine;

namespace Ereader{
        
    public class PdfBook : BasicBook{
        
        private BookMetrics bookMetrics;
        
        private float centreMargin;

        private float pageMarginTop;
        private float pageMarginBottom;
        private float pageMarginRight;
        private float pageMarginLeft;

        private float scaleFactorW;
        private float scaleFactorH;
        private float SPACING_PRECISION = 12;

        
        public PdfBook(string bookPath) : base(bookPath){
            bookMetrics = BookMetrics.FromJson(bookPath);
        }
        
        public new void LoadBook(){
                        
            GameObject gameObj = new GameObject("dummy_page");
            TextMeshProUGUI tmp = gameObj.AddComponent<TextMeshProUGUI>();
            tmp.text = " ";
            tmp.ForceMeshUpdate();

            if (bookMetrics.Pages[0].LineMetrices[0].ToString().Length > 0){
                DetermineScaledDims(tmp, bookMetrics.Pages[0].LineMetrices[0]);
                GameObject.Destroy(gameObj);
            }
            
            tmp.fontSize = SPACING_PRECISION;

            float spaceWidth = tmp.GetPreferredValues(" a").x - tmp.GetPreferredValues("a").x;
            float spaceHeight = tmp.GetPreferredValues("\n").y;
            
            StringBuilder sb = new StringBuilder();
            foreach(PageMetrics page in bookMetrics.Pages){

                foreach(LineMetrics line in page.LineMetrices){
                    tmp.fontSize = line.ChunkMetrices[0].GetFontSize();
                    tmp.ForceMeshUpdate();

                    var v = tmp.GetPreferredValues(line.ToString());
                    
                    Styler.SetScaler(scaleFactorH);
                    Styler.StyleLine(line);
                    sb.Append(line.PaddedLine(bookMetrics.PageMaxWidth, spaceHeight, spaceWidth, SPACING_PRECISION));
                }

                gameObj = new GameObject("page" + page.PageNumber);
                tmp = gameObj.AddComponent<TextMeshProUGUI>();
                TMP_TextEventHandler textHandler = gameObj.AddComponent<TMP_TextEventHandler>();
                AddPage(ConstructPage(tmp, textHandler, "page" + page.PageNumber, page.PageNumber, sb.ToString()));
                sb = new StringBuilder();
            }
        }

        private void DetermineScaledDims(TextMeshProUGUI dummyPage, LineMetrics line){
            float widthProportion  =  bookMetrics.LineWidthProportion(line);
            float heightProportion =  bookMetrics.LineHeightProportion(line);
            
            dummyPage.text = line.ToString();
            dummyPage.fontSize = line.ChunkMetrices[0].GetFontSize();
            Vector2 vec = dummyPage.GetPreferredValues(line.ToString());

            float tmpWidthProportion = vec.x / bookMetrics.PageMaxWidth;
            float tmpHeightProportion = vec.y / bookMetrics.PageMaxHeight;
            
            scaleFactorW = widthProportion / tmpWidthProportion;
            scaleFactorH = heightProportion / tmpHeightProportion;
        }
        
        private new Page ConstructPage(TextMeshProUGUI tmp, TMP_TextEventHandler textHandler, string objName, int pageNum, string text){
            Page page = new Page(tmp, textHandler, objName, pageNum);
            page.Tmp.overflowMode = TextOverflowModes.Overflow;
            page.PreferredVals = new Vector2(bookMetrics.PageMaxWidth, bookMetrics.PageMaxHeight);
            tmp.text = text;
            tmp.faceColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            return page;
        }

        public new void Display(){
            GetCurrentPage().Tmp.rectTransform.position = new Vector3(
                BookPosition.x - (bookMetrics.PageMaxWidth / 2) - centreMargin, BookPosition.y, bookPosition.z);
            GetCurrentPageRight().Tmp.rectTransform.position = new Vector3(
                BookPosition.x + (bookMetrics.PageMaxWidth / 2) + centreMargin, BookPosition.y, bookPosition.z);
            
            GetCurrentPage().Tmp.rectTransform.sizeDelta = new Vector2(bookMetrics.PageMaxWidth, bookMetrics.PageMaxHeight);
            GetCurrentPageRight().Tmp.rectTransform.sizeDelta = new Vector2(bookMetrics.PageMaxWidth, bookMetrics.PageMaxHeight);    
            
            AddPageRect(GetCurrentPage());
            AddPageRect(GetCurrentPageRight(), false);
            
            GetCurrentPage().Enable();
            GetCurrentPageRight().Enable();
        }
        
        public new void AddPageRect(Page page, bool isLeft=true){
                        
            if (page.Tmp.transform.childCount == 0){

                float mRightPlusCenter = isLeft ? pageMarginRight + centreMargin : pageMarginRight;
                float mLeftPlusCenter = !isLeft ? pageMarginLeft + centreMargin : pageMarginLeft;

                float xPosWithOffset = isLeft
                    ? BookPosition.x - (bookMetrics.PageMaxWidth / 2) - centreMargin / 2
                    : BookPosition.x + (bookMetrics.PageMaxWidth / 2) + centreMargin / 2;
                                
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 cubeVec3 = new Vector3(
                    bookMetrics.PageMaxWidth + mLeftPlusCenter + mRightPlusCenter, 
                    bookMetrics.PageMaxHeight + pageMarginTop + pageMarginBottom, 
                    0.001f);
                cube.transform.localScale = cubeVec3;
                cube.transform.localPosition = new Vector3(
                    xPosWithOffset,
                    page.Tmp.transform.localPosition.y, 
                    page.Tmp.transform.localPosition.z + 1f);
                cube.transform.parent = page.Tmp.transform;
            }
        }
    }
}