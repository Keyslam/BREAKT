using System.Collections.Generic;

public class StageTest : StageBase {
	public StageTest(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelTest(),
	}, audioManager, 99) {
	}
}