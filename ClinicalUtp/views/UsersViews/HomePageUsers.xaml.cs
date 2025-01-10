using System.Collections.ObjectModel;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;
using ClinicalUtp.viewModels;
using ClinicalUtp.views.components;

namespace ClinicalUtp.views.UsersViews;

public partial class HomePageUsers : ContentPage
{
    private readonly PacienteServices _appointmentService;

    private readonly ServicesViewModel _servicesViewModel;  
    public ObservableCollection<AppointmentDto> Appointments { get; set; }
    private int _userId;
    private Button _selectedButton;
    public HomePageUsers(int userId)
	{

        _userId = userId;
		InitializeComponent();

        HomeContent();
      
    }

    private void OnMenuButtonClicked(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = !FlyoutMenu.IsVisible;
        if (sender is Button clickedButton)
        {
            // Restablece el color del botón previamente seleccionado (si existe).
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Colors.Transparent; // Color por defecto
            }

            // Cambia el color del botón actualmente seleccionado.
            clickedButton.BackgroundColor = Color.FromArgb("#198B89"); // Color del botón activo.
            _selectedButton = clickedButton; // Actualiza el botón seleccionado.
        }

    }


    private void Inicio(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = !FlyoutMenu.IsVisible;
        if (sender is Button clickedButton)


        {
            HomeContent();
            // Restablece el color del botón previamente seleccionado (si existe).
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Colors.Transparent; // Color por defecto
            }

            // Cambia el color del botón actualmente seleccionado.
            clickedButton.BackgroundColor = Color.FromArgb("#198B89"); // Color del botón activo.
            _selectedButton = clickedButton; // Actualiza el botón seleccionado.
        }

    }

    private async void LogOut(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new ClinicalUtp.MainPage());

    }



    //para llamar la vista de mis donaciones
    private void   DonacionesViews(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = !FlyoutMenu.IsVisible;
        if (sender is Button clickedButton)


        {
            DonacionesView();
            // Restablece el color del botón previamente seleccionado (si existe).
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Colors.Transparent; // Color por defecto
            }

            // Cambia el color del botón actualmente seleccionado.
            clickedButton.BackgroundColor = Color.FromArgb("#198B89"); // Color del botón activo.
            _selectedButton = clickedButton; // Actualiza el botón seleccionado.
        }

    }


   
    private void DonacionesView()
    {
        var appointmentsContentView = new Donaciones(_userId);

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }


    //para ver mis referencias
    private void ReferenciasViews(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = !FlyoutMenu.IsVisible;
        if (sender is Button clickedButton)


        {
            MisReferencias();
            // Restablece el color del botón previamente seleccionado (si existe).
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Colors.Transparent; // Color por defecto
            }

            // Cambia el color del botón actualmente seleccionado.
            clickedButton.BackgroundColor = Color.FromArgb("#198B89"); // Color del botón activo.
            _selectedButton = clickedButton; // Actualiza el botón seleccionado.
        }

    }


    //para ver mis referencias
    private void ContactoV(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = !FlyoutMenu.IsVisible;
        if (sender is Button clickedButton)


        {
            contacto();
            // Restablece el color del botón previamente seleccionado (si existe).
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Colors.Transparent; // Color por defecto
            }

            // Cambia el color del botón actualmente seleccionado.
            clickedButton.BackgroundColor = Color.FromArgb("#198B89"); // Color del botón activo.
            _selectedButton = clickedButton; // Actualiza el botón seleccionado.
        }

    }


    private void MisReferencias()
    {
        var appointmentsContentView = new MisReferencias(_userId);

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }


    private void contacto()
    {
        var appointmentsContentView = new ContactoView();

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }



    private void AppointmentRegister(object sender, EventArgs e)
    {

        FlyoutMenu.IsVisible = !FlyoutMenu.IsVisible;
        // FlyoutMenu.IsVisible = false; 
        if (sender is Button clickedButton)
        {
            OnAppointmentView();

            // Restablece el color del botón previamente seleccionado (si existe).
            if (_selectedButton != null)
            {
                _selectedButton.BackgroundColor = Colors.Transparent; // Color por defecto
            }

            // Cambia el color del botón actualmente seleccionado.
            clickedButton.BackgroundColor = Color.FromArgb("#198B89"); // Color del botón activo.
            _selectedButton = clickedButton; // Actualiza el botón seleccionado.
        }
    }

    private void OnAppointmentView()
    {
        var appointmentsContentView = new Appointments(_userId);

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }


    private void HomeContent()
    {
        var appointmentsContentView = new HomeContentUser(_userId);

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }
    private void OnCloseMenuButtonClicked(object sender, EventArgs e)
    {
        FlyoutMenu.IsVisible = false;
    }
}