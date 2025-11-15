using AutoTyper.UI.Models;
using AutoTyper.UI.Services;

namespace AutoTyper.UI.Tests;

//This attribute generates tests for MainWindowViewModel that
//asserts all constructor arguments are checked for null
[ConstructorTests(typeof(MainWindowViewModel))]
public partial class MainWindowViewModelTests
{
    [Fact]
    public async Task AddSnippetCommand_AddsSnippetToCollection()
    {
        //Arrange
        AutoMocker mocker = new();
        mocker.GetMock<SnippetStorageService>()
            .Setup(x => x.LoadSnippetsAsync())
            .ReturnsAsync(new List<Snippet>());

        MainWindowViewModel viewModel = mocker.CreateInstance<MainWindowViewModel>();
        
        // Wait for initialization
        await Task.Delay(100);

        int initialCount = viewModel.Snippets.Count;

        Snippet testSnippet = new()
        {
            Name = "Test",
            Content = "Test content",
            Delay = 1.0,
            FastTyping = false
        };

        viewModel.Snippets.Add(testSnippet);

        //Assert
        Assert.Equal(initialCount + 1, viewModel.Snippets.Count);
        Assert.Contains(testSnippet, viewModel.Snippets);
    }

    [Fact]
    public async Task DeleteSnippetCommand_RemovesSnippetFromCollection()
    {
        //Arrange
        AutoMocker mocker = new();
        Snippet testSnippet = new()
        {
            Name = "Test",
            Content = "Test content",
            Delay = 1.0,
            FastTyping = false
        };

        mocker.GetMock<SnippetStorageService>()
            .Setup(x => x.LoadSnippetsAsync())
            .ReturnsAsync(new List<Snippet> { testSnippet });

        MainWindowViewModel viewModel = mocker.CreateInstance<MainWindowViewModel>();

        // Wait for initialization
        await Task.Delay(100);

        int initialCount = viewModel.Snippets.Count;

        //Act
        await viewModel.DeleteSnippetCommand.ExecuteAsync(testSnippet);

        //Assert
        Assert.Equal(initialCount - 1, viewModel.Snippets.Count);
        Assert.DoesNotContain(testSnippet, viewModel.Snippets);
    }

    [Fact]
    public async Task IsTopMost_PropertyChanges_UpdatesValue()
    {
        //Arrange
        AutoMocker mocker = new();
        mocker.GetMock<SnippetStorageService>()
            .Setup(x => x.LoadSnippetsAsync())
            .ReturnsAsync(new List<Snippet>());

        MainWindowViewModel viewModel = mocker.CreateInstance<MainWindowViewModel>();

        //Act
        viewModel.IsTopMost = true;

        //Assert
        Assert.True(viewModel.IsTopMost);

        //Act
        viewModel.IsTopMost = false;

        //Assert
        Assert.False(viewModel.IsTopMost);
    }
}