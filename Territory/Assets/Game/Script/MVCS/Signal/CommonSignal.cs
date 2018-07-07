using strange.extensions.signal.impl;


public class FrameSignal : Signal { }

public class StartupSignal : Signal { }

public class PopupUISignal : Signal<eUI> { }

public class MakeTroopSignal : Signal<eTroopType> { }

public class EndTurnSignal : Signal<bool> { }
