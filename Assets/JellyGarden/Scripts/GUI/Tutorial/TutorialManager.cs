using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
	public List<Item> items = new List<Item> ();
	public GameObject tutorial;
	public GameObject text;
	public GameObject canvas;
	bool showed;

	void OnEnable ()
	{
		Debug.Log("[TutorialManager] OnEnable - Begin");
		LevelManager.OnLevelLoaded += CheckNewTarget;
		LevelManager.OnStartPlay += DisableTutorial;
		Debug.Log("[TutorialManager] OnEnable - End");
	}

	void OnDisable ()
	{
		Debug.Log("[TutorialManager] OnDisable - Begin");
		LevelManager.OnLevelLoaded -= CheckNewTarget;
		LevelManager.OnStartPlay -= DisableTutorial;
		Debug.Log("[TutorialManager] OnDisable - End");
	}

	void DisableTutorial ()
	{
		Debug.Log("[TutorialManager] DisableTutorial - Begin");
		if (!showed && LevelManager.THIS.currentLevel == 1) {
			ChangeLayerNum (0);
			tutorial.SetActive (false);
			showed = true;
		}
		Debug.Log("[TutorialManager] DisableTutorial - End");
	}


	void CheckNewTarget ()
	{
		Debug.Log("[TutorialManager] CheckNewTarget - Begin, Level: " + LevelManager.THIS.currentLevel);
		if (LevelManager.THIS.currentLevel == 1 && !showed)
			StartCoroutine (WaitForCombine ());
		Debug.Log("[TutorialManager] CheckNewTarget - End");
	}

	void ShowStarsTutorial ()
	{
		Debug.Log("[TutorialManager] ShowStarsTutorial - Begin");
		//canvas.transform.position = Vector3.down * FindMaxY(items);
		tutorial.SetActive (true);
		ChangeLayerNum (4);
		Debug.Log("[TutorialManager] ShowStarsTutorial - End");
	}

	IEnumerator WaitForCombine ()
	{
		Debug.Log("[TutorialManager] WaitForCombine - Begin");
		yield return new WaitUntil (() => AI.THIS.GetCombine () != null);
//		bool keepWaiting = true;
//		while (keepWaiting) {
//			yield return new WaitForFixedUpdate ();
//			if (AI.THIS.GetCombine () != null)
//				keepWaiting = false;
//		}
		items = AI.THIS.GetCombine ();
		Debug.Log("[TutorialManager] Got items, count: " + (items != null ? items.Count.ToString() : "null"));

		if (items == null || items.Count == 0) {
			Debug.LogWarning("[TutorialManager] No items to sort");
			yield break;
		}
		
		items.RemoveAll(item => item == null);
		Debug.Log("[TutorialManager] After null cleanup, count: " + items.Count);
		
		if (items.Count == 0) {
			Debug.LogWarning("[TutorialManager] All items are null");
			yield break;
		}
		
		items.Sort (SortByDistance);
		Debug.Log("[TutorialManager] Items sorted");
		
		if (LevelManager.THIS.currentLevel == 1 && !showed) {
			ShowStarsTutorial ();
		}
		Debug.Log("[TutorialManager] WaitForCombine - End");
	}

	public Vector3[] GetItemsPositions ()
	{
		Vector3[] positions = new Vector3[items.Count];
		for (int i = 0; i < items.Count; i++) {
			positions [i] = items [i].transform.position + new Vector3 (1, -1, 0);
		}
		return positions;
	}

	private int SortByDistance (Item item1, Item item2)
	{
		if (item1 == null && item2 == null) return 0;
		if (item1 == null) return 1;
		if (item2 == null) return -1;
		if (items.Count == 0 || items[0] == null) return 0;
		
		Item itemFirst = items [0];
		float x = Vector3.Distance (itemFirst.transform.position, item1.transform.position);
		float y = Vector3.Distance (itemFirst.transform.position, item2.transform.position);
		int retval = y.CompareTo (x);

		if (retval != 0) {
			return retval;
		} else {
			return y.CompareTo (x);
		}
	}

	public int FindMaxY (List<Item> list)
	{
		int max = int.MinValue;
		foreach (Item type in list) {
			if (type.transform.position.y > max) {
				max = (int)type.transform.position.y + 2;
			}
		}
		return max;
	}

	void ChangeLayerNum (int num)
	{
		foreach (var item in items) {
			if (item) {

				item.square.GetComponent<SpriteRenderer> ().sortingOrder = num;
				item.sprRenderer.sortingOrder = num + 2;
			}
		}
	}

}
