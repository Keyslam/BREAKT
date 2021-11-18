using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelGinger : LevelBase {
	private static string levelString = String.Join("\n",
		"00bb0000000000000",
		"0bb0b000000000000",
		"bb00bbbbbb0000pp0",
		"0000000000b0000pp",
		"00000bbbbb0000pp0",
		"000000b0000000000",
		"00000bb0000000000",
		"b00000b0000000000",
		"0bbbbb00000000000"
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
					case 'p':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.pinkStandard);
						break;
					case 'c':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.chocolateStandard);
						break;
				}

				if (block != null)
					blocks.Add(block);
			}
		}

		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/Ginger.png");
	public override Image GetName() {
		return image;
	}
}