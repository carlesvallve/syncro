using UnityEngine; // 41 Post - Created by DimasTheDriver on May/17/2012 . Part of the 'Unity: Making a simple audio visualization' post. http://www.41post.com/?p=4776
using System.Collections;
using System.Collections.Generic;

public class AudioVisualizer : MonoBehaviour {
	// audio elements
	private AudioSource aSource;
	public float[] samples = new float[64]; //A float array that stores the audio samples

	// visual elements
	private LineRenderer lRenderer;
	public GameObject cube;
	private Transform goTransform;
	private Vector3 cubePos;

	private Transform[] cubesTransform; //An array that stores the Transforms of all instantiated cubes
	private Vector3 gravity = new Vector3(0.0f,1.0f,0.0f); //The velocity that the cubes will drop

	private float altitude = 60f;


	List<AudioSource> audioSources = new List<AudioSource>();
	

	void Awake () {
		//Get and store a reference to the following attached components: 
		//AudioSource
		//this.aSource = GetComponent<AudioSource>();
		//LineRenderer
		this.lRenderer = GetComponent<LineRenderer>();
		//Transform
		this.goTransform = GetComponent<Transform>();
	}
	
	void Start() {
		//The line should have the same number of points as the number of samples
		lRenderer.SetVertexCount(samples.Length + 1);
		//The cubesTransform array should be initialized with the same length as the samples array
		cubesTransform = new Transform[samples.Length];
		//Center the audio visualization line at the X axis, according to the samples array length
		goTransform.position = new Vector3(-samples.Length / 2, goTransform.position.y, goTransform.position.z);
		
		//Create a temporary GameObject, that will serve as a reference to the most recent cloned cube
		GameObject tempCube;
		
		//For each sample
		for(int i= 0; i< samples.Length; i++) {
			//Instantiate a cube placing it at the right side of the previous one
			tempCube = (GameObject) Instantiate(cube, new Vector3(goTransform.position.x + i * 1, goTransform.position.y, goTransform.position.z),Quaternion.identity);
			//Get the recently instantiated cube Transform component
			cubesTransform[i] = tempCube.GetComponent<Transform>();
			//Make the cube a child of this game object
			cubesTransform[i].parent = goTransform;

			//cubesTransform[i].gameObject.SetActive(false);
		}
	}
	
	void LateUpdate () {
		if (Input.GetMouseButtonUp(0)) {
			setAudioSource();
		}

		if (aSource != null) {
			updateAudioSpectrum();
		}
	}


	void setAudioSource () {
		// get music folder
		string path = "Music/rekkerd music production 2013"; //"Music/rekkerd one sequence"; //"Music/rekkerd music contest moving 2011"; //"Music/rekkerd no-kick pack";

		// get a list of all files inside music folder
		List<string> fileNames = FileUtility.GetFilesInDirectory("Assets/Resources/" + path);
		for (int i = 0; i < fileNames.Count; i++) {
			print (i + "/" + fileNames.Count + " ---> " + fileNames[i]);
		}

		// get a random music file from existing ones
		int n = Random.Range(0, fileNames.Count);
		string fileName = path + "/" + fileNames[n];
		print (n + " = " + fileName);

		// play the audio source and get a reference to it
		int sample = 0;
		if (aSource != null) {
			sample = aSource.timeSamples;

			for (int i = 0; i < audioSources.Count; i++) {
				AudioSource audioSource = audioSources[i];
				int r = Random.Range(1, 100);
				if (r <= 60) {
					audioSources.Remove(audioSource);
					audioSource.Stop();
					Destroy(audioSource);
				}
			}
			
		}
		
		aSource = Audio.playAtSample(fileName, sample, true);
		audioSources.Add(aSource);
	}


	void updateAudioSpectrum () {

		//Obtain the samples from the frequency bands of the attached AudioSource
		aSource.GetSpectrumData(this.samples,0,FFTWindow.BlackmanHarris);
		
		//For each sample
		for(int i = 0; i < samples.Length; i++) {
			// Set the cubePos Vector3 to the same value as the position of the corresponding cube. 
			// However, set it's Y element according to the current sample.
			cubePos.Set(cubesTransform[i].position.x, Mathf.Clamp(samples[i] * (altitude + i * i), -altitude, altitude), cubesTransform[i].position.z);
			
			//If the new cubePos.y is greater than the current cube position
			if(cubePos.y >= cubesTransform[i].position.y) {
				//Set the cube to the new Y position
				cubesTransform[i].position = cubePos;
			} else {
				//The spectrum line is below the cube, make it fall
				cubesTransform[i].position -= gravity;
			}
			
			/*Set the position of each vertex of the line based on the cube position.
			 * Since this method only takes absolute World space positions, it has 
			 * been subtracted by the current game object position.*/
			lRenderer.SetPosition(i + 1, cubePos - goTransform.position);
		}


		
	}
}
