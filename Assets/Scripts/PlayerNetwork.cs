using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerNetwork : MonoBehaviour
{
	public static PlayerNetwork Instance;
	public string PlayerName;

	public GameObject InGameCanvas;

	Coroutine pingCoroutine;
	PhotonView photonView;
	Hashtable prop = new Hashtable();

	private void Awake()
	{
		Instance = this;
		photonView = GetComponent<PhotonView>();
		PlayerName = "Player#" + Random.Range(1000, 9999);

		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}
	
	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex > 0)
		{
			// Add Match Manager in scene
			GameObject MatchManager = new GameObject("MatchManager");
			MatchManager.AddComponent<MatchManager>();

			// Add In-Game Canvas
			Instantiate(InGameCanvas, Vector3.zero, Quaternion.identity).name = "InGameCanvas";
		}
	}
	
	#region Set Teams and Player Props
	bool RedMoreThanBlue()
	{
		int red = 0, blue = 0;
		foreach (PhotonPlayer player in PhotonNetwork.playerList)
		{
			if (player.GetTeam() == PunTeams.Team.blue)
				blue++;
			if (player.GetTeam() == PunTeams.Team.red)
				red++;
		}

		if (red > blue)
			return true;
		else
			return false;
	}
	private void OnJoinedRoom()
	{
		PhotonNetwork.player.SetTeam(PunTeams.Team.none);
		// Set Team
		if (RedMoreThanBlue())
			PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
		else
			PhotonNetwork.player.SetTeam(PunTeams.Team.red);

		// Set initial K/D score
		prop["Kills"] = 0;
		prop["Deaths"] = 0;
		prop["IsDead"] = true;
		PhotonNetwork.player.SetCustomProperties(prop);
	}
	private void OnLeftRoom()
	{
		PhotonNetwork.player.SetTeam(PunTeams.Team.none);

	}
	#endregion Set Teams

	#region Ping
	private IEnumerator SetPing()
	{
		while (PhotonNetwork.connected)
		{
			Hashtable playerCustomProperties = new Hashtable();
			playerCustomProperties["Ping"] = PhotonNetwork.GetPing();
			PhotonNetwork.player.SetCustomProperties(playerCustomProperties);

			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	private void OnConnectedToMaster()
	{
		if (pingCoroutine != null)
			StopCoroutine(pingCoroutine);
		pingCoroutine = StartCoroutine(SetPing());
	}
	#endregion Ping
}
