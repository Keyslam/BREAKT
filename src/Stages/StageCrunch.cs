using System.Collections.Generic;

public class StageCrunch : StageBase {
	public StageCrunch(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelHomeWrecker(),
		new LevelVino(),
		new LevelSaxophone(),
		new LevelAtari(),
		new LevelHawaiiii(),
	}, audioManager, 3) {
	}
}