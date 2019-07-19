using UnityEngine;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{
	MatchManager MM;

	public GameObject KillFeedElement, KillFeed;
	public Text ownHealthText, blueScore, redScore, winScore;
	public GameObject menuAlive, menuDeath, menuChoise, scoreBoard;

	private void Start ()
	{
		MM = GameObject.Find("MatchManager").GetComponent<MatchManager>();

		// Set current win and team's score
		SetCurrentTeamScore((int)PhotonNetwork.room.CustomProperties["Red"], (int)PhotonNetwork.room.CustomProperties["Blue"]);
		int win = (int)PhotonNetwork.room.CustomProperties["Win"];
		winScore.text = win.ToString();

	}

	public void CallKillFeedMsg(string murder, string victim)
	{
		GameObject klfd = Instantiate(KillFeedElement, KillFeed.transform, false);
		klfd.GetComponent<KillFeedElement>().NamesColors(murder, victim);
		Destroy(klfd, 3f);
	}

	public void SetCurrentHealth(int curHealth)
	{
		ownHealthText.text = curHealth.ToString();
	}

	public void SetCurrentTeamScore(int red, int blue)
	{
		blueScore.text = blue.ToString();
		redScore.text = red.ToString();
	}

	public void OnClick_Spawn()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		menuChoise.SetActive(false);
		menuAlive.SetActive(true);

		MM.Spawn();
	}

	/*public void KillPushNotif(string victimName)
	{
		GameObject killPushNotif = Instantiate(killPushNotif_prefab, transform);
		killPushNotif.GetComponent<Text>().text = victimName;
		Destroy(killPushNotif, 2f);
	}*/
}
