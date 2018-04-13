using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class InitializeCube : MonoBehaviour
{
    public enum dir {Up, Left, Down, Right, none }



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

        bool answer = false;

        List<GameObject> forests = new List<GameObject>();

		if (map.GetTile (0, 0, out tile)) {
			if (tile.GetComponent<InitializeCube> ().FullCheck (0, 0, map, out foundTown, out foundForest, forests)) {
				answer = foundTown && foundForest;

                foreach (GameObject x in forests)
                {
                    if (x.GetComponent<InitializeCube>().nextRoad == null)
                    {
                        answer = false;
                        break;
                    }
                }
            }
		}

		return answer;
	}

    protected bool FullCheck(int col, int row, CreateMap map, out bool foundTown, out bool foundForest, List<GameObject> forests)
	{
		GameObject tile;

		foundTown = false;
		foundForest = false;

		bool town;
		bool forest;

		bool answer = true;

		if (type == CubeType.Forest) {
			foundForest = true;
            forests.Add(gameObject);
        }
		if (type == CubeType.Town) {
			foundTown = true;
			answer = RoadCheck (col, row, map, gameObject, dir.none);
		}

		if (answer && map.GetTile (col + 1, row, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();

			answer = tileInst.FullCheck (col + 1, row, map, out town, out forest, forests);

			if (town) {
				foundTown = town;
			}
			if (forest) {
				foundForest = forest;
            }
		}

		if (answer && map.GetTile (col, row + 1, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();

			answer = tileInst.FullUpCheck (col, row + 1, map, out town, out forest, forests);

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

	protected bool FullUpCheck (int col, int row, CreateMap map, out bool foundTown, out bool foundForest, List<GameObject> forests)
	{
		GameObject tile;

		foundTown = false;
		foundForest = false;

		bool town;
		bool forest;

		bool answer = true;

		if (type == CubeType.Forest) {
			foundForest = true;
            forests.Add(gameObject);
        }
		if (type == CubeType.Town) {
			foundTown = true;
			answer = RoadCheck (col, row, map, gameObject, dir.none);
		}

		if (answer && map.GetTile (col, row + 1, out tile)) {
			InitializeCube tileInst = tile.GetComponent<InitializeCube> ();

			answer = tileInst.FullUpCheck (col, row + 1, map, out town, out forest, forests);

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

	protected bool RoadCheck (int col, int row, CreateMap map, GameObject GO, dir dirLast)
	{
		bool up = true;
		bool down = true;
		bool left = true;
		bool right = true;

        bool answer = true;

        if (beenChecked)
        {
            return true;
        }

        beenChecked = true;

        GameObject tile;

        if (dirLast != dir.none && type == CubeType.Town)
        {
            answer = false;
        }
        else if (type == CubeType.Road || dirLast == dir.none)
        {
            nextRoad = GO;

            if (map.GetTile(col, row + 1, out tile) && dirLast != dir.Down)
            {
                up = tile.GetComponent<InitializeCube>().RoadCheck(col, row + 1, map, gameObject, dir.Up);
            }
            if (map.GetTile(col, row - 1, out tile) && dirLast != dir.Up)
            {
                down = tile.GetComponent<InitializeCube>().RoadCheck(col, row - 1, map, gameObject, dir.Down);
            }
            if (map.GetTile(col - 1, row, out tile) && dirLast != dir.Right)
            {
                left = tile.GetComponent<InitializeCube>().RoadCheck(col - 1, row, map, gameObject, dir.Left);
            }
            if (map.GetTile(col + 1, row, out tile) && dirLast != dir.Left)
            {
                right = tile.GetComponent<InitializeCube>().RoadCheck(col + 1, row, map, gameObject, dir.Right);
            }

            answer = up && down && left && right;
        }
        else if (type == CubeType.Forest)
        {
            nextRoad = GO;

            if (map.GetTile(col, row + 1, out tile) && dirLast != dir.Down)
            {
                up = tile.GetComponent<InitializeCube>().CheckYourself();
                failtest(dir.Up, col, row, up);
            }
            if (map.GetTile(col, row - 1, out tile) && dirLast != dir.Up)
            {
                down = tile.GetComponent<InitializeCube>().CheckYourself();
                failtest(dir.Down, col, row, down);
            }
            if (map.GetTile(col - 1, row, out tile) && dirLast != dir.Right)
            {
                left = tile.GetComponent<InitializeCube>().CheckYourself();
                failtest(dir.Left, col, row, left);
            }
            if (map.GetTile(col + 1, row, out tile) && dirLast != dir.Left)
            {
                right = tile.GetComponent<InitializeCube>().CheckYourself();
                failtest(dir.Right, col, row, right);

            }

            answer = up && down && left && right;

        }

        if (!answer)
        {
            Debug.Log("Broke in Road Check at " + col + ", " + row);
        }

        beenChecked = false;

		return answer;
	}

    private void failtest(dir blockFailedOn, int col, int row, bool failed)
    {
        if (!failed)
        {
            Debug.Log("forest at " + col + ", " + row + " had issues with " + blockFailedOn.ToString());
        }
    }

	protected bool CheckYourself ()
	{
		return type == CubeType.Tower;
	}
}
