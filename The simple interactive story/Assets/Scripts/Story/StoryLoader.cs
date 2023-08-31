using System;
using Global;
using Global.Localization;
using JetBrains.Annotations;
using Story.Data;
using UnityEngine;
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
        
        [Header("Images")]
        [SerializeField] private Image left;
        [SerializeField] private Image right;
        [SerializeField] private Image centerUnder;
        [SerializeField] private Image centerOver;
        
        [Header("Texts")]
        [SerializeField] private GameObject textObj;
        [SerializeField] private Text text;

        private JsonStoryParser _parser;
        private const string StoryName = "MySimpleStory";

        private void Start()
        {
            _parser = new JsonStoryParser(StoryName, mainFrame);
            LoadFrame(_parser.Frames[mainFrame]);

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
            
            LoadImage(left, "Images",frame.images?.left);
            LoadImage(right, "Images",frame.images?.right);
            LoadImage(centerUnder, "Images",frame.images?.centerUnder);
            LoadImage(centerOver, "Images",frame.images?.centerOver);
        }

        private void LoadText([CanBeNull] string key)
        {
            textObj.SetActive(true);

            if(key != null) text.GetComponent<Text>().text = LocalizationManager.GetWordByKey(key);
            else textObj.SetActive(false);
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
            if(frameId == null || !_parser.Frames.ContainsKey(frameId)) return;

            LoadFrame(_parser.Frames[frameId]);
            
            if(action == null) return;
            Debug.Log("Action");

        }
        private void LoadImage(Image img, string folder, [CanBeNull] string filename)
        {
            if(filename == null) return;

            Sprite sprite = FileReader.ReadSprite($"{Application.streamingAssetsPath}/{StoryName}/" +
                                                  $"{folder}/{filename}.png");

            if (sprite != null) img.sprite = sprite;
        }
    }
}