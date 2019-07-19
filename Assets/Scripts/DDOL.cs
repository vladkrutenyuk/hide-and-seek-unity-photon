using UnityEngine;

public class DDOL : MonoBehaviour
{	
	private void Awake ()
	{
		DontDestroyOnLoad(this);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}
}
