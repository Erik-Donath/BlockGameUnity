public interface IModel {
    public bool IsPrimitv { get; }
}

public class BlockStates {
    public IModel DefaultModel {
    get; private set; }

    public BlockStates(IModel defaultModel) {
        DefaultModel = defaultModel;
    }
}
