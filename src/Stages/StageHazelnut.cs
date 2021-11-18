using System.Collections.Generic;

public class StageHazelnut : StageBase {
	public StageHazelnut(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelJinxy(),
		new LevelEgg(),
		new LevelEnjoyed(),
		new LevelIceCream(),
		new LevelMainMenu(),
	}, audioManager, 4) {
	}
}