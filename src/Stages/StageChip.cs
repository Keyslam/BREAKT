using System.Collections.Generic;

public class StageChip : StageBase {
	public StageChip(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelSails(),
		new LevelHawaii(),
		new LevelAtari(),
		new LevelSweety(),
		new LevelDestructive(),
	}, audioManager, 1) {
	}
}