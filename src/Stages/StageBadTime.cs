using System.Collections.Generic;

public class StageBadTime : StageBase {
	public StageBadTime(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelBadTime(),
	}, audioManager, 7) {

		audioManager.PlayMegaLove();
	}
}