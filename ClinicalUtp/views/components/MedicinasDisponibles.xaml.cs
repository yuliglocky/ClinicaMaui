using System.Collections.ObjectModel;

namespace ClinicalUtp.views.components;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;
public partial class MedicinasDisponibles : ContentView
{
    private readonly MedicinesServices _medicinesService;

    // Lista de medicinas que se mostrará en la página
    public ObservableCollection<MedicineDto> Medicines { get; set; }
    private int _medicineId;
   
    public MedicinasDisponibles(MedicinesServices medicinesServices)
	{
		InitializeComponent();
        // Asignar el servicio inyectado

        // Asignar el servicio inyectado
        _medicinesService = new MedicinesServices (new HttpClient());
        // Inicializamos la colección de medicinas
        Medicines = new ObservableCollection<MedicineDto>();

        // Establecemos el BindingContext para enlazar datos con la vista
        BindingContext = this;

        // Cargamos las medicinas disponibles
        LoadMedicines();
    }
    public string Message { get; set; }

    // Método para mostrar un mensaje de alerta
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

    // Método para cargar las medicinas disponibles

    private async void LoadMedicines()
    {
        try
        {
            // Obtener las medicinas desde el servicio
            var medicines = await _medicinesService.GetMedicinesAsync();

            if (medicines == null || !medicines.Any())
            {
                await ShowAlert("Aviso", "No se encontraron medicinas disponibles.", "OK");
                return;
            }

            // Limpiar la colección antes de agregar nuevos elementos
            Medicines.Clear();

            // Crear una lista para almacenar los mensajes de alerta
            List<string> lowQuantityMessages = new List<string>();

            // Agregar las medicinas a la colección observable
            foreach (var medicine in medicines)
            {
                Medicines.Add(medicine);

                // Verificar si la cantidad de la medicina está baja (en 10 o menos)
                if (medicine.Quantity <= 10)
                {
                    string message = $"{medicine.Name} está por agotarse. Quedan solo {medicine.Quantity} unidades disponibles.";
                    lowQuantityMessages.Add(message);
                }
            }

            // Si hay mensajes de cantidad baja, mostrar un solo mensaje con todos los mensajes
            if (lowQuantityMessages.Any())
            {
                string allMessages = string.Join("\n", lowQuantityMessages);
                await ShowAlert("Cantidad Baja", allMessages, "OK");
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Error", $"Error al cargar las medicinas: {ex.Message}", "OK");
        }
    }
}
