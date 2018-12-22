using System;
using TMPro;
using UnityEngine;

public class DynamicPageController : MonoBehaviour {

    public Canvas canvas;
    public BookPro bookCurlWrapper;

    // todo call this method when canvas (book) size has changed
    public void fitPageContainer(GameObject pageContainer, Boolean isLeftPage){
        
        RectTransform pageRect = pageContainer.GetComponent<RectTransform>();

        if (isLeftPage){
            pageRect.offsetMax = new Vector2(0, 0);
            pageRect.offsetMin = new Vector2(-(canvas.GetComponent<RectTransform>().rect.width / 2), 0);
        }
        else{
            pageRect.offsetMax = new Vector2(canvas.GetComponent<RectTransform>().rect.width / 2, 0);
            pageRect.offsetMin = new Vector2(canvas.GetComponent<RectTransform>().rect.width, 0);
        }
        
        pageRect.anchorMin = new Vector2(0, 0);
        pageRect.anchorMax = new Vector2(1, 1);
    }

    public void fitTMP(TextMeshProUGUI textMeshProUgui, 
        float paddingTop, float paddingBottom, float paddingLeft, float paddingRight) {
        RectTransform rectTransform = textMeshProUgui.GetComponent<RectTransform>();

        rectTransform.offsetMax = new Vector2(-paddingLeft, -paddingTop);
        rectTransform.offsetMin = new Vector2(paddingRight, paddingBottom);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
    }
    
    public Paper AddPaper(){
        
        int pageNum = bookCurlWrapper.papers.Length * 2;
        
        GameObject frontPage = GetPagePrefab(pageNum - 1);
        frontPage.transform.SetParent(bookCurlWrapper.transform, false);
        
        GameObject backPage = GetPagePrefab(pageNum);
        backPage.transform.SetParent(bookCurlWrapper.transform, false);

        bool isLeft = pageNum % 2 != 0;
        fitPageContainer(frontPage, isLeft);
        fitPageContainer(backPage, isLeft);
        
        return GetPaper(frontPage, backPage);
    }

    private GameObject GetPagePrefab(int pageNum){
        GameObject pagePrefab = (GameObject)Resources.Load("prefabs/PageBlank", typeof(GameObject));
        GameObject page = Instantiate(pagePrefab, new Vector3(0, 0, 0), Quaternion.identity, bookCurlWrapper.transform);
        page.name = "Page" + pageNum;
        return page;
    }

    private Paper GetPaper(GameObject frontPage, GameObject backPage){
        Paper newPaper = new Paper();
        newPaper.Front = frontPage;
        newPaper.Back = backPage;
        
        Paper[] papers = new Paper[bookCurlWrapper.papers.Length + 1];
        for(int i = 0; i < bookCurlWrapper.papers.Length; i++) {
            papers[i] = bookCurlWrapper.papers[i];
        }
        
        papers[papers.Length - 1] = newPaper;
        
        bookCurlWrapper.papers = papers;
        bookCurlWrapper.EndFlippingPaper = bookCurlWrapper.papers.Length - 1;
        bookCurlWrapper.UpdatePages();

        return newPaper;
    }
}
