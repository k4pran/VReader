using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;


namespace Ereader {
    
    // Purpose of this class is to display a list of books available from library and allow user to select
    // a book to read.
    public class BookSelect : MonoBehaviour {

        public Image DisplayedImage;
        public List<Sprite> sprites;

        // Loads book images from library
        private void Start() {
            List<BookInfo> booksInfo = BookInfoMapper.DeserializeAll();

            foreach(var bookInfo in booksInfo) {
                if (File.Exists(bookInfo.thumbnailPath)) {
                    
                    sprites.Add(Generators.GenSpriteFromImg(bookInfo.thumbnailPath));
                }
            }

            if (sprites.Count > 0) DisplayedImage.sprite = sprites.ElementAt(0);
            // todo handle if no books added to library - replace selection with link to import books?
            // todo use default image with book title if no thumbnail exists
        }
    }
}