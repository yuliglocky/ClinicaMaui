namespace ClinicalUtp.views;
using ClinicalUtp.Models;
using ClinicalUtp.Controllers;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;


public partial class AppointmentViews : ContentPage
{

    private readonly MedicinesServices _medicinesService;
    public ObservableCollection<MedicineDto> Medicines { get; set; }
    private int _medicineId;
    private string _medicineName;
    private int _quantity;
    private readonly AppointmentsServices _appointmentService;
    public ObservableCollection<UserDto> Doctor { get; set; }
    public ObservableCollection<AppointmentDto> AppointmentDetails { get; set; }
    private int _appointmentId; // ID de la cita
    private int _userId; // ID del usuario
    private string _userName; // Nombre del usuario

    private int doctorId; // Variable para almacenar el DoctorId
    public AppointmentViews(AppointmentsServices appointmentService, int appointmentId, int userId)
    {
        InitializeComponent();
       
        _userId = userId;
        _medicinesService = new MedicinesServices(new HttpClient());
        Medicines = new ObservableCollection<MedicineDto>();
        _appointmentId = appointmentId;
        _appointmentService = new AppointmentsServices(new HttpClient());
        AppointmentDetails = new ObservableCollection<AppointmentDto>();
        BindingContext = this;
        LoadAppointmentDetails(appointmentId);
        LoadMedicines();
        LoadDoctor(_userId);
        Doctor = new ObservableCollection<UserDto>();

    }


