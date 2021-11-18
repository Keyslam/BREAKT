using Love;

public class BlockIndestructible : BlockBase {
	public static Image pink = Graphics.NewImage("Assets/Blocks/PinkIndestructible.png");
	public static Image blue = Graphics.NewImage("Assets/Blocks/BlueIndestructible.png");
	public static Image choco = Graphics.NewImage("Assets/Blocks/ChocoIndestructible.png");
	public static Image red = Graphics.NewImage("Assets/Blocks/RedIndestructible.png");
	public static Image green = Graphics.NewImage("Assets/Blocks/GreenIndestructible.png");
	public static Image white = Graphics.NewImage("Assets/Blocks/WhiteIndestructible.png");
	public static Image black = Graphics.NewImage("Assets/Blocks/BlackIndestructible.png");

	private Image image = null;

	public BlockIndestructible(Humper.World world, Vector2 position, Image image) : base(world, position) { 
		this.image = image;

		this.health = int.MaxValue;

		this.requiredForFinish = false;
	}

	public override void Draw() {	
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}
}