namespace VDeskTests;

public abstract class TestingContext<T> where T: class  
{  
    private readonly Dictionary<Type, Mock> _injectedMocks;  
    private readonly Dictionary<Type, object> _injectedConcreteClasses;  
    protected Fixture Fixture { get; }

    protected TestingContext()  
    {  
        Fixture = new Fixture();  
        Fixture.Customize(new AutoMoqCustomization());  
        _injectedMocks = new Dictionary<Type, Mock>();  
        _injectedConcreteClasses = new Dictionary<Type, object>();  
    }

    protected Mock<TMockType> GetMockFor<TMockType>() where TMockType : class  
    {  
        var existingMock = _injectedMocks.FirstOrDefault(x => x.Key == typeof(TMockType));  
        if(existingMock.Key == null)  
        {  
            var newMock = new Mock<TMockType>();  
            existingMock = new KeyValuePair<Type, Mock>(typeof(TMockType), newMock);  
            _injectedMocks.Add(existingMock.Key, existingMock.Value);  
            Fixture.Inject(newMock.Object);  
        }  
  
        return (existingMock.Value as Mock<TMockType>)!;  
    }  
      
    public void InjectClassFor<TClassType>(TClassType injectedClass) where TClassType : class  
    {  
        var existingClass = _injectedConcreteClasses
            .FirstOrDefault(x => x.Key == typeof(TClassType));  
        if (existingClass.Key != null)  
        {  
            throw new Exception($"{injectedClass.GetType().Name} has been injected more than once");  
        }  
        _injectedConcreteClasses.Add(typeof(TClassType), injectedClass);  
        Fixture.Inject(injectedClass);  
    }  
      
    public T ClassUnderTest => Fixture.Create<T>();  
}