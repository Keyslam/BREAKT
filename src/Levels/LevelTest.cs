using System.Collections.Generic;
using Love;

public class LevelTest : LevelBase {
	public override List<BlockBase> GetLevel(Humper.World world) {
		List<BlockBase> blocks = new List<BlockBase>();

		BlockBase block = new BlockStandard(world, new Vector2(450, 500), BlockStandard.chocolateStandard);
		blocks.Add(block);
		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/Atari.png");
	public override Image GetName() {
		return image;
	}
}