using System;
using System.Collections.Generic;
using System.Linq;
using Love;

public class LevelBadTime : LevelBase {
	private static string levelString = String.Join("\n",
"0000bbbbbbbbb0000",
"00bbwwwwwwwwwbb00",
"0bwwwwwwwwwwwwwb0",
"0bwwwwwwwwwwwwwb0",
"bwwwwwwwwwwbbbwwb",
"bwwwwwwwwwwbbbwwb",
"bwwbbbwwbwwbbbwwb",
"0bwwwwwbbbwwwwwb0",
"bbwbwwwwwwwwwbwbb",
"bwwbbbbbbbbbbbwwb",
"bwwwbwbwbwbwbwwwb",
"0bbwwbbbbbbbwwbb0",
"000bbwwwwwwwbb000",
"00000bbbbbbb00000"
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
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.choco);
						break;
					case 'R':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.red);
						break;
					case 'c':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.chocolateStandard);
						break;
					case 'B':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.blue);
						break;
					case 'w':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.whiteStandard);
						break;
					case 'r':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.redStandard);
						break;
					case 'p':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.pinkStandard);
						break;
					case 'g':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.greenStandard);
						break;
					case 'G':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.green);
						break;
					case 'W':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.white);
						break;
					case 'P':
						block = new BlockIndestructible(world, new Vector2(8 + x * 52, 8 + y * 52), BlockIndestructible.pink);
						break;
					case 'b':
						block = new BlockStandard(world, new Vector2(8 + x * 52, 8 + y * 52), BlockStandard.black);
						break;
				}

				if (block != null)
					blocks.Add(block);
			}
		}

		return blocks;
	}

	private static Image image = Graphics.NewImage("Assets/Levels/BadTime.png");
	public override Image GetName() {
		return image;
	}
}