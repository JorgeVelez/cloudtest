/* The Launcher by Çağlayan Karagözler (a.k.a Flamacore)
 * The script used for decompressing the downloaded file which is compressed by the uploader.
 * 
 * A combination of GZipStream and FileStream classes are used for the job.
 * Probably you will not need to tamper with this :)
 * It will decompress the file compressed by Compress.cs script only.
 * 
 * TODO: Better tracking for decompression status. Size based particularly.
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

/// <summary>
/// Decompressor.
/// </summary>
public class Decompressor : MonoBehaviour
{
    static bool lastDone = false;
    static string sCompressedFile2;
    /// <summary>
    /// Decompresses the file.
    /// </summary>
    /// <returns><c>true</c>, if file was decompressed, <c>false</c> otherwise.</returns>
    public static bool DecompressFile(string sDir, GZipStream zipStream)
    {
        Debug.Log("decompress started");
        //Decompress file name
        byte[] bytes = new byte[sizeof(int)];
        int Readed = zipStream.Read(bytes, 0, sizeof(int));
        Debug.Log("decompress started2");
        if (Readed < sizeof(int))
        {
            /*Download.DownloadProgress = LocalizationManager.LangStrings[6];
            Download.DownloadStatus = LocalizationManager.LangStrings[12];*/
            Debug.Log("Compression finished");
            try { File.Delete(sCompressedFile2); } catch (Exception e) { Debug.Log(e.ToString()); }
            return false;
        }
        Debug.Log("decompress started3");

        int iNameLen = BitConverter.ToInt32(bytes, 0);
        //	Debug.Log (iNameLen);
        bytes = new byte[sizeof(char)];
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < iNameLen; i++)
        {
            zipStream.Read(bytes, 0, sizeof(char));
            char c = BitConverter.ToChar(bytes, 0);
            sb.Append(c);
            //	Debug.Log (sb.ToString() + i);
        }
        string sFileName = sb.ToString();
        Download.DownloadStatus = LocalizationManager.LangStrings[7] + sFileName;
        //Decompress file content
        bytes = new byte[sizeof(int)];
        zipStream.Read(bytes, 0, sizeof(int));
        int iFileLen = BitConverter.ToInt32(bytes, 0);

        bytes = new byte[iFileLen];
        zipStream.Read(bytes, 0, bytes.Length);

        string sFilePath = Path.Combine(sDir, sFileName);
        string sFinalDir = Path.GetDirectoryName(sFilePath);
        Debug.Log(sFileName);
        Debug.Log(sFilePath);
        Debug.Log(sFinalDir);
        if (!Directory.Exists(sFinalDir))
        {
            Directory.CreateDirectory(sFinalDir);
            Debug.Log("Directory Created");
        }

        FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
        outFile.Write(bytes, 0, iFileLen);
        outFile.Close();
        Download.DownloadStatus = LocalizationManager.LangStrings[7] + sFileName + " / Done.";
        Download.DecompedFiles++;
        Download.DownloadProgress = "Decompressed: " + Download.DecompedFiles.ToString();
        Debug.Log("decompress started4");
        return true;
    }

    /// <summary>
    /// Decompresses to directory.
    /// </summary>
    public static IEnumerator DecompressToDirectory(string sCompressedFile, string sDir)
    {
        lastDone = false;
        sCompressedFile2 = sCompressedFile;
        Debug.Log("decompress  " + sCompressedFile + "  " + sDir);
        FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None);
        GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true);
        while (DecompressFile(sDir, zipStream))
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        lastDone = true;
        zipStream.Flush();
        zipStream.Close();
        inFile.Flush();
        inFile.Close();
        while (!lastDone)
            yield return null;
        yield return new WaitForSeconds(4f);
        Download.DownloadProgress = LocalizationManager.LangStrings[6];
        Download.DownloadStatus = LocalizationManager.LangStrings[12];
        File.Delete(sCompressedFile);
    }
}
