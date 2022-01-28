using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise{


	public static int Noise (int x, int amplitude, int mapWidth, long seed){

		int frequency = 8; //This value represents the amount of noise sample points that are taken across the width of the map.
		float noise = 0f;
		amplitude = amplitude * (mapWidth / 10);
		int octaves = (int)Mathf.Log((mapWidth / frequency), 2f);
		
		for (int currentOctave = 0; currentOctave < octaves; currentOctave++)
		{
			int sampleDistance = (mapWidth / frequency); //How big is chunksize used for assigning a noise value?
			int currentChunk = (int)Mathf.Floor(x / sampleDistance); //In what chunk does x currently reside?
			int chunkIndex = x % sampleDistance;
			float prog = (float)chunkIndex / sampleDistance; //How far into that chunk are we?

			int leftSample = RandomNum(currentOctave, sampleDistance, currentChunk, amplitude, seed);
			int rightSample = RandomNum(currentOctave, sampleDistance, (currentChunk + 1), amplitude, seed);

			Debug.Log("chunkSize is : " + sampleDistance);
			Debug.Log("currentChunk is : " + currentChunk);
			Debug.Log("chunkIndex is : " + chunkIndex);
			//Debug.Log("prog is : " + prog);
			//Debug.Log("lS is : " + leftSample);
			//Debug.Log("rS is : " + rightSample);

			//With the edge values know, we will now interpolate linearly to see the noise at the current x.
			noise += (leftSample + (rightSample - leftSample) * prog);

			frequency *= 2;
			amplitude /= 2;
		}
		return (int)Mathf.Round (noise);
		
	}
	public static int RandomNum (int octave, int size, int currentChunk, int scale, long seed){
		int value = Mathf.Abs((int)((seed + octave + currentChunk + size) * 7) % scale);
		Debug.Log(value + "RandomNum");
		return value;
	}
}
