using ClinicalUtp.viewModels;
using ClinicalUtp.Controllers;
using ClinicalUtp.views.components;
namespace ClinicalUtp.views;

public partial class HomePage : ContentPage
{

    private readonly MedicinesServices _medicinesService;

    private bool _isMenuVisible = false;
     private readonly AppointmentsServices _appointmentService;
    private int _userId;
    public HomePage(int userId)
	{
		InitializeComponent();
        _userId = userId;
        OnAppointmentPendiente();
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Suscribirse al mensaje enviado desde AppointmentView
        MessagingCenter.Subscribe<AppointmentViews>(this, "ActivateAppointmentButton", (sender) =>
        {
            AppointmentAprobada(null, EventArgs.Empty);
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Dejar de escuchar el mensaje cuando la p�gina ya no est� visible
        MessagingCenter.Unsubscribe<AppointmentViews>(this, "ActivateAppointmentButton");
    }

    public void RefreshAppointmentContent()
    {
        // Crear una nueva instancia del ContentView
        var appointmentsContentView = new AppointmentAprobada(_appointmentService, _userId);

        // Verificar si el contenedor din�mico existe en la p�gina
        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            // Asignar el nuevo ContentView al contenedor
            dynamicContentArea.Content = appointmentsContentView;
        }
    }

    private async void OnMenuItemClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage(_userId));
   
      //  FlyoutMenu.IsVisible = false;
    }


   
    private void   AppointmentRegister(object sender, EventArgs e)
    {
       // FlyoutMenu.IsVisible = false; 
        if (sender is Button clickedButton)
        {
            OnAppointmentView();
        }
    }

    private void OnAppointmentView()
    {
        var appointmentsContentView = new HomeContentDoctor();

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }

    private void OnMedicinesView()
    {
        var medicinesContentView = new MedicinasDisponibles(_medicinesService);

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content =  medicinesContentView;
        }
    }



    private void MedicinesDisponible(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            OnMedicinesView();
        }
    }


    private void AppointmentPendiente(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            OnAppointmentPendiente();
        }
    }

    private void OnAppointmentPendiente()
    {
     
        var appointmentsContentView = new AppointmentRecetadas(_appointmentService);

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }


    private void contactoV()
    {

        var appointmentsContentView = new ContactoView();

        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            dynamicContentArea.Content = appointmentsContentView;
        }
    }



    private void Contact(object sender, EventArgs e)
    {
        // Verificar si el sender es un ImageButton
        if (sender is ImageButton clickedImageButton)
        {
            contactoV();
        }
    }
    private void Profile(object sender, EventArgs e)
    {
        // Verificar si el sender es un ImageButton
        if (sender is ImageButton clickedImageButton)
        {
            ProfileViews();
        }
    }


    private void ProfileViews()
    {
        // Crear una instancia del ContentView que deseas mostrar
        var appointmentsContentView = new PerfilViews(_appointmentService, _userId);

        // Verificar si el contenedor din�mico existe en la p�gina
        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            // Asignar el nuevo ContentView al contenedor
            dynamicContentArea.Content = appointmentsContentView;
        }
    }

    private void AppointmentAprobada(object sender, EventArgs e)
    {
        // Verificar si el sender es un ImageButton
        if (sender is ImageButton clickedImageButton)
        {
            // Llamar a la funci�n que maneja el cambio de contenido
            OnAppointmentAprobada();
        }
    }


    private void OnAppointmentAprobada()
    {
        // Crear una instancia del ContentView que deseas mostrar
        var appointmentsContentView = new AppointmentAprobada(_appointmentService, _userId);

        // Verificar si el contenedor din�mico existe en la p�gina
        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            // Asignar el nuevo ContentView al contenedor
            dynamicContentArea.Content = appointmentsContentView;
        }
    }

    private void clickDonantes(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            DonantesViews();
        }
    }

    private void DonantesViews()
    {
        // Crear una instancia del ContentView que deseas mostrar
        var donantesContentView = new DonanteViews();

        // Verificar si el contenedor din�mico existe en la p�gina
        if (this.FindByName<ContentView>("DynamicContentArea") is ContentView dynamicContentArea)
        {
            // Asignar el nuevo ContentView al contenedor
            dynamicContentArea.Content = donantesContentView;
        }
    }


        private async void OnAddAppointmentClicked(object sender, EventArgs e)
    {
        // Navega a la p�gina para a�adir una cita
        await Navigation.PushAsync(new ProfilePage(_userId));
    }
    private async  void VerCitas(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ());

    }

    private  async void CitasAtentidas(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ());

    }

    private void LoadContentView(ContentView contentView)
    {
        DynamicContentArea.Content = contentView;
    }

    // M�todo para manejar el clic del bot�n
    private void OnShowMedicinesClicked(object sender, EventArgs e)
    {

        // Crear una instancia de MedicinasDisponibles
        var medicinasView = new MedicinasDisponibles(_medicinesService);

        // Establecer el ContentView como contenido din�mico
        DynamicContentArea.Content = medicinasView;
    }
    
}