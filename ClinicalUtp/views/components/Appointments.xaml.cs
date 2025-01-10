namespace ClinicalUtp.views.components;
using System.Diagnostics;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;
public partial class Appointments : ContentView
{

    private readonly PacienteServices _pacienteServices;
    private readonly LoginServices _loginServices;
    private int _userId;
    public int SelectedDoctorId { get; private set; }
    private List<DoctorDto> _doctors;
    private List<ServicioDto> _servicio;

    public Appointments(int userId)
	{
		InitializeComponent();
        _userId = userId;
        _pacienteServices = new PacienteServices(new HttpClient());
        LoadDoctors();
        LoadServicio();

        SelectedDoctorId = -1;
    }


  
    private async void LoadDoctors()
    {
        try
        {
            _doctors = await _pacienteServices.GetDoctorsBasicAsync();
            if (_doctors != null && _doctors.Count > 0)
            {
                foreach (var doctor in _doctors)
                {
                    doctorPicker.Items.Add(doctor.Name); // Añade solo el nombre al Picker
                }
            }
            else
            {
                await ShowAlert("Información", "No se encontraron doctores.", "OK");
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Error", $"Ocurrió un error al cargar los doctores: {ex.Message}", "OK");
        }
    }

    private async void LoadServicio()
    {
        try
        {
            _servicio = await _pacienteServices.GetServiciosAsync();
            if (_servicio != null && _servicio.Count > 0)
            {
                // Asegúrate de que el Picker para servicios esté configurado si usas uno aparte
                foreach (var servicio in _servicio)
                {
                    // Lógica para cargar servicios en el Picker
                }
            }
            else
            {
                await ShowAlert("Información", "No se encontraron servicios.", "OK");
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Error", $"Ocurrió un error al cargar los servicios: {ex.Message}", "OK");
        }
    }

    private void OnDoctorSelectedIndexChanged(object sender, EventArgs e)
    {
        if (doctorPicker.SelectedIndex != -1)
        {
            var selectedDoctor = _doctors[doctorPicker.SelectedIndex];
            if (selectedDoctor != null)
            {
                SelectedDoctorId = selectedDoctor.Id;
                Console.WriteLine($"Doctor seleccionado ID: {SelectedDoctorId}");
            }
        }
    }

    private async void OnCreateAppointmentClicked(object sender, EventArgs e)
    {
        if (SelectedDoctorId != -1 && ReasonPicker.SelectedItem != null)
        {
            string reason = ReasonPicker.SelectedItem.ToString(); // Obtiene el texto del motivo
            DateTime appointmentDate = DateTime.Now.AddDays(2); // Establece la fecha de la cita
            string notes = notesEditor.Text; // Obtiene las notas adicionales

            string confirmationMessage = $"Confirmar la creación de la cita con los siguientes datos:\n" +
                                        
                                         $"Motivo: {reason}\n" +
                                         $"Fecha de Cita: {appointmentDate}\n" +
                                         $"Notas: {notes}";

            bool isConfirmed = await ShowAlert("Confirmación de Cita", confirmationMessage, "Confirmar", "Cancelar");

            if (isConfirmed)
            {
                try
                {
                    var appointmentCreateDto = new AppointmentDto
                    {
                        UserId = _userId,
                        DoctorId = SelectedDoctorId,
                        Reason = reason,
                        AppointmentDate = appointmentDate,
                        Notes = notes
                    };

                    var appointmentDto = await _pacienteServices.CreateAppointmentAsync(appointmentCreateDto);

                    if (appointmentDto != null)
                    {
                        Debug.WriteLine($"Cita creada con éxito. ID de la cita: {appointmentDto.Id}");
                        await ShowAlert("Éxito", "Cita creada correctamente", "OK");

                        // Limpia los datos después de crear la cita
                        ClearAppointmentForm();
                    }
                    else
                    {
                        await ShowAlert("Error", "No se pudo crear la cita.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al crear la cita: {ex.Message}");
                    await ShowAlert("Error", "Ocurrió un error al crear la cita.", "OK");
                }
            }
        }
        else
        {
            await ShowAlert("Advertencia", "Selecciona un doctor y completa el motivo de la cita.", "OK");
        }
    }

    private void ClearAppointmentForm()
    {
        // Reinicia el Picker de doctores
        doctorPicker.SelectedIndex = -1;
        SelectedDoctorId = -1;

        // Reinicia el Picker de motivos
        ReasonPicker.SelectedIndex = -1;

        // Limpia el texto del Editor de notas
        notesEditor.Text = string.Empty;

        // Opcional: Mensaje en consola para depuración
        Debug.WriteLine("Formulario de citas limpiado.");
    }

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
}