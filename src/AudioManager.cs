using System;
using System.Collections.Generic;
using Love;

public class AudioManager {
	public List<Source> blockBreakSweetener = null;
	public List<Source> blockBreakPerc = null;
	public List<Source> blockIndestruct = null;

	public List<Source> powerupPaddlesize = null;

	public List<Source> paddleCollide = null;

	public Source powerupPaddleSmaller = null;
	public Source powerupMultiBall = null;
	public Source powerupPiercing = null;
	public Source powerupExtraLife = Audio.NewSource("Assets/Audio/SFX/Powerups/powerup_extralife.wav", SourceType.Static);
	public Source powerupScoreCoin = Audio.NewSource("Assets/Audio/SFX/Powerups/score_get_1.wav", SourceType.Static);
	public Source powerupScoreCard = Audio.NewSource("Assets/Audio/SFX/Powerups/score_get_2.wav", SourceType.Static);

	public Source paddleServe = null;

	public Source track = Audio.NewSource("Assets/Audio/Music/track.mp3", SourceType.Stream);
	public Source gameover = Audio.NewSource("Assets/Audio/Music/game_over.wav", SourceType.Stream);
	public Source menugroove = Audio.NewSource("Assets/Audio/Music/menugroove.mp3", SourceType.Stream);
	public Source scoregroove = Audio.NewSource("Assets/Audio/Music/scoregroove.mp3", SourceType.Stream);
	public Source megalove = Audio.NewSource("Assets/Audio/Music/caramelgaLOVEnia.mp3", SourceType.Stream);
		
	public Source menuConfirm = Audio.NewSource("Assets/Audio/SFX/menu_confirm.wav", SourceType.Stream);
	public Source menuCancel = Audio.NewSource("Assets/Audio/SFX/menu_cancel.wav", SourceType.Stream);

	public AudioManager() {
		track.SetLooping(true);
		track.SetVolume(0.7f);

		menugroove.SetLooping(true);
		scoregroove.SetLooping(true);
		megalove.SetLooping(true);

		this.blockBreakSweetener = new List<Source>() {
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_sweetener_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_sweetener_2.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_sweetener_3.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_sweetener_4.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_sweetener_5.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_sweetener_6.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_sweetener_7.wav", SourceType.Static)
		};

		foreach (Source source in blockBreakSweetener)
			source.SetVolume(0.7f);

		this.blockBreakPerc = new List<Source>() {
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_perc_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_perc_2.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_perc_3.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_perc_4.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_perc_5.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_perc_6.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockBreak/blockbreak_perc_7.wav", SourceType.Static)
		};

		this.blockIndestruct = new List<Source>() {
			Audio.NewSource("Assets/Audio/SFX/BlockIndestruct/block_indestruct_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockIndestruct/block_indestruct_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockIndestruct/block_indestruct_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockIndestruct/block_indestruct_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/BlockIndestruct/block_indestruct_1.wav", SourceType.Static),
		};

		this.powerupPaddlesize = new List<Source>() {
			Audio.NewSource("Assets/Audio/SFX/Powerups/PaddleSize/powerup_paddlesize_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/Powerups/PaddleSize/powerup_paddlesize_2.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/Powerups/PaddleSize/powerup_paddlesize_3.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/Powerups/PaddleSize/powerup_paddlesize_4.wav", SourceType.Static),
		};

		this.paddleCollide = new List<Source>() {
			Audio.NewSource("Assets/Audio/SFX/PaddleCollide/paddle_collide_1.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/PaddleCollide/paddle_collide_2.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/PaddleCollide/paddle_collide_3.wav", SourceType.Static),
			Audio.NewSource("Assets/Audio/SFX/PaddleCollide/paddle_collide_4.wav", SourceType.Static),
		};

		foreach (Source source in paddleCollide)
			source.SetVolume(0.7f);

		powerupPaddleSmaller = Audio.NewSource("Assets/Audio/SFX/Powerups/PaddleSize/powerup_paddlesize_smaller.wav", SourceType.Static);
		powerupMultiBall = Audio.NewSource("Assets/Audio/SFX/Powerups/powerup_multiball.wav", SourceType.Static);
		powerupPiercing = Audio.NewSource("Assets/Audio/SFX/Powerups/powerup_piercing.wav", SourceType.Static);

		paddleServe = Audio.NewSource("Assets/Audio/SFX/paddle_serve.wav", SourceType.Static);

		foreach (Source source in powerupPaddlesize)
			source.SetVolume(0.25f);
	}

	public void PlayMainTrack() {
		menugroove.SetVolume(0);
		gameover.Stop();
		scoregroove.SetVolume(0);
		megalove.Stop();

		track.Play();
	}

	public void PlayMenuGroove() {
		gameover.Stop();
		scoregroove.SetVolume(0);
		track.Stop();
		megalove.Stop();
		
		menugroove.SetVolume(1.0f);

		scoregroove.Stop();
		scoregroove.Play();
		menugroove.Play();
	}

	public void PlayGameOver() {
		scoregroove.Stop();
		track.Stop();
		menugroove.SetVolume(0);
		megalove.Stop();

		gameover.Play();
	}

	public void PlayScoreGroove() {
		track.Stop();
		menugroove.SetVolume(0f);
		gameover.Stop();
		megalove.Stop();

		scoregroove.SetVolume(1.0f);

		menugroove.Stop();
		menugroove.Play();
		scoregroove.Play();
	}

	public void PlayMegaLove() {
		track.Stop();
		gameover.Stop();
		menugroove.SetVolume(0);
		scoregroove.SetVolume(0);

		megalove.Play();
	}


	public void PlaySource(Source source, float maxPitchVariation = 0) {
		source.Stop();

		Random random = new Random();
		float pitch = (float)(1.0f + ((random.NextDouble() * 2 - 1) * maxPitchVariation));
		source.SetPitch(pitch);
		source.Play();
	}

	public void PlayRandomSource(List<Source> sources, float maxPitchVariation = 0) {
		Random random = new Random();
		int index = random.Next(sources.Count);

		this.PlaySource(sources[index], maxPitchVariation);
	}
}