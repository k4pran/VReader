using UnityEngine;

namespace Ereader.Test_Scripts {
    
    public class LoadBookTest : MonoBehaviour{

        public EReaderBridge eReaderBridge;

        private void Start() {
            string bookPath = "Assets/Resources/Books/dracula.txt";

            GameObject importerGameObject = new GameObject();
            BookImporter bookImporter = importerGameObject.AddComponent<BookImporter>();
            bookImporter.Import(bookPath);
            eReaderBridge.InitBook("dracula");
        }
    }
}