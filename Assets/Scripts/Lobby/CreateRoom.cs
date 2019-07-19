using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoom : MonoBehaviour
{
	public InputField roomName;

	public void OnClick_CreateRoom()
	{
		Hashtable roomProp = new Hashtable()
		{ { "InBattle", false }, { "Red", 0 },{ "Blue", 0 }, { "Win", 5 }, { "Mode", "TDM" }, { "Map", 1 } };

		RoomOptions roomOptions = new RoomOptions()
		{
		  IsVisible = true, IsOpen = true, MaxPlayers = 4,
		  CustomRoomProperties = roomProp, CustomRoomPropertiesForLobby = new string[3] { "InBattle", "Mode", "Map" }
		};

		if (PhotonNetwork.CreateRoom(PhotonNetwork.player.NickName + "'s room", roomOptions, TypedLobby.Default))
			print("Create room ");
		else
			print("Create room failed");
	}

	private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
	{
		print("Create room failed: " + codeAndMessage[1]);
	}

	private void OnCreatedRoom()
	{
		print("Room created: " + roomName.text);
	}
}
