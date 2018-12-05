using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Utils{
    public class Generators{

        public static Image GenImageUI(Sprite imgSprite){
            GameObject gameObject = new GameObject(); 
            Image img = gameObject.AddComponent<Image>();
            img.sprite = imgSprite;
            return img;
        }

        public static Material GenMaterialFromImage(string imgPath){
            Material mat = new Material(Shader.Find("Specular"));
            mat.SetTexture("_MainTex", GenSpriteFromImg(imgPath).texture);
            return mat;
        }        
        
        public static Material GenSkyBoxMaterial(string flavour="Cubemap"){
            Material mat = new Material(Shader.Find("Skybox/" + flavour));
            return mat;
        }
        
        public static Sprite GenSpriteFromImg(string imgPath){
            byte[] bytes = File.ReadAllBytes(imgPath);
            Texture2D imgTexture = new Texture2D(0, 0);
            ImageConversion.LoadImage(imgTexture, bytes);             
            Sprite sprite = Sprite.Create(imgTexture, new Rect(0, 0, imgTexture.width, imgTexture.height), new Vector2(0f, 0f), 1f);

            string[] pathParts = imgPath.Split('/');
            sprite.name = "image_" + pathParts[pathParts.Length - 1];
            return sprite;
        }
    }
}