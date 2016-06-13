using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class many : MonoBehaviour {
	public Text MoneyText;
	public Text RandomText;
	private Text Money;
	private Text rnd_Money;

	private int total_money;
	private int random_number;
	private static int nextLevel = 1;

	void Start ()
	{  
		random_number = Random.Range(3, 7);
		Debug.Log (random_number);
		Money = MoneyText.GetComponent<Text> ();
		rnd_Money = RandomText.GetComponent<Text> ();
		rnd_Money.text=random_number.ToString();

		if (PlayerPrefs.HasKey ("Money")) {
			total_money = PlayerPrefs.GetInt ("Money");
			total_money = total_money + random_number;
		} 

		if (PlayerPrefs.HasKey ("Level")) {
			nextLevel = PlayerPrefs.GetInt ("Level");
		}
	}

	void Update ()
	{
		Money.text=total_money.ToString();
	}
		
	public void Save_and_nextLevel()
	{
		PlayerPrefs.SetInt("Money", total_money);
		PlayerPrefs.Save();
		SceneManager.LoadScene(++nextLevel);
	}
}
