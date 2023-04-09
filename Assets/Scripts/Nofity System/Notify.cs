using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.Manager
{
    public class Notify : MonoBehaviour,IInGameNotify,IPopupNotify
    {
        public static Notify Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void NotifyStatus(string message)
        {
            
        }

        public void PopupNotifyError(string message)
        {
            
        }

        public void PopupNotifyWaring(string message)
        {
            
        }
    }
}
