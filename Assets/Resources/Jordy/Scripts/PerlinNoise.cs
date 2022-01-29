using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise
{


	public static int Noise1D(int x, int amplitude, int frequency, int mapWidth, long seed)
	{

		float noise = 0f;
		int octaves = (int)Mathf.Log((mapWidth / frequency), 2f);

		for (int currentOctave = 0; currentOctave < octaves; currentOctave++)
		{
			int sampleDistance = (mapWidth / frequency); //How big is chunksize used for assigning a noise value?
			int offSet = Mathf.RoundToInt(sampleDistance / 3);
			int currentChunk = (int)Mathf.Floor(((x + offSet) / sampleDistance)); //In what chunk does x currently reside?
			int chunkIndex = (x + offSet) % sampleDistance;
			float prog = (float)chunkIndex / sampleDistance; //How far into that chunk are we?

			int leftSample = RandomNum(currentOctave, sampleDistance, currentChunk, amplitude, seed);
			int rightSample = RandomNum(currentOctave, sampleDistance, (currentChunk + 1), amplitude, seed);

			//Debug.Log("chunkSize is : " + sampleDistance);
			//Debug.Log("currentChunk is : " + currentChunk);
			//Debug.Log("chunkIndex is : " + chunkIndex);
			//Debug.Log("prog is : " + prog);
			//Debug.Log("lS is : " + leftSample);
			//Debug.Log("rS is : " + rightSample);

			//With the edge values known, we will now interpolate linearly to see the noise at the current x.
			noise += (leftSample + (rightSample - leftSample) * prog);

			frequency *= 2;
			amplitude /= 2;

		}
		return (int)Mathf.Round(noise);

	}
	public static int Noise2D(int x, int y, int amplitude, int frequency, int mapWidth, long seed)
	{
		float noise = 0f;
		int octaves = (int)Mathf.Log((mapWidth / frequency), 2f);
		for (int currentOctave = 0; currentOctave < octaves; currentOctave++)
		{
			int sampleDistance = (mapWidth / frequency);
			int offSet = Mathf.RoundToInt(sampleDistance / 3);
			int currentChunkX = Mathf.FloorToInt((x + offSet)/ sampleDistance);
			int currentChunkY = Mathf.FloorToInt((y +offSet)/ sampleDistance);
			int chunkIndexX = (x + offSet) % sampleDistance;
			int chunkIndexY = (y + offSet) % sampleDistance;
			float progX = (float)chunkIndexX / sampleDistance;
			float progY = (float)chunkIndexY / sampleDistance;

			int bottomLeftSample = RandomNum2D	(currentOctave, sampleDistance, currentChunkX,		currentChunkY,		amplitude, seed);
			int bottomRightSample = RandomNum2D	(currentOctave, sampleDistance, currentChunkX + 1,	currentChunkY,		amplitude, seed);
			int topLeftSample = RandomNum2D		(currentOctave, sampleDistance, currentChunkX,		currentChunkY + 1,	amplitude, seed);
			int topRightSample = RandomNum2D	(currentOctave, sampleDistance, currentChunkX + 1,	currentChunkY + 1,	amplitude, seed);

			float bottomEdge = (bottomLeftSample + (bottomRightSample - bottomLeftSample) * progX);
			float topEdge = (topLeftSample + (topRightSample - topLeftSample) * progX);

			noise += (bottomEdge + (topEdge - bottomEdge) * progY);

			frequency *= 2;
			if (amplitude > 1)
            {
				amplitude /= 2;
			}
			
		}

		return (int)Mathf.Round(noise);
	}
	public static int RandomNum(int octave, int size, int currentChunk, int scale, long seed)
	{
		int value = Mathf.Abs((int)(seed + (octave + size + 13) ^ (currentChunk + 5) * 60943) % scale);
		//Debug.Log(value + "RandomNum");
		return value;
	}
	public static int RandomNum2D(int octave, int size, int currentChunkX, int currentChunkY, int scale, long seed)
	{
		int value = Mathf.Abs((int)(seed + (octave + 300)^3 * (size + 500)^3 + ((currentChunkX + 201) * (currentChunkY + 201)) * 60943) % scale);
		//Debug.Log(value + "RandomNum");
		return value;
	}
}
