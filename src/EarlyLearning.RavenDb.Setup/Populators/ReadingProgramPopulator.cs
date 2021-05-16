using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms;

namespace EarlyLearning.RavenDb.Setup.Populators
{
    internal class ReadingProgramPopulator
    {
        private readonly ReadingProgramManager _manager;
        private readonly ChildPopulator _childPopulator;
        private readonly string _fakeUserId;

        public ReadingProgramPopulator(ReadingProgramManager manager, ChildPopulator childPopulator, string fakeUserId)
        {
            _manager = manager;
            _childPopulator = childPopulator;
            _fakeUserId = fakeUserId;
        }

        public async Task Run()
        {
            var program1Children = new[]
                {_childPopulator.Zacharias.Id, _childPopulator.Jacqueline.Id, _childPopulator.Dominique.Id};

            if(!await _manager.ReadingProgramExistsFor(program1Children))
                await _manager.CreateNewProgram(program1Children,
                _fakeUserId);

            var phoenixProgram = new[] {_childPopulator.Phoenix.Id};

            if(!await _manager.ReadingProgramExistsFor(phoenixProgram))
                await _manager.CreateNewProgram(phoenixProgram, _fakeUserId);
        }
    }
}