using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldIncrementer : MonoBehaviour {

/* How do we implement clicker farms?
we could just have this class handle everythng, or 
we could have this class be a single farm, and have multiple live
instances of this class. */

	public struct buildingType
	{
		public string Name;
		public int baseValue;
		public int level;
		public bool built;
		public int goldPerSec;
		public GameObject button;
	}
	
	int numberOfBuildings;

	GameState gameState;
	
	public GameObject buttons;

	public GameObject buttonPrefab;

	private float timer;

	//public textf

	List<buildingType> Buildings = new List<buildingType>();
	 
	string [] names;

	void Start () 
	{
		timer = 0;

		gameState = GameObject.FindObjectOfType<GameState>();

		names = new string [9];

		names[0] = "Arrowsmith";
		names[1] = "Bowcrafter";
		names[2] = "Woodcutter";
		names[3] = "Charcoaler";
		names[4] = "Cannonbuilder";
		names[5] = "Bombcrafter";
		names[6] = "Blackpowder Manufactury";
		names[7] = "Frost Well";
		names[8] = "Snow Mine";

		foreach (string name in names)
		{
			buildingType building = CreateBuildingType(name);

			Buildings.Add(building);

			GameObject button = GameObject.Instantiate(buttonPrefab);

			building.button = button;

			button.transform.SetParent(buttons.transform, false);

			button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);

			button.GetComponent<Button>().onClick.AddListener(() => BuildBuilding(building));

			Text buttonText = button.GetComponentInChildren<Text>();

			buttonText.text = building.Name + " Build Cost: " + CurrentBuildingCost().ToString();
		}
	}

	private buildingType CreateBuildingType(string name)
	{
		buildingType building = new buildingType();
		building.Name = name;
		building.baseValue = 0;
		building.level = 0;
		building.goldPerSec = 0;
		building.built = false;

		return building; 
	}

	private void BuildBuilding(buildingType building)
	{
		if (gameState.GetGold() >= CurrentBuildingCost())
		{
			gameState.ChangeGold( -1 * CurrentBuildingCost());

			building.level += 1;

			building.baseValue = CurrentBuildingCost();

			building.goldPerSec = (int)(building.baseValue / 20);
			
			building.built = true;

			numberOfBuildings += 1;

			building.button.GetComponent<Button>().onClick.RemoveAllListeners();

			building.button.GetComponent<Button>().onClick.AddListener(() => LevelBuilding(building));

			Text buttonText = building.button.GetComponentInChildren<Text>();

			buttonText.text = building.Name + " Upgrade Cost: " + BuildingUpgradeCost(building).ToString() + ", current earnings: " + building.goldPerSec.ToString();
		}
	}

	private int CurrentBuildingCost()
	{
		int buildingCost;

		 buildingCost = (int)(100 * Mathf.Pow(2f, numberOfBuildings));

		return buildingCost;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if (timer > 1f)
		{
			foreach(buildingType building in Buildings)
			{
				if (building.goldPerSec > 0)
				{
					gameState.ChangeGold(building.goldPerSec);
					Debug.Log("Earned " + building.goldPerSec.ToString() + " Gold");
				}
			}

			Debug.Log("Earned Gold");

			timer = 0;
		}
	}

	private void LevelBuilding(buildingType building)
	{
		if (gameState.GetGold() >= BuildingUpgradeCost(building))
		{
			gameState.ChangeGold( -1 * BuildingUpgradeCost(building));

			building.level += 1;

			building.goldPerSec = (building.baseValue / 20) * building.level;

			Text buttonText = building.button.GetComponentInChildren<Text>();

			buttonText.text = building.Name + " Upgrade Cost: " + BuildingUpgradeCost(building).ToString() + ", current earnings: " + building.goldPerSec.ToString();
		}

	}

	private int BuildingUpgradeCost(buildingType building)
	{
		int cost;

		cost = building.baseValue * (int)(Mathf.Pow(1.05f, building.level));

		return cost;
	}
}
