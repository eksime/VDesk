using AutoFixture;
using McMaster.Extensions.CommandLineUtils;
using VDesk.Commands;
using VDesk.Services;
using VDesk.Wrappers;

namespace VDeskTests.Commands
{
    public class CreateCommandTest : TestingContext<CreateCommand>
    {
        [Fact]
        public void OnExecute_WhenThreeVirtualDesktop_ShouldCallCreateTwoTimes()
        {
            // Arrange
            var moqArray = new[]{new Mock<IVirtualDesktop>().Object, new Mock<IVirtualDesktop>().Object, new Mock<IVirtualDesktop>().Object};
            GetMockFor<IVirtualDesktopService>().Setup(s => s.GetDesktops()).Returns(moqArray);
            var commandLineApp = new CommandLineApplication();
            var command = new CreateCommand(GetMockFor<IVirtualDesktopService>().Object)
            {
                Number = 5
            };

            // Act
            command.OnExecute(commandLineApp);

            // Assert
            GetMockFor<IVirtualDesktopService>().Verify(s => s.Create(), Times.Exactly(2));
        }
    }
}