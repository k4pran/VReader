using System;
using Ereader;
using TMPro;
using UnityEngine;

public class DynamicPageController : MonoBehaviour {

    public BookPro bookCurlWrapper; 

    public void fitTMP(TextMeshProUGUI textMeshProUgui, 
        float paddingTop, float paddingBottom, float paddingLeft, float paddingRight) {
        RectTransform rectTransform = textMeshProUgui.GetComponent<RectTransform>();

        rectTransform.offsetMax = new Vector2(-paddingLeft, -paddingTop);
        rectTransform.offsetMin = new Vector2(paddingRight, paddingBottom);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
    }
    
    public void AddPaper(TextMeshProUGUI front, TextMeshProUGUI back){

        int pageNum = bookCurlWrapper.papers.Length * 2;
        
        GameObject pagePrefab = (GameObject)Resources.Load("prefabs/PageBlank", typeof(GameObject));
        GameObject frontPage = Instantiate(pagePrefab, new Vector3(0, 0, 0), Quaternion.identity, bookCurlWrapper.transform);
        frontPage.name = "page" + pageNum;
        GameObject backPage = Instantiate(pagePrefab, new Vector3(0, 0, 0), Quaternion.identity, bookCurlWrapper.transform);
        backPage.name = "page" + (pageNum + 1);
        
        frontPage.transform.SetParent(bookCurlWrapper.transform, false);
        backPage.transform.SetParent(bookCurlWrapper.transform, false);
        
        Paper newPaper = new Paper();
        newPaper.Front = frontPage;
        newPaper.Back = backPage;
        front.transform.parent = newPaper.Front.transform;
        back.transform.parent = newPaper.Back.transform;
        
        Paper[] papers = new Paper[bookCurlWrapper.papers.Length + 1];
        for(int i=0; i < bookCurlWrapper.papers.Length ;i++) {
            papers[i] = bookCurlWrapper.papers[i];
        }
        papers[papers.Length-1] = newPaper;
        
        bookCurlWrapper.papers = papers;
        bookCurlWrapper.EndFlippingPaper = bookCurlWrapper.papers.Length - 1;
        bookCurlWrapper.UpdatePages();
    }
}
