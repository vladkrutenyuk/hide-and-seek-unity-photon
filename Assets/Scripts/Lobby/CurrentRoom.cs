using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CurrentRoom : MonoBehaviour
{
	Hashtable roomProps = new Hashtable();
	public Dropdown dd;
	public GameObject MasterSettings;

	private void OnEnable()
	{
		if (PhotonNetwork.isMasterClient)
			MasterSettings.SetActive(true);
		else
			MasterSettings.SetActive(false);
	}

	public void OnClick_Start()
	{
		int level = (int)PhotonNetwork.room.CustomProperties["Map"];

		if (level == 0) // Random map
			level = Random.Range(1, 2);

		roomProps["InBattle"] = true;
		PhotonNetwork.room.SetCustomProperties(roomProps);

		PhotonNetwork.LoadLevel(level); // Loading selected level's number by MapSelector
	}

	public void OnClick_LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void MapSelector()
	{
		roomProps["Map"] = dd.value;
		PhotonNetwork.room.SetCustomProperties(roomProps);
	}

	public void ModeSelector()
	{

	}

	private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (newMasterClient == PhotonNetwork.player)
			MasterSettings.SetActive(true);
		else
			MasterSettings.SetActive(false);
	}
}
