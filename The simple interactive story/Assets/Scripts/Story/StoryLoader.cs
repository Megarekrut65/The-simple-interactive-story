using System;
using Global;
using Global.Localization;
using JetBrains.Annotations;
using Story.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Story
{
    public class StoryLoader : MonoBehaviour
    {
        [SerializeField] private string mainFrame;

        [SerializeField] private GameObject variantPrefab;
        
        [Header("Parts")]
        [SerializeField] private Transform content;
        [SerializeField] private Image background;
        [SerializeField] private Image characterLeft;
        [SerializeField] private Image characterRight;
        [SerializeField] private GameObject textBtn;
        [SerializeField] private GameObject textObj;
        [SerializeField] private Text text;

        private JsonStoryParser parser;
        private void Start()
        {
            parser = new JsonStoryParser(mainFrame);
            LoadFrame(parser.Frames[mainFrame]);

            /*foreach (var item in dic)
            {
                Debug.Log($"{item.Value.id}, {item.Value.textKey}, " +
                          $"{item.Value.background}, {item.Value.characterLeft}, {item.Value.characterRight}");
                if(item.Value.answers == null) continue;
                foreach (var ans in item.Value.answers)
                {
                    Debug.Log($"{ans.textKey}, {ans.action}, {ans.nextFrameId}");
                }
            }*/
        }

        private void LoadFrame(Frame frame)
        {
            LoadText(frame.textKey);
            LoadAnswers(frame.answers);
            LoadImage(background, "Backgrounds",frame.background);
            LoadImage(characterLeft, "Characters",frame.characterLeft);
            LoadImage(characterRight, "Characters",frame.characterRight);
        }

        private void LoadText([CanBeNull] string key)
        {
            textObj.SetActive(true);

            if(key != null) text.GetComponent<Text>().text = LocalizationManager.GetWordByKey(key);
            else textObj.SetActive(false);
            
            textBtn.SetActive(textObj.activeInHierarchy);
        }
        
        private void LoadAnswers([CanBeNull] Answer[] answers)
        {
            foreach (Transform child in content) {
                Destroy(child.gameObject);
            }
            if(answers == null) return;

            foreach (Answer answer in answers)
            {
                GameObject obj = Instantiate(variantPrefab, content, false);
                obj.GetComponent<Variant>().SetData(answer, AnswerClick);
            }
        }

        private void AnswerClick(string frameId, string action)
        {
            if(frameId == null || !parser.Frames.ContainsKey(frameId)) return;

            LoadFrame(parser.Frames[frameId]);
            
            if(action == null) return;
            Debug.Log("Action");

        }
        private void LoadImage(Image img, string folder, [CanBeNull] string filename)
        {
            if(filename == null) return;

            Sprite sprite = GetSpriteFromImage(System.IO.Path.Combine($"{Application.streamingAssetsPath}/{folder}/", 
                $"{filename}.png"));

            if (sprite != null) img.sprite = sprite;
        }
        private Sprite GetSpriteFromImage(string imgPath) {
            byte[] pngBytes = AllFileReader.ReadBytes(imgPath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);
            
            Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), 
                new Vector2(0.5f, 0.5f), 100.0f);
 
            return fromTex;
        }
    }
}