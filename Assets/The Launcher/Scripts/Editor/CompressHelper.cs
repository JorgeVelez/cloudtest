/* The Launcher by Çağlayan Karagözler (a.k.a Flamacore)
 * A Helper Script for compressing the build after the developer selects the build folder. Used by Compress.cs
 * 
 * A combination of GZipStream and FileStream classes are used for the job.
 * Probably you will not need to tamper with this :)
 * Creates a compressed file with a custom extension. 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.AccessControl;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CompressHelper : MonoBehaviour
{
    public static string DecompressStatus;
    static GZipStream strm;
    public static Thread t3_;
    private static bool lastDone;
    public static bool abortThreadOnNextTry = false;
    /// <summary>
    /// Compresses the file.
    /// </summary>
    public static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
    {
        //Compress file name
        DebugLevelController.Log("Compressing File Name: " + sRelativePath,0);
        char[] chars = sRelativePath.ToCharArray();
        zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
        Compress.compressStatus = "File: " + sRelativePath;
        foreach (char c in chars)
        {
            zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));
        }
        //Compress file content
        /* byte[] bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
        zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
        zipStream.Write(bytes, 0, bytes.Length);
        strm = zipStream;*/

        FileStream fsSource = new FileStream(Path.Combine(sDir, sRelativePath), FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fsSource.Length];
        DebugLevelController.Log("Reading Bytes:" + bytes.Length + " File:" + sRelativePath,2);
        int numBytesToRead = (int)fsSource.Length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
            int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
            if (n == 0)
                break;

            numBytesRead += n;
            numBytesToRead -= n;
            DebugLevelController.Log("Reading Bytes:" + bytes.Length + " File:" + sRelativePath,2);
        }
        numBytesToRead = bytes.Length;

        zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
        zipStream.Write(bytes, 0, numBytesToRead);
        strm = zipStream;

    }

    public static void CompressDirectory(string sInDir, string sOutFile)
    {
        StaticCoroutine.Start(CompressRoutine(sInDir, sOutFile));
    }

    public static IEnumerator CompressRoutine(string sInDir, string sOutFile)
    {
        lastDone = false;
        Compress.compressCount = 0;
        Compress.compressStatus = "Getting Files";
        string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
        int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;
        Compress.totalFiles = sFiles.Length;
        yield return null;
        t3_ = new Thread(() =>
        {
            FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            GZipStream str = new GZipStream(outFile, CompressionMode.Compress);
            abortThreadOnNextTry = false;
            foreach (string sFilePath in sFiles)
            {
                lastDone = false;
                Compress.compressCount++;
                string sRelativePath = sFilePath.Substring(iDirLen);
                Compress.compressStatus = "Compressing..";
                //Compress file name
                DebugLevelController.Log("Compressing File Name: " + sRelativePath);
                char[] chars = sRelativePath.ToCharArray();
                str.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
                Compress.compressStatus = "File: " + sRelativePath;
                foreach (char c in chars)
                {
                    str.Write(BitConverter.GetBytes(c), 0, sizeof(char));
                }
                //Compress file content
                DebugLevelController.Log("Compressing File Content: " + sRelativePath,1);
                int bufferSize = 500 * 1024; //* 1024; // 100MB
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;
                long DoneBytes = 0;
                using (FileStream fsSource = new FileStream(Path.Combine(sInDir, sRelativePath), FileMode.Open, FileAccess.Read))
                {
                    DebugLevelController.Log("File Stream Created: " + fsSource.Name,1);
                    while ((bytesRead = fsSource.Read(buffer, 0, bufferSize)) > 0)
                    {
                        if (abortThreadOnNextTry)
                        {
                            return;
                        }
                        DoneBytes += bufferSize;
                        DebugLevelController.Log("Reading Bytes:" + (DoneBytes) + "/" + fsSource.Length + " File:" + sRelativePath,2);
                        Compress.compressStatus = ((double)(((DoneBytes) / fsSource.Length) * 100)).ToString() + "%" + " File:" + sRelativePath;
                        str.Write(BitConverter.GetBytes(buffer.Length), 0, sizeof(int));
                        str.Write(buffer, 0, buffer.Length);
                    }
                }
                strm = str;
            }
            lastDone = true;
            if (abortThreadOnNextTry)
            {
                Compress.compressStatus = "Compression Aborted!";
                DebugLevelController.Log("Compression Aborted At:" + DateTime.Now.ToShortTimeString());
                strm.Close();
            }
            else
            {
                Compress.compressStatus = "Compression Done!";
                DebugLevelController.Log("Compression Done At:" + DateTime.Now.ToShortTimeString());
                strm.Close();
            }
        });
        if (!t3_.IsAlive)
            t3_.Start();
        /*while (!lastDone)
            yield return null;*/
        yield return null;
        EditorLogGenerator.GenerateLog("Compression Done At:" + DateTime.Now.ToShortTimeString());
        yield return null;
    }

    private void OnApplicationQuit()
    {
        t3_.Abort();
    }


}