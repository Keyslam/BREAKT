using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelChocoMilk : LevelBase {
	private static string levelString = String.Join("\n",
		"00000000000000000",
		"00000000000000000",
		"00000C0000C000000",
		"00000CwwwwC000000",
		"00000CccccCCC0000",
		"00000CccccC0C0000",
		"00000CccccCC00000",
		"00000CccccC000000",
		"00000CCCCCC000000"
	);

	public override List<BlockBase> GetLevel(Humper.World world) {
		List<BlockBase> blocks = new List<BlockBase>();

		var lines = levelString.Split("\n");
		
		for (int y = 0; y < lines.Length; y++) {
			var line = lines[y];
			for (int x = 0; x < line.Length; x++) {
				BlockBase block = null;

				switch (line[x]) {
					case 'C':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.red);
						break;
					case 'c':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.chocolateStandard);
						break;
					case 'w':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.whiteStandard);
						break;
				}

				if (block != null)
					blocks.Add(block);
			}
		}

		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/Choco.png");
	public override Image GetName() {
		return image;
	}
}