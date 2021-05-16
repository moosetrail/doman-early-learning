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
            await _manager.CreateNewProgram(
                new[] {_childPopulator.Zacharias.Id, _childPopulator.Jacqueline.Id, _childPopulator.Dominique.Id},
                _fakeUserId);

            await _manager.CreateNewProgram(new[] {_childPopulator.Phoenix.Id}, _fakeUserId);
        }
    }
}