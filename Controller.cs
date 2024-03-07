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
