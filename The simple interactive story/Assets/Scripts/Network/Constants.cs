﻿namespace Network
{
    public static class Constants
    {
        public const string Firestore =
            "https://firestore.googleapis.com/v1/projects/the-simple-interactive-story/databases/(default)/documents/";

        public const string UserStories = Firestore + "userStories/";

        public static string GetStoryPath(string userId, string storyId) 
            => $"{UserStories}{userId}/stories/{storyId}";
        public static string GetScenesPath(string userId, string storyId)
            => $"{UserStories}{userId}/stories/{storyId}/scenes";
    }
}