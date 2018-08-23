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


    [ExecuteInEditMode]
    [System.Serializable]
    public class RecordWorldPosition : MonoBehaviour
    {
        //print debug messages to Console?
        private bool debugFlag = true;

        [Tooltip("Object to record position of. When mapped to the Player or PlayerCamera, this becomes quite flexible.")]
        [SerializeField]
        public Transform RecorderObject;

        //this puts the file between the unity and blender folder in the example project
        [SerializeField]
        public string fileName = "../../MapReportExample.txt";

        [HideInInspector]
        public string _FN;      //a private string to update filepath based on Unity project location.

        [HideInInspector]
        public string[] fileContents;

        [SerializeField]
        public List<POI> mapPoints = new List<POI>();

        public void Start()
        {
            //this places the file relative to the Unity project folder
            this._FN = Path.GetDirectoryName(Application.dataPath) + Path.DirectorySeparatorChar + this.fileName;
        }

        /// <summary>
        /// Adds one entry to the file.
        /// </summary>
        public void RecordPositon()
        {
            // Create StreamWriter to write struct values to text file.
            using (StreamWriter sw = File.AppendText(this._FN))
            {
                // convert to blender corodinate space
                Vector3 v = RecorderObject.transform.position;
                Vector3 bv = new Vector3(v[0], v[2], v[1]);

                //write coords to newline in file
                sw.WriteLine("Map Point, " + bv.ToString());

                if(debugFlag) Debug.Log("Position Recorded: { Map Point, " + bv.ToString() + "}");
            }
        }

        /// <summary>
        /// Reads file entries into list.
        /// </summary>
        public void GetAllPositions()
        {
            //clear current mapPoints
            mapPoints = new List<POI>();

            // Create file reader to retrieve values from text file.
            fileContents = File.ReadAllLines(this._FN);
            foreach(string sr in fileContents)
            {
                // convert from blender corodinate space
                char[] sep = new char[]{ ','};
                string[] ln = sr.Split(sep, 2);

                char[] trm = new char[] { '(', ')', ' '};
                Vector3 v = new Vector3();
                int vi = 0;
                foreach(string si in ln[1].Trim(trm).Split(sep))
                {
                    //build vector
                    Debug.Log(si);
                    v[vi] = float.Parse(si);
                    vi++;
                }
                Vector3 bv = new Vector3(v[0], v[2], v[1]);

                //build list of named points
                POI mp = new POI(ln[0], bv);
                mapPoints.Add(mp);

            }

            if(debugFlag) Debug.Log("Number of Map Points: "+ mapPoints.Count.ToString());
        }

        /// <summary>
        /// Writes empty string to file, erasing all entries.
        /// </summary>
        public void ClearFile()
        {
            File.WriteAllText(this._FN, String.Empty);
            if (debugFlag) Debug.Log("File Cleared.");
        }

        /// <summary>
        /// Writes current mapPoint List to File, one line per list item.
        /// </summary>
        public void RecordList()
        {
            if (mapPoints.Count > 0)
            {
                StreamWriter file = new System.IO.StreamWriter(this._FN);
                foreach(POI pm in mapPoints)
                    file.WriteLine(pm.name+", "+pm.pos.ToString());

                file.Close();
                if (debugFlag) Debug.Log("List written to file.");
            }
        }
        
        //Ugly but working
        //update path to application working folder
        private void LateUpdate()
        {
            this._FN = Path.GetDirectoryName(Application.dataPath) + Path.DirectorySeparatorChar + this.fileName;
        }
    }

    //the line formatting
    [ExecuteInEditMode]
    [Serializable]
    public class POI
    {
        public string name;
        public Vector3 pos;
        public POI(string n, Vector3 v)
        {
            name = n;
            pos = v;
        }
    }

}
