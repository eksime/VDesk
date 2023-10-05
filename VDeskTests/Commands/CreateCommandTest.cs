using McMaster.Extensions.CommandLineUtils;
using Pose;
using WindowsDesktop;
using VDesk.Commands;

namespace VDeskTests.Commands
{
    public class CreateCommandTest : TestingContext<CreateCommand>
    {
        [Fact]
        public void OnExecute_TwoTime_Ok()
        {
            // Arrange
            var virtualDesktop = Fixture.CreateMany<VirtualDesktop>();
            var getDesktopsShim = Shim.Replace(() => VirtualDesktop.GetDesktops()).With(delegate() { return virtualDesktop; }); 
            
            // Act
            PoseContext.Isolate(() =>
            {
                var command = new CreateCommand
                {
                    Number = 20

                };
                command.OnExecute(new CommandLineApplication());

            }, getDesktopsShim);
        }
    }
}