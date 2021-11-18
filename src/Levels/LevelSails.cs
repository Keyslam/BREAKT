using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelSails : LevelBase {
	private static string levelString = String.Join("\n",
		"0000000bb00000000",
		"0000000b0b0000000",
		"0000000b00b000000",
		"0000000bbbbb00000",
		"0000000B000000000",
		"00w0000B00000w000",
		"00wwwwwwwwwwww000",
		"00ww0w0w0w0w0w000",
		"000wwwwwwwwww0000"
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
				}

				if (block != null)
					blocks.Add(block);
			}
		}

		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/Sails.png");
	public override Image GetName() {
		return image;
	}
}