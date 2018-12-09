using System.Collections.Generic;
using UnityEngine;

namespace Ereader{
    abstract public class Book {

        private DynamicPageController dynamicPageController;
        private float paddingTop = 50;
        private float paddingBottom = 50;
        private float paddingLeft = 20;
        private float paddingRight = 20;

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

        public float PaddingTop{
            get{ return paddingTop; }
            set{ paddingTop = value; }
        }

        public float PaddingBottom{
            get{ return paddingBottom; }
            set{ paddingBottom = value; }
        }

        public float PaddingLeft{
            get{ return paddingLeft; }
            set{ paddingLeft = value; }
        }

        public float PaddingRight{
            get{ return paddingRight; }
            set{ paddingRight = value; }
        }
    }
}