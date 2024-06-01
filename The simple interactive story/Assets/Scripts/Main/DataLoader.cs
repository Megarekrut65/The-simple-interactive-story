using Global;
using Global.Localization;
using JetBrains.Annotations;
using Network;
using Network.Data;
using SimpleStory;
using SimpleStory.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = SimpleStory.Data.Scene;

namespace Main
{
    public class DataLoader: MonoBehaviour
    {
        [SerializeField] private Loader loader;
        [SerializeField] private ErrorLogger logger;

        [Header("Story")]
        [SerializeField] private Text title;
        [SerializeField] private Text author;
        [SerializeField] private Image banner;
        
        [Header("Buttons")]
        [SerializeField] private Button continueBtn;

        private string _storyId = "";
        private string _userId = "";
        private string _token = "";
        private void Start()
        {
            continueBtn.interactable = false;
            #if UNITY_EDITOR
            string publishId = "marko-bez-nazvi";// "sasha-taiemnicya-lisovogo-ostrivcya";
            #else
                string publishId = JsManager.UidFromUrl();
                _token = JsManager.Token();
            #endif

            if (publishId == "")
            {
                logger.ShowError(LocalizationManager.GetWordByKey("story-not-found"));
                return;
            }
            
            StartCoroutine(Fetcher.Get(Constants.GetPublishPath(publishId), _token, FetchPublishCallback)); 
        }

        private void FetchPublishCallback([CanBeNull] string error, [CanBeNull] string result)
        {
            if (error != null)
            {
                FirebaseError err = new FirebaseError(error);
                Debug.Log(err);
                logger.ShowError(err.ToString());
                return;
            }

            if (result == null)
            {
                Debug.Log("Story is null");
                logger.ShowError(LocalizationManager.GetWordByKey("story-not-found"));
                return;
            }
            
            FirestoreDocument<PublishFields> publishFields = JsonUtility.FromJson<FirestoreDocument<PublishFields>>(result);
            Publish publish = NetworkConverter.Convert(publishFields);
            _storyId = publish.storyId;
            _userId = publish.authorId;

            LoadStory();
        }

        private void LoadStory()
        {
            LocalStorage.SetValue("storyId", _storyId);
            LocalStorage.SetValue("userId", _userId);
            LocalStorage.SetValue("token", _token);
            
            LocalStorage.SetValue("language", "uk_UK");
            StartCoroutine(Fetcher.Get(Constants.GetStoryPath(_userId, _storyId), _token, FetchStoryCallback)); 
        }

        public void Continue()
        {
            SceneManager.LoadScene("Story", LoadSceneMode.Single);
        }

        public void PlayNew()
        {
            loader.Close();
            LocalStorage.SetValue($"{_userId}/{_storyId}/lastScene", "");
            StartCoroutine(Fetcher.Get(Constants.GetScenesPath(_userId, _storyId),
                _token, FetchScenesCallback));
        }
        
        private void FetchStoryCallback([CanBeNull] string error, [CanBeNull] string result)
        {
            if (error != null)
            {
                FirebaseError err = new FirebaseError(error);
                Debug.Log(err);
                logger.ShowError(err.ToString());
                return;
            }

            if (result == null)
            {
                Debug.Log("Story is null");
                logger.ShowError(LocalizationManager.GetWordByKey("story-not-found"));
                return;
            }
            
            FirestoreDocument<StoryFields> storyFields = JsonUtility.FromJson<FirestoreDocument<StoryFields>>(result);
            Story story = NetworkConverter.Convert(storyFields);

            LocalStorage.SetValue("font", story.font);
            Font font = FontManager.GetTextFont(story.font);
            title.font = font;
            author.font = font;

            title.text = story.title;
            author.text = story.author;

            if (story.banner != null)
            {
                StartCoroutine(UrlReader.ReadSprite(story.banner, (sprite) =>
                {
                    if (sprite != null) banner.sprite = sprite;
                    EndStoryLoad();
                }));
                
                return;
            }

            EndStoryLoad();
        }

        private void EndStoryLoad()
        {
            loader.Open();

            string json = LocalStorage.GetValue($"{_userId}/{_storyId}", "");
            if (json != "")
            {
                continueBtn.interactable = true;
            }
        }
        private void FetchScenesCallback([CanBeNull] string error, [CanBeNull] string result)
        {
            loader.Open();
            if (error != null)
            {
                FirebaseError err = new FirebaseError(error);
                logger.ShowError(err.ToString());
                return;
            }

            if (result == null)
            {
                logger.ShowError(LocalizationManager.GetWordByKey("story-not-found"));
                return;
            }
            
            FirestoreCollection<SceneFields> scenes = JsonUtility.FromJson<FirestoreCollection<SceneFields>>(result);
            Scene[] list = NetworkConverter.Convert(scenes);
            
            string json = JsonUtility.ToJson(new ScenesWrapper{scenes = list});
            
            LocalStorage.SetValue($"{_userId}/{_storyId}", json);
            
            SceneManager.LoadScene("Story", LoadSceneMode.Single);
        }
    }
}