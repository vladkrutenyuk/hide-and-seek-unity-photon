using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{

	public string roomName;//, currentPlayers, maxPlayers;

	public Text roomNameText, currentPlayersText, maxPlayersText, mapText, modeText;

	public GameObject InBattleText;

	public void SetRoomListingProps
		(string _roomName, string _currentPlayers, string _maxPlayers, bool inBattle, int _map, string _mode)
	{
		// Set RoomName Text and get RoomName for Click-Join
		roomName = _roomName;
		roomNameText.text = _roomName;

		// Set CurrentPlayers Text
		currentPlayersText.text = _currentPlayers;
		// Set MaxPlayers Text
		maxPlayersText.text = "/" + _maxPlayers;
		// Set In-Battle Status
		InBattleText.SetActive(inBattle);
		// Set Mode Text
		modeText.text = _mode;
		// Set Map Text
		mapText.text = "Level 0" + _map.ToString();
	}

	public void OnClick_RoomListing()
	{
		PhotonNetwork.JoinRoom(roomName);
	}

}
