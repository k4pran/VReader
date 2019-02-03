using System.Collections.Generic;
using UnityEngine;

namespace Ereader {
    public class LibLoader : MonoBehaviour {
        
        private void Start() {
            List<BookInfo> booksInfo = BookInfoMapper.DeserializeAll();
            Debug.Log("");
        }
    }
}