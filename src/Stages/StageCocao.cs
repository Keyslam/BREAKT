using System.Collections.Generic;

public class StageCocoa : StageBase {
	public StageCocoa(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelBird(),
		new LevelRevive(),
		new LevelSprinkles(),
		new LevelFive(),
		new LevelFone(),
	}, audioManager, 2) {
	}
}