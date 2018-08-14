using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

/// <summary>
/// A terrible mash of static instances....
/// Uses a menu shortcut hack to listen for keydown in edit mode.
/// uses normal event listener in play mode
/// 
/// Adds emnu item...
/// </summary>

namespace gamesolids
{
    [ExecuteInEditMode]
    public class KeyListener : MonoBehaviour {

        //do you want 'p' in edit mode?
        public bool ExecuteInEditMode = true;

        //Menu and KeyEvent both listen to 'P-Key'
        public string ListenToKey = "p";
        [MenuItem("Tools/MapTool/InvokeShortcut _p")]
        static void InvokeShortcut()
        {
            Instance.InvokeKeyEvent();
        }
        
        [Tooltip("Optional Button component to trigger when key is pressed.")]
        public static Button ButtonToTrigger;
        [Tooltip("Events to call directly when key is pressed.")]
        public UnityEvent OnKeyDown;
        
        //Static Instance spaghetti management
        [HideInInspector]
        public static KeyListener Instance;
        [HideInInspector]
        public KeyListener instance;

        //Invoke the Button or Event to be triggered by key press.
        public void InvokeKeyEvent()
        {
            if (KeyListener.ButtonToTrigger != null)
            {
                this.GetComponent<Button>().onClick.Invoke();
            }

            this.OnKeyDown.Invoke();
        }
        
        //Filter space key listener at start
        private void Start()
        {
            if (ListenToKey == " ") { ListenToKey = "space"; }
            
        }

        //use update to listen for key state and trigger event
        void Update()
        {
            //check if editmode is enabled
            if (this.ExecuteInEditMode || Application.isPlaying)
            {
                if (Input.GetKeyDown(ListenToKey))
                {
                    this.InvokeKeyEvent();
                }
            }
        }

        //Ugly but working
        //Fix static instance by twirling spaghetti
        private void LateUpdate()
        {
            Instance = this;
            this.instance = Instance;
        }
    }
}
