using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
	public PhotonPlayer photonPlayer;

	[SerializeField]
	private Text playerNameText, playerPingText;
	public Image playerListImage;
	public GameObject KickButton, RoomOwnerMarker;

	
	public void ApplyPhotonPlayer(PhotonPlayer PhPlayer)
	{
		photonPlayer = PhPlayer;

		// Указываем ник игрока
		playerNameText.text = PhPlayer.NickName;
		// Выделяем элемент нашего игрока в списке
		if (photonPlayer == PhotonNetwork.player)
			playerListImage.enabled = true;
		// Включаем кнопки "kick", если мы Мастер Клиент
		if (PhotonNetwork.player.IsMasterClient)
			KickButton.SetActive(true);
		// Ставим метку на мастера клиента
		if (photonPlayer.IsMasterClient)
			RoomOwnerMarker.SetActive(true);
		//Включаем сопрограмму, показывающую пинг игрока
		StartCoroutine(C_ShowPing());

		if (PhPlayer.GetTeam() == PunTeams.Team.blue)
			gameObject.transform.SetParent(GameObject.Find("PlayerLayoutGroup").GetComponent<PlayerLayoutGroup>().blueGroup, false);
			
		if (PhPlayer.GetTeam() == PunTeams.Team.red)
			gameObject.transform.SetParent(GameObject.Find("PlayerLayoutGroup").GetComponent<PlayerLayoutGroup>().redGroup, false);
	}

	private IEnumerator C_ShowPing()
	{
		while (PhotonNetwork.connected)
		{
			int ping = (int)photonPlayer.CustomProperties["Ping"];
			playerPingText.text = ping.ToString();
			yield return new WaitForSeconds(1f);
		}

		yield break;
	}

	public void OnClick_KickButton()
	{
		if (!PhotonNetwork.player.IsMasterClient)
			return;
		PhotonNetwork.CloseConnection(photonPlayer);
	}
}
