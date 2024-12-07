namespace SignalRDemo.Helpers
{
  public class ServiceScopeFactory : IServiceScopeFactory
  {
    private IServiceScopeFactory _serviceScopeFactory;
    public ServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
    {
      _serviceScopeFactory = serviceScopeFactory;
    }
    public virtual IServiceScope CreateScope()
    {
      return _serviceScopeFactory.CreateScope();
    }
  }
}