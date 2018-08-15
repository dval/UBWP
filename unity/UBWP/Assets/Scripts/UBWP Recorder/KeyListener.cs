using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor;

/// <summary>
/// A terrible mash of static instances....
/// Uses a menu shortcut hack to listen for keydown in edit mode.
/// uses normal event listener in play mode.
/// </summary>

namespace gamesolids
{
    [ExecuteInEditMode]
    public class KeyListener : MonoBehaviour
    {
        public bool ExecuteInEditMode = true;
        
        public enum Modifier { None = 0, CTRL = 1, SHIFT = 2, ALT = 3 }
        [SerializeField]
        Modifier ModifierKey = Modifier.ALT;

        public string ListenToKey = "p";

        [Tooltip("The gamepad or controller button to listen to.")]
        public string JoystickAxis = "Fire3";
        
        [Tooltip("Optional Button component to trigger when key is pressed.")]
        public Button UIButton;

        [Tooltip("Events to call directly when key is pressed.")]
        public UnityEvent OnKeyDown;// = new CustomKeyEvent();

        /// <summary>
        /// Constructer initializes the key mapped listeners
        /// </summary>
        public KeyListener() 
        {
            SceneView.onSceneGUIDelegate += view =>
            {
                Event e = Event.current;
                if (e.type == EventType.KeyDown)
                {
                    //check if editmode is enabled
                    if (this.ExecuteInEditMode)
                    {
                        RespondEvent(e);
                    }
                }
            };
        }
        

        /// <summary>
        /// Invoke the Button or Event to be triggered by key press.
        /// </summary>
        public void InvokeKeyEvent()
        {
            if (UIButton != null)
            {
                UIButton.GetComponent<Button>().onClick.Invoke();
            }

            this.OnKeyDown.Invoke();
        }

        /// <summary>
        /// Filter space key listener at start
        /// </summary>
        private void Start()
        {
            if (ListenToKey == " ") { ListenToKey = "space"; }
            
        }
        
        /// <summary>
        /// use update to listen for key state and trigger event during runtime
        /// </summary>
        void Update()
        {
            //check if editmode is enabled
            if (this.ExecuteInEditMode || Application.isPlaying)
            {
                RespondEvent();
            }
        }

        /// <summary>
        /// Respond to combination of key and modifier during playmode
        /// </summary>
        public void RespondEvent()
        {
            if (Input.GetKeyDown(ListenToKey) || Input.GetButtonDown(JoystickAxis))
            {
                if (ModifierKey == Modifier.None || Input.GetButtonDown(JoystickAxis))
                {
                    this.InvokeKeyEvent();
                }
                else if (ModifierKey == Modifier.CTRL)
                {
                    if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetButtonDown(JoystickAxis))
                    {
                        this.InvokeKeyEvent();
                    }
                }
                else if (ModifierKey == Modifier.SHIFT)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetButtonDown(JoystickAxis))
                    {
                        this.InvokeKeyEvent();
                    }
                }
                else if (ModifierKey == Modifier.ALT)
                {
                    if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt) || Input.GetButtonDown(JoystickAxis))
                    {
                        this.InvokeKeyEvent();
                    }
                }
            }
            //else a big fat nothing. Nope. Done.
        }


        /// <summary>
        /// Respond to combination of key and modifier during edit mode
        /// </summary>
        bool ctrlFlag = false;
        bool shftFlag = false;
        bool altFlag = false;

        public void RespondEvent(Event evt)
        {
            //Flag the modifier keys, since we can't listen to other key events from within this key event. (black hole?)
            if (evt.type == EventType.KeyDown)
            {
                if (ModifierKey == Modifier.CTRL && (evt.keyCode == (KeyCode.LeftControl) || evt.keyCode == (KeyCode.RightControl)))
                    ctrlFlag = true;
                if (ModifierKey == Modifier.SHIFT && (evt.keyCode == (KeyCode.LeftShift) || evt.keyCode == (KeyCode.RightShift)))
                    shftFlag = true;
                if (ModifierKey == Modifier.ALT && (evt.keyCode == (KeyCode.LeftAlt) || evt.keyCode == (KeyCode.RightAlt)))
                    altFlag = true;
            }
            if (evt.type == EventType.KeyUp)
            {
                if (ModifierKey == Modifier.CTRL && (evt.keyCode == (KeyCode.LeftControl) || evt.keyCode == (KeyCode.RightControl)))
                    ctrlFlag = false;
                if (ModifierKey == Modifier.SHIFT && (evt.keyCode == (KeyCode.LeftShift) || evt.keyCode == (KeyCode.RightShift)))
                    shftFlag = false;
                if (ModifierKey == Modifier.ALT && (evt.keyCode == (KeyCode.LeftAlt) || evt.keyCode == (KeyCode.RightAlt)))
                    altFlag = false;
            }

            //Listen for lListenToKey
            if (evt.keyCode == (KeyCode)System.Enum.Parse(typeof(KeyCode), ListenToKey.ToUpper()))
            {
                //check for modifiers and the like
                if (ModifierKey == Modifier.None)
                {
                    this.InvokeKeyEvent();
                }
                else if (ctrlFlag || shftFlag || altFlag)
                {
                    this.InvokeKeyEvent();
                }
            }
            //else nothing.
        }
    }
}
