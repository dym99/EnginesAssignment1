interface ICommand {
    void execute(params object[] args);
    void undo();
    void redo();
}