/* The Launcher by Çağlayan Karagözler (a.k.a Flamacore)
 * Script for the actual uploading job.
 * 
 * Uses a combination of FtpWebRequest and WebClient 
 * in order to upload 3 files to the server via FTP.
 * 1. Version file which contains the latest version of the app.
 * 2. File list container file which contains the folder structure of the app.
 * 3. Compressed build.
 * 
 * TODO: SFTP connection option. Better EditorPrefs settings.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Threading;
using System.Net;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

/// <summary>
/// Uploader Class.
/// </summary>
[ExecuteInEditMode]
public static class Uploader
{

    public static string FilePath;
    public static string FTPUserName;
    public static string FTPPassword;
    public static string FTPHost;
    public static WebClient client2 = new WebClient();
    public static WebClient client3 = new WebClient();
    public static string UploadStatus = "";
    public static Thread t1_;
    public static Thread t2_;
    public static bool abortThreadOnNextTry = false;
    public static bool shouldCheck = true;
    #region Listeners

    /// <summary>
    /// Version file upload complete event.
    /// </summary>
    static void OnFileUploadCompleted2(object sender, UploadFileCompletedEventArgs e)
    {
        DebugLevelController.Log("Version Uploaded");
    }

    /// <summary>
    /// File List upload complete event.
    /// </summary>
	static void OnFileUploadCompleted3(object sender, UploadDataCompletedEventArgs e)
    {
        DebugLevelController.Log("File List Uploaded");
    }
    #endregion
    #region Functions
    /// <summary>
    /// Uploads the file.
    /// </summary>
    public static void UploadFile()
    {
        DebugLevelController.Log("Path: " + FilePath,1);
        DebugLevelController.Log("Username: " + FTPUserName,1);
        DebugLevelController.Log("Pass: " + FTPPassword,1);
        DebugLevelController.Log("Host: ftp://" + FTPHost,1);

        //end uploading
        /*t2_ = new Thread(UploadThread);
        if (!t2_.IsAlive)
            t2_.Start();*/
        StaticCoroutine.Start(UploadThread());

        EditorLogGenerator.GenerateLog("Upload started");
        shouldCheck = true;

        Uri uri2 = new Uri("ftp://" + FTPHost + "version.thln");
        client2.Credentials = new System.Net.NetworkCredential(FTPUserName, FTPPassword);
        client2.UploadFileCompleted += new UploadFileCompletedEventHandler(OnFileUploadCompleted2);
        client2.UploadFileAsync(uri2, "STOR", Application.dataPath + "/../TheLauncher/version/version.thln");
        EditorLogGenerator.GenerateLog("Version File Upload Started");

        Uri uri3 = new Uri("ftp://" + FTPHost + "v.thln");
        client3.Credentials = new System.Net.NetworkCredential(FTPUserName, FTPPassword);
        client3.UploadDataCompleted += new UploadDataCompletedEventHandler(OnFileUploadCompleted3);
        byte[] bytes3 = File.ReadAllBytes(Application.dataPath + "/../TheLauncher/version/v.thln");
        client3.UploadDataAsync(uri3, "STOR", bytes3);
        EditorLogGenerator.GenerateLog("File List Upload Started");
    }

    public static IEnumerator UploadThread()
    {
        yield return null;
        t2_ = new Thread(() =>
        {
            DebugLevelController.Log("--Upload Thread Started",1);
            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create("ftp://" + FTPHost + new FileInfo(FilePath).Name);
            ftpClient.Credentials = new System.Net.NetworkCredential(FTPUserName, FTPPassword);
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            //ftpClient.EnableSsl = true;
            ftpClient.UseBinary = true;
            ftpClient.KeepAlive = true;
            DebugLevelController.Log("--FTP Client Created",1);
            System.IO.FileInfo fi = new System.IO.FileInfo(FilePath);
            ftpClient.ContentLength = fi.Length;
            byte[] buffer = new byte[4097];
            int bytes = 0;
            long total_bytes = (long)fi.Length;
            System.IO.FileStream fs = fi.OpenRead();
            System.IO.Stream rs = ftpClient.GetRequestStream();
            long fileLength = new System.IO.FileInfo(FilePath).Length;
            while (total_bytes > 0)
            {
                if (abortThreadOnNextTry)
                {
                    return;
                }
                bytes = fs.Read(buffer, 0, buffer.Length);
                rs.Write(buffer, 0, bytes);
                total_bytes = total_bytes - bytes;
                UploadStatus = "Upload Progress: " + GetBytesReadable(fileLength - total_bytes) + "/" + GetBytesReadable(fileLength);
                //yield return new WaitForEndOfFrame();
            }
            //fs.Flush();
            fs.Close();
            rs.Close();
            FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
            if (total_bytes == 0)
            {
                UploadStatus = "Upload Done!";
            }
            else
            {
                UploadStatus = uploadResponse.StatusDescription;
            }
            uploadResponse.Close();
        });
        if (!t2_.IsAlive)
            t2_.Start();
    }

    /// <summary>
    /// Gets the bytes in a readable format.
    /// </summary>
    public static string GetBytesReadable(long i)
    {
        long absolute_i = (i < 0 ? -i : i);
        string suffix;
        double readable;
        if (absolute_i >= 0x1000000000000000) // Exabyte
        {
            suffix = "EB";
            readable = (i >> 50);
        }
        else if (absolute_i >= 0x4000000000000) // Petabyte
        {
            suffix = "PB";
            readable = (i >> 40);
        }
        else if (absolute_i >= 0x10000000000) // Terabyte
        {
            suffix = "TB";
            readable = (i >> 30);
        }
        else if (absolute_i >= 0x40000000) // Gigabyte
        {
            suffix = "GB";
            readable = (i >> 20);
        }
        else if (absolute_i >= 0x100000) // Megabyte
        {
            suffix = "MB";
            readable = (i >> 10);
        }
        else if (absolute_i >= 0x400) // Kilobyte
        {
            suffix = "KB";
            readable = i;
        }
        else
        {
            return i.ToString("0 B"); // Byte
        }
        readable = (readable / 1024);
        return readable.ToString("0.### ") + suffix;
    }
    #endregion

}
