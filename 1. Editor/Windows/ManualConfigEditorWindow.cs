using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityMCP.Editor.Models;

namespace UnityMCP.Editor.Windows
{
    // Editor window to display manual configuration instructions
    public class ManualConfigEditorWindow : EditorWindow
    {
        private string configPath;
        private string configJson;
        private Vector2 scrollPos;
        private bool pathCopied = false;
        private bool jsonCopied = false;
        private float copyFeedbackTimer = 0;
        private McpClient mcpClient;

        public static void ShowWindow(string configPath, string configJson, McpClient mcpClient)
        {
            var window = GetWindow<ManualConfigEditorWindow>("Manual Configuration");
            window.configPath = configPath;
            window.configJson = configJson;
            window.mcpClient = mcpClient;
            window.minSize = new Vector2(500, 400);
            window.Show();
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            // Header with improved styling
            EditorGUILayout.Space(10);
            Rect titleRect = EditorGUILayout.GetControlRect(false, 30);
            EditorGUI.DrawRect(new Rect(titleRect.x, titleRect.y, titleRect.width, titleRect.height), new Color(0.2f, 0.2f, 0.2f, 0.1f));
            GUI.Label(new Rect(titleRect.x + 10, titleRect.y + 6, titleRect.width - 20, titleRect.height),
                mcpClient.name + " Manual Configuration", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            // Instructions with improved styling
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = EditorGUILayout.GetControlRect(false, 24);
            EditorGUI.DrawRect(new Rect(headerRect.x, headerRect.y, headerRect.width, headerRect.height), new Color(0.1f, 0.1f, 0.1f, 0.2f));
            GUI.Label(new Rect(headerRect.x + 10, headerRect.y, headerRect.width - 20, headerRect.height), "Instructions", EditorStyles.boldLabel);

            GUIStyle instructionStyle = new GUIStyle(EditorStyles.label);
            instructionStyle.wordWrap = true;
            instructionStyle.padding = new RectOffset(15, 0, 0, 0);
            EditorGUILayout.LabelField("To configure " + mcpClient.name + " with the provided JSON configuration, follow these steps:", instructionStyle);

            EditorGUILayout.Space(10);

            // Path section
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.Label(new Rect(headerRect.x + 10, headerRect.y, headerRect.width - 20, headerRect.height), "Path", EditorStyles.boldLabel);
            instructionStyle.padding = new RectOffset(25, 0, 0, 0);
            EditorGUILayout.LabelField("1. Copy the file path below:", instructionStyle);

            // Path field with copy button
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(configPath);
            if (GUILayout.Button("Copy", GUILayout.Width(60)))
            {
                EditorGUIUtility.systemCopyBuffer = configPath;
                pathCopied = true;
                copyFeedbackTimer = 2f;
            }
            if (pathCopied)
            {
                GUIStyle feedbackStyle = new GUIStyle(EditorStyles.label);
                feedbackStyle.normal.textColor = Color.green;
                EditorGUILayout.LabelField("Copied!", feedbackStyle, GUILayout.Width(60));
            }
            EditorGUILayout.EndHorizontal();

            // JSON section with improved styling  
            EditorGUILayout.Space(10);
            GUI.Label(new Rect(headerRect.x + 10, headerRect.y, headerRect.width - 20, headerRect.height), "JSON", EditorStyles.boldLabel);
            instructionStyle.padding = new RectOffset(25, 0, 0, 0);
            EditorGUILayout.LabelField("2. Paste the JSON configuration below:", instructionStyle);

            // Improved text area for JSON with syntax highlighting colors
            GUIStyle jsonStyle = new GUIStyle(EditorStyles.textArea)
            {
                font = EditorStyles.boldFont,
                wordWrap = true
            };
            jsonStyle.normal.textColor = new Color(0.3f, 0.6f, 0.9f); // Syntax highlighting blue
            EditorGUILayout.TextArea(configJson, jsonStyle, GUILayout.Height(200));

            // Copy JSON button with improved styling
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy JSON", GUILayout.Width(100)))
            {
                EditorGUIUtility.systemCopyBuffer = configJson;
                jsonCopied = true;
                copyFeedbackTimer = 2f;
            }
            if (jsonCopied)
            {
                GUIStyle feedbackStyle = new GUIStyle(EditorStyles.label);
                feedbackStyle.normal.textColor = Color.green;
                EditorGUILayout.LabelField("Copied!", feedbackStyle, GUILayout.Width(60));
            }
            EditorGUILayout.EndHorizontal();

            // Save and restart section
            EditorGUILayout.Space(10);
            GUI.Label(new Rect(headerRect.x + 10, headerRect.y, headerRect.width - 20, headerRect.height), "Save & Restart", EditorStyles.boldLabel);
            instructionStyle.padding = new RectOffset(25, 0, 0, 0);
            EditorGUILayout.LabelField("3. Save the file and restart " + mcpClient.name, instructionStyle);

            // Close button at the bottom
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Close"))
            {
                Close();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }

        private void Update()
        {
            // Handle the feedback message timer
            if (copyFeedbackTimer > 0)
            {
                copyFeedbackTimer -= Time.deltaTime;
                if (copyFeedbackTimer <= 0)
                {
                    pathCopied = false;
                    jsonCopied = false;
                    Repaint();
                }
            }
        }
    }
}
