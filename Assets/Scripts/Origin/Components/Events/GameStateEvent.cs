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
        GAME_OVER,
        PLAY
    }
}