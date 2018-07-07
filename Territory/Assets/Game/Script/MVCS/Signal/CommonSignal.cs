using strange.extensions.signal.impl;
using System.Collections.Generic;


public class FrameSignal : Signal { }

public class StartupSignal : Signal { }

public class PopupUISignal : Signal<eUI> { }

public class MakeTroopSignal : Signal<eTroopType, eCountry, int, int> { }

public class EndTurnSignal : Signal<bool> { }

public class ShowNofitySignal : Signal<string, bool> { }

public class GameStatusChangeSignal : Signal<eInGameStatus> { }

public class MapRefreshSignal : Signal { }

public class GetOccupyTileSignal : Signal<eCountry>
{
    public int OccupyTileCount { get; set; }
}

public class OccupyChangeSignal : Signal { }

public class MoveTroopSignal : Signal<Troop, int, int> { }
