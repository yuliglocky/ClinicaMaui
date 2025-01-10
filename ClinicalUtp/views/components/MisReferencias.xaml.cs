
using System.ComponentModel.Design;
using System.Collections.ObjectModel;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;

namespace ClinicalUtp.views.components;

public partial class MisReferencias : ContentView
{

    private readonly AppointmentsServices _referenceService;
    private int _userId;


    // Lista de citas que se mostrará en la página
    public ObservableCollection<ReferencesDto> References { get; set; }
    private int _appointmentId; // ID de la cita
    public MisReferencias(int userId)
	{

        _referenceService = new AppointmentsServices(new HttpClient());
        References = new ObservableCollection<ReferencesDto>();
        BindingContext = this;
        _userId =userId; 
		InitializeComponent();

        LoadReferences(_userId);    
	}
    // Método para cargar las referencias de un usuario
    public async Task LoadReferences(int userId)
    {
        try
        {
            var references = await _referenceService.GetReferencesByUserIdAsync(_userId);
            References.Clear();

            if (references != null)
            {
                foreach (var reference in references)
                {
                    References.Add(reference);
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Usted no tiene referencias", "OK");
        }
    }

  private async void OnSendPdfClicked(object sender, EventArgs e)
{
    if (sender is Button button && button.BindingContext is ReferencesDto reference)
    {
        // Usa el _userId en lugar de extraer el userId del objeto reference
        var referenceId = reference.ReferenceId;

        // Llama al servicio para enviar el PDF usando el _userId
        var resultMessage = await _referenceService.SendReferencePdfAsync(_userId, referenceId);

        // Muestra el resultado al usuario
        await Application.Current.MainPage.DisplayAlert("Resultado", "El pdf fue enviado a su corre correctamente", "OK");

            await LoadReferences(_userId);
        }
}
}

