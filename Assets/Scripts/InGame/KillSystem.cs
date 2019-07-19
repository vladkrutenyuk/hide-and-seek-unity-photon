using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class KillSystem : MonoBehaviour
{
	MatchManager MM;
	InGameCanvas HUD;

	Camera fpsCam;
	Hashtable prop = new Hashtable();

	int maxHealth = 100;
	int knifeDamage = 25;
	float knifeRange = 3f;

	private void Start ()
	{
		fpsCam = GetComponent<Camera>();
		HUD = GameObject.Find("InGameCanvas").GetComponent<InGameCanvas>();
		MM = GameObject.Find("MatchManager").GetComponent<MatchManager>();

		prop["Health"] = maxHealth;
		prop["IsDead"] = false;
		PhotonNetwork.player.SetCustomProperties(prop);
	}
	
	// Test knife-raycast 
	private void Update ()
	{
		if (fpsCam == null)
			return;
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit hit;
			if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, knifeRange))
			{
				print(hit.collider.name + "WAS HIT by Raycast!!!");
				if(hit.collider.tag == "Player")
				{
					PhotonPlayer victimPlayer = hit.collider.gameObject.GetPhotonView().owner;

					DoDamage(victimPlayer, knifeDamage);
				}
			} 
		}	
	}


	void DoDamage(PhotonPlayer victimPlayer, int amount)
	{
		// Update HEALTH of Victim
		int health = (int)victimPlayer.CustomProperties["Health"] - amount;
		prop["Health"] = health;
		victimPlayer.SetCustomProperties(prop);

		if (health <= 0)
			MM.Kill(victimPlayer);
	}

	private void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		// Are these our own changes?
		//PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
		Hashtable updatedProps = playerAndUpdatedProps[1] as Hashtable;

		if (updatedProps.ContainsKey("Health"))
		{
			// Set current own Health on HUD
			int ownHealth = (int)PhotonNetwork.player.CustomProperties["Health"];
			HUD.SetCurrentHealth(ownHealth);
			// Am I dead? (I hope no)
			if (ownHealth <= 0)
				OwnDeath();
		}
		
	}

	private void OwnDeath()
	{
		PhotonNetwork.Destroy(GetComponentInParent<PhotonView>().gameObject);
		MM.Die();
	}

}
