using System.Collections.Generic;

public class StageLove : StageBase {
	public StageLove(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelLove(),
		new LevelPig(),
		new LevelInspector(),
		new LevelWhale(),
	}, audioManager, 6) {
	}
}