namespace Moosetrail.EarlyLearning.Dataclasses.BasicProgram
{
    public class Activity
    {
        public Activity(string name, int dailyGoal, int activityGoal, string childId, bool isActive = true)
        {
            Name = name;
            CurrentDailyGoal = dailyGoal;
            ActivityGoal = activityGoal;
            ChildId = childId;
            IsActive = isActive;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public int CurrentDailyGoal { get; private set; }

        public int ActivityGoal { get; private set; }

        public string ChildId { get; private set; }

        public bool IsActive { get; private set; }

        public int UpdateDailyGoal(int newGoal)
        {
            CurrentDailyGoal = newGoal;
            return CurrentDailyGoal;
        }

        public int UpdateActivityGoal(int goal)
        {
            ActivityGoal = goal;
            return ActivityGoal;
        }
    }
}