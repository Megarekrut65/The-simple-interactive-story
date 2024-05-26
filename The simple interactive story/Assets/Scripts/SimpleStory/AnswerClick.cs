using System;
using UnityEngine;

namespace SimpleStory
{
    public class AnswerClick : MonoBehaviour
    {
        public Action Click;

        public void ClickEvent()
        {
            Click?.Invoke();
        }
    }
}