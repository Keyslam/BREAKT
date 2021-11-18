using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelRocket : LevelBase {
	private static string levelString = String.Join("\n",
		"00000000000000000",
		"00rrrrrrr00000000",
		"0rr00r00rrr000000",
		"r0000r00000rrr000",
		"rrrrrrrrrrrrrrr00",
		"rr00rrrrrrrr00rrr",
		"r0BB0rrrrrr0BB0rr",
		"0BBBB000000BBBB00",
		"00BB00000000BB000"
	);

	public override List<BlockBase> GetLevel(Humper.World world) {
		List<BlockBase> blocks = new List<BlockBase>();

		var lines = levelString.Split("\n");

		for (int y = 0; y < lines.Length; y++) {
			var line = lines[y];
			for (int x = 0; x < line.Length; x++) {
				BlockBase block = null;

				switch (line[x]) {
					case 'B':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.blue);
						break;
					case 'w':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.whiteStandard);
						break;
					case 'b':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.blueStandard);
						break;
					case 'r':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.redStandard);
						break;
				}

				if (block != null)
					blocks.Add(block);
			}
		}

		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/Rocket.png");
	public override Image GetName() {
		return image;
	}
}