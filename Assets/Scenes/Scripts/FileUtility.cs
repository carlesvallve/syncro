using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 using System.IO;
  
 public class FileUtility {

 	public static List<string> GetDirectoryNames (string path) {
 		DirectoryInfo di = new DirectoryInfo(path);
 		DirectoryInfo[] fi = di.GetDirectories();
 		
 		List<string> returnValue = new List<string>();

 		for (int i = 0; i < fi.Length; i += 1) {
 			returnValue.Add(Path.GetFileNameWithoutExtension(fi[i].FullName));
 		}

 		return returnValue;
 	}


 	public static List<string> GetFilesInDirectory (string path) {
 		DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] fi = di.GetFiles();

        List<string> returnValue = new List<string>();

        for (int i = 0; i < fi.Length; i += 2) {
            returnValue.Add(Path.GetFileNameWithoutExtension(fi[i].FullName)); 
        }

        return returnValue;
     }
 }