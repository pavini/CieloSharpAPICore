namespace Cielo.Core.Configurations
{
    public interface IEnvironment
    {
        string TransactionUrl { get; }
        string QueryUrl { get; }
    }
}
