public interface IState
{
    void OnEnter(Bee bee);
    void OnExecute(Bee bee);
    void OnExit(Bee bee);
}
