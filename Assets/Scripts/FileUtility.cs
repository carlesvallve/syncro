using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 using System.IO;
  
 public class FileUtility {

 	public static List<string> GetFilesInDirectory(string path) {
 		DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] fi = di.GetFiles();

        List<string> returnValue = new List<string>();

        Debug.Log (fi.Length);

        for (int i = 0; i < fi.Length; i+= 2) {
            returnValue.Add(Path.GetFileNameWithoutExtension(fi[i].FullName)); 
        }

        return returnValue;
     }
 }