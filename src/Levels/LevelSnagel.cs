using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelSnagel : LevelBase {
	private static string levelString = String.Join("\n",
		"00000000BBBB00000",
		"0000000Brrrr00000",
		"000000Brrrrrrr000",
		"b0b000BrrrBrrr000",
		"0b0000BrrrrBrrB00",
		"bbbbbrrrBBBrrrB00",
		"bbbbrrrrrrrrrr000",
		"0bbbrrrrrrrrr0000",
		"0000BBBBBBBBB0000"
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
	
	private static Image image = Graphics.NewImage("Assets/Levels/Snagel.png");
	public override Image GetName() {
		return image;
	}
}