    private async void LoadDoctor(int userId)
    {
        try
        {
            // Llamada al servicio para obtener el usuario
            var user = await _appointmentService.GetDoctorByIdAsync(userId);

            if (user != null)
            {
                Doctor.Clear(); // Limpiar la colección antes de agregar el nuevo doctor
                Doctor.Add(user); // Agregar el doctor a la colección

                doctorId = user.Id;
                // Asignar el nombre del usuario desde la colección
                var doctorName = Doctor.FirstOrDefault()?.Name;
                UserNameLabel.Text = $"Doctor: {doctorName}"; // Mostrar en la vista
            }
            else
            {
                await DisplayAlert("Aviso", "No se encontró información del usuario.", "OK");
                await Navigation.PopModalAsync(); // Regresar si no hay datos del usuario
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al cargar el usuario: {ex.Message}", "OK");
        }
    }
    private async void OnClosePage(object sender, EventArgs e)
    {
        // Primero, cierra la página modal

            // Llamar directamente a RefreshAppointmentContent para actualizar el ContentView
            if (Application.Current.MainPage is NavigationPage navPage &&
                navPage.CurrentPage is HomePage homePage)
            {
                homePage.RefreshAppointmentContent();
            }


           await Navigation.PopModalAsync();

    }

    private async void LoadMedicines()
    {
        try
        {
            // Obtener las medicinas desde el servicio
            var medicines = await _medicinesService.GetMedicinesAsync();

            if (medicines == null || !medicines.Any())
            {
                return;
            }

            Medicines.Clear();
            foreach (var medicine in medicines)
            {
                Medicines.Add(medicine);
            }

            // Asignar las medicinas al Picker
            MedicinesPicker.ItemsSource = Medicines;
        }
        catch (Exception ex)
        {
            // Manejo de errores
        }
    }

    private async void LoadAppointmentDetails(int appointmentId)
    {
        try
        {
            // Llamada al servicio para obtener los detalles de la cita
            var appointment = await _appointmentService.GetAppointmentDetailsAsync(appointmentId);

            if (appointment != null)
            {
                AppointmentDetails.Clear();
                AppointmentDetails.Add(appointment);


            }
            else
            {
                await DisplayAlert("Aviso", "No se encontraron detalles para esta cita.", "OK");
                await Navigation.PopModalAsync(); // Regresamos si no hay detalles
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al cargar los detalles: {ex.Message}", "OK");
        }
    }

    public void SpecialtyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Aquí puedes manejar la selección del Picker
        var selectedSpecialty = SpecialtyPicker.SelectedItem as string;
        // Si necesitas hacer algo con la especialidad seleccionada, lo puedes hacer aquí
        Console.WriteLine($"Especialidad seleccionada: {selectedSpecialty}");
    }

    public void MedicinesPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedMedicine = MedicinesPicker.SelectedItem as MedicineDto;
        if (selectedMedicine != null)
        {
            // Asignar el ID y el nombre del medicamento seleccionado
            _medicineId = selectedMedicine.Id;
            _medicineName = selectedMedicine.Name; // Asignar el nombre del medicamento
        }
    }
    private async void OnSubmitMedicine(object sender, EventArgs e)
    {
        try
        {
            // Validar que se haya seleccionado un medicamento
            if (_medicineId == 0)
            {
                await DisplayAlert("Error", "Por favor, seleccione un medicamento.", "OK");
                return;
            }

            // Validar que la cantidad sea un número válido y mayor que 0
            if (string.IsNullOrWhiteSpace(QuantityEntry.Text) || !int.TryParse(QuantityEntry.Text, out _quantity) || _quantity <= 0)
            {
                await DisplayAlert("Error", "Por favor, ingrese una cantidad válida para el medicamento.", "OK");
                return;
            }

            // Obtener los detalles de la cita
            var appointment = AppointmentDetails.FirstOrDefault();
            if (appointment == null)
            {
                await DisplayAlert("Error", "No se encontraron los detalles de la cita.", "OK");
                return;
            }

            // Intentar enviar los datos del medicamento
            var resultMessage = await _appointmentService.CreateAppointmentMedicineAsync(new AppointmentxMedicinesDto
            {
                AppointmentId = _appointmentId,
                MedicineId = _medicineId,
                Quantity = _quantity,
                UserId = appointment.UserId
            });

            // Mostrar el mensaje devuelto por el servicio
            await DisplayAlert("Resultado", resultMessage, "OK");
        }
        catch (Exception ex)
        {
            // Manejo de errores
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }




    // Método principal para enviar todos los datos
    private async void OnSubmitAllData(object sender, EventArgs e)
    {
        try
        {
            // Validar campos de referencia
            string reason = ReasonEntry.Text;
            string diagnosis = DiagnosisEntry.Text;
            string doctorSuggestions = DoctorSuggestionsEntry.Text;
            string doctorSpecialty = SpecialtyPicker.SelectedItem as string;
            DateTime followUpDate = FollowUpDatePicker.Date;

            if (string.IsNullOrWhiteSpace(reason) || string.IsNullOrWhiteSpace(diagnosis) ||
                string.IsNullOrWhiteSpace(doctorSuggestions) || string.IsNullOrWhiteSpace(doctorSpecialty))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos de referencia.", "OK");
                return;
            }

            // Validar campos de información médica
            if (string.IsNullOrWhiteSpace(WeightEntry.Text) ||
                string.IsNullOrWhiteSpace(HeightEntry.Text) ||
                string.IsNullOrWhiteSpace(TemperatureEntry.Text) ||
                string.IsNullOrWhiteSpace(BloodPressureEntry.Text) ||
                string.IsNullOrWhiteSpace(HeartRateEntry.Text))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos de información médica.", "OK");
                return;
            }

            var doctorName = UserNameLabel.Text.Replace("Doctor: ", "");

            decimal weight = Convert.ToDecimal(WeightEntry.Text);
            int height = Convert.ToInt32(HeightEntry.Text);
            int temperature = Convert.ToInt32(TemperatureEntry.Text);
            int bloodPressure = Convert.ToInt32(BloodPressureEntry.Text);
            int heartRate = Convert.ToInt32(HeartRateEntry.Text);

            // Obtener detalles de la cita
            var appointment = AppointmentDetails.FirstOrDefault();
            if (appointment == null)
            {
                await DisplayAlert("Error", "No se encontraron los detalles de la cita.", "OK");
                return;
            }

            // Crear resumen de todos los datos excepto los medicamentos
            string summaryMessage =
                $"**Referencia:**\n" +
                $"- Motivo: {reason}\n" +
                $"- Diagnóstico: {diagnosis}\n" +
                $"- Sugerencias: {doctorSuggestions}\n" +
                $"- Especialidad: {doctorSpecialty}\n" +
                $"- Fecha de seguimiento: {followUpDate:dd/MM/yyyy}\n\n" +
                $"**Información médica:**\n" +
                $"- Peso: {weight} kg\n" +
                $"- Altura: {height} cm\n" +
                $"- Temperatura: {temperature} °C\n" +
                $"- Presión arterial: {bloodPressure} mmHg\n" +
                $"- Frecuencia cardíaca: {heartRate} lpm\n\n" +
                $"**Información de la cita:**\n" +
                $"- UserId: {appointment.UserId}\n" +
                $"- AppointmentId: {appointment.Id}";

            // Mostrar el resumen al usuario
            bool isConfirmed = await DisplayAlert("Resumen de datos", summaryMessage, "Confirmar", "Cancelar");
            if (!isConfirmed) return;

            // Enviar los datos de referencia
            var referenceService = new AppointmentsServices(new HttpClient());
            bool referenceSuccess = await referenceService.CreateReferenceAsync(new ReferencesDto
            {
                UserId = appointment.UserId,
                DoctorId = doctorId,
                DoctorName = doctorName,
                DoctorSpecialty = doctorSpecialty,
                Reason = reason,
                Diagnosis = diagnosis,
                DoctorSuggestions = doctorSuggestions,
                FollowUpDate = followUpDate,
                IsUsed = false,
                Status = "Pendiente",
                ContactDetails = "Teléfono: 123456789"
            });

            var medicalInfoMessage = await _appointmentService.AddMedicalInformationAsync(new MedicalInformationDto
            {
                UserId = appointment.UserId,
                Weight = weight,
                Height = height,
                Temperature = temperature,
                BloodPressure = bloodPressure,
                HeartRate = heartRate
            });

            // Mostrar los resultados del envío
            await DisplayAlert("Resultados",
                $"Referencia: {(referenceSuccess ? "Creada exitosamente" : "Error al crear")}\n" +
                $"Información médica: {medicalInfoMessage}", "OK");

            var result = await _appointmentService.FinalizadaAppointment(_appointmentId);
            if (result)
            {
                // Mensaje de éxito
                await Application.Current.MainPage.DisplayAlert("Éxito", "La cita ha sido finalizada exitosamente.", "OK");
                Application.Current.MainPage = new HomePage(_userId);

            }
            else
            {
                // Mensaje de error
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el estado de la cita.", "OK");
            }


        }
        catch (Exception ex)
        {
            // Manejo de errores
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }




}