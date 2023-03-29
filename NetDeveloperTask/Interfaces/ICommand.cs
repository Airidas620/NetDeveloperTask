namespace NetDeveloperTask.Interfaces
{
    public interface ICommand
    {
        void Execute(ICommandService commandService);
    }
}
