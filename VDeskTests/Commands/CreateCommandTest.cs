using McMaster.Extensions.CommandLineUtils;
using VDesk.Commands;
using VDesk.Services;
using VDesk.Wrappers;

namespace VDeskTests.Commands
{
    public class CreateCommandTest : TestingContext<CreateCommand>
    {
        [Fact]
        public void OnExecute_TwoTime_Ok()
        {
            // Arrange
            var moqArray = new List<IVirtualDesktop>();
            GetMockFor<IVirtualDesktopService>().Setup(s => s.GetDesktops()).Returns(moqArray.ToArray).Callback(() =>
            {
                moqArray.Add(new Mock<IVirtualDesktop>().Object);
            });
            var commandLineApp = new CommandLineApplication();
            var command = new CreateCommand(GetMockFor<IVirtualDesktopService>().Object)
            {
                Number = 5
            };

            // Act
            command.OnExecute(commandLineApp);

            // Assert
            GetMockFor<IVirtualDesktopService>().Verify(s => s.Create(), Times.Exactly(5));
            
        }
    }
}