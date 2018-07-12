using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldIncrementer : MonoBehaviour {

/* How do we implement clicker farms?
we could just have this class handle everythng, or 
we could have this class be a single farm, and have multiple live
instances of this class. */

	public struct BuildingType
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

	List<BuildingType> Buildings = new List<BuildingType>();
	 
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
			BuildingType building = CreateBuildingType(name);

			GameObject button = GameObject.Instantiate(buttonPrefab);

			building.button = button;

			button.transform.SetParent(buttons.transform, false);

			button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);

			button.GetComponent<Button>().onClick.AddListener(() => BuildBuilding(building));

			Text buttonText = button.GetComponentInChildren<Text>();

			buttonText.text = building.Name + " Build Cost: " + CurrentBuildingCost().ToString();

            Buildings.Add(building);
        }
	}

	private BuildingType CreateBuildingType(string name)
	{
        BuildingType building = new BuildingType
        {
            Name = name,
            baseValue = 0,
            level = 0,
            goldPerSec = 0,
            built = false
        };

        return building; 
	}

	private void BuildBuilding(BuildingType building)
	{
		if (gameState.GetGold() >= CurrentBuildingCost())
		{
            Buildings.Remove(building);

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

            foreach(BuildingType buildingInc in Buildings)
            {
                if (!buildingInc.built)
                {
                    buildingInc.button.GetComponentInChildren<Text>().text = buildingInc.Name + " Build Cost: " + CurrentBuildingCost().ToString();
                }
            }

            Buildings.Add(building);
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
			foreach(BuildingType building in Buildings)
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

	private void LevelBuilding(BuildingType building)
	{
		if (gameState.GetGold() >= BuildingUpgradeCost(building))
		{
            Debug.Log(Buildings.Remove(building).ToString());

			gameState.ChangeGold( -1 * BuildingUpgradeCost(building));

			building.level += 1;

			building.goldPerSec += (building.baseValue / 20);

            building.button.GetComponent<Button>().onClick.RemoveAllListeners();

            building.button.GetComponent<Button>().onClick.AddListener(() => LevelBuilding(building));

            Text buttonText = building.button.GetComponentInChildren<Text>();

			buttonText.text = building.Name + " Upgrade Cost: " + BuildingUpgradeCost(building).ToString() + ", current earnings: " + building.goldPerSec.ToString();

            Buildings.Add(building);
		}

	}

	private int BuildingUpgradeCost(BuildingType building)
	{
		int cost;

		cost = (int)(building.baseValue * (Mathf.Pow(1.05f, building.level)));

		return cost;
	}
}
