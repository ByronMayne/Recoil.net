using RecoilNet.Utility;

namespace RecoilNet
{

    public interface IRecoilStore : IDisposable
    {
        int Id { get; }

        void AddState(RecoilState state);
        void RemoveState(RecoilState state);

        Task<TryGetResult<object?>> TryGetAsync(Primitive primitive, CancellationToken cancellationToken = default);

        Task<object?> GetAsync(Primitive primitive, CancellationToken cancellationToken = default);
        void Set(Primitive primitive, object? value);
    }
}