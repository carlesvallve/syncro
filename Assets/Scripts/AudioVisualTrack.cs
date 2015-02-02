using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioVisualTrack : MonoBehaviour {

	public bool displayLine = true;
	public bool displayCubes = true;

	// audio elements
	public AudioSource aSource;
	public float[] samples = new float[64]; //A float array that stores the audio samples

	// visual elements
	private LineRenderer lRenderer;
	public GameObject cube;
	private Transform goTransform;
	private Vector3 cubePos;

	private Transform[] cubesTransform; //An array that stores the Transforms of all instantiated cubes
	private Vector3 gravity = new Vector3(0.0f,1.0f,0.0f); //The velocity that the cubes will drop

	private float altitude = 50f;


	public void init (AudioSource aSource, Color color) {
		this.aSource = aSource;
		this.lRenderer = GetComponent<LineRenderer>();
		this.goTransform = GetComponent<Transform>();

		// set color
		lRenderer.renderer.material.color = color;

		//set line and cube transforms at the same length as the samples array
		lRenderer.SetVertexCount(samples.Length - 0);
		cubesTransform = new Transform[samples.Length];
		goTransform.localPosition = new Vector3(0, goTransform.position.y, goTransform.position.z);
		
		// instantiate cubes
		for(int i= 0; i< samples.Length; i++) {
			GameObject tempCube = (GameObject) Instantiate(cube, new Vector3(goTransform.position.x + i * 1, goTransform.position.y, goTransform.position.z),Quaternion.identity);
			cubesTransform[i] = tempCube.GetComponent<Transform>();
			cubesTransform[i].parent = goTransform;
			tempCube.renderer.material.color = new Color(color.r * 1.2f, color.g * 1.2f, color.b * 1.2f);
		}
	}


	void Update () {
		if (aSource != null) {
			updateAudioSpectrum();
		}
	}


	void updateAudioSpectrum () {
		//Obtain the samples from the frequency bands of the attached AudioSource
		aSource.GetSpectrumData(this.samples,0,FFTWindow.BlackmanHarris);
		
		//For each sample
		for(int i = 0; i < samples.Length; i++) {
			// Set the cubePos Vector3 to the same value as the position of the corresponding cube. 
			// However, set it's Y element according to the current sample.
			cubePos.Set(cubesTransform[i].localPosition.x, Mathf.Clamp(samples[i] * (altitude + i * i), -altitude, altitude), cubesTransform[i].localPosition.z);
			
			//If the new cubePos.y is greater than the current cube position
			if(cubePos.y >= cubesTransform[i].localPosition.y) {
				//Set the cube to the new Y position
				cubesTransform[i].localPosition = cubePos;
			} else {
				//The spectrum line is below the cube, make it fall
				cubesTransform[i].localPosition -= gravity;
				if (cubesTransform[i].localPosition.y <= 0) {
					cubesTransform[i].localPosition = new Vector3 (
						cubesTransform[i].localPosition.x, 0, cubesTransform[i].localPosition.z
					);
				}
			} 

			cubesTransform[i].gameObject.SetActive(displayCubes);
			
			/*Set the position of each vertex of the line based on the cube position.
			 * Since this method only takes absolute World space positions, it has 
			 * been subtracted by the current game object position.*/
			 if (displayLine) {
				lRenderer.SetPosition(i, cubePos - goTransform.localPosition);
			} else {
				lRenderer.SetPosition(i, Vector3.zero);
			}
		}
	}
}
