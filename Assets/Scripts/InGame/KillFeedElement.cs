using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedElement : MonoBehaviour
{
	public Text murder, victim;

	private void Start()
	{
		Destroy(gameObject, 3f);
	}

	public void NamesColors(string murd, string vict)
	{
		murder.text = murd;
		victim.text = vict;
	}
	
}
