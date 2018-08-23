using UnityEngine;
using UnityEditor;

namespace gamesolids
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(RecordWorldPosition))]
    public class RecordWorldPositionEditor : Editor
    {
        SerializedProperty m_fileName;
        SerializedProperty m_recorderObject;
        SerializedProperty m_mapPoints;

        private string[] placeNames;

        void OnEnable()
        {
            m_fileName = serializedObject.FindProperty("fileName");
            m_recorderObject = serializedObject.FindProperty("RecorderObject");
            m_mapPoints = serializedObject.FindProperty("mapPoints");
        }

        /// <summary>
        /// Paints the UI for the RecordWorldPosition instance.
        /// </summary>
        public override void OnInspectorGUI()
        {
            RecordWorldPosition myMapLocationList = (RecordWorldPosition)target;

            //action buttons

            //Expose Component links to variables
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_fileName);
            EditorGUILayout.PropertyField(m_recorderObject);

            GUILayout.Space(8f);

            GUILayout.Label("Actions");

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            if (GUILayout.Button("(Re)Load List", GUILayout.MinWidth(120f), GUILayout.ExpandWidth(true)))
            {
                myMapLocationList.GetAllPositions();
            }
            if (GUILayout.Button("Tag Position", GUILayout.MinWidth(120f), GUILayout.ExpandWidth(true)))
            {
                myMapLocationList.RecordPositon();
            }
            if (GUILayout.Button("Save List Changes", GUILayout.MinWidth(120f), GUILayout.ExpandWidth(true)))
            {
                myMapLocationList.RecordList();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(8f);

            //The interactive list of mapPoints that have already been recorded
            EditorGUILayout.LabelField("Map Point List", GUILayout.MaxWidth(40f), GUILayout.ExpandWidth(true));
            for (int i=0; i < m_mapPoints.arraySize; i++)
            {
                SerializedProperty item = m_mapPoints.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical( GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal( GUILayout.ExpandWidth(true));

                GUILayout.Space(16f);
                
                //Go Button will focus ViewPort on that location vector 
                if (GUILayout.Button("Go", GUILayout.Width(36f)))
                {
                    if (SceneView.sceneViews.Count > 0)
                    {
                        SceneView sceneView = (SceneView)SceneView.sceneViews[0];
                        sceneView.Focus();
                        GameObject prevSelection = Selection.activeGameObject;
                        GameObject focusPoint = new GameObject();
                        focusPoint.transform.position = item.FindPropertyRelative("pos").vector3Value;
                        Selection.activeGameObject = focusPoint;
                        sceneView.FrameSelected(false);
                        GameObject.DestroyImmediate(focusPoint);
                        Selection.activeGameObject = prevSelection;
                    }
                }

                //Points can be tagged with individual names for easier finding
                item.FindPropertyRelative("name").stringValue = EditorGUILayout.TextField(item.FindPropertyRelative("name").stringValue, GUILayout.Height(18f), GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                //the position vector of the tagged point
                item.FindPropertyRelative("pos").vector3Value = EditorGUILayout.Vector3Field("", item.FindPropertyRelative("pos").vector3Value, GUILayout.Height(22f), GUILayout.MinWidth(140f), GUILayout.ExpandWidth(true));

                GUILayout.Space(20f);

                //X button will delete associated mapPoint from List. cannot be undund!...
                if(GUILayout.Button("X", GUILayout.Width(24f)))
                {
                    m_mapPoints.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

            }

            GUILayout.Space(8f);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
