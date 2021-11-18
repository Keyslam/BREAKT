using Love;

public class BlockStandard : BlockBase {
	public static Image chocolateStandard = Graphics.NewImage("Assets/Blocks/ChocolateStandard.png");
	public static Image blueStandard = Graphics.NewImage("Assets/Blocks/BlueStandard.png");
	public static Image pinkStandard = Graphics.NewImage("Assets/Blocks/PinkStandard.png");
	public static Image whiteStandard = Graphics.NewImage("Assets/Blocks/WhiteStandard.png");
	public static Image redStandard = Graphics.NewImage("Assets/Blocks/RedStandard.png");
	public static Image greenStandard = Graphics.NewImage("Assets/Blocks/GreenStandard.png");
	public static Image black = Graphics.NewImage("Assets/Blocks/BlackStandard.png");

	private Image image = null;

	public BlockStandard(Humper.World world, Vector2 position, Image image) : base(world, position) { 
		this.image = image;

		this.requiredForFinish = true;
	}

	public override void Draw() {	
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}

	
}