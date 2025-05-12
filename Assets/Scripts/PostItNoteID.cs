using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class PostItNoteID: MonoBehaviour
{
    public int socketID;

    public bool isOrange;

	public PostItNoteLetter letter;
	public enum PostItNoteLetter
	{
		D,
		E,
		L,
		T,
		I,
		O,
		N,
		C,
		G,
	}
}
