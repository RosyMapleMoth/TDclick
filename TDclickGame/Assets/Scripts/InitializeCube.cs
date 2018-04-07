using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InitializeCube : MonoBehaviour
{
	public Material[] materials;

	private GameState gameState;

	public UnityEvent openMenu;
	public UnityEvent closeMenu;

	public Canvas cubeCreationMenu;
	public Button[] cubeSelectionButtons;

	private GameObject nextRoad;

	public GameObject enemy;

	private bool beenChecked;

	public enum CubeType
	{
		Tower,
		Town,
		Road,
		Forest}

	;

	private CubeType type;

	// Use this for initialization
	void Start ()
	{
		gameState = GameObject.FindObjectOfType<GameState> ();
		MakeBlock (CubeType.Tower);
		gameState.validClick.AddListener (ClickedOn);
		type = CubeType.Tower;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void ClickedOn ()
	{
		if (gameState.objectClicked == gameObject) {
			OpenBlocks ();
		}
	}

	private void OpenBlocks ()
	{
		openMenu.Invoke ();
		cubeCreationMenu.gameObject.SetActive (true);

		cubeSelectionButtons [0].onClick.AddListener (() => MakeBlock (CubeType.Tower));
		cubeSelectionButtons [1].onClick.AddListener (() => MakeBlock (CubeType.Town));
		cubeSelectionButtons [2].onClick.AddListener (() => MakeBlock (CubeType.Road));
		cubeSelectionButtons [3].onClick.AddListener (() => MakeBlock (CubeType.Forest));
		cubeSelectionButtons [4].onClick.AddListener (CloseBlocks);

		Debug.Log ("openeing menu");
	}

	private void CloseBlocks ()
	{
		closeMenu.Invoke ();
		cubeCreationMenu.gameObject.SetActive (false);
		cubeSelectionButtons [0].onClick.RemoveAllListeners ();
		cubeSelectionButtons [1].onClick.RemoveAllListeners ();
		cubeSelectionButtons [2].onClick.RemoveAllListeners ();
		cubeSelectionButtons [3].onClick.RemoveAllListeners ();
		cubeSelectionButtons [4].onClick.RemoveAllListeners ();

		Debug.Log ("closing menu");
	}

	private void MakeBlock (CubeType type)
	{
		this.gameObject.GetComponent<MeshRenderer> ().material = materials [(int)type];

		this.type = type;

		CloseBlocks ();
	}

	public void InitMap ()
	{
		if (type == CubeType.Road) {
			if (nextRoad != null) {
				Path path = gameObject.AddComponent<Path> ();
				path.nextPath = nextRoad;
				gameObject.tag = "Road";
			} else {
				throw new System.Exception ("Tried to initialize invalid road");
			}
		} else if (type == CubeType.Forest) {
			if (nextRoad != null) {
				Path path = gameObject.AddComponent<Path> ();
				path.nextPath = nextRoad;
				PathStart start = gameObject.AddComponent<PathStart> ();
				start.Enemy = enemy;
				gameObject.tag = "Start";
			} else {
				throw new System.Exception ("Tried to initialize invalid road");
			}
		} else if (type == CubeType.Town) {
			gameObject.tag = "End";
		} else if (type == CubeType.Tower) {
			CreateTower tower = gameObject.AddComponent<CreateTower> ();
			CreateMap map = GameObject.FindObjectOfType<CreateMap> ();
			tower.Towers = map.towers;
			tower.towerCreationMenu = map.menu;
			tower.towerSelectionButtons = map.towerbuttons;
			tower.openMenu = new UnityEvent ();
			tower.closeMenu = new UnityEvent ();
			gameObject.tag = "TowerBase";
		}
		GameObject.Destroy (this);
	}

	public CubeType GetCubeType ()
	{
		return type;
	}

	public static bool TryForRoad (CreateMap map)
	{
		GameObject tile;

		bool foundTown;
		bool foundForest;

		if (map.GetTile (0, 0, out tile)) {
			if (tile.GetComponent<InitializeCube> ().FullCheck (0, 0, map, out foundTown, out foundForest)) {
				return foundTown && foundForest;
			}
		}

		return false;
	}

	protected bool FullCheck (int col, int row, CreateMap map, out bool foundTown, out bool foundForest)
	{
		GameObject tile;

		foundTown = false;
		foundForest = false;

		bool town;
		bool forest;

		bool answer = true;

		if (type == CubeType.Forest) {
			foundForest = true;
		}
		if (type == CubeType.Town) {
			foundTown = true;
			answer = RoadCheck (col, row, map);
		}

		if (answer && map.GetTile (col + 1, row, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();

			answer = tileInst.FullCheck (col + 1, row, map, out town, out forest);

			if (town) {
				foundTown = town;
			}
			if (forest) {
				foundForest = forest;
			}
		}

		if (answer && map.GetTile (col, row + 1, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();

			answer = tileInst.FullUpCheck (col, row + 1, map, out town, out forest);

			if (town) {
				foundTown = town;
			}
			if (forest) {
				foundForest = forest;
			}
		}

		if (!answer) {
			Debug.Log ("Broke in Full Check at " + col + ", " + row);
		}

		return answer;
	}

	protected bool FullUpCheck (int col, int row, CreateMap map, out bool foundTown, out bool foundForest)
	{
		GameObject tile;

		foundTown = false;
		foundForest = false;

		bool town;
		bool forest;

		bool answer = true;

		if (type == CubeType.Forest) {
			foundForest = true;
		}
		if (type == CubeType.Town) {
			foundTown = true;
			answer = RoadCheck (col, row, map);
		}

		if (answer && map.GetTile (col, row + 1, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();

			answer = tileInst.FullUpCheck (col, row + 1, map, out town, out forest);

			if (town) {
				foundTown = town;
			}
			if (forest) {
				foundForest = forest;
			}
		}

		if (!answer) {
			Debug.Log ("Broke in Full Check at " + col + ", " + row);
		}

		return answer;
	}

	protected bool RoadCheck (int col, int row, CreateMap map)
	{
		bool up = true;
		bool down = true;
		bool left = true;
		bool right = true;

		beenChecked = true;

		GameObject tile;

		if (map.GetTile (col + 1, row, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();
			right = tileInst.type != CubeType.Forest;
			if (right && tileInst.type != CubeType.Town) {
				right = tile.GetComponent<InitializeCube> ().RoadCheckRight (col + 1, row, map, gameObject);
			}
		}
		if (map.GetTile (col - 1, row, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();
			left = tileInst.type != CubeType.Forest;
			if (left && tileInst.type != CubeType.Town) {
				left = tile.GetComponent<InitializeCube> ().RoadCheckLeft (col - 1, row, map, gameObject);
			}
		}
		if (map.GetTile (col, row - 1, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();
			down = tileInst.type != CubeType.Forest;
			if (down && tileInst.type != CubeType.Town) {
				down = tile.GetComponent<InitializeCube> ().RoadCheckDown (col, row - 1, map, gameObject);
			}
		}
		if (map.GetTile (col, row + 1, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();
			up = tileInst.type != CubeType.Forest;
			if (up && tileInst.type != CubeType.Town) {
				up = tile.GetComponent<InitializeCube> ().RoadCheckUp (col, row + 1, map, gameObject);
			}
		}

		beenChecked = false;

		if (!(up && down && left && right)) {
			Debug.Log ("Broke in Road Check at " + col + ", " + row);
		}

		return up && down && left && right;
	}

	protected bool RoadCheckLeft (int col, int row, CreateMap map, GameObject lastPath)
	{
		bool answer = true;

		if (beenChecked) {
			return true;
		}
		beenChecked = true;

		if (type == CubeType.Road) {
			nextRoad = lastPath;

			bool up = true;
			bool down = true;
			bool left = true;

			GameObject tile;

			if (map.GetTile (col, row + 1, out tile)) {
				up = tile.GetComponent<InitializeCube> ().RoadCheckUp (col, row + 1, map, gameObject);
			}
			if (map.GetTile (col, row - 1, out tile)) {
				down = tile.GetComponent<InitializeCube> ().RoadCheckDown (col, row - 1, map, gameObject);
			}
			if (map.GetTile (col - 1, row, out tile)) {
				left = tile.GetComponent<InitializeCube> ().RoadCheckLeft (col - 1, row, map, gameObject);
			}

			answer = up && down && left;
		} else if (type == CubeType.Forest) {
			nextRoad = lastPath;

			bool up = true;
			bool down = true;
			bool left = true;

			GameObject tile;

			if (map.GetTile (col, row + 1, out tile)) {
				up = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}
			if (map.GetTile (col, row - 1, out tile)) {
				down = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}
			if (map.GetTile (col - 1, row, out tile)) {
				left = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}

			answer = up && down && left;

		} else if (type == CubeType.Town) {
			answer = false;
		}

		if (!answer) {
			Debug.Log ("Broke in Road Check Left at " + col + ", " + row);
		}

		beenChecked = false;

		return answer;
	}

	protected bool RoadCheckRight (int col, int row, CreateMap map, GameObject lastPath)
	{
		bool answer = true;

		if (beenChecked) {
			return true;
		}
		beenChecked = true;

		if (type == CubeType.Road) {
			nextRoad = lastPath;

			bool up = true;
			bool down = true;
			bool right = true;

			GameObject tile;

			if (map.GetTile (col, row + 1, out tile)) {
				up = tile.GetComponent<InitializeCube> ().RoadCheckUp (col, row + 1, map, gameObject);
			}
			if (map.GetTile (col, row - 1, out tile)) {
				down = tile.GetComponent<InitializeCube> ().RoadCheckDown (col, row - 1, map, gameObject);
			}
			if (map.GetTile (col + 1, row, out tile)) {
				right = tile.GetComponent<InitializeCube> ().RoadCheckRight (col + 1, row, map, gameObject);
			}

			answer = up && down && right;
		} else if (type == CubeType.Forest) {
			nextRoad = lastPath;

			bool up = true;
			bool down = true;
			bool right = true;

			GameObject tile;

			if (map.GetTile (col, row + 1, out tile)) {
				up = tile.GetComponent<InitializeCube> ().CheckYourself ();

			}
			if (map.GetTile (col, row - 1, out tile)) {
				down = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}
			if (map.GetTile (col + 1, row, out tile)) {
				right = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}

			answer = up && down && right;

		} else if (type == CubeType.Town) {
			answer = false;
		}

		if (!answer) {
			Debug.Log ("Broke in Road Check Right at " + col + ", " + row);
		}

		beenChecked = false;

		return answer;
	}

	protected bool RoadCheckUp (int col, int row, CreateMap map, GameObject lastPath)
	{
		bool answer = true;

		if (beenChecked) {
			return true;
		}
		beenChecked = true;

		if (type == CubeType.Road) {
			nextRoad = lastPath;

			bool up = true;
			bool right = true;
			bool left = true;

			GameObject tile;

			if (map.GetTile (col, row + 1, out tile)) {
				up = tile.GetComponent<InitializeCube> ().RoadCheckUp (col, row + 1, map, gameObject);
			}
			if (map.GetTile (col + 1, row, out tile)) {
				right = tile.GetComponent<InitializeCube> ().RoadCheckRight (col + 1, row, map, gameObject);
			}
			if (map.GetTile (col - 1, row, out tile)) {
				left = tile.GetComponent<InitializeCube> ().RoadCheckLeft (col - 1, row, map, gameObject);
			}

			answer = up && right && left;
		} else if (type == CubeType.Forest) {
			nextRoad = lastPath;

			bool up = true;
			bool right = true;
			bool left = true;

			GameObject tile;

			if (map.GetTile (col, row + 1, out tile)) {
				up = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}
			if (map.GetTile (col + 1, row, out tile)) {
				right = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}
			if (map.GetTile (col - 1, row, out tile)) {
				left = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}

			answer = up && right && left;

		} else if (type == CubeType.Town) {
			answer = false;
		}

		if (!answer) {
			Debug.Log ("Broke in Road Check Up at " + col + ", " + row);
		}

		beenChecked = false;

		return answer;
	}

	protected bool RoadCheckDown (int col, int row, CreateMap map, GameObject lastPath)
	{
		bool answer = true;

		if (beenChecked) {
			return true;
		}
		beenChecked = true;

		if (type == CubeType.Road) {
			nextRoad = lastPath;

			bool right = true;
			bool down = true;
			bool left = true;

			GameObject tile;

			if (map.GetTile (col + 1, row, out tile)) {
				right = tile.GetComponent<InitializeCube> ().RoadCheckUp (col + 1, row, map, gameObject);
			}
			if (map.GetTile (col, row - 1, out tile)) {
				down = tile.GetComponent<InitializeCube> ().RoadCheckDown (col, row - 1, map, gameObject);
			}
			if (map.GetTile (col - 1, row, out tile)) {
				left = tile.GetComponent<InitializeCube> ().RoadCheckLeft (col - 1, row, map, gameObject);
			}

			answer = right && down && left;
		} else if (type == CubeType.Forest) {
			nextRoad = lastPath;

			bool right = true;
			bool down = true;
			bool left = true;

			GameObject tile;

			if (map.GetTile (col + 1, row, out tile)) {
				right = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}
			if (map.GetTile (col, row - 1, out tile)) {
				down = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}
			if (map.GetTile (col - 1, row, out tile)) {
				left = tile.GetComponent<InitializeCube> ().CheckYourself ();
			}

			answer = right && down && left;

		} else if (type == CubeType.Town) {
			answer = false;
		}

		if (!answer) {
			Debug.Log ("Broke in Road Check Down at " + col + ", " + row);
		}

		beenChecked = false;

		return answer;
	}

	protected bool CheckYourself ()
	{
		return type == CubeType.Tower;
	}
}
