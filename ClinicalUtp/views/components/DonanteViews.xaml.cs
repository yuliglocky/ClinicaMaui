namespace ClinicalUtp.views.components;
using ClinicalUtp.Models;
using ClinicalUtp.Controllers;
using ClinicalUtp.ViewModels;
using System.Collections.ObjectModel;

public partial class DonanteViews : ContentView
{

    private readonly PacienteServices _donationService;



    public ObservableCollection<UserDto> Pacientes { get; set; }

    public DonanteViews()
	{
		InitializeComponent();

        BindingContext = new DonacionViewModel(new PacienteServices(new HttpClient()));
    }


    public string Message { get; set; }

    // Método para mostrar un mensaje
    private async Task<bool> ShowAlert(string title, string message, string accept, string cancel = null)
    {
        if (Application.Current.MainPage != null)
        {
            if (cancel == null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, accept);
                return true;
            }
            else
            {
                return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
            }
        }
        return false;
    }

    // Método que se ejecuta al hacer clic en el botón de añadir donación
    private async void OnAddDonationClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as DonacionViewModel;

        // Validar que haya un donante seleccionado
        if (viewModel?.SelectedDonante == null)
        {
            await ShowAlert("Error", "Por favor, selecciona un donante de la lista.", "OK");
            return;
        }

        // Mostrar confirmación antes de registrar la donación
        bool isConfirmed = await ShowAlert(
            "Confirmación",
            $"¿Quieres registrar la donación para este donante {viewModel.SelectedDonante.Name} en la fecha  {DateTime.Now:dd/MM/yyyy}?",
            "Sí",
            "No"
        );

        if (!isConfirmed)
            return;

        try
        {
            // Crear un objeto de donación
            var donation = new DonationDto
            {
                UserId = viewModel.SelectedDonante.Id,
                CreatedAt = DateTime.Now
            };

            // Llamar al método para añadir la donación
            await viewModel.AddDonationAsync(donation);

            // Mostrar mensaje de éxito
            await ShowAlert("Éxito", $"Donación registrada con éxito para el usuario {viewModel.SelectedDonante.Name} ", "OK");
        }
        catch (Exception ex)
        {
            // Manejar errores y mostrar mensaje
            await ShowAlert("Error", $"Hubo un problema al registrar la donación: {ex.Message}", "OK");
        }
    }
}
