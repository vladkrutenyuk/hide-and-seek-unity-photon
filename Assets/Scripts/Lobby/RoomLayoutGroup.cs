using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour
{
	
	public GameObject roomListingPrefab;

	private List<RoomListing> RoomListingButtons = new List<RoomListing>();

	private void OnReceivedRoomListUpdate()
	{
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		
		RemoveRooms();

		foreach (RoomInfo room in rooms)
		{		
			GetRooms(room);
		}
	}

	private void GetRooms(RoomInfo room)
	{
		if (room.IsVisible)
		{
			GameObject roomListingObj = Instantiate(roomListingPrefab);
			roomListingObj.transform.SetParent(transform, false);

			RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
			RoomListingButtons.Add(roomListing);

			roomListing.SetRoomListingProps
				(room.Name, room.PlayerCount.ToString(), room.MaxPlayers.ToString(), (bool)room.CustomProperties["InBattle"], 
				(int)room.CustomProperties["Map"], (string)room.CustomProperties["Mode"]);
		}
	}

	private void RemoveRooms()
	{
		List<RoomListing> removeRooms = new List<RoomListing>();

		foreach (RoomListing roomListing in RoomListingButtons)
		{
			removeRooms.Add(roomListing);
		}

		foreach (RoomListing roomListing in removeRooms)
		{
			GameObject roomListingObj = roomListing.gameObject;
			RoomListingButtons.Remove(roomListing);
			Destroy(roomListingObj);
		}
	}
}
