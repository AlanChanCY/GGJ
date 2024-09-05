using System.Collections;
using System.Collections.Generic;

public enum GameState { Start = 0, OnGoing = 1, Paused = 2, End = 3}

// defines which corner of the current background the camera is above
public enum CameraBGPos { TopLeft = 0b00, TopRight = 0b01, BottomLeft = 0b10, BottomRight = 0b11}