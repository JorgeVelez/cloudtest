/* The Launcher by Çağlayan Karagözler (a.k.a Flamacore)
 * Script for the Uplaoder editor window.
 * 
 * This script creates the uploader editor window in order to
 * select the build, compress and upload it to a server via FTP connection.
 * Handles all the gui.
 * 
 * TODO: SFTP connection option. Better EditorPrefs settings.
 * 
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// The launcher. This is the class for the window. Also contains the functions the window calls.
/// </summary>
[ExecuteInEditMode]
public class TheLauncher : EditorWindow
{

    #region VARS
    static string FTPHost;
    static string FTPUserName;
    static string FTPPassword;
    static string FilePath;
    static TheLauncher window;
    static Texture2D tex;
    static Texture2D tex2;
    static bool showSettings = false;
    public GUISkin guiSkin;
    string uploadStatus;
    GUIStyle boxback = new GUIStyle();
    GUIStyle titleStyle = new GUIStyle();
    GUIStyle smallButtonStyle = new GUIStyle();
    #endregion

    #region GUI Stuff
    [MenuItem("Window/The Launcher")]
    static void Init()
    {
        FTPHost = EditorPrefs.GetString("host");
        FTPUserName = EditorPrefs.GetString("user");
        FTPPassword = EditorPrefs.GetString("pass");
        Compress.LastVersion = EditorPrefs.GetString("ver");
        FilePath = EditorPrefs.GetString("FilePath");
        window = (TheLauncher)EditorWindow.GetWindow(typeof(TheLauncher));
        window.Show();
    }

    private void OnEnable()
    {
        EditorApplication.update += this.Update;
    }

    void OnGUI()
    {
        window = (TheLauncher)EditorWindow.GetWindow(typeof(TheLauncher), false, null, false);
        GUI.skin = guiSkin;
        tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.SetPixel(0, 0, new Color(0f, 0.41176f, 0.53333f));
        tex.Apply();
        boxback.normal.background = tex;
        titleStyle.fontSize = 20;
        titleStyle.normal.textColor = Color.white;
        smallButtonStyle.normal.background = tex;
        smallButtonStyle.padding = new RectOffset(5, 5, 5, 5);
        smallButtonStyle.normal.textColor = Color.white;
        smallButtonStyle.margin = new RectOffset(5, 5, 5, 5);
        smallButtonStyle.alignment = TextAnchor.MiddleCenter;
        tex2 = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex2.SetPixel(0, 0, new Color(0f, 0.71176f, 0.83333f));
        tex2.Apply();
        smallButtonStyle.hover.background = tex2;

        //Background 1
        tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.SetPixel(0, 0, new Color(0f, 0.20392f, 0.25882f));
        tex.Apply();
        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), tex, ScaleMode.StretchToFill);
        //End Background

        //Horizontal Box: Set version
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Set Version (as string): ");
        Compress.LastVersion = GUILayout.TextField(Compress.LastVersion);
        if (GUILayout.Button("Settings", smallButtonStyle))
        {
            showSettings = true;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Horizontal Box

        //Horizontal Box: Settings
        if (showSettings)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Settings", titleStyle);
            if (GUILayout.Button("Close Settings", smallButtonStyle))
            {
                showSettings = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Set No Log", smallButtonStyle))
            {
                DebugLevelController.DebugLogLevel = -1;
                Debug.Log("No Log");
            }
            if (GUILayout.Button("Set Log Level To: User", smallButtonStyle))
            {
                DebugLevelController.DebugLogLevel = 0;
                Debug.Log("User Level Log");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Set Log Level To: Developer", smallButtonStyle))
            {
                DebugLevelController.DebugLogLevel = 1;
                Debug.Log("Developer Level Log");
            }
            if (GUILayout.Button("Set Log Level To: Hacker", smallButtonStyle))
            {
                DebugLevelController.DebugLogLevel = 2;
                Debug.Log("Hacker Level Log");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        //Horizontal Box

        //Horizontal Box: Select Build
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("\nSelect your build", titleStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Horizontal Box

        //Horizontal Box: Subtitle
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Must contain only build files and nothing else");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Horizontal Box

        if (GUILayout.Button("Select Build Folder", guiSkin.button))
        {
            SelectBuildFolder();
        }

        //Horizontal Box: Compress and upload
        GUILayout.BeginHorizontal();
        GUILayout.Box("Folder to compress: " + Compress.pathToCompress);
        GUILayout.EndHorizontal();

        if (!String.IsNullOrEmpty(Compress.pathToCompress))
        {
            if (String.IsNullOrEmpty(Compress.compressStatus))
            {
                if (GUILayout.Button("Compress"))
                {
                    bool should = EditorUtility.DisplayDialog(
                                     "Compress folder",
                                     "Please be aware that if you start the copmression and do some changes to your scripts and recompile, the thread will halt and unity may become unresponsive. \n" +
                                     "\n The folder |  " + Compress.pathToCompress + "  | is about to be compressed and put in Project Folder/The Launcher/. " +
                                     "\nDo you want to continue?",
                                     "Yes, compress!",
                                     "No, go back"
                                 );
                    if (should)
                    {
                        CompressBuild();
                        DebugLevelController.Log("Compression Started");
                        //DebugLevelController.Log ("Compression Status:" + Compress.compressStatus);
                        FilePath = Compress.pathToUpload;
                        EditorLogGenerator.GenerateLog("Compression Started");
                        Uploader.UploadStatus = null;
                    }
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Box("Compression Status (" + Compress.compressCount + "/" + Compress.totalFiles + ");" + Compress.compressStatus);
            GUILayout.EndHorizontal();
        }

        if (!String.IsNullOrEmpty(Compress.compressStatus))
        {
            if (GUILayout.Button("Stop Compression Thread"))
            {
                bool should = EditorUtility.DisplayDialog(
                                 "Compress folder",
                                 "The Launcher will attempt to stop the compression thread right now." +
                                 "\nDo you want to continue?",
                                 "Yes, stop!",
                                 "No, go back"
                             );
                if (should)
                {
                    CompressHelper.abortThreadOnNextTry = true;
                    DebugLevelController.Log("Compression Stopped");
                    FilePath = Compress.pathToUpload;
                    EditorLogGenerator.GenerateLog("Compression Stopped");
                    Uploader.UploadStatus = null;
                    Compress.compressStatus = null;
                }
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("\n---------------------------------------\n");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        //Vertical Box: FTP seciton
        GUILayout.BeginVertical(boxback);
        //Horizontal Box: FTP Upload
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("\nUpload your build\n", titleStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //Horizontal Box


        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("FTP Host:");
        FTPHost = GUILayout.TextField(FTPHost);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("FTP Username:");
        FTPUserName = GUILayout.TextField(FTPUserName);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("FTP Password:");
        FTPPassword = GUILayout.TextField(FTPPassword);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("Do not include the 'ftp://' part and should include the folder you wish to upload your game. \nEg. test.com/mygame/download/");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (!String.IsNullOrEmpty(FilePath))
        {
            GUILayout.BeginHorizontal();
            if (String.IsNullOrEmpty(Uploader.UploadStatus))
            {
                if (GUILayout.Button("UPLOAD"))
                {
                    bool should = EditorUtility.DisplayDialog(
                                     "Upload File",
                                     FilePath + "  | is about to be uploaded to '" +
                                     "\n" + FTPHost
                                     + "\n with username: " + FTPUserName
                                     + "\n with password: " + FTPPassword,
                                     "Yes, UPLOAD!",
                                     "No, go back"
                                 );
                    if (should)
                    {
                        Uploader.FTPHost = FTPHost;
                        Uploader.FTPUserName = FTPUserName;
                        Uploader.FTPPassword = FTPPassword;
                        Uploader.FilePath = FilePath;
                        Uploader.UploadFile();
                        DebugLevelController.Log("Upload Started");
                        EditorLogGenerator.GenerateLog("Upload Started");
                        should = false;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        //Vertical Box

        //Horizontal Box: Infos
        if (!String.IsNullOrEmpty(FilePath))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("File to upload: " + FilePath);
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        //DebugLevelController.Log (Uploader.client.IsBusy);
        GUILayout.Box(Uploader.UploadStatus);
        GUILayout.EndHorizontal();
        //Horizontal Box
        GUILayout.EndVertical();
        if (!String.IsNullOrEmpty(Uploader.UploadStatus))
        {
            if (Uploader.UploadStatus.Contains("Upload Done"))
            {
                Uploader.UploadStatus = "";
                this.Close();
                EditorUtility.DisplayDialog(
                    "Upload Done!",
                    "File was uploaded to the server. Now the window will be closed.",
                    "OK"
                );
            }
        }
        if (!String.IsNullOrEmpty(Uploader.UploadStatus))
        {
            if (Uploader.UploadStatus.Contains("Upload Progress"))
            {
                if (GUILayout.Button("Cancel Upload"))
                {
                    Uploader.abortThreadOnNextTry = true;
                    Uploader.shouldCheck = false;
                    Uploader.UploadStatus = null;
                    DebugLevelController.Log("Upload Cancelled");
                    EditorLogGenerator.GenerateLog("Upload Cancelled");
                }
            }
        }

    }
    void Update()
    {
        if (!EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            window.Repaint();
            EditorUtility.SetDirty(GameObject.Find("StaticCoroutine"));
            EditorUtility.SetDirty(window);
        }
    }
    void OnDisable()
    {
        EditorApplication.update = this.Update;
        EditorPrefs.SetString("host", FTPHost);
        EditorPrefs.SetString("user", FTPUserName);
        EditorPrefs.SetString("pass", FTPPassword);
        EditorPrefs.SetString("ver", Compress.LastVersion);
        EditorPrefs.SetString("FilePath", FilePath);
    }
    #endregion

    #region functions
    /// <summary>
    /// Selects the build folder.
    /// </summary>
    static void SelectBuildFolder()
    {
        Compress.pathToCompress = null;
        Compress.compressStatus = null;
        string path = EditorUtility.OpenFolderPanel("Select Build Folder (Must only contain files created by unity)", "", "");
        if (path.Length != 0)
        {
            EditorLogGenerator.GenerateLog("Selected Folder:" + path);
            Compress.pathToCompress = path;
        }
    }
    static void CompressBuild()
    {
        Compress.CompressMe();
    }

    /// <summary>
    /// Decompresses the build. Unused at the moment...
    /// </summary>
    static void DecompressBuild()
    {
        string path = EditorUtility.OpenFilePanel("Select zipped file", "", ".thln");
        if (path.Length != 0)
        {
            Compress.CompressedFile = path;
            //	Compress.DecompressMe ();
        }
    }

    void OnDestroy()
    {
        CompressHelper.t3_.Abort();
    }
    #endregion

}