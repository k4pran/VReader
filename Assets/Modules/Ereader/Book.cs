using System.Collections.Generic;
using UnityEngine;

namespace Ereader{
    abstract public class Book{

        abstract public void Display();
        abstract public void AddPage(Page page);
        abstract public void Remove(Page page);
        abstract public void Remove(int index);
        abstract public Page PageAt(int index);
        abstract public Page GetCurrentPage();
        abstract public void Next();
        abstract public void Back();
        abstract public void GoTo(int pageNumber);
        abstract public int PageCount();
        abstract public int CurrentPageNum();
    }
}