
namespace RecoilNet.State
{
    public interface IRecoilStore : IDisposable
    {
        int Id { get; }

        Task<object?> GetAsync(Primitive primitive, CancellationToken cancellationToken = default);
        void Set(Primitive primitive, object? value);
    }
}