using Systems.Game;

namespace Components.Events
{
    public class GameStateEvent
    {
        public GameState State;
    }
    
    public enum GameState
    {
        PAUSE,
        PLAY,
        NOT_INTERACTIVE
    }
}