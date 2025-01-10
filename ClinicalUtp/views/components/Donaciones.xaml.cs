using System.Collections.ObjectModel;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;

namespace ClinicalUtp.views.components;

public partial class Donaciones : ContentView
{
	private int _userId;
    private readonly PacienteServices _pacienteServices;
    public ObservableCollection<DonationDto> Donations { get; set; } = new();
    public Donaciones(int userId)
    {
        _userId = userId;
        InitializeComponent();
        _pacienteServices = new PacienteServices(new HttpClient());
        Donations = new ObservableCollection<DonationDto>();
        this.BindingContext = this; // Establece el contexto de datos

        LoadDonations(_userId);

    }

    public async Task LoadDonations(int userId)
    {
        try
        {
            // Llama al servicio para obtener las donaciones
            var donations = await _pacienteServices.GetDonation(_userId);

            // Limpia y actualiza la lista observable
            Donations.Clear();
            foreach (var donation in donations)
            {
                Donations.Add(donation);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar las donaciones: {ex.Message}");
            // Opcional: muestra un mensaje al usuario
            await Application.Current.MainPage.DisplayAlert("Error", "Usted no es usuario donador .", "OK");
        }
    }
}