using Prism.Mvvm;

namespace MeseMe.MessageBoxModule.ViewModels
{
	public class MessageBoxViewModel : BindableBase
    {
	    private string _name;
	    private string _host;

	    public string Host
	    {
		    get => _host;
		    set
		    {
			    _host = value;
			    RaisePropertyChanged(nameof(Host));
		    }
	    }

	    public string Name
	    {
		    get => _name;
		    set
		    {
			    _name = value;
				RaisePropertyChanged(nameof(Name));
			}
	    }
    }
}
