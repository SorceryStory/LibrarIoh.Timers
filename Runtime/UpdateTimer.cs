using SorceressSpell.LibrarIoh.Math;

namespace SorceressSpell.LibrarIoh.Timers
{
    public class UpdateTimer
    {
        #region Properties

        public float ElapsedTime { private set; get; }

        public float ElapsedTimePercentage
        {
            get { return ElapsedTime / TargetTime; }
        }

        public TimerEndBehaviour EndBehaviour { set; get; }

        public bool HasReachedTargetTime
        {
            get { return ElapsedTime >= TargetTime; }
        }

        public bool IsPaused { private set; get; }

        public float SurplusTime
        {
            get { return MathOperations.Max(ElapsedTime - TargetTime, 0f); }
        }

        public float TargetTime { private set; get; }

        #endregion Properties

        #region Constructors

        public UpdateTimer(float targetTime = 0f, bool startPaused = false, TimerEndBehaviour endBehaviour = TimerEndBehaviour.KeepGoing, float startingTime = 0f)
        {
            Reset(targetTime, false, startingTime);

            IsPaused = startPaused;

            EndBehaviour = endBehaviour;
        }

        #endregion Constructors

        #region Methods

        public void Pause()
        {
            IsPaused = true;
        }

        public void Reset(float startingTime = 0f)
        {
            ElapsedTime = startingTime;
        }

        public void Reset(bool resume, float startingTime = 0f)
        {
            Reset(startingTime);
            if (resume) { Resume(); }
        }

        public void Reset(float targetTime, bool resume, float startingTime = 0f)
        {
            SetTargetTime(targetTime);
            Reset(resume, startingTime);
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void SetTargetTime(float targetTime)
        {
            TargetTime = targetTime;
        }

        public bool Update(float deltaTime)
        {
            if (!IsPaused)
            {
                ElapsedTime += deltaTime;
            }

            bool isTimerOver = !IsPaused && HasReachedTargetTime;

            if (isTimerOver)
            {
                switch (EndBehaviour)
                {
                    case TimerEndBehaviour.AutomaticReset:
                        Reset();
                        break;

                    case TimerEndBehaviour.AutomaticResetMaintainSurplus:
                        Reset(SurplusTime);
                        break;

                    case TimerEndBehaviour.Pause:
                        Pause();
                        break;

                    case TimerEndBehaviour.KeepGoing:
                    default:
                        break;
                }
            }

            return isTimerOver;
        }

        #endregion Methods
    }
}
