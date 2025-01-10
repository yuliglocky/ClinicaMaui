using System.Collections.ObjectModel;
using ClinicalUtp.Models;
using ClinicalUtp.Controllers;
using Microsoft.Maui;
using ClinicalUtp.viewModels;
using System.ComponentModel.Design;



namespace ClinicalUtp.views.components;


public partial class PerfilViews : ContentView
{

    public ObservableCollection<ReferencesDto> References { get; set; } // Colección para las referencias
    private readonly referencesViewModel _viewModel;
    public ObservableCollection<UserDto> Doctor { get; set; }
    private int _userId; // ID del usuario
    private readonly AppointmentsServices _appointmentService;
    private int doctorId; // Variable para almacenar el DoctorId
    public PerfilViews(AppointmentsServices appointmentService, int userId)

	{
        _appointmentService = new AppointmentsServices(new HttpClient());
        _userId = userId;   
		InitializeComponent();
        LoadDoctor(_userId);
        Doctor = new ObservableCollection<UserDto>();
        References = new ObservableCollection<ReferencesDto>();
        LoadReferences(_userId);

        BindingContext = this;



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

                var doctorName = Doctor.FirstOrDefault()?.Name;
                var dni = Doctor.FirstOrDefault()?.Dni;
                var cellphone = Doctor.FirstOrDefault()?.Cellphone;
                var address = Doctor.FirstOrDefault()?.Address;
                var email = Doctor.FirstOrDefault()?.Email;

                // Asignar valores a los Labels
                UserNameLabel.Text = $"Doctor: {doctorName}";
                DniLabel.Text = $"DNI: {dni}";
                CellphoneLabel.Text = $"Teléfono: {cellphone}";
                AddressLabel.Text = $"Dirección: {address}";
                EmailLabel.Text = $"Correo Electrónico: {email}";

            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }

    private async void OnGetReferencesButtonClicked(object sender, EventArgs e)
    {
        var result = await _appointmentService.GetReferencesAndSendEmail(_userId);

        // Mostrar el mensaje en un Label o algún otro control
        if (result.IsSuccess)
        {
            SuccessLabel.Text = result.Message;
            SuccessLabel.TextColor = Colors.Green; // Puedes cambiar el color o estilo
        }
        else
        {
            SuccessLabel.Text = result.Message;
            SuccessLabel.TextColor = Colors.Red; // Error en la solicitud
        }
    }



    private async void LogOut(object sender, EventArgs e)
    {
        
            await Application.Current.MainPage.Navigation.PushAsync(new ClinicalUtp.MainPage());
    }

    private async void LoadReferences(int userId)
    {
        try
        {
            // Llamada al servicio para obtener las referencias por doctorId
            var references = await _appointmentService.GetReferencesByDoctorIdAsync(userId);

            if (references != null && references.Any())
            {
                // Limpiar la lista actual y agregar las nuevas referencias
                References.Clear();
                foreach (var reference in references)
                {
                    References.Add(reference);
                }
            }
            else
            {
                // Si no se encuentran referencias, podrías agregar un mensaje
            }
        }
        catch (Exception ex)
        {
            // Manejo de errores
            Console.WriteLine($"Error al cargar las referencias: {ex.Message}");
        }
    }
}