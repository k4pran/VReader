using System;
using TMPro;
using UnityEngine;

public class DynamicPageController : MonoBehaviour {

    public BookPro book; 
    private float margin;

    public void generatePage(String pageFrontText, String pageBackText){
        GameObject frontObj = new GameObject("pageFront");
        GameObject backObj = new GameObject("pageBack");
        
        TextMeshProUGUI frontTMP = frontObj.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI backTMP = backObj.AddComponent<TextMeshProUGUI>();

        frontTMP.text = pageFrontText;
        backTMP.text = pageBackText;
        
        AddPaper(frontTMP, backTMP);
        fitTMP(frontTMP);
        fitTMP(backTMP);
    }

    public void fitTMP(TextMeshProUGUI textMeshProUgui) {
        RectTransform rectTransform = textMeshProUgui.GetComponent<RectTransform>();

        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, margin, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, margin, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, margin, 0);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, margin, 0);
        
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
    }
    
    public void AddPaper(TextMeshProUGUI front, TextMeshProUGUI back){

        int pageNum = book.papers.Length * 2;
        
        GameObject pagePrefab = (GameObject)Resources.Load("prefabs/PageBlank", typeof(GameObject));
        GameObject frontPage = Instantiate(pagePrefab, new Vector3(0, 0, 0), Quaternion.identity, book.transform);
        frontPage.name = "page" + pageNum;
        GameObject backPage = Instantiate(pagePrefab, new Vector3(0, 0, 0), Quaternion.identity, book.transform);
        backPage.name = "page" + (pageNum + 1);
        
        frontPage.transform.SetParent(book.transform, false);
        backPage.transform.SetParent(book.transform, false);
        
        Paper newPaper = new Paper();
        newPaper.Front = frontPage;
        newPaper.Back = backPage;
        front.transform.parent = newPaper.Front.transform;
        back.transform.parent = newPaper.Back.transform;
        
        Paper[] papers = new Paper[book.papers.Length + 1];
        for(int i=0; i < book.papers.Length ;i++) {
            papers[i] = book.papers[i];
        }
        papers[papers.Length-1] = newPaper;
        
        book.papers = papers;
        book.EndFlippingPaper = book.papers.Length - 1;
        book.UpdatePages();
    }
}
