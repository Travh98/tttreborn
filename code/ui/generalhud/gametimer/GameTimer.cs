using Sandbox.UI;
using Sandbox.UI.Construct;

using TTTReborn.Gamemode;

namespace TTTReborn.UI
{
    public class GameTimer : TTTPanel
    {
        private GameTimerContent _gameTimerContent;

        public GameTimer()
        {
            StyleSheet.Load("/ui/generalhud/gametimer/GameTimer.scss");

            _gameTimerContent = new GameTimerContent(this);
        }

        private class GameTimerContent : TTTPanel
        {
            private readonly Label _textLabel;
            private readonly Label _timeLabel;

            public GameTimerContent(Panel parent)
            {
                Parent = parent;

                _textLabel = Add.Label("", "textlabel");
                _timeLabel = Add.Label("", "timelabel");
            }
            public override void Tick()
            {
                base.Tick();

                if (Game.Instance.Round == null)
                {
                    return;
                }

                bool isWaitingRound = Game.Instance.Round is Rounds.WaitingRound;

                _textLabel.Text = $"{Game.Instance.Round.RoundName}:";
                _timeLabel.Text = isWaitingRound ? "" : $"{Game.Instance.Round.TimeLeftFormatted}";

                _timeLabel.SetClass("hide", isWaitingRound);
            }
        }
    }
}
