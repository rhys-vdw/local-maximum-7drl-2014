using UnityEngine;

[System.Serializable]
public class Range
{
	public float Min, Max;

	public Range( float min, float max )
	{
		Min = min;
		Max = max;
	}

	public float Length
	{
		get { return Max - Min; }
	}

	public float Clamp( float value )
	{
		return Mathf.Clamp( value, Min, Max );
	}

	public float Random()
	{
		return UnityEngine.Random.Range( Min, Max );
	}

	public float Lerp( float t )
	{
		return Mathf.Lerp( Min, Max, t );
	}

	public bool Contains( float value )
	{
		return value >= Min && value <= Max;
	}

	public override string ToString()
	{
		return string.Format( "Range: [{0}, {1}]", Min, Max );
	}
}

[System.Serializable]
public class IntRange
{
	public int Min, Max;

	public IntRange( int min, int max )
	{
		Min = min;
		Max = max;
	}

	public float Size
	{
		get { return Max - Min; }
	}

	public int RandomValue()
	{
		return UnityEngine.Random.Range( Min, Max + 1 );
	}

	public bool Contains( int value )
	{
		return value >= Min && value <= Max;
	}
} 
