using System;
using System.Collections.Generic;
using Retromono.Easings;
using Retromono.Tweens;

public class StageEndless : StageBase {
	public StageEndless(AudioManager audioManager) : base(new List<LevelBase>() {
		new LevelLove(),
	}, audioManager, 15) {
		levelIndex = int.MinValue;
		this.gameContext.doLivesPowerup = false;
	}

	private List<LevelBase> avalevels = new List<LevelBase>() {
		new LevelAtari(),
		new LevelBird(),
		new LevelChocoMilk(),
		new LevelChocoRain(),
		new LevelCool(),
		new LevelDestructive(),
		new LevelEgg(),
		new LevelEnjoyed(),
		new LevelFive(),
		new LevelFone(),
		new LevelGinger(),
		new LevelHawaii(),
		new LevelHawaiiii(),
		new LevelHomeWrecker(),
		new LevelIceCream(),
		new LevelInspector(),
		new LevelJinxy(),
		new LevelLove(),
		new LevelOscilla(),
		new LevelPig(),
		new LevelRevive(),
		new LevelRocket(),
		new LevelSails(),
		new LevelSaxophone(),
		new LevelSnagel(),
		new LevelSprinkles(),
		new LevelSweety(),
		new LevelVino(),
		new LevelWhale(),
		new LevelMainMenu(),
	};

	public override void LoadNextLevel() {
		Random random = new Random();
		this.level = avalevels[random.Next(avalevels.Count)];

		audioManager.PlayMainTrack();

		this.gameContext = new GameContext(
			this.level,
			new LoggerConsole(LogLevel.ERROR),
			new Humper.World(900, 900),
			new Camera(),
			this.audioManager,
			this.gameContext.lives,
			this.gameContext.score
		);

		this.gameContext.doLivesPowerup = false;

		tweensToAdd.Add(
			new TweenSequence(() => { }, new List<ITween>() {
				new TweenAnimateDouble(
					TimeSpan.FromSeconds(0.3f),
					namePos.Y,
					250.0f,
					value => { namePos.Y = (float)value; },
					Easings.CubicOut
				),

				new TweenSleep(TimeSpan.FromSeconds(0.8f)),

				new TweenAnimateDouble(
					TimeSpan.FromSeconds(0.3f),
					250,
					-76,
					value => { namePos.Y = (float)value; },
					Easings.CubicIn
				),
			}
		));
	}
}