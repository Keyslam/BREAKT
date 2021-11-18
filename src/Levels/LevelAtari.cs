using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelAtari : LevelBase {
	private static string levelString = String.Join("\n",
		"000BB000ww000BB00",
		"00BBBB0wwww0BBBB0",
		"00BBBB0wwww0BBBB0",
		"000BB000ww000BB00",
		"00000000000000000",
		"00000000BB0000000",
		"0000000BBBB000000",
		"0000000BBBB000000",
		"00000000BB0000000"
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
				}

				if (block != null)
					blocks.Add(block);
			}
		}

		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/Atari.png");
	public override Image GetName() {
		return image;
	}
}