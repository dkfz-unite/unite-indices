namespace Unite.Indices.Services.Configuration.Options;

public interface IElasticOptions
{
    string Host { get; }
    string User { get; }
    string Password { get; }
}
