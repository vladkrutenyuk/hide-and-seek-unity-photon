using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TeamDeathmatch : MonoBehaviour
{
	Hashtable prop = new Hashtable();
	MatchManager MM;
	InGameCanvas HUD;

	
	public float RespTime = 3f;

	private void Start()
	{
		HUD = GameObject.Find("InGameCanvas").GetComponent<InGameCanvas>();
		MM = GetComponent<MatchManager>();
	}

	public void Respawn()
	{
		//Start Timer
		// When time is over: Spawn

	}
	// respawn system
	// win target
}
