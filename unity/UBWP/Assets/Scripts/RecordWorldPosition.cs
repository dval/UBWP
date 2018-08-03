using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Adds lines to a file in  "name", (0,0,0)  format.
/// 
/// </summary>
namespace gamesolids
{
    //the line formatting
    [ExecuteInEditMode]
    public struct POI { public string name; public Vector3 pos; }


    [ExecuteInEditMode]
    public class RecordWorldPosition : MonoBehaviour
    {


        [Tooltip("Object to record position of. When mapped to the Player or PlayerCamera, this becomes quite flexible.")]
        public Transform RecorderObject;

        //this puts the file between the unity and blender folder in the example project
        public string fileName = "../../MapReportExample.txt";

        [HideInInspector]
        public string _FN;      //a private string to update filepath based on Unity project location.


        public void Start()
        {
            //this places the file relative to the Unity project folder
            this._FN = Path.GetDirectoryName(Application.dataPath) + Path.DirectorySeparatorChar + this.fileName;
        }

        public void RecordPositon()
        {
            // Create StreamWriter to write struct values to text file.
            using (StreamWriter sw = File.AppendText(this._FN))
            {
                // convert to blender corodinate space
                Vector3 v = RecorderObject.transform.position;
                Vector3 bv = new Vector3(v[0], v[2], v[1]);
                
                //write coords to newline in file
                sw.WriteLine("\"name\"," + bv.ToString());
                
                Debug.Log("Position Recorded: " + bv.ToString());
            }
        }

        //Ugly but working
        //update path to application working folder
        private void LateUpdate()
        {
            this._FN = Path.GetDirectoryName(Application.dataPath) + Path.DirectorySeparatorChar + this.fileName;
        }
    }
}
