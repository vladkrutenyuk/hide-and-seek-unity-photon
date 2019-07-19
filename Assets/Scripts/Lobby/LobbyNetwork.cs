using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviour {

	public GameObject lobbyMenu;
	public GameObject roomMenu;
	public GameObject LoadingText;

	public Text roomName;


	private void Start ()
	{
		print("LOBBYNETWORK: Connecting to server...");

		PhotonNetwork.ConnectUsingSettings("0.0.0");
	}

	private void OnConnectedToMaster()
	{
		LoadingText.SetActive(false);
		print("LOBBYNETWORK: Connected to master");

		PhotonNetwork.automaticallySyncScene = true;//     The Master Client of a room will sync
													// the loaded level with every other player in the room.
		PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
		PhotonNetwork.JoinLobby(TypedLobby.Default);

		lobbyMenu.SetActive(true);
	}

	private void OnJoinedLobby()
	{
		print("LOBBYNETWORK: Joined lobby");

		lobbyMenu.SetActive(true);
		roomMenu.SetActive(false);
	}

	
	private void OnJoinedRoom()
	{
		print("LOBBYNETWORK: Joined room");

		roomMenu.SetActive(true);
		lobbyMenu.SetActive(false);
		roomName.text = PhotonNetwork.room.Name;
	}
}
