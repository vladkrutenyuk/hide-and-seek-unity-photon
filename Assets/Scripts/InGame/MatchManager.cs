using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MatchManager : MonoBehaviour
{
	PhotonView PhV;
	InGameCanvas HUD;

	Hashtable prop = new Hashtable();

	string mode;

	int p = 0;
	public bool gameOver = false;

	private void Start()
	{
		PhV = gameObject.AddComponent<PhotonView>();
		PhV.viewID = 2;
		HUD = GameObject.Find("InGameCanvas").GetComponent<InGameCanvas>();
		 
		mode = (string)PhotonNetwork.room.CustomProperties["Mode"];
		if (mode == "TDM")
			gameObject.AddComponent<TeamDeathmatch>();

		PhV.RPC("HaveBeenLoaded", PhotonTargets.AllBufferedViaServer);
	}

	public void Spawn()
	{
		GameObject.Find("SceneCamera").GetComponent<Camera>().enabled = false;
		HUD.menuDeath.SetActive(false);
		HUD.menuAlive.SetActive(true);

		Vector3 spawnPoint = new Vector3(Random.Range(0, 8), 0, Random.Range(0, 8));
		PhotonNetwork.Instantiate("mainPlayer", spawnPoint, Quaternion.identity, 0);
	}

	public void Kill(PhotonPlayer victimPlayer)
	{
		// Show on HUD for ALL Players
		PhV.RPC("RPC_AddKlfd", PhotonTargets.All, PhotonNetwork.player.NickName, victimPlayer.NickName);

		// Murder: Kills + 1
		prop["Kills"] = (int)victimPlayer.CustomProperties["Kills"] + 1;
		PhotonNetwork.player.SetCustomProperties(prop);

		// +1 for team Score in TDM
		if (mode == "TDM")
			PlusMinusScore(PhotonNetwork.player.GetTeam(), true);
	}

	[PunRPC]
	private void RPC_AddKlfd(string murder, string victim)
	{
		HUD.CallKillFeedMsg(murder, victim);
	}

	public void Die()
	{
		//spawnMenu.SetActive(true);
		GameObject.Find("SceneCamera").GetComponent<Camera>().enabled = true;
		HUD.menuAlive.SetActive(false);
		HUD.menuDeath.SetActive(true);
		
		// Your player is dead: +1 Death 
		prop["IsDead"] = true;
		prop["Deaths"] = (int)PhotonNetwork.player.CustomProperties["Deaths"] + 1;
		PhotonNetwork.player.SetCustomProperties(prop);

		StartCoroutine(resp());
	}

	IEnumerator resp()
	{
		yield return new WaitForSeconds(3f);
		Spawn();
		
	}

	public void PlusMinusScore(PunTeams.Team team, bool plus)
	{
		if (gameOver == true)
			return;

		if (plus)
		{
			if (team == PunTeams.Team.blue)
				prop["Blue"] = (int)PhotonNetwork.room.CustomProperties["Blue"] + 1;
			else if (team == PunTeams.Team.red)
				prop["Red"] = (int)PhotonNetwork.room.CustomProperties["Red"] + 1;
		}
		else
		{
			if (team == PunTeams.Team.blue)
				prop["Blue"] = (int)PhotonNetwork.room.CustomProperties["Blue"] - 1;
			else if (team == PunTeams.Team.red)
				prop["Red"] = (int)PhotonNetwork.room.CustomProperties["Red"] - 1;
		}
		PhotonNetwork.room.SetCustomProperties(prop);
	}

	private void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("Blue") || propertiesThatChanged.ContainsKey("Red"))
		{
			HUD.SetCurrentTeamScore((int)PhotonNetwork.room.CustomProperties["Red"], (int)PhotonNetwork.room.CustomProperties["Blue"]);

			if ((int)PhotonNetwork.room.CustomProperties["Red"] >= (int)PhotonNetwork.room.CustomProperties["Win"])
				GameOver(PunTeams.Team.red);
			if ((int)PhotonNetwork.room.CustomProperties["Blue"] >= (int)PhotonNetwork.room.CustomProperties["Win"])
				GameOver(PunTeams.Team.blue);
		}
	}

	private void GameOver(PunTeams.Team Winner)
	{
		gameOver = true;

		if (PhotonNetwork.player.GetTeam() == Winner)
			print("WIIIN !!!!!!");
		else
			print("DEFEAT... :(");
	}


}
