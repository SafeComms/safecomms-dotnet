namespace SafeComms.Client.Tests;

public class SafeCommsClientTests
{
    [Fact]
    public void Constructor_CanInstantiate()
    {
        var client = new SafeCommsClient("test-key");
        Assert.NotNull(client);
    }
}
