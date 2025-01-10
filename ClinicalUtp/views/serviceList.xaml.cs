

using ClinicalUtp.viewModels;

namespace ClinicalUtp.views;

public partial class serviceList : ContentPage
{
	public serviceList()
	{
		InitializeComponent();
        BindingContext = new ServicesViewModel();
    }

   
}