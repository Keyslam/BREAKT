using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelJinxy : LevelBase {
	private static string levelString = String.Join("\n",
		"00000000000000000",
		"0RRRRRRR0b0b0RRR0",
		"000000000bbb00000",
		"00000b000bbb00000",
		"0000b000bbb000000",
		"0000b00bbbb000000",
		"0000b0bbbbb000000",
		"0RRR0bbbbbb0RRRR0",
		"00000000000000000"
	);

	public override List<BlockBase> GetLevel(Humper.World world) {
		List<BlockBase> blocks = new List<BlockBase>();

		var lines = levelString.Split("\n");
		
		for (int y = 0; y < lines.Length; y++) {
			var line = lines[y];
			for (int x = 0; x < line.Length; x++) {
				BlockBase block = null;

				switch (line[x]) {
					case 'R':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.pink);
						break;
					case 'b':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.chocolateStandard);
						break;
				}

				if (block != null)
					blocks.Add(block);
			}
		}

		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/Jinxy.png");
	public override Image GetName() {
		return image;
	}
}