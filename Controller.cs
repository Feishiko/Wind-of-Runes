[System.Serializable]
public class Controller
{
	private static Controller uniqueInstance;
	public Player player;
	public int currentFloor = 0;
	public int maxFloor = -1;
	// LevelWidth is 40, LevelHeight is 40
	public BaseObject[,,,] wholeLevels = new BaseObject[40, 40, 4, 20];
	// public int turn = 0;
	public bool isUp = false;
	// public string[] logs = new string[4];
	public bool isWin = false;
	public bool isAnimation = false;
	public string playerName = null;
	public string playerSpecies = null;
	public string playerGender = null;
	private Controller()
	{
	}

	public static Controller GetInstance()
	{
		if (uniqueInstance == null)
		{
			uniqueInstance = new Controller();
		}
		return uniqueInstance;
	}
}
