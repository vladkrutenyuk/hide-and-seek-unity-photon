using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : MonoBehaviour
{
	[SerializeField]
	private GameObject PlayerListingPrefab;
	private List<PlayerListing> PlayerListings = new List<PlayerListing>();

	public Transform redGroup, blueGroup;

	private void OnEnable()
	{
		foreach (Transform child in redGroup)
		{
			Destroy(child.gameObject);
		}
		foreach (Transform child in blueGroup)
		{
			Destroy(child.gameObject);
		}

		PhotonPlayer[] players = PhotonNetwork.playerList;
		for (int i = 0; i < players.Length; i++)
		{
			GetPlayerListing(players[i]);
		}
	}

	private void GetPlayerListing(PhotonPlayer player)
	{
		if (player == null)
			return;
		RemovePlayerListing(player);

		GameObject plListingObj = Instantiate(PlayerListingPrefab);
		//plListingObj.transform.SetParent(transform, false);

		PlayerListing plListing = plListingObj.GetComponent<PlayerListing>();
		PlayerListings.Add(plListing);

		StartCoroutine(WaitingForSetTeam(player, plListing));
	}

	IEnumerator WaitingForSetTeam(PhotonPlayer player, PlayerListing plListing)
	{
		while (player.GetTeam() == 0)
			yield return new WaitForSeconds(0.01f);
		plListing.ApplyPhotonPlayer(player);
		yield break;

	}

	private void RemovePlayerListing(PhotonPlayer player)
	{
		int index = PlayerListings.FindIndex(x => x.photonPlayer == player);
		if (index != -1)
		{
			Destroy(PlayerListings[index].gameObject);
			PlayerListings.RemoveAt(index);
		}
	}

	private void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		GetPlayerListing(player);
	}

	private void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		RemovePlayerListing(player);
	}

	private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		// Ищем в списке игроков комнаты нового мастера клиента и ставим ему метку 
		int index = PlayerListings.FindIndex(x => x.photonPlayer == newMasterClient);
		print(index + " index = PlayerListings.FindIndex");
		if (index != -1)
		{
			PlayerListings[index].RoomOwnerMarker.SetActive(true);
		}

		// Для нового мастера клиента ...
		if (PhotonNetwork.player != newMasterClient)
			return;
		// ... Включаем кнопки Kick на всех игроках в комнате
		foreach (Transform child in blueGroup)
		{
			PlayerListing plList = child.GetComponent<PlayerListing>();
			plList.KickButton.SetActive(true);
		}
		foreach (Transform child in redGroup)
		{
			PlayerListing plList = child.GetComponent<PlayerListing>();
			plList.KickButton.SetActive(true);
		}
	}

	



}
