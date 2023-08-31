using System;
using System.Collections;
using Global;
using Global.Localization;
using Global.Sound;
using JetBrains.Annotations;
using Story.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Story
{
    public class StoryLoader : MonoBehaviour
    {
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

        private StoryController _controller;

        private void Start()
        {
            string storyId = LocalStorage.GetValue("storyId", "");
            string language = LocalStorage.GetValue("language", "en_US");
            if(storyId == "") return;
            
            StartCoroutine(Load(storyId, language));
        }

        private IEnumerator Load(string storyId, string language)
        {
            _controller = new StoryController(storyId, language);
            LoadFrame(_controller.GetFrame(_controller.Config.mainFrame));
            yield return null;
        }
        private void LoadFrame(Frame frame)
        {
            if(frame == null) return;

            LoadMusic(frame);
            
            LoadText(frame.textKey);
            LoadAnswers(frame.answers);
            
            LoadImage(background, "Backgrounds",frame.background);
            
            LoadImage(left, "Images",frame.images?.left);
            LoadImage(right, "Images",frame.images?.right);
            LoadImage(centerUnder, "Images",frame.images?.centerUnder);
            LoadImage(centerOver, "Images",frame.images?.centerOver);
        }

        private void LoadMusic(Frame frame)
        {
            if(frame.music == null) return;
            
            MusicManager.ChangeAudioClip(_controller.GetClip(frame.music));
        }
        private void LoadText([CanBeNull] string key)
        {
            textObj.SetActive(true);

            if(key != null) text.GetComponent<Text>().text = _controller.GetWord(key);
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
                
                answer.textKey = _controller.GetWord(answer.textKey);
                obj.GetComponent<Variant>().SetData(answer, AnswerClick);
            }
        }

        private void AnswerClick(string frameId, string action)
        {
            if(frameId == null) return;

            LoadFrame(_controller.GetFrame(frameId));
            
            if(action == null) return;
            Debug.Log("Action");

        }
        private void LoadImage(Image img, string folder, [CanBeNull] string filename)
        {
            if(filename == null) return;

            Sprite sprite = _controller.GetSprite($"{folder}/{filename}");

            if (sprite != null) img.sprite = sprite;
        }
    }
}