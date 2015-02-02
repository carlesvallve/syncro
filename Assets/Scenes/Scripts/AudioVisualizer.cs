using UnityEngine; // 41 Post - Created by DimasTheDriver on May/17/2012 . Part of the 'Unity: Making a simple audio visualization' post. http://www.41post.com/?p=4776
using System.Collections;
using System.Collections.Generic;

public class AudioVisualizer : MonoBehaviour {

	List<AudioSource> audioSources = new List<AudioSource>();
	List<AudioVisualTrack> audioTracks = new List<AudioVisualTrack>();
	public GameObject audioTrack;
	
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			setAudioTrack();
		}


		if (Input.GetMouseButtonUp(1)) {
			removeAudioTrack(Random.Range(0, audioSources.Count));
		}
	}


	private string getRandomFileName () {
		// get music folder
		List<string> folders = FileUtility.GetDirectoryNames("Assets/Resources/Music/");
		/*for (int i = 0; i < folders.Count; i++) {
			print (i + "/" + folders.Count + " ---> " + folders[i]);
		}*/



		string path = "Music/" + folders[Random.Range(0, folders.Count)]; //"Music/rekkerd free loops 06"; //"Music/rekkerd music contest moving 2011"; //"Music/rekkerd music production 2013"; //"Music/rekkerd one sequence"; //"Music/rekkerd music contest moving 2011"; //"Music/rekkerd no-kick pack";
		//string path = "Music/rekkerd free loops 03";
		string fileName = null;

		

		// get a list of all files inside music folder
		List<string> fileNames = FileUtility.GetFilesInDirectory("Assets/Resources/" + path);
		/*for (int i = 0; i < fileNames.Count; i++) {
			print (i + "/" + fileNames.Count + " ---> " + fileNames[i]);
		}*/

		bool ok = false;
		while (ok == false) {
			// get a random music file from existing ones
			int n = Random.Range(0, fileNames.Count);
			fileName = path + "/" + fileNames[n];
			print (n + " = " + fileName);

			// make sure that this file is not already playing
			ok = true;
			for (int i = 0; i < audioTracks.Count; i++) {
				AudioVisualTrack audioTrack = audioTracks[i];
				if (fileName == audioTrack.aSource.clip.name) {
					ok = false;
				}
			}
		}

		return fileName;
	}

	
	private void setAudioTrack () {
		// get random loop to play
		string fileName = getRandomFileName();

		// get sample time of first track
		int sample = audioSources.Count > 0 ? audioSources[0].timeSamples : 0;

		// consider if we should remove a soundtrack
		/*if (audioSources.Count > 1) {
			for (int i = 0; i < audioSources.Count; i++) {
				int r = Random.Range(1, 100);
				if (r <= 60) { removeAudioTrack(i); }
			}
		}*/
		
		// play the audio source and get a reference to it
		AudioSource aSource = Audio.playAtSample(fileName, sample, true);
		audioSources.Add(aSource);

		// Instantiate track gameobject
		GameObject obj = (GameObject) Instantiate(audioTrack, Vector3.zero, Quaternion.identity);
		obj.transform.parent = transform;

		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		
		AudioVisualTrack track = obj.GetComponent<AudioVisualTrack>();
		track.init(aSource, color);

		audioTracks.Add(track);
	}


	private void removeAudioTrack (int num) {
		AudioVisualTrack aTrack = audioTracks[num];
		audioTracks.Remove(aTrack);
		Destroy(aTrack.gameObject);

		AudioSource audioSource = audioSources[num];
		audioSources.Remove(audioSource);
		audioSource.Stop();
		Destroy(audioSource);
	}


	
}
