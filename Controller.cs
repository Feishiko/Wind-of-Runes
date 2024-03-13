public class Controller
{
	private static Controller uniqueInstance;
	public Player player { get; set; }
	public int currentFloor { get; set; } = 0;
	public int maxFloor { get; set; } = -1;
	// LevelWidth is 40, LevelHeight is 40
	public BaseObject[,,,] wholeLevels = new BaseObject[40, 40, 4, 20];
	public bool isUp { get; set; } = false;
	public bool isWin { get; set; } = false;
	public bool isAnimation { get; set; } = false;
	public string playerName { get; set; } = null;
	public string playerSpecies { get; set; } = null;
	public string playerGender { get; set; } = null;
	public bool isSave = false;
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

	public void Init()
	{
		player = null;
		currentFloor = 0;
		maxFloor = -1;
		// LevelWidth is 40, LevelHeight is 40
		wholeLevels = new BaseObject[40, 40, 4, 20];
		// public int turn = 0;
		isUp = false;
		// public string[] logs = new string[4];
		isWin = false;
		isAnimation = false;
		playerName = null;
		playerSpecies = null;
		playerGender = null;
	}
}
