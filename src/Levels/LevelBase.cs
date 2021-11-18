using System.Collections.Generic;
using Love;

public abstract class LevelBase {
	public abstract List<BlockBase> GetLevel(Humper.World world);
	public abstract Image GetName();
}