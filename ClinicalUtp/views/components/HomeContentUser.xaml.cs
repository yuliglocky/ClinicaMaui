using System.Collections.ObjectModel;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;

namespace ClinicalUtp.views.components;

public partial class HomeContentUser : ContentView
{
    private readonly PacienteServices _appointmentService;
    public ObservableCollection<AppointmentDto> PendingAppointments { get; set; }

    // ObservableCollection para citas aprobadas (estado 1)
    public ObservableCollection<AppointmentDto> ApprovedAppointments { get; set; }
    private int _userId;
    public HomeContentUser(int userId)
	{
		InitializeComponent();

        _userId = userId;
        _appointmentService = new PacienteServices(new HttpClient());
        PendingAppointments  = new ObservableCollection<AppointmentDto>();
        ApprovedAppointments = new ObservableCollection<AppointmentDto>();

        BindingContext = this;
        LoadInitialAppointments(_userId);
    }


    private async void Services(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new ClinicalUtp.views.serviceList());

    }
    public async Task LoadPendingAppointments(int userId)
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsWithUserIdAndStatus0(_userId);
            PendingAppointments.Clear();
            foreach (var appointment in appointments)
            {
                PendingAppointments.Add(appointment);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar citas pendientes: {ex.Message}");
        }
    }

    // Método para cargar citas aprobadas (estado 1)
    public async Task LoadApprovedAppointments(int userId)
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsWithUserIdAndStatus1(_userId);
            ApprovedAppointments.Clear();
            foreach (var appointment in appointments)
            {
                ApprovedAppointments.Add(appointment);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar citas aprobadas: {ex.Message}");
        }
    }

  

    // Carga inicial de las citas
    private async void LoadInitialAppointments(int userId)
    {
        await LoadPendingAppointments(_userId);  // Carga citas pendientes
        await LoadApprovedAppointments(_userId); // Carga citas aprobadas
    }
}
