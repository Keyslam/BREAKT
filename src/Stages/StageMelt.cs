using System.Collections.Generic;

public class StageMelt : StageBase {
	public StageMelt(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelRocket(),
		new LevelChocoRain(),
		new LevelGinger(),
		new LevelCool(),
		new LevelOscilla(),
	}, audioManager, 5) {
	}
}