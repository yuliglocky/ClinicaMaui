using System.Diagnostics;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;

namespace ClinicalUtp.views;

public partial class addAppointments : ContentPage
{

    private readonly PacienteServices _pacienteServices;
    private int _userId;

    public addAppointments(int userId)
	{
		InitializeComponent();

        _userId = userId;
        _pacienteServices = new PacienteServices(HttpClientProvider.Client); // Usa HttpClientProvider aquí

        // Cargar la lista de doctores al abrir la página
        LoadDoctorsAsync();
    }
    private async Task LoadDoctorsAsync()
    {
        try
        {
            var doctors = await _pacienteServices.GetDoctorsAsync();

            if (doctors != null && doctors.Any())
            {
                DoctorPicker.ItemsSource = doctors.ToList(); // Convierte a List antes de asignar
                DoctorPicker.ItemDisplayBinding = new Binding("Servicio"); // Usa 'Name' si esa es la propiedad de tu DTO
            }
            else
            {
                await DisplayAlert("Información", "No se encontraron doctores.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al cargar doctores: {ex.Message}");
            await DisplayAlert("Error", "No se pudieron cargar los doctores.", "OK");
        }
    }


    private async void OnCreateAppointmentClicked(object sender, EventArgs e)
    {
        // Verifica que haya un doctor seleccionado y un motivo para la cita
        if (DoctorPicker.SelectedItem is DoctorDto selectedDoctor && !string.IsNullOrWhiteSpace(ReasonEntry.Text))
        {
            // Preparar los detalles de la cita
            string reason = ReasonEntry.Text;
            DateTime appointmentDate = DateTime.Now.AddDays(2);
            string notes = "Notas adicionales de la cita";

            // Mensaje de confirmación
            string confirmationMessage = $"Confirmar la creación de la cita con los siguientes datos:\n" +
                                         $"UserId: {_userId}\n" +
                                         $"DoctorId: {selectedDoctor.Id} \n" +
                                         $"Motivo: {reason}\n" +
                                         $"Fecha de Cita: {appointmentDate}\n" +
                                         $"Notas: {notes}";

            bool isConfirmed = await DisplayAlert("Confirmación de Cita", confirmationMessage, "Confirmar", "Cancelar");

            if (isConfirmed)
            {
                try
                {
                    // Crear el DTO con los datos confirmados
                    var appointmentCreateDto = new AppointmentDto
                    {
                        UserId = _userId,
                        DoctorId = selectedDoctor.Id,
                        Reason = reason,
                        AppointmentDate = appointmentDate,
                        Notes = notes
                    };

                    // Llama al servicio para crear la cita
                    var appointmentDto = await _pacienteServices.CreateAppointmentAsync(appointmentCreateDto);

                    if (appointmentDto != null)
                    {
                        Debug.WriteLine($"Cita creada con éxito. ID de la cita: {appointmentDto.Id}");
                        await DisplayAlert("Éxito", "Cita creada correctamente", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo crear la cita.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al crear la cita: {ex.Message}");
                    await DisplayAlert("Error", "Ocurrió un error al crear la cita.", "OK");
                }
            }
        }
        else
        {
            await DisplayAlert("Advertencia", "Selecciona un doctor y completa el motivo de la cita.", "OK");
        }
    }
}
